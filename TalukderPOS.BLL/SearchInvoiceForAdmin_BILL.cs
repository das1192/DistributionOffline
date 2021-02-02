using System;
using System.Data.Common;
using System.Collections.Generic;
using System.Text;
using TalukderPOS.BusinessObjects;
using TalukderPOS.DAL;
using System.Data;

namespace TalukderPOS.BLL
{
    public class SearchInvoiceForAdmin_BILL
    {
        SearchInvoiceForAdmin_DAL DAL = new SearchInvoiceForAdmin_DAL();

        public DataTable GetInvoiceList(T_PROD entity)
        {
            try
            {
                return DAL.GetInvoiceList(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable DetailDatabase(T_PROD entity)
        {
            try
            {
                return DAL.DetailDatabase(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetCASHINOUT(T_PROD entity)
        {
            try
            {
                return DAL.GetCASHINOUT(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable CustomerCare(T_PROD entity)
        {
            try
            {
                return DAL.CustomerCare(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SPP_GetInvoice(string InvoiceNo)
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


        public DataTable GetDropInvoiceList(T_PROD entity)
        {
            try
            {
                return DAL.GetDropInvoiceList(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       





    }
}
