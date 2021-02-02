using System;
using System.Data.Common;
using System.Collections.Generic;
using System.Text;
using TalukderPOS.BusinessObjects;
using TalukderPOS.DAL;
using System.Data;

namespace TalukderPOS.BLL
{
    public class StockReportForAdmin_BILL
    {
        StockReportForAdmin_DAL DAL = new StockReportForAdmin_DAL();

        public DataTable CurrentStockInBranch(T_PROD entity)
        {
            try
            {
                return DAL.CurrentStockInBranch(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public T_PROD GetProductInformation(String OID)
        {
            try
            {
                return DAL.GetProductInformation(OID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AdjustStoreMasterStock(T_PROD entity)
        {
            try
            {
                DAL.AdjustStoreMasterStock(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable TotalStock(T_PROD entity)
        {
            try
            {
                return DAL.TotalStock(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable TotalStockQuantity(T_PROD entity)
        {
            try
            {
                return DAL.TotalStockQuantity(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetDetailData(T_PROD entity)
        {
            try
            {
                return DAL.GetDetailData(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable TotalFaultyStock(T_PROD entity)
        {
            try
            {
                return DAL.TotalFaultyStock(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable CurrentStockByBranch(T_PROD entity)
        {
            try
            {
                return DAL.CurrentStockByBranch(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable CurrentStockValue(T_PROD entity)
        {
            try
            {
                return DAL.CurrentStockValue(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable StockQuantitySummary(T_PROD entity)
        {
            try
            {
                return DAL.StockQuantitySummary(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable StockAdjustSearch(T_PROD entity)
        {
            try
            {
                return DAL.StockAdjustSearch(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable AccessoriesStockInHistory(T_PROD entity)
        {
            try
            {
                return DAL.AccessoriesStockInHistory(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable DailyClosingBalance(T_PROD entity)
        {
            try
            {
                return DAL.DailyClosingBalance(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




    }
}
