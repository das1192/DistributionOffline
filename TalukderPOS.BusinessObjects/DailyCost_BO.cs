using System;
using System.Text;

namespace TalukderPOS.BusinessObjects
{
    [Serializable()]
    public class DailyCost_BO
    {
        public string OID { get; set; }
        public string CostingHeadID { get; set; }
        public string AMOUNT { get; set; }
        public string IUSER { get; set; }
        public string IDAT { get; set; }
        public string CURBALANCE { get; set; }
        public string Shop_id { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Remarks { get; set; }
        public string RefNo { get; set; }
        public string AMOUNTBUSINESS { get; set; }
        public string RemarksBUSINESS { get; set; }
        public string RefNoBUSINESS { get; set; }
        public string IDATBUSINESS { get; set; }




    }
}
