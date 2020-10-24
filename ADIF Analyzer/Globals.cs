/*
 * Globals for Pactor Monitor program.
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Globalization;

namespace ADIF_Analyzer
{
    /*------------------------------------------------------------------------------
     * Store latitude and longitude.
     * South latitudes and west longitudes are negative.
     */
    public class LatitudeLongitude
    {
        public double dblLatitude = 0.0;
        public double dblLongitude = 0.0;
        public bool blnValid;
        public LatitudeLongitude(bool blnValidArg = true)
        {
            blnValid = blnValidArg;
        }
    }

    /*----------------------------------------------------------------------------
     * Store a range and bearing.
     */
    public class RangeBearing
    {
        public double dblRange = 0.0;
        public double dblBearing = 0.0;
        public bool blnValid = false;
    }

    /*------------------------------------------------------------------------------------------
     * Describe connections for some class of item.
     */
     public class ConnectionCountItem
    {
        public string strItem = "";
        public int intCount = 0;
    }

    /*----------------------------------------------------------------------------------------------
     * Global values and methods.
     */
    public static class Globals
    {
        public static MainForm objMain;
        public static string strExecutionDirectory;
        public static string strLogsDirectory;
        public static string strLogFile;
        public static string strExceptionFile;
        public static INIFile objINIFile;
        public static SortedDictionary<string, AdifRecord> dicADIF = new SortedDictionary<string, AdifRecord>();
        public static Dictionary<string, AdifFile> dicAdifFile = new Dictionary<string, AdifFile>();
        public static long intLastRecordAccession = 0;
        public static int TotalRecords = 0;
        public static int TotalFiles = 0;
        public static List<CountryCodeEntry> lstCountryCodes = new List<CountryCodeEntry>();
        public static int intWindowIndex = 0;
        public static AdifPath[] aryAdifPath = {new AdifPath(), new AdifPath(), new AdifPath()};
        public static string strRMSCallsign = "";
        public static LatitudeLongitude objStationLocation = new LatitudeLongitude();
        public static object objLogLock = new object();
        public static object objAdifLock = new object();
        public static AdifProcess objAdif;
        public static bool blnRangeMiles = false;
        public static double DegreesToRadians = 0.017453293;
        public static DateTime dttFullScanTime = DateTime.MaxValue;
        public static DateTime dttNewFileScanTime = DateTime.MaxValue;
        public static bool blnNeedChartUpdate = false;
        public static Boolean blnAutoupdateRestart;
        public static bool blnNeedFullRescan = false;
        public static bool blnAutoupdateTest = false;
        public static bool blnAutoupdateForce = false;
        public static Thread thrUpdate = null;
        public static bool blnLimitADIFStartDate = false;
        public static bool blnLimitADIFEndDate = false;
        public static DateTime dttStartDate;
        public static DateTime dttEndDate;
        public static int intMaxAdifTableEntries = 100;
        public static int intMaxMapContacts = 100;
        public static double KmToMiles = 0.621371;      /* Scale km to miles */
        public static bool blnTimeZoneUTC = false;

        private static Encoding objISOEncoder = Encoding.GetEncoding("iso-8859-1");

        /*----------------------------------------------------------------------------
         * Get the accession number assigned to the last record.
         */
        public static long GetLastAccession()
        {
            return intLastRecordAccession;
        }

        /*----------------------------------------------------------------------------------------------
          * Format an extended time-stamp
          */
        public static string TimeStampEx(bool blnUTC = false)
        {
            if (blnUTC)
            {
                return DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm UTC");
            }
            else
            {
                return DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            }
        }

        /*----------------------------------------------------------------------------------------------
          * Format a date/time value.
          */
        public static string FormatDateTime(DateTime dttDate, bool blnSeconds = false)
        {
            if (blnSeconds)
            {
                return dttDate.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                return dttDate.ToString("yyyy-MM-dd HH:mm");
            }
        }


        /*------------------------------------------------------------------
         * Convert string to byte array.
         */
        public static byte[] StringToByteArray(string strText)
        {
            return objISOEncoder.GetBytes(strText);
        }

#if false
        /*-----------------------------------------------------------------------
         * Report unhandled exception.
         */
        public static void ReportUnhandledException(Exception ex)
        {
            try
            {
                /*
                 * Remove CrLf and noise words from the stack trace.
                 */
                string strTrace = ex.StackTrace.Replace("\n", "$").Replace("   at ", " ");
                /*
                 *  Remove arguments in parentheses.
                 */
                for (int intCount = 1; intCount < 100; intCount++)
                {
                    int intStart = strTrace.IndexOf("(");
                    if (intStart < 0) break;
                    int intEnd = strTrace.IndexOf(")", intStart);
                    if (intEnd < 0) break;
                    strTrace = strTrace.Substring(0, intStart) + strTrace.Substring(intEnd + 1);
                }
                strTrace = strTrace.Replace("  ", " ").Trim();
                /*
                 *  Find start of routines in ADIF Analyzer.
                 */
                int intName = strTrace.IndexOf("ADIF_Analyzer.");
                if (intName > 0)
                {
                    strTrace = strTrace.Substring(intName);
                }
                strTrace = strTrace.Replace("ADIF_Analyzer.", "RE.").Replace("System.Windows.Forms.", "SWF.");
                /*
                 *  Write the information to the logging system.
                 */
                if (strTrace.Length > 200) strTrace = strTrace.Substring(0, 200);
                WriteSysLog("Unhandled exception:" + strTrace.Substring(0, 200), SyslogLib.SyslogSeverity.Error);
                CloseSysLog(5);
            }
            catch
            {
            }
            /*
             *  Finished
             */
            return;
        }
#endif

        /*------------------------------------------------------------------------------------------
         * Writes an entry to the log log.
         */
        public static void LogWrite(string strText)
        {

            lock (objLogLock)
            {
                try
                {
                    File.AppendAllText(strLogsDirectory + "ADIF Analyzer " +
                       DateTime.UtcNow.ToString("yyyyMMdd") + ".log", TimestampEx() + " [" + Application.ProductVersion + "] " + strText.Trim() + "\n");
                }
                catch
                {

                }
            }
        }


        /*------------------------------------------------------------------------------------------
         * Writes an entry to the exception log.
         */
        public static void Exceptions(string strText)
        {

            lock (objLogLock)
            {
                try
                {
                    File.AppendAllText(strLogsDirectory + "ADIF Analyzer Exceptions " +
                       DateTime.UtcNow.ToString("yyyyMMdd") + ".log", TimestampEx() + " [" + Application.ProductVersion + "] " + strText.Trim() + "\n");
                }
                catch
                {

                }
            }
        }

        /*-------------------------------------------------------------------------------------
         * Generate a timestamp yyyymmddhhmm.
         */
        public static string Timestamp()
        {
            string strDateTime = DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm");
            /* Handle localication issue */
            strDateTime = strDateTime.Replace(".", "/");
            return strDateTime;
        }

        /*----------------------------------------------------------------------------------
         * Generate a timestamp with seconds.
         */
        public static string TimestampEx()
        {
            string strDateTime = DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss");
            strDateTime = strDateTime.Replace(".", "/");
            return strDateTime;
        }

        /*-----------------------------------------------------------------------------------------
         * Map a connection mode to the display name.
         */
        public static string MapModeName(string strMode)
        {
            /* Map the mode name */
            string strModeUc = strMode.Trim().ToUpper();
            if (strModeUc == "PAC2" || strModeUc == "PACTOR 2")
            {
                strMode = "Pactor 2";
            }
            else if (strModeUc == "PAC3" || strModeUc == "PACTOR 3")
            {
                strMode = "Pactor 3";
            }
            else if (strModeUc == "PAC4" || strModeUc == "PACTOR 4")
            {
                strMode = "Pactor 4";
            }
            else if (strModeUc == "WINMOR16")
            {
                strMode = "WM 1600";
            }
            else if (strModeUc == "WINMOR5")
            {
                strMode = "WM 500";
            }
            else if (strModeUc == "VARA500")
            {
                strMode = "Vara 500";
            }
            else if (strModeUc == "VARA2300")
            {
                strMode = "Vara 2300";
            }
            else if (strModeUc == "ARDOP2")
            {
                strMode = "ARDOP 200";
            }
            else if (strModeUc == "ARDOP5")
            {
                strMode = "ARDOP 500";
            }
            else if (strModeUc == "ARDOP10")
            {
                strMode = "ARDOP 1000";
            }
            else if (strModeUc == "ARDOP20")
            {
                strMode = "ARDOP 2000";
            }
            else if (strModeUc.Contains("WINMOR"))
            {
                strMode = "Winmor";
            }
            else if (strModeUc.Contains("ARDOP"))
            {
                strMode = "ARDOP";
            }
            else if (strModeUc.Contains("VARA"))
            {
                strMode = "Vara 2300";
            }
            else if (strMode == "")
            {
                strMode = "(unknown)";
            }
            return strMode;
        }

        /*-----------------------------------------------------------------------------------------
         * Subroutine to convert grid square (4 or 6 character)to latitude/longitude
         * Returns a LatitudeLongitude object with blnValid true on success.
         */
        public static LatitudeLongitude GridSquareToDecimalDegrees(string strGridSquare)
            {
            string strUCGridSquare;
            double dblLatitude, dblLongitude;
            LatitudeLongitude objLatLong = new LatitudeLongitude(false);

            /*
             * Validate the grid square.
             */
            string strError = ValidateGridSquare(strGridSquare);
            if (strError != "")
            {
                objLatLong.dblLatitude = 0.0;
                objLatLong.dblLongitude = 0.0;
                //Exceptions("[GridSquareToDecimalDegrees] " + strError + ". Gridsquare = " + strGridSquare, false);
                return objLatLong;
            }
            /*
             * Do the conversion.
             */
            try
            {
                strUCGridSquare = strGridSquare.Trim().ToUpper();
                /*
                 * Verify that first two characters are letters and second two are digits.
                 */
                if (strUCGridSquare.Length < 4)
                {
                    Exceptions("[GridSquareToDecimalDegrees] GridSquare < 4 char. Gridsquare = " + strGridSquare);
                    return objLatLong;
                }
                if (!Char.IsLetter(strUCGridSquare[0]) || !Char.IsLetter(strUCGridSquare[1]) || !Char.IsDigit(strUCGridSquare[2]) || !Char.IsDigit(strUCGridSquare[3]))
                {
                    Exceptions("[GridSquareToDecimalDegrees] Invalid GridSquare. Gridsquare = " + strGridSquare);
                    return objLatLong;
                }
                if (strUCGridSquare.Length > 4)
                {
                    if (strUCGridSquare.Length < 6)
                    {
                        Exceptions("[GridSquareToDecimalDegrees] GridSquare < 6 char. Gridsquare = " + strGridSquare);
                        return objLatLong;
                    }
                    if (!Char.IsLetter(strUCGridSquare[4]) || !Char.IsLetter(strUCGridSquare[5]))
                    {
                        Exceptions("[GridSquareToDecimalDegrees] Invalid GridSquare. Gridsquare = " + strGridSquare);
                        return objLatLong;
                    }
                }
                string strAlpha = "ABCDEFGHIJKLMNOPQRSTUVWX";
                dblLatitude = -100 + (10 * (1 + strAlpha.IndexOf(strUCGridSquare.Substring(1, 1))) + Convert.ToInt32(strUCGridSquare.Substring(3, 1)));
                if (strAlpha.IndexOf(strUCGridSquare.Substring(0, 1)) < 9)
                {
                    /* West longitude no rounding */
                    dblLongitude = -200 + (20 * (1 + strAlpha.IndexOf(strUCGridSquare.Substring(0, 1))) + 2 * Convert.ToInt32(strUCGridSquare.Substring(2, 1)));
                }
                else
                {
                    /* East longitude no rounding */
                    dblLongitude = (20 * (1 + strAlpha.IndexOf(strUCGridSquare.Substring(0, 1))) - 200 + 2 * Convert.ToInt32(strUCGridSquare.Substring(2, 1)));
                }

                if (strUCGridSquare.Length != 6)
                {
                    /* Round to center of grid square */
                    dblLatitude = dblLatitude + 0.5;
                    dblLongitude = dblLongitude + 1;
                }
                else
                {
                    /* 
                     * This processes the 6 char grid square case for more accuracy.
                     * Now add the minutes correction converted to decimal degrees
                     * Should work for both N and S latitude - this checked out 7/28/2002
                     */
                    dblLatitude = dblLatitude + (2.5 * (1 + strAlpha.IndexOf(strUCGridSquare.Substring(5, 1))) - 1.25) / 60;
                    dblLongitude = dblLongitude + (-2.5 + 5 * (1 + strAlpha.IndexOf(strUCGridSquare.Substring(4, 1)))) / 60;
                }
                /*
                 * Successful conversion.
                 */
                objLatLong.dblLatitude = dblLatitude;
                objLatLong.dblLongitude = dblLongitude;
                objLatLong.blnValid = true;
                return objLatLong;
            }
            catch (Exception ex)
            {
                Exceptions("[GridSquareToDecimalDegrees] (" + strGridSquare + ") " + ex.Message);
                dblLatitude = 0.0;
                dblLongitude = 0.0;
                return objLatLong;
            }
        }

        /*---------------------------------------------------------------------------------------------
         * Validate a grid square.  Return an error message if it's not valid.
         */
        public static string ValidateGridSquare(string strGridSquare)
        {
            if (strGridSquare == null || strGridSquare.Trim() == "")
            {
                return "Error: The grid square is missing";
            }
            /* Check the length */
            if (strGridSquare.Length != 6)
            {
                return "Error: The grid square must be 6 characters long.";
            }

            /* First two characters must be letters */
            if (!Char.IsLetter(strGridSquare[0]) || !Char.IsLetter(strGridSquare[1]))
            {
                return "Error: The first two characters of the grid square must be letters";
            }
            /* First two letters must be in the range A through R */
            if (strGridSquare[0] > 'R' || strGridSquare[1] > 'R')
            {
                return "Error: The first two letters of the grid square must be in the range A to R";
            }
            /* Next two characters must be digits */
            if (!Char.IsDigit(strGridSquare[2]) || !Char.IsDigit(strGridSquare[3]))
            {
                return "Error: The third and fourth characters of a grid square must be digits";
            }
            /* Last two characters must be letters */
            if (!Char.IsLetter(strGridSquare[4]) || !Char.IsLetter(strGridSquare[5]))
            {
                return "Error: The last two characters of the grid square must be letters";
            }
            /* Last two letters must be in the range A through X */
            if (strGridSquare[4] > 'X' || strGridSquare[5] > 'X')
            {
                return "Error: The last two letters of the grid square must be in the range A to X";
            }
            /* No error */
            return "";
        }

        /*--------------------------------------------------------------------------------------
         * Subroutine to compute the range and bearing from two latitude/longitude positions
         * Latitude and longitude are in decimal degrees. (not degrees, minutes)
         * +lat = N, -lat = S.  +lon = E, -lon = W
         * lat range -90 to +90   lon range -180 to + 180
         * Computes great circle route (shortest route).
         * Bearing is in True degrees (0-360)
         * Range in nautical miles
         * Equations from Bowditch Practical Navigator page 1304
         * Written by Rick Muething
         */
        public static RangeBearing RangeAndBearing(LatitudeLongitude objFromLocation, LatitudeLongitude objToLocation, bool blnRangeInMiles = false)
        {
            RangeBearing objResult = new RangeBearing();
            try
            {
                double dblFromLatitude = objFromLocation.dblLatitude;
                double dblFromLongitude = objFromLocation.dblLongitude;
                double dblToLatitude = objToLocation.dblLatitude;
                double dblToLongitude = objToLocation.dblLongitude;
                objResult.dblRange = 1000;
                objResult.dblBearing = 0.0;
                double dblLO;
                double dblRangeCos;
                double dblRad;
                double dblDenom;
                double dblRange;
                double dblBearing;

                /* First compute difference in Longitude dblLO (must be 180 or less. + for E, - for W) */
                dblLO = dblToLongitude - dblFromLongitude;
                if (dblLO < -180)
                {
                    dblLO += 360;
                }
                else if (dblLO > 180)
                {
                    dblLO -= 360;
                }

                /* Now compute distance (60 nautical miles per degree) */
                dblRad = Math.PI / 180; // radians per degree
                dblRangeCos = (Math.Sin(dblFromLatitude * dblRad) * Math.Sin(dblToLatitude * dblRad)) + (Math.Cos(dblFromLatitude * dblRad) *
                    Math.Cos(dblToLatitude * dblRad) * Math.Cos(dblLO * dblRad));

                /* Since VB does not have an Arc Cos function use the identy tan(x) = sin(x)/cos(x) and use Atn function */
                if (dblRangeCos > 0)
                {
                    /* Range is within +/- 90 deg */
                    dblRange = (60 * 180 / Math.PI) * Math.Atan(Math.Sqrt((1 - (dblRangeCos * dblRangeCos))) / dblRangeCos);
                }
                else
                {
                    /* Range is > 90 deg or < -90 deg */
                    dblRange = 60 * (90 + ((180 / Math.PI) * ((Math.PI / 2) - Math.Atan(Math.Sqrt((1 - (dblRangeCos * dblRangeCos))) / -dblRangeCos))));
                }

                /* Scales nautical miles to kilometers */
                dblRange *= 1.61 * 1.15;

                /* And now compute great circle bearing: From to To */
                dblDenom = ((Math.Cos(dblFromLatitude * dblRad) * Math.Tan(dblToLatitude * dblRad)) -
                    (Math.Sin(dblFromLatitude * dblRad) * Math.Cos(dblLO * dblRad)));

                /* Compute the normal bearing calculation using  4 Quadrant  ATAN2 */
                /* This also handles the error case when Denom = 0 */
                dblBearing = (180 / Math.PI) * Math.Atan2(Math.Sin(dblLO * dblRad), dblDenom);

                /* Convert bearing to postive if its a - value */
                if (dblBearing < 0) dblBearing += 360;

                /* See if we need to convert the range from km to miles */
                if (blnRangeInMiles) dblRange *= KmToMiles;

                objResult.dblRange = dblRange;
                objResult.dblBearing = dblBearing;
                objResult.blnValid = true;
            }
            catch
            {
                objResult.dblRange = 0;
                objResult.dblBearing = 0;
                objResult.blnValid = false;
            }
            return objResult;
        }

        public static double ComputeRange(LatitudeLongitude objFromLocation, LatitudeLongitude objToLocation)
        {
            double dblRange = 0.0;
            try
            {
                double dblFromLatitude = objFromLocation.dblLatitude;
                double dblFromLongitude = objFromLocation.dblLongitude;
                double dblToLatitude = objToLocation.dblLatitude;
                double dblToLongitude = objToLocation.dblLongitude;
                double dblLO;
                double dblRangeCos;
                double dblRad;

                /* First compute difference in Longitude dblLO (must be 180 or less. + for E, - for W) */
                dblLO = dblToLongitude - dblFromLongitude;
                if (dblLO < -180)
                {
                    dblLO += 360;
                }
                else if (dblLO > 180)
                {
                    dblLO -= 360;
                }

                /* Now compute distance (60 nautical miles per degree) */
                dblRad = Math.PI / 180; // radians per degree
                dblRangeCos = (Math.Sin(dblFromLatitude * dblRad) * Math.Sin(dblToLatitude * dblRad)) + (Math.Cos(dblFromLatitude * dblRad) *
                    Math.Cos(dblToLatitude * dblRad) * Math.Cos(dblLO * dblRad));

                /* Since VB does not have an Arc Cos function use the identy tan(x) = sin(x)/cos(x) and use Atn function */
                if (dblRangeCos > 0)
                {
                    /* Range is within +/- 90 deg */
                    dblRange = (60 * 180 / Math.PI) * Math.Atan(Math.Sqrt((1 - (dblRangeCos * dblRangeCos))) / dblRangeCos);
                }
                else
                {
                    /* Range is > 90 deg or < -90 deg */
                    dblRange = 60 * (90 + ((180 / Math.PI) * ((Math.PI / 2) - Math.Atan(Math.Sqrt((1 - (dblRangeCos * dblRangeCos))) / -dblRangeCos))));
                }

                /* Scales nautical miles to kilometers */
                dblRange *= 1.61 * 1.15;
            }
            catch
            {
                dblRange = 0.0;
            }
            return dblRange;
        }

        public static double ComputeBearing(LatitudeLongitude objFromLocation, LatitudeLongitude objToLocation)
        {
            double dblBearing = 0.0;
            try
            {
                double dblFromLatitude = objFromLocation.dblLatitude;
                double dblFromLongitude = objFromLocation.dblLongitude;
                double dblToLatitude = objToLocation.dblLatitude;
                double dblToLongitude = objToLocation.dblLongitude;
                double dblLO;
                double dblRad;
                double dblDenom;

                /* First compute difference in Longitude dblLO (must be 180 or less. + for E, - for W) */
                dblLO = dblToLongitude - dblFromLongitude;
                if (dblLO < -180)
                {
                    dblLO += 360;
                }
                else if (dblLO > 180)
                {
                    dblLO -= 360;
                }
                /* And now compute great circle bearing: From to To */
                dblRad = Math.PI / 180; // radians per degree
                dblDenom = ((Math.Cos(dblFromLatitude * dblRad) * Math.Tan(dblToLatitude * dblRad)) -
                    (Math.Sin(dblFromLatitude * dblRad) * Math.Cos(dblLO * dblRad)));

                /* Compute the normal bearing calculation using  4 Quadrant  ATAN2 */
                /* This also handles the error case when Denom = 0 */
                dblBearing = (180 / Math.PI) * Math.Atan2(Math.Sin(dblLO * dblRad), dblDenom);

                /* Convert bearing to postive if its a - value */
                if (dblBearing < 0) dblBearing += 360;
            }
            catch
            {
                dblBearing = 0.0;
            }
            return dblBearing;
        }

        /*-----------------------------------------------------------------------------------------------
         * Convert a date-time value to the format yyyyMMddHHmmss.
         */
        public static string FormatNetworkDate(DateTime dttDateTime)
        {
            return dttDateTime.ToString("yyyyMMddHHmmss");
        }

        /*----------------------------------------------------------
         * Parse a network date in the format yyyyMMddHHmmss.
         */
        public static DateTime ParseNetworkDate(string strDate)
        {
            DateTime dttDefault = new DateTime(1999, 1, 1);
            DateTime dttDate;
            if (strDate == null || strDate.Length != 14)
            {
                return dttDefault;
            }
            CultureInfo provider = CultureInfo.InvariantCulture;
            try
            {
                dttDate = DateTime.ParseExact(strDate, "yyyyMMddHHmmss", provider);
            }
            catch
            {
                return dttDefault;
            }
            return dttDate;
        }
 
        /*-----------------------------------------------------------------------------------
         * Save the size and position of a form.
         */
        public static void SaveFormPosition(string strName, Form objForm)
        {
            if (objForm.WindowState == FormWindowState.Normal && objINIFile != null)
            {
                if (objForm.Top >= 0) objINIFile.WriteInteger(strName, "Top", objForm.Top);
                if (objForm.Left >= 0) objINIFile.WriteInteger(strName, "Left", objForm.Left);
                if (objForm.Height > 0) objINIFile.WriteInteger(strName, "Height", objForm.Height);
                if (objForm.Width > 0) objINIFile.WriteInteger(strName, "Width", objForm.Width);
            }
            return;
        }

        /*------------------------------------------------------------------------------------
         * Restore the position of a form from the ini file.
         */
        public static void RestoreFormPosition(string strName, Form objForm, int intDefWidth=400, int intDefHeight=300, bool blnMainWindow = false)
        {
            int tmpTop;
            int tmpLeft;
            int tmpWidth;
            int tmpHeight;
            if (blnMainWindow)
            {
                tmpTop = objINIFile.GetInteger(strName, "Top", 200);
                tmpLeft = objINIFile.GetInteger(strName, "Left", 200);
                tmpWidth = objINIFile.GetInteger(strName, "Width", 550);
                tmpHeight = objINIFile.GetInteger(strName, "Height", 500);
            }
            else
            {
                tmpTop = objINIFile.GetInteger(strName, "Top", 100);
                tmpLeft = objINIFile.GetInteger(strName, "Left", -1);
                if (tmpLeft < 0) tmpLeft = 500 + intWindowIndex++ + 30;
                tmpWidth = objINIFile.GetInteger(strName, "Width", intDefWidth);
                tmpHeight = objINIFile.GetInteger(strName, "Height", intDefHeight);
            }
            if (tmpTop < 0) tmpTop = 100;
            if (tmpLeft < 0) tmpLeft = 100;
            if (tmpWidth < 100) tmpWidth = 900;
            if (tmpHeight < 100) tmpHeight = 400;
            /*
             * Check that the window will end up on one of the available screens
             */
            System.Windows.Forms.Screen[] screen = System.Windows.Forms.Screen.AllScreens;
            for (int i = 0; i < screen.Length; i++)
            {
                if (screen[i].Bounds.Top <= tmpTop &&
                   screen[i].Bounds.Bottom >= (tmpTop + tmpHeight) &&
                   screen[i].Bounds.Left <= tmpLeft &&
                   screen[i].Bounds.Right >= (tmpLeft + tmpWidth))
                    {
                    /*
                     * Position window in its last location only if it is within the bounds of the screen
                     */
                    objForm.Top = tmpTop;
                    objForm.Left = tmpLeft;
                    if (!blnMainWindow)
                    {
                        objForm.Width = tmpWidth;
                        objForm.Height = tmpHeight;
                    }
                    break;
                }
            }
            return;
        }
    }
}
