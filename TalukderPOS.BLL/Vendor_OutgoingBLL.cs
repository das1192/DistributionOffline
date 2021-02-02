using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using TalukderPOS.BusinessObjects;
using TalukderPOS.DAL;

namespace TalukderPOS.BLL
{
    public class Vendor_Outgoing
    
    {
        Vendor_OutgoingDAL DAL = new Vendor_OutgoingDAL();

        public void Add(Vendor_Outgoing_BO obj)
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


        public void AddCommission(Vendor_Outgoing_BO obj)
        {
            try
            {
                DAL.AddCommission(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddCustomer(Vendor_Outgoing_BO obj)
        {
            try
            {
                DAL.AddCustomer(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public DataTable BindListdelete(Vendor_Outgoing_BO entity)
        {
            try
            {
                return DAL.BindListdelete(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Delete(Vendor_Outgoing_BO entity)
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

        public DataTable BindList(Vendor_Outgoing_BO entity)
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


        public Vendor_Outgoing_BO GetById(string OID)
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

        public Vendor_Outgoing_BO GetRetailerById(string OID)
        {
            try
            {
                return DAL.GetRetailerById(OID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Retailer Payment Adjustment
        // Date:- 22-Jul-2019
        public void AddRetailerPaymentAdj(Vendor_Outgoing_BO entity) 
        {
            try
            {
                DAL.AddRetailerPaymentAdj(entity);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public DataTable RetailerPaymentHistory(Vendor_Outgoing_BO entity) 
        {
            try
            {
                return DAL.GetRetailerList(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Vendor_Outgoing_BO RetailerPaymentById(string OID)
        {
            try
            {
                return DAL.GetRetailerPaymentById(OID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
