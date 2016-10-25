using System;
using System.Drawing;

using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Steganography
{
    class LSBEncryptor
    {
        public static string LSBMark = "EE";
       
        static public void InsertToImage(Bitmap image, byte[] source, int offset = 0)
        {
            if (source.Length > image.Height * image.Width - offset)
                throw new ArgumentOutOfRangeException("source", source.Length, "Target data size more than container size");
            //Get current pixel from offset
            Point position = new Point();
            if (offset < image.Width)
                position.X = offset;
            else
            {
                int y = offset;
                while(y >= image.Width)
                {
                    position.Y++;
                    y -= image.Width;
                }
                position.X = y;
            }

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
        }
        
        static public byte [] OutFromImage(Bitmap image, int length, int offset = 0)
        {
            if(length > image.Height * image.Width - offset)
                throw new ArgumentOutOfRangeException("length", length, "Target data size more than container size");
            //Get current pixel from offset
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
