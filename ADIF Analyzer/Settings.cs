using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace ADIF_Analyzer
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
        }

        /*--------------------------------------------------------------------------
         * This form is being loaded.
         */
        private void Settings_Load(object sender, EventArgs e)
        {
        /*
         * If we don't have basic items, try to get them from the Trimode ini file.
         */
         if (Globals.strRMSCallsign == "")
            {
                BtnCancel.Enabled = false;
                InitializeFromTrimode();
            }
            txtCallsign.Text = Globals.strRMSCallsign;
            txtLatitude.Text = string.Format("{0:0.0000}", Globals.objStationLocation.dblLatitude);
            txtLongitude.Text = string.Format("{0:0.0000}", Globals.objStationLocation.dblLongitude);
            txtAdifPath1.Text = Globals.aryAdifPath[0].strPath;
            txtAdifPath2.Text = Globals.aryAdifPath[1].strPath;
            txtAdifPath3.Text = Globals.aryAdifPath[2].strPath;
            rboDistKm.Checked = !Globals.blnRangeMiles;
            rboDistMiles.Checked = Globals.blnRangeMiles;
            rboTimeUTC.Checked = Globals.blnTimeZoneUTC;
            rboTimeLocal.Checked = !Globals.blnTimeZoneUTC;
            chkLimitADIFStartDate.Checked = Globals.blnLimitADIFStartDate;
            chkLimitADIFEndDate.Checked = Globals.blnLimitADIFEndDate;
            dtpStartDate.Value = Globals.dttStartDate;
            dtpEndDate.Value = Globals.dttEndDate;
            dtpStartDate.Enabled = Globals.blnLimitADIFStartDate;
            dtpEndDate.Enabled = Globals.blnLimitADIFEndDate;
            chkAutoupdateTest.Checked = Globals.blnAutoupdateTest;
            txtMaxAdifTableEntries.Text = Globals.intMaxAdifTableEntries.ToString();
            txtMaxMapContacts.Text = Globals.intMaxMapContacts.ToString();
          return;
        }

        /*------------------------------------------------------------------------
         * Attempt to initialize items from the Trimode ini file.
         */
        private void InitializeFromTrimode()
        {
            if (!File.Exists("C:\\RMS\\RMS Trimode\\RMS Trimode.ini")) return;
            INIFile objTrimodeIni = new INIFile(true);
            if (!objTrimodeIni.Load("C:\\RMS\\RMS Trimode\\RMS Trimode.ini")) return;
            Globals.strRMSCallsign = objTrimodeIni.GetString("Registration", "Base Callsign", "");
            Globals.blnAutoupdateTest = objTrimodeIni.GetBoolean("Main", "Test Autoupdate", false);
            string strGridSquare = objTrimodeIni.GetString("Site Properties", "Grid Square", "");
            if (strGridSquare != "")
            {
                Globals.objStationLocation = Globals.GridSquareToDecimalDegrees(strGridSquare);
            }
            Globals.aryAdifPath[0].strPath = "C:\\RMS\\RMS Trimode\\Logs\\";
            /* See if ADIF file generation needs to be enabled in Trimode */
            bool blnTrimodeAdif = objTrimodeIni.GetBoolean("Site Properties", "ADIF Log 2", false);
            if (!blnTrimodeAdif)
            {
                blnTrimodeAdif = objTrimodeIni.GetBoolean("Site Properties", "ADIF Log", false);
                if (!blnTrimodeAdif)
                {
                    MessageBox.Show("ADIF log file generation needs to be enabled on Trimode\'s Site Settings screen", "ADIF File Generation Needed");
                }
            }
            /*
            * Finished
            */
            return;
        }

        /*----------------------------------------------------------------------
         * Browse for ADIF folder 1.
         */
        private void BtnAdifBrowse1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog objBrowse = new FolderBrowserDialog();
            objBrowse.Description = "Folder 1 with ADIF files";
            objBrowse.SelectedPath = Globals.aryAdifPath[0].strPath;
            if (objBrowse.ShowDialog() == DialogResult.OK)
            {
                /* Set the selected folder */
                txtAdifPath1.Text = objBrowse.SelectedPath;
            }
            return;
        }

        /*----------------------------------------------------------------------
        * Browse for ADIF folder 2.
        */
        private void BtnAdifBrowse2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog objBrowse = new FolderBrowserDialog();
            objBrowse.Description = "Folder 2 with ADIF files";
            objBrowse.SelectedPath = Globals.aryAdifPath[1].strPath;
            if (objBrowse.ShowDialog() == DialogResult.OK)
            {
                /* Set the selected folder */
                txtAdifPath2.Text = objBrowse.SelectedPath;
            }
            return;
        }

        /*----------------------------------------------------------------------
        * Browse for ADIF folder 3.
        */
        private void BtnAdifBrowse3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog objBrowse = new FolderBrowserDialog();
            objBrowse.Description = "Folder 3 with ADIF files";
            objBrowse.SelectedPath = Globals.aryAdifPath[2].strPath;
            if (objBrowse.ShowDialog() == DialogResult.OK)
            {
                /* Set the selected folder */
                txtAdifPath3.Text = objBrowse.SelectedPath;
            }
            return;
        }

        /*----------------------------------------------------------------------
         * Save new values.
         */
        private void btnSave_Click(object sender, EventArgs e)
        {

            Globals.aryAdifPath[0].strPath = txtAdifPath1.Text.Trim();
            Globals.aryAdifPath[1].strPath = txtAdifPath2.Text.Trim();
            Globals.aryAdifPath[2].strPath = txtAdifPath3.Text.Trim();
            int index = 0;
            foreach (AdifPath objPath in Globals.aryAdifPath)
            {
                Globals.objINIFile.WriteString("Main", "ADIF Path " + index.ToString(), objPath.strPath);
                index++;
            }
            Globals.strRMSCallsign = txtCallsign.Text.Trim();
            Globals.objINIFile.WriteString("Main", "RMS Callsign", Globals.strRMSCallsign);
            Globals.blnRangeMiles = rboDistMiles.Checked;
            Globals.blnTimeZoneUTC = rboTimeUTC.Checked;
            Globals.blnAutoupdateTest = chkAutoupdateTest.Checked;
            Globals.dttStartDate = dtpStartDate.Value;
            Globals.dttEndDate = dtpEndDate.Value;
            Globals.blnLimitADIFStartDate = chkLimitADIFStartDate.Checked;
            Globals.blnLimitADIFEndDate = chkLimitADIFEndDate.Checked;
            Globals.intMaxAdifTableEntries = Convert.ToInt32(txtMaxAdifTableEntries.Text.Trim());
            Globals.intMaxMapContacts = Convert.ToInt32(txtMaxMapContacts.Text.Trim());
            Globals.objINIFile.WriteBoolean("Main", "Limit ADIF Start Date", Globals.blnLimitADIFStartDate);
            Globals.objINIFile.WriteBoolean("Main", "Limit ADIF End Date", Globals.blnLimitADIFEndDate);
            Globals.objINIFile.WriteDateTime("Main", "ADIF Start Date", Globals.dttStartDate);
            Globals.objINIFile.WriteDateTime("Main", "ADIF End Date", Globals.dttEndDate);
            Globals.objINIFile.WriteBoolean("Main", "Test Autoupdate", Globals.blnAutoupdateTest);
            Globals.objINIFile.WriteBoolean("Main", "Distance miles", Globals.blnRangeMiles);
            Globals.objINIFile.WriteBoolean("Main", "Time zone UTC", Globals.blnTimeZoneUTC);
            Globals.objINIFile.WriteInteger("Main", "Max ADIF Table Entries", Globals.intMaxAdifTableEntries);
            Globals.objINIFile.WriteInteger("Main", "Max Map Contacts", Globals.intMaxMapContacts);
            string strValue = txtLatitude.Text.Trim();
            try
            {
                Globals.objStationLocation.dblLatitude = Globals.ConvertToDouble(strValue);
                Globals.objINIFile.WriteDouble("Main", "RMS Latitude", Globals.objStationLocation.dblLatitude);
            }
            catch
            {
                MessageBox.Show("Invalid latitude value", "Invalid latitude", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtLatitude.Focus();
                return;
            }
            strValue = txtLongitude.Text.Trim();
            try
            {
                Globals.objStationLocation.dblLongitude = Globals.ConvertToDouble(strValue);
                Globals.objINIFile.WriteDouble("Main", "RMS Longitude", Globals.objStationLocation.dblLongitude);
            }
            catch
            {
                MessageBox.Show("Invalid latitude value", "Invalid longitude", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtLongitude.Focus();
                return;
            }
            Globals.blnLimitADIFStartDate = chkLimitADIFStartDate.Checked;
            Globals.blnLimitADIFEndDate = chkLimitADIFEndDate.Checked;
            Globals.objINIFile.WriteBoolean("Main", "Limit ADIF Start Date", Globals.blnLimitADIFStartDate);
            Globals.objINIFile.WriteBoolean("Main", "Limit ADIF End Date", Globals.blnLimitADIFEndDate);
            /*
             * Flush the .ini file and exit.
             */
            Globals.objINIFile.Flush();
            this.DialogResult = DialogResult.OK;
            this.Close();
            return;
        }

        /*----------------------------------------------------------------------
         * Cancel without saving.
         */
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
            return;
        }

        /*------------------------------------------------------------------
         * Limit start date check changed.
         */
        private void chkLimitADIFStartDate_CheckedChanged(object sender, EventArgs e)
        {
            dtpStartDate.Enabled = chkLimitADIFStartDate.Checked;
            return;
        }

        /*----------------------------------------------------------------------------
         * Limit end date check changed.
         */
        private void chkLimitADIFEndDate_CheckedChanged(object sender, EventArgs e)
        {
            dtpEndDate.Enabled = chkLimitADIFEndDate.Checked;
            return;
        }

        /*--------------------------------------------------------------------------
         * Convert a grid square to latitude/longitude.
         */
        private void btnGridSquareToLatLong_Click(object sender, EventArgs e)
        {
            GridSquareToLatLong objDialog = new GridSquareToLatLong();
            DialogResult enmResult = objDialog.ShowDialog();
            if (enmResult == DialogResult.OK)
            {
                LatitudeLongitude objLatLong = objDialog.GetLatLong();
                txtLatitude.Text = objLatLong.dblLatitude.ToString("0.0000");
                txtLongitude.Text = objLatLong.dblLongitude.ToString("0.0000");
            }
            return;
        }
    }
}
