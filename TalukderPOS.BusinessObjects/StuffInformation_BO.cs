using System;
using System.Text;
using System.Collections.Generic;

namespace TalukderPOS.BusinessObjects
{
    [Serializable()]
    public class StuffInformation_BO
    {
        public String OID { get; set; }
        public string StuffID { get; set; }
        public String Name { get; set; }
        public String CCOM_OID { get; set; }
        public String MobileNumber { get; set; }

        public String AlternativeMobileNo { get; set; }
        public String EMailAddress { get; set; }
        public String AlternativeEMailAddress { get; set; }
        public string address { get; set; }
        public string nid { get; set; }
        

        public String ActiveStatus { get; set; }
        public String IUSER { get; set; }
        public String IDAT { get; set; }
        public String EUSER { get; set; }
        public String EDAT { get; set; }
        public string UserMaxID { get; set; }
    }
}
