namespace ADIF_Analyzer
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.chkShowBearings = new System.Windows.Forms.CheckBox();
            this.chkChartRange = new System.Windows.Forms.CheckBox();
            this.chkChartDayOfWeek = new System.Windows.Forms.CheckBox();
            this.chkChartHourOfDay = new System.Windows.Forms.CheckBox();
            this.chkChartMonth = new System.Windows.Forms.CheckBox();
            this.chkChartBand = new System.Windows.Forms.CheckBox();
            this.tmrStartup = new System.Windows.Forms.Timer(this.components);
            this.chkChartMode = new System.Windows.Forms.CheckBox();
            this.tmrTick = new System.Windows.Forms.Timer(this.components);
            this.chkTableRecords = new System.Windows.Forms.CheckBox();
            this.btnSettings = new System.Windows.Forms.Button();
            this.chkTableRecentCount = new System.Windows.Forms.CheckBox();
            this.chkTableMode = new System.Windows.Forms.CheckBox();
            this.chkTableBand = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkChartMap = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkTableCountry = new System.Windows.Forms.CheckBox();
            this.chkTableFrequencyMode = new System.Windows.Forms.CheckBox();
            this.chkChartFrequency = new System.Windows.Forms.CheckBox();
            this.chkTableFrequency = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkShowBearings
            // 
            this.chkShowBearings.AutoSize = true;
            this.chkShowBearings.Location = new System.Drawing.Point(47, 46);
            this.chkShowBearings.Name = "chkShowBearings";
            this.chkShowBearings.Size = new System.Drawing.Size(55, 17);
            this.chkShowBearings.TabIndex = 1;
            this.chkShowBearings.Text = "Radar";
            this.chkShowBearings.UseVisualStyleBackColor = true;
            this.chkShowBearings.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // chkChartRange
            // 
            this.chkChartRange.AutoSize = true;
            this.chkChartRange.Location = new System.Drawing.Point(47, 66);
            this.chkChartRange.Name = "chkChartRange";
            this.chkChartRange.Size = new System.Drawing.Size(58, 17);
            this.chkChartRange.TabIndex = 2;
            this.chkChartRange.Text = "Range";
            this.chkChartRange.UseVisualStyleBackColor = true;
            this.chkChartRange.CheckedChanged += new System.EventHandler(this.chkChartRange_CheckedChanged);
            // 
            // chkChartDayOfWeek
            // 
            this.chkChartDayOfWeek.AutoSize = true;
            this.chkChartDayOfWeek.Location = new System.Drawing.Point(47, 85);
            this.chkChartDayOfWeek.Name = "chkChartDayOfWeek";
            this.chkChartDayOfWeek.Size = new System.Drawing.Size(89, 17);
            this.chkChartDayOfWeek.TabIndex = 3;
            this.chkChartDayOfWeek.Text = "Day of Week";
            this.chkChartDayOfWeek.UseVisualStyleBackColor = true;
            this.chkChartDayOfWeek.CheckedChanged += new System.EventHandler(this.chkChartDayOfWeek_CheckedChanged);
            // 
            // chkChartHourOfDay
            // 
            this.chkChartHourOfDay.AutoSize = true;
            this.chkChartHourOfDay.Location = new System.Drawing.Point(47, 104);
            this.chkChartHourOfDay.Name = "chkChartHourOfDay";
            this.chkChartHourOfDay.Size = new System.Drawing.Size(83, 17);
            this.chkChartHourOfDay.TabIndex = 4;
            this.chkChartHourOfDay.Text = "Hour of Day";
            this.chkChartHourOfDay.UseVisualStyleBackColor = true;
            this.chkChartHourOfDay.CheckedChanged += new System.EventHandler(this.chkChartHourOfDay_CheckedChanged);
            // 
            // chkChartMonth
            // 
            this.chkChartMonth.AutoSize = true;
            this.chkChartMonth.Location = new System.Drawing.Point(47, 124);
            this.chkChartMonth.Name = "chkChartMonth";
            this.chkChartMonth.Size = new System.Drawing.Size(56, 17);
            this.chkChartMonth.TabIndex = 5;
            this.chkChartMonth.Text = "Month";
            this.chkChartMonth.UseVisualStyleBackColor = true;
            this.chkChartMonth.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged_1);
            // 
            // chkChartBand
            // 
            this.chkChartBand.AutoSize = true;
            this.chkChartBand.Location = new System.Drawing.Point(159, 46);
            this.chkChartBand.Name = "chkChartBand";
            this.chkChartBand.Size = new System.Drawing.Size(51, 17);
            this.chkChartBand.TabIndex = 6;
            this.chkChartBand.Text = "Band";
            this.chkChartBand.UseVisualStyleBackColor = true;
            this.chkChartBand.CheckedChanged += new System.EventHandler(this.chkChartBand_CheckedChanged);
            // 
            // tmrStartup
            // 
            this.tmrStartup.Tick += new System.EventHandler(this.tmrStartup_Tick);
            // 
            // chkChartMode
            // 
            this.chkChartMode.AutoSize = true;
            this.chkChartMode.Location = new System.Drawing.Point(159, 66);
            this.chkChartMode.Name = "chkChartMode";
            this.chkChartMode.Size = new System.Drawing.Size(53, 17);
            this.chkChartMode.TabIndex = 7;
            this.chkChartMode.Text = "Mode";
            this.chkChartMode.UseVisualStyleBackColor = true;
            this.chkChartMode.CheckedChanged += new System.EventHandler(this.chkChartMode_CheckedChanged);
            // 
            // tmrTick
            // 
            this.tmrTick.Tick += new System.EventHandler(this.tmrTick_Tick);
            // 
            // chkTableRecords
            // 
            this.chkTableRecords.AutoSize = true;
            this.chkTableRecords.Location = new System.Drawing.Point(21, 123);
            this.chkTableRecords.Name = "chkTableRecords";
            this.chkTableRecords.Size = new System.Drawing.Size(93, 17);
            this.chkTableRecords.TabIndex = 9;
            this.chkTableRecords.Text = "ADIF Records";
            this.chkTableRecords.UseVisualStyleBackColor = true;
            this.chkTableRecords.CheckedChanged += new System.EventHandler(this.chkTableRecords_CheckedChanged);
            // 
            // btnSettings
            // 
            this.btnSettings.Location = new System.Drawing.Point(225, 209);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(75, 30);
            this.btnSettings.TabIndex = 10;
            this.btnSettings.Text = "Settings";
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // chkTableRecentCount
            // 
            this.chkTableRecentCount.AutoSize = true;
            this.chkTableRecentCount.Location = new System.Drawing.Point(21, 25);
            this.chkTableRecentCount.Name = "chkTableRecentCount";
            this.chkTableRecentCount.Size = new System.Drawing.Size(152, 17);
            this.chkTableRecentCount.TabIndex = 11;
            this.chkTableRecentCount.Text = "Recent connection counts";
            this.chkTableRecentCount.UseVisualStyleBackColor = true;
            this.chkTableRecentCount.CheckedChanged += new System.EventHandler(this.chkTableRecentCount_CheckedChanged);
            // 
            // chkTableMode
            // 
            this.chkTableMode.AutoSize = true;
            this.chkTableMode.Location = new System.Drawing.Point(21, 45);
            this.chkTableMode.Name = "chkTableMode";
            this.chkTableMode.Size = new System.Drawing.Size(128, 17);
            this.chkTableMode.TabIndex = 12;
            this.chkTableMode.Text = "Connections by mode";
            this.chkTableMode.UseVisualStyleBackColor = true;
            this.chkTableMode.CheckedChanged += new System.EventHandler(this.chkTableMode_CheckedChanged);
            // 
            // chkTableBand
            // 
            this.chkTableBand.AutoSize = true;
            this.chkTableBand.Location = new System.Drawing.Point(21, 64);
            this.chkTableBand.Name = "chkTableBand";
            this.chkTableBand.Size = new System.Drawing.Size(126, 17);
            this.chkTableBand.TabIndex = 13;
            this.chkTableBand.Text = "Connections by band";
            this.chkTableBand.UseVisualStyleBackColor = true;
            this.chkTableBand.CheckedChanged += new System.EventHandler(this.chkTableBand_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkChartFrequency);
            this.groupBox1.Controls.Add(this.chkChartMap);
            this.groupBox1.Location = new System.Drawing.Point(21, 21);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(227, 170);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Charts";
            // 
            // chkChartMap
            // 
            this.chkChartMap.AutoSize = true;
            this.chkChartMap.Location = new System.Drawing.Point(26, 123);
            this.chkChartMap.Name = "chkChartMap";
            this.chkChartMap.Size = new System.Drawing.Size(47, 17);
            this.chkChartMap.TabIndex = 16;
            this.chkChartMap.Text = "Map";
            this.chkChartMap.UseVisualStyleBackColor = true;
            this.chkChartMap.CheckedChanged += new System.EventHandler(this.chkChartMap_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkTableFrequency);
            this.groupBox2.Controls.Add(this.chkTableCountry);
            this.groupBox2.Controls.Add(this.chkTableFrequencyMode);
            this.groupBox2.Controls.Add(this.chkTableBand);
            this.groupBox2.Controls.Add(this.chkTableMode);
            this.groupBox2.Controls.Add(this.chkTableRecentCount);
            this.groupBox2.Controls.Add(this.chkTableRecords);
            this.groupBox2.Location = new System.Drawing.Point(282, 21);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(199, 170);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Tables";
            // 
            // chkTableCountry
            // 
            this.chkTableCountry.AutoSize = true;
            this.chkTableCountry.Location = new System.Drawing.Point(21, 143);
            this.chkTableCountry.Name = "chkTableCountry";
            this.chkTableCountry.Size = new System.Drawing.Size(70, 17);
            this.chkTableCountry.TabIndex = 15;
            this.chkTableCountry.Text = "Countries";
            this.chkTableCountry.UseVisualStyleBackColor = true;
            this.chkTableCountry.CheckedChanged += new System.EventHandler(this.chkTableCountry_CheckedChanged);
            // 
            // chkTableFrequencyMode
            // 
            this.chkTableFrequencyMode.AutoSize = true;
            this.chkTableFrequencyMode.Location = new System.Drawing.Point(21, 103);
            this.chkTableFrequencyMode.Name = "chkTableFrequencyMode";
            this.chkTableFrequencyMode.Size = new System.Drawing.Size(136, 17);
            this.chkTableFrequencyMode.TabIndex = 14;
            this.chkTableFrequencyMode.Text = "Frequency-Mode matrix";
            this.chkTableFrequencyMode.UseVisualStyleBackColor = true;
            this.chkTableFrequencyMode.CheckedChanged += new System.EventHandler(this.chkTableFrequencyMode_CheckedChanged);
            // 
            // chkChartFrequency
            // 
            this.chkChartFrequency.AutoSize = true;
            this.chkChartFrequency.Location = new System.Drawing.Point(138, 64);
            this.chkChartFrequency.Name = "chkChartFrequency";
            this.chkChartFrequency.Size = new System.Drawing.Size(76, 17);
            this.chkChartFrequency.TabIndex = 17;
            this.chkChartFrequency.Text = "Frequency";
            this.chkChartFrequency.UseVisualStyleBackColor = true;
            this.chkChartFrequency.CheckedChanged += new System.EventHandler(this.chkChartFrequency_CheckedChanged);
            // 
            // chkTableFrequency
            // 
            this.chkTableFrequency.AutoSize = true;
            this.chkTableFrequency.Location = new System.Drawing.Point(21, 83);
            this.chkTableFrequency.Name = "chkTableFrequency";
            this.chkTableFrequency.Size = new System.Drawing.Size(149, 17);
            this.chkTableFrequency.TabIndex = 16;
            this.chkTableFrequency.Text = "Connections by frequency";
            this.chkTableFrequency.UseVisualStyleBackColor = true;
            this.chkTableFrequency.CheckedChanged += new System.EventHandler(this.chkTableFrequency_CheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(505, 256);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnSettings);
            this.Controls.Add(this.chkChartMode);
            this.Controls.Add(this.chkChartBand);
            this.Controls.Add(this.chkChartMonth);
            this.Controls.Add(this.chkChartHourOfDay);
            this.Controls.Add(this.chkChartDayOfWeek);
            this.Controls.Add(this.chkChartRange);
            this.Controls.Add(this.chkShowBearings);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Winlink ADIF Analyzer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckBox chkShowBearings;
        private System.Windows.Forms.CheckBox chkChartRange;
        private System.Windows.Forms.CheckBox chkChartDayOfWeek;
        private System.Windows.Forms.CheckBox chkChartHourOfDay;
        private System.Windows.Forms.CheckBox chkChartMonth;
        private System.Windows.Forms.CheckBox chkChartBand;
        private System.Windows.Forms.Timer tmrStartup;
        private System.Windows.Forms.CheckBox chkChartMode;
        private System.Windows.Forms.Timer tmrTick;
        private System.Windows.Forms.CheckBox chkTableRecords;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.CheckBox chkTableRecentCount;
        private System.Windows.Forms.CheckBox chkTableMode;
        private System.Windows.Forms.CheckBox chkTableBand;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkTableFrequencyMode;
        private System.Windows.Forms.CheckBox chkTableCountry;
        private System.Windows.Forms.CheckBox chkChartMap;
        private System.Windows.Forms.CheckBox chkChartFrequency;
        private System.Windows.Forms.CheckBox chkTableFrequency;
    }
}

