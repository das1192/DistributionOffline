using System;
using System.Text;

namespace TalukderPOS.BusinessObjects
{
	[Serializable()]
    public class Vendor_Outgoing_BO
	{
		public string  OID { get; set; }
        public string Vendor_ID { get; set; }
        public string AMOUNT { get; set; }
        public string IUSER { get; set; }
        public string IDAT { get; set; }
        public string Remarks { get; set; }
        public string Shop_id { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string RefferenceNumber { get; set; }
        public string PaymentFrom { get; set; }
        public string AccountID { get; set; }
        public string Narration { get; set; }
        // newly added to get customer name and number
        public string CustomerName { get; set; }
        public string CustomerNumber { get; set; }

        // Newly Added By Yeasin 17-Jul-2019
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string ActiveStatus { get; set; }
        public string OpeningBalance { get; set; }
        public string Address { get; set; }
        public string PaymentModeID { get; set; }
        public string CardAmt { get; set; }
        public string BankId { get; set; }
        public string  PaymentId { get; set; }

	}
}
