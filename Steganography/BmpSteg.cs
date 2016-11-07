using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using System.Threading;

namespace Steganography
{
    class BmpSteg
    {
        public byte [] fileMarker = Encoding.ASCII.GetBytes("FE");
        public byte [] textMarker = Encoding.ASCII.GetBytes("TE");
        /// <summary>
        /// Put simply text into image
        /// </summary>
        /// <param name="image">Bitmap image</param>
        /// <param name="text">Text</param>
        public virtual void PutText(Bitmap image, string text)
        {
            byte[] source = Encoding.Unicode.GetBytes(text);
            if (!IsAvailableFreeSpace(image, source.Length))
                throw new ArgumentOutOfRangeException("source", source.Length, "Target data size more than container size");
            //Offset position
            int position = 0;
            //Insert marker
            this.Insert(image, this.textMarker, position);
            position += this.textMarker.Length;
            //Insert size
            this.Insert(image, BitConverter.GetBytes(source.Length), position);
            position += sizeof(int);
            //Insert source
            this.Insert(image, source, position);
        }

        /// <summary>
        /// Get Text data from image
        /// </summary>
        /// <param name="image">Bitmap image</param>
        /// <returns>Text data</returns>
        public virtual string GetText(Bitmap image)
        {
            //Offset position
            int position = 0;
            //Check marker
            if (!this.Extract(image, this.textMarker.Length, position).SequenceEqual(this.textMarker))
                throw new ArgumentException("Image doesn't have a mark", "image");
            position += this.textMarker.Length;
            //Get size
            int size = BitConverter.ToInt32(this.Extract(image, sizeof(int), position), 0);
            position += sizeof(int);
            //Get and return message
            return Encoding.Unicode.GetString(this.Extract(image, size, position));
        }

        public virtual void PutFile(Bitmap image, FileStream fs)
        {
            //Get extenstion
            byte [] extension = Encoding.Unicode.GetBytes(Path.GetExtension(fs.Name));
            
            //Get total data size
            long totalSize = this.fileMarker.Length + sizeof(int) + extension.Length + sizeof(int) + fs.Length;
            //Check
            if (!IsAvailableFreeSpace(image, totalSize))
                throw new ArgumentOutOfRangeException("fs", totalSize, "Target data size more than container size");

            byte[] source = new byte[fs.Length];
            fs.Read(source, 0, source.Length);

            int position = 0;
            //Put marker
            this.Insert(image, this.fileMarker, position);
            position += fileMarker.Length;

            //Put extension size
            this.Insert(image, BitConverter.GetBytes(extension.Length), position);
            position += sizeof(int);

            //Put extension
            this.Insert(image, extension, position);
            position += extension.Length;

            //Put data size
            this.Insert(image, BitConverter.GetBytes(source.Length), position);
            position += sizeof(int);

            //Put data
            this.Insert(image, source, position);
        }

        public virtual async Task PutFile(Bitmap image, FileStream fs, CancellationToken token)
        {
                //Get extenstion
                byte[] extension = Encoding.Unicode.GetBytes(Path.GetExtension(fs.Name));

                //Get total data size
                long totalSize = this.fileMarker.Length + sizeof(int) + extension.Length + sizeof(int) + fs.Length;
                //Check
                if (!IsAvailableFreeSpace(image, totalSize))
                    throw new ArgumentOutOfRangeException("fs", totalSize, "Target data size more than container size");

                byte[] source = new byte[fs.Length];
                await fs.ReadAsync(source, 0, source.Length, token);
               
                int position = 0;
                //Put marker
                this.InsertWithCancellation(image, this.fileMarker, position, token);
                    position += fileMarker.Length;

                //Put extension size
                this.InsertWithCancellation(image, BitConverter.GetBytes(extension.Length), position, token);
                    position += sizeof(int);

                //Put extension
                this.InsertWithCancellation(image, extension, position, token);
                    position += extension.Length;

                //Put data size
                this.InsertWithCancellation(image, BitConverter.GetBytes(source.Length), position, token);
                    position += sizeof(int);

                //Put data
                this.InsertWithCancellation(image, source, position, token);

                
        }

        public virtual FileStream GetFile(Bitmap image, string outputFileName)
        {
            int position = 0;
            if (!this.Extract(image, this.fileMarker.Length, position).SequenceEqual(this.fileMarker))
                throw new ArgumentException("Image doesn't have file marker", "image");
            position += this.fileMarker.Length;

            //Get file extension length
            int fileExtensionLength = BitConverter.ToInt32(this.Extract(image, sizeof(int), position), 0);
            position += sizeof(int);

            //Get fileName
            string fileExtension = Encoding.Unicode.GetString(this.Extract(image, fileExtensionLength, position));
            position += fileExtensionLength;

            //Get FileSize
            int fileSize = BitConverter.ToInt32(this.Extract(image, sizeof(int), position), 0);
            position += sizeof(int);

            //Create file
            FileStream fs = new FileStream(outputFileName + fileExtension, FileMode.CreateNew, FileAccess.ReadWrite);
            
            //Write file
            fs.Write(this.Extract(image, fileSize, position), 0, fileSize);
            return fs;
        }

