using System;
using System.Text;

namespace TalukderPOS.BusinessObjects
{
    [Serializable()]
    public class DailyPettyCash_BO
    {
        public string OID { get; set; }
        public string Shop_id { get; set; }
        public string Amount { get; set; }      
        public string IUSER { get; set; }
        public string IDAT { get; set; }
        public string CURBALANCE { get; set; }
    }
}
