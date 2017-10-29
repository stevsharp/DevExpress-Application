using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraScheduler;
using DevExpress.Utils.Menu;
using DevExpress.XtraScheduler.UI;
using CleanS.CleanS;
using CleanS.Dataset.CleanSDatasetTableAdapters;
using System.Data;

namespace CleanS.Views
{
    public partial class frmAddNewApp : DevExpress.XtraEditors.XtraForm
    {
        protected SchedulerControl control;
        protected Appointment apt;
        protected bool openRecurrenceForm = false;
        protected int suspendUpdateCount;
        protected IDXMenuManager menuManager;
        protected MyAppointmentFormController controller;
        protected ViewEmpPerContractTableAdapter ViewEmpPerContract;
        protected ContractEmployeeTableAdapter ContractEmployeeTB;
        protected EmployeeTableAdapter employeeTableAdapter;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        /// <param name="apt"></param>
        /// <param name="openRecurrenceForm"></param>
        public frmAddNewApp(SchedulerControl control, Appointment apt, bool openRecurrenceForm)
        {
            this.openRecurrenceForm = openRecurrenceForm;
 
            this.controller = new MyAppointmentFormController(control, apt);
            this.apt = apt;
            this.control = control;

            SuspendUpdate();
            InitializeComponent();
            ResumeUpdate();
            UpdateForm();
        }
        /// <summary>
        /// 
        /// </summary>
        public Appointment Appointment
        {
            get
            {
                return apt;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void UpdateForm()
        {
            SuspendUpdate();

            try
            {
                txSubject.Text = controller.Subject;
                edStatus.Status = Appointments.Statuses[controller.StatusId];
                edLabel.Label = Appointments.Labels[controller.LabelId];
                dtStart.DateTime = controller.DisplayStart.Date;
                dtEnd.DateTime = controller.DisplayEnd.Date;
                timeStart.Time = DateTime.MinValue.AddTicks(controller.Start.TimeOfDay.Ticks);
                timeEnd.Time = DateTime.MinValue.AddTicks(controller.End.TimeOfDay.Ticks);
                edStatus.Storage = control.Storage;
                edLabel.Storage = control.Storage;
                memoEdit1.Text = controller.Description;
                searchLookUpEdit2.EditValue = controller.ContractMapping;
                FillGrid(controller.ContractMapping);
            }
            finally
            {
                ResumeUpdate();
            }
            UpdateIntervalControls();
        }

        protected virtual void UpdateIntervalControls()
        {
            if (IsUpdateSuspended)
                return;

            SuspendUpdate();

            try
            {
                dtStart.EditValue = controller.DisplayStart.Date;
                dtEnd.EditValue = controller.DisplayEnd.Date;
                timeStart.EditValue = controller.DisplayStart.TimeOfDay;
                timeEnd.EditValue = controller.DisplayEnd.TimeOfDay;

                Appointment editedAptCopy = controller.EditedAppointmentCopy;
                bool showControls = IsNewAppointment || editedAptCopy.Type != AppointmentType.Pattern;
                dtStart.Enabled = showControls;
                dtEnd.Enabled = showControls;
                bool enableTime = showControls && !controller.AllDay;
                timeStart.Visible = enableTime;
                timeStart.Enabled = enableTime;
                timeEnd.Visible = enableTime;
                timeEnd.Enabled = enableTime;
            }
            finally
            {
                ResumeUpdate();
            }
        }

        public void SetMenuManager(IDXMenuManager menuManager)
        {
            SetMenuManagerCore(Controls, menuManager);
            this.menuManager = menuManager;
        }

        void SetMenuManagerCore(Control.ControlCollection controls, IDXMenuManager menuManager)
        {
            int count = controls.Count;
            for (int i = 0; i < count; i++)
            {
                Control control = controls[i];
                SetMenuManagerCore(control.Controls, menuManager);
                BaseEdit baseEdit = control as BaseEdit;
                if (baseEdit == null)
                    continue;
                baseEdit.MenuManager = menuManager;
            }
        }
        protected AppointmentStorage Appointments { get { return control.Storage.Appointments; } }
        protected internal bool IsNewAppointment { get { return controller != null ? controller.IsNewAppointment : true; } }
        protected bool IsUpdateSuspended { get { return suspendUpdateCount > 0; } }
        public IDXMenuManager MenuManager { get { return menuManager; } }

        public int AppId { get; }

        protected void SuspendUpdate()
        {
            suspendUpdateCount++;
        }
        protected void ResumeUpdate()
        {
            if (suspendUpdateCount > 0)
                suspendUpdateCount--;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        public DataTable GetEmp
        {
            get
            {
                return dataSet1.Tables[0];
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!controller.IsConflictResolved())
                return;

            dxErrorProvider1.ClearErrors();

            if (txSubject.Text == String.Empty)
            {
                txSubject.Focus();
                dxErrorProvider1.SetError(searchLookUpEdit2, "*");
                return;
            }

            dxErrorProvider1.ClearErrors();

            if (searchLookUpEdit2.EditValue.ToString() == String.Empty)
            {
                searchLookUpEdit2.Focus();
                dxErrorProvider1.SetError(searchLookUpEdit2, "*");
                return;
            }

            controller.SetStatus(edStatus.Status);
            controller.SetLabel(edLabel.Label);
            controller.DisplayStart = this.dtStart.DateTime.Date + this.timeStart.Time.TimeOfDay;
            controller.DisplayEnd = this.dtEnd.DateTime.Date + this.timeEnd.Time.TimeOfDay;
            controller.Subject = txSubject.Text;
            controller.Description = memoEdit1.Text;
            controller.ContractMapping = Convert.ToInt32(searchLookUpEdit2.EditValue.ToString());
            controller.dsEmp = this.GetEmp;

            controller.ApplyChanges();
            this.DialogResult = DialogResult.OK;


        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void searchLookUpEdit2_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                var value = searchLookUpEdit2.EditValue;
                //FillGrid(value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FillGrid(object value)
        {
            dataSet1.Tables[0].Clear();
            dataSet1.Tables[0].AcceptChanges();
            gridControl1.RefreshDataSource();


            if (ViewEmpPerContract == null)
                ViewEmpPerContract = new ViewEmpPerContractTableAdapter();

            if (employeeTableAdapter == null)
                employeeTableAdapter = new EmployeeTableAdapter();

           
            ViewEmpPerContract.Fill(this.cleanSDataset.ViewEmpPerContract);
            employeeTableAdapter.Fill(this.cleanSDataset.Employee);

            if (this.cleanSDataset.ViewEmpPerContract.Count > 0)
            {
                DataView dv = new DataView(cleanSDataset.ViewEmpPerContract) { RowFilter = "IdContract=" + value };

                if (dv.Count > 0)
                {
                    dataSet1.Tables[0].Clear();
                    dataSet1.Tables[0].AcceptChanges();

                    foreach (DataRowView rowView in dv)
                    {
                        var row = dataSet1.Tables[0].NewRow();
                        row[0] = true;
                        row[1] = rowView["FirstName"];
                        row[2] = rowView["LastName"];
                        row["Id"] = rowView["IdEmployee"];

                        dataSet1.Tables[0].Rows.Add(row);
                    }

                    dataSet1.Tables[0].AcceptChanges();
                }

                foreach (var rowEmp in this.cleanSDataset.Employee)
                {
                    var dataView = new DataView(dataSet1.Tables[0]) { RowFilter = "id=" + rowEmp["IdEmployee"] };
                    if (dataView.Count == 0)
                    {

                        var row = dataSet1.Tables[0].NewRow();
                        row[0] = false;
                        row[1] = rowEmp["FirstName"];
                        row[2] = rowEmp["LastName"];
                        row["Id"] = rowEmp["IdEmployee"];

                        dataSet1.Tables[0].Rows.Add(row);
                        dataSet1.Tables[0].AcceptChanges();
                    }
                }
            }
        }

        /// <summary>
        /// /
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmAddNewApp_Load(object sender, EventArgs e)
        {
            //this.cleanSDataset.EnforceConstraints = false;

            ViewEmpPerContract = new ViewEmpPerContractTableAdapter();
            employeeTableAdapter = new EmployeeTableAdapter();
            this.contractTableAdapter.Fill(this.cleanSDataset.Contract);
            this.customerTableAdapter.Fill(this.cleanSDataset.Customer);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void searchLookUpEdit1_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                this.searchLookUpEdit2.EditValueChanged -= new System.EventHandler(this.searchLookUpEdit2_EditValueChanged);
                var value = searchLookUpEdit1.EditValue;
                this.contractTableAdapter.FillByIdCustomer(cleanSDataset.Contract, (int)value);
                this.searchLookUpEdit2.EditValueChanged += new System.EventHandler(this.searchLookUpEdit2_EditValueChanged);
            }
            catch (Exception)
            { }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class MyAppointmentFormController : AppointmentFormController
    {
        public MyAppointmentFormController(SchedulerControl control, Appointment apt) : base(control, apt)
        {
        }

        public override bool IsAppointmentChanged()
        {
            if (base.IsAppointmentChanged())
                return true;


            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        protected override void ApplyCustomFieldsValues()
        {

        }

        public DataTable dsEmp
        {
            get
            {
                var id = new DataTable();

                try
                {

                    id = (DataTable)EditedAppointmentCopy.CustomFields["dsEmp"];
                }
                catch (Exception) { }
                return id;

            }
            set
            {
                EditedAppointmentCopy.CustomFields["dsEmp"] = value;
            }
        }
        public int ContractMapping
        {
            get {
                int id = 0;

                try
                {
                    if (int.TryParse(EditedAppointmentCopy.CustomFields["IdContract"].ToString(), out id))
                    {
                        id = Convert.ToInt32(EditedAppointmentCopy.CustomFields["IdContract"].ToString()); 
                    }
                }
                catch (Exception)
                {}
                return id;

            }
            set { EditedAppointmentCopy.CustomFields["IdContract"] = value; }
        }

    }
}