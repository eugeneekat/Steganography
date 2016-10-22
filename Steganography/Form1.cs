using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
            Image img = Image.FromFile(@"D:\1.JPG");

            Bitmap bmp = new Bitmap(img);

            LSBEncryptor enc = new LSBEncryptor("Hello", "123", bmp);
            enc.Encrypt();
            enc.Decrypt();
            this.picBoxResult.Image = bmp;
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
