﻿using System;
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
using CleanS.Base;
using CleanS.CleanS;

namespace CleanS.Views
{
    public partial class frmScheduler : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public frmScheduler()
        {
            InitializeComponent();

            this.schedulerStorage1.AppointmentsInserted += new DevExpress.XtraScheduler.PersistentObjectsEventHandler(this.schedulerStorage_AppointmentsChanged);
            this.schedulerStorage1.AppointmentsChanged += new DevExpress.XtraScheduler.PersistentObjectsEventHandler(this.schedulerStorage_AppointmentsChanged);
            this.schedulerStorage1.AppointmentsDeleted += new DevExpress.XtraScheduler.PersistentObjectsEventHandler(this.schedulerStorage_AppointmentsChanged);
        }

        private void schedulerStorage_AppointmentsChanged(object sender, PersistentObjectsEventArgs e)
        {
            try
            {
                AppointmentBaseCollection collection = e.Objects as AppointmentBaseCollection;

                foreach (Appointment appointment in collection)
                {
                    var app = new Appointments(session1)
                    {
                        Description = appointment.Description
                    };

                    app.Save();
                }


                schedulerControl1.Refresh();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        /// <summary>
        /// Day
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            schedulerControl1.ActiveViewType = DevExpress.XtraScheduler.SchedulerViewType.Day;
        }
        /// <summary>
        /// Week
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            schedulerControl1.ActiveViewType = DevExpress.XtraScheduler.SchedulerViewType.Week;
        }
        /// <summary>
        /// Month
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            schedulerControl1.ActiveViewType = DevExpress.XtraScheduler.SchedulerViewType.Month;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            schedulerControl1.ActiveViewType = DevExpress.XtraScheduler.SchedulerViewType.Agenda;
        }

        private void schedulerControl1_EditAppointmentFormShowing(object sender, AppointmentFormEventArgs e)
        {
            Appointment apt = e.Appointment;

            bool openRecurrenceForm = apt.IsRecurring && schedulerStorage1.Appointments.IsNewAppointment(apt);

            var frm = new frmAddNewApp((SchedulerControl)sender, apt, openRecurrenceForm);
            frm.SetMenuManager(this.schedulerControl1.MenuManager);
            frm.LookAndFeel.ParentLookAndFeel = this.LookAndFeel.ParentLookAndFeel;
            e.DialogResult = frm.ShowDialog();
            e.Handled = true;

            if (apt.Type == AppointmentType.Pattern && schedulerControl1.SelectedAppointments.Contains(apt))
                schedulerControl1.SelectedAppointments.Remove(apt);

            bindingSource1.DataSource = this.xpCollection1;

            

            schedulerControl1.Refresh();
        }
    }
}