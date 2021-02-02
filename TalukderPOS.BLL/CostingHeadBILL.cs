﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using TalukderPOS.BusinessObjects;
using TalukderPOS.DAL;

namespace TalukderPOS.BLL
{
    public class CostingHeadBILL
    {
        CostingHeadDAL DAL = new CostingHeadDAL();

        public void Add(CostingHead_BO obj)
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



        public void Delete(CostingHead_BO entity)
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

        public DataTable BindList(CostingHead_BO entity)
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


        public CostingHead_BO GetById(string OID)
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
