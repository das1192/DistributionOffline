using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using TalukderPOS.BusinessObjects;
using TalukderPOS.DAL;

namespace TalukderPOS.BLL
{
	public class T_SALES_DTLBLL
	{
        public T_SALES_DTLDAL DAL = new T_SALES_DTLDAL();       

       public void Add(T_SALES_DTL entity)
        {
            try
            {
                DAL.Add(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       public String getpuchaseprice(string prod_des,string shop_id)
       {
           try
           {
               return DAL.getpuchaseprice(prod_des,shop_id);
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       public String getpuchasepriceOID(string prod_des, string shop_id)
       {
           try
           {
               return DAL.getpuchasepriceOID(prod_des, shop_id);
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       public String getpuchaseavail234(string oid)
       {
           try
           {
               return DAL.getpuchaseavail234(oid);
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
        
        public void T_SALES_DTL_Add(T_SALES_DTL entity)
        {
            try
            {
                DAL.T_SALES_DTL_Add(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void T_SALES_DTL_Journal(T_SALES_DTL entity)
        {
            try
            {
                DAL.T_SALES_DTL_Journal(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void T_SALES_DTL_Stock(T_SALES_DTL entity)
        {
            try
            {
                DAL.T_SALES_DTL_Stock(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void T_SALES_DTL_StockNew(T_SALES_DTL entity)
        {
            try
            {
                DAL.T_SALES_DTL_StockNew(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void T_SALES_Acc_Journal(T_SALES_DTL entity)
        {
            try
            {
                DAL.T_SALES_Acc_Journal(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void T_SALES_Acc_Journal2(T_SALES_DTL entity)
        {
            try
            {
                DAL.T_SALES_Acc_Journal2(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void T_SALES_Acc_Journal3(T_SALES_DTL entity)
        {
            try
            {
                DAL.T_SALES_Acc_Journal3(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void T_SALES_Acc_Journal4(T_SALES_DTL entity)
        {
            try
            {
                DAL.T_SALES_Acc_Journal4(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        //new9/18
        public void T_SALES_Acc_Journal_oncr(T_SALES_DTL entity)
        {
            try
            {
                DAL.T_SALES_Acc_Journal_oncr(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void T_SALES_Acc_Journalforcard(T_SALES_DTL entity)
        {
            try
            {
                DAL.T_SALES_Acc_Journalforcard(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void T_SALES_Acc_JournalForDiscount(T_SALES_DTL entity)
        {
            try
            {
                DAL.T_SALES_Acc_JournalForDiscount(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void T_SALES_Acc_JournalForGiftDiscount(T_SALES_DTL entity)
        {
            try
            {
                DAL.T_SALES_Acc_JournalForGiftDiscount(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
      
        
        
        //new adjust
        public void Adjust_Oncredit2(T_SALES_DTL entity)
        {
            try
            {
                DAL.Adjust_Oncredit2(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Adjust_Oncredit(T_SALES_DTL entity)
        {
            try
            {
                DAL.Adjust_Oncredit(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public T_SALES_DTL GetSalesInformation(String Invoiceid)
        {
            try
            {
                return DAL.GetSalesInformation(Invoiceid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable SPP_GetInvoice(String InvoiceNo)
        {
            try
            {
                return DAL.SPP_GetInvoice(InvoiceNo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable SPP_GetProductForSale(T_PROD entity)
        {
            try
            {
                return DAL.SPP_GetProductForSale(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //103 barcode  sadiq
        public DataTable SPP_GetProductForSaleByBarcode(T_PROD entity)
        {
            try
            {
                return DAL.SPP_GetProductForSaleByBarcode(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public double GetNetAmount(string subtotal, string discount)
        {
            double netamount;
            if (string.IsNullOrEmpty(subtotal))
            {
                subtotal = "0";
            }
            else if (string.IsNullOrEmpty(discount))
            {
                discount = "0";
            }
            netamount = Convert.ToDouble(subtotal) - Convert.ToDouble(discount);
            return netamount;
        }

        public double GetVatAmount(string netamount, string vat)
        {
            double vatamount;
            if (string.IsNullOrEmpty(netamount))
            {
                netamount = "0";
            }
            else if (string.IsNullOrEmpty(vat))
            {
                vat = "0";
            }
            vatamount = ((Convert.ToDouble(netamount) * Convert.ToDouble(vat)) / 100);
            return vatamount;
        }

        // Added By Yeasin 15-May-2019
        public void AddSalesMaster(T_SALES_DTL entity)
        {
            try
            {
                DAL.AddSalesMaster(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Added By Yeasin 15-May-2019
        public void AddSalesDetails(T_SALES_DTL entity)
        {
            try
            {
                DAL.AddSalesDetails(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

	}
}
