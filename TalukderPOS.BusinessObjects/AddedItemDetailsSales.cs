using System;
using System.Collections.Generic;
using System.Text;

namespace TalukderPOS.BusinessObjects
{
    public class AddedItemDetailsSales
    {
        public int SlNo { get; set; }

        public string OID { get; set; }
        public string PROD_WGPG { get; set; }
        public string WGPG_NAME { get; set; }
        public string SubCategoryName { get; set; }
        public string DescriptionID { get; set; }
        public string Description { get; set; }
        public string Barcode { get; set; }
        public string StoreID { get; set; }
        public string LotNo { get; set; }
        public decimal? SalePrice { get; set; }
        public decimal TotalPrice { get; set; }
        public int Stock { get; set; } 
        public int? Qty { get; set; }
        public int RefAmount { get; set; }
        public string SubCategoryID { get; set; }
        
    }
}
