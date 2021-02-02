using System;
using System.Collections.Generic;
using System.Text;

namespace TalukderPOS.BusinessObjects
{
    [Serializable()]
    public class NewsFeed_BO
    {
        public string OID { get; set; }
        public string FromDate { get; set; }    
        public string ToDate { get; set; }
        public string BranchOID { get; set; }
        public string Message { get; set; }
        public string ActiveStatus { get; set; }
        public string IUSER { get; set; }
        public string IDAT { get; set; }
        public string EUSER { get; set; }
        public string EDAT { get; set; }
    }
}
