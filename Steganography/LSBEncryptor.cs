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

        public void Encrypt()
        {
            int bitCount = 0;
            int byteNumber = 0;

            byte[] check = new byte[message.Length];

            for (int i = 0; i < bmp.Width; i++)
            {
                for(int j = 0; j < bmp.Height; j++)
                {
                    if (byteNumber == this.message.Length - 1)
                        return;
                    if (bitCount % 8 == 0 && bitCount != 0)
                    {
                        byteNumber++;
                        
                        bitCount = 0;
                    }

                    
                    Color color = bmp.GetPixel(i, j);
                    int argb = color.ToArgb();

                    //Меняет 2 бита в пикселе
                    for (int k = 0; k < 2; k++)
                    {
                        //Если бит поднят у сообщения то поднимает его в пикселе иначе опускаем
                        if ((this.message[byteNumber] & 1 << bitCount) != 0)
                        {
                            argb |= 1 << k;
                            check[byteNumber] = (byte)(check[byteNumber] | 1 << bitCount);
                        }
                        else
                        {
                            argb &= ~(1 << k);
                            
                        }
                        bitCount++;
                    }

                    color = Color.FromArgb(argb);
                    this.bmp.SetPixel(i, j, color);
                    
                }
            }
        }

        public void Decrypt()
        {
            byte[] msg = new byte[this.message.Length];

            int bitCount = 0;
            int byteNumber = 0;

            for (int i = 0; i < bmp.Width; i++)
            {
                for(int j = 0; j < bmp.Height; j++)
                {

                    if (byteNumber == this.message.Length - 1)
                    {

                        MessageBox.Show(Encoding.Unicode.GetString(msg));


                        return;
                    }
                    if (bitCount % 8 == 0 && bitCount != 0)
                    {
                        byteNumber++;

                        bitCount = 0;
                    }

                  
                    if (bitCount % 8 == 0 && bitCount != 0)
                        byteNumber++;

                    Color color = bmp.GetPixel(i, j);

                    int argb = color.ToArgb();

                    for (int k = 0; k < 2; k++)
                    {
                        if ((argb & 1 << k) != 0)
                            msg[byteNumber] = (byte)(msg[byteNumber] | 1 << bitCount);
                        bitCount++;
                    }
                }
            }

            
            
        }


    }
}
