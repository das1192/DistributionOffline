using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using TalukderPOS.BusinessObjects;
using TalukderPOS.DAL;

namespace TalukderPOS.BLL
{
    public class Color_BLL
    {
        Color_DAL DAL = new Color_DAL();

        public void Add(Color_BO entity)
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



        public void Delete(Color_BO entity)
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
        public void ActiveDescription(Color_BO entity)
        {
            try
            {
                DAL.ActiveDescription(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable BindList(Color_BO entity)
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


        public Color_BO GetById(string OID)
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

        public string[] Validation(Color_BO entity)
        {
            string[] valid = new string[2];
            try
            {
                //if (entity.SubCategoryID == string.Empty || entity.SubCategoryID == "0")
                //{
                //    valid[0] = "False";
                //    valid[1] = "PLEASE SELECT A MODEL";
                //}
                //else 
               if (entity.SESPrice == string.Empty)
                {
                    valid[0] = "False";
                    valid[1] = "PLEASE INSERT SES PRICE";
                }
                else if (entity.MRP == string.Empty)
                {
                    valid[0] = "False";
                    valid[1] = "PLEASE INSERT MRP";
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
