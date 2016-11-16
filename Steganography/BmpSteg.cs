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

        public byte[] fileMarker = Encoding.ASCII.GetBytes("FE");
        public byte[] textMarker = Encoding.ASCII.GetBytes("TE");

        /*----------------Encode and decode methods----------------*/

        public virtual void EncodeText(Bitmap image, string text)
        {
            byte[] source = Encoding.Unicode.GetBytes(text);
            if (!IsAvailableFreeSpace(image, source.Length))
                throw new ArgumentOutOfRangeException("Target data size more than container size");
            //Offset position
            int position = 0;
            //Insert marker
            this.Encode(image, this.textMarker, position);
            position += this.textMarker.Length;
            //Insert size
            this.Encode(image, BitConverter.GetBytes(source.Length), position);
            position += sizeof(int);
            //Insert source
            this.Encode(image, source, position);
        }

        public virtual string DecodeText(Bitmap image)
        {
            //Offset position
            int position = 0;
            //Check marker
            if (!this.Decode(image, this.textMarker.Length, position).SequenceEqual(this.textMarker))
                throw new ArgumentException("Image doesn't have a mark");
            position += this.textMarker.Length;
            //Get size
            int size = BitConverter.ToInt32(this.Decode(image, sizeof(int), position), 0);
            position += sizeof(int);
            //Get and return message
            return Encoding.Unicode.GetString(this.Decode(image, size, position));
        }

        public virtual void EncodeFile(Bitmap image, FileStream file)
        {
            //Get extenstion
            byte[] extension = Encoding.Unicode.GetBytes(Path.GetExtension(file.Name));
            //Get total data size
            long totalSize = this.fileMarker.Length + sizeof(int) + extension.Length + sizeof(int) + file.Length;
            //Check
            if (!IsAvailableFreeSpace(image, totalSize))
                throw new ArgumentOutOfRangeException("Target data size more than container size");
            byte[] source = new byte[file.Length];
            file.Read(source, 0, source.Length);
            int position = 0;
            //Put marker
            this.Encode(image, this.fileMarker, position);
            position += fileMarker.Length;
            //Put extension size
            this.Encode(image, BitConverter.GetBytes(extension.Length), position);
            position += sizeof(int);
            //Put extension
            this.Encode(image, extension, position);
            position += extension.Length;
            //Put data size
            this.Encode(image, BitConverter.GetBytes(source.Length), position);
            position += sizeof(int);
            //Put data
            this.Encode(image, source, position);
        }

        public virtual FileStream DecodeFile(Bitmap image, string outputPath)
        {
            int position = 0;
            if (!this.Decode(image, this.fileMarker.Length, position).SequenceEqual(this.fileMarker))
                throw new ArgumentException("Image doesn't have file marker");
            position += this.fileMarker.Length;
            //Get file extension length
            int fileExtensionLength = BitConverter.ToInt32(this.Decode(image, sizeof(int), position), 0);
            position += sizeof(int);
            //Get fileName
            string fileExtension = Encoding.Unicode.GetString(this.Decode(image, fileExtensionLength, position));
            position += fileExtensionLength;
            //Get FileSize
            int fileSize = BitConverter.ToInt32(this.Decode(image, sizeof(int), position), 0);
            position += sizeof(int);
            //Create file
            FileStream fs = new FileStream(outputPath + fileExtension, FileMode.CreateNew, FileAccess.ReadWrite);
            //Write file
            fs.Write(this.Decode(image, fileSize, position), 0, fileSize);
            return fs;
        }

        /*-------------Encode and decode async methods-------------*/

        public virtual async Task EncodeFileAsync(Bitmap image, FileStream file)
        {
            await Task.Run(async () =>
            {
                byte[] extension = Encoding.Unicode.GetBytes(Path.GetExtension(file.Name));
                long totalSize = this.fileMarker.Length + sizeof(int) + extension.Length + sizeof(int) + file.Length;
                if (!IsAvailableFreeSpace(image, totalSize))
                    throw new ArgumentOutOfRangeException("Target data size more than container size");
                byte[] source = new byte[file.Length];
                await file.ReadAsync(source, 0, source.Length);
                int position = 0;
                await Task.FromResult(this.Encode(image, this.fileMarker, position));
                position += fileMarker.Length;
                await Task.FromResult(this.Encode(image, BitConverter.GetBytes(extension.Length), position));
                position += sizeof(int);
                await Task.FromResult(this.Encode(image, extension, position));
                position += extension.Length;
                await Task.FromResult(this.Encode(image, BitConverter.GetBytes(source.Length), position));
                position += sizeof(int);
                await Task.FromResult(this.Encode(image, source, position));
            });
        }

        public virtual async Task EncodeFileAsync(Bitmap image, FileStream file, CancellationToken token)
        {
            await Task.Run(async () =>
            {
                byte[] extension = Encoding.Unicode.GetBytes(Path.GetExtension(file.Name));
                long totalSize = this.fileMarker.Length + sizeof(int) + extension.Length + sizeof(int) + file.Length;
                if (!IsAvailableFreeSpace(image, totalSize))
                    throw new ArgumentOutOfRangeException("Target data size more than container size");
                byte[] source = new byte[file.Length];
                await file.ReadAsync(source, 0, source.Length, token);
                int position = 0;
                await Task.FromResult(this.Encode(image, this.fileMarker, position, token));
                position += fileMarker.Length;
                await Task.FromResult(this.Encode(image, BitConverter.GetBytes(extension.Length), position, token));
                position += sizeof(int);
                await Task.FromResult(this.Encode(image, extension, position, token));
                position += extension.Length;
                await Task.FromResult(this.Encode(image, BitConverter.GetBytes(source.Length), position, token));
                position += sizeof(int);
                await Task.FromResult(this.Encode(image, source, position, token));
            });
        }

        public virtual async Task EncodeFileAsync(Bitmap image, FileStream file, CancellationToken token, Action<int> progress)
        {
            //Start Task for encoding
            await Task.Run(async () =>
            {
                //Get extenstion
                byte[] extension = Encoding.Unicode.GetBytes(Path.GetExtension(file.Name));
                //Get total data size
                long totalSize = this.fileMarker.Length + sizeof(int) + extension.Length + sizeof(int) + file.Length;
                //Check
                if (!IsAvailableFreeSpace(image, totalSize))
                    throw new ArgumentOutOfRangeException("Target data size more than container size");
                byte[] source = new byte[file.Length];
                //Read file
                await file.ReadAsync(source, 0, source.Length, token);
                int position = 0;
                //Encode marker
                await Task.FromResult(this.Encode(image, this.fileMarker, position, token));
                position += fileMarker.Length;
                //Encode extension size
                await Task.FromResult(this.Encode(image, BitConverter.GetBytes(extension.Length), position, token));
                position += sizeof(int);
                //Encode extension
                await Task.FromResult(this.Encode(image, extension, position, token));
                position += extension.Length;
                //Encode data size
                await Task.FromResult(this.Encode(image, BitConverter.GetBytes(source.Length), position, token));
                position += sizeof(int);
                //Encode data with progress
                await Task.FromResult(this.Encode(image, source, position, token, progress));
            });
        }

        public virtual async Task<FileStream> DecodeFileAsync(Bitmap image, string outputFileName)
        {
            return await Task.Run(async () =>
            {
                int position = 0;
                if (!this.Decode(image, this.fileMarker.Length, position).SequenceEqual(this.fileMarker))
                    throw new ArgumentException("Image doesn't have file marker");
                position += this.fileMarker.Length;
                int fileExtensionLength = BitConverter.ToInt32(await Task.FromResult(this.Decode(image, sizeof(int), position)), 0);
                position += sizeof(int);
                string fileExtensionName = Encoding.Unicode.GetString(await Task.FromResult(this.Decode(image, fileExtensionLength, position)));
                position += fileExtensionLength;
                int fileSize = BitConverter.ToInt32(await Task.FromResult(this.Decode(image, sizeof(int), position)), 0);
                position += sizeof(int);
                FileStream fs = null;
                try
                {
                    fs = new FileStream(outputFileName + fileExtensionName, FileMode.CreateNew, FileAccess.ReadWrite);
                    await fs.WriteAsync(await Task.FromResult(this.Decode(image, fileSize, position)), 0, fileSize);
                }
                catch (Exception)
                {
                    if (fs != null)
                    {
                        fs.Close();
                        File.Delete(fs.Name);
                    }
                    throw;
                }
                return fs;
            });
        }

        public virtual async Task<FileStream> DecodeFileAsync(Bitmap image, string outputFileName, CancellationToken token)
        {
            return await Task.Run(async () =>
            {
                int position = 0;
                if (!this.Decode(image, this.fileMarker.Length, position).SequenceEqual(this.fileMarker))
                    throw new ArgumentException("Image doesn't have file marker");
                position += this.fileMarker.Length;
                int fileExtensionLength = BitConverter.ToInt32(await Task.FromResult(this.Decode(image, sizeof(int), position, token)), 0);
                position += sizeof(int);
                string fileExtensionName = Encoding.Unicode.GetString(await Task.FromResult(this.Decode(image, fileExtensionLength, position, token)));
                position += fileExtensionLength;
                int fileSize = BitConverter.ToInt32(await Task.FromResult(this.Decode(image, sizeof(int), position, token)), 0);
                position += sizeof(int);
                FileStream fs = null;
                try
                {
                    fs = new FileStream(outputFileName + fileExtensionName, FileMode.CreateNew, FileAccess.ReadWrite);
                    await fs.WriteAsync(await Task.FromResult(this.Decode(image, fileSize, position, token)), 0, fileSize, token);
                }
                catch (Exception)
                {
                    if (fs != null)
                    {
                        fs.Close();
                        File.Delete(fs.Name);
                    }
                    throw;
                }
                return fs;
            });
        }

        public virtual async Task<FileStream> DecodeFileAsync(Bitmap image, string outputFileName, CancellationToken token, Action<int> progress)
        {
            //Start Task for encoding
            return await Task.Run(async () =>
            {
                int position = 0;
                //If image don't have marker throw exception
                if (!this.Decode(image, this.fileMarker.Length, position).SequenceEqual(this.fileMarker))
                    throw new ArgumentException("Image doesn't have file marker", "image");
                position += this.fileMarker.Length;

                //Decode file extension length
                int fileExtensionLength = BitConverter.ToInt32(await Task.FromResult(this.Decode(image, sizeof(int), position, token)), 0);
                position += sizeof(int);

                //Decode file name
                string fileExtensionName = Encoding.Unicode.GetString(await Task.FromResult(this.Decode(image, fileExtensionLength, position, token)));
                position += fileExtensionLength;

                //Decode FileSize
                int fileSize = BitConverter.ToInt32(await Task.FromResult(this.Decode(image, sizeof(int), position, token)), 0);
                position += sizeof(int);

                FileStream fs = null;
                try
                {
                    //Create file for save
                    fs = new FileStream(outputFileName + fileExtensionName, FileMode.CreateNew, FileAccess.ReadWrite);
                    //Decode data in file
                    await fs.WriteAsync(await Task.FromResult(this.Decode(image, fileSize, position, token, progress)), 0, fileSize, token);
                }
                catch (Exception)
                {
                    if (fs != null)
                    {
                        fs.Close();
                        File.Delete(fs.Name);
                    }
                    throw;
                }
                //Return file
                return fs;
            });
        }








        /*---------------------Helper Methods---------------------*/

        /// <summary>
        /// Get pixtel position from offset
        /// </summary>
        /// <param name="image">Bitmap image</param>
        /// <param name="offset">offset</param>
        /// <returns>Point of pixel position</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        protected Point GetPoint(Bitmap image, int offset)
        {
            if (image == null)
                throw new ArgumentNullException("image", "Image null argument");
            if (offset > image.Width * image.Height)
                throw new ArgumentOutOfRangeException("offset", offset, "Offset higher than pixel count");
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
        /// The method returns the availability of free space in bitmap image
        /// </summary>
        /// <param name="image">Bitmap image</param>
        /// <param name="bitCount">Count of bits</param>
        /// <returns>True - freespace available, false - not available</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        protected bool IsAvailableFreeSpace(Bitmap image, long bitCount)
        {
            if (image == null)
                throw new ArgumentNullException("image", "Image null argument");
            if (bitCount <= 0)
                throw new ArgumentException("Incorrect bitCount value", "bitCount");
            return image.Height * image.Width - this.fileMarker.Length - sizeof(int) >= bitCount;
        }

        /// <summary>
        /// Encode byte array into bitmap image from offset position
        /// </summary>
        /// <param name="image">Bitmap image</param>
        /// <param name="source">Byte array</param>
        /// <param name="offset">offset</param>
        /// <returns>Count of encoded bytes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        protected long Encode(Bitmap image, byte[] source, int offset)
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
                    if (byteIndex == source.Length)
                        return byteIndex;
                    Color color = image.GetPixel(position.X, position.Y);
                    byte[] argb = BitConverter.GetBytes(color.ToArgb());
                    for (int i = 0; i < argb.Length; i++)
                    {
                        for (int j = 0; j < 2; j++)
                        {
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

        /// <summary>
        /// Decode byte array from bitmap image from offset position
        /// </summary>
        /// <param name="image">Bitmap image</param>
        /// <param name="decodeByteCount">Count of bytes for decoding</param>
        /// <param name="offset">offset</param>
        /// <returns>Decoded byte array</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        protected byte[] Decode(Bitmap image, long decodeByteCount, int offset)
        {
            if (decodeByteCount > image.Width * image.Height - offset || decodeByteCount <= 0)
                throw new ArgumentOutOfRangeException("length", decodeByteCount, "Incorrect length value");
            Point position = GetPoint(image, offset);
            byte[] stream = new byte[decodeByteCount];
            int bitCount = 0;
            long byteIndex = 0;
            for (; position.Y < image.Height; position.Y++)
            {
                for (; position.X < image.Width; position.X++)
                {
                    if (byteIndex == decodeByteCount)
                        return stream;
                    Color color = image.GetPixel(position.X, position.Y);
                    byte[] b = BitConverter.GetBytes(color.ToArgb());
                    for (int i = 0; i < b.Length; i++)
                    {
                        for (int j = 0; j < 2; j++)
                        {
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

        /// <summary>
        /// Encode byte array into bitmap image from offset position with cancellation
        /// </summary>
        /// <param name="image">Bitmap image</param>
        /// <param name="source">Byte array</param>
        /// <param name="offset">offset</param>
        /// <param name="token">CancellationToken</param>
        /// <returns>Count of encoded bytes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="OperationCanceledException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        protected long Encode(Bitmap image, byte[] source, int offset, CancellationToken token)
        {
            if (source == null)
                throw new ArgumentNullException("source", "Source null argument");
            if (token.IsCancellationRequested)
                token.ThrowIfCancellationRequested();
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

        /// <summary>
        /// Encode byte array into bitmap image from offset position with cancellation and progress notifier
        /// </summary>
        /// <param name="image">Bitmap image</param>
        /// <param name="source">Byte array</param>
        /// <param name="offset">offset</param>
        /// <param name="token">CancellationToken</param>
        /// <param name="progress">Progress notifier method</param>
        /// <returns>Count of encoded bytes</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="OperationCanceledException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        protected long Encode(Bitmap image, byte[] source, int offset, CancellationToken token, Action<int> progress)
        {
            //Check input params
            if (source == null)
                throw new ArgumentNullException("source", "Source null argument");
            if (progress == null)
                throw new ArgumentNullException("progress", "Progress null argument");
            //Check CancellationToken
            if (token.IsCancellationRequested)
                token.ThrowIfCancellationRequested();
            //Get pixel position from offset
            Point position = GetPoint(image, offset);
            //Set index for bits and index for bytes
            int bitIndex = 0;
            long byteIndex = 0;
            //Reset progress
            progress(0);
            for (; position.Y < image.Height; position.Y++)
            {
                for (; position.X < image.Width; position.X++)
                {
                    //Encoding completed
                    if (byteIndex == source.LongLength)
                    {
                        progress(100);
                        return byteIndex;
                    }
                    //Get color from pixel
                    Color color = image.GetPixel(position.X, position.Y);
                    //Get array 4 bytes of: Alpha, Red, Green and Blue
                    byte[] argb = BitConverter.GetBytes(color.ToArgb());
                    //Change 2 least significant bit for each color and alpha to our source byte
                    for (int i = 0; i < argb.Length; i++)
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            //Check cencellation, if operation cancelled throw exception
                            if (token.IsCancellationRequested)
                                token.ThrowIfCancellationRequested();
                            //Test a bit (if bit is set)
                            if ((source[byteIndex] & 1 << bitIndex) != 0)
                                //Set bit in argb
                                argb[i] = (byte)(argb[i] | 1 << j);
                            else
                                //Else clear bit in argb
                                argb[i] = (byte)(argb[i] & ~(1 << j));
                            bitIndex++;
                        }
                    }
                    //Byte encoding completed
                    bitIndex = 0;
                    byteIndex++;
                    //Get new color from argb and set into image
                    color = Color.FromArgb(BitConverter.ToInt32(argb, 0));
                    image.SetPixel(position.X, position.Y, color);
                    if (byteIndex % 4096 == 0)
                        //Update progress
                        progress((int)(byteIndex * 100 / source.LongLength));
                }
                position.X = 0;
            }
            progress(100);
            return byteIndex;
        }

        /// <summary>
        /// Decode byte array from bitmap image from offset position with cancellation
        /// </summary>
        /// <param name="image">Bitmap image<</param>
        /// <param name="decodeByteCount">Count of bytes for decoding</param>
        /// <param name="offset">offset</param>
        /// <param name="token">CancellationToken</param>
        /// <returns>Decoded byte array</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="OperationCanceledException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        protected byte[] Decode(Bitmap image, long decodeByteCount, int offset, CancellationToken token)
        {
            if (decodeByteCount > image.Width * image.Height - offset || decodeByteCount <= 0)
                throw new ArgumentOutOfRangeException("length", decodeByteCount, "Incorrect length value");
            if (token.IsCancellationRequested)
                token.ThrowIfCancellationRequested();
            Point position = GetPoint(image, offset);
            byte[] stream = new byte[decodeByteCount];
            int bitCount = 0;
            long byteIndex = 0;
            for (; position.Y < image.Height; position.Y++)
            {
                for (; position.X < image.Width; position.X++)
                {
                    if (byteIndex == decodeByteCount)
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

        /// <summary>
        /// Decode byte array from bitmap image from offset position with cancellation and progress notifier
        /// </summary>
        /// <param name="image">Bitmap image</param>
        /// <param name="decodeByteCount">Count of bytes for decoding</param>
        /// <param name="offset">offset</param>
        /// <param name="token">CancellationToken</param>
        /// <param name="progress">Progress notifier method</param>
        /// <returns>Decoded byte array</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="OperationCanceledException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        protected byte[] Decode(Bitmap image, long decodeByteCount, int offset, CancellationToken token, Action<int> progress)
        {
            //Check input params
            if (progress == null)
                throw new ArgumentNullException("progress", "Progress null argument");
            if (decodeByteCount > image.Width * image.Height - offset || decodeByteCount <= 0)
                throw new ArgumentOutOfRangeException("length", decodeByteCount, "Incorrect length value");
            //Check CancellationToken
            if (token.IsCancellationRequested)
                token.ThrowIfCancellationRequested();
            //Get pixel position from offset
            Point position = GetPoint(image, offset);
            //Allocate memory - all bits clear
            byte[] stream = new byte[decodeByteCount];
            //Set index for bits and index for bytes
            int bitCount = 0;
            long byteIndex = 0;
            //Reset progress
            progress(0);
            for (; position.Y < image.Height; position.Y++)
            {
                for (; position.X < image.Width; position.X++)
                {
                    //Decoding completed
                    if (byteIndex == decodeByteCount)
                    {
                        progress(100);
                        return stream;
                    }
                    //Get color from pixel
                    Color color = image.GetPixel(position.X, position.Y);
                    //Get array 4 bytes of: Alpha, Red, Green and Blue
                    byte[] b = BitConverter.GetBytes(color.ToArgb());
                    //Get 2 2 least significant bits from each color component
                    for (int i = 0; i < b.Length; i++)
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            //Check cencellation, if operation cancelled throw exception
                            if (token.IsCancellationRequested)
                                token.ThrowIfCancellationRequested();
                            //Test a bit
                            if ((b[i] & 1 << j) != 0)
                                //Set bit
                                stream[byteIndex] = (byte)(stream[byteIndex] | 1 << bitCount);
                            bitCount++;
                        }
                    }
                    //Byte completed
                    bitCount = 0;
                    byteIndex++;
                    //Change progress
                    if (byteIndex % 4096 == 0)
                        progress((int)(byteIndex * 100 / stream.LongLength));
                }
                position.X = 0;
            }
            progress(100);
            return stream;
        }
    }
}