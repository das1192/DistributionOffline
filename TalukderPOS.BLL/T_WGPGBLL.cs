using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using TalukderPOS.BusinessObjects;
using TalukderPOS.DAL;

namespace TalukderPOS.BLL
{
    public class T_WGPGBLL
    {
        T_WGPGDAL DAL = new T_WGPGDAL();

        public void Add(T_WGPG obj)
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

        public string getcatname(string catname, string shopid)
        {
            try
            {
                return DAL.getcatname(catname, shopid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Delete(T_WGPG entity)
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

        public DataTable BindList(T_WGPG entity)
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


        public T_WGPG GetById(string OID)
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
