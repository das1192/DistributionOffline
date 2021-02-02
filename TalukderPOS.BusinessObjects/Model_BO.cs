using System;
using System.Text;

namespace TalukderPOS.BusinessObjects
{
    [Serializable()]
    public class Model_BO
    {
        public string OID { get; set; }
        public string CategoryID { get; set; }
        public string SubCategoryName { get; set; }
        public string Active { get; set; }
        public string ShowOnDropdown { get; set; }
        public string RunningModel { get; set; }
        public string IUSER { get; set; }
        public string EUSER { get; set; }
        public string IDAT { get; set; }
        public string EDAT { get; set; }
        public string Shop_id { get; set; }
    }
}
