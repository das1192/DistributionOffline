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
    public class Vendor_OutgoingDAL
    {
        SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
        SqlDataAdapter da = new SqlDataAdapter();
        SqlCommand cmd;
        String sql;
        SqlDataReader reader;
        public int maxid;
        public void Add(Vendor_Outgoing_BO obj)
        {
            if (string.IsNullOrEmpty(obj.OID))
            {
                sql = "insert into Vendor_Outgoing(Shop_id,Vendor_ID,AMOUNT,IUSER,IDAT,Remarks,ReferenceNo) values (@Shop_id,@Vendor_ID,@AMOUNT,@IUSER,@IDAT,@Remarks,@ReferenceNo)";
                cmd = new SqlCommand(sql, dbConnect);
                cmd.Parameters.Add("@Shop_id", SqlDbType.VarChar, 100).Value = obj.Shop_id;
                cmd.Parameters.Add("@Vendor_ID", SqlDbType.VarChar, 100).Value = obj.Vendor_ID;
                cmd.Parameters.Add("@AMOUNT", SqlDbType.VarChar, 100).Value = obj.AMOUNT;
                cmd.Parameters.Add("@Remarks", SqlDbType.VarChar, 100).Value = obj.Remarks;
                cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 100).Value = obj.IUSER;
                cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = obj.IDAT; // DateTime.Today.Date;
                cmd.Parameters.Add("@ReferenceNo", SqlDbType.VarChar, 100).Value = obj.RefferenceNumber;

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


                sql = "SELECT MAX([OID]) as Id FROM [Vendor_Outgoing]";
                cmd = new SqlCommand(sql, dbConnect);
                try
                {
                    if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        maxid = Convert.ToInt32(reader["Id"].ToString());
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

                sql = "INSERT INTO CASHINOUT(Branch,CashOUT,IUSER,IDAT,PAYMENTID,ReferenceNo) VALUES(@Shop_id,@AMOUNT,@IUSER,@IDAT,@PAYMENTOID,@ReferenceNo)";
                cmd = new SqlCommand(sql, dbConnect);
                cmd.Parameters.Add("@Shop_id", SqlDbType.VarChar, 100).Value = obj.Shop_id;

                cmd.Parameters.Add("@AMOUNT", SqlDbType.VarChar, 100).Value = obj.AMOUNT;
                cmd.Parameters.Add("@ReferenceNo", SqlDbType.VarChar, 100).Value = obj.RefferenceNumber;

                cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 100).Value = obj.IUSER;
                cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = obj.IDAT; // DateTime.Today.Date;
                cmd.Parameters.Add("@PAYMENTOID", SqlDbType.VarChar, 100).Value = Convert.ToInt32(maxid);
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
            else
            {
                sql = "update Vendor_Outgoing set Remarks=@Remarks,Vendor_ID=@Vendor_ID,AMOUNT=@AMOUNT,IDAT=@IDAT where OID=" + obj.OID + " ";
                cmd = new SqlCommand(sql, dbConnect);
                cmd.Parameters.Add("@Vendor_ID", SqlDbType.VarChar, 100).Value = obj.Vendor_ID;
                cmd.Parameters.Add("@AMOUNT", SqlDbType.VarChar, 100).Value = obj.AMOUNT;
                cmd.Parameters.Add("@Remarks", SqlDbType.VarChar, 100).Value = obj.Remarks;
                cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 100).Value = obj.IUSER;
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

                sql = "update CASHINOUT set CashOUT=@AMOUNT,IDAT=@IDAT,IUSER=@IUSER where PAYMENTID=" + obj.OID + " ";
                cmd = new SqlCommand(sql, dbConnect);
                cmd.Parameters.Add("@Vendor_ID", SqlDbType.VarChar, 100).Value = obj.Vendor_ID;
                cmd.Parameters.Add("@AMOUNT", SqlDbType.VarChar, 100).Value = obj.AMOUNT;

                cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 100).Value = obj.IUSER;
                cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = obj.IDAT;  // DateTime.Today.Date;

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

            string strAccountID = obj.PaymentFrom;
            string strParticular = obj.PaymentFrom == "1" ? "Cash" : "Bank";
            string strRemarks = obj.PaymentFrom == "1" ? "Cash" : "Bank";
            string strNarraation = obj.PaymentFrom == "1" ? "Cash Payment to Supplier for Oncredit purchase" : "Bank Payment to Supplier for Oncredit purchase";
            //journal
            //string StoreID = Session["StoreID"].ToString();
            string InsertDR = string.Format(@"
INSERT INTO Acc_Journal(AccountID,Branch,Particular,Remarks,Debit,Credit,RefferenceNumber,IDAT,IDATTIME,Narration) 
VALUES({0},{1},'{2}','{3}',{4}
,{5},'{6}','{7}','{8}','{9}')
", strAccountID
, obj.Shop_id
, strParticular
, strRemarks
, 0

, obj.AMOUNT
, obj.RefferenceNumber
, obj.IDAT
, obj.IDAT
, strNarraation);

            string InsertCR = string.Format(@"
INSERT INTO Acc_Journal(AccountID,Branch,Particular,Remarks,Debit,Credit,RefferenceNumber,IDAT,IDATTIME,Narration) 
VALUES({0},'{1}','{2}','{3}',{4}
,{5},'{6}','{7}','{8}','{9}')
"
, obj.Vendor_ID
, obj.Shop_id
, "A/P", "Supplier", obj.AMOUNT

, 0
, obj.RefferenceNumber
, obj.IDAT, obj.IDAT, "Payment to Supplier for Oncredit purchase");

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

        }
        // New Method for adding/Editing Retailer
        public void AddCustomer(Vendor_Outgoing_BO obj)
        {
            // gets values from the customer page
            string strOpeningBalance = obj.OpeningBalance.Trim() == "" ? "0.00" : obj.OpeningBalance.Trim();
            string strTelephone = obj.Telephone.Trim() == ""?"0": obj.Telephone.Trim();
            int CustomerStatus = (obj.ActiveStatus == "True") ? 1 : 0;
            string strOID = obj.OID.Trim() == "" ? "0" : obj.OID.Trim();

            cmd = new SqlCommand("spInsertOrUpdateRetailer", dbConnect);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@CustomerID", SqlDbType.BigInt).Value = Convert.ToInt64(strOID);
            cmd.Parameters.Add("@Name", SqlDbType.VarChar,100).Value = obj.CustomerName;
            cmd.Parameters.Add("@Number", SqlDbType.Int).Value = Convert.ToInt32(obj.CustomerNumber);
            cmd.Parameters.Add("@Branch", SqlDbType.VarChar,50).Value = obj.Shop_id;
            cmd.Parameters.Add("@Address", SqlDbType.NVarChar, 150).Value = obj.Address;
            cmd.Parameters.Add("@Telephone", SqlDbType.Int).Value = Convert.ToInt32(strTelephone);
            cmd.Parameters.Add("@Email", SqlDbType.NVarChar,150).Value = obj.Email;
            cmd.Parameters.Add("@OpeningBalance", SqlDbType.Decimal).Value = Convert.ToDecimal(strOpeningBalance);
            cmd.Parameters.Add("@RefNo", SqlDbType.NVarChar,100).Value = obj.RefferenceNumber;
            cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = Convert.ToDateTime(obj.IDAT).Date;
            cmd.Parameters.Add("@CustomerStatus", SqlDbType.Bit).Value = Convert.ToBoolean(obj.ActiveStatus);
            cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar, 150).Value = obj.Remarks;
            cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 100).Value = obj.IUSER;

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

        public void AddCommission(Vendor_Outgoing_BO obj)
        {
           

            string strAccountID = obj.AccountID ;
            string strParticular = obj.Remarks == "Cash" ? "Cash" : "A/P";
            string strRemarks = obj.Remarks == "Cash" ? "Cash" : "Supplier";
            string strNarraation = obj.Narration ;
            //journal
            //string StoreID = Session["StoreID"].ToString();
            string InsertDR = string.Format(@"
INSERT INTO Acc_Journal(AccountID,Branch,Particular,Remarks,Debit,Credit,RefferenceNumber,IDAT,IDATTIME,Narration) 
VALUES({0},{1},'{2}','{3}',{4}
,{5},'{6}','{7}','{8}','{9}')
", strAccountID
, obj.Shop_id
, strParticular
, strRemarks
, obj.AMOUNT

, 0
, obj.RefferenceNumber
, obj.IDAT
, obj.IDAT
, strNarraation);

            string InsertCR = string.Format(@"
INSERT INTO Acc_Journal(AccountID,Branch,Particular,Remarks,Debit,Credit,RefferenceNumber,IDAT,IDATTIME,Narration) 
VALUES({0},'{1}','{2}','{3}',{4}
,{5},'{6}','{7}','{8}','{9}')
"
, 11999
, obj.Shop_id
, "Commission", "Commission", 0

,  obj.AMOUNT
, obj.RefferenceNumber
, obj.IDAT, obj.IDAT, strNarraation);

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

        }

        public void Delete(Vendor_Outgoing_BO obj)
        {
            sql = "delete from Vendor_Outgoing where OID=" + obj.OID + " ";
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


            sql = "delete from CASHINOUT where PAYMENTID=" + obj.OID + " ";
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

            sql = "insert into Vendor_Outgoing_Delete(Shop_id,Vendor_ID,AMOUNT,IUSER,IDAT,Remarks) values (@Shop_id,@Vendor_ID,@AMOUNT,@IUSER,@IDAT,@Remarks)";
            cmd = new SqlCommand(sql, dbConnect);
            cmd.Parameters.Add("@Shop_id", SqlDbType.VarChar, 100).Value = obj.Shop_id;
            cmd.Parameters.Add("@Vendor_ID", SqlDbType.VarChar, 100).Value = obj.Vendor_ID;
            cmd.Parameters.Add("@AMOUNT", SqlDbType.VarChar, 100).Value = obj.AMOUNT;
            cmd.Parameters.Add("@Remarks", SqlDbType.VarChar, 100).Value = obj.Remarks;
            cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 100).Value = obj.IUSER;
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
        }


        public DataTable BindList(Vendor_Outgoing_BO obj)
        {
            DataTable dt = new DataTable();
            List<String> myList = new List<String>();
            string WhereCondition = string.Empty;

            if (obj.Shop_id != String.Empty & obj.Shop_id != "0")
            {
                myList.Add("Vendor_Outgoing.Shop_id = '" + obj.Shop_id + "' ");
            }

            if (obj.Vendor_ID != String.Empty & obj.Vendor_ID != "0")
            {
                myList.Add("Vendor_Outgoing.Vendor_ID = '" + obj.Vendor_ID + "' ");
            }
            if (obj.FromDate != String.Empty & obj.ToDate != string.Empty)
            {
                myList.Add("Vendor_Outgoing.IDAT between '" + obj.FromDate + "' and '" + obj.ToDate + "' ");
            }
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

            sql = "Select Vendor_Outgoing.OID,Vendor.Vendor_Name,Vendor_Outgoing.Vendor_ID,Vendor_Outgoing.AMOUNT,Vendor_Outgoing.Remarks,convert(CHAR(10), Vendor_Outgoing.IDAT, 120) as NEWDATE,Vendor_Outgoing.ReferenceNo from Vendor_Outgoing inner join Vendor on Vendor_Outgoing.Vendor_ID=Vendor.OID  " + WhereCondition + " order by Vendor_Outgoing.OID Desc";
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

        public DataTable BindListdelete(Vendor_Outgoing_BO obj)
        {
            DataTable dt = new DataTable();
            List<String> myList = new List<String>();
            string WhereCondition = string.Empty;

            if (obj.Shop_id != String.Empty & obj.Shop_id != "0")
            {
                myList.Add("Vendor_Outgoing_Delete.Shop_id = '" + obj.Shop_id + "' ");
            }


            if (obj.FromDate != String.Empty & obj.ToDate != string.Empty)
            {
                myList.Add("Vendor_Outgoing_Delete.IDAT between '" + obj.FromDate + "' and '" + obj.ToDate + "' ");
            }
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
            sql = "Select Vendor_Outgoing_Delete.OID,Vendor.Vendor_Name,Vendor_Outgoing_Delete.Vendor_ID,Vendor_Outgoing_Delete.AMOUNT,Vendor_Outgoing_Delete.Remarks,convert(CHAR(10), Vendor_Outgoing_Delete.IDAT, 120) as NEWDATE from Vendor_Outgoing_Delete inner join Vendor on Vendor_Outgoing_Delete.Vendor_ID=Vendor.OID  " + WhereCondition + " order by Vendor_Outgoing_Delete.OID Desc ";
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

        public Vendor_Outgoing_BO GetById(string OID)
        {
            Vendor_Outgoing_BO obj = new Vendor_Outgoing_BO();
            sql = "select OID,Vendor_ID,AMOUNT,Remarks from Vendor_Outgoing where OID=" + OID + " ";
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

                    if (string.IsNullOrEmpty(reader["Vendor_ID"].ToString()))
                    {
                        obj.Vendor_ID = string.Empty;
                    }
                    else
                    {
                        obj.Vendor_ID = reader["Vendor_ID"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["AMOUNT"].ToString()))
                    {
                        obj.AMOUNT = string.Empty;
                    }
                    else
                    {
                        obj.AMOUNT = reader["AMOUNT"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["Remarks"].ToString()))
                    {
                        obj.Remarks = string.Empty;
                    }
                    else
                    {
                        obj.Remarks = reader["Remarks"].ToString();
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

        public Vendor_Outgoing_BO GetRetailerById(string OID)
        {
            Vendor_Outgoing_BO obj = new Vendor_Outgoing_BO();
            sql = "Select * From Customers  where ID=" + OID + " ";
            cmd = new SqlCommand(sql, dbConnect);
            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (string.IsNullOrEmpty(reader["ID"].ToString()))
                    {
                        obj.OID = string.Empty;
                    }
                    else
                    {
                        obj.OID = reader["ID"].ToString();
                    }

                    if (string.IsNullOrEmpty(reader["Name"].ToString()))
                    {
                        obj.CustomerName = string.Empty;
                    }
                    else
                    {
                        obj.CustomerName = reader["Name"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["Address"].ToString()))
                    {
                        obj.Address = string.Empty;
                    }
                    else
                    {
                        obj.Address = reader["Address"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["Number"].ToString()))
                    {
                        obj.CustomerNumber = string.Empty;
                    }
                    else
                    {
                        obj.CustomerNumber = reader["Number"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["Telephone"].ToString()))
                    {
                        obj.Telephone = string.Empty;
                    }
                    else
                    {
                        obj.Telephone = reader["Telephone"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["Email"].ToString()))
                    {
                        obj.Email = string.Empty;
                    }
                    else
                    {
                        obj.Email = reader["Email"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["OpeningBalance"].ToString()))
                    {
                        obj.OpeningBalance = "0.00";
                    }
                    else
                    {
                        obj.OpeningBalance = reader["OpeningBalance"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["ReferenceNumber"].ToString()))
                    {
                        obj.RefferenceNumber = string.Empty;
                    }
                    else
                    {
                        obj.RefferenceNumber = reader["ReferenceNumber"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["CustomerStatus"].ToString()))
                    {
                        obj.ActiveStatus = "False";
                    }
                    else
                    {
                        obj.ActiveStatus = reader["CustomerStatus"].ToString() == "False" ? "False" : "True";
                    }
                   
                    if (string.IsNullOrEmpty(reader["Remarks"].ToString()))
                    {
                        obj.Remarks = string.Empty;
                    }
                    else
                    {
                        obj.Remarks = reader["Remarks"].ToString();
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

        // Retailer Payment Adjustment //
        // Date:- 22-Jun-2019
        public void AddRetailerPaymentAdj(Vendor_Outgoing_BO enitity) 
        {
            cmd = new SqlCommand("spAdjustRetailerDuePayment", dbConnect);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@CustomerId", SqlDbType.BigInt).Value = Convert.ToInt64(enitity.Vendor_ID);
            cmd.Parameters.Add("@Branch", SqlDbType.VarChar,100).Value =enitity.Shop_id;
            cmd.Parameters.Add("@PaymentMode", SqlDbType.Int).Value = Convert.ToInt32(enitity.PaymentModeID);
            cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar,150).Value = enitity.Remarks;
            cmd.Parameters.Add("@BankId", SqlDbType.Int).Value = Convert.ToInt32(enitity.BankId);
            cmd.Parameters.Add("@CardAmt", SqlDbType.Decimal).Value = Convert.ToDecimal(enitity.CardAmt);
            cmd.Parameters.Add("@CashAmt", SqlDbType.Decimal).Value = Convert.ToDecimal(enitity.AMOUNT);
            cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = Convert.ToDateTime(enitity.IDAT).Date;
            cmd.Parameters.Add("@Ref", SqlDbType.VarChar,100).Value =enitity.RefferenceNumber;
            cmd.Parameters.Add("@PaymentId", SqlDbType.BigInt).Value = Convert.ToInt64(enitity.PaymentId);
            cmd.Parameters.Add("@IUser", SqlDbType.VarChar,50).Value = enitity.IUSER;

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

        public DataTable GetRetailerList(Vendor_Outgoing_BO obj)
        {
            DataTable dt = new DataTable();
            List<String> myList = new List<String>();
            string WhereCondition = string.Empty;

            if (obj.Shop_id != String.Empty & obj.Shop_id != "0")
            {
                myList.Add("RP.Shop_id = '" + obj.Shop_id + "' ");
            }

            if (obj.Vendor_ID != String.Empty & obj.Vendor_ID != "0")
            {
                myList.Add("C.ID = '" + obj.Vendor_ID + "' ");
            }
            if (obj.FromDate != String.Empty & obj.ToDate != string.Empty)
            {
                myList.Add("RP.IDAT between '" + obj.FromDate + "' and '" + obj.ToDate + "' ");
            }
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

            //sql = "Select Vendor_Outgoing.OID,Vendor.Vendor_Name,Vendor_Outgoing.Vendor_ID,Vendor_Outgoing.AMOUNT,Vendor_Outgoing.Remarks,convert(CHAR(10), Vendor_Outgoing.IDAT, 120) as NEWDATE,Vendor_Outgoing.ReferenceNo from Vendor_Outgoing inner join Vendor on Vendor_Outgoing.Vendor_ID=Vendor.OID  " + WhereCondition + " order by Vendor_Outgoing.OID Desc";

            sql = string.Format(@"Select RP.OID, C.Name 'RetailerName',RP.ReferenceNo,Case When CashAmount is null or CashAmount='' then '0' Else CashAmount End 'CashAmount',RP.BankID,Case When CardAmount is null or CardAmount='' then '0' Else CardAmount End 'CardAmount',Case When RP.BankID is null OR RP.BankID=0 then '-' Else B.BankName+'('+B.AccountNo+')' End 'BankName' ,(ISNULL(CashAmount,0)+ISNULL(CardAmount,0)) 'TotalAmount',
Convert(Varchar(15),RP.IDAT,106) 'IDAT',RP.Remarks
From RetailerPayments RP
Inner Join Customers C On RP.RetailerID = C.ID
Left Join BankInfo B On RP.BankID = B.OID {0}", WhereCondition);

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

        public Vendor_Outgoing_BO GetRetailerPaymentById(string OID) 
        {
            Vendor_Outgoing_BO entity = new Vendor_Outgoing_BO();
            sql = string.Format(@"Select RP.OID,C.ID,C.Name'RetailerName',RP.PaymentModeID,RP.ReferenceNo,Case When CashAmount is null '0' Else CashAmount End 'CashAmount',RP.BankID,Case When CardAmount is null then '0' Else CardAmount End 'CardAmount',(ISNULL(CashAmount,0)+ISNULL(CardAmount,0)) 'TotalAmount',RP.IDAT,RP.Remarks
From RetailerPayments RP
Inner Join Customers C On RP.RetailerID = C.ID
Left Join BankInfo B On RP.BankID = B.OID Where RP.OID={0}", OID);
            cmd = new SqlCommand(sql, dbConnect);

            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    entity.PaymentId = rdr["OID"].ToString();
                    entity.CustomerName = rdr["RetailerName"].ToString();
                    entity.RefferenceNumber = rdr["ReferenceNo"].ToString();
                    entity.AMOUNT = rdr["CashAmount"].ToString();
                    entity.CardAmt = rdr["CardAmount"].ToString();
                    entity.BankId = rdr["BankID"].ToString();
                    entity.PaymentModeID = rdr["PaymentModeID"].ToString();
                    entity.IDAT = rdr["IDAT"].ToString();
                    entity.Vendor_ID = rdr["ID"].ToString();
                    entity.Remarks = rdr["Remarks"].ToString();
                }
                dbConnect.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally 
            {
                dbConnect.Close();
            }
            return entity;
        }
    }
}
