namespace Steganography
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.btnStart = new System.Windows.Forms.Button();
            this.groupProps = new System.Windows.Forms.GroupBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtBoxPassword = new System.Windows.Forms.TextBox();
            this.checkBoxEncrypt = new System.Windows.Forms.CheckBox();
            this.progressBarEncode = new System.Windows.Forms.ProgressBar();
            this.groupData = new System.Windows.Forms.GroupBox();
            this.progressBarCapacity = new System.Windows.Forms.ProgressBar();
            this.btnChooseOutput = new System.Windows.Forms.Button();
            this.txtBoxOutputImage = new System.Windows.Forms.TextBox();
            this.lblOutputImage = new System.Windows.Forms.Label();
            this.txtBoxText = new System.Windows.Forms.TextBox();
            this.txtBoxFile = new System.Windows.Forms.TextBox();
            this.radioBtnText = new System.Windows.Forms.RadioButton();
            this.radioBtnFile = new System.Windows.Forms.RadioButton();
            this.lblData = new System.Windows.Forms.Label();
            this.lblInputImage = new System.Windows.Forms.Label();
            this.txtBoxInputImage = new System.Windows.Forms.TextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.radioBtnEncode = new System.Windows.Forms.RadioButton();
            this.radioBtnDecode = new System.Windows.Forms.RadioButton();
            this.folderBrowserDialogOutput = new System.Windows.Forms.FolderBrowserDialog();
            this.groupProps.SuspendLayout();
            this.groupData.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(267, 460);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 40);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // groupProps
            // 
            this.groupProps.Controls.Add(this.lblPassword);
            this.groupProps.Controls.Add(this.txtBoxPassword);
            this.groupProps.Controls.Add(this.checkBoxEncrypt);
            this.groupProps.Location = new System.Drawing.Point(12, 404);
            this.groupProps.Name = "groupProps";
            this.groupProps.Size = new System.Drawing.Size(330, 50);
            this.groupProps.TabIndex = 1;
            this.groupProps.TabStop = false;
            this.groupProps.Text = "Properties";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(91, 20);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(56, 13);
            this.lblPassword.TabIndex = 11;
            this.lblPassword.Text = "Password:";
            // 
            // txtBoxPassword
            // 
            this.txtBoxPassword.Enabled = false;
            this.txtBoxPassword.Location = new System.Drawing.Point(153, 16);
            this.txtBoxPassword.Name = "txtBoxPassword";
            this.txtBoxPassword.Size = new System.Drawing.Size(167, 20);
            this.txtBoxPassword.TabIndex = 11;
            // 
            // checkBoxEncrypt
            // 
            this.checkBoxEncrypt.AutoSize = true;
            this.checkBoxEncrypt.Location = new System.Drawing.Point(9, 19);
            this.checkBoxEncrypt.Name = "checkBoxEncrypt";
            this.checkBoxEncrypt.Size = new System.Drawing.Size(76, 17);
            this.checkBoxEncrypt.TabIndex = 0;
            this.checkBoxEncrypt.Text = "Encryption";
            this.checkBoxEncrypt.UseVisualStyleBackColor = true;
            this.checkBoxEncrypt.CheckedChanged += new System.EventHandler(this.checkBoxEncrypt_CheckedChanged);
            // 
            // progressBarEncode
            // 
            this.progressBarEncode.Location = new System.Drawing.Point(80, 460);
            this.progressBarEncode.Name = "progressBarEncode";
            this.progressBarEncode.Size = new System.Drawing.Size(181, 40);
            this.progressBarEncode.TabIndex = 11;
            // 
            // groupData
            // 
            this.groupData.Controls.Add(this.progressBarCapacity);
            this.groupData.Controls.Add(this.btnChooseOutput);
            this.groupData.Controls.Add(this.txtBoxOutputImage);
            this.groupData.Controls.Add(this.lblOutputImage);
            this.groupData.Controls.Add(this.txtBoxText);
            this.groupData.Controls.Add(this.txtBoxFile);
            this.groupData.Controls.Add(this.radioBtnText);
            this.groupData.Controls.Add(this.radioBtnFile);
            this.groupData.Controls.Add(this.lblData);
            this.groupData.Controls.Add(this.lblInputImage);
            this.groupData.Controls.Add(this.txtBoxInputImage);
            this.groupData.Location = new System.Drawing.Point(12, 12);
            this.groupData.Name = "groupData";
            this.groupData.Size = new System.Drawing.Size(330, 386);
            this.groupData.TabIndex = 0;
            this.groupData.TabStop = false;
            this.groupData.Text = "Data";
            // 
            // progressBarCapacity
            // 
            this.progressBarCapacity.Location = new System.Drawing.Point(9, 296);
            this.progressBarCapacity.Name = "progressBarCapacity";
            this.progressBarCapacity.Size = new System.Drawing.Size(311, 23);
            this.progressBarCapacity.TabIndex = 10;
            // 
            // btnChooseOutput
            // 
            this.btnChooseOutput.Location = new System.Drawing.Point(114, 326);
            this.btnChooseOutput.Name = "btnChooseOutput";
            this.btnChooseOutput.Size = new System.Drawing.Size(75, 23);
            this.btnChooseOutput.TabIndex = 9;
            this.btnChooseOutput.Text = "Choose...";
            this.btnChooseOutput.UseVisualStyleBackColor = true;
            this.btnChooseOutput.Click += new System.EventHandler(this.btnChooseOutput_Click);
            // 
            // txtBoxOutputImage
            // 
            this.txtBoxOutputImage.Location = new System.Drawing.Point(9, 355);
            this.txtBoxOutputImage.Name = "txtBoxOutputImage";
            this.txtBoxOutputImage.Size = new System.Drawing.Size(311, 20);
            this.txtBoxOutputImage.TabIndex = 8;
            // 
            // lblOutputImage
            // 
            this.lblOutputImage.AutoSize = true;
            this.lblOutputImage.Location = new System.Drawing.Point(6, 331);
            this.lblOutputImage.Name = "lblOutputImage";
            this.lblOutputImage.Size = new System.Drawing.Size(102, 13);
            this.lblOutputImage.TabIndex = 7;
            this.lblOutputImage.Text = "Output image folder:";
            // 
            // txtBoxText
            // 
            this.txtBoxText.Enabled = false;
            this.txtBoxText.Location = new System.Drawing.Point(9, 120);
            this.txtBoxText.Multiline = true;
            this.txtBoxText.Name = "txtBoxText";
            this.txtBoxText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtBoxText.Size = new System.Drawing.Size(311, 170);
            this.txtBoxText.TabIndex = 6;
            this.txtBoxText.TextChanged += new System.EventHandler(this.txtBoxText_TextChanged);
            // 
            // txtBoxFile
            // 
            this.txtBoxFile.Location = new System.Drawing.Point(56, 71);
            this.txtBoxFile.Name = "txtBoxFile";
            this.txtBoxFile.Size = new System.Drawing.Size(264, 20);
            this.txtBoxFile.TabIndex = 5;
            this.txtBoxFile.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtBoxFile_MouseDown);
            // 
            // radioBtnText
            // 
            this.radioBtnText.AutoSize = true;
            this.radioBtnText.Location = new System.Drawing.Point(9, 97);
            this.radioBtnText.Name = "radioBtnText";
            this.radioBtnText.Size = new System.Drawing.Size(143, 17);
            this.radioBtnText.TabIndex = 4;
            this.radioBtnText.Text = "Text (Max - 32767 chars)";
            this.radioBtnText.UseVisualStyleBackColor = true;
            // 
            // radioBtnFile
            // 
            this.radioBtnFile.AutoSize = true;
            this.radioBtnFile.Checked = true;
            this.radioBtnFile.Location = new System.Drawing.Point(9, 72);
            this.radioBtnFile.Name = "radioBtnFile";
            this.radioBtnFile.Size = new System.Drawing.Size(41, 17);
            this.radioBtnFile.TabIndex = 3;
            this.radioBtnFile.TabStop = true;
            this.radioBtnFile.Text = "File";
            this.radioBtnFile.UseVisualStyleBackColor = true;
            this.radioBtnFile.CheckedChanged += new System.EventHandler(this.radioBtnFile_CheckedChanged);
            // 
            // lblData
            // 
            this.lblData.AutoSize = true;
            this.lblData.Location = new System.Drawing.Point(6, 55);
            this.lblData.Name = "lblData";
            this.lblData.Size = new System.Drawing.Size(58, 13);
            this.lblData.TabIndex = 2;
            this.lblData.Text = "Input data:";
            // 
            // lblInputImage
            // 
            this.lblInputImage.AutoSize = true;
            this.lblInputImage.Location = new System.Drawing.Point(6, 16);
            this.lblInputImage.Name = "lblInputImage";
            this.lblInputImage.Size = new System.Drawing.Size(39, 13);
            this.lblInputImage.TabIndex = 1;
            this.lblInputImage.Text = "Image:";
            // 
            // txtBoxInputImage
            // 
            this.txtBoxInputImage.Location = new System.Drawing.Point(9, 32);
            this.txtBoxInputImage.Name = "txtBoxInputImage";
            this.txtBoxInputImage.Size = new System.Drawing.Size(311, 20);
            this.txtBoxInputImage.TabIndex = 0;
            this.txtBoxInputImage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtBoxInputImage_MouseDown);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(9, 19);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(74, 17);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "Encrypted";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(145, 17);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(175, 20);
            this.textBox1.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(83, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 11;
            // 
            // radioBtnEncode
            // 
            this.radioBtnEncode.AutoSize = true;
            this.radioBtnEncode.Checked = true;
            this.radioBtnEncode.Location = new System.Drawing.Point(12, 460);
            this.radioBtnEncode.Name = "radioBtnEncode";
            this.radioBtnEncode.Size = new System.Drawing.Size(62, 17);
            this.radioBtnEncode.TabIndex = 11;
            this.radioBtnEncode.TabStop = true;
            this.radioBtnEncode.Text = "Encode";
            this.radioBtnEncode.UseVisualStyleBackColor = true;
            // 
            // radioBtnDecode
            // 
            this.radioBtnDecode.AutoSize = true;
            this.radioBtnDecode.Location = new System.Drawing.Point(12, 483);
            this.radioBtnDecode.Name = "radioBtnDecode";
            this.radioBtnDecode.Size = new System.Drawing.Size(63, 17);
            this.radioBtnDecode.TabIndex = 12;
            this.radioBtnDecode.Text = "Decode";
            this.radioBtnDecode.UseVisualStyleBackColor = true;
            this.radioBtnDecode.CheckedChanged += new System.EventHandler(this.radioBtnDecode_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(354, 509);
            this.Controls.Add(this.radioBtnDecode);
            this.Controls.Add(this.radioBtnEncode);
            this.Controls.Add(this.progressBarEncode);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.groupData);
            this.Controls.Add(this.groupProps);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupProps.ResumeLayout(false);
            this.groupProps.PerformLayout();
            this.groupData.ResumeLayout(false);
            this.groupData.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.GroupBox groupProps;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtBoxPassword;
        private System.Windows.Forms.CheckBox checkBoxEncrypt;
        private System.Windows.Forms.ProgressBar progressBarEncode;
        private System.Windows.Forms.GroupBox groupData;
        private System.Windows.Forms.ProgressBar progressBarCapacity;
        private System.Windows.Forms.Button btnChooseOutput;
        private System.Windows.Forms.TextBox txtBoxOutputImage;
        private System.Windows.Forms.Label lblOutputImage;
        private System.Windows.Forms.TextBox txtBoxText;
        private System.Windows.Forms.TextBox txtBoxFile;
        private System.Windows.Forms.RadioButton radioBtnText;
        private System.Windows.Forms.RadioButton radioBtnFile;
        private System.Windows.Forms.Label lblData;
        private System.Windows.Forms.Label lblInputImage;
        private System.Windows.Forms.TextBox txtBoxInputImage;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton radioBtnEncode;
        private System.Windows.Forms.RadioButton radioBtnDecode;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogOutput;
    }
}

