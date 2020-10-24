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

    public partial class TableRecentCount : Form
    {
        private int intTimerCountdown = 1;

        public TableRecentCount()
        {
            InitializeComponent();
        }

        /*---------------------------------------------------------------------------
         * This form is loading.
         */
        private void TableRecentCount_Load(object sender, EventArgs e)
        {
            Globals.RestoreFormPosition("Table Recent Count", this, 288, 282);
            dataGridView1.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            intTimerCountdown = 1;
            timer1.Enabled = true;
            return;
        }

        /*------------------------------------------------------------------------------
         * This form is being closed.
         */
        private void TableRecentCount_FormClosing(object sender, FormClosingEventArgs e)
        {
            Globals.SaveFormPosition("Table Recent Count", this);
            Globals.objMain.TableRecentCountClosed();
            return;
        }

        /*-----------------------------------------------------------------------------
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
            int intNumDays = 30;
            this.Cursor = Cursors.WaitCursor;
            /*
             * Count connections for last 30 days.
             */
            DayStatistics[] aryDay = Globals.objAdif.GetRecentCount(intNumDays);
            /*
             * Display the table
             */
            dataGridView1.Rows.Clear();
            for (int day = 0; day < intNumDays; day++)
            {
                DayStatistics objDay = aryDay[day];
                DataGridViewRow objRow = new DataGridViewRow();
                objRow.CreateCells(dataGridView1);
                objRow.Cells[0].Value = objDay.dttDate.ToString("yyyy-MM-dd");
                objRow.Cells[1].Value = objDay.Connections;
                objRow.Cells[2].Value = objDay.ConnectTime.ToString("c");
                dataGridView1.Rows.Add(objRow);
            }
            this.Cursor = Cursors.Default;
            return;
        }
    }
}
