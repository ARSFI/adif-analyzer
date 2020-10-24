using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;

namespace ADIF_Analyzer
{

    /*--------------------------------------------------------------------------------------
     * Class describing an ADIF file we've processed.
     */
    public class AdifFile
    {
        public string strFilePath;
        public DateTime dttDate;
        public long intSize;
    }

    /*----------------------------------------------------------------------------------------
     * Class describing an ADIF entry.
     */
    public class AdifRecord
    {
        /*
         * Data members.
         */
        public string strConnectedCallsign;         /* Callsign that connected */
        public DateTime dttQsoStart;                /* Start time of the connection */
        public DateTime dttQsoEnd;                  /* End time of the connection */
        public string strMode;                      /* Connecting mode */
        public string strGridSquare;                /* Grid square of connecting person */
        public string strBand;                      /* HF band for connection */
        public double dblFrequency;                 /* Frequency in kHz of connection */
        public string strLastCommand;               /* Last things received from client */
        public string strComment;                   /* Comment on ADIF record */
        public LatitudeLongitude Location;          /* Latitude/Longitude of connecting station */
        public string strCountry;                   /* Country matching the callsign */
        public long intAccession;                   /* Accession number */
        public bool blnNew;                         /* New entry that needs to be processed */
        public AdifRecord()
        {
            intAccession = ++Globals.intLastRecordAccession;
        }
    }

    public class UpdatedAdifFile
    {
        public string strFilePath;
        public DateTime dttChangeTime;
    }

    /*----------------------------------------------------------------------------
     * Path to a folder holding ADIF files to process.
     */
    public class AdifPath
    {
        public string strPath = "";                         /* Full file path */
        public FileSystemWatcher objFileWatcher = null;     /* File change notifications */
    }

    /*--------------------------------------------------------------------------------
     * Entry describing a callsign country code prefix.
     */
    public class CountryCodeEntry
    {
        public string strPrefix;                    /* Callsign prefix */
        public string strCountry;                   /* Name of country */
        public int intPrefixLen;                    /* Length of the prefix */
    }

    /*-------------------------------------------------------------------------------------
     * Objects to hold country names and connnection count.
     */
    public class CountryCount
    {
        public string strCountry;
        public int intCount;
    }

    /*----------------------------------------------------------------------------------
     * Statistics for one day.
     */
    public class DayStatistics
    {
        public DateTime dttDate;
        public int Connections = 0;
        public TimeSpan ConnectTime = TimeSpan.MinValue;
    }

    /*----------------------------------------------------------------------------------
     * Frequency-Mode matrix data.
     */
    public class FrequencyModeData
    {
        public int NumFreq;                 /* Number of frequencies */
        public int NumModes;                /* Number of modes */
        public double[] aryFrequency;              /* Array of frequencies */
        public string[] aryModes;                  /* Array of modes */
        public int[,] aryCount;                    /* Frequency,Mode connection counts */
    }
    public class AdifProcess
    {
        private object objQueueFileLock = new object();
        private List<string> lstPendingFiles = new List<string>();

        /*--------------------------------------------------------------------------------
         * Queue a request to scan a particular ADIF file.
         */
        public void QueueFileScan(string strPath)
        {
            lock (objQueueFileLock)
            {
                if (!lstPendingFiles.Contains(strPath))
                {
                    lstPendingFiles.Add(strPath);
                    Globals.LogWrite("File change notification: " + strPath);
                }
            }
            return;
        }

        /*-------------------------------------------------------------------------------------
         * Scan the files in the pending file list.
         * Return true if new records were added.
         */
        public bool ScanChangedFiles()
        {
            bool blnFoundNewRecords = false;
            lock (objQueueFileLock)
            {
                foreach (string strPath in lstPendingFiles)
                {
                    blnFoundNewRecords |= ScanAdifFile(strPath);
                }
                lstPendingFiles.Clear();
            }
            return blnFoundNewRecords;
        }

        /*-----------------------------------------------------------------------------
         * Scan all ADIF folders and process changes.
         * Return true if new records were added.
         */
        public bool ScanAllFolders(bool blnFullReset)
        {
            /*
             * if we're doing a full reset, clear out some things.
             */
            if (blnFullReset)
            {
                lock (Globals.objAdifLock)
                {
                    Globals.dicAdifFile.Clear();
                    Globals.dicADIF.Clear();
                    Globals.intLastRecordAccession = 0;
                }
            }
            /*
             * Since we're checking all files, clear the list of changed files.
             */
            lock (objQueueFileLock)
            {
                lstPendingFiles.Clear();
                Globals.dttNewFileScanTime = DateTime.MaxValue;
            }
            /*
             * Scan each ADIF folder looking for changed files.
             */
            bool blnNewRecord = false;
            foreach (AdifPath objPath in Globals.aryAdifPath)
            {
                if (objPath.strPath != "")
                {
                    blnNewRecord |= ReadNewAdifFiles(objPath.strPath);
                }
            }
            /*
             * Finished
             */
            return blnNewRecord;
        }

