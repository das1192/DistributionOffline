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
    public class CostingHeadDAL
    {
        SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
        SqlDataAdapter da = new SqlDataAdapter();
        SqlCommand cmd;
        String sql;
        SqlDataReader reader;
        string shop_id;
        public void Add(CostingHead_BO obj)
        {
            if (string.IsNullOrEmpty(obj.OID))
            {
                sql = "insert into CostingHead(Shop_id,ExpenseType,CostingHead,IUSER,IDAT) values (@Shop_id,@ExpenseType,@CostingHead,@IUSER,@IDAT)";
                cmd = new SqlCommand(sql, dbConnect);
                cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 100).Value = obj.IUSER;
                cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = DateTime.Today.Date;
            }
            else
            {
                sql = "update CostingHead set CostingHead=@CostingHead,ExpenseType=@ExpenseType where OID=" + obj.OID + " ";
                cmd = new SqlCommand(sql, dbConnect);
            }
            cmd.Parameters.Add("@Shop_id", SqlDbType.VarChar, 100).Value = obj.Shop_id;
            cmd.Parameters.Add("@ExpenseType", SqlDbType.VarChar, 100).Value = obj.ExpenseType ;
            cmd.Parameters.Add("@CostingHead", SqlDbType.VarChar, 100).Value = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(obj.CostingHead.ToLower());
          
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



        public void Delete(CostingHead_BO obj)
        {
            sql = "delete from CostingHead where OID=" + obj.OID + " ";
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


        public DataTable BindList(CostingHead_BO obj)
        {
          
            DataTable dt = new DataTable();
            sql = "select OID,ExpenseType,CostingHead from CostingHead where Shop_id=" + obj.Shop_id + " ";

//            sql = string.Format(@"
//select OID,ExpenseType,CostingHead from CostingHead where Shop_id={0} order by ExpenseType,CostingHead", obj.Shop_id);
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



        public CostingHead_BO GetById(string OID)
        {
            CostingHead_BO obj = new CostingHead_BO();
            sql = "select OID,CostingHead from CostingHead where OID=" + OID + " ";
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

                    if (string.IsNullOrEmpty(reader["CostingHead"].ToString()))
                    {
                        obj.CostingHead = string.Empty;
                    }
                    else
                    {
                        obj.CostingHead = reader["CostingHead"].ToString();
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
