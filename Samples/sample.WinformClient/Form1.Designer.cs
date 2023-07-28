namespace sample.WinformClient
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pic = new PictureBox();
            btnGenerate = new Button();
            ((System.ComponentModel.ISupportInitialize)pic).BeginInit();
            SuspendLayout();
            // 
            // pic
            // 
            pic.BackColor = SystemColors.AppWorkspace;
            pic.Location = new Point(12, 12);
            pic.Name = "pic";
            pic.Size = new Size(100, 50);
            pic.TabIndex = 0;
            pic.TabStop = false;
            // 
            // btnGenerate
            // 
            btnGenerate.Location = new Point(12, 68);
            btnGenerate.Name = "btnGenerate";
            btnGenerate.Size = new Size(100, 34);
            btnGenerate.TabIndex = 1;
            btnGenerate.Text = "Generate";
            btnGenerate.UseVisualStyleBackColor = true;
            btnGenerate.Click += btnGenerate_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(355, 120);
            Controls.Add(btnGenerate);
            Controls.Add(pic);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)pic).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pic;
        private Button btnGenerate;
    }
}