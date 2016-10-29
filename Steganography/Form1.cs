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
using System.IO;

namespace Steganography
{
    public partial class Form1 : Form
    {
        Bitmap bmp = null;
        BmpSteg seg = new BmpSteg();
        int? BmpPixelCount
        {
            get
            {
                if (this.bmp == null)
                    return null;
                else
                    return this.bmp.Width * this.bmp.Height;
            }
        }

        FileStream file = null;

        public Form1()
        {
            InitializeComponent();
        }

        //CheckBox encrypt and textBoxEncrypt
        private void checkBoxEncrypt_CheckedChanged(object sender, EventArgs e)
        {
            this.txtBoxPassword.Enabled = this.checkBoxEncrypt.Checked;
        }

        //Change control elements - File/Text
        private void radioBtnFile_CheckedChanged(object sender, EventArgs e)
        {
            this.txtBoxText.Enabled = this.radioBtnText.Checked;
            this.txtBoxFile.Enabled = this.radioBtnFile.Checked;          
            if (this.radioBtnText.Checked == true)
            {
                this.progressBarCapacity.Maximum = this.txtBoxText.MaxLength;
                this.progressBarCapacity.Value = this.txtBoxText.Text.Length;               
            }
            else
            {
                this.progressBarCapacity.Maximum = this.BmpPixelCount ?? 0;
                this.progressBarCapacity.Value      = this.file == null ? 0 : (int)this.file.Length;                
            }
        }

        private void txtBoxInputImage_MouseDown(object sender, MouseEventArgs e)
        {
            this.openFileDialog.Filter = "Image Files(BMP, JPG, GIF)| *.BMP; *.JPG; *.GIF";
            if (this.openFileDialog.ShowDialog() == DialogResult.OK && this.txtBoxInputImage.Text != this.openFileDialog.FileName)
            {
                try
                {
                    using (Image image = Image.FromFile(this.openFileDialog.FileName))
                        this.bmp = new Bitmap(image);
                    this.txtBoxInputImage.Text          = this.openFileDialog.FileName;
                    //Служебная информация +
                    if(this.radioBtnFile.Checked)
                        this.progressBarCapacity.Maximum    = this.BmpPixelCount ?? 0;

                    if (this.file != null && this.file.Length > this.progressBarCapacity.Maximum)
                    {
                        this.file.Close();
                        this.txtBoxFile.Text = string.Empty;
                        this.progressBarCapacity.Value = 0;
                    }                                                     
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                }
            }

        }

        //Change control elements - encode/decode
        private void radioBtnDecode_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioBtnDecode.Checked == true)
            {
                this.radioBtnText.Checked = this.txtBoxText.ReadOnly = true;
                this.radioBtnFile.Enabled = false;
                this.lblOutputImage.Text = "Output file folder:";
            }
            else
            {
                this.radioBtnFile.Enabled = this.btnChooseOutput.Enabled = this.txtBoxOutputImage.Enabled = true;
                this.txtBoxText.ReadOnly = false;
                this.lblOutputImage.Text = "Output image folder:";
            }
        }

        //Open Encoding file
        private void txtBoxFile_MouseDown(object sender, MouseEventArgs e)
        {
            this.openFileDialog.Filter = string.Empty;
            if (this.openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (this.file != null)
                    this.file.Close();
                try
                {
                    this.file = File.Open(this.openFileDialog.FileName, FileMode.Open, FileAccess.ReadWrite);

                    if (this.progressBarCapacity.Maximum < this.file.Length)
                    {
                        this.file.Close();
                        this.file = null;
                        throw new OutOfMemoryException("File size more than image capacity");
                    }

                    this.txtBoxFile.Text            = this.openFileDialog.FileName;
                    this.progressBarCapacity.Value  = (int)this.file.Length;
                }
                catch (OutOfMemoryException ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error open file: " + ex.Message, "Error");
                }
            }
        }

        //Textbox Typing
        private void txtBoxText_TextChanged(object sender, EventArgs e)
        {
            this.progressBarCapacity.Value = this.txtBoxText.Text.Length;
        }




        private void btnChooseOutput_Click(object sender, EventArgs e)
        {
            if(this.folderBrowserDialogOutput.ShowDialog() == DialogResult.OK)
            {
                this.txtBoxOutputImage.Text = folderBrowserDialogOutput.SelectedPath;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (this.radioBtnEncode.Checked)
            {
                if (this.radioBtnFile.Checked)
                {
                    byte[] buf = new byte[file.Length];
                    this.file.Read(buf, 0, buf.Length);
                    ByteEncryptor.Xor.Encrypt(ref buf, Encoding.Unicode.GetBytes(this.txtBoxPassword.Text));
                    this.seg.InsertToImage(this.bmp, buf);
                    this.bmp.Save(this.txtBoxOutputImage.Text);
                }
            }
            else
            {
                this.bmp = new Bitmap(this.openFileDialog.FileName);
                byte[] decoded = seg.OutFromImage(this.bmp);
                ByteEncryptor.Xor.Decrypt(ref decoded, Encoding.Unicode.GetBytes(this.txtBoxPassword.Text));
                File.WriteAllBytes(this.txtBoxOutputImage.Text, decoded);
            }
        }
    }
}
