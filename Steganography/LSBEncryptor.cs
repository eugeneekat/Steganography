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
        enum LSBLevel { One = 1, Two = 2, Four = 4, Eight = 8};

        byte[] message = null;
        byte[] password = null;

        LSBLevel level = LSBLevel.Eight;
        
        Bitmap bmp = null;

        public LSBEncryptor(string message, string password, Bitmap bmp)
        {
            MD5 md = MD5.Create();
            
            this.message = Encoding.Unicode.GetBytes(message);
            this.password = md.ComputeHash(Encoding.ASCII.GetBytes(password));
            this.bmp = bmp;          
        }

        public Bitmap Encrypt()
        {
            Point p = new Point();
            this.InsertToImage(this.bmp, Encoding.ASCII.GetBytes("EE"), ref p);
            this.InsertToImage(this.bmp, this.password, ref p);
            this.InsertToImage(this.bmp, this.message, ref p);
            return this.bmp;
        }

        public void Decrypt()
        {
            Point p = new Point(0,0);
            
            

            byte[] msg = this.OutFromImage(this.bmp, Encoding.ASCII.GetByteCount("EE"), ref p);
            
            byte[] password = this.OutFromImage(this.bmp, this.password.Length, ref p);

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
            if (position.X > image.Width || position.X < 0 ||
               position.Y > image.Height || position.Y < 0)
                throw new IndexOutOfRangeException();

            int bitCount = 0;
            int byteIndex = 0;

            for (; position.Y < image.Height; position.Y++)
            {
                for(; position.X < image.Width; position.X++)
                {
                   
                    //Если последний индекс байта и счетчик бита равен 8 - значит конец
                    if (byteIndex == source.Length - 1 && bitCount == 8)
                        return;

                    if (bitCount == 8)
                    {
                        bitCount = 0;
                        byteIndex++;
                    }

                    Color color = image.GetPixel(position.X, position.Y);

                    int argb = color.ToArgb();

                    for (int k = 0; k < (int)this.level; k++)
                    {
                        //Если бит поднят у сообщения то поднимает его в пикселе иначе опускаем
                        if ((source[byteIndex] & 1 << bitCount) != 0)
                            argb |= 1 << k;
                        else
                            argb &= ~(1 << k);
                        bitCount++;
                    }

                    color = Color.FromArgb(argb);

                    image.SetPixel(position.X, position.Y, color);                   
                }
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

            byte[] stream = new byte[length];

            int bitCount = 0;
            int byteIndex = 0;

            for(; position.Y < image.Height; position.Y++)
            {
                for(; position.X < image.Width; position.X++)
                {
                    //Если последний индекс байта и счетчик бита равен 8 - значит конец
                    if (byteIndex == length - 1 && bitCount == 8)
                        return stream;
                    if(bitCount == 8)
                    {
                        bitCount = 0;
                        byteIndex++;
                    }
                    Color color = bmp.GetPixel(position.X, position.Y);
                    int argb = color.ToArgb();

                    for (int k = 0; k < (int)this.level; k++)
                    {
                        if ((argb & 1 << k) != 0)
                            stream[byteIndex] = (byte)(stream[byteIndex] | 1 << bitCount);
                        bitCount++;
                    }
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