        public virtual async Task<FileStream> GetFile(Bitmap image, string outputFileName, CancellationToken token)
        {
            int position = 0;
            if (!this.Extract(image, this.fileMarker.Length, position).SequenceEqual(this.fileMarker))
                throw new ArgumentException("Image doesn't have file marker", "image");
            position += this.fileMarker.Length;

            //Get file extension length
            int fileExtensionLength = BitConverter.ToInt32(this.ExtractWithCancellation(image, sizeof(int), position, token), 0);
            position += sizeof(int);

            //Get fileName
            string fileExtension = Encoding.Unicode.GetString(this.ExtractWithCancellation(image, fileExtensionLength, position, token));
            position += fileExtensionLength;

            //Get FileSize
            int fileSize = BitConverter.ToInt32(this.ExtractWithCancellation(image, sizeof(int), position, token), 0);
            position += sizeof(int);

            //Create file
            FileStream fs = new FileStream(outputFileName + fileExtension, FileMode.CreateNew, FileAccess.ReadWrite);

            //Write file
            await fs.WriteAsync(this.ExtractWithCancellation(image, fileSize, position, token), 0, fileSize);
            return fs;
        }

        /// <summary>
        /// Insert in bitmap image byte array
        /// </summary>
        /// <param name="image">Bitmap image</param>
        /// <param name="source">Byte array</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public virtual void InsertToImage(Bitmap image, byte[] source)
        {
            if (!IsAvailableFreeSpace(image, source.Length))
                throw new ArgumentOutOfRangeException("source", source.Length, "Target data size more than container size");
            if (Extract(image, fileMarker.Length, 0).SequenceEqual(fileMarker))
                throw new ArgumentException("Image is encoding now!", "image");
            //Insert makrer
            Insert(image, this.fileMarker, 0);
            //Insert length           
            Insert(image, BitConverter.GetBytes(source.Length), this.fileMarker.Length);
            //Insert data
            Insert(image, source, this.fileMarker.Length + sizeof(int));
        }

        /// <summary>
        /// Extract byte array from bitmap image
        /// </summary>
        /// <param name="image">Bitmap image</param>
        /// <returns>Extracted byte array</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public virtual byte [] OutFromImage(Bitmap image)
        {
            //Check marker
            if (!this.Extract(image, this.fileMarker.Length, 0).SequenceEqual(this.fileMarker))
                throw new ArgumentException("Image doesn't have a mark", "image");
            //Get size
            int size = BitConverter.ToInt32(Extract(image, sizeof(int), this.fileMarker.Length), 0);
            //Extract data
            return this.Extract(image, size, sizeof(int) + this.fileMarker.Length);          
        }



        /*******************Helper Methods*******************/

        /// <summary>
        /// Get pixel coordinates from offset
        /// </summary>
        /// <param name="image">Bitmap image</param>
        /// <param name="offset">Offset position</param>
        /// <returns>Point with coordinates</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        protected Point GetPoint(Bitmap image, int offset)
        {
            if (offset > image.Width * image.Height)
                throw new ArgumentOutOfRangeException("offset", offset, "Offset higher than pixel count");
            if (image == null)
                throw new ArgumentNullException("image","Image null argument");
            Point position = new Point();
            if (offset < image.Width)
                position.X = offset;
            else
            {
                int y = offset;
                while (y >= image.Width)
                {
                    position.Y++;
                    y -= image.Width;
                }
                position.X = y;
            }
            return position;
        }

        /// <summary>
        /// Check - is available free space in bitmap image for our data
        /// </summary>
        /// <param name="image">Bitmap image</param>
        /// <param name="length">Data byte array length</param>
        /// <returns>True- available, False - not available</returns>
        /// <exception cref="ArgumentNullException"></exception>
        protected bool IsAvailableFreeSpace(Bitmap image, long length)
        {
            if (image == null)
                throw new ArgumentNullException("image", "Image null argument");
            return image.Height * image.Width - this.fileMarker.Length - sizeof(int) >= length;
        }

