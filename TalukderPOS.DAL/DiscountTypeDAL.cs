using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.Common;
using System.Collections.Generic;
using TalukderPOS.BusinessObjects;
using System.Globalization;
using System.Security.Permissions;
using System.Threading;


namespace TalukderPOS.DAL
{
    public class DiscountTypeDAL
    {
        SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
        SqlDataAdapter da = new SqlDataAdapter();
        SqlCommand cmd;
        String sql;
        SqlDataReader reader;

        public void Add(DiscountType_BO entity)
        {
            if (string.IsNullOrEmpty(entity.OID))
            {
                sql = "insert into DiscountType(DiscountType,ActiveStatus,IUSER,IDAT,EUSER,EDAT) values (@DiscountType,@ActiveStatus,@IUSER,@IDAT,@EUSER,@EDAT)";
                cmd = new SqlCommand(sql, dbConnect);
                cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 50).Value = entity.IUSER;
                cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = entity.IDAT;
            }
            else
            {
                sql = "update DiscountType set DiscountType=@DiscountType,ActiveStatus=@ActiveStatus,EUSER=@EUSER,EDAT=@EDAT where OID=" + entity.OID + " ";
                cmd = new SqlCommand(sql, dbConnect);
            }
            cmd.Parameters.Add("@DiscountType", SqlDbType.VarChar, 200).Value = entity.DiscountType;
            cmd.Parameters.Add("@ActiveStatus", SqlDbType.Int).Value = entity.ActiveStatus;
            cmd.Parameters.Add("@EUSER", SqlDbType.VarChar, 50).Value = entity.EUSER;
            cmd.Parameters.Add("@EDAT", SqlDbType.Date).Value = entity.EDAT;
            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbConnect.Close();
            }
        }



        public void Delete(DiscountType_BO entity)
        {
            sql = "update DiscountType set ActiveStatus=@ActiveStatus,EUSER=@EUSER,EDAT=@EDAT where OID=" + entity.OID + " ";
            cmd = new SqlCommand(sql, dbConnect);
            cmd.Parameters.Add("@ActiveStatus", SqlDbType.Int).Value = 0;
            cmd.Parameters.Add("@EUSER", SqlDbType.VarChar, 100).Value = entity.EUSER;
            cmd.Parameters.Add("@EDAT", SqlDbType.Date).Value = DateTime.Today.Date;
            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbConnect.Close();
            }
        }


        public DataTable BindList()
        {
            DataTable dt = new DataTable();
            sql = "select OID,DiscountType,CASE ActiveStatus WHEN 1 THEN 'Y' Else 'N' END as ActiveStatus from DiscountType where ActiveStatus = 1 ";
            cmd = new SqlCommand(sql, dbConnect);
            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                da.SelectCommand = cmd;
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbConnect.Close();
            }
            return dt;
        }



        public DiscountType_BO GetById(string OID)
        {
            DiscountType_BO entity = new DiscountType_BO();
            sql = "select OID,DiscountType from DiscountType where OID=" + OID + " ";
            cmd = new SqlCommand(sql, dbConnect);
            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (string.IsNullOrEmpty(reader["OID"].ToString()))
                    {
                        entity.OID = string.Empty;
                    }
                    else
                    {
                        entity.OID = reader["OID"].ToString();
                    }

                    if (string.IsNullOrEmpty(reader["DiscountType"].ToString()))
                    {
                        entity.DiscountType = string.Empty;
                    }
                    else
                    {
                        entity.DiscountType = reader["DiscountType"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbConnect.Close();
            }
            return entity;
        }



    }
}
