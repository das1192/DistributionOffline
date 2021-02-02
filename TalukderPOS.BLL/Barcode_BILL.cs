using System;
using System.Data.Common;
using System.Collections.Generic;
using System.Text;
using TalukderPOS.BusinessObjects;
using TalukderPOS.DAL;
using System.Data;

namespace TalukderPOS.BLL
{
    public class Barcode_BILL
    {
        Barcode_DAL dal = new Barcode_DAL();

        public void Add(Barcode_BO obj)
        {
            try
            {
                dal.Add(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Delete(String OID)
        {
            try
            {
                dal.Delete(OID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } 
       

        public string GetBarcode(String Barcode)
        {
            try
            {
                return dal.GetBarcode(Barcode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable Gettable(String PROD_DES)
        {
            try
            {
                return dal.Gettable(PROD_DES);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetBarcodeForPROD_DES(String PROD_DES)
        {
            try
            {
                return dal.GetBarcodeForPROD_DES(PROD_DES);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable Search(Barcode_BO entity)
        {
            try
            {
                return dal.Search(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable StoreMasterStockAccessoriesBarcodeList(Barcode_BO entity)
        {
            try
            {
                return dal.StoreMasterStockAccessoriesBarcodeList(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }





    }
}
