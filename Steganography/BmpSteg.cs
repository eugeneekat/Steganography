﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Steganography
{
    class BmpSteg
    {
        protected byte [] marker = Encoding.ASCII.GetBytes("EE");

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
            if (Extract(image, marker.Length, 0).SequenceEqual(marker))
                throw new ArgumentException("Image is encoding now!", "image");
            //Insert makrer
            Insert(image, this.marker, 0);
            //Insert length
            Insert(image, BitConverter.GetBytes(source.Length), this.marker.Length);
            //Insert data
            Insert(image, source, this.marker.Length + sizeof(int));
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
            if (!this.Extract(image, this.marker.Length, 0).SequenceEqual(this.marker))
                throw new ArgumentException("Image doesn't have a mark", "image");
            //Get size
            int size = BitConverter.ToInt32(Extract(image, sizeof(int), this.marker.Length), 0);
            //Extract data
            return this.Extract(image, size, sizeof(int) + this.marker.Length);          
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
        protected bool IsAvailableFreeSpace(Bitmap image, int length)
        {
            if (image == null)
                throw new ArgumentNullException("image", "Image null argument");
            return image.Height * image.Width - this.marker.Length - sizeof(int) >= length;
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
    }
}