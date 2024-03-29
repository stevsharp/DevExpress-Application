﻿using System;
using System.Windows.Forms;
using DevExpress.XtraScheduler;
using DevExpress.XtraScheduler.Services;
using DevExpress.XtraScheduler.Commands;
using DevExpress.Utils.Menu;
using CleanS.Dataset.CleanSDatasetTableAdapters;
using System.Data;

namespace CleanS.Views
{
    public partial class frmScheduler : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private DataTable dsEmp = null;

        public frmScheduler()
        {
            InitializeComponent();
            AddCustomFieldsMapping();
            this.schedulerControl1.PopupMenuShowing += new DevExpress.XtraScheduler.PopupMenuShowingEventHandler(this.schedulerControl1_PopupMenuShowing);

            this.schedulerStorage1.AppointmentsInserted += new DevExpress.XtraScheduler.PersistentObjectsEventHandler(this.schedulerStorage_AppointmentsChanged);
            this.schedulerStorage1.AppointmentsChanged += new DevExpress.XtraScheduler.PersistentObjectsEventHandler(this.schedulerStorage_AppointmentsChanged);
            this.schedulerStorage1.AppointmentsDeleted += new DevExpress.XtraScheduler.PersistentObjectsEventHandler(this.schedulerStorage_AppointmentsChanged);
        }

