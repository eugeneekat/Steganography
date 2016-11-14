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
        /*-------------------------Markers-------------------------*/
        public byte [] fileMarker = Encoding.ASCII.GetBytes("FE");
        public byte [] textMarker = Encoding.ASCII.GetBytes("TE");

        /*-------------------Put and get methods-------------------*/
       
        /// <summary>
        /// Put text into image
        /// </summary>
        /// <param name="image">Bitmap image</param>
        /// <param name="text">Text</param>
        public virtual void PutText(Bitmap image, string text)
        {
            byte[] source = Encoding.Unicode.GetBytes(text);
            if (!IsAvailableFreeSpace(image, source.Length))
                throw new ArgumentOutOfRangeException("Target data size more than container size");
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
        /// Get Text from image
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

        /// <summary>
        /// Put FileStream into image
        /// </summary>
        /// <param name="image">Bitmap Image</param>
        /// <param name="file">FileStream</param>
        public virtual void PutFile(Bitmap image, FileStream file)
        {
            //Get extenstion
            byte [] extension = Encoding.Unicode.GetBytes(Path.GetExtension(file.Name));
            
            //Get total data size
            long totalSize = this.fileMarker.Length + sizeof(int) + extension.Length + sizeof(int) + file.Length;
            //Check
            if (!IsAvailableFreeSpace(image, totalSize))
                throw new ArgumentOutOfRangeException("file", totalSize, "Target data size more than container size");

            byte[] source = new byte[file.Length];
            file.Read(source, 0, source.Length);

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

        /// <summary>
        /// Get FileStream from image
        /// </summary>
        /// <param name="image">Bitmap image</param>
        /// <param name="outputPath">Output path</param>
        /// <returns>FileStream with result file</returns>
        public virtual FileStream GetFile(Bitmap image, string outputPath)
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
            FileStream fs = new FileStream(outputPath + fileExtension, FileMode.CreateNew, FileAccess.ReadWrite);

            //Write file
            fs.Write(this.Extract(image, fileSize, position), 0, fileSize);
            return fs;
        }

        
        
        
        /*----------Put and get async methods----------*/

        public virtual async Task PutFileAsync(Bitmap image, FileStream file)
        {
            await Task.Run(async() =>
            {
                //Get extenstion
                byte[] extension = Encoding.Unicode.GetBytes(Path.GetExtension(file.Name));

                //Get total data size
                long totalSize = this.fileMarker.Length + sizeof(int) + extension.Length + sizeof(int) + file.Length;
                //Check
                if (!IsAvailableFreeSpace(image, totalSize))
                    throw new ArgumentOutOfRangeException("file", totalSize, "Target data size more than container size");

                byte[] source = new byte[file.Length];
                await file.ReadAsync(source, 0, source.Length);
                int position = 0;
                //Put marker
                await Task.FromResult(this.Insert(image, this.fileMarker, position));
                position += fileMarker.Length;
                //Put extension size
                await Task.FromResult(this.Insert(image, BitConverter.GetBytes(extension.Length), position));
                position += sizeof(int);
                //Put extension
                await Task.FromResult(this.Insert(image, extension, position));
                position += extension.Length;
                //Put data size
                await Task.FromResult(this.Insert(image, BitConverter.GetBytes(source.Length), position));
                position += sizeof(int);
                //Put data
                await Task.FromResult(this.Insert(image, source, position));
            });
        }

        public virtual async Task PutFileAsync(Bitmap image, FileStream file, CancellationToken token)
        {
            await Task.Run(async () =>
            {
                //Get extenstion
                byte[] extension = Encoding.Unicode.GetBytes(Path.GetExtension(file.Name));

                //Get total data size
                long totalSize = this.fileMarker.Length + sizeof(int) + extension.Length + sizeof(int) + file.Length;
                //Check
                if (!IsAvailableFreeSpace(image, totalSize))
                    throw new ArgumentOutOfRangeException("file", totalSize, "Target data size more than container size");

                byte[] source = new byte[file.Length];
                await file.ReadAsync(source, 0, source.Length, token);
                int position = 0;
                //Put marker
                await Task.FromResult(this.InsertWithCancellation(image, this.fileMarker, position, token));
                position += fileMarker.Length;
                //Put extension size
                await Task.FromResult(this.InsertWithCancellation(image, BitConverter.GetBytes(extension.Length), position, token));
                position += sizeof(int);
                //Put extension
                await Task.FromResult(this.InsertWithCancellation(image, extension, position, token));
                position += extension.Length;
                //Put data size
                await Task.FromResult(this.InsertWithCancellation(image, BitConverter.GetBytes(source.Length), position, token));
                position += sizeof(int);
                //Put data
                await Task.FromResult(this.InsertWithCancellation(image, source, position, token));
            });
        }

        public virtual async Task PutFileAsync(Bitmap image, FileStream file, CancellationToken token, Action<int>progress)
        {
            progress(0);
            await Task.Run(async() =>
            {
                //Get extenstion
                byte[] extension = Encoding.Unicode.GetBytes(Path.GetExtension(file.Name));

                //Get total data size
                long totalSize = this.fileMarker.Length + sizeof(int) + extension.Length + sizeof(int) + file.Length;
                //Check
                if (!IsAvailableFreeSpace(image, totalSize))
                    throw new ArgumentOutOfRangeException("file", totalSize, "Target data size more than container size");

                byte[] source = new byte[file.Length];
                await file.ReadAsync(source, 0, source.Length, token);
                int position = 0;
                //Put marker
                await Task.FromResult(this.InsertWithCancellation(image, this.fileMarker, position, token));
                position += fileMarker.Length;
                //Put extension size
                await Task.FromResult(this.InsertWithCancellation(image, BitConverter.GetBytes(extension.Length), position, token));
                position += sizeof(int);
                //Put extension
                await Task.FromResult(this.InsertWithCancellation(image, extension, position, token));
                position += extension.Length;
                //Put data size
                await Task.FromResult(this.InsertWithCancellation(image, BitConverter.GetBytes(source.Length), position, token));
                position += sizeof(int);
                //Put data
                await Task.FromResult(this.InsertWithCancellation(image, source, position, token, progress));
            });
        }

        public virtual async Task<FileStream> GetFileAsync(Bitmap image, string outputFileName)
        {
            return await Task.Run(async () =>
            {
                int position = 0;
                if (!this.Extract(image, this.fileMarker.Length, position).SequenceEqual(this.fileMarker))
                    throw new ArgumentException("Image doesn't have file marker", "image");
                position += this.fileMarker.Length;

                //Get file extension length
                int fileExtensionLength = BitConverter.ToInt32(await Task.FromResult(this.Extract(image, sizeof(int), position)), 0);
                position += sizeof(int);

                //Get fileName
                string fileExtensionName = Encoding.Unicode.GetString(await Task.FromResult(this.Extract(image, fileExtensionLength, position)));
                position += fileExtensionLength;

                //Get FileSize
                int fileSize = BitConverter.ToInt32(await Task.FromResult(this.Extract(image, sizeof(int), position)), 0);
                position += sizeof(int);
                //Create file
                FileStream fs = null;
                try
                {
                    fs = new FileStream(outputFileName + fileExtensionName, FileMode.CreateNew, FileAccess.ReadWrite);
                    await fs.WriteAsync(await Task.FromResult(this.Extract(image, fileSize, position)), 0, fileSize);
                }
                catch (Exception)
                {
                    fs.Close();
                    File.Delete(fs.Name);
                    throw;
                }
                return fs;
            });
        }

        public virtual async Task<FileStream> GetFileAsync(Bitmap image, string outputFileName, CancellationToken token)
        {
            return await Task.Run(async () =>
            {

                int position = 0;
                if (!this.Extract(image, this.fileMarker.Length, position).SequenceEqual(this.fileMarker))
                    throw new ArgumentException("Image doesn't have file marker", "image");
                position += this.fileMarker.Length;

                //Get file extension length
                int fileExtensionLength = BitConverter.ToInt32(await Task.FromResult(this.ExtractWithCancellation(image, sizeof(int), position, token)), 0);
                position += sizeof(int);

                //Get fileName
                string fileExtensionName = Encoding.Unicode.GetString(await Task.FromResult(this.ExtractWithCancellation(image, fileExtensionLength, position, token)));
                position += fileExtensionLength;

                //Get FileSize
                int fileSize = BitConverter.ToInt32(await Task.FromResult(this.ExtractWithCancellation(image, sizeof(int), position, token)), 0);
                position += sizeof(int);

                //Create file
                FileStream fs = null;
                try
                {
                    fs = new FileStream(outputFileName + fileExtensionName, FileMode.CreateNew, FileAccess.ReadWrite);
                    await fs.WriteAsync(await Task.FromResult(this.ExtractWithCancellation(image, fileSize, position, token)), 0, fileSize, token);
                }
                catch (Exception)
                {
                    fs.Close();
                    File.Delete(fs.Name);
                    throw;
                }
                return fs;
            });
        }

        public virtual async Task<FileStream> GetFileAsync(Bitmap image, string outputFileName, CancellationToken token, Action<int> progress)
        {
            return await Task.Run(async () =>
            {
                int position = 0;
                if (!this.Extract(image, this.fileMarker.Length, position).SequenceEqual(this.fileMarker))
                    throw new ArgumentException("Image doesn't have file marker", "image");
                position += this.fileMarker.Length;

                //Get file extension length
                int fileExtensionLength = BitConverter.ToInt32(await Task.FromResult(this.ExtractWithCancellation(image, sizeof(int), position, token)), 0);
                position += sizeof(int);

                //Get fileName
                string fileExtensionName = Encoding.Unicode.GetString(await Task.FromResult(this.ExtractWithCancellation(image, fileExtensionLength, position, token)));
                position += fileExtensionLength;

                //Get FileSize
                int fileSize = BitConverter.ToInt32(await Task.FromResult(this.ExtractWithCancellation(image, sizeof(int), position, token)), 0);
                position += sizeof(int);

                //Create file
                FileStream fs = null;
                try
                {
                    fs = new FileStream(outputFileName + fileExtensionName, FileMode.CreateNew, FileAccess.ReadWrite);
                    await fs.WriteAsync(await Task.FromResult(this.ExtractWithCancellation(image, fileSize, position, token, progress)), 0, fileSize, token);
                }
                catch (Exception)
                {
                    fs.Close();
                    File.Delete(fs.Name);
                    throw;
                }
                return fs;
            });
        }








        /*---------------------Helper Methods---------------------*/

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
        protected long Insert(Bitmap image, byte[] source, int offset)
        {
            if (source == null)
                throw new ArgumentNullException("source", "Source null argument");
            //Get pixel position from offset
            Point position = GetPoint(image, offset);
            
            //Set Counts for bits and index for bytes
            int bitCount = 0;
            long byteIndex = 0;

            for (; position.Y < image.Height; position.Y++)
            {
                for (; position.X < image.Width; position.X++)
                {
                    //Insert byte array completed                 
                    if (byteIndex == source.Length)
                        return byteIndex;
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
            return byteIndex;
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
        protected byte[] Extract(Bitmap image, long length, int offset)
        {
            if (length > image.Width * image.Height - offset)
                throw new ArgumentOutOfRangeException("length", length, "Length higher than pixel count");
            //Get current pixel from offset
            Point position = GetPoint(image, offset);

            //Allocate memory(all bits clear)
            byte[] stream = new byte[length];

            int bitCount = 0;
            long byteIndex = 0;

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



        /*------------Helper Methods with cancellation------------*/

        protected long InsertWithCancellation(Bitmap image, byte[] source, int offset, CancellationToken token)
        {
            if (source == null)
                throw new ArgumentNullException("source", "Source null argument");

            Point position = GetPoint(image, offset);

            int bitCount = 0;
            long byteIndex = 0;

            for (; position.Y < image.Height; position.Y++)
            {
                for (; position.X < image.Width; position.X++)
                {
                    if (byteIndex == source.LongLength)
                        return byteIndex;

                    Color color = image.GetPixel(position.X, position.Y);

                    byte[] argb = BitConverter.GetBytes(color.ToArgb());

                    for (int i = 0; i < argb.Length; i++)
                    {
                        for (int j = 0; j < 2; j++)
                        {
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
            return byteIndex;
        }

        protected long InsertWithCancellation(Bitmap image, byte[] source, int offset, CancellationToken token, Action<int> progress)
        {
            if (source == null)
                throw new ArgumentNullException("source", "Source null argument");

            Point position = GetPoint(image, offset);

            int bitCount = 0;
            long byteIndex = 0;
            progress(0);

            for (; position.Y < image.Height; position.Y++)
            {
                for (; position.X < image.Width; position.X++)
                {
                    if (byteIndex == source.LongLength)
                        return byteIndex;

                    Color color = image.GetPixel(position.X, position.Y);

                    byte[] argb = BitConverter.GetBytes(color.ToArgb());

                    for (int i = 0; i < argb.Length; i++)
                    {
                        for (int j = 0; j < 2; j++)
                        {
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
                    if (byteIndex % 4096 == 0)
                        progress((int)(byteIndex * 100 / source.LongLength));
                }
                position.X = 0;
            }
            return byteIndex;
        }

        protected byte[] ExtractWithCancellation(Bitmap image, long length, int offset, CancellationToken token)
        {
            if (length > image.Width * image.Height - offset)
                throw new ArgumentOutOfRangeException("length", length, "Length higher than pixel count");
            Point position = GetPoint(image, offset);

            byte[] stream = new byte[length];

            int bitCount = 0;
            long byteIndex = 0;

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

        protected byte[] ExtractWithCancellation(Bitmap image, long length, int offset, CancellationToken token, Action<int> progress)
        {
            if (length > image.Width * image.Height - offset)
                throw new ArgumentOutOfRangeException("Length higher than pixel count");

            Point position = GetPoint(image, offset);
            byte[] stream = new byte[length];
            int bitCount = 0;
            long byteIndex = 0;
            progress(0);

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
                    if (byteIndex % 4096 == 0)
                        progress((int)(byteIndex * 100 / stream.LongLength));
                }
                position.X = 0;
            }
            return stream;
        }
    }
}
