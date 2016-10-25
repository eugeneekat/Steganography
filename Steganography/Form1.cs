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


namespace Steganography
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            

            Bitmap bmp = new Bitmap(Image.FromFile(@"1.jpg"));
            
            
            byte[] b1 = Encoding.Unicode.GetBytes("H");
            LSBEncryptor.InsertToImage(bmp, b1, 0);

          

            byte [] result = LSBEncryptor.OutFromImage(bmp, b1.Length);
            MessageBox.Show(Encoding.Unicode.GetString(result));
            this.picBoxResult.Image = bmp;
            //bmp.Save(@"5.bmp");

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
