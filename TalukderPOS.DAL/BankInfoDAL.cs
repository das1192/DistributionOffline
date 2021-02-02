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
    public class BankInfoDAL
    {
        SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
        SqlDataAdapter da = new SqlDataAdapter();
        SqlCommand cmd;
        String sql;
        SqlDataReader reader;

        public void AddBank(BankInfo_BO obj)
        {
            if (string.IsNullOrEmpty(obj.OID))
            {
                sql = "insert into BankInfo(BankName,ActiveStatus,IUSER,IDAT,EUSER,EDAT,AccountNo,ShopID) values (@BankName,@ActiveStatus,@IUSER,@IDAT,@EUSER,@EDAT,@AccountNo,@ShopID)";
                cmd = new SqlCommand(sql, dbConnect);
                cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 100).Value = obj.IUSER;
                cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = DateTime.Today.Date;
            }
            else
            {
                sql = "update BankInfo set BankName=@BankName,AccountNo=@AccountNo,ActiveStatus=@ActiveStatus,EUSER=@EUSER,EDAT=@EDAT where OID=" + obj.OID + " ";
                cmd = new SqlCommand(sql, dbConnect);
            }
            cmd.Parameters.Add("@BankName", SqlDbType.VarChar, 100).Value = obj.BankName;
            cmd.Parameters.Add("@AccountNo", SqlDbType.VarChar, 100).Value = obj.AccountNo;
            cmd.Parameters.Add("@ShopID", SqlDbType.Int).Value = obj.ShopID;
            cmd.Parameters.Add("@ActiveStatus", SqlDbType.Int).Value = obj.ActiveStatus;
            cmd.Parameters.Add("@EUSER", SqlDbType.VarChar, 100).Value = obj.EUSER;
            cmd.Parameters.Add("@EDAT", SqlDbType.Date).Value = DateTime.Today.Date;
            try
            {
                if (dbConnect.State == ConnectionState.Closed)
                {
                    dbConnect.Open();
                    cmd.ExecuteNonQuery();
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
        }



        public void Delete(BankInfo_BO obj)
        {
            sql = "update BankInfo set ActiveStatus=@ActiveStatus,EUSER=@EUSER,EDAT=@EDAT where OID=" + obj.OID + " ";
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


        public DataTable BindList(int ShopID)
        {
            //int shopid = Convert.ToInt32(Session["StoreID"].ToString());
            DataTable dt = new DataTable();
            sql = "select OID,BankName,CASE ActiveStatus WHEN 1 THEN 'Y' Else 'N' END as Active from BankInfo where ActiveStatus = 1 ";
            sql = string.Format(@"
select OID,BankName,AccountNo
,CASE ActiveStatus WHEN 1 THEN 'Y' Else 'N' END as Active 
from BankInfo 
where ShopID={0} -- ActiveStatus = 1
order by ActiveStatus desc
", ShopID);

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



        public BankInfo_BO GetById(string OID)
        {
            BankInfo_BO obj = new BankInfo_BO();
            sql = "select OID,BankName,AccountNo,ActiveStatus from BankInfo where OID=" + OID + " ";
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

                    if (string.IsNullOrEmpty(reader["BankName"].ToString()))
                    {
                        obj.BankName = string.Empty;
                    }
                    else
                    {
                        obj.BankName = reader["BankName"].ToString();
                    }//
                    if (string.IsNullOrEmpty(reader["AccountNo"].ToString()))
                    {
                        obj.AccountNo = string.Empty;
                    }
                    else
                    {
                        obj.AccountNo = reader["AccountNo"].ToString();
                    }//
                    if (string.IsNullOrEmpty(reader["AccountNo"].ToString()))
                    {
                        obj.ActiveStatus = string.Empty;
                    }
                    else
                    {
                        obj.ActiveStatus = reader["ActiveStatus"].ToString();
                    }//
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

        public DataTable BankAccountReport(T_PROD entity)
        {
            DataTable dt = new DataTable();
            List<String> myList = new List<String>();
            string WhereCondition = string.Empty;
            if (entity.PaymentMode != String.Empty & entity.PaymentMode != "0")
            {
                myList.Add("T_SALES_MST.PaymentModeID = " + entity.PaymentMode + " ");
            }
            if (entity.BankName != String.Empty & entity.BankName != "0")
            {
                myList.Add("T_SALES_MST.BankInfoOID = " + entity.BankName + " ");
            }
            if (entity.Branch != String.Empty & entity.Branch != "0")
            {
                myList.Add("T_SALES_MST.StoreID = '" + entity.Branch + "' ");
            }

            if (entity.FromDate != String.Empty & entity.ToDate != string.Empty)
            {
                myList.Add("dbo.T_SALES_MST.IDAT between '" + entity.FromDate + "' and '" + entity.ToDate + "' ");
            }
            else if (entity.FromDate != String.Empty & entity.ToDate == string.Empty)
            {
                myList.Add("dbo.T_SALES_MST.IDAT between '" + entity.FromDate + "' and '" + entity.FromDate + "' ");
            }
            else if (entity.FromDate == String.Empty & entity.ToDate != string.Empty)
            {
                myList.Add("dbo.T_SALES_MST.IDAT between '" + entity.ToDate + "' and '" + entity.ToDate + "' ");
            }
            myList.Add("dbo.T_SALES_MST.DropStatus=0");
            string[] myArray = myList.ToArray();
            string where1 = string.Join(" and ", myArray);

            if (where1 == string.Empty)
            {
                WhereCondition = string.Empty;
            }
            else
            {
                WhereCondition = "where " + where1 + " ";
            }


            sql = "select T_CCOM.CCOM_NAME,T_SALES_MST.InvoiceNo,PaymentMode.PaymentMode,BankInfo.BankName,CASE T_SALES_MST.VatAmountCashed   WHEN 0 THEN CAST(sum(T_SALES_MST.NetAmount-T_SALES_MST.ReceiveAmount)as Integer)   WHEN 1 THEN sum(T_SALES_MST.NetAmount-T_SALES_MST.ReceiveAmount)- ((T_SALES_MST.SubTotal-(T_SALES_MST.ReceiveAmount+T_SALES_MST.Discount))*T_SALES_MST.Vat/100)  END as amount,CONVERT(VARCHAR(10),T_SALES_MST.IDAT,103) AS IDAT from T_SALES_MST  inner join BankInfo on T_SALES_MST.BankInfoOID = BankInfo.OID  inner join PaymentMode on T_SALES_MST.PaymentModeID = PaymentMode.OID   inner join T_CCOM on T_SALES_MST.StoreID = T_CCOM.OID " + WhereCondition + " group by BankInfo.BankName,T_SALES_MST.IDAT,  PaymentMode.PaymentMode,T_CCOM.CCOM_NAME,T_SALES_MST.InvoiceNo,T_SALES_MST.VatAmountCashed,T_SALES_MST.NetAmount,T_SALES_MST.Discount,T_SALES_MST.Vat,T_SALES_MST.ReceiveAmount,T_SALES_MST.SubTotal order by BankInfo.BankName";

            da = new SqlDataAdapter(sql, dbConnect);
            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
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

    }
}
