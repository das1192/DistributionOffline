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
    public class DiscountReference_DAL
    {
        SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
        SqlDataAdapter da = new SqlDataAdapter();
        SqlCommand cmd;
        String sql;
        SqlDataReader reader;

        public void Add(DiscountReference_BO entity)
        {
            if (string.IsNullOrEmpty(entity.OID))
            {
                sql = "insert into DiscountReference(DiscountTypeOID,Reference,Email,ActiveStatus,IUSER,IDAT,EUSER,EDAT) values (@DiscountTypeOID,@Reference,@Email,@ActiveStatus,@IUSER,@IDAT,@EUSER,@EDAT)";
                cmd = new SqlCommand(sql, dbConnect);
                cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 50).Value = entity.IUSER;
                cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = entity.IDAT;
            }
            else
            {
                sql = "update DiscountReference set DiscountTypeOID=@DiscountTypeOID,Reference=@Reference,Email=@Email,ActiveStatus=@ActiveStatus,EUSER=@EUSER,EDAT=@EDAT where OID=" + entity.OID + " ";
                cmd = new SqlCommand(sql, dbConnect);
            }
            cmd.Parameters.Add("@DiscountTypeOID", SqlDbType.BigInt).Value = entity.DiscountTypeOID;
            cmd.Parameters.Add("@Reference", SqlDbType.VarChar, 200).Value = entity.Reference;
            cmd.Parameters.Add("@Email", SqlDbType.VarChar, 100).Value = entity.Email;
            cmd.Parameters.Add("@ActiveStatus", SqlDbType.Int).Value = entity.ActiveStatus;
            cmd.Parameters.Add("@EUSER", SqlDbType.VarChar, 100).Value = entity.EUSER;
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



        public void Delete(DiscountReference_BO entity)
        {
            sql = "update DiscountReference set ActiveStatus=@ActiveStatus,EUSER=@EUSER,EDAT=@EDAT where OID=" + entity.OID + " ";
            cmd = new SqlCommand(sql, dbConnect);
            cmd.Parameters.Add("@ActiveStatus", SqlDbType.Int).Value = 0;
            cmd.Parameters.Add("@EUSER", SqlDbType.VarChar, 100).Value = entity.EUSER;
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


        public DataTable BindList(DiscountReference_BO entity)
        {
            DataTable dt = new DataTable();
            List<String> myList = new List<String>();
            string WhereCondition = string.Empty;

            if (entity.DiscountTypeOID != String.Empty & entity.DiscountTypeOID != "0")
            {
                myList.Add("DiscountType.OID = " + entity.DiscountTypeOID + " ");
            }
            myList.Add("DiscountReference.ActiveStatus = 1");

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
            sql = "select DiscountReference.OID,DiscountType.DiscountType,DiscountReference.Reference,DiscountReference.Email from DiscountReference inner join DiscountType on DiscountReference.DiscountTypeOID=DiscountType.OID " + WhereCondition + " order by DiscountType.DiscountType";
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



        public DiscountReference_BO GetById(string OID)
        {
            DiscountReference_BO entity = new DiscountReference_BO();
            sql = "select OID,DiscountTypeOID,Reference,Email from DiscountReference where OID=" + OID + " ";
            cmd = new SqlCommand(sql, dbConnect);
            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (string.IsNullOrEmpty(reader["OID"].ToString()))
                    {
                        entity.OID = string.Empty;
                    }
                    else
                    {
                        entity.OID = reader["OID"].ToString();
                    }

                    if (string.IsNullOrEmpty(reader["DiscountTypeOID"].ToString()))
                    {
                        entity.DiscountTypeOID = string.Empty;
                    }
                    else
                    {
                        entity.DiscountTypeOID = reader["DiscountTypeOID"].ToString();
                    }

                    if (string.IsNullOrEmpty(reader["Reference"].ToString()))
                    {
                        entity.Reference = string.Empty;
                    }
                    else
                    {
                        entity.Reference = reader["Reference"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["Email"].ToString()))
                    {
                        entity.Email = string.Empty;
                    }
                    else
                    {
                        entity.Email = reader["Email"].ToString();
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



    }
}
