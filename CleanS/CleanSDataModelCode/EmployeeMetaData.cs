using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CleanS.Infrastructure;
using DevExpress.Xpo;

namespace CleanS.CleanS
{
    public class EmployeeMetaData
    {
        [Struct.CustomDisplayName("EmployeeId","IdEmployee")]
        [ReadOnly(true)]
        public int IdEmployee { get; set; }

        [Struct.CustomDisplayName("EmployeeFirstName","FirstName")]
        [Required]
        public string FirstName { get; set; }

        [Struct.CustomDisplayName("EmployeeLastName", "LastName")]
        [Required]
        public string LastName { get; set; }

        [Struct.CustomDisplayName("EmployeeVat", "Vat")]
        [Required]
        public string Vat { get; set; }

        [Struct.CustomDisplayName("EmployeeDoy", "Doy")]
        //[Required]
        public string Doy { get; set; }

        [Struct.CustomDisplayName("EmployeeWage", "΅Wage")]
        //[Required]
        public double Wage { get; set; }

        //[Required]
        [Struct.FieldTypeAttribute("EmployeeComp", "IdEmployeeComp", "Description", "EmployeeComp")]
        [Struct.CustomDisplayName("EmployeeIdEmployeeComp", "IdEmployeeComp")]
        [Browsable(false)]
        public EmployeeComp IdEmployeeComp { get; set; }

        //[Required]
        [Struct.FieldTypeAttribute("EmployeeGroup", "IdEmployeeGroup", "Description", "EmployeeGroup")]
        [Struct.CustomDisplayName("EmployeeIdEmployeeGroup", "IdEmployeeGroup")]
        [Browsable(false)]
        public EmployeeGroup IdEmployeeGroup { get; set; }



    }
}