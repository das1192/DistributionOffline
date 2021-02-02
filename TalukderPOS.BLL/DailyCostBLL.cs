using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using TalukderPOS.BusinessObjects;
using TalukderPOS.DAL;

namespace TalukderPOS.BLL
{
    public class DailyCost    
    {
        DailyCostDAL DAL = new DailyCostDAL();

        public void Add(DailyCost_BO obj)
        {
            try
            {
                DAL.Add(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String getpettycash(string shop_id)
        {
            try
            {
                return DAL.getpettycash(shop_id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Delete(DailyCost_BO entity)
        {
            try
            {
                DAL.Delete(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BindList(DailyCost_BO entity)
        {
            try
            {
                return DAL.BindList(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable CurrentCashBalance(DailyCost_BO entity)
        {
            try
            {
                return DAL.CurrentCashBalance(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable CashFlowSearch(DailyCost_BO entity)
        {
            try
            {
                return DAL.CashFlowSearch(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DailyCost_BO GetById(string OID)
        {
            try
            {
                return DAL.GetById(OID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
