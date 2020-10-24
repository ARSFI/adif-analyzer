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
    public partial class GridSquareToLatLong : Form
    {
        private LatitudeLongitude objLatLong = null;

        public GridSquareToLatLong()
        {
            InitializeComponent();
        }

        /*-------------------------------------------------------------
         * Get the latitude/longitude values.
         */
        public LatitudeLongitude GetLatLong()
        {
            return objLatLong;
        }

        /*--------------------------------------------------------
         * Use the latitude/longitude.
         */
        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
            return;
        }

        /*-----------------------------------------------------------
         * Cancel.
         */
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
            return;
        }

        /*----------------------------------------------------------------
         * Convert grid square to lat/long.
         */
        private void btnConvert_Click(object sender, EventArgs e)
        {
            btnOk.Enabled = false;
            string strGridSquare = txtGridSquare.Text.Trim().ToUpper();
            if (strGridSquare.Length < 6)
            {
                MessageBox.Show("A grid square must be at least 6 characters long.");
                txtGridSquare.Focus();
                return;
            }
            objLatLong = Globals.GridSquareToDecimalDegrees(strGridSquare);
            if (!objLatLong.blnValid)
            {
                MessageBox.Show("The specified square is not valid.");
                txtGridSquare.Focus();
                return;
            }
            txtLatitude.Text = objLatLong.dblLatitude.ToString("0.0000");
            txtLongitude.Text = objLatLong.dblLongitude.ToString("0.000");
            btnOk.Enabled = true;
            return;
        }
    }
}
