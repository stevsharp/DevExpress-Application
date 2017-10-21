using DevExpress.XtraBars;
using DevExpress.XtraBars.Docking2010.Views.Tabbed;
using CleanS.Views;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraEditors;
using System.Windows.Forms;
using DevExpress.XtraNavBar;
using System;

namespace CleanS
{
    public partial class FrmMain : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public FrmMain()
        {
            InitializeComponent();

            documentManager.MdiParent = this;
            documentManager.View = new TabbedView();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void navBarItem1_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            var navBarItem = sender as NavBarItem;

            var frm = new FrmGridView(navBarItem.Tag.ToString())
            {
                MdiParent = this,
                Name = "Frm" + navBarItem.Tag.ToString(), 
                Text = "Frm" + navBarItem.Tag.ToString()
            };
            frm.Show();
        }
        /// <summary>
        /// Refresh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            SplashScreenManager.ShowDefaultWaitForm();

            var frm = this.ActiveMdiChild as FrmGridView;
            frm.RefreshData();

            SplashScreenManager.CloseDefaultWaitForm();
        }
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
         
            var dCliResTest = XtraMessageBox.Show("Delete Record ? ", "Application", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dCliResTest == DialogResult.Yes)
            {
                SplashScreenManager.ShowDefaultWaitForm();
                var frm = this.ActiveMdiChild as FrmGridView;
                frm.Delete();
                SplashScreenManager.CloseDefaultWaitForm();
            }
        }
        /// <summary>
        /// /
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            folderBrowserDialog1.SelectedPath = "";
            folderBrowserDialog1.ShowDialog(this);
            try
            {
                if (folderBrowserDialog1.SelectedPath.Length != 0)
                {
                    var frm = this.ActiveMdiChild as FrmGridView;
                    frm.GridControl1.ExportToPdf(folderBrowserDialog1.SelectedPath);
                    MessageBox.Show("Η εξαγωγή ολοκληρώθηκε επιτυχώς.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }
        /// <summary>
        /// PDF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
       
        /// <summary>
        /// New
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            var frm = this.ActiveMdiChild as FrmGridView;
            frm.AddNewRow();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void navBarItem5_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            var frm = new frmScheduler()
            {
                MdiParent = this,
                Name = "Frm" + navBarItem1.Tag.ToString()
            };
            frm.Show();
        }
        /// <summary>
        /// Word
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Word_ItemClick(object sender, ItemClickEventArgs e)
        {
            folderBrowserDialog1.SelectedPath = "";
            folderBrowserDialog1.ShowDialog(this);
            try
            {
                if (folderBrowserDialog1.SelectedPath.Length != 0)
                {
                    var frm = this.ActiveMdiChild as FrmGridView;
                    frm.GridControl1.ExportToRtf(folderBrowserDialog1.SelectedPath);
                    MessageBox.Show("Η εξαγωγή ολοκληρώθηκε επιτυχώς.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }
        /// <summary>
        /// XLS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Xls_ItemClick(object sender, ItemClickEventArgs e)
        {
            folderBrowserDialog1.SelectedPath = "";
            folderBrowserDialog1.ShowDialog(this);
            try
            {
                if (folderBrowserDialog1.SelectedPath.Length != 0)
                {
                    var frm = this.ActiveMdiChild as FrmGridView;
                    frm.GridControl1.ExportToXlsx(folderBrowserDialog1.SelectedPath);
                    MessageBox.Show("Η εξαγωγή ολοκληρώθηκε επιτυχώς.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void Report_ItemClick(object sender, ItemClickEventArgs e)
        {

            try
            {

                    var frm = this.ActiveMdiChild as FrmGridView;
                    frm.GridControl1.ShowPrintPreview();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }
    }
}