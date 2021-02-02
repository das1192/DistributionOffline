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
    public class AddShopDAL
    {

        SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
        SqlDataAdapter da = new SqlDataAdapter();
        SqlCommand cmd;
        String sql;
        SqlDataReader reader;

        public void Add(AddShop_BO obj)
        {
            if (string.IsNullOrEmpty(obj.OID))
            {
                sql = "insert into ShopInfo(ShopName,ActiveStatus,IUSER,IDAT,EUSER,EDAT) values (@ShopName,@ActiveStatus,@IUSER,@IDAT,@EUSER,@EDAT)";
                cmd = new SqlCommand(sql, dbConnect);
                cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 100).Value = obj.IUSER;
                cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = DateTime.Today.Date;
            }
            else
            {
                sql = "update ShopInfo set ShopName=@ShopName,ActiveStatus=@ActiveStatus,EUSER=@EUSER,EDAT=@EDAT where OID=" + obj.OID + " ";
                cmd = new SqlCommand(sql, dbConnect);
            }
            cmd.Parameters.Add("@ShopName", SqlDbType.VarChar, 100).Value = obj.ShopName;
            cmd.Parameters.Add("@ActiveStatus", SqlDbType.Int).Value = obj.ActiveStatus;
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

        public void AddLogo(AddShop_BO obj)
        {

            sql = "insert into Shop_Logo(Shop_id,ImageByte,IUSER,IDAT) values (@Shop_id,@ImageByte,@IUSER,@IDAT)";
                cmd = new SqlCommand(sql, dbConnect);
                cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 100).Value = obj.IUSER;
                cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = DateTime.Today.Date;
                cmd.Parameters.Add("@ImageByte", SqlDbType.Image ).Value = obj .imgarray ;
                cmd.Parameters.Add("@Shop_id", SqlDbType.VarChar, 100).Value = obj.Shop_id;
            
           
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

        public void Delete(AddShop_BO obj)
        {
            sql = "update ShopInfo set ActiveStatus=@ActiveStatus,EUSER=@EUSER,EDAT=@EDAT where OID=" + obj.OID + " ";
            cmd = new SqlCommand(sql, dbConnect);
            cmd.Parameters.Add("@ActiveStatus", SqlDbType.Int).Value = 0;
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
        public void DeleteLogo(AddShop_BO obj)
        {
            sql = "delete from Shop_Logo where OID=" + obj.OID + " ";
            cmd = new SqlCommand(sql, dbConnect);
           
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
            sql = "select OID,ShopName,CASE ActiveStatus WHEN 1 THEN 'Y' Else 'N' END as Active from ShopInfo where ActiveStatus = 1 ";
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

        public DataTable BindListLogo(string Shop_id)
        {
            DataTable dt = new DataTable();
            sql = "select Top(1) OID,ImageByte from Shop_Logo where Shop_id =" + Shop_id + " ";
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

        public AddShop_BO GetById(string OID)
        {
            AddShop_BO obj = new AddShop_BO();
            sql = "select OID,ShopName from ShopInfo where OID=" + OID + " ";
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

                    if (string.IsNullOrEmpty(reader["ShopName"].ToString()))
                    {
                        obj.ShopName = string.Empty;
                    }
                    else
                    {
                        obj.ShopName = reader["ShopName"].ToString();
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

        
       

    }
}

