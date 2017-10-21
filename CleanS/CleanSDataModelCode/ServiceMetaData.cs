using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanS.Infrastructure;

namespace CleanS.CleanS
{   
    public class ServiceMetaData
    {
        [Struct.CustomDisplayName("ServiceId","IdService")]
        [ReadOnly(true)]
        public int IdService { get; set; }

        [Struct.CustomDisplayName("ServiceDescription", "Description")]
        public string Description { get; set; }

        [Struct.FieldTypeAttribute("PriceList", "IdPriceList", "Amount", "PriceList")]
        [Struct.CustomDisplayName("ServiceIdPriceList", "IdPriceList")]
        public int IdPriceList { get; set; }

    }
}
