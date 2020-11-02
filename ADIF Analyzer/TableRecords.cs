using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ADIF_Analyzer
{
    public partial class TableRecords : Form
    {
        private int intTimerCountdown = 1;

        public TableRecords()
        {
            InitializeComponent();
            /* Set custom sort routine */
            dataGridView1.SortCompare += new DataGridViewSortCompareEventHandler(
            this.dataGridView1_SortCompare);
        }

        /*-----------------------------------------------------------------------------------
         * This form is being loaded.
         */
        private void TableRecords_Load(object sender, EventArgs e)
        {
            Globals.RestoreFormPosition("Table Records", this, 657, 280);
            intTimerCountdown = 1;
            timer1.Enabled = true;
            return;
        }

        /*------------------------------------------------------------------------------
         * This form is closing.
         */
        private void TableRecords_FormClosing(object sender, FormClosingEventArgs e)
        {
            Globals.SaveFormPosition("Table Records", this);
            Globals.objMain.TableRecordsClosed();
            return;
        }

        /*--------------------------------------------------------------------
         * A timer tick occurred.
         */
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            if (intTimerCountdown > 0 && --intTimerCountdown <= 0)
            {
                intTimerCountdown = 0;
                FillTable();
            }
            timer1.Enabled = true;
            return;
        }

        /*--------------------------------------------------------------------------
         * New data has arrived.  Update the table.
         */
        public void UpdateTable()
        {
            intTimerCountdown = 1;
            return;
        }

        /*------------------------------------------------------------------------------
         * Fill the table with entries.
         */
        private void FillTable()
        {
            this.Cursor = Cursors.WaitCursor;
            dataGridView1.Rows.Clear();
            /*
             * Get a list of recent ADIF records.
             */
            List<AdifRecord> lstRec = Globals.objAdif.GetRecentAdifRecords(Globals.intMaxAdifTableEntries);
            /*
             * Populate the table.
             */
            foreach (AdifRecord objRec in lstRec)
            {
                DataGridViewRow objRow = new DataGridViewRow();
                objRow.CreateCells(dataGridView1);
                if (Globals.blnTimeZoneUTC)
                {
                    objRow.Cells[ColIndex("StartTime")].Value = objRec.dttQsoStart.ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    DateTime dttQsoStart = objRec.dttQsoStart;
                    dttQsoStart = dttQsoStart.ToLocalTime();
                    objRow.Cells[ColIndex("StartTime")].Value = objRec.dttQsoStart.ToString("yyyy-MM-dd HH:mm:ss");
                }
                TimeSpan Duration = objRec.dttQsoEnd - objRec.dttQsoStart;
                objRow.Cells[ColIndex("Duration")].Value = Duration.ToString("c");
                objRow.Cells[ColIndex("LastCommand")].Value = objRec.strLastCommand;
                objRow.Cells[ColIndex("Callsign")].Value = objRec.strConnectedCallsign;
                objRow.Cells[ColIndex("Mode")].Value = Globals.MapModeName(objRec.strMode);
                objRow.Cells[ColIndex("Frequency")].Value = objRec.dblFrequency.ToString("0.0");
                objRow.Cells[ColIndex("Band")].Value = objRec.strBand;
                if (objRec.strGridSquare != "")
                {
                    RangeBearing objRange = Globals.RangeAndBearing(Globals.objStationLocation, objRec.Location, Globals.blnRangeMiles);
                    if (objRange.blnValid)
                    {
                        objRow.Cells[ColIndex("Range")].Value = ((int)(0.5 + objRange.dblRange)).ToString();
                        objRow.Cells[ColIndex("Bearing")].Value = ((int)(0.5 + objRange.dblBearing)).ToString();
                    }
                    else
                    {
                        objRow.Cells[ColIndex("Range")].Value = "";
                        objRow.Cells[ColIndex("Bearing")].Value = "";
                    }
                }
                else
                {
                    objRow.Cells[ColIndex("Range")].Value = "";
                    objRow.Cells[ColIndex("Bearing")].Value = "";
                }
                objRow.Cells[9].Value = objRec.strCountry;
                dataGridView1.Rows.Add(objRow);
            }
            /* Sort the rows in descending order of start date/time */
            dataGridView1.Columns[0].HeaderCell.SortGlyphDirection = SortOrder.Descending;
            this.Cursor = Cursors.Default;
            return;
        }

        /*---------------------------------------------------------------------------
         * Custom sort compare routine.
         */
        private void dataGridView1_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Name == "Range" || e.Column.Name == "Bearing")
            {
                e.SortResult = Globals.CellValue(e.CellValue1) - Globals.CellValue(e.CellValue2);
            }
            else if (e.Column.Name == "Frequency")
            {
                e.SortResult = (int)(Globals.CellValueDouble(e.CellValue1) - Globals.CellValueDouble(e.CellValue2));
            }
            else
            {
                e.SortResult = System.String.Compare(
                    e.CellValue1.ToString(), e.CellValue2.ToString());
            }
            e.Handled = true;
            return;
        }

        /*--------------------------------------------------------------------------------
         * Convert the name of a column to its index number.
         */
        private int ColIndex(string strName)
        {
            return dataGridView1.Columns[strName].Index;
        }
    }
}
