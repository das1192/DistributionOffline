using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using TalukderPOS.BusinessObjects;
using TalukderPOS.DAL;

namespace TalukderPOS.BLL
{
    public class DiscountReference_BILL
    {
        DiscountReference_DAL DAL = new DiscountReference_DAL();

        public void Add(DiscountReference_BO entity)
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



        public void Delete(DiscountReference_BO entity)
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

        public DataTable BindList(DiscountReference_BO entity)
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


        public DiscountReference_BO GetById(string OID)
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

        public string[] Validation(DiscountReference_BO entity)
        {
            string[] valid = new string[2];
            try
            {
                if (entity.DiscountTypeOID == String.Empty || entity.DiscountTypeOID == "0")
                {
                    valid[0] = "False";
                    valid[1] = "PLEASE SELECT A DISCOUNT TYPE";
                }
                else if (entity.Reference == string.Empty)
                {
                    valid[0] = "False";
                    valid[1] = "DISCOUNT REFERENCE IS REQUIRED";
                }
                else
                {
                    valid[0] = "True";
                    valid[1] = String.Empty;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return valid;
        }



    }
}
