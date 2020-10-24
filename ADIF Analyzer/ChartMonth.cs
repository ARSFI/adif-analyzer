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
    public partial class ChartMonth : Form
    {
        private int intTimerCountdown = 1;

        public ChartMonth()
        {
            InitializeComponent();
        }

        /*----------------------------------------------------------------
         * This form is being loaded.
         */
        private void ChartMonth_Load(object sender, EventArgs e)
        {
            Globals.RestoreFormPosition("Chart Month", this, 780, 280);
            intTimerCountdown = 1;
            timer1.Enabled = true;
            return;
        }

        /*-----------------------------------------------------------------------
         * This form is closing.
         */
        private void ChartMonth_FormClosing(object sender, FormClosingEventArgs e)
        {
            Globals.SaveFormPosition("Chart Month", this);
            Globals.objMain.ChartMonthClosed();
            return;
        }

        /*--------------------------------------------------------------------------
         * New data has arrived.  Update the chart.
         */
        public void UpdateChart()
        {
            intTimerCountdown = 1;
            return;
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
                DrawChart();
            }
            timer1.Enabled = true;
            return;
        }

        /*-----------------------------------------------------------------------------
         * Read all ADIF records and draw the chart.
         */
        public void ResetChart(long intNewAccession)
        {
            DrawChart();
            return;
        }

        /*---------------------------------------------------------------------------------------------
        * Draw the full chart.
        */
        private void DrawChart()
        {
            /*
             * Define the bars.
             */
            chart1.ChartAreas["ChartArea1"].AxisX.Interval = 1;
            int[] aryCounts = Globals.objAdif.ConnectionsByMonth();
            chart1.Series["Month"].Points.Clear();
            chart1.Series["Month"].Points.AddXY("Jan", aryCounts[0]);
            chart1.Series["Month"].Points.AddXY("Feb", aryCounts[1]);
            chart1.Series["Month"].Points.AddXY("Mar", aryCounts[2]);
            chart1.Series["Month"].Points.AddXY("Apr", aryCounts[3]);
            chart1.Series["Month"].Points.AddXY("May", aryCounts[4]);
            chart1.Series["Month"].Points.AddXY("Jun", aryCounts[5]);
            chart1.Series["Month"].Points.AddXY("Jul", aryCounts[6]);
            chart1.Series["Month"].Points.AddXY("Aug", aryCounts[7]);
            chart1.Series["Month"].Points.AddXY("Sep", aryCounts[8]);
            chart1.Series["Month"].Points.AddXY("Oct", aryCounts[9]);
            chart1.Series["Month"].Points.AddXY("Nov", aryCounts[10]);
            chart1.Series["Month"].Points.AddXY("Dec", aryCounts[11]);
            /*
             * Finished
             */
            return;
        }
    }
}
