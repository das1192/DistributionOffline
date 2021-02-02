using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using TalukderPOS.BusinessObjects;
using TalukderPOS.DAL;

namespace TalukderPOS.BLL
{
    public class BankInfoBILL
    {
        BankInfoDAL DAL = new BankInfoDAL();

        public void AddBank(BankInfo_BO obj)
        {
            try
            {
                DAL.AddBank(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public void Delete(BankInfo_BO entity)
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

        public DataTable BindList(int shopID)
        {
            try
            {
                return DAL.BindList(shopID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public BankInfo_BO GetById(string OID)
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
