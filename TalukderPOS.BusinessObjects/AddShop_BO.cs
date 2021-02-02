using System;
using System.Collections.Generic;
using System.Text;

namespace TalukderPOS.BusinessObjects
{
    public class AddShop_BO
    {
        public string OID { get; set; }
        public string ShopName { get; set; }
        public string Shop_id { get; set; }
        public string ActiveStatus { get; set; }
        public string IUSER { get; set; }
        public string IDAT { get; set; }
        public string EUSER { get; set; }
        public string EDAT { get; set; }
        public byte[]  imgarray { get; set; }
    }
}