        /*-----------------------------------------------------------------------------
         * Read newly updated files in a specified folder.
         * Return true if records were added.
         */
        private bool ReadNewAdifFiles(string strPath)
        {
            string[] aryFiles;
            if (strPath == null || strPath == "") return false;
            bool blnNewRecord = false;
            /*
            * Get a list of files in this folder.
            */
            try
            {
                aryFiles = Directory.GetFiles(strPath, "*.adi", SearchOption.TopDirectoryOnly);
            }
            catch
            {
                return false;
            }
            foreach (string strFileEntry in aryFiles)
            {
                blnNewRecord |= ScanAdifFile(strFileEntry);
            }
            return blnNewRecord;
        }

        /*--------------------------------------------------------------------------
         * Scan an ADIF file.  Return true if there are new records.
         */
        private bool ScanAdifFile(string strFileEntry)
        {
            bool blnNewRecord = false;
            try
            {
                /* Get information about this file */
                FileInfo objInfo = new System.IO.FileInfo(strFileEntry);
                AdifFile objFile = new AdifFile();
                objFile.intSize = objInfo.Length;
                objFile.dttDate = objInfo.LastWriteTime;
                /*
                 * See if we have an entry for this file.
                 */
                if (Globals.dicAdifFile.ContainsKey(strFileEntry))
                {
                    AdifFile objEntry = Globals.dicAdifFile[strFileEntry];
                    if (objEntry.dttDate != objFile.dttDate || objEntry.intSize != objFile.intSize)
                    {
                        /* File changed.  Process it */
                        blnNewRecord = ReadAdifFile(strFileEntry);
                        objEntry.intSize = objFile.intSize;
                        objEntry.dttDate = objFile.dttDate;
                    }
                }
                else
                {
                    /* This is a new file */
                    blnNewRecord = ReadAdifFile(strFileEntry);
                    Globals.dicAdifFile.Add(strFileEntry, objFile);
                    Globals.TotalFiles++;
                }
            }
            catch
            {

            }
            if (blnNewRecord) Globals.LogWrite("Found new records in file " + strFileEntry);
            return blnNewRecord;
        }

        /*-----------------------------------------------------------------------------
         * Try to read an ADIF file, parse the records, and add new entries to dicADIF.
         * Return true if new records were added.
         */
        public bool ReadAdifFile(string strFileSpec)
        {
            /*
             * Attempt to read the entire ADIF file.
             */
            Globals.LogWrite("Scanning file for new records: " + strFileSpec);
            bool blnNewRecord = false;
            string strError = "";
            string[] aryLines = null;
            for (int intTry = 0; intTry < 5; intTry++)
            {
                try
                {
                    strError = "";
                    aryLines = File.ReadAllLines(strFileSpec);
                    break;
                }
                catch (Exception ex)
                {
                    strError = ex.Message;
                    Thread.Sleep(1000 + 500 * intTry);
                }
            }
            if (strError != "")
            {
                /* Unable to read the file */
                return false;
            }
            /*
             * We read the file.
             * Parse each line and add new records to dicADIF.
             */
            lock (Globals.objAdifLock)
            {
                AdifRecord objAdif = new AdifRecord();
                foreach (string strLine in aryLines)
                {
                    /* Skip the file header line */
                    if (!strLine.Contains("<adif_ver:"))
                    {
                        /*
                         * Parse the ADIF record and generatge a AdifRecord object.
                         */
                        objAdif = ParseAdifRecord(strLine);
                        if (objAdif != null)
                        {
                            /*
                             * See if we are limiting records by date range.
                             */
                            DateTime dttQsoStart = objAdif.dttQsoStart;
                            DateTime dttQsoEnd = objAdif.dttQsoEnd;
                            if (!Globals.blnTimeZoneUTC)
                            {
                                dttQsoStart = dttQsoStart.ToLocalTime();
                                dttQsoEnd = dttQsoEnd.ToLocalTime();
                            }
                            if ((!Globals.blnLimitADIFStartDate || dttQsoStart >= Globals.dttStartDate) &&
                                (!Globals.blnLimitADIFEndDate || dttQsoEnd <= Globals.dttEndDate))
                            {
                                /*
                                 * See if we're already storing this record.
                                 */
                                string strKey = objAdif.dttQsoStart.ToString("yyyyMMddhhmmss") + "|" + objAdif.strConnectedCallsign + "|" + strFileSpec;
                                if (!Globals.dicADIF.ContainsKey(strKey))
                                {
                                    /* This is a new record that needs to be added and processed */
                                    objAdif.blnNew = true;
                                    Globals.dicADIF.Add(strKey, objAdif);
                                    Globals.TotalRecords = Globals.dicADIF.Count;
                                    blnNewRecord = true;
                                }
                            }
                        }
                    }
                }
            }
            /*
             * Finished
             */
            return blnNewRecord;
        }

