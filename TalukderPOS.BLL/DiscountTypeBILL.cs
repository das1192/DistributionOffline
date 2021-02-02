using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using TalukderPOS.BusinessObjects;
using TalukderPOS.DAL;

namespace TalukderPOS.BLL
{
    public class DiscountTypeBILL
    {
        DiscountTypeDAL DAL = new DiscountTypeDAL();

        public void Add(DiscountType_BO entity)
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



        public void Delete(DiscountType_BO entity)
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

        public DataTable BindList()
        {
            try
            {
                return DAL.BindList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DiscountType_BO GetById(string OID)
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
