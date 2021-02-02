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
    public class StuffInformation_DAL
    {
        SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
        SqlDataAdapter da = new SqlDataAdapter();
        SqlCommand cmd;
        String sql;
        SqlDataReader reader;

        public void Add(StuffInformation_BO entity)
        {
            if (entity.OID == string.Empty)
            {
                sql = "insert into StuffInformation(StuffID,Name,UserMaxID,CCOM_OID,MobileNumber,AlternativeMobileNo,EMailAddress,AlternativeEMailAddress,ActiveStatus,IUSER,IDAT,EUSER,EDAT,NID,Address) values (@StuffID,@Name,@UserMaxID,@CCOM_OID,@MobileNumber,@AlternativeMobileNo,@EMailAddress,@AlternativeEMailAddress,@ActiveStatus,@IUSER,@IDAT,@EUSER,@EDAT,@NID,@Address)";
                cmd = new SqlCommand(sql, dbConnect);
                cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 50).Value = entity.IUSER;
                cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = entity.IDAT;
            }
            else {
                sql = "update StuffInformation set NID=@NID,Address=@Address,StuffID=@StuffID,Name=@Name,CCOM_OID=@CCOM_OID,MobileNumber=@MobileNumber,AlternativeMobileNo=@AlternativeMobileNo,EMailAddress=@EMailAddress,AlternativeEMailAddress=@AlternativeEMailAddress,ActiveStatus=@ActiveStatus,EUSER=@EUSER,EDAT=@EDAT where UserMaxID=" + entity.OID + " ";
                cmd = new SqlCommand(sql, dbConnect);             
            }            
            
            cmd.Parameters.Add("@StuffID", SqlDbType.VarChar, 50).Value = entity.StuffID;
            cmd.Parameters.Add("@Name", SqlDbType.VarChar, 100).Value = entity.Name;
            cmd.Parameters.Add("@UserMaxID", SqlDbType.VarChar, 50).Value = entity.UserMaxID;
            cmd.Parameters.Add("@CCOM_OID", SqlDbType.VarChar, 50).Value = entity.CCOM_OID;
            cmd.Parameters.Add("@MobileNumber", SqlDbType.VarChar, 50).Value = entity.MobileNumber;
            cmd.Parameters.Add("@AlternativeMobileNo", SqlDbType.VarChar, 50).Value = entity.AlternativeMobileNo;
            cmd.Parameters.Add("@Address", SqlDbType.VarChar, 50).Value = entity.address;
            cmd.Parameters.Add("@NID", SqlDbType.VarChar, 50).Value = entity.nid;
            cmd.Parameters.Add("@EMailAddress", SqlDbType.VarChar, 50).Value = entity.EMailAddress;
            cmd.Parameters.Add("@AlternativeEMailAddress", SqlDbType.VarChar, 50).Value = entity.AlternativeEMailAddress;
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

        public void update(StuffInformation_BO entity)
        {
            sql = "update StuffInformation set StuffID=@StuffID,Name=@Name,CCOM_OID=@CCOM_OID,MobileNumber=@MobileNumber where OID="+ entity.OID +" ";
            cmd = new SqlCommand(sql, dbConnect);
            cmd.Parameters.Add("@StuffID", SqlDbType.VarChar, 50).Value = entity.StuffID;
            cmd.Parameters.Add("@Name", SqlDbType.VarChar, 100).Value = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(entity.Name.ToLower());
            cmd.Parameters.Add("@CCOM_OID", SqlDbType.VarChar, 50).Value = entity.CCOM_OID;
            cmd.Parameters.Add("@MobileNumber", SqlDbType.VarChar, 50).Value = entity.MobileNumber;
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

        public void Delete(StuffInformation_BO entity)
        {
           sql = "update StuffInformation set ActiveStatus=@ActiveStatus,EUSER=@EUSER,EDAT=@EDAT where OID=@OID ";
           cmd = new SqlCommand(sql, dbConnect);
           cmd.Parameters.Add("@ActiveStatus", SqlDbType.Int).Value = entity.ActiveStatus;           
           cmd.Parameters.Add("@EUSER", SqlDbType.VarChar, 50).Value = entity.EUSER;
           cmd.Parameters.Add("@EDAT", SqlDbType.Date).Value = entity.EDAT;
           cmd.Parameters.Add("@OID", SqlDbType.Int).Value = entity.OID;
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


        public DataTable StuffInformation_BindList(StuffInformation_BO entity)
        {
            DataTable dt = new DataTable();
            List<String> myList = new List<String>();
            string WhereCondition = string.Empty;

            if (entity.CCOM_OID != String.Empty & entity.CCOM_OID != "0")
            {
                myList.Add("StuffInformation.CCOM_OID = '" + entity.CCOM_OID + "' ");
            }
            myList.Add("StuffInformation.ActiveStatus=1");
            myList.Add("ShopInfo.ActiveStatus=1");

            string[] myArray = myList.ToArray();
            string where = string.Join(" and ", myArray);

            if (where == string.Empty)
            {
                WhereCondition = string.Empty;
            }
            else
            {
                WhereCondition = "where " + where + " ";
            }


            sql = "select ShopInfo.ShopName,StuffInformation.OID,StuffInformation.StuffID,StuffInformation.Name,StuffInformation.MobileNumber,StuffInformation.AlternativeMobileNo,StuffInformation.EMailAddress,StuffInformation.AlternativeEMailAddress from StuffInformation inner join ShopInfo on StuffInformation.CCOM_OID=ShopInfo.OID " + WhereCondition + " order by StuffInformation.OID DESC";
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

      

        public List<StuffInformation_BO> GetById(string OID)
        {
            List<StuffInformation_BO> lst = new List<StuffInformation_BO>();
            sql = "select StuffInformation.OID,StuffInformation.StuffID, StuffInformation.Name,StuffInformation.CCOM_OID,StuffInformation.MobileNumber ,StuffInformation.AlternativeMobileNo,StuffInformation.EMailAddress,StuffInformation.AlternativeEMailAddress from StuffInformation where StuffInformation.OID=" + OID + " ";
            cmd = new SqlCommand(sql, dbConnect);
            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    StuffInformation_BO entity = new StuffInformation_BO();
                    if (string.IsNullOrEmpty(reader["OID"].ToString()))
                    {
                        entity.OID = string.Empty;
                    }
                    else
                    {
                        entity.OID = reader["OID"].ToString();
                    }
                    
                    if (string.IsNullOrEmpty(reader["StuffID"].ToString()))
                    {
                        entity.StuffID = string.Empty;
                    }
                    else 
                    {
                        entity.StuffID = reader["StuffID"].ToString();
                    }

                    if (string.IsNullOrEmpty(reader["Name"].ToString()))
                    {
                        entity.Name = string.Empty;
                    }
                    else
                    {
                        entity.Name = reader["Name"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["CCOM_OID"].ToString()))
                    {
                        entity.CCOM_OID = string.Empty;
                    }
                    else
                    {
                        entity.CCOM_OID = reader["CCOM_OID"].ToString();
                    }

                    if (string.IsNullOrEmpty(reader["MobileNumber"].ToString()))
                    {
                        entity.MobileNumber = string.Empty;
                    }
                    else
                    {
                        entity.MobileNumber = reader["MobileNumber"].ToString();
                    }

                    if (string.IsNullOrEmpty(reader["AlternativeMobileNo"].ToString()))
                    {
                        entity.AlternativeMobileNo = string.Empty;
                    }
                    else
                    {
                        entity.AlternativeMobileNo = reader["AlternativeMobileNo"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["EMailAddress"].ToString()))
                    {
                        entity.EMailAddress = string.Empty;
                    }
                    else
                    {
                        entity.EMailAddress = reader["EMailAddress"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["AlternativeEMailAddress"].ToString()))
                    {
                        entity.AlternativeEMailAddress = string.Empty;
                    }
                    else
                    {
                        entity.AlternativeEMailAddress = reader["AlternativeEMailAddress"].ToString();
                    }
                    lst.Add(entity);
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
            return lst;
        }

      





    }
}
