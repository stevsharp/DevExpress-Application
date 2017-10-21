using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanS.Infrastructure;

namespace CleanS.CleanS
{
    public class ContractMetaData
    {
        [Struct.CustomDisplayName("ContractId", "IdContract")]
        [ReadOnly(true)]
        public int IdContract { get; set; }

        [Struct.FieldTypeAttribute("Customer", "IdCustomer", "LastName", "Customer")]
        [Struct.CustomDisplayName("ContractCustomerId", "IdCustomer")]
        public Customer IdCustomer { get; set; }

        [Struct.CustomDisplayName("ContractCreateDate", "CreateDate")]
        public DateTime CreateDate { get; set; }

        [Struct.CustomDisplayName("ContractHours", "Hours")]
        public double Hours { get; set; }
    }
}
