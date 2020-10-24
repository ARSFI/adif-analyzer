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
    public partial class ChartFrequency : Form
    {
        private int intTimerCountdown = 1;

        public ChartFrequency()
        {
            InitializeComponent();
        }

        /*----------------------------------------------------------------------
         * This form is being loaded.
         */
        private void ChartFrequency_Load(object sender, EventArgs e)
        {
            Globals.RestoreFormPosition("Chart Frequency", this, 400, 200);
            intTimerCountdown = 1;
            timer1.Enabled = true;
            return;
        }

        /*------------------------------------------------------------------------
        * This form is closing.
        */
        private void ChartFrequency_FormClosing(object sender, FormClosingEventArgs e)
        {
            Globals.SaveFormPosition("Chart Frequency", this);
            Globals.objMain.ChartFrequencyClosed();
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
            List<ConnectionCountItem> lstCount = Globals.objAdif.ConnectionsByFrequency();
            chart1.Series["Band"].Points.Clear();
            foreach (ConnectionCountItem objCount in lstCount)
            {
                chart1.Series["Band"].Points.AddXY(objCount.strItem, objCount.intCount);
            }
            /*
             * Finished
             */
            return;
        }
    }
}
