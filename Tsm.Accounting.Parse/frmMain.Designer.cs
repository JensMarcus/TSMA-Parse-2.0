namespace Tsm.Accounting.Parse
{
    partial class frmMain
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
            this.lblInputFile = new System.Windows.Forms.Label();
            this.txtInputFile = new System.Windows.Forms.TextBox();
            this.lblOutputFile = new System.Windows.Forms.Label();
            this.txtOutputFile = new System.Windows.Forms.TextBox();
            this.cmdParse = new System.Windows.Forms.Button();
            this.cmdInputFile = new System.Windows.Forms.Button();
            this.cmdOutputFile = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblInputFile
            // 
            this.lblInputFile.AutoSize = true;
            this.lblInputFile.Location = new System.Drawing.Point(12, 15);
            this.lblInputFile.Name = "lblInputFile";
            this.lblInputFile.Size = new System.Drawing.Size(109, 13);
            this.lblInputFile.TabIndex = 0;
            this.lblInputFile.Text = "TSM Accounting File:";
            // 
            // txtInputFile
            // 
            this.txtInputFile.Location = new System.Drawing.Point(124, 12);
            this.txtInputFile.Name = "txtInputFile";
            this.txtInputFile.Size = new System.Drawing.Size(309, 20);
            this.txtInputFile.TabIndex = 1;
            // 
            // lblOutputFile
            // 
            this.lblOutputFile.AutoSize = true;
            this.lblOutputFile.Location = new System.Drawing.Point(12, 41);
            this.lblOutputFile.Name = "lblOutputFile";
            this.lblOutputFile.Size = new System.Drawing.Size(61, 13);
            this.lblOutputFile.TabIndex = 3;
            this.lblOutputFile.Text = "Output File:";
            // 
            // txtOutputFile
            // 
            this.txtOutputFile.Location = new System.Drawing.Point(124, 38);
            this.txtOutputFile.Name = "txtOutputFile";
            this.txtOutputFile.Size = new System.Drawing.Size(309, 20);
            this.txtOutputFile.TabIndex = 4;
            // 
            // cmdParse
            // 
            this.cmdParse.Location = new System.Drawing.Point(229, 82);
            this.cmdParse.Name = "cmdParse";
            this.cmdParse.Size = new System.Drawing.Size(75, 23);
            this.cmdParse.TabIndex = 6;
            this.cmdParse.Text = "Parse";
            this.cmdParse.UseVisualStyleBackColor = true;
            this.cmdParse.Click += new System.EventHandler(this.cmdParse_Click);
            // 
            // cmdInputFile
            // 
            this.cmdInputFile.Location = new System.Drawing.Point(439, 10);
            this.cmdInputFile.Name = "cmdInputFile";
            this.cmdInputFile.Size = new System.Drawing.Size(75, 23);
            this.cmdInputFile.TabIndex = 2;
            this.cmdInputFile.Text = "Browse..";
            this.cmdInputFile.UseVisualStyleBackColor = true;
            this.cmdInputFile.Click += new System.EventHandler(this.cmdInputFile_Click);
            // 
            // cmdOutputFile
            // 
            this.cmdOutputFile.Location = new System.Drawing.Point(439, 36);
            this.cmdOutputFile.Name = "cmdOutputFile";
            this.cmdOutputFile.Size = new System.Drawing.Size(75, 23);
            this.cmdOutputFile.TabIndex = 5;
            this.cmdOutputFile.Text = "Browse..";
            this.cmdOutputFile.UseVisualStyleBackColor = true;
            this.cmdOutputFile.Click += new System.EventHandler(this.cmdOutputFile_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(533, 125);
            this.Controls.Add(this.cmdOutputFile);
            this.Controls.Add(this.cmdInputFile);
            this.Controls.Add(this.cmdParse);
            this.Controls.Add(this.txtOutputFile);
            this.Controls.Add(this.lblOutputFile);
            this.Controls.Add(this.txtInputFile);
            this.Controls.Add(this.lblInputFile);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.Text = "TSM Parse";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblInputFile;
        private System.Windows.Forms.TextBox txtInputFile;
        private System.Windows.Forms.Label lblOutputFile;
        private System.Windows.Forms.TextBox txtOutputFile;
        private System.Windows.Forms.Button cmdParse;
        private System.Windows.Forms.Button cmdInputFile;
        private System.Windows.Forms.Button cmdOutputFile;
    }
}