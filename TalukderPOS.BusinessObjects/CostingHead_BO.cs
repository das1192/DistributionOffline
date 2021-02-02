using System;
using System.Text;

namespace TalukderPOS.BusinessObjects
{
    [Serializable()]
    public class CostingHead_BO
    {
        public string OID { get; set; }
        public string Shop_id { get; set; }
        public string CostingHead { get; set; }
        public string ExpenseType { get; set; }
        public string IUSER { get; set; }
        public string IDAT { get; set; }
        
    }
}
