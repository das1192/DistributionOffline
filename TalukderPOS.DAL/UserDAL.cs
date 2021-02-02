using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using TalukderPOS.BusinessObjects;
using System.Configuration;


namespace TalukderPOS.DAL
{
    public class UserDAL
    {
        SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
        SqlCommand cmd;
        SqlCommand cmd2;
        SqlDataAdapter da = new SqlDataAdapter();
        String sql;
        String sql2;
        SqlDataReader reader;


        public void Add(User entity)
        {
            if (entity.Id == string.Empty)
            {
                sql = "INSERT INTO [User](CCOM_OID,UserId,UserName,Password,ConfirmPassword,UserFullName,ActiveStatus,IUSER,IDAT,EUSER,EDAT) VALUES(@CCOM_OID,@UserId,@UserName,@Password,@ConfirmPassword,@UserFullName,@ActiveStatus,@IUSER,@IDAT,@EUSER,@EDAT)";
                cmd = new SqlCommand(sql, dbConnect);
                cmd.Parameters.Add("@ActiveStatus", SqlDbType.Int).Value = entity.ActiveStatus;
                cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 50).Value = entity.IUSER;
                cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = entity.IDAT;
            }
            else
            {
                sql = "update [User] set CCOM_OID=@CCOM_OID,UserId=@UserId,UserName=@UserName,Password=@Password,ConfirmPassword=@ConfirmPassword,UserFullName=@UserFullName,EUSER=@EUSER,EDAT=@EDAT where Id=@Id";
                cmd = new SqlCommand(sql, dbConnect);
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = entity.Id;
            }
            cmd.Parameters.Add("@CCOM_OID", SqlDbType.VarChar, 100).Value = entity.CCOM_OID;
            cmd.Parameters.Add("@UserId", SqlDbType.VarChar, 100).Value = entity.UserId;

