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
using DevExpress.XtraScheduler;
using DevExpress.Utils.Menu;
using DevExpress.XtraScheduler.UI;

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

        private void UpdateForm()
        {

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
    }

    public class MyAppointmentFormController : AppointmentFormController
    {
        public MyAppointmentFormController(SchedulerControl control, Appointment apt) : base(control, apt)
        {
        }
    }
}