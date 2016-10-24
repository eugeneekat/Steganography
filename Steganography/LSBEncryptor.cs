using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Steganography
{
    class LSBEncryptor
    {

        byte[] message = null;
       
        
        Bitmap bmp = null;

        public LSBEncryptor(string message, string password, Bitmap bmp)
        {
            MD5 md = MD5.Create();
            
            this.message = Encoding.Unicode.GetBytes(message);
            
            this.bmp = bmp;          
        }

        public Bitmap Encrypt()
        {
            Point p = new Point(0,0);
            this.InsertToImage(bmp, this.message, ref p);
            return this.bmp;
        }

        public void Decrypt()
        {
            Point p = new Point(0,0);
           

            byte[] message = this.OutFromImage(this.bmp, this.message.Length,  ref p);

            MessageBox.Show(Encoding.Unicode.GetString(message));
        }


        /// <summary>
        /// Insert byte array to image
        /// </summary>
        /// <param name="image">Bitmap image</param>
        /// <param name="source">Bite array of source</param>
        /// <param name="position">Reference point of start position(every step change point)</param>
        protected void InsertToImage(Bitmap image, byte[] source, ref Point position)
        {
            //Check correct coordinates
            if (position.X > image.Width || position.X < 0 ||
               position.Y > image.Height || position.Y < 0)
                throw new IndexOutOfRangeException();

            //Set Counts for bits and index for bytes
            int bitCount = 0;
            int byteIndex = 0;

            for (; position.Y < image.Height; position.Y++)
            {
                for(; position.X < image.Width; position.X++)
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
            if (byteIndex != source.Length)
                //Exception 
                throw new OutOfMemoryException("");           
        }

        /// <summary>
        /// Read byte array from image
        /// </summary>
        /// <param name="image">Bitmap image</param>
        /// <param name="length">Number of bytes out</param>
        /// <param name="position">Reference point of start position(every step change point)</param>
        /// <returns>Return byte array from image</returns>
        protected byte [] OutFromImage(Bitmap image, int length, ref Point position)
        {
            if (position.X > image.Width || position.X < 0 ||
               position.Y > image.Height || position.Y < 0)
                throw new IndexOutOfRangeException();

            //Allocate memory(all bits clear)
            byte[] stream = new byte[length];

            int bitCount = 0;
            int byteIndex = 0;

            for(; position.Y < image.Height; position.Y++)
            {
                for(; position.X < image.Width; position.X++)
                {
                    if (byteIndex == length)
                        return stream;
                    Color color = bmp.GetPixel(position.X, position.Y);
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
            if (byteIndex != length - 1)
                //Exception 
                throw new OutOfMemoryException("");
            return stream;
        }

    }
}
