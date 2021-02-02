using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using TalukderPOS.BusinessObjects;
using TalukderPOS.DAL;

namespace TalukderPOS.BLL
{
    public class ProductTransferToSIS_BILL
    {
        ProductTransferToSIS_DAL DAL = new ProductTransferToSIS_DAL();

        public void T_PROD_Add(T_PROD entity)
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

        public string[] AddValidation(T_PROD obj)
        {
            string[] valid = new string[2];
            try
            {
                if (obj.PROD_WGPG.ToString() == String.Empty || obj.PROD_WGPG.ToString() == "0")
                {
                    valid[0] = "False";
                    valid[1] = "PLEASE SELECT A PRODUCT CATEGORY";
                }
                else if (obj.PROD_SUBCATEGORY.ToString() == String.Empty || obj.PROD_SUBCATEGORY.ToString() == "0")
                {
                    valid[0] = "False";
                    valid[1] = "PLEASE SELECT A MODEL";
                }
                else if (obj.PROD_DES.ToString() == String.Empty || obj.PROD_DES.ToString() == "0")
                {
                    valid[0] = "False";
                    valid[1] = "PLEASE SELECT A COLOR/DESCRIPTION";
                }
                else if (obj.Branch.ToString() == String.Empty || obj.Branch.ToString() == "0")
                {
                    valid[0] = "False";
                    valid[1] = "PLEASE SELECT A BRANCH";
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

        public string[] ValidationForPriceUpdate(T_PROD obj)
        {
            string[] valid = new string[2];
            try
            {
               if (obj.SalePrice == String.Empty)
                {
                    valid[0] = "False";
                    valid[1] = "PLEASE ENTER UPDATE SALE PRICE";
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

        public T_PROD FindBarcode(String Barcode)
        {
            try
            {
                return DAL.FindBarcode(Barcode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String CheckProductonSIS(String Barcode)
        {
            try
            {
                return DAL.CheckProductonSIS(Barcode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
