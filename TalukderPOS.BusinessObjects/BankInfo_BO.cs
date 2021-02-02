using System;
using System.Text;

namespace TalukderPOS.BusinessObjects
{
	[Serializable()]
    public class BankInfo_BO
	{
		public string  OID { get; set; }
        public string BankName { get; set; }
        public string ActiveStatus { get; set; }
        public string IUSER { get; set; }
        public string IDAT { get; set; }
        public string EUSER { get; set; }        
        public string EDAT { get; set; }

        public int ShopID { get; set; }
        public string AccountNo { get; set; }
	}
}
