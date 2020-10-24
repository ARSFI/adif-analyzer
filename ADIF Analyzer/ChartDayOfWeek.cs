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
    public partial class ChartDayOfWeek : Form
    {
        private int intTimerCountdown = 1;

        public ChartDayOfWeek()
        {
            InitializeComponent();
        }

        /*-----------------------------------------------------------------
         * This form is being loaded.
         */
        private void ChartDayOfWeek_Load(object sender, EventArgs e)
        {
            Globals.RestoreFormPosition("Chart Day of Week", this, 450, 260);
            intTimerCountdown = 1;
            timer1.Enabled = true;
            return;
        }

        /*------------------------------------------------------------------------
         * This form is being closed.
         */
        private void ChartDayOfWeek_FormClosing(object sender, FormClosingEventArgs e)
        {
            Globals.SaveFormPosition("Chart Day of week", this);
            Globals.objMain.ChartDayOfWeekClosed();
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

        /*--------------------------------------------------------------------------
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
            int[] aryCounts = Globals.objAdif.ConnectionsByDayOfWeek();
            chart1.Series["DayOfWeek"].Points.Clear();
            chart1.Series["DayOfWeek"].Points.AddXY("Sun", aryCounts[0]);
            chart1.Series["DayOfWeek"].Points.AddXY("Mon", aryCounts[1]);
            chart1.Series["DayOfWeek"].Points.AddXY("Tue", aryCounts[2]);
            chart1.Series["DayOfWeek"].Points.AddXY("Wed", aryCounts[3]);
            chart1.Series["DayOfWeek"].Points.AddXY("Thu", aryCounts[4]);
            chart1.Series["DayOfWeek"].Points.AddXY("Fri", aryCounts[5]);
            chart1.Series["DayOfWeek"].Points.AddXY("Sat", aryCounts[6]);
            /*
             * Finished
             */
            return;
        }

    }
}
