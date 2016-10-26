using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Encryptors;
using EncryptionExtenstions;

namespace Steganography
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            

            Bitmap bmp = new Bitmap(Image.FromFile(@"1.jpg"));
            BmpSteg st = new BmpSteg();
            char[] msg = new char[(bmp.Height * bmp.Width)/2 - 3];
            for(int i = 0; i < msg.Length; i++)
            {
                msg[i] = 'H';
            }
            //byte[] source = Encoding.Unicode.GetBytes("Hello");
            //source = ByteEncryptor.Xor.Encrypt(source, Encoding.Unicode.GetBytes("World"));
            
            st.InsertToImage(bmp, Encoding.Unicode.GetBytes(msg));      
            bmp.Save(@"4.bmp");


            Bitmap bmp2 = new Bitmap(@"4.bmp");
            byte[] result = st.OutFromImage(bmp2);
            //result = ByteEncryptor.Xor.Decrypt(source, Encoding.Unicode.GetBytes("World"));
            string s = Encoding.Unicode.GetString(result);
        }

        //Open File
        private void btnFileChoose_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    this.picBoxSource.Load(this.txtBoxFilePath.Text);
            //}
            //catch(Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "Error");
            //}
        }


    }
}
