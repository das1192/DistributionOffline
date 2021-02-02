using System;
using System.Text;

namespace TalukderPOS.BusinessObjects
{
	[Serializable()]
    public class ProductPrice_BO
	{
		public string  OID { get; set; }
        public string CategoryOID { get; set; }
        public string SubCategoryOID { get; set; }
        public string DescriptionOID { get; set; }
        public string PurchasePrice { get; set; }
        public string SalePrice { get; set; }        
        public string IUSER { get; set; }
        public string EUSER { get; set; }
        public string IDAT { get; set; }
        public string EDAT { get; set; }
	}
}