            cmd.Parameters.Add("@UserName", SqlDbType.VarChar, 100).Value = entity.UserName;
            cmd.Parameters.Add("@Password", SqlDbType.VarChar, 100).Value = entity.Password;
            cmd.Parameters.Add("@ConfirmPassword", SqlDbType.VarChar, 100).Value = entity.ConfirmPassword;
            cmd.Parameters.Add("@UserFullName", SqlDbType.VarChar, 100).Value = entity.UserFullName;
            cmd.Parameters.Add("@EUSER", SqlDbType.VarChar, 50).Value = entity.EUSER;
            cmd.Parameters.Add("@EDAT", SqlDbType.Date).Value = entity.EDAT;
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
        public string GetKEYOF()
        {
            string CategorySchema = string.Empty;
            sql = "select TOP(1) CategorySchema from CategorySchema order by OID desc";
            cmd = new SqlCommand(sql, dbConnect);
            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    CategorySchema = reader["CategorySchema"].ToString();
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
            return CategorySchema;
        }
        public DataTable GetUserList(User entity)
        {

            DataTable dt = new DataTable();
            List<String> myList = new List<String>();
            string WhereCondition = string.Empty;

            if (entity.CCOM_OID != String.Empty & entity.CCOM_OID != "0")
            {
                myList.Add("[User].CCOM_OID = '" + entity.CCOM_OID + "' ");
            }
            myList.Add("[User].ActiveStatus=1");
            myList.Add("T_CCOM.CCOM_ACTV=1");

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

            sql = "select [User].Id,[User].UserId,[User].UserFullName,[User].UserName,[User].Password,T_CCOM.CCOM_NAME from [User] inner join T_CCOM on [User].CCOM_OID=T_CCOM.OID " + WhereCondition + " order by T_CCOM.CCOM_NAME ";
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
        public DataTable GetUserListShop(User entity)
        {

            DataTable dt = new DataTable();
            List<String> myList = new List<String>();
            string WhereCondition = string.Empty;

            if (entity.CCOM_OID != String.Empty & entity.CCOM_OID != "0")
            {
                myList.Add("[User].CCOM_OID = '" + entity.CCOM_OID + "' ");
            }
            myList.Add("[User].ActiveStatus=1");
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

            sql = "select [StuffInformation].StuffID,[StuffInformation].MobileNumber,[StuffInformation].EMailAddress,[User].Id,[User].UserId,[User].UserFullName,[User].UserName,[User].Password,ShopInfo.ShopName from [User] inner join ShopInfo on [User].CCOM_OID=ShopInfo.OID LEFT JOIN StuffInformation ON [User].Id = StuffInformation.UserMaxID " + WhereCondition + " ";
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
        

        
        public void User_Delete(User entity)
        {
            String UserId = string.Empty;
            sql = "select [User].UserId from [User]  where [User].Id=" + entity.Id + " ";
            cmd = new SqlCommand(sql, dbConnect);
            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    UserId = reader["UserId"].ToString();
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




            if (UserId != string.Empty)
            {

                sql = "update [User] set ActiveStatus=@ActiveStatus,EUSER=@EUSER,EDAT=@EDAT where Id=@Id ";
                cmd = new SqlCommand(sql, dbConnect);
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = entity.Id;
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

                sql = "delete from MenuPermission where UserID='" + UserId + "'";
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
        }

        public string getusername(string username)
        {
            string userID = string.Empty;
            sql = "select [User].UserId from [User] where [User].UserName='" + username + "' ";

            cmd = new SqlCommand(sql, dbConnect);
            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    userID = reader["UserId"].ToString();
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
            return userID;
        }

        public int User_GetMaxID()
        {
            int UserId = 0;
            sql = "SELECT MAX([Id]) as Id FROM [User]";
            cmd = new SqlCommand(sql, dbConnect);
            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    UserId = Convert.ToInt32(reader["Id"].ToString());
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
            return UserId;
        }


        public string GetUserIDByUserNamePassword(String username, string password)
        {
            string userID = string.Empty;
            sql = "select [User].UserId from [User] where [User].UserName='" + username + "' and [User].Password='" + password + "' AND [User].ActiveStatus='1'";

            cmd = new SqlCommand(sql, dbConnect);
            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    userID = reader["UserId"].ToString();
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
            return userID;
        }



        public DataTable User_GetAllForDDL(string storeid)
        {
            DataTable dt = new DataTable();
            if (storeid == "3")
            {
                sql = "SELECT [User].UserId,[User].UserName FROM [User] where [User].ActiveStatus=1";
            }
            else
            {
                sql = "SELECT [User].UserId,[User].UserName FROM [User] where [User].ActiveStatus=1 AND CCOM_OID=" + storeid + " ";
            }

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



        public User GetById(string Id)
        {
            User obj = new User();
            sql = "select [StuffInformation].EMailAddress,[StuffInformation].Address,[StuffInformation].MobileNumber,[StuffInformation].NID,[User].Id,[User].CCOM_OID,[User].UserId,[User].UserName,[User].Password,[User].ConfirmPassword,[User].UserFullName from [User] LEFT JOIN [StuffInformation] ON [User].Id = [StuffInformation].UserMaxID where [User].Id=" + Id + " ";
            cmd = new SqlCommand(sql, dbConnect);
            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (string.IsNullOrEmpty(reader["Id"].ToString()))
                    {
                        obj.Id = string.Empty;
                    }
                    else
                    {
                        obj.Id = reader["Id"].ToString();
                    }

                    if (string.IsNullOrEmpty(reader["CCOM_OID"].ToString()))
                    {
                        obj.CCOM_OID = string.Empty;
                    }
                    else
                    {
                        obj.CCOM_OID = reader["CCOM_OID"].ToString();
                    }

                    if (string.IsNullOrEmpty(reader["UserId"].ToString()))
                    {
                        obj.UserId = string.Empty;
                    }
                    else
                    {
                        obj.UserId = reader["UserId"].ToString();
                    }

                    if (string.IsNullOrEmpty(reader["UserName"].ToString()))
                    {
                        obj.UserName = string.Empty;
                    }
                    else
                    {
                        obj.UserName = reader["UserName"].ToString();
                    }

                    if (string.IsNullOrEmpty(reader["Password"].ToString()))
                    {
                        obj.Password = string.Empty;
                    }
                    else
                    {
                        obj.Password = reader["Password"].ToString();
                    }

                    if (string.IsNullOrEmpty(reader["ConfirmPassword"].ToString()))
                    {
                        obj.ConfirmPassword = string.Empty;
                    }
                    else
                    {
                        obj.ConfirmPassword = reader["ConfirmPassword"].ToString();
                    }

                    if (string.IsNullOrEmpty(reader["UserFullName"].ToString()))
                    {
                        obj.UserFullName = string.Empty;
                    }
                    else
                    {
                        obj.UserFullName = reader["UserFullName"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["EMailAddress"].ToString()))
                    {
                        obj.EmailID = string.Empty;
                    }
                    else
                    {
                        obj.EmailID = reader["EMailAddress"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["MobileNumber"].ToString()))
                    {
                        obj.MobileNumber = string.Empty;
                    }
                    else
                    {
                        obj.MobileNumber = reader["MobileNumber"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["NID"].ToString()))
                    {
                        obj.nid = string.Empty;
                    }
                    else
                    {
                        obj.nid = reader["NID"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["Address"].ToString()))
                    {
                        obj.address = string.Empty;
                    }
                    else
                    {
                        obj.address = reader["Address"].ToString();
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