        /*---------------------------------------------------------------------
         * Parse an ADIF record and return an AdifRecord object.
         */
        private AdifRecord ParseAdifRecord(string strLine)
        {
            AdifRecord objAdif = new AdifRecord();
            /*
             * Get and process each field.
             */
            string strField = GetField(strLine, "call");
            if (strField == null || strField == "") return null;
            objAdif.strConnectedCallsign = strField;
            string strDate = GetField(strLine, "qso_date");
            if (strDate == null || strField == "") return null;
            strField = GetField(strLine, "time_on");
            if (strField == null || strField == "") return null;
            DateTime dttTime;
            string strFullTime = strDate + strField;
            if (!DateTime.TryParseExact(strFullTime, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dttTime))
            {
                return null;
            }
            objAdif.dttQsoStart = dttTime;
            strField = GetField(strLine, "time_off");
            if (strField == null || strField == "") return null;
            if (!DateTime.TryParseExact(strDate + strField, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dttTime))
            {
                return null;
            }
            objAdif.dttQsoEnd = dttTime;
            if (objAdif.dttQsoEnd < objAdif.dttQsoStart)
            {
                /* QSO crossed a day boundary.  Add 24 hours to end time to correct */
                objAdif.dttQsoEnd = objAdif.dttQsoEnd.AddDays(1);
            }
            strField = GetField(strLine, "mode");
            if (strField == null) return null;
            objAdif.strMode = strField;
            strField = GetField(strLine, "gridsquare");
            if (strField == null) return null;
            objAdif.strGridSquare = strField;
            strField = GetField(strLine, "band");
            if (strField == null) strField = "";
            objAdif.strBand = strField;
            strField = GetField(strLine, "freq");
            if (strField == null) return null;
            try
            {
                objAdif.dblFrequency = 1000.0 * Convert.ToDouble(strField);
            }
            catch
            {
                Globals.Exceptions("Invalid frequency for record at " + Globals.FormatDateTime(objAdif.dttQsoStart) + "  " + strField);
                objAdif.dblFrequency = 0.0;
            }
            /* Correct for missing 60M band entries */
            if (objAdif.strBand == "" && objAdif.dblFrequency >= 5300.0 && objAdif.dblFrequency <= 5500.0)
            {
                objAdif.strBand = "60M";
            }
            strField = GetField(strLine, "comment");
            if (strField == null) return null;
            objAdif.strComment = strField;
            objAdif.Location = Globals.GridSquareToDecimalDegrees(objAdif.strGridSquare);
            /*
             * If there are missing items, try to get them out of the comments.
             */
            if (objAdif.strComment != null && objAdif.strComment != "")
            {
                /* Split the comment fields */
                string[] strTok = objAdif.strComment.Split('|');
                if (strTok.Length >= 13)
                {
                    /* Get the last command we received from the client */
                    objAdif.strLastCommand = strTok[12].Trim();
                }
                if (objAdif.strMode == "")
                {
                    /* Mitigate Trimode bug by getting the mode out of the comment */
                    if (strTok.Length >= 9)
                    {
                        string strMode = strTok[8].Trim().ToLower();
                        if (strMode.StartsWith("pactor") && strMode.Length >= 8)
                        {
                            strMode = "PAC" + strMode.Substring(7);
                        }
                        objAdif.strMode = strTok[8];
                    }
                }
                if (objAdif.Location.dblLatitude == 0.0 && objAdif.Location.dblLongitude == 0.0)
                {
                    /* Missing grid square */
                    if (strTok.Length >= 19)
                    {
                        objAdif.Location = Globals.GridSquareToDecimalDegrees(strTok[18]);
                   }
                }
            }
            /*
             * Try to map the callsign to the corresponding country.
             */
            objAdif.strCountry = CallsignToCountry(objAdif.strConnectedCallsign);
            /*
             * Finished.  Return the complete AdifRecord.
             */
            return objAdif;
        }

        /*---------------------------------------------------------------------------------
         * Extract a field from the ADIF record.  Return null if the field doesn't exist.
         */
        private string GetField(string strLine, string keyword)
        {

            int intStart = strLine.IndexOf("<" + keyword + ":");
            if (intStart < 0)
            {
                return null;
            }
            intStart = strLine.IndexOf(">", intStart);
            if (intStart < 0)
            {
                return null;
            }
            intStart++;
            int intEnd = strLine.IndexOf("<", intStart);
            if (intEnd < 0)
            {
                return null;
            }
            return strLine.Substring(intStart, intEnd - intStart).Trim();
        }

