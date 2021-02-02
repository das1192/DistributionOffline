using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using TalukderPOS.BusinessObjects;
using TalukderPOS.DAL;

namespace TalukderPOS.BLL
{
    public class AddShopBILL
    {

        AddShopDAL DAL = new AddShopDAL();
        
        public void Add(AddShop_BO obj)
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

        public void AddLogo(AddShop_BO obj)
        {
            try
            {
                DAL.AddLogo(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Delete(AddShop_BO entity)
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
        public void DeleteLogo(AddShop_BO entity)
        {
            try
            {
                DAL.DeleteLogo(entity);
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
        public DataTable BindListLogo(string Shop_id)
        {
            try
            {
                return DAL.BindListLogo(Shop_id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public AddShop_BO GetById(string OID)
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
