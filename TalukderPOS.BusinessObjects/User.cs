using System;
using System.Text;

namespace TalukderPOS.BusinessObjects
{
    [Serializable()]
    public class User
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string CCOM_OID { get; set; }
        public string UserTypeOID { get; set; }
        public string UserFullName { get; set; }        
        public string EmailID { get; set; }
        public string ActiveStatus { get; set; }
        public string IUSER { get; set; }
        public string IDAT { get; set; }
        public string EUSER { get; set; }
        public string EDAT { get; set; }
        public string address { get; set; }
        public string nid { get; set; }
        public string MobileNumber { get; set; }
        public string AlternativeMobileNo { get; set; }
        public int StuffID { get; set; }
        

            }


}
