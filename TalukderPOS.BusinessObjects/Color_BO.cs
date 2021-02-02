using System;
using System.Text;

namespace TalukderPOS.BusinessObjects
{
    [Serializable()]
    public class Color_BO
    {
        public string OID { get; set; }
        public string CategoryID { get; set; }
        public string SubCategoryID { get; set; }
        public string Description { get; set; }
        public string Active { get; set; }

        public string SESPrice { get; set; }
        public string MRP { get; set; }

        public string Shop_id { get; set; }
        public string IUSER { get; set; }
        public string EUSER { get; set; }
        public string IDAT { get; set; }
        public string EDAT { get; set; }
    }
}
