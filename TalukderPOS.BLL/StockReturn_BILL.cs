using System;
using System.Data.Common;
using System.Collections.Generic;
using System.Text;
using TalukderPOS.BusinessObjects;
using TalukderPOS.DAL;
using System.Data;

namespace TalukderPOS.BLL
{
    public class StockReturn_BILL
    {
        StockReturn_DAL DAL = new StockReturn_DAL();

        public String Add_StockReturnMST(StockReturn_BO entity)
        {
            try
            {
                return DAL.Add_StockReturnMST(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Add_StockReturnDTL(StockReturn_BO entity)
        {
            try
            {
                DAL.Add_StockReturnDTL(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }         


        public DataTable txtBarcode_TextChanged(T_PROD entity)
        {
            try
            {
                return DAL.txtBarcode_TextChanged(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable txtBarcode_TextChangedDOA(T_PROD entity)
        {
            try
            {
                return DAL.txtBarcode_TextChanged(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable txtBarcode_TextChangedDeleteProduct(T_PROD entity)
        {
            try
            {
                return DAL.txtBarcode_TextChangedDeleteProduct(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable GetStockReturnList(T_PROD entity)
        {
            try
            {
                return DAL.GetStockReturnList(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetReceiveStockReturnList(T_PROD entity)
        {
            try
            {
                return DAL.GetReceiveStockReturnList(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetReceiveStockReturnListReceived(T_PROD entity)
        {
            try
            {
                return DAL.GetReceiveStockReturnListReceived(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable StockReturnNotReceivedDetails(T_PROD entity)
        {
            try
            {
                return DAL.StockReturnNotReceivedDetails(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable StockReturn_Detail(StockReturn_BO entity)
        {
            try
            {
                return DAL.StockReturn_Detail(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable StockReturn_Detail_ForPreview(StockReturn_BO entity)
        {
            try
            {
                return DAL.StockReturn_Detail_ForPreview(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public string DeleteStockReturn(StockReturn_BO entity)
        {
            try
            {
                return DAL.DeleteStockReturn(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string DeleteStockReturnItem(string StockReturnDetailID)
        {
            try
            {
                return DAL.DeleteStockReturnItem(StockReturnDetailID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ReceiveStockReturn(StockReturn_BO entity)
        {
            try
            {
                DAL.ReceiveStockReturn(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

      





    }
}
