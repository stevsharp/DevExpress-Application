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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!controller.IsConflictResolved())
                return;

            controller.SetStatus(edStatus.Status);
            controller.SetLabel(edLabel.Label);
            controller.DisplayStart = this.dtStart.DateTime.Date + this.timeStart.Time.TimeOfDay;
            controller.DisplayEnd = this.dtEnd.DateTime.Date + this.timeEnd.Time.TimeOfDay;
            controller.Subject = txSubject.Text;
            controller.Description = memoEdit1.Text;
            controller.ContractMapping = searchLookUpEdit2.EditValue.ToString();

            ContractEmployeeTB = new ContractEmployeeTableAdapter();
            ContractEmployeeTB.DeleteQueryWithIdContract(Convert.ToInt32(controller.ContractMapping));

            foreach (DataRow row in dataSet1.Tables[0].Rows)
            {
                if (Convert.ToBoolean(row[0].ToString()))
                {
                    ContractEmployeeTB.InsertQuery(Convert.ToInt32(searchLookUpEdit2.EditValue.ToString()), Convert.ToInt32(row[3].ToString()));
                }
            }

            controller.ApplyChanges();
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
                ViewEmpPerContract.Fill(this.cleanSDataset.ViewEmpPerContract);

                if (this.cleanSDataset.ViewEmpPerContract.Count > 0)
                {
                    DataView dv = new DataView(cleanSDataset.ViewEmpPerContract)
                    {
                        RowFilter = "IdEmployee=" + value
                    };

                    if (dv.Count > 0)
                    {
                        dataSet1.Tables[0].Clear();
                        dataSet1.Tables[0].AcceptChanges();

                        foreach (DataRowView rowView in dv)
                        {
                            var row = dataSet1.Tables[0].NewRow();
                            row[1] = rowView["FirstName"];
                            row[2] = rowView["LastName"];
                            row["Id"] = rowView["IdEmployee"];

                            dataSet1.Tables[0].Rows.Add(row);
                        }

                        dataSet1.Tables[0].AcceptChanges();

                    }
                }


            }
            catch (Exception)
            {

            }
        }
        /// <summary>
        /// /
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmAddNewApp_Load(object sender, EventArgs e)
        {
            ViewEmpPerContract = new ViewEmpPerContractTableAdapter();
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
                this.contractTableAdapter.FillByIdCustomer(cleanSDataset.Contract,(int)value);
                this.searchLookUpEdit2.EditValueChanged += new System.EventHandler(this.searchLookUpEdit2_EditValueChanged);
            }
            catch (Exception)
            {}
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

        public string ContractMapping
        {
            get { return (string)EditedAppointmentCopy.CustomFields["IdContract"]; }
            set { EditedAppointmentCopy.CustomFields["IdContract"] = value; }
        }

    }
}