using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using System.IO;

namespace ADIF_Analyzer
{
    public class INIFile
    {
        static object objINIFileLock = new object();
        private string strFilePath;
        private bool blnReadOnly = false;

        private Dictionary<string, Dictionary<string, string>> dicSections = new Dictionary<string, Dictionary<string, string>>();
        public INIFile(bool blnReadOnlyArg = false)
        {
            blnReadOnly = blnReadOnlyArg; 
            strFilePath = Globals.strExecutionDirectory + "ADIF Analyzer.ini";
        }

        ~INIFile()
        { 
            Flush();
        }

        public void DeleteSection(string strSection)
        {
            //
            // Deletes a section from an .ini file along with all associated Keys
            //
            if (blnReadOnly) return;
            lock (objINIFileLock)
            {
                strSection = FindSection(strSection);
                if (strSection != null)
                {
                    dicSections.Remove(strSection);
                    Flush();
                }
            }
        }
        // End DeleteSection

        public void DeleteKey(string strSection, string strKey)
        {
            //
            // Deletes a key from the desired section
            //
            lock (objINIFileLock)
            {
                strSection = FindSection(strSection);
                if (strSection != null)
                {
                    strKey = FindKey(strSection, strKey);
                    if (strKey != null)
                    {
                        dicSections[strSection].Remove(strKey);
                        Flush();
                    }
                }
            }
        }
        // End DeleteKey

        public bool Load(string strPath = "")
        {
            //
            // Load the .ini file parameters
            //
            if (strPath != "") strFilePath = strPath;
            lock (objINIFileLock)
            {
                dicSections.Clear();
                //
                // Add a common 'Main' section at the top of the INI file
                //
                dicSections.Add("Main", new Dictionary<string, string>());

                if (File.Exists(strFilePath))
                {
                    string[] strContent = null;
                    string strLine = null;
                    string strCurrentSection = "";

                    try
                    {
                        strContent = File.ReadAllLines(strFilePath);
                        //
                        // Loop through all the lines inthe ini file
                        //
                        for (int i = 0; i <= strContent.Length - 1; i++)
                        {
                            strLine = strContent[i].Trim();
                            if (strLine.StartsWith("[") & strLine.EndsWith("]") & strLine.Length > 2)
                            {
                                //
                                // Found a section header
                                //
                                strLine = strLine.Replace("[", "").Replace("]", "");

                                if (FindSection(strLine) == null)
                                {
                                    //
                                    // The section doesn't exist, so create it.
                                    //
                                    dicSections.Add(strLine, new Dictionary<string, string>());
                                }
                                strCurrentSection = strLine;
                            }
                            else
                            {
                                string[] strTokens = strLine.Split('=');
                                if (strTokens.Length == 2)
                                {
                                    strTokens[0] = strTokens[0].Trim();
                                    strTokens[1] = strTokens[1].Trim();
                                    if (!string.IsNullOrEmpty(strTokens[0]))
                                    {
                                        //
                                        // Save the key.  If the key already exists, it is overwritten
                                        //
                                        WriteRecord(strCurrentSection, strTokens[0], strTokens[1]);
                                    }
                                }
                            }
                        }
                    }
                    catch 
                    {
                        //Logs.WriteException("[INIFile.Load] " + Err().Description);
                        return false;
                    }
                    return true;
                }
                else
                {
                    /* Specified .ini file does not exist */
                    return false;
                }
            }
        }
        // End Load

        public void Flush()
        {
            //
            // Flush the contents of the structure back to the file
            //
            if (blnReadOnly) return;
            StringBuilder sbdContent = new StringBuilder();
            foreach (string strSection in dicSections.Keys)
            {
                sbdContent.AppendLine("\n[" + strSection + "]");
                foreach (string strKey in dicSections[strSection].Keys)
                {
                    string strValue = dicSections[strSection][strKey];
                    sbdContent.AppendLine(strKey + "=" + strValue);
                }
            }
            try
            {
                File.WriteAllText(strFilePath, sbdContent.ToString());
            }
            catch 
            {
                //Logs.WriteException("[INIFile.Flush] " + ex.Message);
            }
        }
        // End Flush

        /*------------------------------------------------------------------------------------------
         * Get a list of the section names.
         */
        public List<string> GetSectionList()
        {
            List<string> lstSections = new List<string>();
            foreach (KeyValuePair<string, Dictionary<string, string>> kvp in dicSections)
            {
                lstSections.Add(kvp.Key);
            }
            return lstSections;
        }

        //
        // Read routines
        //
        public bool GetBoolean(string strSection, string strKey, bool blnDefault)
        {
            bool blnResult = false;
            try
            {
                blnResult = Convert.ToBoolean(GetRecord(strSection, strKey, blnDefault.ToString()));
            }
            catch
            {
                blnResult = blnDefault;
            }
            return blnResult;
        }
        // End GetBoolean

