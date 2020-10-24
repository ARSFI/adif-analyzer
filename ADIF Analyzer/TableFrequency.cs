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
    public partial class TableFrequency : Form
    {
        private int intTimerCountdown = 1;

        public TableFrequency()
        {
            InitializeComponent();
        }

        /*----------------------------------------------------------------------------
         * This form is being loaded.
         */
        private void TableFrequency_Load(object sender, EventArgs e)
        {
            Globals.RestoreFormPosition("Table Frequency", this, 215, 285);
            dataGridView1.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            intTimerCountdown = 1;
            timer1.Enabled = true;
            return;
        }

        /*-------------------------------------------------------------------------------
         * This form is closing.
         */
        private void TableFrequency_FormClosing(object sender, FormClosingEventArgs e)
        {
            Globals.SaveFormPosition("Table Frequency", this);
            Globals.objMain.TableFrequencyClosed();
            return;
        }

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
            List<ConnectionCountItem> lstItems = Globals.objAdif.ConnectionsByFrequency();
            int intTotalCount = 0;
            foreach (ConnectionCountItem objItem in lstItems)
            {
                intTotalCount += objItem.intCount;
            }
            if (intTotalCount == 0) intTotalCount = 1;
            /*
             * Display the table
             */
            dataGridView1.Rows.Clear();
            foreach (ConnectionCountItem objItem in lstItems)
            {
                DataGridViewRow objRow = new DataGridViewRow();
                objRow.CreateCells(dataGridView1);
                objRow.Cells[0].Value = objItem.strItem;
                objRow.Cells[1].Value = objItem.intCount;
                double dblPercent = 100.0 * (double)objItem.intCount / (double)intTotalCount;
                objRow.Cells[2].Value = dblPercent.ToString("0.00");
                dataGridView1.Rows.Add(objRow);
            }
            this.Cursor = Cursors.Default;
            return;
        }
    }
}
