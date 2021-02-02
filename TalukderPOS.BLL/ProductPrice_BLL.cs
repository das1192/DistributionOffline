using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using TalukderPOS.BusinessObjects;
using TalukderPOS.DAL;

namespace TalukderPOS.BLL
{
    public class ProductPrice_BILL
	{
        ProductPrice_DAL DAL = new ProductPrice_DAL();

        public void Add(ProductPrice_BO obj)
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



        public void Delete(ProductPrice_BO entity)
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

        public DataTable BindList(ProductPrice_BO entity)
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


        public ProductPrice_BO GetById(string OID)
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

        public string[] Validation(ProductPrice_BO obj)
        {
            string[] valid = new string[2];
            try
            {
                if (obj.DescriptionOID == String.Empty || obj.DescriptionOID == "0")
                {
                    valid[0] = "False";
                    valid[1] = "PLEASE SELECT A MODEL/DESCRIPTION";
                }
                else if (obj.PurchasePrice == string.Empty)
                {
                    valid[0] = "False";
                    valid[1] = "PLEASE INPUT PURCHASE PRICE";
                }
                else if (obj.SalePrice == string.Empty)
                {
                    valid[0] = "False";
                    valid[1] = "PLEASE INPUT SALE PRICE";
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
