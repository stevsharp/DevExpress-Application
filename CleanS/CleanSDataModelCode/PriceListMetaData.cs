using System.ComponentModel;
using CleanS.Infrastructure;

namespace CleanS.CleanS
{
    public class PriceListMetaData
    {
        [Struct.CustomDisplayName("PriceListId","IdPriceList")]
        [ReadOnly(true)]
        public int IdPriceList { get; set; }

        [Struct.CustomDisplayName("PriceListAmount", "Amount")]
        public double Amount { get; set; }
    }
}