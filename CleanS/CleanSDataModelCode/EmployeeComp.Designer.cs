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
using System.ComponentModel.DataAnnotations;

namespace CleanS.CleanS
{

    [MetadataType(typeof(EmployeeCompMetaData))]
    public partial class EmployeeComp : BaseObject
    {
        int fIdEmployeeComp;
        [DevExpress.Xpo.Key(true)]
        public int IdEmployeeComp
        {
            get { return fIdEmployeeComp; }
            set { SetPropertyValue<int>("IdEmployeeComp", ref fIdEmployeeComp, value); }
        }
        string fDescription;
        public string Description
        {
            get { return fDescription; }
            set { SetPropertyValue<string>("Description", ref fDescription, value); }
        }
        double fSum;
        public double Sum
        {
            get { return fSum; }
            set { SetPropertyValue<double>("Sum", ref fSum, value); }
        }
        //[Association(@"EmployeeReferencesEmployeeComp")]
        //public XPCollection<Employee> Employees { get { return GetCollection<Employee>("Employees"); } }
    }

}