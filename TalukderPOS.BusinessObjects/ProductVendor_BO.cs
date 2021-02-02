using System;
using System.Text;

namespace TalukderPOS.BusinessObjects
{
    [Serializable()]
    public class ProductVendor_BO
    {
        public string OID { get; set; }
        public string Shop_id { get; set; }
        public string Vendor_Name { get; set; }
        public string Vendor_Address { get; set; }
        public string Vendor_tr { get; set; }
        public string Vendor_mobile { get; set; }
        public string Vendor_Active { get; set; }
        public string IUSER { get; set; }
        public string IDAT { get; set; }
        public string EUSER { get; set; }
        public string EDAT { get; set; }
        
    }
}
