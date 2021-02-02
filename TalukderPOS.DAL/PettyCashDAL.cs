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
    public class PettyCashDAL
    {
        SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
        SqlDataAdapter da = new SqlDataAdapter();
        SqlCommand cmd;
        String sql;
        SqlDataReader reader;
        string shop_id;

        public String getpettycash(string Shop_id)
        {
            string balance = "";
            sql = "SELECT T1C1,T2C1 ,(T1C1-T2C1) as balance  FROM ( select ISNULL(SUM(Amount), 0) T1C1 FROM  DailyPettyCash where DailyPettyCash.Shop_id =" + Shop_id + ") A CROSS JOIN ( select ISNULL(SUM(AMOUNT), 0) T2C1 FROM  DailyCost where DailyCost.Shop_id =" + Shop_id + ") B";
            cmd = new SqlCommand(sql, dbConnect);
            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    if (string.IsNullOrEmpty(reader["balance"].ToString()))
                    {
                        balance = "0";
                    }
                    else
                    {
                        balance = reader["balance"].ToString();
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
            return balance;
        }

       



        public void Add(DailyPettyCash_BO obj)
        {
            if (string.IsNullOrEmpty(obj.OID))
            {
                sql = "insert into DailyPettyCash(Shop_id,Amount,IUSER,IDAT) values (@Shop_id,@Amount,@IUSER,@IDAT)";
                cmd = new SqlCommand(sql, dbConnect);
                cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 100).Value = obj.IUSER;
                cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = DateTime.Today.Date;
            }
            else
            {
                sql = "update DailyPettyCash set Amount=@Amount where OID=" + obj.OID + " ";
                cmd = new SqlCommand(sql, dbConnect);
            }
            cmd.Parameters.Add("@Shop_id", SqlDbType.VarChar, 100).Value = obj.Shop_id;
            cmd.Parameters.Add("@Amount", SqlDbType.BigInt, 100).Value = Convert.ToInt64(obj.Amount);

          
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
            if (string.IsNullOrEmpty(obj.OID))
            {
                string neevwl = getpettycash(obj.Shop_id);
                sql = "insert into PettyCashMaintainance(Shop_id,PrevBalance,Amount,NewBalance,Particular,Purpose,IUSER,IDAT) values (@Shop_id,@PrevBalance,@Amount,@NewBalance,@Particular,@Purpose,@IUSER,@IDAT)";
                cmd = new SqlCommand(sql, dbConnect);
                cmd.Parameters.Add("@Shop_id", SqlDbType.VarChar, 100).Value = obj.Shop_id;
                cmd.Parameters.Add("@PrevBalance", SqlDbType.BigInt, 100).Value = Convert.ToInt64(obj.CURBALANCE);
                cmd.Parameters.Add("@Amount", SqlDbType.BigInt, 100).Value = Convert.ToInt64(obj.Amount);
                cmd.Parameters.Add("@NewBalance", SqlDbType.BigInt, 100).Value = Convert.ToInt64(neevwl);
                cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 100).Value = "Cash IN";
                cmd.Parameters.Add("@Purpose", SqlDbType.VarChar, 100).Value = "";
                cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 100).Value = obj.IUSER;
                cmd.Parameters.Add("@IDAT", SqlDbType.VarChar, 100).Value = DateTime.Today.Date;
            }
            else
            {
                string neevwl = getpettycash(obj.Shop_id);
                sql = "insert into PettyCashMaintainance(Shop_id,PrevBalance,Amount,NewBalance,Particular,Purpose,IUSER,IDAT) values (@Shop_id,@PrevBalance,@Amount,@NewBalance,@Particular,@Purpose,@IUSER,@IDAT)";
                cmd = new SqlCommand(sql, dbConnect);
                cmd.Parameters.Add("@Shop_id", SqlDbType.VarChar, 100).Value = obj.Shop_id;
                cmd.Parameters.Add("@PrevBalance", SqlDbType.BigInt, 100).Value = Convert.ToInt64(obj.CURBALANCE);
                cmd.Parameters.Add("@Amount", SqlDbType.BigInt, 100).Value = Convert.ToInt64(obj.Amount);
                cmd.Parameters.Add("@NewBalance", SqlDbType.BigInt, 100).Value = Convert.ToInt64(neevwl);
                cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 100).Value = "Cash IN Edit";
                cmd.Parameters.Add("@Purpose", SqlDbType.VarChar, 100).Value = "";
                cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 100).Value = obj.IUSER;
                cmd.Parameters.Add("@IDAT", SqlDbType.VarChar, 100).Value = DateTime.Today.Date;
            }

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



        public void Delete(DailyPettyCash_BO obj)
        {
            sql = "delete from DailyPettyCash where OID=" + obj.OID + " ";
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


        public DataTable BindList(DailyPettyCash_BO obj)
        {
          
            DataTable dt = new DataTable();
            sql = "select OID,Amount,convert(CHAR(10), DailyPettyCash.IDAT, 120) as NEWDATE from DailyPettyCash where Shop_id=" + obj.Shop_id + " ";
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



        public DailyPettyCash_BO GetById(string OID)
        {
            DailyPettyCash_BO obj = new DailyPettyCash_BO();
            sql = "select OID,Amount from DailyPettyCash where OID=" + OID + " ";
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

                    if (string.IsNullOrEmpty(reader["Amount"].ToString()))
                    {
                        obj.Amount = string.Empty;
                    }
                    else
                    {
                        obj.Amount = reader["Amount"].ToString();
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
