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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.btnStart = new System.Windows.Forms.Button();
            this.groupProps = new System.Windows.Forms.GroupBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtBoxPassword = new System.Windows.Forms.TextBox();
            this.checkBoxEncrypt = new System.Windows.Forms.CheckBox();
            this.progressBarEncode = new System.Windows.Forms.ProgressBar();
            this.groupInputData = new System.Windows.Forms.GroupBox();
            this.lblInputImage = new System.Windows.Forms.Label();
            this.lblData = new System.Windows.Forms.Label();
            this.radioBtnFile = new System.Windows.Forms.RadioButton();
            this.txtBoxInputImage = new System.Windows.Forms.TextBox();
            this.progressBarCapacity = new System.Windows.Forms.ProgressBar();
            this.radioBtnText = new System.Windows.Forms.RadioButton();
            this.txtBoxFile = new System.Windows.Forms.TextBox();
            this.txtBoxText = new System.Windows.Forms.TextBox();
            this.txtBoxOutputFileName = new System.Windows.Forms.TextBox();
            this.lblOutputFile = new System.Windows.Forms.Label();
            this.txtBoxOutputFolder = new System.Windows.Forms.TextBox();
            this.lblOutputFolder = new System.Windows.Forms.Label();
            this.radioBtnEncode = new System.Windows.Forms.RadioButton();
            this.radioBtnDecode = new System.Windows.Forms.RadioButton();
            this.folderBrowserDialogOutput = new System.Windows.Forms.FolderBrowserDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.groupOutputData = new System.Windows.Forms.GroupBox();
            this.lblProgress = new System.Windows.Forms.Label();
            this.groupProps.SuspendLayout();
            this.groupInputData.SuspendLayout();
            this.groupOutputData.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(267, 483);
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
            this.groupProps.Location = new System.Drawing.Point(12, 427);
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
            this.progressBarEncode.Location = new System.Drawing.Point(80, 483);
            this.progressBarEncode.Name = "progressBarEncode";
            this.progressBarEncode.Size = new System.Drawing.Size(181, 40);
            this.progressBarEncode.TabIndex = 11;
            // 
            // groupInputData
            // 
            this.groupInputData.Controls.Add(this.lblInputImage);
            this.groupInputData.Controls.Add(this.lblData);
            this.groupInputData.Controls.Add(this.radioBtnFile);
            this.groupInputData.Controls.Add(this.txtBoxInputImage);
            this.groupInputData.Controls.Add(this.progressBarCapacity);
            this.groupInputData.Controls.Add(this.radioBtnText);
            this.groupInputData.Controls.Add(this.txtBoxFile);
            this.groupInputData.Controls.Add(this.txtBoxText);
            this.groupInputData.Location = new System.Drawing.Point(12, 12);
            this.groupInputData.Name = "groupInputData";
            this.groupInputData.Size = new System.Drawing.Size(330, 326);
            this.groupInputData.TabIndex = 0;
            this.groupInputData.TabStop = false;
            this.groupInputData.Text = "Input Data";
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
            // lblData
            // 
            this.lblData.AutoSize = true;
            this.lblData.Location = new System.Drawing.Point(6, 55);
            this.lblData.Name = "lblData";
            this.lblData.Size = new System.Drawing.Size(58, 13);
            this.lblData.TabIndex = 2;
            this.lblData.Text = "Input data:";
            // 
            // radioBtnFile
            // 
            this.radioBtnFile.AutoSize = true;
            this.radioBtnFile.Enabled = false;
            this.radioBtnFile.Location = new System.Drawing.Point(9, 72);
            this.radioBtnFile.Name = "radioBtnFile";
            this.radioBtnFile.Size = new System.Drawing.Size(41, 17);
            this.radioBtnFile.TabIndex = 3;
            this.radioBtnFile.Text = "File";
            this.radioBtnFile.UseVisualStyleBackColor = true;
            this.radioBtnFile.CheckedChanged += new System.EventHandler(this.radioBtnFile_CheckedChanged);
            // 
            // txtBoxInputImage
            // 
            this.txtBoxInputImage.Location = new System.Drawing.Point(9, 32);
            this.txtBoxInputImage.Name = "txtBoxInputImage";
            this.txtBoxInputImage.ReadOnly = true;
            this.txtBoxInputImage.Size = new System.Drawing.Size(311, 20);
            this.txtBoxInputImage.TabIndex = 0;
            this.txtBoxInputImage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtBoxInputImage_MouseDown);
            // 
            // progressBarCapacity
            // 
            this.progressBarCapacity.Location = new System.Drawing.Point(9, 296);
            this.progressBarCapacity.Maximum = 0;
            this.progressBarCapacity.Name = "progressBarCapacity";
            this.progressBarCapacity.Size = new System.Drawing.Size(311, 23);
            this.progressBarCapacity.TabIndex = 10;
            // 
            // radioBtnText
            // 
            this.radioBtnText.AutoSize = true;
            this.radioBtnText.Enabled = false;
            this.radioBtnText.Location = new System.Drawing.Point(9, 97);
            this.radioBtnText.Name = "radioBtnText";
            this.radioBtnText.Size = new System.Drawing.Size(46, 17);
            this.radioBtnText.TabIndex = 4;
            this.radioBtnText.Text = "Text";
            this.radioBtnText.UseVisualStyleBackColor = true;
            // 
            // txtBoxFile
            // 
            this.txtBoxFile.Enabled = false;
            this.txtBoxFile.Location = new System.Drawing.Point(56, 71);
            this.txtBoxFile.Name = "txtBoxFile";
            this.txtBoxFile.ReadOnly = true;
            this.txtBoxFile.Size = new System.Drawing.Size(264, 20);
            this.txtBoxFile.TabIndex = 5;
            this.txtBoxFile.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtBoxFile_MouseDown);
            // 
            // txtBoxText
            // 
            this.txtBoxText.Location = new System.Drawing.Point(9, 120);
            this.txtBoxText.MaxLength = 0;
            this.txtBoxText.Multiline = true;
            this.txtBoxText.Name = "txtBoxText";
            this.txtBoxText.ReadOnly = true;
            this.txtBoxText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtBoxText.Size = new System.Drawing.Size(311, 170);
            this.txtBoxText.TabIndex = 6;
            this.txtBoxText.TextChanged += new System.EventHandler(this.txtBoxText_TextChanged);
            // 
            // txtBoxOutputFileName
            // 
            this.txtBoxOutputFileName.Enabled = false;
            this.txtBoxOutputFileName.Location = new System.Drawing.Point(113, 47);
            this.txtBoxOutputFileName.Name = "txtBoxOutputFileName";
            this.txtBoxOutputFileName.Size = new System.Drawing.Size(206, 20);
            this.txtBoxOutputFileName.TabIndex = 12;
            // 
            // lblOutputFile
            // 
            this.lblOutputFile.AutoSize = true;
            this.lblOutputFile.Location = new System.Drawing.Point(5, 50);
            this.lblOutputFile.Name = "lblOutputFile";
            this.lblOutputFile.Size = new System.Drawing.Size(87, 13);
            this.lblOutputFile.TabIndex = 11;
            this.lblOutputFile.Text = "Output file name:";
            // 
            // txtBoxOutputFolder
            // 
            this.txtBoxOutputFolder.Enabled = false;
            this.txtBoxOutputFolder.Location = new System.Drawing.Point(113, 19);
            this.txtBoxOutputFolder.Name = "txtBoxOutputFolder";
            this.txtBoxOutputFolder.ReadOnly = true;
            this.txtBoxOutputFolder.Size = new System.Drawing.Size(206, 20);
            this.txtBoxOutputFolder.TabIndex = 8;
            this.txtBoxOutputFolder.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtBoxOutputFolder_MouseDown);
            // 
            // lblOutputFolder
            // 
            this.lblOutputFolder.AutoSize = true;
            this.lblOutputFolder.Location = new System.Drawing.Point(5, 22);
            this.lblOutputFolder.Name = "lblOutputFolder";
            this.lblOutputFolder.Size = new System.Drawing.Size(87, 13);
            this.lblOutputFolder.TabIndex = 7;
            this.lblOutputFolder.Text = "Output file folder:";
            // 
            // radioBtnEncode
            // 
            this.radioBtnEncode.AutoSize = true;
            this.radioBtnEncode.Location = new System.Drawing.Point(12, 483);
            this.radioBtnEncode.Name = "radioBtnEncode";
            this.radioBtnEncode.Size = new System.Drawing.Size(62, 17);
            this.radioBtnEncode.TabIndex = 11;
            this.radioBtnEncode.Text = "Encode";
            this.radioBtnEncode.UseVisualStyleBackColor = true;
            // 
            // radioBtnDecode
            // 
            this.radioBtnDecode.AutoSize = true;
            this.radioBtnDecode.Location = new System.Drawing.Point(12, 506);
            this.radioBtnDecode.Name = "radioBtnDecode";
            this.radioBtnDecode.Size = new System.Drawing.Size(63, 17);
            this.radioBtnDecode.TabIndex = 12;
            this.radioBtnDecode.Text = "Decode";
            this.radioBtnDecode.UseVisualStyleBackColor = true;
            this.radioBtnDecode.CheckedChanged += new System.EventHandler(this.radioBtnDecode_CheckedChanged);
            // 
            // groupOutputData
            // 
            this.groupOutputData.Controls.Add(this.txtBoxOutputFolder);
            this.groupOutputData.Controls.Add(this.txtBoxOutputFileName);
            this.groupOutputData.Controls.Add(this.lblOutputFolder);
            this.groupOutputData.Controls.Add(this.lblOutputFile);
            this.groupOutputData.Location = new System.Drawing.Point(12, 344);
            this.groupOutputData.Name = "groupOutputData";
            this.groupOutputData.Size = new System.Drawing.Size(330, 77);
            this.groupOutputData.TabIndex = 13;
            this.groupOutputData.TabStop = false;
            this.groupOutputData.Text = "Output data";
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.BackColor = System.Drawing.SystemColors.Control;
            this.lblProgress.Location = new System.Drawing.Point(149, 497);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(0, 13);
            this.lblProgress.TabIndex = 14;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(354, 537);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.groupOutputData);
            this.Controls.Add(this.radioBtnDecode);
            this.Controls.Add(this.radioBtnEncode);
            this.Controls.Add(this.progressBarEncode);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.groupInputData);
            this.Controls.Add(this.groupProps);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "Steganography";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.Form1_HelpButtonClicked);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.groupProps.ResumeLayout(false);
            this.groupProps.PerformLayout();
            this.groupInputData.ResumeLayout(false);
            this.groupInputData.PerformLayout();
            this.groupOutputData.ResumeLayout(false);
            this.groupOutputData.PerformLayout();
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
        private System.Windows.Forms.GroupBox groupInputData;
        private System.Windows.Forms.ProgressBar progressBarCapacity;
        private System.Windows.Forms.TextBox txtBoxOutputFolder;
        private System.Windows.Forms.Label lblOutputFolder;
        private System.Windows.Forms.TextBox txtBoxText;
        private System.Windows.Forms.TextBox txtBoxFile;
        private System.Windows.Forms.RadioButton radioBtnText;
        private System.Windows.Forms.RadioButton radioBtnFile;
        private System.Windows.Forms.Label lblData;
        private System.Windows.Forms.Label lblInputImage;
        private System.Windows.Forms.TextBox txtBoxInputImage;
        private System.Windows.Forms.RadioButton radioBtnEncode;
        private System.Windows.Forms.RadioButton radioBtnDecode;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogOutput;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.TextBox txtBoxOutputFileName;
        private System.Windows.Forms.Label lblOutputFile;
        private System.Windows.Forms.GroupBox groupOutputData;
        private System.Windows.Forms.Label lblProgress;
    }
}

