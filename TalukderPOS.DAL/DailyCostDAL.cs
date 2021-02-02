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
    public class DailyCostDAL
    {
        SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
        SqlDataAdapter da = new SqlDataAdapter();
        SqlCommand cmd;
        String sql;
        SqlDataReader reader;
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
       
        public void Add(DailyCost_BO obj)
        {
            if (string.IsNullOrEmpty(obj.OID))
            {
                sql = "insert into DailyCost(Shop_id,CostingHeadID,AMOUNT,IUSER,IDAT,Remarks,ReferenceNo) values (@Shop_id,@CostingHeadID,@AMOUNT,@IUSER,@IDAT,@Remarks,@ReferenceNo)";
                cmd = new SqlCommand(sql, dbConnect);
                cmd.Parameters.Add("@Shop_id", SqlDbType.VarChar, 100).Value = obj.Shop_id;
                
            }
            else
            {
                sql = "update DailyCost set CostingHeadID=@CostingHeadID,AMOUNT=@AMOUNT,IDAT=@IDAT,Remarks=@Remarks where OID=" + obj.OID + " ";
                cmd = new SqlCommand(sql, dbConnect);
            }
            cmd.Parameters.Add("@CostingHeadID", SqlDbType.VarChar, 100).Value = obj.CostingHeadID;
            cmd.Parameters.Add("@ReferenceNo", SqlDbType.VarChar, 100).Value = obj.RefNo;
            cmd.Parameters.Add("@AMOUNT", SqlDbType.BigInt, 100).Value = Convert.ToInt64(obj.AMOUNT);
            cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 100).Value = obj.IUSER;
            cmd.Parameters.Add("@Remarks", SqlDbType.VarChar, 100).Value = obj.Remarks;
            cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = obj.IDAT; // DateTime.Today.Date;
            
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
                sql = "insert into PettyCashMaintainance(Shop_id,PrevBalance,Amount,NewBalance,Particular,Purpose,IUSER,IDAT,Remarks) values (@Shop_id,@PrevBalance,@Amount,@NewBalance,@Particular,@Purpose,@IUSER,@IDAT,@Remarks)";
                cmd = new SqlCommand(sql, dbConnect);
                cmd.Parameters.Add("@Shop_id", SqlDbType.VarChar, 100).Value = obj.Shop_id;
                cmd.Parameters.Add("@PrevBalance", SqlDbType.BigInt, 100).Value = Convert.ToInt64(obj.CURBALANCE);
                cmd.Parameters.Add("@Amount", SqlDbType.BigInt, 100).Value = Convert.ToInt64(obj.AMOUNT);
                cmd.Parameters.Add("@NewBalance", SqlDbType.BigInt, 100).Value = Convert.ToInt64(obj.CURBALANCE) - Convert.ToInt64(obj.AMOUNT);
                cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 100).Value = "Cash Out";
                cmd.Parameters.Add("@Purpose", SqlDbType.VarChar, 100).Value = obj.CostingHeadID;
                cmd.Parameters.Add("@Remarks", SqlDbType.VarChar, 100).Value = obj.Remarks;
                cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 100).Value = obj.IUSER;
                cmd.Parameters.Add("@IDAT", SqlDbType.VarChar, 100).Value = DateTime.Today.Date;
            
            }
            else
            {
                string neebal = getpettycash(obj.Shop_id);
                sql = "insert into PettyCashMaintainance(Shop_id,PrevBalance,Amount,NewBalance,Particular,Purpose,IUSER,IDAT,Remarks) values (@Shop_id,@PrevBalance,@Amount,@NewBalance,@Particular,@Purpose,@IUSER,@IDAT,@Remarks)";
                cmd = new SqlCommand(sql, dbConnect);
                cmd.Parameters.Add("@Shop_id", SqlDbType.VarChar, 100).Value = obj.Shop_id;
                cmd.Parameters.Add("@PrevBalance", SqlDbType.BigInt, 100).Value = Convert.ToInt64(obj.CURBALANCE);
                cmd.Parameters.Add("@Amount", SqlDbType.BigInt, 100).Value = Convert.ToInt64(obj.AMOUNT);
                cmd.Parameters.Add("@NewBalance", SqlDbType.BigInt, 100).Value = Convert.ToInt64(neebal);
                cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 100).Value = "Cash Out Edit";
                cmd.Parameters.Add("@Purpose", SqlDbType.VarChar, 100).Value = obj.CostingHeadID;
                cmd.Parameters.Add("@Remarks", SqlDbType.VarChar, 100).Value = obj.Remarks;
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
        public String getamount(string OID)
        {
            string amount = "";
            sql = "select AMOUNT from DailyCost where OID=" + OID + " ";
            cmd = new SqlCommand(sql, dbConnect);
            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    if (string.IsNullOrEmpty(reader["AMOUNT"].ToString()))
                    {
                        amount = "0";
                    }
                    else
                    {
                        amount = reader["AMOUNT"].ToString();
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
            return amount;
        }
        public String getcostinghead(string OID)
        {
            string CostingHeadID = "";
            sql = "select CostingHeadID from DailyCost where OID=" + OID + " ";
            cmd = new SqlCommand(sql, dbConnect);
            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    if (string.IsNullOrEmpty(reader["CostingHeadID"].ToString()))
                    {
                        CostingHeadID = "0";
                    }
                    else
                    {
                        CostingHeadID = reader["CostingHeadID"].ToString();
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
            return CostingHeadID;
        }
        public void Delete(DailyCost_BO obj)
        {
            string neee = getamount(obj.OID);
            sql = "update DailyCost set AMOUNT=0 where OID=" + obj.OID + " ";
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
            string kkk = getcostinghead(obj.OID);
            sql = "insert into PettyCashMaintainance(Shop_id,PrevBalance,Amount,NewBalance,Particular,Purpose,IUSER,IDAT) values (@Shop_id,@PrevBalance,@Amount,@NewBalance,@Particular,@Purpose,@IUSER,@IDAT)";
            cmd = new SqlCommand(sql, dbConnect);
            cmd.Parameters.Add("@Shop_id", SqlDbType.VarChar, 100).Value = obj.Shop_id;
            cmd.Parameters.Add("@PrevBalance", SqlDbType.BigInt, 100).Value = Convert.ToInt64(obj.CURBALANCE);
            cmd.Parameters.Add("@Amount", SqlDbType.BigInt, 100).Value = Convert.ToInt64(neee);
            cmd.Parameters.Add("@NewBalance", SqlDbType.BigInt, 100).Value = Convert.ToInt64(obj.CURBALANCE) + Convert.ToInt64(neee);
            cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 100).Value = "Cash Out Delete";
            cmd.Parameters.Add("@Purpose", SqlDbType.VarChar, 100).Value = kkk;
            cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 100).Value = obj.IUSER;
            cmd.Parameters.Add("@IDAT", SqlDbType.VarChar, 100).Value = DateTime.Today.Date;

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


        public DataTable BindList(DailyCost_BO obj)
        {
            DataTable dt = new DataTable();
            sql = "Select DailyCost.OID,DailyCost.Remarks,CostingHead.CostingHead,DailyCost.AMOUNT,convert(CHAR(10), DailyCost.IDAT, 120) as NEWDATE from DailyCost inner join CostingHead on DailyCost.CostingHeadID=CostingHead.OID where DailyCost.Shop_id=" + obj.Shop_id + " AND DailyCost.AMOUNT>0 order by DailyCost.OID desc";
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

        public DataTable CurrentCashBalance(DailyCost_BO obj)
        {
            DataTable dt = new DataTable();
            sql = "SELECT T1C1,T2C1 ,(T1C1-T2C1) as balance  FROM ( select ISNULL(SUM(Amount), 0) T1C1 FROM  DailyPettyCash where DailyPettyCash.Shop_id =" + obj.Shop_id + ") A CROSS JOIN ( select ISNULL(SUM(AMOUNT), 0) T2C1 FROM  DailyCost where DailyCost.Shop_id =" + obj.Shop_id + ") B";
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
        public DataTable CashFlowSearch(DailyCost_BO obj)
        {
            DataTable dt = new DataTable();
            sql = "SELECT [PettyCashMaintainance].[OID],[PettyCashMaintainance].[Remarks],[PettyCashMaintainance].[PrevBalance],[PettyCashMaintainance].[Amount],[PettyCashMaintainance].[NewBalance],[PettyCashMaintainance].[Particular],[CostingHead].[CostingHead],[User].[UserFullName],[PettyCashMaintainance].[IDAT] FROM [dbo].[PettyCashMaintainance] inner join [dbo].[User] on [PettyCashMaintainance].[IUSER] = [User].[UserId] Left outer join [dbo].[CostingHead] on [PettyCashMaintainance].[Purpose]=[CostingHead].[OID] where [PettyCashMaintainance].[Shop_id]=" + obj.Shop_id + " ";
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
        public DailyCost_BO GetById(string OID)
        {
            DailyCost_BO obj = new DailyCost_BO();
            sql = "select OID,CostingHeadID,AMOUNT from DailyCost where OID=" + OID + " ";
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

                    if (string.IsNullOrEmpty(reader["CostingHeadID"].ToString()))
                    {
                        obj.CostingHeadID = string.Empty;
                    }
                    else
                    {
                        obj.CostingHeadID = reader["CostingHeadID"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["AMOUNT"].ToString()))
                    {
                        obj.AMOUNT = string.Empty;
                    }
                    else
                    {
                        obj.AMOUNT = reader["AMOUNT"].ToString();
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
