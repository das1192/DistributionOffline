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
    public class ProductPrice_DAL
	{
        SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
        SqlDataAdapter da = new SqlDataAdapter();
        SqlCommand cmd;
        String sql;
        SqlDataReader reader;

        public void Add(ProductPrice_BO obj)
        {
            if (string.IsNullOrEmpty(obj.OID))
            {
                sql = "insert into ProductPrice(DescriptionOID,PurchasePrice,SalePrice,Active,IUSER,IDAT,EUSER,EDAT) values (@DescriptionOID,@PurchasePrice,@SalePrice,@Active,@IUSER,@IDAT,@EUSER,@EDAT)";
                cmd = new SqlCommand(sql, dbConnect);
                cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 100).Value = obj.IUSER;
                cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = DateTime.Today.Date;
            }
            else
            {
                sql = "update ProductPrice set DescriptionOID=@DescriptionOID,PurchasePrice=@PurchasePrice,SalePrice=@SalePrice,Active=@Active,EUSER=@EUSER,EDAT=@EDAT where OID='" + obj.OID + "'";
                cmd = new SqlCommand(sql, dbConnect);
            }
            cmd.Parameters.Add("@DescriptionOID", SqlDbType.BigInt).Value = Convert.ToInt64(obj.DescriptionOID);
            cmd.Parameters.Add("@PurchasePrice", SqlDbType.BigInt).Value = Convert.ToInt64(obj.PurchasePrice);
            cmd.Parameters.Add("@SalePrice", SqlDbType.BigInt).Value = Convert.ToInt64(obj.SalePrice);
            cmd.Parameters.Add("@Active", SqlDbType.VarChar,1).Value = "1";      
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

            if (string.IsNullOrEmpty(obj.OID))
            {}
            else {
                T_PROD entity = new T_PROD();
                entity.Branch = string.Empty;
                entity.PROD_WGPG = obj.CategoryOID;
                entity.PROD_SUBCATEGORY = obj.SubCategoryOID;
                entity.PROD_DES = obj.DescriptionOID;
                entity.SalePrice = obj.SalePrice;
                T_PRODDAL DAL = new T_PRODDAL();
                DAL.ProductCostUpdate(entity);
            }
        }



        public void Delete(ProductPrice_BO obj)
        {
            sql = "update ProductPrice set Active=@Active,EUSER=@EUSER,EDAT=@EDAT where OID='" + obj.OID + "'";
            cmd = new SqlCommand(sql, dbConnect);
            cmd.Parameters.Add("@Active", SqlDbType.VarChar, 1).Value = "0";
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


        public DataTable BindList(ProductPrice_BO obj)
        {
            DataTable dt = new DataTable();
            List<String> myList = new List<String>();
            string WhereCondition = string.Empty;

            if (obj.CategoryOID != String.Empty & obj.CategoryOID != "0")
            {
                myList.Add("T_WGPG.OID = " + obj.CategoryOID + " ");
            }
            if (obj.SubCategoryOID != String.Empty & obj.SubCategoryOID != "0")
            {
                myList.Add("SubCategory.OID = " + obj.SubCategoryOID + " ");
            }
            if (obj.DescriptionOID != String.Empty & obj.DescriptionOID != "0")
            {
                myList.Add("ProductPrice.DescriptionOID = " + obj.DescriptionOID + " ");
            }
            myList.Add("ProductPrice.Active = 1");

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
            sql = "select ProductPrice.OID,T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,Description.Description,ProductPrice.PurchasePrice,ProductPrice.SalePrice from ProductPrice inner join Description on ProductPrice.DescriptionOID=Description.OID inner join SubCategory on Description.SubCategoryID=SubCategory.OID inner join T_WGPG on SubCategory.CategoryID=T_WGPG.OID " + WhereCondition + " order by T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,Description.Description";           
            
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



        public ProductPrice_BO GetById(string OID)
        {
            ProductPrice_BO obj = new ProductPrice_BO();
            sql = "select ProductPrice.OID,T_WGPG.OID as CategoryOID,SubCategory.OID as SubCategoryOID,Description.OID as DescriptionOID,ProductPrice.PurchasePrice,ProductPrice.SalePrice from ProductPrice inner join Description on ProductPrice.DescriptionOID=Description.OID inner join SubCategory on Description.SubCategoryID=SubCategory.OID inner join T_WGPG on SubCategory.CategoryID=T_WGPG.OID where ProductPrice.OID='" + OID + "' ";
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

                    if (string.IsNullOrEmpty(reader["CategoryOID"].ToString()))
                    {
                        obj.CategoryOID = string.Empty;
                    }
                    else
                    {
                        obj.CategoryOID = reader["CategoryOID"].ToString();
                    }

                    if (string.IsNullOrEmpty(reader["SubCategoryOID"].ToString()))
                    {
                        obj.SubCategoryOID = string.Empty;
                    }
                    else
                    {
                        obj.SubCategoryOID = reader["SubCategoryOID"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["DescriptionOID"].ToString()))
                    {
                        obj.DescriptionOID = string.Empty;
                    }
                    else
                    {
                        obj.DescriptionOID = reader["DescriptionOID"].ToString();
                    }

                    if (string.IsNullOrEmpty(reader["PurchasePrice"].ToString()))
                    {
                        obj.PurchasePrice = string.Empty;
                    }
                    else
                    {
                        obj.PurchasePrice = reader["PurchasePrice"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["SalePrice"].ToString()))
                    {
                        obj.SalePrice = string.Empty;
                    }
                    else
                    {
                        obj.SalePrice = reader["SalePrice"].ToString();
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
