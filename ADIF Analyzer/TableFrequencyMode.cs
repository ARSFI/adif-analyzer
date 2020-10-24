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
    public partial class TableFrequencyMode : Form
    {
        private int intTimerCountdown = 1;

        public TableFrequencyMode()
        {
            InitializeComponent();
        }

        /*-----------------------------------------------------------------------------
         * The table is being loaded.
         */
        private void TableFrequencyMode_Load(object sender, EventArgs e)
        {
            Globals.RestoreFormPosition("Table Frequency-Mode", this, 500, 220);
            intTimerCountdown = 1;
            timer1.Enabled = true;
            return;
        }

        /*--------------------------------------------------------------------------------------
         * This form is closing
         */
        private void TableFrequencyMode_FormClosing(object sender, FormClosingEventArgs e)
        {
            Globals.SaveFormPosition("Table Frequency-Mode", this);
            Globals.objMain.TableFrequencyModeClosed();
            return;
        }

        /*-------------------------------------------------------------------------------------
         * A timer tick has occurred.
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
            /*
             * Get the data block with the matrix info.
             */
            FrequencyModeData objFreqMode = Globals.objAdif.GetFrequencyMode();
            /*
             * Create a column for each mode.
             */
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("Frquency", "Frequency");
            int col;
            for (col = 0; col < objFreqMode.NumModes; col++)
            {
                dataGridView1.Columns.Add(objFreqMode.aryModes[col], objFreqMode.aryModes[col]);
            }
            foreach (DataGridViewColumn item in dataGridView1.Columns)
            {
                item.Width = 60;
                item.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                item.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            /*
             * Display the table
             */
            dataGridView1.Rows.Clear();
            for (int row = 0; row < objFreqMode.NumFreq; row++)
            {
                DataGridViewRow objRow = new DataGridViewRow();
                objRow.CreateCells(dataGridView1);
                objRow.Cells[0].Value = objFreqMode.aryFrequency[row].ToString("0.0");
                for (col = 0; col < objFreqMode.NumModes; col++)
                {
                    objRow.Cells[col + 1].Value = objFreqMode.aryCount[row, col];
                }
                dataGridView1.Rows.Add(objRow);
            }
            this.Cursor = Cursors.Default;
            return;
        }
    }
}
