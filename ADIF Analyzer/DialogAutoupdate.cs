using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Timers;

namespace ADIF_Analyzer
{
    public partial class DialogAutoupdate : Form
    {
        private int intTimeout = 30;
        private string strNewVersion;

        public DialogAutoupdate(string strNewVer)
        {
            InitializeComponent();
            strNewVersion = strNewVer;
            return;
        }

        /*-----------------------------------------------------------------------------
         * This form is being loaded.
         */
        private void DialogAutoupdate_Load(object sender, EventArgs e)
        {
            lblCV.Text = Application.ProductVersion;
            lblNV.Text = strNewVersion;
            CheckForIllegalCrossThreadCalls = false;

#if false
            int tmpTop As Integer = objMain.Top + (objMain.Height \ 2)
        Dim tmpLeft As Integer = objMain.Left + (objMain.Width \ 2)
        Me.Top = tmpTop - (Me.Height \ 2)
        Me.Left = tmpLeft - (Me.Width \ 2)
#endif

            lblTR.Text = intTimeout.ToString();
            BtnUpdate.Focus();
            this.Activate();
            tmrTimeout.Start();
        }

        /*---------------------------------------------------------------------------------
         * This form is closing.
         */
        private void DialogAutoupdate_FormClosing(object sender, FormClosingEventArgs e)
        {
            tmrTimeout.Stop();
            return;
        }

        /*---------------------------------------------------------------------------
         * Perform the update.
         */
        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            tmrTimeout.Enabled = false;
            this.DialogResult = DialogResult.OK;
            this.Close();
            return;
        }

        /*-----------------------------------------------------------------------------
         * Do not update.
         */
        private void btnAbort_Click(object sender, EventArgs e)
        {
            tmrTimeout.Enabled = false;
            this.DialogResult = DialogResult.Cancel;
            this.Close();
            return;
        }

        /*-------------------------------------------------------------
         * The timeout timer ticked.
         */
        private void tmrTimeout_Tick(object sender, EventArgs e)
        {
            intTimeout--;
            lblTR.Text = intTimeout.ToString();
            if (intTimeout <= 0)
            {
                tmrTimeout.Enabled = false;
                this.DialogResult = DialogResult.OK;
                this.Close();
                return;
            }
        }
    }
}
