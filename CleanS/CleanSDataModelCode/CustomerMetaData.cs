using MyEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Resources;
using CleanS.Infrastructure;
using DevExpress.Data.Filtering.Helpers;
using DevExpress.Utils.Extensions;
using DevExpress.Xpo;
using DevExpress.XtraScheduler;

namespace CleanS
{
    public sealed class CustomerMetaData
    {
        [Struct.CustomDisplayName("CustomerId", "IdCustomer")]
        [ReadOnly(true)]
        public int IdCustomer { get; set; }

        [Struct.CustomDisplayName("CustomerFirstName", "FirstName")]
        [Required]
        public string FirstName { get; set; }

        [Struct.CustomDisplayName("CustomerLastName", "LastName")]
        [Required]
        public string LastName { get; set; }

        [Struct.CustomDisplayName("CustomerAfm","Vat")]
        public string Vat { get; set; }

        [Struct.CustomDisplayName("CustomerDoy", "Doy")]
        public string Doy { get; set; }
        
        [Struct.CustomDisplayName("CustomerCompany", "CompanyName")]
        public string CompanyName { get; set; }
    }
}