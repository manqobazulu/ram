using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSCustomerPortal.Models.Common {
    public class ProductAutocomplete {
        public int ProductStagingID { get; set; }
        public string ProdCode { get; set; }
        public string EANCode { get; set; }
        public string ShortDesc { get; set; }
        public string LongDesc { get; set; }
        public decimal? UnitCost { get; set; }
        public decimal? SalesPrice { get; set; }

        public ProductAutocomplete() {

        }

        public ProductAutocomplete(int pProductStagingID, string pProdCode, string pEANCode, string pShortDesc, string pLongDesc, decimal? pUnitCost, decimal? pSalesPrice) {
            ProductStagingID = pProductStagingID;
            ProdCode = pProdCode;
            EANCode = pEANCode;
            ShortDesc = pShortDesc;
            LongDesc = pLongDesc;
            UnitCost = pUnitCost;
            SalesPrice = pSalesPrice;            
        }
    }
}
