using System;
using System.Web;
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
    public class ProductVendorDAL
    {
        SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
        SqlDataAdapter da = new SqlDataAdapter();
        SqlCommand cmd;
        String sql;
        SqlDataReader reader;
        string shop_id;
        public void Add(ProductVendor_BO obj)
        {
            if (string.IsNullOrEmpty(obj.OID))
            {
                sql = "insert into Vendor(Shop_id,Vendor_Name,Vendor_Address,Vendor_NID,Vendor_mobile,Vendor_Active,IUSER,IDAT,EUSER,EDAT) values (@Shop_id,@Vendor_Name,@Address,@TRID,@Vendor_mobile,@Vendor_Active,@IUSER,@IDAT,@EUSER,@EDAT)";
                cmd = new SqlCommand(sql, dbConnect);
                cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 100).Value = obj.IUSER;
                cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = DateTime.Today.Date;
            }
            else
            {
                sql = "update Vendor set Vendor_mobile=@Vendor_mobile,Vendor_Address=@Address,Vendor_NID=@TRID,Vendor_Name=@Vendor_Name,Vendor_Active=@Vendor_Active,EUSER=@EUSER,EDAT=@EDAT where OID=" + obj.OID + " ";
                cmd = new SqlCommand(sql, dbConnect);
            }
            cmd.Parameters.Add("@Shop_id", SqlDbType.VarChar, 100).Value = obj.Shop_id;
            cmd.Parameters.Add("@Vendor_Name", SqlDbType.VarChar, 100).Value = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(obj.Vendor_Name.ToLower());
            cmd.Parameters.Add("@Vendor_Active", SqlDbType.VarChar, 1).Value = obj.Vendor_Active;
            cmd.Parameters.Add("@Address", SqlDbType.VarChar, 100).Value = obj.Vendor_Address;
            cmd.Parameters.Add("@Vendor_mobile", SqlDbType.VarChar, 100).Value = obj.Vendor_mobile;
            cmd.Parameters.Add("@TRID", SqlDbType.VarChar, 100).Value = obj.Vendor_tr;
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



        public void Delete(ProductVendor_BO obj)
        {
            sql = "update Vendor set Vendor_Active=@Vendor_Active,EUSER=@EUSER,EDAT=@EDAT where OID=" + obj.OID + " ";
            cmd = new SqlCommand(sql, dbConnect);
            cmd.Parameters.Add("@Vendor_Active", SqlDbType.VarChar, 1).Value = "0";
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


        public DataTable BindList(ProductVendor_BO obj)
        {
          
            DataTable dt = new DataTable();
            sql = "select OID,Vendor_Name,Vendor_Address,Vendor_NID,Vendor_mobile,CASE Vendor_Active WHEN '1' THEN 'Y' Else 'N' END as Active from Vendor where  Shop_id=" + obj.Shop_id + "   order by Vendor_Active desc";
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



        public ProductVendor_BO GetById(string OID)
        {
            ProductVendor_BO obj = new ProductVendor_BO();
            sql = "select OID,Vendor_Name,Vendor_Address,Vendor_NID,Vendor_Active,Vendor_mobile from Vendor where OID=" + OID + " ";
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

                    if (string.IsNullOrEmpty(reader["Vendor_Name"].ToString()))
                    {
                        obj.Vendor_Name = string.Empty;
                    }
                    else
                    {
                        obj.Vendor_Name = reader["Vendor_Name"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["Vendor_Address"].ToString()))
                    {
                        obj.Vendor_Address = string.Empty;
                    }
                    else
                    {
                        obj.Vendor_Address = reader["Vendor_Address"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["Vendor_NID"].ToString()))
                    {
                        obj.Vendor_tr = string.Empty;
                    }
                    else
                    {
                        obj.Vendor_tr = reader["Vendor_NID"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["Vendor_mobile"].ToString()))
                    {
                        obj.Vendor_mobile = string.Empty;
                    }
                    else
                    {
                        obj.Vendor_mobile = reader["Vendor_mobile"].ToString();
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
