using CleanS.CleanS;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraGrid;

namespace CleanS.Base
{
    public class BaseView : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        protected System.Windows.Forms.BindingSource bindingSource1;
        private UnitOfWork unitOfWork1;
        private XPCollection xpCollection1;
        private Session session1;
        private System.ComponentModel.IContainer components;

        private DevExpress.XtraGrid.GridControl gridControl1;

        public GridControl GridControl1 { get => gridControl1; set => gridControl1 = value; }

        protected BaseView()
        {
            InitializeComponent();
            unitOfWork1.ConnectionString = ConnectionHelper.ConnectionString;

        }

        public XPCollection<T> GetCollection<T>() where T : XPLiteObject
        {

            var entry = new XPCollection<T>(unitOfWork1);
            return entry;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entry"></param>
        protected void Save<T>(T entry) where T : XPLiteObject
        {

            //var customer = entry as Customer;

            //PropertyInfo[] properties = entry.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            //PropertyInfo IdProperty = (from PropertyInfo property in properties
            //                           where property.GetCustomAttributes(typeof(KeyAttribute), true).Length > 0
            //                           select property).First();

           

            entry.Save();
            unitOfWork1.CommitChanges();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entry"></param>
        protected void Delete<T>(T entry) where T : XPLiteObject
        {
            entry.Delete();
            unitOfWork1.CommitChanges();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="select"></param>
        /// <returns></returns>
        protected System.Data.DataTable GetData(string select)
        {
            var dt = new DataTable();
            using (var sqlDp = new System.Data.SqlClient.SqlDataAdapter(select, ConnectionHelper.ConnectionStringSql)){
                sqlDp.Fill(dt);
            }
           
            return dt;
        }
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.unitOfWork1 = new DevExpress.Xpo.UnitOfWork(this.components);
            this.xpCollection1 = new DevExpress.Xpo.XPCollection(this.components);
            this.session1 = new DevExpress.Xpo.Session(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.unitOfWork1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xpCollection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.session1)).BeginInit();
            this.SuspendLayout();
            // 
            // unitOfWork1
            // 
            this.unitOfWork1.IsObjectModifiedOnNonPersistentPropertyChange = null;
            this.unitOfWork1.TrackPropertiesModifications = false;
            // 
            // xpCollection1
            // 
            this.xpCollection1.Session = this.unitOfWork1;
            // 
            // session1
            // 
            this.session1.IsObjectModifiedOnNonPersistentPropertyChange = null;
            this.session1.TrackPropertiesModifications = false;
            // 
            // BaseView
            // 
            this.ClientSize = new System.Drawing.Size(664, 292);
            this.Name = "BaseView";
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.unitOfWork1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xpCollection1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.session1)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
