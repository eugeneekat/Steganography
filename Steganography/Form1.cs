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
using System.Threading;

using Encryptors;
using EncryptionExtension;
using System.Security.Cryptography;

namespace Steganography
{
    public partial class Form1 : Form
    {
        //MD5 reserved size for encryption data
        readonly int md5Size = MD5.Create().HashSize * 2;
        Bitmap bmp = null;
        BmpSteg seg = new BmpSteg();
        
        //Get aviable pixels for encode
        int? BmpPixelCount
        {
            get
            {
                if (this.bmp == null)
                    return null;
                else
                    return this.bmp.Width * this.bmp.Height - this.seg.fileMarker.Length - md5Size;                
            }
        }

        FileStream file = null;

        CancellationTokenSource source = new CancellationTokenSource();
        CancellationToken token;

        public Form1()
        {
            InitializeComponent();
            this.token = source.Token;
        }
        
        //CheckBox encrypt and textBoxEncrypt
        private void checkBoxEncrypt_CheckedChanged(object sender, EventArgs e)
        {
            this.txtBoxPassword.Enabled = this.checkBoxEncrypt.Checked;
            
        }
        
        //Folder dialog for output folder
        private void txtBoxOutputFolder_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.folderBrowserDialogOutput.ShowDialog() == DialogResult.OK)
                this.txtBoxOutputFolder.Text = folderBrowserDialogOutput.SelectedPath;
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
                    //using (Image image = Image.FromFile(this.openFileDialog.FileName))//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    using (FileStream fs = File.Open(this.openFileDialog.FileName, FileMode.Open, FileAccess.ReadWrite))
                        this.bmp = new Bitmap(fs);
                    //Set capacity filename and reset texbox input
                    this.txtBoxText.Text                = string.Empty;
                    this.txtBoxInputImage.Text          = this.openFileDialog.FileName;
                    this.progressBarCapacity.Maximum    = this.txtBoxText.MaxLength = this.BmpPixelCount ?? 0;
                    //Unblock interface
                    foreach (RadioButton button in this.groupInputData.Controls.OfType<RadioButton>())
                        button.Enabled = true;
                    foreach (TextBox textBox in this.groupOutputData.Controls.OfType<TextBox>())
                        textBox.Enabled = true;
                    this.radioBtnFile.Checked = this.radioBtnEncode.Checked = true;
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
                    //If image capacity less than opened file size and file extension size - close file and generate exception
                    if (this.progressBarCapacity.Maximum - Path.GetExtension(openFileDialog.FileName).Length < this.file.Length)
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
            this.progressBarCapacity.Value  = this.txtBoxText.Text.Length;
        }        

        

        //Start work
        private async void btnStart_Click(object sender, EventArgs e)
        {
            //Input data validation
            string validation = this.IsValidData();
            if(validation != string.Empty)
            {
                MessageBox.Show(validation, "Error");
                return;
            }           
            //Prepare file for save output result and temp file for encoding output result file
            FileStream saveFile = null;
            FileStream tempFile = null;           
            try
            {
                //Prepare path for save
                string path = this.txtBoxOutputFolder.Text + '\\' + this.txtBoxOutputFileName.Text;
                //Block controls
                foreach (Control c in this.Controls)
                    c.Enabled = false;
                //Encode
                if (this.radioBtnEncode.Checked)
                {
                    this.bmp = new Bitmap(Image.FromFile(this.txtBoxInputImage.Text));                    
                    //File
                    if (this.radioBtnFile.Checked)
                    {
                        //Encrypt
                        if (this.checkBoxEncrypt.Checked && this.txtBoxPassword.Text != string.Empty)
                        {
                            //Get file name and extension
                            string tmpPath = Path.GetFileName(this.file.Name);
                            if (File.Exists(tmpPath))
                                throw new FileLoadException(string.Format("Can't create temp file: {0} - file alreadyExist", tmpPath));
                            //Create temp file for encrypion
                            tempFile = new FileStream(tmpPath, FileMode.CreateNew, FileAccess.ReadWrite);
                            //Async file in temp, encrypt file and PutFile into image
                            await this.file.CopyToAsync(tempFile, (int)file.Length, this.token);
                            await tempFile.EncryptAsync(ByteEncryptor.Xor, Encoding.Unicode.GetBytes(this.txtBoxPassword.Text), this.token);
                            await Task.Run(() => this.seg.PutFile(this.bmp, tempFile, this.token));
                        }
                        //Don't encrypt
                        else
                            await Task.Run(() => this.seg.PutFile(this.bmp, this.file, this.token));                          
                    }
                    //Text
                    else
                    {
                        //Encrypt
                        if (this.checkBoxEncrypt.Checked && this.txtBoxPassword.Text != string.Empty)
                            this.seg.PutText(this.bmp, this.EncryptionXorText(this.txtBoxText.Text, this.txtBoxPassword.Text));
                        //Don't encrypt
                        else
                            this.seg.PutText(this.bmp, this.txtBoxText.Text);
                    }
                    //Save new bmp if operation not cancelled
                    if(!this.token.IsCancellationRequested)
                        this.bmp.Save(path);
                }
                //Decode
                else
                {
                    //this.bmp = new Bitmap(File.Open(this.txtBoxInputImage.Text, FileMode.Open, FileAccess.ReadWrite));
                    //File
                    if (this.txtBoxOutputFolder.Text != string.Empty)
                    {
                        //Async get file from bmp
                        saveFile = await Task.Run(() => this.seg.GetFile(this.bmp, path, this.token));
                        //Decrypt
                        if (this.checkBoxEncrypt.Checked)
                            await saveFile.DecryptAsync(ByteEncryptor.Xor, Encoding.Unicode.GetBytes(this.txtBoxPassword.Text), this.token);
                    }
                    //Text
                    else
                    {
                        string outMessage = this.seg.GetText(this.bmp);
                        //Decrypt
                        if (this.checkBoxEncrypt.Checked && this.txtBoxPassword.Text != string.Empty)
                            outMessage = EncryptionXorText(outMessage, this.txtBoxPassword.Text);
                        this.txtBoxText.Text = outMessage;
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            finally
            {
                if (saveFile != null)
                {
                    saveFile.Close();
                    if (this.token.IsCancellationRequested)
                        File.Delete(saveFile.Name);            
                }
                if (tempFile != null)
                {
                    tempFile.Close();
                    File.Delete(tempFile.Name);
                }
                //Unblock controls
                foreach (Control c in this.Controls)
                    c.Enabled = true;
            }           
        }

        //Check input data
        private string IsValidData()
        {
            if (!this.radioBtnEncode.Checked && !this.radioBtnDecode.Checked)
                return "Select encode/decode";
            //Encode
            if (this.radioBtnEncode.Checked)
            {
                //File
                if(this.radioBtnFile.Checked)
                {
                    if (this.txtBoxFile.Text == string.Empty)
                        return "Select decoding file";
                }
                //Text
                else
                {
                    if(this.txtBoxText.Text == string.Empty)
                        return "Enter text for encoding";
                }                                 
                if (this.txtBoxOutputFolder.Text == string.Empty || this.txtBoxOutputFileName.Text == string.Empty)
                    return "Select output data - folder/file name";
            }
            return string.Empty;
        }

        //Helper method xor text encryption
        private string EncryptionXorText(string text, string password)
        {
            byte[] encText      = Encoding.Unicode.GetBytes(text);
            byte[] encPassword  = Encoding.Unicode.GetBytes(password);
            ByteEncryptor.Xor.Encrypt(ref encText, encPassword);
            return Encoding.Unicode.GetString(encText);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {          
            if (this.btnStart.Enabled == false)
            {
                e.Cancel = true;
                this.source.Cancel();               
            }
        }
    }
}
