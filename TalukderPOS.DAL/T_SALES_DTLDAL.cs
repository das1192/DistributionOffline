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
    public class T_SALES_DTLDAL
    {
        SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
        SqlDataAdapter da = new SqlDataAdapter();
        SqlCommand cmd;
        String sql;
        SqlDataReader reader;
        //SqlDataReader reader;


        public void Add(T_SALES_DTL entity)
        {
            //cmd = new SqlCommand("SPP_SALE", dbConnect);
            cmd = new SqlCommand("SPP_SALE_181222", dbConnect);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@StoreID", SqlDbType.VarChar, 100).Value = entity.StoreID;
            cmd.Parameters.Add("@InvoiceNo", SqlDbType.VarChar, 100).Value = entity.InvoiceNo;
            cmd.Parameters.Add("@PaymentModeID", SqlDbType.Int).Value = Convert.ToInt32(entity.PaymentModeID);
            cmd.Parameters.Add("@BankInfoOID", SqlDbType.Int).Value = Convert.ToInt32(entity.BankInfoOID);
            if (entity.SubTotal == string.Empty)
            {
                cmd.Parameters.Add("@SubTotal", SqlDbType.Decimal).Value = Convert.ToDecimal("0.00");
            }
            else
            {
                cmd.Parameters.Add("@SubTotal", SqlDbType.Decimal).Value = Convert.ToDecimal(entity.SubTotal);
            }

            if (entity.Discount == string.Empty)
            {
                cmd.Parameters.Add("@Discount", SqlDbType.Decimal).Value = Convert.ToDecimal("0.00");
            }
            else
            {
                cmd.Parameters.Add("@Discount", SqlDbType.Decimal).Value = Convert.ToDecimal(entity.Discount);
            }

            cmd.Parameters.Add("@DiscountReferencedBy", SqlDbType.VarChar, 100).Value = entity.DiscountReferencedBy;


            if (entity.NetAmount == string.Empty)
            {
                cmd.Parameters.Add("@NetAmount", SqlDbType.Decimal).Value = Convert.ToDecimal("0.00");
            }
            else
            {
                cmd.Parameters.Add("@NetAmount", SqlDbType.Decimal).Value = Convert.ToDecimal(entity.NetAmount);
            }
            if (entity.ReceiveAmount == string.Empty)
            {
                cmd.Parameters.Add("@ReceiveAmount", SqlDbType.Decimal).Value = Convert.ToDecimal("0.00");
            }
            else
            {
                cmd.Parameters.Add("@ReceiveAmount", SqlDbType.Decimal).Value = Convert.ToDecimal(entity.ReceiveAmount);
            }
            if (entity.CashPaid == string.Empty)
            {
                cmd.Parameters.Add("@CashPaid", SqlDbType.Decimal).Value = Convert.ToDecimal("0.00");
            }
            else
            {
                cmd.Parameters.Add("@CashPaid", SqlDbType.Decimal).Value = Convert.ToDecimal(entity.CashPaid);
            }
            if (entity.CashChange == string.Empty)
            {
                cmd.Parameters.Add("@CashChange", SqlDbType.Decimal).Value = Convert.ToDecimal("0.00");
            }
            else
            {
                cmd.Parameters.Add("@CashChange", SqlDbType.Decimal).Value = Convert.ToDecimal(entity.CashChange);
            }

            cmd.Parameters.Add("@StuffID", SqlDbType.VarChar, 50).Value = entity.StuffID;
            cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 50).Value = entity.IUSER;
            if (entity.IDAT == string.Empty)
            {
                cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = DateTime.Now.Date;
            }
            else
            {
                cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = entity.IDAT;
            }
            if (entity.EDAT == string.Empty)
            {
                cmd.Parameters.Add("@EDAT", SqlDbType.Date).Value = DateTime.Now.Date;
            }
            else
            {
                cmd.Parameters.Add("@EDAT", SqlDbType.Date).Value = entity.EDAT;
            }


            
            cmd.Parameters.Add("@Remarks", SqlDbType.VarChar, 100).Value = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(entity.Remarks.ToLower());

            cmd.Parameters.Add("@CustomerName", SqlDbType.VarChar, 100).Value = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(entity.CustomerName.ToLower());
            cmd.Parameters.Add("@Address", SqlDbType.VarChar, 200).Value = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(entity.Address.ToLower());
            cmd.Parameters.Add("@MobileNo", SqlDbType.VarChar, 50).Value = entity.MobileNo;
            cmd.Parameters.Add("@AlternativeMobileNo", SqlDbType.VarChar, 50).Value = entity.AlternativeMobileNo;
            cmd.Parameters.Add("@DateOfBirth", SqlDbType.VarChar, 50).Value = entity.DateOfBirth;
            cmd.Parameters.Add("@EmailAddress", SqlDbType.VarChar, 100).Value = entity.EmailAddress;
            try
            {
                if (dbConnect.State == ConnectionState.Closed)
                    dbConnect.Open();
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
// new adjust

        public void Adjust_Oncredit2(T_SALES_DTL entity)
        {
            sql = "update T_SALES_MST set ReceiveAmount='" + entity.ReceiveAmount + "' , CashPaid='" + entity.ReceiveAmount + "' where InvoiceNo='" + entity.InvoiceNo + "' AND PaymentModeID='17'";
            cmd = new SqlCommand(sql, dbConnect);
            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbConnect.Close();
            }


            //OLD QUERY TO INSERT THE VALUE IN MST TABLE
            //sql = "insert into T_SALES_MST(StoreID,InvoiceNo,PaymentModeID,BankInfoOID,SubTotal,NetAmount,ReceiveAmount,CashPaid,CashChange,DropStatus,StuffID,Remarks,IUSER,EUSER,IDAT,EDAT) values (@StoreID,@InvoiceNo,@PaymentModeID,@BankInfoOID,@SubTotal,@NetAmount,@ReceiveAmount,@CashPaid,@CashChange,@DropStatus,@StuffID,@Remarks,@IUSER,@IUSER,@IDAT,@IDAT)";
            //cmd = new SqlCommand(sql, dbConnect);
            //cmd.Parameters.Add("@StoreID", SqlDbType.VarChar, 100).Value = entity.StoreID;
            //cmd.Parameters.Add("@InvoiceNo", SqlDbType.VarChar, 100).Value = entity.InvoiceNo;
            //cmd.Parameters.Add("@PaymentModeID", SqlDbType.VarChar, 100).Value = "11";
            //cmd.Parameters.Add("@BankInfoOID", SqlDbType.VarChar, 100).Value = entity.BankInfoOID;
            //cmd.Parameters.Add("@SubTotal", SqlDbType.VarChar, 100).Value = entity.SubTotal;
            //cmd.Parameters.Add("@Discount", SqlDbType.VarChar, 100).Value = entity.Discount;
            //cmd.Parameters.Add("@DiscountReferencedBy", SqlDbType.VarChar, 100).Value = "2";
            //cmd.Parameters.Add("@NetAmount", SqlDbType.VarChar, 100).Value = entity.NetAmount;
            //cmd.Parameters.Add("@ReceiveAmount", SqlDbType.VarChar, 100).Value = entity.ReceiveAmount;
            //cmd.Parameters.Add("@CashPaid", SqlDbType.VarChar, 100).Value = entity.CashPaid;
            //cmd.Parameters.Add("@CashChange", SqlDbType.VarChar, 100).Value = "0";
            //cmd.Parameters.Add("@DropStatus", SqlDbType.VarChar, 100).Value = "0";
            //cmd.Parameters.Add("@StuffID", SqlDbType.VarChar, 100).Value = entity.StuffID;
            //cmd.Parameters.Add("@Remarks", SqlDbType.VarChar, 100).Value = entity.Remarks;
            //cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 100).Value = entity.IUSER;
            //cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = DateTime.Today.Date;

            //try
            //{
            //    if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
            //    cmd.ExecuteNonQuery();
            //    dbConnect.Close();
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //finally
            //{
            //    dbConnect.Close();
            //}

            sql = "insert into CASHINOUT(Branch,CashIN,IUSER,IDAT,INVOICEID,PAYMENTMODE) values (@StoreID,@CashPaid,@IUSER,@IDAT,@InvoiceNo,@PaymentModeID)";
            cmd = new SqlCommand(sql, dbConnect);
            cmd.Parameters.Add("@StoreID", SqlDbType.VarChar, 100).Value = entity.StoreID;
            cmd.Parameters.Add("@InvoiceNo", SqlDbType.VarChar, 100).Value = entity.InvoiceNo;
            cmd.Parameters.Add("@PaymentModeID", SqlDbType.VarChar, 100).Value = "11";

            cmd.Parameters.Add("@CashPaid", SqlDbType.VarChar, 100).Value = entity.CashPaid;

            cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 100).Value = entity.IUSER;
            cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = DateTime.Today.Date;

            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                cmd.ExecuteNonQuery();
               // dbConnect.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbConnect.Close();
            }


            #region create journal

           // string StoreID = Session["StoreID"].ToString();
            string InsertDR = string.Format(@"
INSERT INTO Acc_Journal(AccountID,Branch,Particular,Remarks,Debit,Credit,RefferenceNumber,IDAT,IDATTIME,Narration,Customer_Name) 
VALUES({0},{1},'{2}','{3}',{4}
,{5},'{6}','{7}','{8}','{9}','{10}')
", 1
, entity.StoreID
, "Cash"
, "Cash"
, entity.CashPaid

, 0
, entity.InvoiceNo
, DateTime.Now.ToString()
, DateTime.Now.ToString()
, "Cash Received from Customer on Credit Sale",
entity.CustomerName);

            string InsertCR = string.Format(@"
INSERT INTO Acc_Journal(AccountID,Branch,Particular,Remarks,Debit,Credit,RefferenceNumber,IDAT,IDATTIME,Narration,Customer_Name) 
VALUES({0},'{1}','{2}','{3}',{4}
,{5},'{6}','{7}','{8}','{9}','{10}')
"
, entity.MobileNo
, entity.StoreID
, "A/R", "Customer", 0

, entity.CashPaid
, entity.InvoiceNo
, DateTime.Now.ToString(), DateTime.Now.ToString(), "Cash Received on Credit Sale", entity.CustomerName);

            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                SqlCommand cmdDR = new SqlCommand(InsertDR, dbConnect);
                SqlCommand cmdCR = new SqlCommand(InsertCR, dbConnect);
              //  dbConnect.Open();
                cmdDR.ExecuteNonQuery();
                cmdCR.ExecuteNonQuery();
                //dbConnect.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbConnect.Close();
            }
            #endregion

        }

        public T_SALES_DTL GetSalesInformation2(String Invoiceid)
        {
            T_SALES_DTL entity = new T_SALES_DTL();
            sql = string.Format(@"
select T_SALES_MST.StoreID,T_SALES_MST.InvoiceNo,T_SALES_MST.PaymentModeID,T_SALES_MST.BankInfoOID
,T_SALES_MST.SubTotal,T_SALES_MST.Discount,T_SALES_MST.NetAmount,T_SALES_MST.StuffID,T_SALES_MST.Remarks 
,MobileNo=(
select top 1 c.MobileNo from CustomerInformation c where c.InvoiceNo=T_SALES_MST.InvoiceNo
)

from T_SALES_MST 
where T_SALES_MST.InvoiceNo='{0}' 
", Invoiceid);
            cmd = new SqlCommand(sql, dbConnect);
            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    if (string.IsNullOrEmpty(reader["MobileNo"].ToString()))
                    {
                        entity.MobileNo = string.Empty;
                    }
                    else
                    {
                        entity.MobileNo = reader["MobileNo"].ToString();
                    }


                    if (string.IsNullOrEmpty(reader["StoreID"].ToString()))
                    {
                        entity.StoreID = string.Empty;
                    }
                    else
                    {
                        entity.StoreID = reader["StoreID"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["InvoiceNo"].ToString()))
                    {
                        entity.InvoiceNo = string.Empty;
                    }
                    else
                    {
                        entity.InvoiceNo = reader["InvoiceNo"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["BankInfoOID"].ToString()))
                    {
                        entity.BankInfoOID = string.Empty;
                    }
                    else
                    {
                        entity.BankInfoOID = reader["BankInfoOID"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["SubTotal"].ToString()))
                    {
                        entity.SubTotal = string.Empty;
                    }
                    else
                    {
                        entity.SubTotal = reader["SubTotal"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["PaymentModeID"].ToString()))
                    {
                        entity.PaymentModeID = string.Empty;
                    }
                    else
                    {
                        entity.PaymentModeID = reader["PaymentModeID"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["Discount"].ToString()))
                    {
                        entity.Discount = string.Empty;
                    }
                    else
                    {
                        entity.Discount = reader["Discount"].ToString();
                    }

                    entity.Barcode = string.Empty;


                    if (string.IsNullOrEmpty(reader["NetAmount"].ToString()))
                    {
                        entity.NetAmount = string.Empty;
                    }
                    else
                    {
                        entity.NetAmount = reader["NetAmount"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["StuffID"].ToString()))
                    {
                        entity.StuffID = string.Empty;
                    }
                    else
                    {
                        entity.StuffID = reader["StuffID"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["Remarks"].ToString()))
                    {
                        entity.Remarks = string.Empty;

                    }
                    else
                    {
                        entity.Remarks = reader["Remarks"].ToString();

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





        //end of adjust
        public void Adjust_Oncredit(T_SALES_DTL entity)
        {
            sql = "update T_SALES_MST set DiscountReferencedBy='1' where InvoiceNo='" + entity.InvoiceNo + "' AND PaymentModeID='17'";
            cmd = new SqlCommand(sql, dbConnect);
            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbConnect.Close();
            }

            sql = "insert into T_SALES_MST(StoreID,InvoiceNo,PaymentModeID,BankInfoOID,SubTotal,Discount,DiscountReferencedBy,NetAmount,ReceiveAmount,CashPaid,CashChange,DropStatus,StuffID,Remarks,IUSER,EUSER,IDAT,EDAT) values (@StoreID,@InvoiceNo,@PaymentModeID,@BankInfoOID,@SubTotal,@Discount,@DiscountReferencedBy,@NetAmount,@NetAmount,@NetAmount,@CashChange,@DropStatus,@StuffID,@Remarks,@IUSER,@IUSER,@IDAT,@IDAT)";
            cmd = new SqlCommand(sql, dbConnect);
            cmd.Parameters.Add("@StoreID", SqlDbType.VarChar, 100).Value = entity.StoreID;
            cmd.Parameters.Add("@InvoiceNo", SqlDbType.VarChar, 100).Value = entity.InvoiceNo;
            cmd.Parameters.Add("@PaymentModeID", SqlDbType.VarChar, 100).Value = "11";
            cmd.Parameters.Add("@BankInfoOID", SqlDbType.VarChar, 100).Value = entity.BankInfoOID;
            cmd.Parameters.Add("@SubTotal", SqlDbType.VarChar, 100).Value = entity.SubTotal;
            cmd.Parameters.Add("@Discount", SqlDbType.VarChar, 100).Value = entity.Discount;
            cmd.Parameters.Add("@DiscountReferencedBy", SqlDbType.VarChar, 100).Value = "2";
            cmd.Parameters.Add("@NetAmount", SqlDbType.VarChar, 100).Value = entity.NetAmount;
            cmd.Parameters.Add("@CashChange", SqlDbType.VarChar, 100).Value = "0";
            cmd.Parameters.Add("@DropStatus", SqlDbType.VarChar, 100).Value = "0";
            cmd.Parameters.Add("@StuffID", SqlDbType.VarChar, 100).Value = entity.StuffID;
            cmd.Parameters.Add("@Remarks", SqlDbType.VarChar, 100).Value = entity.Remarks;
            cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 100).Value = entity.IUSER;
            cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = DateTime.Today.Date;

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

            sql = "insert into CASHINOUT(Branch,CashIN,IUSER,IDAT,INVOICEID,PAYMENTMODE) values (@StoreID,@NetAmount,@IUSER,@IDAT,@InvoiceNo,@PaymentModeID)";
            cmd = new SqlCommand(sql, dbConnect);
            cmd.Parameters.Add("@StoreID", SqlDbType.VarChar, 100).Value = entity.StoreID;
            cmd.Parameters.Add("@InvoiceNo", SqlDbType.VarChar, 100).Value = entity.InvoiceNo;
            cmd.Parameters.Add("@PaymentModeID", SqlDbType.VarChar, 100).Value = "11";

            cmd.Parameters.Add("@NetAmount", SqlDbType.VarChar, 100).Value = entity.NetAmount;

            cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 100).Value = entity.IUSER;
            cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = DateTime.Today.Date;

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


            #region create journal

            //string StoreID = Session["StoreID"].ToString();
            string InsertDR = string.Format(@"
INSERT INTO Acc_Journal(AccountID,Branch,Particular,Remarks,Debit,Credit,RefferenceNumber,IDAT,IDATTIME,Narration) 
VALUES({0},{1},'{2}','{3}',{4}
,{5},'{6}','{7}','{8}','{9}')
", 1
, entity.StoreID
, "Cash"
, "Cash"
, entity.NetAmount

, 0
, entity.InvoiceNo
, DateTime.Now.ToString()
, DateTime.Now.ToString()
, "Cash Received from Customer on Credit Sale");

            string InsertCR = string.Format(@"
INSERT INTO Acc_Journal(AccountID,Branch,Particular,Remarks,Debit,Credit,RefferenceNumber,IDAT,IDATTIME,Narration) 
VALUES({0},'{1}','{2}','{3}',{4}
,{5},'{6}','{7}','{8}','{9}')
"
, entity.MobileNo
, entity.StoreID
, "A/R", "Customer", 0

, entity.NetAmount
, entity.InvoiceNo
, DateTime.Now.ToString(), DateTime.Now.ToString(), "Cash Received on Credit Sale");

            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                SqlCommand cmdDR = new SqlCommand(InsertDR, dbConnect);
                SqlCommand cmdCR = new SqlCommand(InsertCR, dbConnect);
                //dbConnect.Open();
                cmdDR.ExecuteNonQuery();
                cmdCR.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                // dbConnect.Close();
            }
            #endregion

        }
        public T_SALES_DTL GetSalesInformation(String Invoiceid)
        {
            T_SALES_DTL entity = new T_SALES_DTL();
            sql = string.Format(@"
select T_SALES_MST.StoreID,T_SALES_MST.InvoiceNo,T_SALES_MST.PaymentModeID,T_SALES_MST.BankInfoOID
,T_SALES_MST.SubTotal,T_SALES_MST.Discount,T_SALES_MST.NetAmount,T_SALES_MST.StuffID,T_SALES_MST.Remarks ,T_SALES_MST.ReceiveAmount
,MobileNo=(
select top 1 J.AccountID from Acc_Journal J where J.RefferenceNumber=T_SALES_MST.InvoiceNo and J.Remarks='Customer' and J.Particular='A/R' and J.Narration='Sales on Credit'
),CustomerName=(
select top 1 J.Customer_Name from Acc_Journal J where J.RefferenceNumber=T_SALES_MST.InvoiceNo and J.Remarks='Customer' and J.Particular='A/R' and J.Narration='Sales on Credit'
)

from T_SALES_MST 
where T_SALES_MST.InvoiceNo='{0}' 
", Invoiceid);
            cmd = new SqlCommand(sql, dbConnect);
            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //
                    if (string.IsNullOrEmpty(reader["CustomerName"].ToString()))
                    {
                        entity.CustomerName = string.Empty;
                    }
                    else
                    {
                        entity.CustomerName = reader["CustomerName"].ToString();
                    }

                    //

                    if (string.IsNullOrEmpty(reader["MobileNo"].ToString()))
                    {
                        entity.MobileNo = string.Empty;
                    }
                    else
                    {
                        entity.MobileNo = reader["MobileNo"].ToString();
                    }

                    //
                    if (string.IsNullOrEmpty(reader["StoreID"].ToString()))
                    {
                        entity.StoreID = string.Empty;
                    }
                    else
                    {
                        entity.StoreID = reader["StoreID"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["InvoiceNo"].ToString()))
                    {
                        entity.InvoiceNo = string.Empty;
                    }
                    else
                    {
                        entity.InvoiceNo = reader["InvoiceNo"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["BankInfoOID"].ToString()))
                    {
                        entity.BankInfoOID = string.Empty;
                    }
                    else
                    {
                        entity.BankInfoOID = reader["BankInfoOID"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["SubTotal"].ToString()))
                    {
                        entity.SubTotal = string.Empty;
                    }
                    else
                    {
                        entity.SubTotal = reader["SubTotal"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["PaymentModeID"].ToString()))
                    {
                        entity.PaymentModeID = string.Empty;
                    }
                    else
                    {
                        entity.PaymentModeID = reader["PaymentModeID"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["Discount"].ToString()))
                    {
                        entity.Discount = string.Empty;
                    }
                    else
                    {
                        entity.Discount = reader["Discount"].ToString();
                    }

                    //entity.Barcode = string.Empty;


                    if (string.IsNullOrEmpty(reader["NetAmount"].ToString()))
                    {
                        entity.NetAmount = string.Empty;
                    }
                    else
                    {
                        entity.NetAmount = reader["NetAmount"].ToString();
                    }

                    if (string.IsNullOrEmpty(reader["ReceiveAmount"].ToString()))
                    {
                        entity.ReceiveAmount = string.Empty;
                    }
                    else
                    {
                        entity.ReceiveAmount = reader["ReceiveAmount"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["StuffID"].ToString()))
                    {
                        entity.StuffID = string.Empty;
                    }
                    else
                    {
                        entity.StuffID = reader["StuffID"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["Remarks"].ToString()))
                    {
                        entity.Remarks = string.Empty;

                    }
                    else
                    {
                        entity.Remarks = reader["Remarks"].ToString();

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



        public String getpuchaseprice(string DESOID, string storeid)
        {
            string CostPrice = "";
            sql = "select TOP(1) CostPrice from Acc_Stock where PROD_DES=" + DESOID + " AND Branch=" + storeid + " AND Quantity>0 order by ACC_STOCKID ASC ";
            cmd = new SqlCommand(sql, dbConnect);
            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    if (string.IsNullOrEmpty(reader["CostPrice"].ToString()))
                    {
                        CostPrice = "0";
                    }
                    else
                    {
                        CostPrice = reader["CostPrice"].ToString();
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
            return CostPrice;
        }

        public String getpuchaseavail234(string OID)
        {
            string availquan = "";
            sql = "select Quantity from Acc_Stock where ACC_STOCKID=" + OID + " ";
            cmd = new SqlCommand(sql, dbConnect);
            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    if (string.IsNullOrEmpty(reader["Quantity"].ToString()))
                    {
                        availquan = "0";
                    }
                    else
                    {
                        availquan = reader["Quantity"].ToString();
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
            return availquan;
        }
        public String getpuchasepriceOID(string DESOID, string storeid)
        {
            string ACC_STOCKID = "";
            sql = "select TOP(1) ACC_STOCKID from Acc_Stock where PROD_DES=" + DESOID + " AND Branch=" + storeid + " AND Quantity>0 order by ACC_STOCKID ASC ";
            cmd = new SqlCommand(sql, dbConnect);
            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    if (string.IsNullOrEmpty(reader["ACC_STOCKID"].ToString()))
                    {
                        ACC_STOCKID = "0";
                    }
                    else
                    {
                        ACC_STOCKID = reader["ACC_STOCKID"].ToString();
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
            return ACC_STOCKID;
        }
        public void T_SALES_DTL_Journal(T_SALES_DTL entity)
        {
            string saveDailyCost = "no";
            Int32 giftAmount = 0;

            sql = "insert into Acc_Journal(AccountID,Branch,Particular,Remarks,Debit,Credit,RefferenceNumber,IDAT,IDATTIME,Narration) values (@AccountID,@Branch,@Particular,@Remarks,0,@Credit,@RefferenceNumber,@IDAT,@IDATTIME,@Narration) ";
            cmd = new SqlCommand(sql, dbConnect);
            cmd.Parameters.Add("@AccountID", SqlDbType.VarChar, 50).Value = entity.DescriptionID;
            cmd.Parameters.Add("@Branch", SqlDbType.VarChar, 50).Value = entity.StoreID;
            cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "Sale";
            cmd.Parameters.Add("@Remarks", SqlDbType.VarChar, 50).Value = "Product";

            if (Convert.ToDecimal(entity.GiftAmount) > 0)
            {
                cmd.Parameters.Add("@Credit", SqlDbType.Int).Value = Convert.ToInt32(entity.CostPrice) * Convert.ToInt32(entity.SaleQty);

                //dailycost
                saveDailyCost = "yes";
                giftAmount = (Convert.ToInt32(entity.CostPrice) * Convert.ToInt32(entity.SaleQty));
            }
            else
            {
                cmd.Parameters.Add("@Credit", SqlDbType.Int).Value = Convert.ToInt32(entity.SalePrice) * Convert.ToInt32(entity.SaleQty);
            }
            cmd.Parameters.Add("@RefferenceNumber", SqlDbType.VarChar, 50).Value = entity.InvoiceNo;
            cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = entity.IDAT;
            cmd.Parameters.Add("@IDATTIME", SqlDbType.DateTime).Value = entity.EDAT; // DateTime.Now;
            cmd.Parameters.Add("@Narration", SqlDbType.VarChar, 500).Value = entity.Narration;

            //dailyCost
            string sqlDailyCost = string.Format(@"
insert into DailyCost(Shop_id,CostingHeadID,AMOUNT,IUSER,IDAT,Remarks) 
values ({0},{1},{2},'{3}','{4}','{5}')
", entity.StoreID, 4, giftAmount, entity.IUSER, entity.IDAT, "Gift");

            sqlDailyCost = string.Format(@"
declare @IDExpenseForGift NVARCHAR(10)
select @IDExpenseForGift=(select ch.OID from CostingHead ch where ch.CostingHead='Expense For Gift' and ch.Shop_id={1})
--select @IDExpenseForGift

insert into DailyCost(Shop_id,CostingHeadID,AMOUNT,IUSER,IDAT,Remarks) 
values ({0},@IDExpenseForGift,{2},'{3}','{4}','{5}')

", entity.StoreID, entity.StoreID, giftAmount, entity.IUSER, entity.IDAT, "Gift");

            SqlCommand cmdDailyCost = new SqlCommand(sqlDailyCost, dbConnect);

            try
            {
                if (dbConnect.State == ConnectionState.Closed)
                {
                    dbConnect.Open();
                    cmd.ExecuteNonQuery();
                    if (saveDailyCost == "yes") { cmdDailyCost.ExecuteNonQuery(); }
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
        public void T_SALES_Acc_Journal(T_SALES_DTL entity)
        {
            sql = "insert into Acc_Journal(AccountID,Branch,Customer_Name,Particular,Remarks,Debit,Credit,RefferenceNumber,IDAT,IDATTIME,Narration) values (@AccountID,@Branch,@Customer_Name,@Particular,@Remarks,@Debit,0,@RefferenceNumber,@IDAT,@IDATTIME,@Narration) ";
            cmd = new SqlCommand(sql, dbConnect);
            cmd.Parameters.Add("@AccountID", SqlDbType.VarChar, 50).Value = entity.LedgerAccID;
            cmd.Parameters.Add("@Branch", SqlDbType.VarChar, 50).Value = entity.StoreID;
            cmd.Parameters.Add("@Customer_Name", SqlDbType.VarChar, 50).Value = entity.LedgerAccCusName;
            cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = entity.LedgerAccParticular;

            cmd.Parameters.Add("@Remarks", SqlDbType.VarChar, 50).Value = entity.LedgerAccRemarks;

            ///cmd.Parameters.Add("@Debit", SqlDbType.BigInt).Value = Convert.ToInt64(entity.ReceiveAmount);

            cmd.Parameters.Add("@Debit", SqlDbType.BigInt).Value = Convert.ToInt32(entity.ReceiveAmount.ToString());
            cmd.Parameters.Add("@RefferenceNumber", SqlDbType.VarChar, 50).Value = entity.InvoiceNo;
            cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = entity.IDAT;
            cmd.Parameters.Add("@IDATTIME", SqlDbType.DateTime).Value = entity.EDAT; // DateTime.Now;
            cmd.Parameters.Add("@Narration", SqlDbType.VarChar, 500).Value = entity.Narration;

            try
            {
                if (dbConnect.State == ConnectionState.Closed)
                    dbConnect.Open();
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
        public void T_SALES_Acc_Journal2(T_SALES_DTL entity)
        {
            sql = "insert into Acc_Journal(AccountID,Branch,Particular,Remarks,Debit,Credit,RefferenceNumber,IDAT,IDATTIME,Narration) values (@AccountID,@Branch,@Particular,@Remarks,@Debit,0,@RefferenceNumber,@IDAT,@IDATTIME,@Narration) ";
            cmd = new SqlCommand(sql, dbConnect);
            cmd.Parameters.Add("@AccountID", SqlDbType.VarChar, 50).Value = entity.LedgerAccID;
            cmd.Parameters.Add("@Branch", SqlDbType.VarChar, 50).Value = entity.StoreID;
            cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = entity.LedgerAccParticular;
            cmd.Parameters.Add("@Remarks", SqlDbType.VarChar, 50).Value = entity.LedgerAccRemarks;
            cmd.Parameters.Add("@Debit", SqlDbType.Int).Value = Convert.ToInt32(entity.LedgerAccCardPaid);
            cmd.Parameters.Add("@RefferenceNumber", SqlDbType.VarChar, 50).Value = entity.InvoiceNo;
            cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = entity.IDAT;
            cmd.Parameters.Add("@IDATTIME", SqlDbType.DateTime).Value = entity.EDAT; // DateTime.Now;
            cmd.Parameters.Add("@Narration", SqlDbType.VarChar, 500).Value = entity.Narration;


            try
            {
                if (dbConnect.State == ConnectionState.Closed)
                    dbConnect.Open();
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
        public void T_SALES_Acc_Journal3(T_SALES_DTL entity)
        {
            sql = "insert into Acc_Journal(AccountID,Branch,Customer_Name,Particular,Remarks,Debit,Credit,RefferenceNumber,IDAT,IDATTIME,Narration) values (@AccountID,@Branch,@Customer_Name,@Particular,@Remarks,@Debit,0,@RefferenceNumber,@IDAT,@IDATTIME,@Narration) ";
            cmd = new SqlCommand(sql, dbConnect);
            cmd.Parameters.Add("@AccountID", SqlDbType.VarChar, 50).Value = entity.LedgerAccID;
            cmd.Parameters.Add("@Branch", SqlDbType.VarChar, 50).Value = entity.StoreID;
            cmd.Parameters.Add("@Customer_Name", SqlDbType.VarChar, 50).Value = entity.LedgerAccCusName;
            cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = entity.LedgerAccParticular;
            cmd.Parameters.Add("@Remarks", SqlDbType.VarChar, 50).Value = entity.LedgerAccRemarks;
            cmd.Parameters.Add("@Debit", SqlDbType.Int).Value = Convert.ToInt32(entity.LedgerAccCardPaid);
            cmd.Parameters.Add("@RefferenceNumber", SqlDbType.VarChar, 50).Value = entity.InvoiceNo;
            cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = entity.IDAT;
            cmd.Parameters.Add("@IDATTIME", SqlDbType.DateTime).Value = DateTime.Now;
            cmd.Parameters.Add("@Narration", SqlDbType.VarChar, 500).Value = entity.Narration;

            try
            {
                if (dbConnect.State == ConnectionState.Closed)
                    dbConnect.Open();
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


        public void T_SALES_Acc_Journal_oncr(T_SALES_DTL entity)
        {
            sql = "insert into Acc_Journal(AccountID,Branch,Customer_Name,Particular,Remarks,Debit,Credit,RefferenceNumber,IDAT,IDATTIME,Narration) values (@AccountID,@Branch,@Customer_Name,@Particular,@Remarks,@Debit,0,@RefferenceNumber,@IDAT,@IDATTIME,@Narration) ";
            cmd = new SqlCommand(sql, dbConnect);
            cmd.Parameters.Add("@AccountID", SqlDbType.VarChar, 50).Value = entity.CustomerID;
            cmd.Parameters.Add("@Branch", SqlDbType.VarChar, 50).Value = entity.StoreID;
            cmd.Parameters.Add("@Customer_Name", SqlDbType.VarChar, 50).Value = entity.LedgerAccCusName;
            cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = entity.LedgerAccParticular;
            cmd.Parameters.Add("@Remarks", SqlDbType.VarChar, 50).Value = entity.LedgerAccRemarks;
            cmd.Parameters.Add("@Debit", SqlDbType.Int).Value = Convert.ToInt32(entity.RemainingAmount);
            cmd.Parameters.Add("@RefferenceNumber", SqlDbType.VarChar, 50).Value = entity.InvoiceNo;
            cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = entity.IDAT;
            cmd.Parameters.Add("@IDATTIME", SqlDbType.DateTime).Value = DateTime.Now;
            cmd.Parameters.Add("@Narration", SqlDbType.VarChar, 500).Value = entity.Narration;

            try
            {
                if (dbConnect.State == ConnectionState.Closed)
                    dbConnect.Open();
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
        public void T_SALES_Acc_Journal4(T_SALES_DTL entity)
        {
            sql = "insert into Acc_Journal(AccountID,Branch,Customer_Name,Particular,Remarks,Debit,Credit,RefferenceNumber,IDAT,IDATTIME,Narration) values (@AccountID,@Branch,@Customer_Name,@Particular,@Remarks,@Debit,0,@RefferenceNumber,@IDAT,@IDATTIME,@Narration) ";
            cmd = new SqlCommand(sql, dbConnect);
            cmd.Parameters.Add("@AccountID", SqlDbType.VarChar, 50).Value = entity.MobileNo;
            cmd.Parameters.Add("@Branch", SqlDbType.VarChar, 50).Value = entity.StoreID;
            cmd.Parameters.Add("@Customer_Name", SqlDbType.VarChar, 50).Value = entity.LedgerAccCusName;
            cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = entity.LedgerAccParticular;
            cmd.Parameters.Add("@Remarks", SqlDbType.VarChar, 50).Value = entity.LedgerAccRemarks;
            cmd.Parameters.Add("@Debit", SqlDbType.Int).Value = Convert.ToInt32(entity.NetAmount);
            cmd.Parameters.Add("@RefferenceNumber", SqlDbType.VarChar, 50).Value = entity.InvoiceNo;
            cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = entity.IDAT;
            cmd.Parameters.Add("@IDATTIME", SqlDbType.DateTime).Value = entity.EDAT; // DateTime.Now;
            cmd.Parameters.Add("@Narration", SqlDbType.VarChar, 500).Value = entity.Narration;

            try
            {
                if (dbConnect.State == ConnectionState.Closed)
                    dbConnect.Open();
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
        public void T_SALES_Acc_Journalforcard(T_SALES_DTL entity)
        {
            sql = "insert into Acc_Journal(AccountID,Branch,Customer_Name,Particular,Remarks,Debit,Credit,RefferenceNumber,IDAT,IDATTIME,Narration) values (@AccountID,@Branch,@Customer_Name,@Particular,@Remarks,@Debit,0,@RefferenceNumber,@IDAT,@IDATTIME,@Narration) ";
            cmd = new SqlCommand(sql, dbConnect);
            cmd.Parameters.Add("@AccountID", SqlDbType.VarChar, 50).Value = entity.LedgerAccID ;
            cmd.Parameters.Add("@Branch", SqlDbType.VarChar, 50).Value = entity.StoreID;
            cmd.Parameters.Add("@Customer_Name", SqlDbType.VarChar, 50).Value = entity.LedgerAccCusName;
            cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = entity.LedgerAccParticular;
            cmd.Parameters.Add("@Remarks", SqlDbType.VarChar, 50).Value = entity.LedgerAccRemarks;
            cmd.Parameters.Add("@Debit", SqlDbType.Int).Value = Convert.ToInt32(entity.NetAmount);
            cmd.Parameters.Add("@RefferenceNumber", SqlDbType.VarChar, 50).Value = entity.InvoiceNo;
            cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = entity.IDAT;
            cmd.Parameters.Add("@IDATTIME", SqlDbType.DateTime).Value = entity.EDAT; // DateTime.Now;
            cmd.Parameters.Add("@Narration", SqlDbType.VarChar, 500).Value = entity.Narration;

            try
            {
                if (dbConnect.State == ConnectionState.Closed)
                    dbConnect.Open();
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
        public void T_SALES_Acc_JournalForDiscount(T_SALES_DTL entity)
        {

          

            sql = "insert into Acc_Journal(AccountID,Branch,Customer_Name,Particular,Remarks,Debit,Credit,RefferenceNumber,IDAT,IDATTIME,Narration) values (@AccountID,@Branch,@Customer_Name,@Particular,@Remarks,@Debit,0,@RefferenceNumber,@IDAT,@IDATTIME,@Narration) ";
            cmd = new SqlCommand(sql, dbConnect);
            cmd.Parameters.Add("@AccountID", SqlDbType.VarChar, 50).Value = entity.LedgerAccID;
            cmd.Parameters.Add("@Branch", SqlDbType.VarChar, 50).Value = entity.StoreID;
            cmd.Parameters.Add("@Customer_Name", SqlDbType.VarChar, 50).Value = entity.LedgerAccCusName;
            cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = entity.LedgerAccParticular;
            cmd.Parameters.Add("@Remarks", SqlDbType.VarChar, 50).Value = entity.LedgerAccRemarks;
            cmd.Parameters.Add("@Debit", SqlDbType.Int).Value = Convert.ToInt32(entity.PassedAmount);
            cmd.Parameters.Add("@RefferenceNumber", SqlDbType.VarChar, 50).Value = entity.InvoiceNo;
            cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = entity.IDAT;
            cmd.Parameters.Add("@IDATTIME", SqlDbType.DateTime).Value = entity.EDAT; // DateTime.Now;
            cmd.Parameters.Add("@Narration", SqlDbType.VarChar, 500).Value = entity.Narration;



            try
            {
                if (dbConnect.State == ConnectionState.Closed)
                    dbConnect.Open();
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
        public void T_SALES_Acc_JournalForGiftDiscount(T_SALES_DTL entity)
        {
            sql = "insert into Acc_Journal(AccountID,Branch,Customer_Name,Particular,Remarks,Debit,Credit,RefferenceNumber,IDAT,IDATTIME,Narration) values (@AccountID,@Branch,@Customer_Name,@Particular,@Remarks,@Debit,0,@RefferenceNumber,@IDAT,@IDATTIME,@Narration) ";
            cmd = new SqlCommand(sql, dbConnect);
            cmd.Parameters.Add("@AccountID", SqlDbType.VarChar, 50).Value = entity.LedgerAccID;
            cmd.Parameters.Add("@Branch", SqlDbType.VarChar, 50).Value = entity.StoreID;
            cmd.Parameters.Add("@Customer_Name", SqlDbType.VarChar, 50).Value = entity.LedgerAccCusName;
            cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = entity.LedgerAccParticular;
            cmd.Parameters.Add("@Remarks", SqlDbType.VarChar, 50).Value = entity.LedgerAccRemarks;
            cmd.Parameters.Add("@Debit", SqlDbType.Int).Value = Convert.ToInt32(entity.PassedAmount)*Convert.ToInt32(entity .SaleQty );
            cmd.Parameters.Add("@RefferenceNumber", SqlDbType.VarChar, 50).Value = entity.InvoiceNo;
            cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = entity.IDAT;
            cmd.Parameters.Add("@IDATTIME", SqlDbType.DateTime).Value = entity.EDAT; // DateTime.Now;
            cmd.Parameters.Add("@Narration", SqlDbType.VarChar, 500).Value = entity.Narration;



            try
            {
                if (dbConnect.State == ConnectionState.Closed)
                    dbConnect.Open();
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
        public void T_SALES_STOCK_MODIFY(T_SALES_DTL entity)
        {
            sql = "insert into Acc_StockDetail(Po_Number,PROD_WGPG,PROD_SUBCATEGORY,PROD_DES,Branch,Quantity,CostPrice,SalePrice,Total,Flag,Remarks,IDAT,IUSER,IDATTIME) values(@InvoiceNo,@CategoryID,@SubCategoryID,@DescriptionID,@Branch,@SaleQty,@PURCHASECOST,@SalePrice,@TOTAL,@Flag,@Remarks,@IDAT,@IUSER,@IDATTIME) ";
            cmd = new SqlCommand(sql, dbConnect);
            cmd.Parameters.Add("@InvoiceNo", SqlDbType.VarChar, 50).Value = entity.InvoiceNo;
            cmd.Parameters.Add("@CategoryID", SqlDbType.BigInt).Value = Convert.ToInt64(entity.CategoryID);
            cmd.Parameters.Add("@SubCategoryID", SqlDbType.BigInt).Value = Convert.ToInt64(entity.SubCategoryID);
            cmd.Parameters.Add("@DescriptionID", SqlDbType.BigInt).Value = Convert.ToInt64(entity.DescriptionID);
            cmd.Parameters.Add("@Branch", SqlDbType.VarChar, 50).Value = entity.StoreID;
            cmd.Parameters.Add("@SaleQty", SqlDbType.Int).Value = Convert.ToInt32(entity.SaleQty);
            cmd.Parameters.Add("@PURCHASECOST", SqlDbType.Int).Value = Convert.ToInt32(entity.PURCHASECOST);
            cmd.Parameters.Add("@SalePrice", SqlDbType.Int).Value = Convert.ToInt32(entity.SalePrice);          //
            cmd.Parameters.Add("@TOTAL", SqlDbType.Int).Value = Convert.ToInt32(entity.PURCHASECOST) * Convert.ToInt32(entity.SaleQty);
            cmd.Parameters.Add("@Flag", SqlDbType.VarChar, 50).Value = "Sale";
            cmd.Parameters.Add("@Remarks", SqlDbType.VarChar, 50).Value = "Sale";
            cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = entity.IDAT;
            cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 50).Value = entity.IUSER;
            cmd.Parameters.Add("@IDATTIME", SqlDbType.Date).Value = DateTime.Now;

            try
            {
                if (dbConnect.State == ConnectionState.Closed)
                    dbConnect.Open();
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




            sql = "update Acc_Stock set Quantity = (Quantity - @Quantity) where ACC_STOCKID = @OID";
            cmd = new SqlCommand(sql, dbConnect);
            cmd.Parameters.Add("@OID", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PURCHASECOSTOID);
            cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = Convert.ToInt32(entity.SaleQty);
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
        public void T_SALES_DTL_StockNew(T_SALES_DTL entity)
        {
            sql = "insert into Acc_StockDetail(Po_Number,PROD_WGPG,PROD_SUBCATEGORY,PROD_DES,Branch,Quantity,CostPrice,SalePrice,Total,Discount,Flag,Remarks,IDAT,IUSER,IDATTIME,Barcode) values(@InvoiceNo,@CategoryID,@SubCategoryID,@DescriptionID,@Branch,@SaleQty,@PURCHASECOST,@SalePrice,@TOTAL,@Discount,@Flag,@Remarks,@IDAT,@IUSER,@IDATTIME,@Barcode) ";
            cmd = new SqlCommand(sql, dbConnect);
            cmd.Parameters.Add("@InvoiceNo", SqlDbType.VarChar, 50).Value = entity.InvoiceNo;
            cmd.Parameters.Add("@Barcode", SqlDbType.VarChar, 50).Value = entity.Barcode; // entity.InvoiceNo;
            cmd.Parameters.Add("@CategoryID", SqlDbType.BigInt).Value = Convert.ToInt64(entity.CategoryID);
            cmd.Parameters.Add("@SubCategoryID", SqlDbType.BigInt).Value = Convert.ToInt64(entity.SubCategoryID);
            cmd.Parameters.Add("@DescriptionID", SqlDbType.BigInt).Value = Convert.ToInt64(entity.DescriptionID);
            cmd.Parameters.Add("@Branch", SqlDbType.VarChar, 50).Value = entity.StoreID;
            cmd.Parameters.Add("@SaleQty", SqlDbType.BigInt).Value = Convert.ToInt32(entity.SaleQty);
            cmd.Parameters.Add("@PURCHASECOST", SqlDbType.Int).Value = Convert.ToInt32(entity.PURCHASECOST);
            if ((Convert.ToDecimal(entity.GiftAmount)) > 0)
            {
                cmd.Parameters.Add("@SalePrice", SqlDbType.Int).Value = Convert.ToInt32(entity.PURCHASECOST);          //
                
                cmd.Parameters.Add("@Remarks", SqlDbType.VarChar, 50).Value = "Gift";
            }
            else
            {
                cmd.Parameters.Add("@SalePrice", SqlDbType.Int).Value = Convert.ToInt32(entity.SalePrice);          //
                cmd.Parameters.Add("@Remarks", SqlDbType.VarChar, 50).Value = "Sale";
            }

            if ((Convert.ToDecimal(entity.DiscountAmount)) > 0)
            {
                cmd.Parameters.Add("@Discount", SqlDbType.BigInt).Value = Convert.ToInt32(entity.DiscountAmount);          //


            }
            else
            {
                cmd.Parameters.Add("@Discount", SqlDbType.BigInt).Value = 0;   
            }//
            cmd.Parameters.Add("@TOTAL", SqlDbType.Int).Value = Convert.ToInt32(entity.PURCHASECOST) * Convert.ToInt32(entity.SaleQty);
            cmd.Parameters.Add("@Flag", SqlDbType.VarChar, 50).Value = "Sale";
            
            cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = entity.IDAT;
            cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 50).Value = entity.IUSER;
            cmd.Parameters.Add("@IDATTIME", SqlDbType.Date).Value = entity.EDAT; // DateTime.Now;

            try
            {
                if (dbConnect.State == ConnectionState.Closed)
                    dbConnect.Open();
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
        public void T_SALES_DTL_Stock(T_SALES_DTL entity)
        {
            sql = "insert into Acc_StockDetail(Po_Number,PROD_WGPG,PROD_SUBCATEGORY,PROD_DES,Branch,Quantity,CostPrice,SalePrice,Total,Flag,Remarks,IDAT,IUSER,IDATTIME) values(@InvoiceNo,@CategoryID,@SubCategoryID,@DescriptionID,@Branch,@SaleQty,@PURCHASECOST,@SalePrice,@TOTAL,@Flag,@Remarks,@IDAT,@IUSER,@IDATTIME) ";
            cmd = new SqlCommand(sql, dbConnect);
            cmd.Parameters.Add("@InvoiceNo", SqlDbType.VarChar, 50).Value = entity.InvoiceNo;
            cmd.Parameters.Add("@CategoryID", SqlDbType.BigInt).Value = Convert.ToInt64(entity.CategoryID);
            cmd.Parameters.Add("@SubCategoryID", SqlDbType.BigInt).Value = Convert.ToInt64(entity.SubCategoryID);
            cmd.Parameters.Add("@DescriptionID", SqlDbType.BigInt).Value = Convert.ToInt64(entity.DescriptionID);
            cmd.Parameters.Add("@Branch", SqlDbType.VarChar, 50).Value = entity.StoreID;
            cmd.Parameters.Add("@SaleQty", SqlDbType.Int).Value = Convert.ToInt32(entity.SaleQty);
            cmd.Parameters.Add("@PURCHASECOST", SqlDbType.Int).Value = Convert.ToInt32(entity.PURCHASECOST);
            cmd.Parameters.Add("@SalePrice", SqlDbType.Int).Value = Convert.ToInt32(entity.SalePrice);          //
            cmd.Parameters.Add("@TOTAL", SqlDbType.Int).Value = Convert.ToInt32(entity.PURCHASECOST) * Convert.ToInt32(entity.SaleQty);
            cmd.Parameters.Add("@Flag", SqlDbType.VarChar, 50).Value = "Sale";
            cmd.Parameters.Add("@Remarks", SqlDbType.VarChar, 50).Value = "Sale";
            cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = entity.IDAT;
            cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 50).Value = entity.IUSER;
            cmd.Parameters.Add("@IDATTIME", SqlDbType.Date).Value = DateTime.Now;

            try
            {
                if (dbConnect.State == ConnectionState.Closed)
                    dbConnect.Open();
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




            sql = "update Acc_Stock set Quantity = (Quantity - @Quantity) where ACC_STOCKID = @OID";
            cmd = new SqlCommand(sql, dbConnect);
            cmd.Parameters.Add("@OID", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PURCHASECOSTOID);
            cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = Convert.ToInt32(entity.SaleQty);
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
        public void T_SALES_DTL_Add(T_SALES_DTL entity)
        {
            #region
            /*
            cmd = new SqlCommand("SPP_T_SALES_DTL_Add", dbConnect);
            cmd.CommandType = CommandType.StoredProcedure;

            //sql = "insert into Description(SubCategoryID,Description,Active,SESPrice,MRP,IUSER,IDAT,EUSER,EDAT) OUTPUT INSERTED.OID values (@SubCategoryID,@Description,@Active,@SESPrice,@MRP,@IUSER,@IDAT,@EUSER,@EDAT)";
            //cmd = new SqlCommand(sql, dbConnect);

            cmd.Parameters.Add("@StoreID", SqlDbType.VarChar, 100).Value = entity.StoreID;
            cmd.Parameters.Add("@InvoiceNo", SqlDbType.VarChar,100).Value = entity.InvoiceNo;
            cmd.Parameters.Add("@DescriptionID", SqlDbType.BigInt).Value =Convert.ToInt64(entity.DescriptionID);
            cmd.Parameters.Add("@Barcode", SqlDbType.VarChar, 100).Value = entity.Barcode;
            cmd.Parameters.Add("@SalePrice", SqlDbType.Decimal).Value = Convert.ToDecimal(entity.SalePrice);
            cmd.Parameters.Add("@SaleQty", SqlDbType.Int).Value = Convert.ToInt32(entity.SaleQty);
            cmd.Parameters.Add("@DiscountReferenceOID", SqlDbType.Int).Value = Convert.ToInt32(entity.DiscountReferenceOID);
            cmd.Parameters.Add("@DiscountAmount", SqlDbType.Int).Value = Convert.ToInt32(entity.DiscountAmount);
            cmd.Parameters.Add("@ReturnQty", SqlDbType.VarChar, 50).Value = Convert.ToInt32(entity.ReturnQty);            

            cmd.Parameters.Add("@OID", SqlDbType.BigInt).Value = Convert.ToInt64(entity.OID);            
            cmd.Parameters.Add("@SaleStatus", SqlDbType.Int).Value = 1;
            cmd.Parameters.Add("@CategoryID", SqlDbType.BigInt).Value = Convert.ToInt64(entity.CategoryID); 
            cmd.Parameters.Add("@SalesDate", SqlDbType.Date).Value = entity.IDAT;            
            cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 50).Value = entity.IUSER;
            cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = entity.IDAT;
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
            */
            //////////////Sale Ledger///////////////////////

            #endregion

            /////////////////////Sale Ledger Ends/////////////////

            sql = "insert into T_SALES_DTL(InvoiceNo,DescriptionID,Barcode,SalePrice,SaleQty,DiscountReferenceOID,DiscountAmount,ReturnQty,IDAT,GiftAmount) values(@InvoiceNo,@DescriptionID,@Barcode,@SalePrice,@SaleQty,@DiscountReferenceOID,@DiscountAmount,@ReturnQty,@IDAT,@GiftAmount) ";
            cmd = new SqlCommand(sql, dbConnect);
            cmd.Parameters.Add("@InvoiceNo", SqlDbType.VarChar, 50).Value = entity.InvoiceNo;
            cmd.Parameters.Add("@DescriptionID", SqlDbType.BigInt).Value = Convert.ToInt64(entity.DescriptionID);
            cmd.Parameters.Add("@Barcode", SqlDbType.VarChar, 100).Value = entity.Barcode;
            cmd.Parameters.Add("@SalePrice", SqlDbType.Decimal).Value = Convert.ToDecimal(entity.SalePrice);
            cmd.Parameters.Add("@SaleQty", SqlDbType.Int).Value = Convert.ToInt32(entity.SaleQty);
            cmd.Parameters.Add("@DiscountReferenceOID", SqlDbType.BigInt).Value = Convert.ToInt64(entity.DiscountReferenceOID);
            cmd.Parameters.Add("@DiscountAmount", SqlDbType.BigInt).Value = Convert.ToInt64(entity.DiscountAmount);
            cmd.Parameters.Add("@GiftAmount", SqlDbType.Decimal).Value = Convert.ToDecimal(entity.GiftAmount);
            cmd.Parameters.Add("@ReturnQty", SqlDbType.Int).Value = Convert.ToInt32(entity.ReturnQty);
            cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = entity.IDAT;

//            dailyCost
//            string sqlDailyCost = string.Format(@"
//            insert into DailyCost(Shop_id,CostingHeadID,AMOUNT,IUSER,IDAT,Remarks) 
//            values ({0},{1},{2},'{3}','{4}','{5}')
//            ", entity.StoreID, 3, entity.DiscountAmount, entity.IUSER, DateTime.Now.ToString(), "Discount");

            string sqlDailyCost = string.Format(@"
declare @IDDiscountOnSales NVARCHAR(10)
select @IDDiscountOnSales = (select ch.OID from CostingHead ch where ch.CostingHead='Discount On Sales' and ch.Shop_id={1})
--select @IDDiscountOnSales

insert into DailyCost(Shop_id,CostingHeadID,AMOUNT,IUSER,IDAT,Remarks) 
values ({0},@IDDiscountOnSales,{2},'{3}','{4}','{5}')
            ", entity.StoreID, entity.StoreID, entity.DiscountAmount, entity.IUSER, entity.IDAT, "Discount");


            SqlCommand cmdDailyCost = new SqlCommand(sqlDailyCost, dbConnect);

            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                cmd.ExecuteNonQuery();
                if ((Convert.ToInt64(entity.DiscountAmount)) > 0) { cmdDailyCost.ExecuteNonQuery(); }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbConnect.Close();
            }


            sql = "INSERT INTO StockPosting(BranchOID,DescriptionOID,Barcode,InwardQty,OutwardQty,Particulars,IUSER,IDAT) VALUES(@BranchOID,@DescriptionOID,@Barcode,@InwardQty,@OutwardQty,@Particulars,@IUSER,@IDAT) ";
            cmd = new SqlCommand(sql, dbConnect);
            cmd.Parameters.Add("@BranchOID", SqlDbType.VarChar, 100).Value = entity.StoreID;
            cmd.Parameters.Add("@DescriptionOID", SqlDbType.BigInt).Value = Convert.ToInt64(entity.DescriptionID);
            cmd.Parameters.Add("@Barcode", SqlDbType.VarChar, 100).Value = entity.Barcode;
            cmd.Parameters.Add("@InwardQty", SqlDbType.Int).Value = 0;
            cmd.Parameters.Add("@OutwardQty", SqlDbType.Int).Value = Convert.ToInt32(entity.SaleQty);
            cmd.Parameters.Add("@Particulars", SqlDbType.VarChar, 100).Value = "Sale";
            cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 50).Value = entity.IUSER;
            cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = entity.IDAT;
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



            sql = "update StoreMasterStock set SaleQuantity = (SaleQuantity + @SaleQty) where OID = @OID";
            cmd = new SqlCommand(sql, dbConnect);
            cmd.Parameters.Add("@OID", SqlDbType.BigInt).Value = Convert.ToInt64(entity.OID);
            cmd.Parameters.Add("@Barcode", SqlDbType.VarChar, 100).Value = entity.Barcode;
            cmd.Parameters.Add("@SaleQty", SqlDbType.Int).Value = Convert.ToInt32(entity.SaleQty);
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


        public DataTable SPP_GetInvoice(String InvoiceNo)
        {
            DataTable dtsales = new DataTable();
            da = new SqlDataAdapter();
            cmd = new SqlCommand("SPP_GetInvoice_2", dbConnect);
            cmd.Parameters.Add("@InvoiceNo", SqlDbType.VarChar, 100).Value = InvoiceNo;
            cmd.CommandType = CommandType.StoredProcedure;
            da = new SqlDataAdapter(cmd);
            da.SelectCommand.CommandTimeout = 300;
            da.Fill(dtsales);
            return dtsales;
        }

        public DataTable SPP_GetProductForSale(T_PROD entity)
        {
            DataTable dt = new DataTable();
            cmd = new SqlCommand("SPP_GetProductForSale", dbConnect);
            cmd.Parameters.Add("@PROD_WGPG", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_WGPG);
            cmd.Parameters.Add("@PROD_SUBCATEGORY", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_SUBCATEGORY);
            cmd.Parameters.Add("@PROD_DES", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_DES);
            cmd.CommandType = CommandType.StoredProcedure;
            da = new SqlDataAdapter(cmd);
            da.SelectCommand.CommandTimeout = 300;
            da.Fill(dt);
            return dt;
        }

        //103 barcode sadiq
        public DataTable SPP_GetProductForSaleByBarcode(T_PROD entity)
        {
            DataTable dt = new DataTable();
            cmd = new SqlCommand("SPP_GetProductForSaleByBarcode", dbConnect);

            cmd.Parameters.Add("@CategoryID", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_WGPG);
            cmd.Parameters.Add("@SubCategoryID", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_SUBCATEGORY);
            cmd.Parameters.Add("@ProductID", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_DES);
            cmd.Parameters.Add("@Barcode", SqlDbType.NVarChar).Value = entity.Barcode;

            cmd.CommandType = CommandType.StoredProcedure;
            da = new SqlDataAdapter(cmd);
            da.SelectCommand.CommandTimeout = 300;
            da.Fill(dt);
            return dt;
        }

        // Added By Yeasin 15-May-2019
        public void AddSalesDetails(T_SALES_DTL entity)
        {
            cmd = new SqlCommand("spInsertSalesDetails_2", dbConnect);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ShopId", SqlDbType.Int).Value = entity.StoreID;
            cmd.Parameters.Add("@InvoiceNo", SqlDbType.VarChar, 50).Value = entity.InvoiceNo;
            cmd.Parameters.Add("@CategoryId", SqlDbType.BigInt).Value = entity.CategoryID;
            cmd.Parameters.Add("@DescriptionID", SqlDbType.BigInt).Value = entity.DescriptionID;
            cmd.Parameters.Add("@SubCategoryID", SqlDbType.BigInt).Value = entity.SubCategoryID;
            cmd.Parameters.Add("@Barcode", SqlDbType.VarChar, 50).Value = entity.Barcode;
            cmd.Parameters.Add("@SalePrice", SqlDbType.Decimal).Value = entity.SalePrice;
            cmd.Parameters.Add("@SaleQty", SqlDbType.Int).Value = entity.SaleQty;
            cmd.Parameters.Add("@DiscountAmount", SqlDbType.BigInt).Value = entity.DiscountAmount;
            cmd.Parameters.Add("@ReturnQty", SqlDbType.Int).Value = entity.ReturnQty;
            cmd.Parameters.Add("@GiftAmount", SqlDbType.Decimal).Value = entity.GiftAmount;
            cmd.Parameters.Add("@Narration", SqlDbType.VarChar, 150).Value = entity.Narration;
            //entity.DiscountReferenceOID = entity.DiscountReferenceOID == "" ? "0" : entity.DiscountReferenceOID;
            //cmd.Parameters.Add("@DiscountReferenceOID", SqlDbType.BigInt).Value = entity.DiscountReferenceOID;
            cmd.Parameters.Add("@IUser", SqlDbType.VarChar, 50).Value = entity.IUSER;
            cmd.Parameters.Add("@OID", SqlDbType.BigInt).Value = entity.OID;
            cmd.Parameters.Add("@Customer_Name", SqlDbType.VarChar, 100).Value = entity.CustomerName;
            cmd.Parameters.Add("@IDAT", SqlDbType.DateTime).Value = entity.IDAT;

            //@
            //DiscountReferenceOID DiscountReferenceOID

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

        public void AddSalesMaster(T_SALES_DTL entity)
        {
            cmd = new SqlCommand("spInsertSalesMaster_2", dbConnect);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@StoreID", SqlDbType.VarChar, 100).Value = entity.StoreID;
            cmd.Parameters.Add("@InvoiceNo", SqlDbType.VarChar, 100).Value = entity.InvoiceNo;
            cmd.Parameters.Add("@PaymentModeID", SqlDbType.Int).Value = Convert.ToInt32(entity.PaymentModeID);
            cmd.Parameters.Add("@BankInfoOID", SqlDbType.Int).Value = Convert.ToInt32(entity.BankInfoOID);

            entity.SubTotal = entity.SubTotal.Trim() == "" ? "0.00" : entity.SubTotal;
            cmd.Parameters.Add("@SubTotal", SqlDbType.Decimal).Value = Convert.ToDecimal(entity.SubTotal);

            entity.Discount = entity.Discount.Trim() == "" ? "0.00" : entity.Discount;
            cmd.Parameters.Add("@Discount", SqlDbType.Decimal).Value = Convert.ToDecimal(entity.Discount);

            cmd.Parameters.Add("@DiscountReferencedBy", SqlDbType.VarChar, 100).Value = entity.DiscountReferencedBy;

            entity.NetAmount = entity.NetAmount.Trim() == "" ? "0.00" : entity.NetAmount;
            cmd.Parameters.Add("@NetAmount", SqlDbType.Decimal).Value = Convert.ToDecimal(entity.NetAmount);
            

            entity.ReceiveAmount = entity.ReceiveAmount.Trim() == "" ? "0.00" : entity.ReceiveAmount;
            cmd.Parameters.Add("@ReceiveAmount", SqlDbType.Decimal).Value = Convert.ToDecimal(entity.ReceiveAmount);

            entity.CashPaid = entity.CashPaid.Trim() == "" ? "0.00" : entity.CashPaid;
            cmd.Parameters.Add("@CashPaid", SqlDbType.Decimal).Value = Convert.ToDecimal(entity.CashPaid);

            entity.CashChange = entity.CashChange.Trim() == "" ? "0.00" : entity.CashChange;
            cmd.Parameters.Add("@CashChange", SqlDbType.Decimal).Value = Convert.ToDecimal(entity.CashChange);

            cmd.Parameters.Add("@StuffID", SqlDbType.VarChar, 50).Value = entity.StuffID;
            cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 50).Value = entity.IUSER;

            cmd.Parameters.Add("@Remarks", SqlDbType.VarChar, 100).Value = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(entity.Remarks.ToLower());

            cmd.Parameters.Add("@CustomerName", SqlDbType.VarChar, 100).Value = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(entity.CustomerName.ToLower());

            cmd.Parameters.Add("@CustomerID", SqlDbType.BigInt).Value = Convert.ToInt64(entity.CustomerID);

            cmd.Parameters.Add("@Address", SqlDbType.VarChar, 100).Value = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(entity.Address.ToLower());
            cmd.Parameters.Add("@MobileNo", SqlDbType.VarChar, 50).Value = entity.MobileNo;
            cmd.Parameters.Add("@AlternativeMobileNo", SqlDbType.VarChar, 50).Value = entity.AlternativeMobileNo;
            cmd.Parameters.Add("@DateOfBirth", SqlDbType.VarChar, 50).Value = entity.DateOfBirth;
            cmd.Parameters.Add("@EmailAddress", SqlDbType.VarChar, 100).Value = entity.EmailAddress;
            cmd.Parameters.Add("@BankID", SqlDbType.Int).Value = entity.BankId;

            entity.CardAmount = entity.CardAmount.Trim() == "" ? "0.00" : entity.CardAmount;
            cmd.Parameters.Add("@CardAmt", SqlDbType.Int).Value = entity.CardAmount;

            cmd.Parameters.Add("@IDAT", SqlDbType.DateTime).Value = entity.IDAT;
            cmd.Parameters.Add("@EDAT", SqlDbType.DateTime).Value = entity.EDAT;
            entity.PreviousDue = entity.PreviousDue.Trim() == "" ? "0.00" : entity.PreviousDue;
            cmd.Parameters.Add("@PreviousDue", SqlDbType.Decimal).Value = Convert.ToDecimal(entity.PreviousDue);

            try
            {
                if (dbConnect.State == ConnectionState.Closed)
                    dbConnect.Open();
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



    }
}