        /// <summary>
        /// Method insert into image data from source, start from offset value
        /// </summary>
        /// <param name="image">Bitmap image</param>
        /// <param name="source">Source byte array</param>
        /// <param name="offset">Offset</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentException"></exception>
        protected void Insert(Bitmap image, byte[] source, int offset)
        {
            if (source == null)
                throw new ArgumentNullException("source", "Source null argument");
            //Get pixel position from offset
            Point position = GetPoint(image, offset);
            
            //Set Counts for bits and index for bytes
            int bitCount = 0;
            int byteIndex = 0;

            for (; position.Y < image.Height; position.Y++)
            {
                for (; position.X < image.Width; position.X++)
                {
                    //Insert byte array completed                 
                    if (byteIndex == source.Length)
                        return;
                    //Get color from pixel
                    Color color = image.GetPixel(position.X, position.Y);

                    //Get array 4 bytes of: Alpha, Red, Green and Blue
                    byte[] argb = BitConverter.GetBytes(color.ToArgb());

                    //Change 2 least significant bit for each color and alpha to our source byte
                    for (int i = 0; i < argb.Length; i++)
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            //Test a bit (if bit is set)
                            if ((source[byteIndex] & 1 << bitCount) != 0)
                                //Set bit in argb
                                argb[i] = (byte)(argb[i] | 1 << j);
                            //Else clear bit in argb
                            else
                                argb[i] = (byte)(argb[i] & ~(1 << j));
                            bitCount++;
                        }
                    }
                    //Byte completed
                    bitCount = 0;
                    byteIndex++;
                    //Get new color from argb and set into image                    
                    color = Color.FromArgb(BitConverter.ToInt32(argb, 0));
                    image.SetPixel(position.X, position.Y, color);
                }
                //New pixel row
                position.X = 0;
            }
        }

        /// <summary>
        /// Extract bytes from image with specific length from offset position
        /// </summary>
        /// <param name="image">Bitmap image</param>
        /// <param name="length">Byte count</param>
        /// <param name="offset">Offset</param>
        /// <returns>Return extracted byte array</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="Exception"></exception>
        protected byte[] Extract(Bitmap image, int length, int offset)
        {
            if (length > image.Width * image.Height - offset)
                throw new ArgumentOutOfRangeException("length", length, "Length higher than pixel count");
            //Get current pixel from offset
            Point position = GetPoint(image, offset);

            //Allocate memory(all bits clear)
            byte[] stream = new byte[length];

            int bitCount = 0;
            int byteIndex = 0;

            for (; position.Y < image.Height; position.Y++)
            {
                for (; position.X < image.Width; position.X++)
                {
                    if (byteIndex == length)
                        return stream;
                    Color color = image.GetPixel(position.X, position.Y);
                    byte[] b = BitConverter.GetBytes(color.ToArgb());
                    for (int i = 0; i < b.Length; i++)
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            //Test a bit (if bit is set - set bit in stream)
                            if ((b[i] & 1 << j) != 0)
                                stream[byteIndex] = (byte)(stream[byteIndex] | 1 << bitCount);
                            bitCount++;
                        }
                    }
                    bitCount = 0;
                    byteIndex++;
                }
                position.X = 0;
            }
            return stream;
        }

        protected void InsertWithCancellation(Bitmap image, byte[] source, int offset, CancellationToken token)
        {
            if (source == null)
                throw new ArgumentNullException("source", "Source null argument");

            Point position = GetPoint(image, offset);

            int bitCount = 0;
            int byteIndex = 0;

            for (; position.Y < image.Height; position.Y++)
            {
                for (; position.X < image.Width; position.X++)
                {              
                    if (byteIndex == source.Length)
                        return;

                    Color color = image.GetPixel(position.X, position.Y);

                    byte[] argb = BitConverter.GetBytes(color.ToArgb());

                    for (int i = 0; i < argb.Length; i++)
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            //!!!!!!!!!!!!!!!!!!!!!!!!!!
                            if (token.IsCancellationRequested)
                                token.ThrowIfCancellationRequested();
                            if ((source[byteIndex] & 1 << bitCount) != 0)
                                argb[i] = (byte)(argb[i] | 1 << j);
                            else
                                argb[i] = (byte)(argb[i] & ~(1 << j));
                            bitCount++;
                        }
                    }
                    bitCount = 0;
                    byteIndex++;                  
                    color = Color.FromArgb(BitConverter.ToInt32(argb, 0));
                    image.SetPixel(position.X, position.Y, color);
                }
                position.X = 0;
            }
        }

        protected byte[] ExtractWithCancellation(Bitmap image, int length, int offset, CancellationToken token)
        {
            if (length > image.Width * image.Height - offset)
                throw new ArgumentOutOfRangeException("length", length, "Length higher than pixel count");
            Point position = GetPoint(image, offset);

            byte[] stream = new byte[length];

            int bitCount = 0;
            int byteIndex = 0;

            for (; position.Y < image.Height; position.Y++)
            {
                for (; position.X < image.Width; position.X++)
                {
                    if (byteIndex == length)
                        return stream;
                    Color color = image.GetPixel(position.X, position.Y);
                    byte[] b = BitConverter.GetBytes(color.ToArgb());
                    for (int i = 0; i < b.Length; i++)
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            if (token.IsCancellationRequested)
                                token.ThrowIfCancellationRequested();
                            if ((b[i] & 1 << j) != 0)
                                stream[byteIndex] = (byte)(stream[byteIndex] | 1 << bitCount);
                            bitCount++;
                        }
                    }
                    bitCount = 0;
                    byteIndex++;
                }
                position.X = 0;
            }
            return stream;
        }

    }
}
