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
    public partial class ChartMode : Form
    {
        private int intTimerCountdown = 1;

        public ChartMode()
        {
            InitializeComponent();
        }

        private void ChartMode_Load(object sender, EventArgs e)
        {
            Globals.RestoreFormPosition("Chart Mode", this, 500, 210);
            intTimerCountdown = 1;
            timer1.Enabled = true;
            return;
        }

        private void ChartMode_FormClosing(object sender, FormClosingEventArgs e)
        {
            Globals.SaveFormPosition("Chart Mode", this);
            Globals.objMain.ChartModeClosed();
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
            List<ConnectionCountItem> lstCount = Globals.objAdif.ConnectionsByMode();
            chart1.Series["Mode"].Points.Clear();
            foreach (ConnectionCountItem objCount in lstCount)
            {
                chart1.Series["Mode"].Points.AddXY(objCount.strItem, objCount.intCount);
            }
            /*
             * Finished
             */
            return;
        }
    }
}
