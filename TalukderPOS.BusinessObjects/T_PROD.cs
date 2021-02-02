using System;
using System.Text;

namespace TalukderPOS.BusinessObjects
{
    [Serializable()]
    public class T_PROD
    {
        public string OID { get; set; }
        public string TPRODID { get; set; }
        public string ACCOID { get; set; }
        public string PONUMBER { get; set; }
        public string CashTrans { get; set; }
        public string OLDQUANTITY { get; set; }
        public string PROD_WGPG { get; set; }
        public string PROD_SUBCATEGORY { get; set; }
        public string PROD_DES { get; set; }
        public string STOCK_TYPE { get; set; }
        public string Branch { get; set; }
        public string BranchText { get; set; }
        public string Barcode { get; set; }
        public string CostPrice { get; set; }
        public string SalePrice { get; set; }
        public string Quantity { get; set; }
        public string Closing { get; set; }
        public string SaleStatus { get; set; }
        public string IUSER { get; set; }
        public string IDAT { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string AccessoriesEdit { get; set; }
        public string NoOfDate { get; set; }
        public string ActiveStatus { get; set; }
        public string InActiveReason { get; set; }
        public string EUSER { get; set; }
        public string EDAT { get; set; }
        public string Action { get; set; }
        public string ReceiveAmount { get; set; }
        public string RemainingAmount { get; set; }
        public string x { get; set; }
        public string SearchType { get; set; }

        public string PaymentMode { get; set; }
        public string BankName { get; set; }

        public string DiscountType { get; set; }
        public string Reference { get; set; }
        public string FromStoreID { get; set; }
        public string ToStoreID { get; set; }
        public string SaleQuantity { get; set; }
        public string ProductPriceFrom { get; set; }
        public string ProductPriceTo { get; set; }
        public string DropStatus { get; set; }
        public string SearchOption { get; set; }
        public string Entrymode { get; set; }
        public string Vendor_ID { get; set; }
       
        //sadiq 170920
        public string RunningAvg { get; set; }
        public string Narration { get; set; }
        public string ACC_STOCKID { get; set; }

        // Yeasin 11-May-2019
        public string Flag { get; set; }
        public const string AddNewPurchase = "AddNewPurchase";
        public const string EditPurchase = "EditPurchase";
    }
}
