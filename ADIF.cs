using System;
using System.IO;
using System.Threading;

/*----------------------------------------------------------------------------------------
 * Class describing an ADIF entry.
 */
public class AdifRecord
{
    public string strConnectedCallsign;
    public DateTime dttQsoStart;
    public DateTime dttQsoEnd;
    public string strMode;
    public string strGridSquare;
    public string strBand;
    public double dblFrequency;
    public string strComment;

	public Class1()
	{
	}
}

/*-----------------------------------------------------------------------------
 * Try to read an ADIF file, parse the records, and add them to dicADIF.
 * Return an empty string on success or an error message.
 */
public string ReadAdifFile(string strFileSpec)
{
    /*
     * Attempt to read the entire ADIF file.
     */
    string strError = "";
    string[] aryLines;
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
        return strError;
    }
    /*
     * We read the file.
     * Parse each line and addrecords to dicADIF.
     */
     foreach (string strLine in aryLines)
     {
        ParseAdifRecord(strLine);

     }
    /*
     * Finished
     */
    return "";
}