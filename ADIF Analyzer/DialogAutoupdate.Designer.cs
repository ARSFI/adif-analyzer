namespace ADIF_Analyzer
{
    partial class DialogAutoupdate
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
            this.components = new System.ComponentModel.Container();
            this.Label4 = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.lblTR = new System.Windows.Forms.Label();
            this.lblNV = new System.Windows.Forms.Label();
            this.lblCV = new System.Windows.Forms.Label();
            this.BtnUpdate = new System.Windows.Forms.Button();
            this.btnAbort = new System.Windows.Forms.Button();
            this.Label1 = new System.Windows.Forms.Label();
            this.tmrTimeout = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Location = new System.Drawing.Point(105, 155);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(86, 13);
            this.Label4.TabIndex = 17;
            this.Label4.Text = "Time Remaining:";
            this.Label4.Visible = false;
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(105, 136);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(70, 13);
            this.Label3.TabIndex = 16;
            this.Label3.Text = "New Version:";
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(105, 118);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(85, 13);
            this.Label2.TabIndex = 15;
            this.Label2.Text = "Current Version: ";
            // 
            // lblTR
            // 
            this.lblTR.AutoSize = true;
            this.lblTR.Location = new System.Drawing.Point(196, 157);
            this.lblTR.Name = "lblTR";
            this.lblTR.Size = new System.Drawing.Size(16, 13);
            this.lblTR.TabIndex = 14;
            this.lblTR.Text = "---";
            this.lblTR.Visible = false;
            // 
            // lblNV
            // 
            this.lblNV.AutoSize = true;
            this.lblNV.Location = new System.Drawing.Point(196, 138);
            this.lblNV.Name = "lblNV";
            this.lblNV.Size = new System.Drawing.Size(16, 13);
            this.lblNV.TabIndex = 13;
            this.lblNV.Text = "---";
            // 
            // lblCV
            // 
            this.lblCV.AutoSize = true;
            this.lblCV.Location = new System.Drawing.Point(196, 118);
            this.lblCV.Name = "lblCV";
            this.lblCV.Size = new System.Drawing.Size(16, 13);
            this.lblCV.TabIndex = 12;
            this.lblCV.Text = "---";
            // 
            // BtnUpdate
            // 
            this.BtnUpdate.Location = new System.Drawing.Point(26, 198);
            this.BtnUpdate.Name = "BtnUpdate";
            this.BtnUpdate.Size = new System.Drawing.Size(113, 28);
            this.BtnUpdate.TabIndex = 11;
            this.BtnUpdate.Text = "Update Now\r\n";
            this.BtnUpdate.UseVisualStyleBackColor = true;
            this.BtnUpdate.Click += new System.EventHandler(this.BtnUpdate_Click);
            // 
            // btnAbort
            // 
            this.btnAbort.Location = new System.Drawing.Point(220, 198);
            this.btnAbort.Name = "btnAbort";
            this.btnAbort.Size = new System.Drawing.Size(113, 28);
            this.btnAbort.TabIndex = 12;
            this.btnAbort.Text = "Remind Me Later";
            this.btnAbort.UseVisualStyleBackColor = true;
            this.btnAbort.Click += new System.EventHandler(this.btnAbort_Click);
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.Location = new System.Drawing.Point(23, 24);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(310, 64);
            this.Label1.TabIndex = 9;
            this.Label1.Text = "A new version of this program has been released.  \r\nClick \'Update Now\' to install" +
    " the update and restart.\r\nClick \'Remind Me Later\' if you do not wish to update\r\n" +
    "at this time.";
            this.Label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // tmrTimeout
            // 
            this.tmrTimeout.Interval = 1000;
            this.tmrTimeout.Tick += new System.EventHandler(this.tmrTimeout_Tick);
            // 
            // DialogAutoupdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(368, 259);
            this.Controls.Add(this.Label4);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.lblTR);
            this.Controls.Add(this.lblNV);
            this.Controls.Add(this.lblCV);
            this.Controls.Add(this.BtnUpdate);
            this.Controls.Add(this.btnAbort);
            this.Controls.Add(this.Label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DialogAutoupdate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Update Available";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DialogAutoupdate_FormClosing);
            this.Load += new System.EventHandler(this.DialogAutoupdate_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.Label lblTR;
        internal System.Windows.Forms.Label lblNV;
        internal System.Windows.Forms.Label lblCV;
        internal System.Windows.Forms.Button BtnUpdate;
        internal System.Windows.Forms.Button btnAbort;
        internal System.Windows.Forms.Label Label1;
        private System.Windows.Forms.Timer tmrTimeout;
    }
}