using System;
using System.Collections.Generic;
using System.Text;

namespace TalukderPOS.BusinessObjects
{


    public class AddedItemDetailsGRN
    {
        public int ReceiveDetailId { get; set; }
        public string PCategoryID { get; set; }
        public string PCategoryName { get; set; }

        public string ProductID { get; set; }
        public string ProductName { get; set; }

        public string UnitID { get; set; }
        public string UnitName { get; set; }
        public decimal QtyPcs { get; set; }
        public string LotNo { get; set; }
       
        
              
        
        
    }
   
}