        /*-------------------------------------------------------------------------------------------------
         * Get range and/or bearing for connections.
         */
        public List<RangeBearing> GetRangeBearings(long intStartAccession, bool blnWantRange = true, bool blnWantBearing = true, int intMaxLinesWanted = 10000)
        {
            List<RangeBearing> lstItems = new List<RangeBearing>();
            /*
             * Determine proportion of items to get.
             */
            lock (Globals.objAdifLock)
            {
                int NumItems = 0;
                foreach (KeyValuePair<string, AdifRecord> objEntry in Globals.dicADIF)
                {
                    if (objEntry.Value.intAccession >= intStartAccession) NumItems++;
                }
                if (NumItems == 0) return lstItems;
                double dblProportion = (double)intMaxLinesWanted / (double)NumItems;
                /*
                 * Create the list of items to return.
                 * Do random sampling if we want fewer than the full count.
                 */
                Random objRandom = new Random();
                foreach (KeyValuePair<string, AdifRecord> objEntry in Globals.dicADIF)
                {
                    AdifRecord objRec = objEntry.Value;
                    if (objRec.intAccession >= intStartAccession && objRec.Location.dblLatitude != 0.0 && objRec.Location.dblLongitude != 0.0)
                    {
                        if (dblProportion >= 0.99 || dblProportion >= objRandom.NextDouble())
                        {
                            RangeBearing objRangeBearing;
                            if (blnWantBearing && blnWantRange)
                            {
                                objRangeBearing = Globals.RangeAndBearing(Globals.objStationLocation, objRec.Location);
                            }
                            else if (blnWantBearing)
                            {
                                objRangeBearing = new RangeBearing();
                                objRangeBearing.dblBearing = Globals.ComputeBearing(Globals.objStationLocation, objRec.Location);
                            }
                            else
                            {
                                objRangeBearing = new RangeBearing();
                                objRangeBearing.dblRange = Globals.ComputeRange(Globals.objStationLocation, objRec.Location);
                            }
                            lstItems.Add(objRangeBearing);
                        }
                    }
                }
            }
            return lstItems;
        }

        /*-------------------------------------------------------------------------------------------------------
         * Get a vector with connections for each day of the week.
         */
        public int[] ConnectionsByDayOfWeek()
        {
            int[] aryDays = new int[7];
            for (int i = 0; i < 7; i++) aryDays[i] = 0;
            lock (Globals.objAdifLock)
            {
                foreach (KeyValuePair<string, AdifRecord> objEntry in Globals.dicADIF)
                {
                    int DayOfWeek;
                    if (Globals.blnTimeZoneUTC)
                    {
                        DayOfWeek = (int)objEntry.Value.dttQsoStart.DayOfWeek;
                    }
                    else
                    {
                        DayOfWeek = (int)objEntry.Value.dttQsoStart.ToLocalTime().DayOfWeek;
                    }
                    aryDays[DayOfWeek]++;
                }
            }
            return aryDays;
        }

        /*-------------------------------------------------------------------------------------------------------
         * Get a vector with connections for each hour of the day.
         */
        public int[] ConnectionsByHourOfDay()
        {
            int[] aryHours = new int[24];
            for (int i = 0; i < 24; i++) aryHours[i] = 0;
            lock (Globals.objAdifLock)
            {
                foreach (KeyValuePair<string, AdifRecord> objEntry in Globals.dicADIF)
                {
                    int hour;
                    if (Globals.blnTimeZoneUTC)
                    {
                        hour = (int)objEntry.Value.dttQsoStart.Hour;
                    }
                    else
                    {
                        hour = (int)objEntry.Value.dttQsoStart.ToLocalTime().Hour;
                    }
                    aryHours[hour]++;
                }
            }
            return aryHours;
        }

        /*-------------------------------------------------------------------------------------------------------
         * Get a vector with connections for each month of the year.
         */
        public int[] ConnectionsByMonth()
        {
            int[] aryCount = new int[12];
            for (int i = 0; i < 12; i++) aryCount[i] = 0;
            lock (Globals.objAdifLock)
            {
                foreach (KeyValuePair<string, AdifRecord> objEntry in Globals.dicADIF)
                {
                    int month;
                    if (Globals.blnTimeZoneUTC)
                    {
                        month = objEntry.Value.dttQsoStart.Month - 1;
                    }
                    else
                    {
                        month = objEntry.Value.dttQsoStart.ToLocalTime().Month - 1;
                    }
                    aryCount[month]++;
                }
            }
            /*
             * Normalize the month counts by dividing by the number of occurrences of each month in the data.
             */
            double[] dblWeight = GetMonthFactors();
            for (int i = 0; i < 12; i++)
            {
                if (aryCount[i] > 0)
                {
                    if (dblWeight[i] > 0.0)
                    {
                        aryCount[i] = (int)(0.5 + aryCount[i] / dblWeight[i]);
                    }
               }
            }
            return aryCount;
        }

