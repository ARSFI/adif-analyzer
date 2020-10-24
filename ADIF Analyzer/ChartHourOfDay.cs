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
    public partial class ChartHourOfDay : Form
    {
        private int intTimerCountdown = 1;

        public ChartHourOfDay()
        {
            InitializeComponent();
        }

        /*-----------------------------------------------------------------
         * This form is being loaded.
         */
        private void ChartHourOfDay_Load(object sender, EventArgs e)
        {
            Globals.RestoreFormPosition("Chart Hour of Day", this, 700, 280);
            if (Globals.blnTimeZoneUTC)
            {
                this.Text = "Connections by Hour (UTC) -- " + Globals.strRMSCallsign;
            }
            else
            {
                this.Text = "Connections by Hour (Local time) -- " + Globals.strRMSCallsign;
            }
            intTimerCountdown = 1;
            timer1.Enabled = true;
            return;
        }

        /*------------------------------------------------------------------------
         * This form is closing.
         */
        private void ChartHourOfDay_FormClosing(object sender, FormClosingEventArgs e)
        {
            Globals.SaveFormPosition("Chart Hour of Day", this);
            Globals.objMain.ChartHourOfDayClosed();
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

        /*--------------------------------------------------------------------
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
            int[] aryCounts = Globals.objAdif.ConnectionsByHourOfDay();
            chart1.Series["HourOfDay"].Points.Clear();
            for (int hour = 0; hour < 24; hour++)
            {
                chart1.Series["HourOfDay"].Points.AddXY(hour.ToString("00"), aryCounts[hour]);
            }
            /*
             * Finished
             */
            return;
        }
    }
}
