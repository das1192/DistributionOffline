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
    public class T_WGPGDAL
    {
        SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
        SqlDataAdapter da = new SqlDataAdapter();
        SqlCommand cmd;
        String sql;
        SqlDataReader reader;

        public void Add(T_WGPG obj)
        {
            if (string.IsNullOrEmpty(obj.OID))
            {
                sql = "insert into T_WGPG(Shop_id,WGPG_NAME,WGPG_ACTV,IUSER,IDAT,EUSER,EDAT) values (@Shop_id,@WGPG_NAME,@WGPG_ACTV,@IUSER,@IDAT,@EUSER,@EDAT)";
                cmd = new SqlCommand(sql, dbConnect);
                cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 100).Value = obj.IUSER;
                cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = DateTime.Today.Date;
            }
            else
            {
                sql = "update T_WGPG set WGPG_NAME=@WGPG_NAME,WGPG_ACTV=@WGPG_ACTV,EUSER=@EUSER,EDAT=@EDAT where OID=" + obj.OID + " ";
                cmd = new SqlCommand(sql, dbConnect);
            }
            cmd.Parameters.Add("@WGPG_NAME", SqlDbType.VarChar, 100).Value = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(obj.WGPG_NAME.ToLower());
            cmd.Parameters.Add("@WGPG_ACTV", SqlDbType.VarChar, 100).Value = obj.WGPG_ACTV;
            cmd.Parameters.Add("@Shop_id", SqlDbType.VarChar, 100).Value = obj.Shop_id;
            cmd.Parameters.Add("@EUSER", SqlDbType.VarChar, 100).Value = obj.EUSER;
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



        public void Delete(T_WGPG obj)
        {
            sql = "update T_WGPG set WGPG_ACTV=@WGPG_ACTV,EUSER=@EUSER,EDAT=@EDAT where OID=" + obj.OID + " ";
            cmd = new SqlCommand(sql, dbConnect);
            cmd.Parameters.Add("@WGPG_ACTV", SqlDbType.VarChar, 1).Value = "0";
            cmd.Parameters.Add("@EUSER", SqlDbType.VarChar, 100).Value = obj.EUSER;
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


        public DataTable BindList(T_WGPG obj)
        {
            DataTable dt = new DataTable();
            sql = "select OID,WGPG_NAME,CASE WGPG_ACTV WHEN '1' THEN 'Y' Else 'N' END as Active from T_WGPG where WGPG_ACTV = '1' AND Shop_id=" + obj.Shop_id + " ";
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



        public T_WGPG GetById(string OID)
        {
            T_WGPG obj = new T_WGPG();
            sql = "select OID,WGPG_NAME from T_WGPG where OID=" + OID + " ";
            cmd = new SqlCommand(sql, dbConnect);
            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (string.IsNullOrEmpty(reader["OID"].ToString()))
                    {
                        obj.OID = string.Empty;
                    }
                    else
                    {
                        obj.OID = reader["OID"].ToString();
                    }

                    if (string.IsNullOrEmpty(reader["WGPG_NAME"].ToString()))
                    {
                        obj.WGPG_NAME = string.Empty;
                    }
                    else
                    {
                        obj.WGPG_NAME = reader["WGPG_NAME"].ToString();
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
            return obj;
        }

        public string getcatname(string catname, string shopid)
        {
            string WGPG_NAME = string.Empty;
            sql = "select [T_WGPG].WGPG_NAME from [T_WGPG] where [T_WGPG].WGPG_NAME='" + catname + "' AND [T_WGPG].Shop_id = '" + shopid + "'";

            cmd = new SqlCommand(sql, dbConnect);
            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    WGPG_NAME = reader["WGPG_NAME"].ToString();
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
            return WGPG_NAME;
        }



    }
}
