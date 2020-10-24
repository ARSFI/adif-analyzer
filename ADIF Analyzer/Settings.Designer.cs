namespace ADIF_Analyzer
{
    partial class Settings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
            this.txtAdifPath1 = new System.Windows.Forms.TextBox();
            this.BtnAdifBrowse1 = new System.Windows.Forms.Button();
            this.BtnAdifBrowse2 = new System.Windows.Forms.Button();
            this.txtAdifPath2 = new System.Windows.Forms.TextBox();
            this.BtnAdifBrowse3 = new System.Windows.Forms.Button();
            this.txtAdifPath3 = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.txtCallsign = new System.Windows.Forms.TextBox();
            this.txtLatitude = new System.Windows.Forms.TextBox();
            this.txtLongitude = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnGridSquareToLatLong = new System.Windows.Forms.Button();
            this.rboDistKm = new System.Windows.Forms.RadioButton();
            this.rboDistMiles = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.chkLimitADIFEndDate = new System.Windows.Forms.CheckBox();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.chkLimitADIFStartDate = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.txtMaxMapContacts = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtMaxAdifTableEntries = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.chkAutoupdateTest = new System.Windows.Forms.CheckBox();
            this.rboTimeLocal = new System.Windows.Forms.RadioButton();
            this.rboTimeUTC = new System.Windows.Forms.RadioButton();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtAdifPath1
            // 
            this.txtAdifPath1.Location = new System.Drawing.Point(25, 24);
            this.txtAdifPath1.Name = "txtAdifPath1";
            this.txtAdifPath1.Size = new System.Drawing.Size(322, 20);
            this.txtAdifPath1.TabIndex = 0;
            // 
            // BtnAdifBrowse1
            // 
            this.BtnAdifBrowse1.Location = new System.Drawing.Point(365, 24);
            this.BtnAdifBrowse1.Name = "BtnAdifBrowse1";
            this.BtnAdifBrowse1.Size = new System.Drawing.Size(55, 20);
            this.BtnAdifBrowse1.TabIndex = 1;
            this.BtnAdifBrowse1.Text = "Browse";
            this.BtnAdifBrowse1.UseVisualStyleBackColor = true;
            this.BtnAdifBrowse1.Click += new System.EventHandler(this.BtnAdifBrowse1_Click);
            // 
            // BtnAdifBrowse2
            // 
            this.BtnAdifBrowse2.Location = new System.Drawing.Point(365, 49);
            this.BtnAdifBrowse2.Name = "BtnAdifBrowse2";
            this.BtnAdifBrowse2.Size = new System.Drawing.Size(55, 20);
            this.BtnAdifBrowse2.TabIndex = 3;
            this.BtnAdifBrowse2.Text = "Browse";
            this.BtnAdifBrowse2.UseVisualStyleBackColor = true;
            this.BtnAdifBrowse2.Click += new System.EventHandler(this.BtnAdifBrowse2_Click);
            // 
            // txtAdifPath2
            // 
            this.txtAdifPath2.Location = new System.Drawing.Point(25, 49);
            this.txtAdifPath2.Name = "txtAdifPath2";
            this.txtAdifPath2.Size = new System.Drawing.Size(322, 20);
            this.txtAdifPath2.TabIndex = 2;
            // 
            // BtnAdifBrowse3
            // 
            this.BtnAdifBrowse3.Location = new System.Drawing.Point(365, 72);
            this.BtnAdifBrowse3.Name = "BtnAdifBrowse3";
            this.BtnAdifBrowse3.Size = new System.Drawing.Size(55, 20);
            this.BtnAdifBrowse3.TabIndex = 5;
            this.BtnAdifBrowse3.Text = "Browse";
            this.BtnAdifBrowse3.UseVisualStyleBackColor = true;
            this.BtnAdifBrowse3.Click += new System.EventHandler(this.BtnAdifBrowse3_Click);
            // 
            // txtAdifPath3
            // 
            this.txtAdifPath3.Location = new System.Drawing.Point(25, 72);
            this.txtAdifPath3.Name = "txtAdifPath3";
            this.txtAdifPath3.Size = new System.Drawing.Size(322, 20);
            this.txtAdifPath3.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.BtnAdifBrowse3);
            this.groupBox1.Controls.Add(this.txtAdifPath3);
            this.groupBox1.Controls.Add(this.BtnAdifBrowse2);
            this.groupBox1.Controls.Add(this.txtAdifPath2);
            this.groupBox1.Controls.Add(this.BtnAdifBrowse1);
            this.groupBox1.Controls.Add(this.txtAdifPath1);
            this.groupBox1.Location = new System.Drawing.Point(12, 120);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(454, 106);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Folders with ADIF files";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(138, 417);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(85, 27);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.Location = new System.Drawing.Point(265, 416);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(88, 27);
            this.BtnCancel.TabIndex = 8;
            this.BtnCancel.Text = "Cancel";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // txtCallsign
            // 
            this.txtCallsign.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCallsign.Location = new System.Drawing.Point(73, 23);
            this.txtCallsign.Name = "txtCallsign";
            this.txtCallsign.Size = new System.Drawing.Size(100, 20);
            this.txtCallsign.TabIndex = 9;
            // 
            // txtLatitude
            // 
            this.txtLatitude.Location = new System.Drawing.Point(73, 48);
            this.txtLatitude.Name = "txtLatitude";
            this.txtLatitude.Size = new System.Drawing.Size(100, 20);
            this.txtLatitude.TabIndex = 10;
            // 
            // txtLongitude
            // 
            this.txtLongitude.Location = new System.Drawing.Point(73, 72);
            this.txtLongitude.Name = "txtLongitude";
            this.txtLongitude.Size = new System.Drawing.Size(100, 20);
            this.txtLongitude.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(176, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(166, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "(decimal degrees, south negative)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(176, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(162, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "(decimal degrees, west negative)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Latitude:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 74);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Longitude:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(25, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Callsign:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnGridSquareToLatLong);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.txtLongitude);
            this.groupBox2.Controls.Add(this.txtLatitude);
            this.groupBox2.Controls.Add(this.txtCallsign);
            this.groupBox2.Location = new System.Drawing.Point(11, 10);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(365, 104);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "RMS Information";
            // 
            // btnGridSquareToLatLong
            // 
            this.btnGridSquareToLatLong.Location = new System.Drawing.Point(211, 22);
            this.btnGridSquareToLatLong.Name = "btnGridSquareToLatLong";
            this.btnGridSquareToLatLong.Size = new System.Drawing.Size(131, 24);
            this.btnGridSquareToLatLong.TabIndex = 17;
            this.btnGridSquareToLatLong.Text = "Grid square to Lat/Long";
            this.btnGridSquareToLatLong.UseVisualStyleBackColor = true;
            this.btnGridSquareToLatLong.Click += new System.EventHandler(this.btnGridSquareToLatLong_Click);
            // 
            // rboDistKm
            // 
            this.rboDistKm.AutoSize = true;
            this.rboDistKm.Location = new System.Drawing.Point(9, 19);
            this.rboDistKm.Name = "rboDistKm";
            this.rboDistKm.Size = new System.Drawing.Size(40, 17);
            this.rboDistKm.TabIndex = 18;
            this.rboDistKm.TabStop = true;
            this.rboDistKm.Text = "Km";
            this.rboDistKm.UseVisualStyleBackColor = true;
            // 
            // rboDistMiles
            // 
            this.rboDistMiles.AutoSize = true;
            this.rboDistMiles.Location = new System.Drawing.Point(9, 41);
            this.rboDistMiles.Name = "rboDistMiles";
            this.rboDistMiles.Size = new System.Drawing.Size(49, 17);
            this.rboDistMiles.TabIndex = 19;
            this.rboDistMiles.TabStop = true;
            this.rboDistMiles.Text = "Miles";
            this.rboDistMiles.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rboDistMiles);
            this.groupBox3.Controls.Add(this.rboDistKm);
            this.groupBox3.Location = new System.Drawing.Point(376, 235);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(90, 67);
            this.groupBox3.TabIndex = 20;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Distance Units";
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.CustomFormat = "yyyy-MM-dd";
            this.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEndDate.Location = new System.Drawing.Point(113, 47);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(97, 20);
            this.dtpEndDate.TabIndex = 31;
            // 
            // chkLimitADIFEndDate
            // 
            this.chkLimitADIFEndDate.AutoSize = true;
            this.chkLimitADIFEndDate.Location = new System.Drawing.Point(16, 47);
            this.chkLimitADIFEndDate.Name = "chkLimitADIFEndDate";
            this.chkLimitADIFEndDate.Size = new System.Drawing.Size(95, 17);
            this.chkLimitADIFEndDate.TabIndex = 30;
            this.chkLimitADIFEndDate.Text = "Limit end date:";
            this.chkLimitADIFEndDate.UseVisualStyleBackColor = true;
            this.chkLimitADIFEndDate.CheckedChanged += new System.EventHandler(this.chkLimitADIFEndDate_CheckedChanged);
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.CustomFormat = "yyyy-MM-dd";
            this.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStartDate.Location = new System.Drawing.Point(113, 23);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Size = new System.Drawing.Size(97, 20);
            this.dtpStartDate.TabIndex = 29;
            // 
            // chkLimitADIFStartDate
            // 
            this.chkLimitADIFStartDate.AutoSize = true;
            this.chkLimitADIFStartDate.Location = new System.Drawing.Point(16, 24);
            this.chkLimitADIFStartDate.Name = "chkLimitADIFStartDate";
            this.chkLimitADIFStartDate.Size = new System.Drawing.Size(97, 17);
            this.chkLimitADIFStartDate.TabIndex = 28;
            this.chkLimitADIFStartDate.Text = "Limit start date:";
            this.chkLimitADIFStartDate.UseVisualStyleBackColor = true;
            this.chkLimitADIFStartDate.CheckedChanged += new System.EventHandler(this.chkLimitADIFStartDate_CheckedChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.txtMaxMapContacts);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.txtMaxAdifTableEntries);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.dtpEndDate);
            this.groupBox4.Controls.Add(this.chkLimitADIFStartDate);
            this.groupBox4.Controls.Add(this.chkLimitADIFEndDate);
            this.groupBox4.Controls.Add(this.dtpStartDate);
            this.groupBox4.Location = new System.Drawing.Point(12, 235);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(310, 129);
            this.groupBox4.TabIndex = 32;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Optional ADIF Date Range";
            // 
            // txtMaxMapContacts
            // 
            this.txtMaxMapContacts.Location = new System.Drawing.Point(226, 100);
            this.txtMaxMapContacts.Name = "txtMaxMapContacts";
            this.txtMaxMapContacts.Size = new System.Drawing.Size(66, 20);
            this.txtMaxMapContacts.TabIndex = 35;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(74, 102);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(152, 13);
            this.label7.TabIndex = 34;
            this.label7.Text = "Max contacts to show on map:";
            // 
            // txtMaxAdifTableEntries
            // 
            this.txtMaxAdifTableEntries.Location = new System.Drawing.Point(226, 72);
            this.txtMaxAdifTableEntries.Name = "txtMaxAdifTableEntries";
            this.txtMaxAdifTableEntries.Size = new System.Drawing.Size(66, 20);
            this.txtMaxAdifTableEntries.TabIndex = 33;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 75);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(213, 13);
            this.label6.TabIndex = 32;
            this.label6.Text = "Max entries to display in ADIF Record table:";
            // 
            // chkAutoupdateTest
            // 
            this.chkAutoupdateTest.AutoSize = true;
            this.chkAutoupdateTest.Location = new System.Drawing.Point(28, 378);
            this.chkAutoupdateTest.Name = "chkAutoupdateTest";
            this.chkAutoupdateTest.Size = new System.Drawing.Size(311, 17);
            this.chkAutoupdateTest.TabIndex = 319;
            this.chkAutoupdateTest.TabStop = false;
            this.chkAutoupdateTest.Text = "Automaticaly install field-test (beta) versions of ADIF Analyzer";
            this.chkAutoupdateTest.UseVisualStyleBackColor = true;
            // 
            // rboTimeLocal
            // 
            this.rboTimeLocal.AutoSize = true;
            this.rboTimeLocal.Location = new System.Drawing.Point(10, 16);
            this.rboTimeLocal.Name = "rboTimeLocal";
            this.rboTimeLocal.Size = new System.Drawing.Size(51, 17);
            this.rboTimeLocal.TabIndex = 320;
            this.rboTimeLocal.TabStop = true;
            this.rboTimeLocal.Text = "Local";
            this.rboTimeLocal.UseVisualStyleBackColor = true;
            // 
            // rboTimeUTC
            // 
            this.rboTimeUTC.AutoSize = true;
            this.rboTimeUTC.Location = new System.Drawing.Point(10, 36);
            this.rboTimeUTC.Name = "rboTimeUTC";
            this.rboTimeUTC.Size = new System.Drawing.Size(47, 17);
            this.rboTimeUTC.TabIndex = 321;
            this.rboTimeUTC.TabStop = true;
            this.rboTimeUTC.Text = "UTC";
            this.rboTimeUTC.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.rboTimeUTC);
            this.groupBox5.Controls.Add(this.rboTimeLocal);
            this.groupBox5.Location = new System.Drawing.Point(375, 322);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(91, 65);
            this.groupBox5.TabIndex = 322;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Time Zone";
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(491, 464);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.chkAutoupdateTest);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox4);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Settings";
            this.Text = "ADIF Analyzer Settings";
            this.Load += new System.EventHandler(this.Settings_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtAdifPath1;
        private System.Windows.Forms.Button BtnAdifBrowse1;
        private System.Windows.Forms.Button BtnAdifBrowse2;
        private System.Windows.Forms.TextBox txtAdifPath2;
        private System.Windows.Forms.Button BtnAdifBrowse3;
        private System.Windows.Forms.TextBox txtAdifPath3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.TextBox txtCallsign;
        private System.Windows.Forms.TextBox txtLatitude;
        private System.Windows.Forms.TextBox txtLongitude;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rboDistKm;
        private System.Windows.Forms.RadioButton rboDistMiles;
        private System.Windows.Forms.GroupBox groupBox3;
        internal System.Windows.Forms.DateTimePicker dtpEndDate;
        internal System.Windows.Forms.CheckBox chkLimitADIFEndDate;
        internal System.Windows.Forms.DateTimePicker dtpStartDate;
        internal System.Windows.Forms.CheckBox chkLimitADIFStartDate;
        private System.Windows.Forms.GroupBox groupBox4;
        internal System.Windows.Forms.CheckBox chkAutoupdateTest;
        private System.Windows.Forms.TextBox txtMaxAdifTableEntries;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnGridSquareToLatLong;
        private System.Windows.Forms.TextBox txtMaxMapContacts;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.RadioButton rboTimeLocal;
        private System.Windows.Forms.RadioButton rboTimeUTC;
        private System.Windows.Forms.GroupBox groupBox5;
    }
}