        public int GetInteger(string strSection, string strKey, int intDefault)
        {
            int intResult = 0;
            try
            {
                intResult = Convert.ToInt32(GetRecord(strSection, strKey, intDefault.ToString()));
            }
            catch
            {
                intResult = intDefault;
            }
            return intResult;
        }
        // End GetInteger

        public double GetDouble(string strSection, string strKey, double dblDefault)
        {
            double dblResult = 0;
            try
            {
                dblResult = Convert.ToDouble(GetRecord(strSection, strKey, dblDefault.ToString()));
            }
            catch
            {
                dblResult = dblDefault;
            }
            return dblResult;
        }
        // End GetInteger

        public string GetString(string strSection, string strKey, string strDefault)
        {
            return GetRecord(strSection, strKey, strDefault);
        }
        // End GetString

        public DateTime GetDateTime(string strSection, string strKey, DateTime dttDefaultDate)
        {
            string strDate = GetRecord(strSection, strKey, "");
            if (strDate.Length != 14) return dttDefaultDate;
            return Globals.ParseNetworkDate(strDate);
        }

        private string GetRecord(string strSection, string strKey, string strDefault)
        {
            string functionReturnValue = null;
            //
            // Common read funtion
            //
            functionReturnValue = strDefault;
            lock (objINIFileLock)
            {
                strSection = FindSection(strSection);
                if (strSection != null)
                {
                    strKey = FindKey(strSection, strKey);
                    if (strKey != null)
                    {
                        if (dicSections[strSection][strKey].Length > 0)
                        {
                            functionReturnValue = dicSections[strSection][strKey];
                        }
                    }
                }
            }
            return functionReturnValue;
        }
        // End GetRecord

        //
        // Write routines
        //
        public void WriteBoolean(string strSection, string strKey, bool blnValue)
        {
            WriteRecord(strSection, strKey, blnValue.ToString());
        }
        // End WriteBoolean

        public void WriteInteger(string strSection, string strKey, int intValue)
        {
            WriteRecord(strSection, strKey, intValue.ToString());
        }
        //End WriteInteger
        public void WriteDouble(string strSection, string strKey, double dblValue)
        {
            WriteRecord(strSection, strKey, dblValue.ToString());
        }

        public void WriteDateTime(string strSection, string strKey, DateTime dttDate)
        {
            WriteRecord(strSection, strKey, Globals.FormatNetworkDate(dttDate));
            return;
        }

        public void WriteString(string strSection, string strKey, string strValue)
        {
            WriteRecord(strSection, strKey, strValue);
        }
        // End WriteString

        private void WriteRecord(string strSection, string strKey, string strValue)
        {
            //
            // Common write routine
            //
            string strExactSection = null;
            string strExactKey = null;
            lock (objINIFileLock)
            {
                //
                // Locate the exact section name
                //
                strExactSection = FindSection(strSection);
                if (strExactSection == null)
                {
                    strExactSection = strSection;
                    dicSections.Add(strExactSection, new Dictionary<string, string>());
                }
                //
                // Locate the exact key name
                //
                strExactKey = FindKey(strExactSection, strKey);
                if (strExactKey == null)
                {
                    strExactKey = strKey;
                    dicSections[strExactSection].Add(strExactKey, strValue);
                }
                else
                {
                    Dictionary<string, string> dicRecord = dicSections[strExactSection];
                    dicRecord.Remove(strExactKey);
                    dicRecord.Add(strExactKey, strValue);
                }
                Flush();
            }
        }
        // End WriteRecord

        //
        // Helper routines to locate section and key names in a case-insensitive manner.
        //
        private string FindSection(string strSection)
        {
            string functionReturnValue = null;
            //
            // Return the key of the desired section if it exists, otherwise Null
            //
            functionReturnValue = null;
            strSection = strSection.ToLower();
            foreach (KeyValuePair<string, Dictionary<string, string>> dicObj in dicSections)
            {
                if (dicObj.Key.ToLower() == strSection)
                {
                    functionReturnValue = dicObj.Key;
                    return functionReturnValue;
                }
            }
            return functionReturnValue;
        }

        private string FindKey(string strSection, string strKey)
        {
            string functionReturnValue = null;
            //
            // Return the key of the desired section if it exists, otherwise Null
            //
            functionReturnValue = null;
            strKey = strKey.ToLower();
            strSection = FindSection(strSection);
            if (strSection != null)
            {
                Dictionary<string, string> dicObj = dicSections[strSection];
                foreach (KeyValuePair<string, string> keyObj in dicObj)
                {
                    if (keyObj.Key.ToLower() == strKey)
                    {
                        functionReturnValue = keyObj.Key;
                        return functionReturnValue;
                    }
                }
            }
            return functionReturnValue;
        }
    }
}
