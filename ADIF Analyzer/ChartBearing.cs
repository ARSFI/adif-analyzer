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
    public partial class ChartBearing : Form
    { 
        private long intLastAccession = -1;
        private int intTimerCountdown = 0;
        int intMaxLinesWanted = 2000;
        private List<RangeBearing> lstItems;

        public ChartBearing()
        {
            InitializeComponent();
        }

        /*-----------------------------------------------------------------------
         * This form is being loaded.
         */
        private void ChartBearing_Load(object sender, EventArgs e)
        {
            Globals.RestoreFormPosition("Chart Bearing", this, 450, 420);
            intTimerCountdown = 1;
            timer1.Enabled = true;
        }

        /*------------------------------------------------------------------------
         * A timer tick occurred.
         */
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            if (intTimerCountdown > 0 && --intTimerCountdown <= 0)
            {
                intTimerCountdown = 0;
                UpdateChart();
            }
            timer1.Enabled = true;
            return;
        }

        /*-----------------------------------------------------------------------------
         * Read all ADIF records and draw the chart.
         */
        public void ResetChart()
        {
            intLastAccession = -1;
            lstItems = null;
            UpdateChart();
            return;
        }

        /*--------------------------------------------------------------------------
         * New ADIF record(s) have been added.  Update the chart.
         */
        public void UpdateChart()
        {
            long intNewAccession = Globals.GetLastAccession();
            if (intNewAccession > intLastAccession)
            {
                /* Add new items */
                if (lstItems == null || lstItems.Count == 0)
                {
                    lstItems = Globals.objAdif.GetRangeBearings(intLastAccession, true, true, intMaxLinesWanted);
                }
                else
                {
                    List<RangeBearing> newItems = Globals.objAdif.GetRangeBearings(intLastAccession, true, true, intMaxLinesWanted);
                    lstItems.AddRange(newItems);
                }
                intLastAccession = intNewAccession;
                DrawChart();
            }
            return;
        }

        private void DrawChart()
        {
            /*
             * Display busy cursor.
             */
            this.Cursor = Cursors.WaitCursor;
            /*
             * Draw a point for each contact.
             */
            chart1.Series["Series1"]["PolarDrawingStyle"] = "Marker";
            chart1.Series["Series1"].Points.Clear();
            foreach (RangeBearing objBearing in lstItems)
            {
                double dblRange = objBearing.dblRange;
                if (Globals.blnRangeMiles) dblRange *= Globals.KmToMiles;
                chart1.Series["Series1"].Points.AddXY(objBearing.dblBearing, objBearing.dblRange);
            }
            /*
             * Finished
             */
            this.Cursor = Cursors.Default;
            return;
        }

        /*------------------------------------------------------------------
         * This form is closing.
         */
        private void ChartBearing_FormClosing(object sender, FormClosingEventArgs e)
        {
            Globals.SaveFormPosition("Chart Bearing", this);
            Globals.objMain.ChartBearingClosed();
            return;
        }
        private void ChartBearing_FormClosed(object sender, FormClosedEventArgs e)
        {
            Globals.SaveFormPosition("Chart Bearing", this);
            Globals.objMain.ChartBearingClosed();
            return;
        }

    }
}
