using CleanS.Base;
using CleanS.CleanS;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using CleanS.Infrastructure;

namespace CleanS.Views
{
    public partial class FrmGridView : BaseView
    {
        /// <summary>
        /// 
        /// </summary>
        protected const string namespaceName = "CleanS.CleanS"; // typeof(OSM_COUNTRY).Namespace; 
        protected const string assemblyName = "CleanS";         //typeof(OSM_COUNTRY).Assembly.GetName().Name;
        protected Type type;
        protected MetadataTypeAttribute nestedType;
        /// <summary>
        /// 
        /// </summary>
        protected readonly string _typeName = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeName"></param>
        public FrmGridView(string typeName)
        {
            _typeName = typeName;
            InitializeComponent();
            RefreshData();
            this.GridControl1 = gridControl1;

            gridView1.OptionsBehavior.Editable = true;
            gridView1.OptionsView.NewItemRowPosition = NewItemRowPosition.Top;
            gridView1.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
        }
        /// <summary>
        /// 
        /// </summary>
        public void CreateDynamicGrid()
        {
            type = Type.GetType($"{namespaceName}.{_typeName}, {assemblyName}");

            nestedType = type.GetCustomAttributes(typeof(MetadataTypeAttribute), true).OfType<MetadataTypeAttribute>().FirstOrDefault();

            object metadata = Activator.CreateInstance(nestedType.MetadataClassType);
            var requiredProperties =
                metadata.GetType().GetProperties().Where(property =>
                property.IsDefined(typeof(Struct.FieldTypeAttribute), false)).ToList();

            // Display 
            //var DisplayProperties =
            //    metadata.GetType().GetProperties().Where(property =>
            //    property.IsDefined(typeof(DisplayAttribute), false)).ToList();

            var properties = metadata.GetType().GetProperties();

            gridControl1.MainView.BeginDataUpdate();
            try
            {
                var gridView = (DevExpress.XtraGrid.Views.Grid.GridView)gridControl1.MainView;

                foreach (var property in properties)
                {
                    var attributes = property.GetCustomAttributes(false);
                    foreach (var attribute in attributes)
                    {
                        if (attribute.GetType() == typeof(Struct.FieldTypeAttribute))
                        {
                            var prop = attribute as Struct.FieldTypeAttribute;

                            var riLookup = new RepositoryItemLookUpEdit()
                            {
                                Name = "RepositoryItemLookUpEdit" + prop.Table
                            };
                            var select = string.Format("SELECT {0},{1} FROM {2}", prop.Value, prop.Display, prop.Table);
                            var dt = GetData(select);

                            riLookup.DataSource = dt;
                            riLookup.ValueMember = prop.Value;
                            riLookup.DisplayMember = prop.Display;

                            riLookup.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                            riLookup.DropDownRows = dt.Rows.Count;
                            riLookup.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                            riLookup.AutoSearchColumnIndex = 1;

                            
                            if (prop.Index > 0)
                            {
                                gridView.Columns[prop.Index].Caption = prop.Caption;
                                gridView.Columns[prop.Index].ColumnEdit = riLookup;
                            }
                            else
                            {
                                gridView.Columns[property.Name + "!Key"].ColumnEdit = riLookup;
                                gridView.Columns[property.Name + "!Key"].Caption = prop.Caption;
                            }
                            gridView1.BestFitColumns();
                        }
                        
                        if (attribute.GetType() == typeof(Struct.CustomDisplayName))
                        {
                            var dispprop = attribute as Struct.CustomDisplayName;
                            if (gridView.Columns[property.Name] != null)
                            {
                                gridView.Columns[property.Name].Caption = dispprop.DisplayName;
                            }
                            else
                            {
                                gridView.Columns[property.Name+"!Key"].Caption = dispprop.DisplayName;
                            }
                        }
                    }
                }
            }
            finally
            {
                gridControl1.MainView.EndDataUpdate();
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void gridControl1_DataSourceChanged(object sender, EventArgs e)
        {
            gridControl1.MainView.PopulateColumns();
            (gridControl1.MainView as GridView).BestFitColumns();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView1_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            var entry = this.bindingSource1.Current as BaseObject;

            if (entry.GetType() == typeof(Employee))
            {
                var emp = entry as Employee;
                //emp.IdEntitydet = 1;
            }

            switch (_typeName)
            {
                case "Employee":
                    //var idIndetitySet = gridView1.GetRowCellValue(e.RowHandle, gridView1.Columns["IdEntitydet"]);
                    break;
                default:
                    break;
            }

            this.Save(entry);
            Cursor.Current = Cursors.Default;
        }
        /// <summary>
        /// 
        /// </summary>
        public void AddNewRow()
        {
            //gridView1.AddNewRow();

            this.gridView1.AddNewRow();
            this.gridView1.FocusedRowHandle = this.gridView1.RowCount - 1;
            this.gridView1.TopRowIndex = this.gridView1.RowCount - 1;
            //this.gridView1.ShowPopupEditForm();

        }
        /// <summary>
        /// 
        /// </summary>
        public void Delete()
        {
            var entry = this.bindingSource1.Current as BaseObject;
            this.Delete(entry);
            RefreshData();
        }
        /// <summary>
        /// 
        /// </summary>
        public void RefreshData()
        {
            Cursor.Current = Cursors.WaitCursor;
            var type = Type.GetType($"{namespaceName}.{_typeName}, {assemblyName}");
            var method = this.GetType().GetMethod("GetCollection").MakeGenericMethod(type);
            var result = method.Invoke(this, null);
            bindingSource1.DataSource = result;
            gridControl1.DataSource = bindingSource1;
            gridView1.DataSourceChanged += gridControl1_DataSourceChanged;
            CreateDynamicGrid();

            Cursor.Current = Cursors.Default;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridControl1_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            if (e.Button.ButtonType == NavigatorButtonType.Remove)
            {
                if (MessageBox.Show("Do you want to delete the current row?", "Confirm deletion", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    this.Delete();
                    e.Handled = true;
                }
            }
        }
    }
}
//var type = typeof(Customer);
//var metadataType = type.GetCustomAttributes(typeof(MetadataTypeAttribute), true)
//    .OfType<MetadataTypeAttribute>().FirstOrDefault();