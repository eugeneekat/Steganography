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

using System.IO;

using Encryptors;
using EncryptionExtenstions;
using System.Security.Cryptography;

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
                    return this.bmp.Width * this.bmp.Height - this.seg.marker.Length;                
            }
            set
            {
                this.BmpPixelCount = value;
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
            //Enable/Disable interface for Text/File
            this.txtBoxText.Enabled                 = this.radioBtnText.Checked;
            this.txtBoxFile.Enabled                 = this.radioBtnFile.Checked;    
            //Set progressBar for text and file input      
            if (this.radioBtnText.Checked == true)
            {
                this.txtBoxText.ReadOnly = false;
                this.progressBarCapacity.Value      = this.txtBoxText.Text.Length;
                int maxLength = this.BmpPixelCount > (int)Int16.MaxValue ? (int)Int16.MaxValue : this.BmpPixelCount ?? 0;
                this.progressBarCapacity.Maximum    = this.txtBoxText.MaxLength = maxLength;         
            }
            else
            {
                this.progressBarCapacity.Maximum    = this.BmpPixelCount??0;
                this.progressBarCapacity.Value      = this.file == null ? 0 : (int)this.file.Length;             
            }
        }

        //Load Image
        private void txtBoxInputImage_MouseDown(object sender, MouseEventArgs e)
        {
            //Set filter
            this.openFileDialog.Filter = "Image Files(BMP, JPG, GIF)| *.BMP; *.JPG; *.GIF";
            if (this.openFileDialog.ShowDialog() == DialogResult.OK && this.txtBoxInputImage.Text != this.openFileDialog.FileName)
            {                 
                try
                {
                    //Open image and covert into Bitmap
                    using (Image image = Image.FromFile(this.openFileDialog.FileName))
                        this.bmp = new Bitmap(image);
                    //Set capacity filename and reset texbox input
                    this.txtBoxText.Text                = string.Empty;
                    this.txtBoxInputImage.Text          = this.openFileDialog.FileName;
                    this.progressBarCapacity.Maximum    = this.txtBoxText.MaxLength = this.BmpPixelCount ?? 0;
                    //Unblock interface
                    foreach (RadioButton button in this.groupInputData.Controls.OfType<RadioButton>())
                        button.Enabled = true;
                    foreach (TextBox textBox in this.groupOutputData.Controls.OfType<TextBox>())
                        textBox.Enabled = true;
                    this.radioBtnFile.Checked = true;
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
            //If decode checked Unblock interface for decode and clear texbox
            if (this.radioBtnDecode.Checked == true)
            {
                this.radioBtnText.Checked = this.txtBoxText.ReadOnly = true;
                this.radioBtnFile.Enabled = false;
                this.txtBoxText.Text = this.txtBoxOutputFolder.Text = this.txtBoxOutputFileName.Text = string.Empty;             
            }
            //Else encode checked
            else
            {
                this.radioBtnFile.Enabled = this.txtBoxOutputFolder.Enabled = true;
                this.txtBoxText.ReadOnly = false;               
            }
        }

        //Load file for encoding
        private void txtBoxFile_MouseDown(object sender, MouseEventArgs e)
        {
            //Clear fileFilter
            this.openFileDialog.Filter = string.Empty;
            if (this.openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //if some file open - close it, clear text and reset progressbar
                if (this.file != null)
                {
                    this.file.Close();
                    this.txtBoxFile.Text = string.Empty;
                    this.progressBarCapacity.Value = 0;
                }
                try
                {
                    this.file = File.Open(this.openFileDialog.FileName, FileMode.Open, FileAccess.ReadWrite);
                    //If image capacity less than opened file size - close file and generate exception
                    if (this.progressBarCapacity.Maximum < this.file.Length)
                    {
                        this.file.Close();
                        this.file = null;
                        throw new OutOfMemoryException("File size more than image capacity");
                    }
                    //Set file name in textbox and capacity in progressbar
                    this.txtBoxFile.Text            = this.openFileDialog.FileName;
                    this.progressBarCapacity.Value  = (int)this.file.Length;                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                }
            }
        }

        //Textbox Typing - change Capacity progressbar and block Encode/Decode interface when text empty
        private void txtBoxText_TextChanged(object sender, EventArgs e)
        {
            this.radioBtnDecode.Enabled     = this.radioBtnEncode.Enabled = this.checkBoxEncrypt.Enabled = this.txtBoxText.TextLength > 0 ? true : false;
            this.progressBarCapacity.Value  = this.txtBoxText.Text.Length;
        }        

        //Folder dialog for output folder choice
        private void txtBoxOutputFolder_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.folderBrowserDialogOutput.ShowDialog() == DialogResult.OK)
                this.txtBoxOutputFolder.Text = folderBrowserDialogOutput.SelectedPath;
        }

        //Block encode/decode interface when radiobutton selected file, but file not open
        private void txtBoxFile_TextChanged(object sender, EventArgs e)
        {
            this.radioBtnEncode.Enabled = this.radioBtnDecode.Enabled = this.checkBoxEncrypt.Enabled = this.txtBoxFile.Text == string.Empty ? false : true;
        }

        //Start work
        private async void btnStart_Click(object sender, EventArgs e)
        {
            string path = this.txtBoxOutputFolder.Text + '\\' + this.txtBoxOutputFileName.Text;
            //Encode
            if (this.radioBtnEncode.Checked)
            {
                //File
                if (this.radioBtnFile.Checked)
                {
                    //Encrypt
                    if (this.checkBoxEncrypt.Checked && this.txtBoxPassword.Text != string.Empty)
                    {
                        FileStream tmp = null;
                        //Get file name and extension
                        string tmpPath = Path.GetFileName(this.file.Name);
                        if (File.Exists(tmpPath))
                        {
                            MessageBox.Show(string.Format("Can't create temp file: {0} - file alreadyExist", tmpPath), "Error");
                            return;
                        }
                        try
                        {
                            //Create temp file for encrypion
                            tmp = new FileStream(tmpPath, FileMode.CreateNew, FileAccess.ReadWrite);
                            //Async file in temp, encrypt file and PutFile into image
                            await this.file.CopyToAsync(tmp);
                            await tmp.EncryptAsync(ByteEncryptor.Xor, Encoding.Unicode.GetBytes(this.txtBoxPassword.Text));
                            await Task.Run(() => this.seg.PutFile(this.bmp, tmp));
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Error");
                        }
                        finally
                        {
                            if (tmp != null)
                            {
                                tmp.Close();
                                File.Delete(tmp.Name);
                            }
                        }
                    }
                    //Don't encrypt
                    else
                        await Task.Run(() => this.seg.PutFile(this.bmp, this.file));
                }
                //Text
                else
                {
                    this.seg.PutText(this.bmp, this.txtBoxText.Text);
                }
                this.bmp.Save(path);
            }
            //Decode
            else
            {
                this.bmp = new Bitmap(this.openFileDialog.FileName);
                //File
                if (this.txtBoxOutputFolder.Text != string.Empty)
                {
                    FileStream fs = null;
                    try
                    {
                        //Async get file from bmp
                        fs = await Task.Run(() => this.seg.GetFile(this.bmp, path));
                        //Decrypt
                        if (this.checkBoxEncrypt.Checked)
                            await fs.DecryptAsync(ByteEncryptor.Xor, Encoding.Unicode.GetBytes(this.txtBoxPassword.Text));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error");
                    }
                    finally
                    {
                        if (fs != null)
                        {
                            fs.Close();
                            File.Delete(fs.Name);
                        }
                    }
                }
                else
                {
                    this.txtBoxText.Text = this.seg.GetText(this.bmp);
                }
            }
        }
    }
}
