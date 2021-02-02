using System;
using System.Collections.Generic;
using System.Text;

namespace TalukderPOS.BusinessObjects
{
    [Serializable()]
    public class StockReturn_BO
    {
        public String StockReturnID { get; set; }
        public String StockReturnNo { get; set; }
        public String FromStoreID { get; set; }
        public String ApprovedStatus { get; set; }
        public String ReferenceBy { get; set; }
        public string FaultyProd { get; set; }
        public String StockReturnDetailID { get; set; }        
        public string PROD_WGPG { get; set; }
        public string PROD_SUBCATEGORY { get; set; }
        public string PROD_DES { get; set; }
        public string Branch { get; set; }
        public string BarnchText { get; set; }
        public string Barcode { get; set; }

        public String RQty { get; set; }
        public String ToStoreID { get; set; }
        public String IUSER { get; set; }
        public String IDAT { get; set; }

        public String EUSER { get; set; }
        public String EDAT { get; set; }

        public String TransferDate { get; set; }

    }
}
