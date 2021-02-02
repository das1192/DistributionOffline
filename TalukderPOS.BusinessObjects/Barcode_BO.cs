using System;
using System.Collections.Generic;
using System.Text;

namespace TalukderPOS.BusinessObjects
{
    [Serializable()]
    public class Barcode_BO
    {
        public String OID { get; set; }
        public String PROD_WGPG { get; set; }
        public String PROD_SUBCATEGORY { get; set; }
        public String PROD_DES { get; set; }
        public String BARCODE { get; set; }
        public String Branch { get; set; }
    }
}
