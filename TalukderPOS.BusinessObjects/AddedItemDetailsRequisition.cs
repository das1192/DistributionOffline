using System;
using System.Collections.Generic;
using System.Text;

namespace TalukderPOS.BusinessObjects
{


    public class AddedItemDetailsRequisition
    {
        public int SlNo { get; set; }

        public int RequisitionDetailId { get; set; }

        public string PCategoryID { get; set; }
        public string PCategoryName { get; set; }

        public string SubCategoryID { get; set; }
        public string SubCategory { get; set; }

        public string DescriptionID { get; set; }
        public string Description { get; set; }

        public string ProductID { get; set; }
        public string ProductName { get; set; }
        public string Barcode { get; set; }

        public string UnitID { get; set; }
        public string UnitName { get; set; }
        public decimal QtyPcs { get; set; }
        public string StockInHand { get; set; }



        public string OID { get; set; }
        public string CCOM_NAME { get; set; }
        public string WGPG_NAME { get; set; }
        public string SubCategoryName { get; set; }
      
           
           
    }
   
}
