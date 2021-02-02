using System;
using System.Collections.Generic;
using System.Text;

namespace TalukderPOS.BusinessObjects
{
    [Serializable()]
    public class SalesReport_BO
    {
        public string StoreID { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }

        public string PROD_WGPG { get; set; }
        public string PROD_SUBCATEGORY { get; set; }
        public string PROD_DES { get; set; }
    }
}