        /*--------------------------------------------------------------------------------------------------
         * Get a vector to normalize connect counts by month.
         * The vector has the number of whole or partial occurrences of each month.
         */
        public double[] GetMonthFactors()
        {
            double[] dblWeight = new double[12];
            for (int i = 0; i < 12; i++) dblWeight[i] = 0.0;
            /*
             * Determine the range of connect dates we're working with.
             */
            DateTime dttMinDate = DateTime.MaxValue;
            DateTime dttMaxDate = DateTime.MinValue;
            int count = 0;
            lock (Globals.objAdifLock)
            {
                foreach (KeyValuePair<string, AdifRecord> objEntry in Globals.dicADIF)
                {
                    DateTime date = objEntry.Value.dttQsoStart;
                    if (!Globals.blnTimeZoneUTC) date = date.ToLocalTime();
                    if (date < dttMinDate) dttMinDate = date;
                    if (date > dttMaxDate) dttMaxDate = date;
                    count++;
                }
            }
            if (count == 0) return dblWeight;
            /* Handle the case where the start and end are in the same month */
            if ((dttMaxDate - dttMinDate).TotalDays <= 31 && dttMaxDate.Month == dttMinDate.Month)
            {
                dblWeight[dttMinDate.Month - 1] = 1;
                return dblWeight;
            }
            /* We span at least two months.  Get portion of first month */
            double dblDaysInMonth = DateTime.DaysInMonth(dttMinDate.Year, dttMinDate.Month);
            dblWeight[dttMinDate.Month - 1] = (dblDaysInMonth + 1 - dttMinDate.Day) / dblDaysInMonth;
            /* Get portion of ending month */
            dblDaysInMonth = DateTime.DaysInMonth(dttMaxDate.Year, dttMaxDate.Month);
            dblWeight[dttMaxDate.Month - 1] += dttMaxDate.Day / dblDaysInMonth;
            /* Increment counts for whole months between the start and end */
            DateTime dttMonth = new DateTime(dttMinDate.Year, dttMinDate.Month, 1);
            dttMonth = dttMonth.AddMonths(1);
            while (dttMonth.AddMonths(1) < dttMaxDate)
            {
                dblWeight[dttMonth.Month - 1]++;
                dttMonth = dttMonth.AddMonths(1);
            }
            /*
             * Finished
             */
            return dblWeight;
        }

        /*-------------------------------------------------------------------------------------------------------
         * Get a list of ConnectionCountItem objects for each frequency.
         */
        public List<ConnectionCountItem> ConnectionsByFrequency()
        {
            /*
             * Create a sorted dictionary with counts for each frequency.
             */
            SortedDictionary<double, ConnectionCountItem> dicItems = new SortedDictionary<double, ConnectionCountItem>();
            lock (Globals.objAdifLock)
            {
                foreach (KeyValuePair<string, AdifRecord> objEntry in Globals.dicADIF)
                {
                    double dblFreq = objEntry.Value.dblFrequency;
                    if (dblFreq > 0.0)
                    { 
                        string strFreq = dblFreq.ToString("0.0");
                       if (dicItems.ContainsKey(dblFreq))
                        {
                            dicItems[dblFreq].intCount++;
                        }
                        else
                        {
                            ConnectionCountItem objItem = new ConnectionCountItem();
                            objItem.strItem = strFreq;
                            objItem.intCount = 1;
                            dicItems.Add(dblFreq, objItem);
                        }
                    }
                }
            }
            /*
             * Convert the sorted dictionary to a list.
             */
            List<ConnectionCountItem> lstItems = new List<ConnectionCountItem>();
            foreach (KeyValuePair<double, ConnectionCountItem> kvp in dicItems)
            {
                lstItems.Add(kvp.Value);
            }
            return lstItems;
        }

        /*-------------------------------------------------------------------------------------------------------
         * Get a list of ConnectionCountItem objects for each band.
         */
        public List<ConnectionCountItem> ConnectionsByBand()
        {
            /*
             * Create a sorted dictionary with counts for each band.
             */
            SortedDictionary<string, ConnectionCountItem> dicItems = new SortedDictionary<string, ConnectionCountItem>();
            lock (Globals.objAdifLock)
            {
                foreach (KeyValuePair<string, AdifRecord> objEntry in Globals.dicADIF)
                {
                    string strBand = objEntry.Value.strBand.Trim();
                    if (strBand != "")
                    {
                        if (dicItems.ContainsKey(strBand))
                        {
                            dicItems[strBand].intCount++;
                        }
                        else
                        {
                            ConnectionCountItem objItem = new ConnectionCountItem();
                            objItem.strItem = strBand;
                            objItem.intCount = 1;
                            dicItems.Add(strBand, objItem);
                        }
                    }
                }
            }
            /*
             * Convert the sorted dictionary to a list.
             */
            List<ConnectionCountItem> lstItems = new List<ConnectionCountItem>();
            foreach (KeyValuePair<string, ConnectionCountItem> kvp in dicItems)
            {
                lstItems.Add(kvp.Value);
            }
            return lstItems;
        }

