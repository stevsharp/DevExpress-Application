﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;

namespace CleanS.CleanS
{

    public partial class Appointments : XPLiteObject
    {
        int fUniqueID;
        [Key(true)]
        public int UniqueID
        {
            get { return fUniqueID; }
            set { SetPropertyValue<int>("UniqueID", ref fUniqueID, value); }
        }
        int fType;
        public int Type
        {
            get { return fType; }
            set { SetPropertyValue<int>("Type", ref fType, value); }
        }
        DateTime fStartDate;
        public DateTime StartDate
        {
            get { return fStartDate; }
            set { SetPropertyValue<DateTime>("StartDate", ref fStartDate, value); }
        }
        DateTime fEndDate;
        public DateTime EndDate
        {
            get { return fEndDate; }
            set { SetPropertyValue<DateTime>("EndDate", ref fEndDate, value); }
        }
        bool fAllDay;
        public bool AllDay
        {
            get { return fAllDay; }
            set { SetPropertyValue<bool>("AllDay", ref fAllDay, value); }
        }
        string fSubject;
        [Size(50)]
        public string Subject
        {
            get { return fSubject; }
            set { SetPropertyValue<string>("Subject", ref fSubject, value); }
        }
        string fLocation;
        [Size(50)]
        public string Location
        {
            get { return fLocation; }
            set { SetPropertyValue<string>("Location", ref fLocation, value); }
        }
        string fDescription;
        [Size(SizeAttribute.Unlimited)]
        public string Description
        {
            get { return fDescription; }
            set { SetPropertyValue<string>("Description", ref fDescription, value); }
        }
        int fStatus;
        public int Status
        {
            get { return fStatus; }
            set { SetPropertyValue<int>("Status", ref fStatus, value); }
        }
        int fLabel;
        public int Label
        {
            get { return fLabel; }
            set { SetPropertyValue<int>("Label", ref fLabel, value); }
        }
        int fResourceID;
        public int ResourceID
        {
            get { return fResourceID; }
            set { SetPropertyValue<int>("ResourceID", ref fResourceID, value); }
        }
        string fResourceIDs;
        [Size(SizeAttribute.Unlimited)]
        public string ResourceIDs
        {
            get { return fResourceIDs; }
            set { SetPropertyValue<string>("ResourceIDs", ref fResourceIDs, value); }
        }
        string fReminderInfo;
        [Size(SizeAttribute.Unlimited)]
        public string ReminderInfo
        {
            get { return fReminderInfo; }
            set { SetPropertyValue<string>("ReminderInfo", ref fReminderInfo, value); }
        }
        string fRecurrenceInfo;
        [Size(SizeAttribute.Unlimited)]
        public string RecurrenceInfo
        {
            get { return fRecurrenceInfo; }
            set { SetPropertyValue<string>("RecurrenceInfo", ref fRecurrenceInfo, value); }
        }
        int fIdContract;
        public int IdContract
        {
            get { return fIdContract; }
            set { SetPropertyValue<int>("IdContract", ref fIdContract, value); }
        }
        [Association(@"AppointmentEmployeeReferencesAppointments")]
        public XPCollection<AppointmentEmployee> AppointmentEmployees { get { return GetCollection<AppointmentEmployee>("AppointmentEmployees"); } }
    }

}
