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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.picBoxSource = new System.Windows.Forms.PictureBox();
            this.picBoxResult = new System.Windows.Forms.PictureBox();
            this.groupFileSelection = new System.Windows.Forms.GroupBox();
            this.btnFileChoose = new System.Windows.Forms.Button();
            this.txtBoxFilePath = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnMake = new System.Windows.Forms.Button();
            this.txtBoxMessage = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxResult)).BeginInit();
            this.groupFileSelection.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Location = new System.Drawing.Point(12, 87);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.picBoxSource);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.picBoxResult);
            this.splitContainer1.Size = new System.Drawing.Size(1182, 542);
            this.splitContainer1.SplitterDistance = 591;
            this.splitContainer1.TabIndex = 0;
            // 
            // picBoxSource
            // 
            this.picBoxSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picBoxSource.Location = new System.Drawing.Point(0, 0);
            this.picBoxSource.Name = "picBoxSource";
            this.picBoxSource.Size = new System.Drawing.Size(591, 542);
            this.picBoxSource.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picBoxSource.TabIndex = 0;
            this.picBoxSource.TabStop = false;
            // 
            // picBoxResult
            // 
            this.picBoxResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picBoxResult.Location = new System.Drawing.Point(0, 0);
            this.picBoxResult.Name = "picBoxResult";
            this.picBoxResult.Size = new System.Drawing.Size(587, 542);
            this.picBoxResult.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picBoxResult.TabIndex = 1;
            this.picBoxResult.TabStop = false;
            // 
            // groupFileSelection
            // 
            this.groupFileSelection.Controls.Add(this.btnFileChoose);
            this.groupFileSelection.Controls.Add(this.txtBoxFilePath);
            this.groupFileSelection.Location = new System.Drawing.Point(12, 12);
            this.groupFileSelection.Name = "groupFileSelection";
            this.groupFileSelection.Size = new System.Drawing.Size(308, 57);
            this.groupFileSelection.TabIndex = 1;
            this.groupFileSelection.TabStop = false;
            this.groupFileSelection.Text = "Select File";
            // 
            // btnFileChoose
            // 
            this.btnFileChoose.Location = new System.Drawing.Point(232, 17);
            this.btnFileChoose.Name = "btnFileChoose";
            this.btnFileChoose.Size = new System.Drawing.Size(70, 23);
            this.btnFileChoose.TabIndex = 1;
            this.btnFileChoose.Text = "Load File";
            this.btnFileChoose.UseVisualStyleBackColor = true;
            this.btnFileChoose.Click += new System.EventHandler(this.btnFileChoose_Click);
            // 
            // txtBoxFilePath
            // 
            this.txtBoxFilePath.Location = new System.Drawing.Point(6, 19);
            this.txtBoxFilePath.Name = "txtBoxFilePath";
            this.txtBoxFilePath.Size = new System.Drawing.Size(220, 20);
            this.txtBoxFilePath.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnMake);
            this.groupBox1.Controls.Add(this.txtBoxMessage);
            this.groupBox1.Location = new System.Drawing.Point(326, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(308, 57);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Enter Message";
            // 
            // btnMake
            // 
            this.btnMake.Location = new System.Drawing.Point(232, 17);
            this.btnMake.Name = "btnMake";
            this.btnMake.Size = new System.Drawing.Size(70, 23);
            this.btnMake.TabIndex = 1;
            this.btnMake.Text = "Make";
            this.btnMake.UseVisualStyleBackColor = true;
            // 
            // txtBoxMessage
            // 
            this.txtBoxMessage.Location = new System.Drawing.Point(6, 19);
            this.txtBoxMessage.Name = "txtBoxMessage";
            this.txtBoxMessage.Size = new System.Drawing.Size(220, 20);
            this.txtBoxMessage.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1206, 641);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupFileSelection);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picBoxSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxResult)).EndInit();
            this.groupFileSelection.ResumeLayout(false);
            this.groupFileSelection.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PictureBox picBoxSource;
        private System.Windows.Forms.PictureBox picBoxResult;
        private System.Windows.Forms.GroupBox groupFileSelection;
        private System.Windows.Forms.Button btnFileChoose;
        private System.Windows.Forms.TextBox txtBoxFilePath;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnMake;
        private System.Windows.Forms.TextBox txtBoxMessage;
    }
}

