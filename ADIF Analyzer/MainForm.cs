using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ADIF_Analyzer
{
    public partial class MainForm : Form
    {
        /* Chart objects */
        private ChartBearing objChartBearing = null;
        private ChartRange objChartRange = null;
        private ChartDayOfWeek objChartDayOfWeek = null;
        private ChartHourOfDay objChartHourOfDay = null;
        private ChartMonth objChartMonth = null;
        private ChartBand objChartBand = null;
        private ChartFrequency objChartFrequency = null;
        private ChartMode objChartMode = null;
        private ChartMap objChartMap = null;
        private TableRecords objTableRecords = null;
        private TableCountry objTableCountry = null;
        private TableRecentCount objTableRecentCount = null;
        private TableMode objTableMode = null;
        private TableBand objTableBand = null;
        private TableFrequency objTableFrequency = null;
        private TableFrequencyMode objTableFrequencyMode = null;
        private int intCurrentMinute = -1;
        private int intCurrentDay = -1;
        Thread thrReadAdif = null;
        List<string> lstWatchedFolders = new List<string>();

        public MainForm()
        {
            InitializeComponent();
            Globals.objMain = this;
        }

        /*--------------------------------------------------------------
         * The main form has been loaded.
         */
        private void MainForm_Load(object sender, EventArgs e)
        {
            /*
             * Figure out where to store the .ini file and open it.
             */
            if (Application.StartupPath.IndexOf("PhilTFSSource") == -1)
            {
                Globals.strExecutionDirectory = Application.StartupPath + "\\";
            }
            else
            {
                Globals.strExecutionDirectory = "C:\\RMS\\ADIF Analyzer\\";
                if (!Directory.Exists(Globals.strExecutionDirectory))
                {
                    Directory.CreateDirectory(Globals.strExecutionDirectory);
                }
            }
            Globals.strLogsDirectory = Globals.strExecutionDirectory + "Logs\\";
            if (!Directory.Exists(Globals.strLogsDirectory))
            {
                Directory.CreateDirectory(Globals.strLogsDirectory);
            }
            Globals.objINIFile = new INIFile();
            Globals.objINIFile.Load();
            /*
             * Set our screen position.
             */
            Globals.RestoreFormPosition("Main", this, 500, 500, true);
            this.Text = "Winlink ADIF Analyzer version " + Application.ProductVersion;
            /*
             * Trigger the startup timer.
             */
            tmrStartup.Enabled = true;
            return;
        }

        /*------------------------------------------------------------------
         * The startup timer ticked.
         */
        private void tmrStartup_Tick(object sender, EventArgs e)
        {
            tmrStartup.Enabled = false;
            this.Cursor = Cursors.WaitCursor;
            Globals.objAdif = new AdifProcess();
            /*
             * Initialize values from the .ini file.
             */
            Globals.strRMSCallsign = Globals.objINIFile.GetString("Main", "RMS Callsign", "");
            /*
             * Setup ADIF folders and file watching on those folders.
             */
            bool blnHaveAdifPath = false;
             for (int index = 0; index < Globals.aryAdifPath.Length; index++)
             {
                Globals.aryAdifPath[index].strPath = Globals.objINIFile.GetString("Main", "ADIF Path " + index.ToString(), "");
                if (Globals.aryAdifPath[index].strPath != "") blnHaveAdifPath = true;
             }
            SetFileWatchers();
            /*
             * Initialize other values.
             */
            Globals.objStationLocation.dblLatitude = Globals.objINIFile.GetDouble("Main", "RMS Latitude", 0.0);
            Globals.objStationLocation.dblLongitude = Globals.objINIFile.GetDouble("Main", "RMS Longitude", 0.0);
            Globals.blnRangeMiles = Globals.objINIFile.GetBoolean("Main", "Distance miles", false);
            Globals.blnTimeZoneUTC = Globals.objINIFile.GetBoolean("Main", "Time zone UTC", false);
            Globals.blnAutoupdateTest = Globals.objINIFile.GetBoolean("Main", "Test Autoupdate", false);
            Globals.blnAutoupdateForce = Globals.objINIFile.GetBoolean("Main", "Force Autoupdate", false);
            Globals.blnLimitADIFStartDate = Globals.objINIFile.GetBoolean("Main", "Limit ADIF Start Date", false);
            Globals.blnLimitADIFEndDate = Globals.objINIFile.GetBoolean("Main", "Limit ADIF End Date", false);
            Globals.dttStartDate = Globals.objINIFile.GetDateTime("Main", "ADIF Start Date", DateTime.Now.AddYears(-1));
            Globals.dttEndDate = Globals.objINIFile.GetDateTime("Main", "ADIF End Date", DateTime.Now);
            Globals.intMaxAdifTableEntries = Globals.objINIFile.GetInteger("Main", "Max ADIF Table Entries", 100);
            Globals.intMaxMapContacts = Globals.objINIFile.GetInteger("Main", "Max Map Contacts", 500);
            /*
             * If we are missing settings, display the Settings screen.
             */
            if (Globals.strRMSCallsign == "" || !blnHaveAdifPath)
            {
                Settings objSettings = new Settings();
                objSettings.ShowDialog();
                if (Globals.strRMSCallsign == "")
                {
                    this.Close();
                    return;
                }
            }
            /*
             * Log that we're starting.
             */
            Globals.LogWrite("----  ADIF Analyzer version " + Application.ProductVersion + " starting for " + Globals.strRMSCallsign + "  ----");
            /*
             * Read the Country Codes file.
             */
            string strError = Globals.objAdif.ReadCountryCodes();
            /*
             * Start a thread to read ADIF records.
             */
            Globals.LogWrite("Start scan of all ADIF folders after program startup");
            ScheduleFullScan(1000);
            thrReadAdif = new Thread(ReadAdifThread);
            thrReadAdif.Start();
            /*
             * Initialize chart displays.
             */
            chkShowBearings.Checked = Globals.objINIFile.GetBoolean("Main", "Chart Bearings", false);
            chkChartRange.Checked = Globals.objINIFile.GetBoolean("Main", "Chart Range", false);
            chkChartDayOfWeek.Checked = Globals.objINIFile.GetBoolean("Main", "Chart Day of Week", false);
            chkChartHourOfDay.Checked = Globals.objINIFile.GetBoolean("Main", "Chart Hour of Day", false);
            chkChartMonth.Checked = Globals.objINIFile.GetBoolean("Main", "Chart Month", false);
            chkChartBand.Checked = Globals.objINIFile.GetBoolean("Main", "Chart Band", false);
            chkChartFrequency.Checked = Globals.objINIFile.GetBoolean("Main", "Chart Frequency", false);
            chkChartMode.Checked = Globals.objINIFile.GetBoolean("Main", "Chart Mode", false);
            chkChartMap.Checked = Globals.objINIFile.GetBoolean("Main", "Chart Map", false);
            chkTableRecords.Checked = Globals.objINIFile.GetBoolean("Main", "Table Records", false);
            chkTableRecentCount.Checked = Globals.objINIFile.GetBoolean("Main", "Table Recent Count", false);
            chkTableMode.Checked = Globals.objINIFile.GetBoolean("Main", "Table Mode", false);
            chkTableBand.Checked = Globals.objINIFile.GetBoolean("Main", "Table Band", false);
            chkTableFrequency.Checked = Globals.objINIFile.GetBoolean("Main", "Table Frequency", false);
            chkTableFrequencyMode.Checked = Globals.objINIFile.GetBoolean("Main", "Table Frequency-Mode", false);
            chkTableCountry.Checked = Globals.objINIFile.GetBoolean("Main", "Table Country", false);
            /*
             * Start the main timer.
             */
            tmrTick.Enabled = true;
            /*
             * Finished
             */
            this.Cursor = Cursors.Default;
            return;
        }

        /*------------------------------------------------------------------------
         * Set file watchers for each ADIF folder.
         */
        private void SetFileWatchers()
        {
            int index = 0;
            foreach (AdifPath objPath in Globals.aryAdifPath)
            {
                objPath.strPath = Globals.objINIFile.GetString("Main", "ADIF Path " + index.ToString(), "");
                if (objPath.strPath != "")
                {
                    /* If we aren't already monitoring this folder, start monitoring */
                    if (!lstWatchedFolders.Contains(objPath.strPath))
                    {
                        lstWatchedFolders.Add(objPath.strPath);
                        objPath.objFileWatcher = CreateFileWatcher(objPath.strPath);
                    }
                }
                index++;
            }
            return;
        }

        /*-----------------------------------------------------------------------
         * The main timer ticked.
         */
        private void tmrTick_Tick(object sender, EventArgs e)
        {
            tmrTick.Enabled = false;
            /*
             * See if it's time to once-a-minute processing.
             */
            if (DateTime.Now.Minute != intCurrentMinute)
            {
                intCurrentMinute = DateTime.Now.Minute;
                DoMinuteProcessing();
            }
            /*
             * See if we need to update the charts.
             */
            if (Globals.blnNeedChartUpdate)
            {
                Globals.LogWrite("Update all charts");
                Globals.blnNeedChartUpdate = false;
                UpdateAllCharts();
            }
            tmrTick.Enabled = true;
            return;
        }

        /*--------------------------------------------------------------------------------
         * Processing done once a minute.
         */
        private void DoMinuteProcessing()
        {

            /*
             * If we just started a new day, refresh all charts.
             */
            if (DateTime.Now.Day != intCurrentDay)
            {
                /*
                 * Do daily processing.
                 */
                intCurrentDay = DateTime.Now.Day;
                Globals.LogWrite("Entering new day.  Request update of all charts.");
                Globals.blnNeedChartUpdate = true;
            }
            return;
        }

        /*----------------------------------------------------------------------------
         * Update all charts and tables.
         */
        private void UpdateAllCharts()
        {
            if (objChartRange != null) objChartRange.UpdateChart();
            if (objChartBearing != null) objChartBearing.UpdateChart();
            if (objChartDayOfWeek != null) objChartDayOfWeek.UpdateChart();
            if (objChartHourOfDay != null) objChartHourOfDay.UpdateChart();
            if (objChartMonth != null) objChartMonth.UpdateChart();
            if (objChartBand != null) objChartBand.UpdateChart();
            if (objChartFrequency != null) objChartFrequency.UpdateChart();
            if (objChartMode != null) objChartMode.UpdateChart();
            if (objChartMap != null) objChartMap.UpdateChart();
            if (objTableRecords != null) objTableRecords.UpdateTable();
            if (objTableRecentCount != null) objTableRecentCount.UpdateTable();
            if (objTableMode != null) objTableMode.UpdateTable();
            if (objTableBand != null) objTableBand.UpdateTable();
            if (objTableFrequency != null) objTableFrequency.UpdateTable();
            if (objTableFrequencyMode != null) objTableFrequencyMode.UpdateTable();
            if (objTableCountry != null) objTableCountry.UpdateTable();
            return;
        }

        /*-----------------------------------------------------------------------------
         * Create a file watcher for one of our ADIF folders.
         */
        private FileSystemWatcher CreateFileWatcher(string path)
        {
                try
                {
                    // Create a new FileSystemWatcher and set its properties.
                    FileSystemWatcher watcher = new FileSystemWatcher();
                    watcher.Path = path;
                    /* Watch for changes in LastAccess and LastWrite times */
                    watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite;
                    // Only watch ADIF files.
                    watcher.Filter = "*.adi";

                    // Add event handlers.
                    watcher.Changed += new FileSystemEventHandler(OnFileChanged);
                    watcher.Created += new FileSystemEventHandler(OnFileChanged);

                    // Begin watching.
                    watcher.EnableRaisingEvents = true;
                    return watcher;
                }
                catch
                {
                    return null;
                }
        }

        /*------------------------------------------------------------------------
         * Event handler for file change notifications.
         */
        private void OnFileChanged(object source, FileSystemEventArgs e)
        {
            /*
             * Schedule a scan of the ADIF files.
             */
            string strPath = e.FullPath;
            Globals.objAdif.QueueFileScan(strPath);
            ScheduleNewFileScan(4000);
            return;
        }

        /*-------------------------------------------------------------------------------
         * Schedule a scan of all folders after a specified number of milliseconds.
         */
        private void ScheduleFullScan(double dblMilliseconds)
        {
            Globals.dttFullScanTime = DateTime.UtcNow.AddMilliseconds(dblMilliseconds);
            return;
        }

        /*------------------------------------------------------------------------------
         * Schedule a scan of new/changed files.
         */
        private void ScheduleNewFileScan(double dblMilliseconds)
        {
            Globals.dttNewFileScanTime = DateTime.UtcNow.AddMilliseconds(dblMilliseconds);
            return;
        }

        /*-----------------------------------------------------------------------------
         * This routine runs as a thread to read new ADIF files in a specified folder.
         */
        private static void ReadAdifThread()
        {
            while (true)
            {
                /*
                 * See if we need to do a full scan looking for changed files in all folders.
                 */
                if (DateTime.UtcNow >= Globals.dttFullScanTime)
                {
                    /* Full scan for file changes */
                    Globals.dttFullScanTime = DateTime.MaxValue;
                    if (Globals.objAdif.ScanAllFolders(Globals.blnNeedFullRescan))
                    {
                        /* New records were added.  Request all charts be updated. */
                        Globals.LogWrite("New ADIF records were added.  Request update all charts.");
                        Globals.blnNeedChartUpdate = true;
                    }
                }
                /*
                 * See if we need to scan files that had changes.
                 */
                if (DateTime.UtcNow >= Globals.dttNewFileScanTime)
                {
                    /* Scan files that had changes */
                    Globals.dttNewFileScanTime = DateTime.MaxValue;
                    if (Globals.objAdif.ScanChangedFiles())
                    {
                        /* We added new ADIF records */
                        Globals.LogWrite("New ADIF records were found in changed files.  Request update all charts.");
                        Globals.blnNeedChartUpdate = true;
                    }
                }
                /* Sleep for a while */
                Thread.Sleep(200);
            }
        }

        /*---------------------------------------------------------------
         * Display settings screen.
         */
        private void btnSettings_Click(object sender, EventArgs e)
        {
            Settings objSettings = new Settings();
            DialogResult enmResult = objSettings.ShowDialog();
            if (enmResult == DialogResult.OK)
            {
                /* Set up file watching on each ADIF folder */
                SetFileWatchers();
                /* Settings were changed, do a full ADIF file scan */
                Globals.LogWrite("Settings were changed.  Do full file scan.");
                Globals.blnNeedFullRescan = true;
                ScheduleFullScan(100);
            }
            return;
        }

        /*------------------------------------------------------------------------------
         * The main form is closing.
         */
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveForClose();
            return;
        }

        /*---------------------------------------------------------------------------------------
         * Save information and close windows before stopping.
         */
        private void SaveForClose()
        {
            /*
             * Stop the timer.
             */
            tmrTick.Stop();
            Thread.Sleep(300);
            /*
             *  Stop the ADIF file scanner thread.
             */
            if (thrReadAdif != null)
            {
                thrReadAdif.Abort();
                thrReadAdif = null;
            }
            /*
             * Remember which charts need to be displayed.
             */
            if (Globals.objINIFile != null)
            {
                Globals.objINIFile.WriteBoolean("Main", "Chart Bearings", chkShowBearings.Checked);
                Globals.objINIFile.WriteBoolean("Main", "Chart Range", chkChartRange.Checked);
                Globals.objINIFile.WriteBoolean("Main", "Chart Day of Week", chkChartDayOfWeek.Checked);
                Globals.objINIFile.WriteBoolean("Main", "Chart Hour of Day", chkChartHourOfDay.Checked);
                Globals.objINIFile.WriteBoolean("Main", "Chart Month", chkChartMonth.Checked);
                Globals.objINIFile.WriteBoolean("Main", "Chart Band", chkChartBand.Checked);
                Globals.objINIFile.WriteBoolean("Main", "Chart Frequency", chkChartFrequency.Checked);
                Globals.objINIFile.WriteBoolean("Main", "Chart Mode", chkChartMode.Checked);
                Globals.objINIFile.WriteBoolean("Main", "Chart Map", chkChartMap.Checked);
                Globals.objINIFile.WriteBoolean("Main", "Table Records", chkTableRecords.Checked);
                Globals.objINIFile.WriteBoolean("Main", "Table Recent Count", chkTableRecentCount.Checked);
                Globals.objINIFile.WriteBoolean("Main", "Table Mode", chkTableMode.Checked);
                Globals.objINIFile.WriteBoolean("Main", "Table Band", chkTableBand.Checked);
                Globals.objINIFile.WriteBoolean("Main", "Table Frequency", chkTableFrequency.Checked);
                Globals.objINIFile.WriteBoolean("Main", "Table Frequency-Mode", chkTableFrequencyMode.Checked);
                Globals.objINIFile.WriteBoolean("Main", "Table Country", chkTableCountry.Checked);
                Globals.SaveFormPosition("Main", this);
            }
            /*
             * Close any charts that are open.
             */
            if (objChartRange != null)
            {
                objChartRange.Close();
                objChartRange = null;
            }
            if (objChartBearing != null)
            {
                objChartBearing.Close();
                objChartBearing = null;
            }
            if (objChartDayOfWeek != null)
            {
                objChartDayOfWeek.Close();
                objChartDayOfWeek = null;
            }
            if (objChartHourOfDay != null)
            {
                objChartHourOfDay.Close();
                objChartHourOfDay = null;
            }
            if (objChartMonth != null)
            {
                objChartMonth.Close();
                objChartMonth = null;
            }
            if (objChartBand != null)
            {
                objChartBand.Close();
                objChartBand = null;
            }
            if (objChartFrequency != null)
            {
                objChartFrequency.Close();
                objChartFrequency = null;
            }
            if (objChartMode != null)
            {
                objChartMode.Close();
                objChartMode = null;
            }
            if (objChartMap != null)
            {
                objChartMap.Close();
                objChartMap = null;
            }
            if (objTableRecords != null)
            {
                objTableRecords.Close();
                objTableRecords = null;
            }
            if (objTableRecentCount != null)
            {
                objTableRecentCount.Close();
                objTableRecentCount = null;
            }
            if (objTableMode != null)
            {
                objTableMode.Close();
                objTableMode = null;
            }
            if (objTableBand != null)
            {
                objTableBand.Close();
                objTableBand = null;
            }
            if (objTableFrequency != null)
            {
                objTableFrequency.Close();
                objTableFrequency = null;
            }
            if (objTableFrequencyMode != null)
            {
                objTableFrequencyMode.Close();
                objTableFrequencyMode = null;
            }
            if (objTableCountry != null)
            {
                objTableCountry.Close();
                objTableCountry = null;
            }
            /*
             * Report that we're closing.
             */
            Globals.LogWrite("----  ADIF Analyzer is closing  ----");
            /*
             * Flush the .ini file and close it.
             */
            if (Globals.objINIFile != null)
            {
                Globals.objINIFile.Flush();
                Globals.objINIFile = null;
            }
            return;
        }

        /*-------------------------------------------------------------------------------------
         * Routine called from Autoupdate to restart the program.
         */
        delegate void BeginRestartCallback();
        public void BeginRestart()
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.btnSettings.InvokeRequired)
            {
                BeginRestartCallback d = new BeginRestartCallback(BeginRestart);
                this.Invoke(d, new object[] { });
            }
            else
            {
                DoRestart();
            }
        }

        /*-------------------------------------------------------------------------------------
         * Close and restart the program.
         */
        private void DoRestart()
        {
            tmrTick.Stop();
            this.Close();
            Application.Restart();
            return;
        }

        /*--------------------------------------------------------------------
         * Display or remove the Bearings chart.
         */
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShowBearings.Checked)
            {
                objChartBearing = new ChartBearing();
                objChartBearing.Show();
            }
            else if (objChartBearing != null)
            {
                objChartBearing.Close();
                objChartBearing = null;
            }
            return;
        }

        /*-----------------------------------------------------------------------
         * Called when the bearings chart is closed.
         */
        public void ChartBearingClosed()
        {
            objChartBearing = null;
            chkShowBearings.Checked = false;
            return;
        }

        /*----------------------------------------------------------------------
         * Display or kill the Range chart.
         */
        private void chkChartRange_CheckedChanged(object sender, EventArgs e)
        {
            if (chkChartRange.Checked)
            {
                objChartRange = new ChartRange();
                objChartRange.Show();
            }
            else if (objChartRange != null)
            {
                objChartRange.Close();
                objChartRange = null;
            }
            return;
        }

        /*-----------------------------------------------------------------------
         * Called when the Range chart is closed.
         */
        public void ChartRangeClosed()
        {
            objChartRange = null;
            chkChartRange.Checked = false;
            return;
        }

        /*-----------------------------------------------------------------------------
         * Day of week chart state changed.
         */
        private void chkChartDayOfWeek_CheckedChanged(object sender, EventArgs e)
        {
            if (chkChartDayOfWeek.Checked)
            {
                objChartDayOfWeek = new ChartDayOfWeek();
                objChartDayOfWeek.Show();
            }
            else if (objChartDayOfWeek != null)
            {
                objChartDayOfWeek.Close();
                objChartDayOfWeek = null;
            }
            return;
        }

        /*-----------------------------------------------------------------------
         * Called when the Day-of-Week chart is closed.
         */
        public void ChartDayOfWeekClosed()
        {
            objChartDayOfWeek = null;
            chkChartDayOfWeek.Checked = false;
            return;
        }

        /*------------------------------------------------------------------------
         * Show/hide hour of day chart.
         */
        private void chkChartHourOfDay_CheckedChanged(object sender, EventArgs e)
        {
            if (chkChartHourOfDay.Checked)
            {
                objChartHourOfDay = new ChartHourOfDay();
                objChartHourOfDay.Show();
            }
            else if (objChartHourOfDay != null)
            {
                objChartHourOfDay.Close();
                objChartHourOfDay = null;
            }
            return;
        }

        /*-----------------------------------------------------------------------
         * Called when the Hour of day chart is closed.
         */
        public void ChartHourOfDayClosed()
        {
            objChartHourOfDay = null;
            chkChartHourOfDay.Checked = false;
            return;
        }

        /*-----------------------------------------------------------------
         * Show/hide connections by month.
         */
        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            if (chkChartMonth.Checked)
            {
                objChartMonth = new ChartMonth();
                objChartMonth.Show();
            }
            else if (objChartMonth != null)
            {
                objChartMonth.Close();
                objChartMonth = null;
            }
            return;
        }

        /*-----------------------------------------------------------------------
         * Called when the connections by month chart is closed.
         */
        public void ChartMonthClosed()
        {
            objChartMonth = null;
            chkChartMonth.Checked = false;
            return;
        }

        /*-------------------------------------------------------------------------
         * Show/hide chart of connections by band.
         */
        private void chkChartBand_CheckedChanged(object sender, EventArgs e)
        {
            if (chkChartBand.Checked)
            {
                objChartBand = new ChartBand();
                objChartBand.Show();
            }
            else if (objChartBand != null)
            {
                objChartBand.Close();
                objChartBand = null;
            }
            return;
        }

        /*-------------------------------------------------------------------------
          * Show/hide chart of connections by frequency.
          */
        private void chkChartFrequency_CheckedChanged(object sender, EventArgs e)
        {
            if (chkChartFrequency.Checked)
            {
                objChartFrequency = new ChartFrequency();
                objChartFrequency.Show();
            }
            else if (objChartFrequency != null)
            {
                objChartFrequency.Close();
                objChartFrequency = null;
            }
            return;
        }

        /*-------------------------------------------------------------------------
         * Show/hide table of connections by frequency.
         */
        private void chkTableFrequency_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTableFrequency.Checked)
            {
                objTableFrequency = new TableFrequency();
                objTableFrequency.Show();
            }
            else if (objTableFrequency != null)
            {
                objTableFrequency.Close();
                objTableFrequency = null;
            }
            return;
        }

        /*-----------------------------------------------------------------------
         * Called when the connections by band chart is closed.
         */
        public void ChartBandClosed()
        {
            objChartBand = null;
            chkChartBand.Checked = false;
            return;
        }

        /*-----------------------------------------------------------------------
         * Called when the connections by frequency chart is closed.
         */
        public void ChartFrequencyClosed()
        {
            objChartFrequency = null;
            chkChartFrequency.Checked = false;
            return;
        }

        /*-----------------------------------------------------------------------
         * Called when the connections by frequency table is closed.
         */
        public void TableFrequencyClosed()
        {
            objTableFrequency = null;
            chkTableFrequency.Checked = false;
            return;
        }

        /*------------------------------------------------------------------
         * Show/hide connnections by mode.
         */
        private void chkChartMode_CheckedChanged(object sender, EventArgs e)
        {
            if (chkChartMode.Checked)
            {
                objChartMode = new ChartMode();
                objChartMode.Show();
            }
            else if (objChartMode != null)
            {
                objChartMode.Close();
                objChartMode = null;
            }
            return;
        }

        /*-----------------------------------------------------------------------
         * Called when the connections by mode is closed.
         */
        public void ChartModeClosed()
        {
            objChartMode = null;
            chkChartMode.Checked = false;
            return;
        }

        /*-------------------------------------------------------------------------
         * Show/hide the ADIF records table.
         */
        private void chkTableRecords_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTableRecords.Checked)
            {
                objTableRecords = new TableRecords();
                objTableRecords.Show();
            }
            else if (objTableRecords != null)
            {
                objTableRecords.Close();
                objTableRecords = null;
            }
            return;
        }

        /*-----------------------------------------------------------------------
         * The ADIF records table has been closed.
         */
        public void TableRecordsClosed()
        {
            objTableRecords = null;
            chkTableRecords.Checked = false;
            return;
        }

        /*-------------------------------------------------------------------------------
         * Show/hide recent connection counts.
         */
        private void chkTableRecentCount_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTableRecentCount.Checked)
            {
                objTableRecentCount = new TableRecentCount();
                objTableRecentCount.Show();
            }
            else if (objTableRecentCount != null)
            {
                objTableRecentCount.Close();
                objTableRecentCount = null;
            }
            return;
        }

        /*-----------------------------------------------------------------------
         * The Recent Count table has been closed.
         */
        public void TableRecentCountClosed()
        {
            objTableRecentCount = null;
            chkTableRecentCount.Checked = false;
            return;
        }

        /*------------------------------------------------------------------------
         * Show/hide connections by mode.
         */
        private void chkTableMode_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTableMode.Checked)
            {
                objTableMode = new TableMode();
                objTableMode.Show();
            }
            else if (objTableMode != null)
            {
                objTableMode.Close();
                objTableMode = null;
            }
            return;
        }

        /*-----------------------------------------------------------------------
         * The connections by mode table has been closed.
         */
        public void TableModeClosed()
        {
            objTableMode = null;
            chkTableMode.Checked = false;
            return;
        }

        /*-------------------------------------------------------------------------
         * Show/hide connections by band table.
         */
        private void chkTableBand_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTableBand.Checked)
            {
                objTableBand = new TableBand();
                objTableBand.Show();
            }
            else if (objTableBand != null)
            {
                objTableBand.Close();
                objTableBand = null;
            }
            return;
        }

        /*-----------------------------------------------------------------------
         * The connections by band table has been closed.
         */
        public void TableBandClosed()
        {
            objTableBand = null;
            chkTableBand.Checked = false;
            return;
        }

        /*------------------------------------------------------------------------
         * Show/hide the frequency-mode table.
         */
        private void chkTableFrequencyMode_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTableFrequencyMode.Checked)
            {
                objTableFrequencyMode = new TableFrequencyMode();
                objTableFrequencyMode.Show();
            }
            else if (objTableFrequencyMode != null)
            {
                objTableFrequencyMode.Close();
                objTableFrequencyMode = null;
            }
            return;
        }

        /*-----------------------------------------------------------------------
         * The Frequency-Mode table has been closed.
         */
        public void TableFrequencyModeClosed()
        {
            objTableFrequencyMode = null;
            chkTableFrequencyMode.Checked = false;
            return;
        }

        /*----------------------------------------------------------------------
         * Show/hide the table of countries.
         */
        private void chkTableCountry_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTableCountry.Checked)
            {
                objTableCountry = new TableCountry();
                objTableCountry.Show();
            }
            else if (objTableCountry != null)
            {
                objTableCountry.Close();
                objTableCountry = null;
            }
            return;
        }

        /*-----------------------------------------------------------------------
         * The country table has been closed.
         */
        public void TableCountryClosed()
        {
            objTableCountry = null;
            chkTableCountry.Checked = false;
            return;
        }

        /*-------------------------------------------------------------------
         * Show/Hide the map.
         */
        private void chkChartMap_CheckedChanged(object sender, EventArgs e)
        {
            if (chkChartMap.Checked)
            {
                objChartMap = new ChartMap();
                objChartMap.Show();
            }
            else if (objChartMap != null)
            {
                objChartMap.Close();
                objChartMap = null;
            }
            return;
        }

        /*-----------------------------------------------------------------------
         * The map has been closed.
         */
        public void ChartMapClosed()
        {
            objChartMap = null;
            chkChartMap.Checked = false;
            return;
        }
    }
}