        /*-------------------------------------------------------------------------------------------------------
         * Get a list of ConnectionCountItem objects for each mode.
         */
        public List<ConnectionCountItem> ConnectionsByMode()
        {
            /*
             * Create a sorted dictionary with counts for each mode.
             */
            SortedDictionary<string, ConnectionCountItem> dicItems = new SortedDictionary<string, ConnectionCountItem>();
            lock (Globals.objAdifLock)
            {
                foreach (KeyValuePair<string, AdifRecord> objEntry in Globals.dicADIF)
                {
                    string strMode = objEntry.Value.strMode.Trim();
                    /* Map the mode name */
                    strMode = Globals.MapModeName(strMode);
                    /*
                     * Add an entry to the dictionary.
                     */
                    if (dicItems.ContainsKey(strMode))
                    {
                        dicItems[strMode].intCount++;
                    }
                    else
                    {
                        ConnectionCountItem objItem = new ConnectionCountItem();
                        objItem.strItem = strMode;
                        objItem.intCount = 1;
                        dicItems.Add(strMode, objItem);
                    }
                }
            }
            /*
             * Convert the sorted dictionary to a list.
             */
            List<ConnectionCountItem> lstItems = new List<ConnectionCountItem>();
            foreach (KeyValuePair<string, ConnectionCountItem> kvp in dicItems)
            {
                lstItems.Add(kvp.Value);
            }
            return lstItems;
        }

        /*--------------------------------------------------------------------------------------
         * Get a list of the most recnet ADIF records.
         * The list is returned in descending starttime order.
         */
        public List<AdifRecord> GetRecentAdifRecords(int intMaxWanted)
        {
            List<AdifRecord> lstRec = new List<AdifRecord>();
            /*
             * Initially get all records.
             */
            lock (Globals.objAdifLock)
            {
                foreach (KeyValuePair<string, AdifRecord> kvp in Globals.dicADIF)
                {
                    lstRec.Add(kvp.Value);
                }
            }
            /*
             * Sort in descending start time order.
             */
            lstRec.Sort((x, y) => CompareDates(y.dttQsoStart, x.dttQsoStart));
            /*
             * Get only as many entries as wanted.
             */
            if (lstRec.Count > intMaxWanted)
            {
                lstRec = lstRec.GetRange(0, intMaxWanted);
            }
            /*
             * Finished
             */
            return lstRec;
        }

        /*--------------------------------------------------------------------------------------
         * Get a list of the most recnet ADIF records that have unique grid squares.
         * The list is returned in descending starttime order.
         */
        public List<AdifRecord> GetUniqueLocationAdifRecords(int intMaxWanted)
        {
            List<AdifRecord> lstRec = new List<AdifRecord>();
            Dictionary<string, AdifRecord> dicLocation = new Dictionary<string, AdifRecord>();
            AdifRecord objAdif;
            /*
             * Get newest records with unique grid squares.
             */
            lock (Globals.objAdifLock)
            {
                foreach (KeyValuePair<string, AdifRecord> kvp in Globals.dicADIF)
                {
                    objAdif = kvp.Value;
                    string strGridSquare = objAdif.strGridSquare;
                    if (strGridSquare != "")
                    {
                        if (dicLocation.ContainsKey(strGridSquare))
                        {
                            /* We've already seen a record with this grid square.  Which is newer? */
                            if (objAdif.dttQsoStart > dicLocation[strGridSquare].dttQsoStart)
                            {
                                /* Keep newer contact at this grid square */
                                dicLocation[strGridSquare] = objAdif;
                            }
                        }
                        else
                        {
                            /* New grid square entry */
                            dicLocation.Add(strGridSquare, objAdif);
                        }
                    }
                }
            }
            /*
             * Convert dictionary entries to a list.
             */
            foreach (KeyValuePair<string, AdifRecord> kvp in dicLocation)
            {
                lstRec.Add(kvp.Value);
            }
            /*
             * Sort in descending start-time order.
             */
            lstRec.Sort((x, y) => CompareDates(y.dttQsoStart, x.dttQsoStart));
            /*
             * Get only as many entries as wanted.
             */
            if (lstRec.Count > intMaxWanted)
            {
                lstRec = lstRec.GetRange(0, intMaxWanted);
            }
            /*
             * Finished
             */
            return lstRec;
        }

