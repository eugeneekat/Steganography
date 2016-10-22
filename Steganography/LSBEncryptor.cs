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
        byte[] password = null;

        
        Bitmap bmp = null;

        public LSBEncryptor(string message, string password, Bitmap bmp)
        {
            MD5 md = MD5.Create();
            this.message = Encoding.Unicode.GetBytes(message);
            this.password = md.ComputeHash(Encoding.Unicode.GetBytes(password));
            this.bmp = bmp;          
        }

        public Bitmap Encrypt()
        {
            int bitCount = 0;
            int byteIndex = 0;

           

            for (int i = 0; i < bmp.Width; i++)
            {
                for(int j = 0; j < bmp.Height; j++)
                {
                    if (byteIndex == this.message.Length - 1)
                        return this.bmp;
                    if (bitCount == 8)
                    {
                        byteIndex++;
                        
                        bitCount = 0;
                    }

                    
                    Color color = bmp.GetPixel(i, j);
                    int argb = color.ToArgb();

                    //Меняет 2 бита в пикселе
                    for (int k = 0; k < 2; k++)
                    {
                        //Если бит поднят у сообщения то поднимает его в пикселе иначе опускаем
                        if ((this.message[byteIndex] & 1 << bitCount) != 0)
                            argb |= 1 << k;                          
                        else
                            argb &= ~(1 << k);
                        bitCount++;
                    }

                    color = Color.FromArgb(argb);
                    this.bmp.SetPixel(i, j, color);
                    
                }
            }

            return this.bmp;
            
        }

        public void Decrypt()
        {
            byte[] msg = new byte[this.message.Length];

            int bitCount = 0;
            int byteIndex = 0;

            for (int i = 0; i < bmp.Width; i++)
            {
                for(int j = 0; j < bmp.Height; j++)
                {

                    if (byteIndex == this.message.Length - 1)
                    {
                        MessageBox.Show(Encoding.Unicode.GetString(msg));
                        return;
                    }
                    if (bitCount == 8)
                    {
                        byteIndex++;
                        bitCount = 0;
                    }

                    Color color = bmp.GetPixel(i, j);

                    int argb = color.ToArgb();

                    for (int k = 0; k < 2; k++)
                    {
                        if ((argb & 1 << k) != 0)
                            msg[byteIndex] = (byte)(msg[byteIndex] | 1 << bitCount);
                        bitCount++;
                    }
                }
            }

            
            
        }


    }
}
