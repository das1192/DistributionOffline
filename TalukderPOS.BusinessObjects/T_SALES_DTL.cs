using System;
using System.Text;

namespace TalukderPOS.BusinessObjects
{
	[Serializable()]
	public class T_SALES_DTL
	{
        public string SlNo { get; set; }
        public string StoreID { get; set; }
        public string InvoiceNo { get; set; }
        public string PaymentModeID { get; set; }
        public string DebitAmount { get; set; }
        public string PassedAmount { get; set; }
        public string CreditAmount { get; set; }
        public string BankInfoOID { get; set; }
        public string SubTotal { get; set; }
        public string Discount { get; set; }
        public string DiscountReferenceOID { get; set; }
        public string DiscountAmount { get; set; }
        public string GiftAmount { get; set; }
        public string DiscountReferencedBy { get; set; }
        public string Vat { get; set; }
        public string VatReferencedBy { get; set; }
        public string VatAmountCashed { get; set; }
        public string NetAmount { get; set; }
       
        public string ReceiveAmount { get; set; }
        public string CashPaid { get; set; }
        public string CashChange { get; set; }
        public string DropStatus { get; set; }
        public string StuffID { get; set; }
        public string Remarks { get; set; }
        public string IUSER { get; set; }
        public string IDAT { get; set; }
        public string EUSER { get; set; }
        public string EDAT { get; set; }
        public string PURCHASECOST { get; set; }
        public string PURCHASECOSTOID { get; set; }
        public string SubCategoryID { get; set; }
        public string OID { get; set; }
        public string CategoryID { get; set; }
        public string DescriptionID { get; set; }
        public string Barcode { get; set; }
        public string SalePrice { get; set; }
        public string SaleQty { get; set; }
        public string ReturnQty { get; set; }

        public string CostPrice { get; set; }
        public string RemainingAmount { get; set; }
        public string LedgerAccID { get; set; }
        public string LedgerAccCusName { get; set; }
        public string LedgerAccParticular { get; set; }
        public string LedgerAccRemarks { get; set; }
        public string LedgerAccCardPaid { get; set; }

        // new
        public string AccountID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerID { get; set; }
        public string Address { get; set; }        
        public string MobileNo { get; set; }
        public string AlternativeMobileNo { get; set; }
        public string DateOfBirth { get; set; }
        public string EmailAddress { get; set; }

        //jouranl
        public string Narration { get; set; }

        // Added By Yeasin 15-May-2019
        public string BankId { get; set; }
        public string CardAmount { get; set; }
        //added by das
        public string PreviousDue { get; set; }
	}
}