        private void schedulerControl1_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (e.Menu.Id == DevExpress.XtraScheduler.SchedulerMenuItemId.DefaultMenu)
            {
                // Hide the "Change View To" menu item.
                SchedulerPopupMenu itemChangeViewTo = e.Menu.GetPopupMenuById(SchedulerMenuItemId.SwitchViewMenu);
                itemChangeViewTo.Visible = false;

                // Remove unnecessary items.
                e.Menu.RemoveMenuItem(SchedulerMenuItemId.NewAllDayEvent);
                e.Menu.RemoveMenuItem(SchedulerMenuItemId.NewRecurringAppointment);
                e.Menu.RemoveMenuItem(SchedulerMenuItemId.NewRecurringEvent);

                // Disable the "New Recurring Appointment" menu item.
                e.Menu.DisableMenuItem(SchedulerMenuItemId.NewRecurringAppointment);

                // Find the "New Appointment" menu item and rename it.
                SchedulerMenuItem item = e.Menu.GetMenuItemById(SchedulerMenuItemId.NewAppointment);
                if (item != null) item.Caption = "&Νέο ραντεβού";

                // Create a menu item for the Scheduler command.
                ISchedulerCommandFactoryService service =
                (ISchedulerCommandFactoryService)schedulerControl1.GetService(typeof(ISchedulerCommandFactoryService));
                SchedulerCommand cmd = service.CreateCommand(SchedulerCommandId.SwitchToGroupByResource);
                SchedulerMenuItemCommandWinAdapter menuItemCommandAdapter =
                    new SchedulerMenuItemCommandWinAdapter(cmd);
                DXMenuItem menuItem = (DXMenuItem)menuItemCommandAdapter.CreateMenuItem(DXMenuItemPriority.Normal);
                menuItem.BeginGroup = true;
                e.Menu.Items.Add(menuItem);
            }
        }

        private void AddCustomFieldsMapping()
        {
            var IdContractMapping = new AppointmentCustomFieldMapping("IdContract", "IdContract");
            var ds = new AppointmentCustomFieldMapping("dsEmp", "dsEmp");
            schedulerStorage1.Appointments.CustomFieldMappings.Add(IdContractMapping);
            schedulerStorage1.Appointments.CustomFieldMappings.Add(ds);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void schedulerStorage_AppointmentsChanged(object sender, PersistentObjectsEventArgs e)
        {
            try
            {

                int rowID = this.appointmentsTableAdapter.Update(this.cleanSDataset);
                this.cleanSDataset.AcceptChanges();

                DataView dv = new DataView(this.cleanSDataset.Tables["Appointments"]) { RowFilter = "ResourceID=1"};
                int id = 0;

                if (dv.Count  > 0)
                {
                    foreach (DataRowView rowView in dv)
                    {
                        id = Convert.ToInt32(rowView["UniqueID"].ToString());

                        Appointment app = (Appointment)e.Objects[0];

                        this.dsEmp = app.CustomFields["dsEmp"] as DataTable;

                        if (dsEmp.Rows.Count > 0)
                        {
                            var appointmentEmployeeTableAdapter = new AppointmentEmployeeTableAdapter();
                            appointmentEmployeeTableAdapter.DeleteQueryUniqueID(id);

                            foreach (DataRow row in dsEmp.Rows)
                            {
                                var select = Convert.ToBoolean(row[0].ToString());
                                if (select)
                                    appointmentEmployeeTableAdapter.Insert(Convert.ToInt32(row["id"].ToString()), id);
                            }
                        }

                        schedulerControl1.Storage.SetAppointmentId((Appointment)e.Objects[0], id);


                    }
                }

                foreach(DataRow r in this.cleanSDataset.Tables["Appointments"].Rows)
                {
                    if (Convert.ToInt32(r["ResourceID"].ToString()) == 1)
                        r["ResourceID"] = 0;
                }


                this.cleanSDataset.AcceptChanges();
                schedulerControl1.Refresh();

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }

        }

        private void schedulerStorage_UpdatedAppointmentsChanged(object sender, PersistentObjectsEventArgs e)
        {
            try
            {

                var current = bindingSource1.Current as System.Data.DataRowView;
                var guid = Guid.NewGuid();

                var changes = this.cleanSDataset.Tables["Appointments"].GetChanges();
                changes.Rows[0]["ResourceIDs"] = guid.ToString();
                changes.AcceptChanges();

                int rowID = this.appointmentsTableAdapter.Update(this.cleanSDataset);
                this.cleanSDataset.AcceptChanges();

                DataView dv = new DataView(this.cleanSDataset.Tables["Appointments"]) { RowFilter = "ResourceIDs='" + guid.ToString() + "'" };
                int id = 0;

                if (dv.Count > 0)
                {
                    foreach (DataRowView rowView in dv)
                    {
                        id = Convert.ToInt32(rowView["UniqueID"].ToString());
                    }
                }

                if (current != null)
                {

                    Appointment app = (Appointment)e.Objects[0];

                    this.dsEmp = app.CustomFields["dsEmp"] as DataTable;

                    if (dsEmp.Rows.Count > 0)
                    {
                        var appointmentEmployeeTableAdapter = new AppointmentEmployeeTableAdapter();
                        appointmentEmployeeTableAdapter.DeleteQueryUniqueID(id);

                        foreach (DataRow row in dsEmp.Rows)
                        {
                            var select = Convert.ToBoolean(row[0].ToString());
                            if (select)
                                appointmentEmployeeTableAdapter.Insert(Convert.ToInt32(row["id"].ToString()), id);
                        }
                    }

                    schedulerControl1.Storage.SetAppointmentId((Appointment)e.Objects[0], id);
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

            if (e.DialogResult == DialogResult.OK)
            {
                this.dsEmp = frm.GetEmp;   
               
            }

            e.Handled = true;
           
            if (apt.Type == AppointmentType.Pattern && schedulerControl1.SelectedAppointments.Contains(apt))
                schedulerControl1.SelectedAppointments.Remove(apt);

            schedulerControl1.Refresh();
        }

        private void frmScheduler_Load(object sender, EventArgs e)
        {
            this.appointmentsTableAdapter.Fill(this.cleanSDataset.Appointments);

            this.cleanSDataset.Appointments.RowChanged += Appointments_RowChanged;
            this.cleanSDataset.Appointments.TableNewRow += Appointments_TableNewRow;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Appointments_TableNewRow(object sender, DataTableNewRowEventArgs e)
        {
           
        }

        private void Appointments_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            switch (e.Action)
            {
                case DataRowAction.Nothing:
                    break;
                case DataRowAction.Delete:
                    break;
                case DataRowAction.Change:
                    break;
                case DataRowAction.Rollback:
                    break;
                case DataRowAction.Commit:
                    break;
                case DataRowAction.Add:
                    e.Row["ResourceID"] = (Int32)1;
                    break;
                case DataRowAction.ChangeOriginal:
                    break;
                case DataRowAction.ChangeCurrentAndOriginal:
                    break;
                default:
                    break;
            }
        }
    }
}