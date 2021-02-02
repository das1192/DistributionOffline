using System;
using System.Text;

namespace TalukderPOS.BusinessObjects
{
	[Serializable()]
	public class DiscountType_BO
	{
		public string  OID { get; set; }
        public string DiscountType { get; set; }
        public string ActiveStatus { get; set; }
        public string IUSER { get; set; }
        public string IDAT { get; set; }
        public string EUSER { get; set; }        
        public string EDAT { get; set; }
	}
}
