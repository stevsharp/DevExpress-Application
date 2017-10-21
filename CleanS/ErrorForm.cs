using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Diagnostics;

namespace CleanS
{
    public partial class ErrorForm : DevExpress.XtraEditors.XtraForm
    {
        public ErrorForm()
        {
            InitializeComponent();
        }

        protected void SetBGColor(Form form)
        {
            form.BackColor = BGColor;
        }

        protected Color BGColor
        {
            get
            {
                return Color.FromArgb(122, 150, 223);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtaExceptionDetails.Text.Trim());
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            (new ApplicationContext()).Dispose();
        }

        public string ExceptionMessage
        {
            get;
            set;
        }

        private void Error_Load(object sender, EventArgs e)
        {
            SetBGColor(this);
            txtaExceptionDetails.Text = ExceptionMessage;
        }

        private void btnReportBug_Click(object sender, EventArgs e)
        {
            string supportMail = "sponaris@gmail.com";
            string subject = "Account Plus : Bug Report";
            string mailBody = txtaExceptionDetails.Text.Trim();
            Process.Start("mailto:" + supportMail + "&subject=" + subject + "&Body=" + mailBody);
        }
    }
}