        /*-----------------------------------------------------------------------
         * Compare two dates.
         * Date1>Date2 = 1; Date1<Date2 = -1; Date1=Date2 = 0.
         */
        private int CompareDates(DateTime dttDate1, DateTime dttDate2)
        {
            if (dttDate1 < dttDate2)
            {
                return -1;
            }
            else if (dttDate1 > dttDate2)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        /*----------------------------------------------------------------------------------------------
         * Get connection statistics for recent days.
         */
        public DayStatistics[] GetRecentCount(int intNumDays)
        {
            DateTime dttToday;
            DateTime dttNext;
            if (Globals.blnTimeZoneUTC)
            {
                dttToday = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day);
                dttNext = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day);
            }
            else
            {
                dttToday = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                dttNext = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            }
            DayStatistics[] aryDay = new DayStatistics[intNumDays];
            for (int i = 0; i < intNumDays; i++)
            {
                aryDay[i] = new DayStatistics();
                aryDay[i].dttDate = dttNext.AddDays(-i);
                aryDay[i].Connections = 0;
                aryDay[i].ConnectTime = TimeSpan.FromSeconds(0);
            }
            lock (Globals.objAdifLock)
            {
                foreach (KeyValuePair<string, AdifRecord> kvp in Globals.dicADIF)
                {
                    AdifRecord objRec = kvp.Value;
                    DateTime dttStart = objRec.dttQsoStart;
                    if (!Globals.blnTimeZoneUTC) dttStart = dttStart.ToLocalTime();
                    DateTime dttRec = new DateTime(dttStart.Year, dttStart.Month, dttStart.Day);
                    int intDayIndex = (int)(dttToday - dttRec).TotalDays;
                    if (intDayIndex >= 0 && intDayIndex < intNumDays)
                    {
                        aryDay[intDayIndex].Connections++;
                        TimeSpan spnDuration = (objRec.dttQsoEnd - objRec.dttQsoStart);
                        if (spnDuration > TimeSpan.FromSeconds(1) && spnDuration < TimeSpan.FromDays(1))
                        {
                            aryDay[intDayIndex].ConnectTime += spnDuration;
                        }
                    }
                }
            }
            return aryDay;
        }

        /*-------------------------------------------------------------------------------------------------
         * Get the data for a Frequency-Mode matrix.
         */
        public FrequencyModeData GetFrequencyMode()
        {
            FrequencyModeData objFreqMode = new FrequencyModeData();
            SortedDictionary<double, int> dicFrequency = new SortedDictionary<double, int>();
            SortedDictionary<string, int> dicMode = new SortedDictionary<string, int>();
            lock (Globals.objAdifLock)
            {
                /*
                 * Scan the data and create sorted dictionaries of the frequencies and modes.
                 */
                AdifRecord objRec;
                string strMode;
                foreach (KeyValuePair<string, AdifRecord> kvp in Globals.dicADIF)
                {
                    objRec = kvp.Value;
                    if (!dicFrequency.ContainsKey(objRec.dblFrequency))
                    {
                        /* Add new frequency */
                        dicFrequency.Add(objRec.dblFrequency, 0);
                    }
                    strMode = Globals.MapModeName(objRec.strMode);
                    if (!dicMode.ContainsKey(strMode))
                    {
                        /* Add a new mode */
                        dicMode.Add(strMode, 0);
                    }
                }
                /*
                 * Set up vectors of frequency and mode.
                 */
                objFreqMode.aryFrequency = new double[dicFrequency.Count];
                objFreqMode.aryModes = new string[dicMode.Count];
                int NumFreq = 0;
                int NumModes = 0;
                foreach (KeyValuePair<double, int> kvp in dicFrequency)
                {
                    objFreqMode.aryFrequency[NumFreq++] = kvp.Key;
                }
                foreach (KeyValuePair<string, int> kvp in dicMode)
                {
                    objFreqMode.aryModes[NumModes++] = kvp.Key;
                }
                objFreqMode.NumFreq = NumFreq;
                objFreqMode.NumModes = NumModes;
                /*
                 * Set up the matrix of Frequency-by-mode counts.
                 */
                objFreqMode.aryCount = new int[NumFreq, NumModes];
                for (int i = 0; i < NumFreq; i++)
                {
                    for (int j = 0; j < NumModes; j++)
                    {
                        objFreqMode.aryCount[i, j] = 0;
                    }
                }
                foreach (KeyValuePair<string, AdifRecord> kvp in Globals.dicADIF)
                {
                    objRec = kvp.Value;
                    int ixFreq = Array.IndexOf(objFreqMode.aryFrequency,objRec.dblFrequency);
                    if (ixFreq < 0)
                    {
                        ixFreq = 0;
                    }
                    strMode = Globals.MapModeName(objRec.strMode);
                    int ixMode = Array.IndexOf(objFreqMode.aryModes, strMode);
                    if (ixMode < 0)
                    {
                        ixMode = 0;
                    }
                    objFreqMode.aryCount[ixFreq, ixMode]++;
                }
            }
            /*
             * Finished
             */
            return objFreqMode;
        }

