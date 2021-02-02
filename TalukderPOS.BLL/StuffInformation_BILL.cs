using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using TalukderPOS.BusinessObjects;
using TalukderPOS.DAL;


namespace TalukderPOS.BLL
{
    public class StuffInformation_BILL
    {
        StuffInformation_DAL dal = new StuffInformation_DAL();

        public void Add(StuffInformation_BO entity)
        {
            try
            {
                dal.Add(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void StuffInformation_Update(StuffInformation_BO entity)
        {
            try
            {
                dal.update(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void StuffInformation_Delete(StuffInformation_BO entity)
        {
            try
            {
                dal.Delete(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable StuffInformation_BindList(StuffInformation_BO entity)
        {
            try
            {
                return dal.StuffInformation_BindList(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<StuffInformation_BO> GetById(string OID)
        {
            try
            {
                return dal.GetById(OID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




    }


}
