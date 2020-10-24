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
    public partial class TableCountry : Form
    {
        private int intTimerCountdown = 1;

        public TableCountry()
        {
            InitializeComponent();
        }

        /*----------------------------------------------------------------------------
         * This form is being loaded.
         */
        private void TableCountry_Load(object sender, EventArgs e)
        {
            Globals.RestoreFormPosition("Table Country", this, 350, 300);
            intTimerCountdown = 1;
            timer1.Enabled = true;
            return;
        }

        /*--------------------------------------------------------------------------------
         * This form is closing.
         */
        private void TableCountry_FormClosing(object sender, FormClosingEventArgs e)
        {
            Globals.SaveFormPosition("Table Country", this);
            Globals.objMain.TableCountryClosed();
            return;
        }

        /*---------------------------------------------------------------------------
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
            List<CountryCount> lstCount = Globals.objAdif.GetCountryCount();
            int intTotalCount = 0;
            foreach (CountryCount objCount in lstCount)
            {
                intTotalCount += objCount.intCount;
            }
            if (intTotalCount == 0) intTotalCount = 1;
            double dblTotalCount = intTotalCount;
            foreach (CountryCount objCount in lstCount)
            {
                DataGridViewRow objRow = new DataGridViewRow();
                objRow.CreateCells(dataGridView1);
                objRow.Cells[0].Value = objCount.intCount;
                objRow.Cells[1].Value = (int)(0.5 + (100.0 * objCount.intCount / dblTotalCount));
                objRow.Cells[2].Value = objCount.strCountry;
                dataGridView1.Rows.Add(objRow);
            }
            /* Sort the rows in descending order connections */
            dataGridView1.Columns[0].HeaderCell.SortGlyphDirection = SortOrder.Descending;
            dataGridView1.Sort(dataGridView1.Columns[0], ListSortDirection.Descending);
            this.Cursor = Cursors.Default;
            return;
        }
    }
}