        /*---------------------------------------------------------------------------------------
         * Read the CountryCodes.txt file and set up a list of CountryCodeEntry objects.
         */
        public string ReadCountryCodes()
        {
            /*
             * Try to read the CountryCodes.txt file into a string vector.
             */
            string[] aryCodes;
            if (!File.Exists(Globals.strExecutionDirectory + "CountryCodes.txt"))
            {
                return "CountryCodes.txt file is missing";
            }
            try
            {
                aryCodes = File.ReadAllLines(Globals.strExecutionDirectory + "CountryCodes.txt");
            }
            catch (Exception ex)
            {
                return "Exception reading CountryCodes.txt file: " + ex.Message;
            }
            /*
             * We got the vector of prefixes and country names.
             * Set up a list of CountryCodeEntry items.
             */
            foreach (string strEntry in aryCodes)
            {
                string strCountryLine = strEntry.Trim();
                if (strCountryLine != "")
                {
                    string[] aryTok = strCountryLine.Split('\t');
                    if (aryTok.Length == 2)
                    {
                        string strPrefix = aryTok[0].Trim().Replace('–', '-');
                        string strCountry = aryTok[1];
                        if (strPrefix.Contains("-"))
                        {
                            /* Split up the ranges */
                            string[] aryPrefix = strPrefix.Split('-');
                            if (aryPrefix.Length == 2)
                            {
                                string strNextPrefix = aryPrefix[0];
                                string strLastPrefix = aryPrefix[1];
                                int intCount = 0;
                                while (String.Compare(strNextPrefix, strLastPrefix, comparisonType: StringComparison.OrdinalIgnoreCase) <= 0 && intCount++ < 30)
                                {
                                    CountryCodeEntry objEntry = new CountryCodeEntry();
                                    objEntry.strPrefix = strNextPrefix;
                                    objEntry.strCountry = strCountry;
                                    objEntry.intPrefixLen = objEntry.strPrefix.Length;
                                    Globals.lstCountryCodes.Add(objEntry);
                                    strNextPrefix = IncrementPrefix(strNextPrefix);
                                }
                            }
                            else
                            {
                                Globals.Exceptions("Invalid country code entry: " + strCountryLine);
                            }
                        }
                        else
                        {
                            /* Single prefix */
                            CountryCodeEntry objEntry = new CountryCodeEntry();
                            objEntry.strPrefix = strPrefix;
                            objEntry.strCountry = strCountry;
                            objEntry.intPrefixLen = objEntry.strPrefix.Length;
                            Globals.lstCountryCodes.Add(objEntry);
                        }
                    }
                    else
                    {
                        Globals.Exceptions("Invalid country code entry: " + strCountryLine);
                    }
                }
            }
            /*
             * Sort the list in descending length on country code prefix length (long prefixes first).
             */
            Globals.lstCountryCodes.Sort((x,y) => y.intPrefixLen - x.intPrefixLen);
            /*
             * Finished
             */
            return "";
        }

        /*------------------------------------------------------------------------------------------------------
         * Increment a callsign prefix.
         */
        private string IncrementPrefix(string strPrefix)
        {
            /*
             * Increment the last letter of the prefix.
             */
            int intNumLetters = strPrefix.Length;
            char chrLast = strPrefix[intNumLetters - 1];
            chrLast++;
            string strNewPrefix;
            if (intNumLetters > 1)
            {
                strNewPrefix = strPrefix.Substring(0, intNumLetters - 1) + char.ToString(chrLast);
            }
            else
            {
                strNewPrefix = char.ToString(chrLast);
            }
            return strNewPrefix;
        }

        /*-----------------------------------------------------------------------------------------------
         * Attempt to map a callsign to the associated country.
         */
        public string CallsignToCountry(string strCallsign)
        {
            /*
             * Scan the list of country codes looking for a match.
             */
            foreach (CountryCodeEntry objEntry in Globals.lstCountryCodes)
            {
                if (strCallsign.StartsWith(objEntry.strPrefix))
                {
                    /* We found a matching country entry */
                    return objEntry.strCountry;
                }
            }
            /*
             * We didn't find a matching country entry.
             */
            return "";
        }

        /*---------------------------------------------------------------------------------------------------
         * Get count of connections by country.
         */
        public List<CountryCount> GetCountryCount()
        {
            SortedDictionary<string, CountryCount> dicCountry = new SortedDictionary<string, CountryCount>();
            lock (Globals.objAdifLock)
            {
                foreach (KeyValuePair<string, AdifRecord> kvp in Globals.dicADIF)
                {
                    AdifRecord objAdif = kvp.Value;
                    string strCountry = objAdif.strCountry;
                    if (strCountry == "")
                    {
                        strCountry = "(unknown)";
                    }
                    if (dicCountry.ContainsKey(strCountry))
                    {
                        dicCountry[strCountry].intCount++;
                    }
                    else
                    {
                        CountryCount objCount = new CountryCount();
                        objCount.strCountry = strCountry;
                        objCount.intCount = 1;
                        dicCountry.Add(strCountry, objCount);
                    }
                }
            }
            /*
             * Convert sorted dictionary entries to a list.
             */
            List<CountryCount> lstCountry = new List<CountryCount>();
            foreach (KeyValuePair<string, CountryCount> kvp in dicCountry)
            {
                lstCountry.Add(kvp.Value);
            }
            /*
             * Finished
             */
            return lstCountry;
        }
    }
}