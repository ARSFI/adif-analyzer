using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

/*----------------------------------------------------------------------------
 * Display a chart showing the range of connections.
 */
namespace ADIF_Analyzer
{

    public partial class ChartRange : Form
    {
        private long intLastAccession = -1;
        int intMaxLinesWanted = 50000;
        private List<RangeBearing> lstItems;
        private int intTimerCountdown = 1;

        public ChartRange()
        {
            InitializeComponent();
        }

        /*----------------------------------------------------------------------
         * This form is being loaded.
         */
        private void ChartRange_Load(object sender, EventArgs e)
        {
            Globals.RestoreFormPosition("Chart Range", this, 550, 300);
            if (Globals.blnRangeMiles)
            {
                this.Text = "Range of connections (miles)";
            }
            else
            {
                this.Text = "Range of connections (km)";
            }
            intTimerCountdown = 1;
            timer1.Enabled = true;
            return;
        }

        /*-----------------------------------------------------------------------
         * This form is closing.
         */
        private void ChartRange_FormClosing(object sender, FormClosingEventArgs e)
        {
            Globals.SaveFormPosition("Chart Range", this);
            Globals.objMain.ChartRangeClosed();
            return;
        }

        /*----------------------------------------------------------------------
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
        public void ResetChart(long intNewAccession)
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
            intLastAccession = -1;
            long intNewAccession = Globals.GetLastAccession();
            if (intNewAccession > intLastAccession)
            {
                /* Add new items */
                if (lstItems == null || lstItems.Count == 0)
                {
                    lstItems = Globals.objAdif.GetRangeBearings(intLastAccession, true, false, intMaxLinesWanted);
                }
                else
                {
                    List<RangeBearing> newItems = Globals.objAdif.GetRangeBearings(intLastAccession, true, false, intMaxLinesWanted);
                    lstItems.AddRange(newItems);
                }
                intLastAccession = intNewAccession;
                if (Globals.blnRangeMiles)
                {
                    DrawChartMiles();
                }
                else
                {
                    DrawChartKm();
                }
            }
            return;
        }

        /*---------------------------------------------------------------------------------------------
         * Draw the full chart with km units.
         */
        private void DrawChartKm()
        {
            int[] aryBars = new int[11];
            /*
             * Create histogram of connections by range.
             */
            chart1.Series["Range"].Points.Clear();
            double dblMaxRange = 0.0;
            int index;
            foreach (RangeBearing objRange in lstItems)
            {
                double dblRange = objRange.dblRange;
                if (dblRange > dblMaxRange) dblMaxRange = dblRange;
                if (dblRange <= 200)
                {
                    aryBars[0]++;
                }
                else if (dblRange <= 500)
                {
                    aryBars[1]++;
                }
                else if (dblRange <= 1000)
                {
                    aryBars[2]++;
                }
                else if (dblRange <= 2000)
                {
                    aryBars[3]++;
                }
                else if (dblRange <= 3000)
                {
                    aryBars[4]++;
                }
                else if (dblRange <= 4000)
                {
                    aryBars[5]++;
                }
                else if (dblRange <= 6000)
                {
                    aryBars[6]++;
                }
                else if (dblRange <= 8000)
                {
                    aryBars[7]++;
                }
                else
                {
                    aryBars[8]++;
                }
            }
            /*
             * Define the bar labels and heights.
             */
            index = 0;
            chart1.Series["Range"].Points.Clear();
            chart1.Series["Range"].Points.AddXY("200",aryBars[index++]);
            chart1.Series["Range"].Points.AddXY("500", aryBars[index++]);
            chart1.Series["Range"].Points.AddXY("1000", aryBars[index++]);
            chart1.Series["Range"].Points.AddXY("2000", aryBars[index++]);
            chart1.Series["Range"].Points.AddXY("3000", aryBars[index++]);
            if (dblMaxRange > 3000)
            {
                chart1.Series["Range"].Points.AddXY("4000", aryBars[index++]);
                if (dblMaxRange > 4000)
                {
                    chart1.Series["Range"].Points.AddXY("6000", aryBars[index++]);
                    if (dblMaxRange > 6000)
                    {
                        chart1.Series["Range"].Points.AddXY("8000", aryBars[index++]);
                        if (dblMaxRange > 8000)
                        {
                            chart1.Series["Range"].Points.AddXY(">8000", aryBars[index++]);
                        }
                    }
                }
            }
            /*
             * Finished
             */
            return;
        }

        /*---------------------------------------------------------------------------------------------
         * Draw the full chart with miles units.
         */
        private void DrawChartMiles()
        {
            int[] aryBars = new int[11];
            /*
             * Create histogram of connections by range.
             */
            double dblMaxRange = 0.0;
            int index;
            chart1.Series["Range"].Points.Clear();
            foreach (RangeBearing objRange in lstItems)
            {
                double dblRange = Globals.KmToMiles * objRange.dblRange;
                if (dblRange > dblMaxRange) dblMaxRange = dblRange;
                if (dblRange <= 100)
                {
                    aryBars[0]++;
                }
                else if (dblRange <= 500)
                {
                    aryBars[1]++;
                }
                else if (dblRange <= 1000)
                {
                    aryBars[2]++;
                }
                else if (dblRange <= 1500)
                {
                    aryBars[3]++;
                }
                else if (dblRange <= 2000)
                {
                    aryBars[4]++;
                }
                else if (dblRange <= 3000)
                {
                    aryBars[5]++;
                }
                else if (dblRange <= 4000)
                {
                    aryBars[6]++;
                }
                else if (dblRange <= 5000)
                {
                    aryBars[7]++;
                }
                else
                {
                    aryBars[8]++;
                }
            }
            /*
             * Define the bar labels and heights.
             */
            index = 0;
            chart1.Series["Range"].Points.AddXY("100", aryBars[index++]);
            chart1.Series["Range"].Points.AddXY("500", aryBars[index++]);
            chart1.Series["Range"].Points.AddXY("1000", aryBars[index++]);
            chart1.Series["Range"].Points.AddXY("1500", aryBars[index++]);
            chart1.Series["Range"].Points.AddXY("2000", aryBars[index++]);
            if (dblMaxRange > 2000)
            {
                chart1.Series["Range"].Points.AddXY("3000", aryBars[index++]);
                if (dblMaxRange > 3000)
                {
                    chart1.Series["Range"].Points.AddXY("4000", aryBars[index++]);
                    if (dblMaxRange > 4000)
                    {
                        chart1.Series["Range"].Points.AddXY("5000", aryBars[index++]);
                        if (dblMaxRange > 5000)
                        {
                            chart1.Series["Range"].Points.AddXY(">5000", aryBars[index++]);
                        }
                    }
                }
            }
            /*
             * Finished
             */
            return;
        }
    }
}
