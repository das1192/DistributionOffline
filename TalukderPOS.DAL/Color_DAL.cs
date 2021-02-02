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
using System.Security.Cryptography;


namespace TalukderPOS.DAL
{
    public class Color_DAL
    {
        SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
        SqlDataAdapter da = new SqlDataAdapter();
        SqlCommand cmd;
        String sql;
        SqlDataReader reader;


        public void Add(Color_BO entity)
        {
            if (string.IsNullOrEmpty(entity.OID))
            {
                Int64 DescriptionOID;

                sql = "insert into Description(SubCategoryID,Description,Active,SESPrice,MRP,IUSER,IDAT,EUSER,EDAT) OUTPUT INSERTED.OID values (@SubCategoryID,@Description,@Active,@SESPrice,@MRP,@IUSER,@IDAT,@EUSER,@EDAT)";
                cmd = new SqlCommand(sql, dbConnect);
                cmd.Parameters.Add("@SubCategoryID", SqlDbType.BigInt).Value = Convert.ToInt64(entity.SubCategoryID);
                cmd.Parameters.Add("@Description", SqlDbType.VarChar, 100).Value = entity.Description;
                cmd.Parameters.Add("@Active", SqlDbType.VarChar, 1).Value = entity.Active;
                cmd.Parameters.Add("@SESPrice", SqlDbType.BigInt).Value = entity.SESPrice;
                cmd.Parameters.Add("@MRP", SqlDbType.BigInt).Value = entity.MRP;
                cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 100).Value = entity.IUSER;
                cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = DateTime.Today.Date;
                cmd.Parameters.Add("@EUSER", SqlDbType.VarChar, 100).Value = entity.EUSER;
                cmd.Parameters.Add("@EDAT", SqlDbType.Date).Value = DateTime.Today.Date;
                try
                {
                    if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                    DescriptionOID = Convert.ToInt64(cmd.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dbConnect.Close();
                }
                if (entity.CategoryID == "111")
                {
                    GenerateBarcodeForAccessories(DescriptionOID);
                }

            }
            else
            {
                sql = "update Description set Description=@Description,Active=@Active,SESPrice=@SESPrice,MRP=@MRP,EUSER=@EUSER,EDAT=@EDAT where OID=" + entity.OID + " ";
                cmd = new SqlCommand(sql, dbConnect);
                //cmd.Parameters.Add("@SubCategoryID", SqlDbType.BigInt).Value = Convert.ToInt64(entity.SubCategoryID);
                cmd.Parameters.Add("@Description", SqlDbType.VarChar, 100).Value = entity.Description;
                cmd.Parameters.Add("@Active", SqlDbType.VarChar, 1).Value = entity.Active;
                cmd.Parameters.Add("@SESPrice", SqlDbType.BigInt).Value = entity.SESPrice;
                cmd.Parameters.Add("@MRP", SqlDbType.BigInt).Value = entity.MRP;
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

        }

        public void GenerateBarcodeForAccessories(Int64 DescriptionOID)
        {
            sql = "insert into Barcode(PROD_DES,BARCODE) values(@PROD_DES,@BARCODE)";
            cmd = new SqlCommand(sql, dbConnect);
            cmd.Parameters.Add("@PROD_DES", SqlDbType.BigInt).Value = DescriptionOID.ToString();
            cmd.Parameters.Add("@BARCODE", SqlDbType.VarChar, 50).Value = GetUniqueKey(8);
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

        public string GetUniqueKey(int maxSize)
        {
            char[] chars = new char[61];
            chars = "123456789".ToCharArray();
            byte[] data = new byte[0];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            data = new byte[maxSize - 1];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(maxSize);
            foreach (byte b in data)
            {
                result.Append(chars[(b % chars.Length)]);
            }
            return result.ToString();
        }



        public void Delete(Color_BO entity)
        {

            sql = "update Description set Active=@Active,EUSER=@EUSER,EDAT=@EDAT where OID=" + entity.OID + " ";
            cmd = new SqlCommand(sql, dbConnect);
            cmd.Parameters.Add("@Active", SqlDbType.VarChar, 1).Value = "0";
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
        public void ActiveDescription(Color_BO entity)
        {

            sql = "update Description set Active=@Active,EUSER=@EUSER,EDAT=@EDAT where OID=" + entity.OID + " ";
            cmd = new SqlCommand(sql, dbConnect);
            cmd.Parameters.Add("@Active", SqlDbType.VarChar, 1).Value = "1";
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

        public DataTable BindList(Color_BO entity)
        {
            DataTable dt = new DataTable();
            List<String> myList = new List<String>();
            string WhereCondition = string.Empty;

            if (entity.CategoryID != String.Empty & entity.CategoryID != "0")
            {
                myList.Add("T_WGPG.OID = " + entity.CategoryID + " ");
            }
            if (entity.Shop_id != String.Empty & entity.Shop_id != "0")
            {
                myList.Add("T_WGPG.Shop_id = " + entity.Shop_id + " ");
            }
            if (entity.SubCategoryID != String.Empty & entity.SubCategoryID != "0")
            {
                myList.Add("SubCategory.OID = " + entity.SubCategoryID + " ");
            }

            // sadiq 170917
            if (entity.Active == "1")
            {
                myList.Add("Description.Active = '1' ");
            }
            else if (entity.Active == "0")
            {
                myList.Add("Description.Active = '0' ");
            }
            //else
            //{
            //    //myList.Add("Description.Active = '1' ");
            //}
            myList.Add("T_WGPG.WGPG_ACTV='1'");
            myList.Add("SubCategory.Active='1'");
            //myList.Add("Description.Active='1'");
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
            sql = "select Description.OID,T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,Description.Description,Description.SESPrice,Description.MRP from Description inner join SubCategory on Description.SubCategoryID=SubCategory.OID inner join T_WGPG on SubCategory.CategoryID=T_WGPG.OID " + WhereCondition + " order by T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,Description.Description";
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



        public Color_BO GetById(string OID)
        {
            Color_BO entity = new Color_BO();
            sql = "select Description.OID,T_WGPG.OID as CategoryOID,SubCategory.OID as SubCategoryOID,Description.Description,Description.SESPrice,Description.MRP from Description  inner join SubCategory on Description.SubCategoryID=SubCategory.OID inner join T_WGPG on SubCategory.CategoryID=T_WGPG.OID where Description.OID=" + OID + " ";
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

                    if (string.IsNullOrEmpty(reader["CategoryOID"].ToString()))
                    {
                        entity.CategoryID = string.Empty;
                    }
                    else
                    {
                        entity.CategoryID = reader["CategoryOID"].ToString();
                    }

                    if (string.IsNullOrEmpty(reader["SubCategoryOID"].ToString()))
                    {
                        entity.SubCategoryID = string.Empty;
                    }
                    else
                    {
                        entity.SubCategoryID = reader["SubCategoryOID"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["Description"].ToString()))
                    {
                        entity.Description = string.Empty;
                    }
                    else
                    {
                        entity.Description = reader["Description"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["SESPrice"].ToString()))
                    {
                        entity.SESPrice = string.Empty;
                    }
                    else
                    {
                        entity.SESPrice = reader["SESPrice"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["MRP"].ToString()))
                    {
                        entity.MRP = string.Empty;
                    }
                    else
                    {
                        entity.MRP = reader["MRP"].ToString();
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
