using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using TalukderPOS.BusinessObjects;
using TalukderPOS.DAL;

namespace TalukderPOS.BLL
{
    public class Model_BLL
    {
        Model_DAL DAL = new Model_DAL();

        public void Add(Model_BO obj)
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



        public void Delete(Model_BO entity)
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

        public DataTable BindList(Model_BO entity)
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


        public Model_BO GetById(string OID)
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

        public string[] Validation(Model_BO obj)
        {
            string[] valid = new string[2];
            try
            {
                if (obj.CategoryID == String.Empty || obj.CategoryID == "0")
                {
                    valid[0] = "False";
                    valid[1] = "PLEASE SELECT A PRODUCT CATEGORY";
                }
                else if (obj.SubCategoryName == string.Empty)
                {
                    valid[0] = "False";
                    valid[1] = "MODEL NAME IS REQUIRED";
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
