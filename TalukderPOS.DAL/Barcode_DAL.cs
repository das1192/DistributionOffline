using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.Common;
using System.Collections.Generic;
using TalukderPOS.BusinessObjects;

namespace TalukderPOS.DAL
{
    public class Barcode_DAL
    {
        SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
        SqlDataAdapter da = new SqlDataAdapter();
        SqlCommand cmd;
        String sql;
        SqlDataReader reader;


        public void Add(Barcode_BO obj)
        {
            sql = "insert into Barcode(PROD_DES,BARCODE) values(@PROD_DES1,@BARCODE)";
            cmd = new SqlCommand(sql, dbConnect);
            cmd.Parameters.Add("@PROD_DES1", SqlDbType.BigInt).Value = obj.PROD_DES;
            cmd.Parameters.Add("@BARCODE", SqlDbType.VarChar, 50).Value = obj.BARCODE;            
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

       
        public string GetBarcode(String Barcode)
        {
            String found = string.Empty;
            sql = "select BARCODE from Barcode where BARCODE='" + Barcode + "' ";
            cmd = new SqlCommand(sql, dbConnect);
            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    found = reader["BARCODE"].ToString();
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
            return found;

        }

        public DataTable Gettable(String PROD_DES)
        {
            DataTable dt = new DataTable();
            sql = "select Barcode.OID, T_WGPG.OID as CategotyID,T_WGPG.WGPG_NAME,SubCategory.OID as SubcategoryID,SubCategory.SubCategoryName,Description.OID as DescriptionID,Description.Description,Barcode.BARCODE from Barcode,T_WGPG,SubCategory,Description where Barcode.PROD_DES=Description.OID and Description.SubCategoryID=SubCategory.OID and SubCategory.CategoryID=T_WGPG.OID and PROD_DES='" + PROD_DES + "' ";
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

        public void Delete(String OID)
        {
            sql = "delete from Barcode where OID='" + OID + "'";
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

        public string GetBarcodeForPROD_DES(String PROD_DES)
        {
            String found = string.Empty;
            sql = "select BARCODE from Barcode where PROD_DES='" + PROD_DES + "' ";
            cmd = new SqlCommand(sql, dbConnect);
            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    found = reader["BARCODE"].ToString();
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
            return found;

        }



        public DataTable Search(Barcode_BO entity)
        {
            DataTable dt = new DataTable();
            List<String> myList = new List<String>();
            string WhereCondition = string.Empty;
           
            if (entity.PROD_WGPG != String.Empty & entity.PROD_WGPG != "0")
            {
                myList.Add("T_WGPG.OID = " + entity.PROD_WGPG + " ");
            }
            if (entity.PROD_SUBCATEGORY != String.Empty & entity.PROD_SUBCATEGORY != "0")
            {
                myList.Add("SubCategory.OID = " + entity.PROD_SUBCATEGORY + " ");
            }
            if (entity.PROD_DES != String.Empty & entity.PROD_DES != "0")
            {
                myList.Add("Description.OID = " + entity.PROD_DES + " ");
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
            sql = "select Barcode.OID,T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,Description.Description,Barcode.BARCODE from Barcode inner join Description on Barcode.PROD_DES=Description.OID inner join SubCategory on Description.SubCategoryID=SubCategory.OID inner join T_WGPG on  SubCategory.CategoryID=T_WGPG.OID " + WhereCondition + " order by T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,Description.Description";            
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


        public DataTable StoreMasterStockAccessoriesBarcodeList(Barcode_BO entity)
        {
            DataTable dt = new DataTable();
            List<String> myList = new List<String>();
            string WhereCondition = string.Empty;

            if (entity.Branch != String.Empty & entity.Branch != "0")
            {
                myList.Add("StoreMasterStock.Branch = '" + entity.Branch + "' ");
            }          
            if (entity.PROD_SUBCATEGORY != String.Empty & entity.PROD_SUBCATEGORY != "0")
            {
                myList.Add("SubCategory.OID = " + entity.PROD_SUBCATEGORY + " ");
            }
            if (entity.PROD_DES != String.Empty & entity.PROD_DES != "0")
            {
                myList.Add("Description.OID = " + entity.PROD_DES + " ");
            }
            myList.Add("T_WGPG.OID = 111"); 
            myList.Add("StoreMasterStock.Quantity > 0 ");

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
            sql = "select T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,Description.Description,StoreMasterStock.Barcode,StoreMasterStock.SalePrice from StoreMasterStock inner join Description on StoreMasterStock.PROD_DES=Description.OID inner join SubCategory on Description.SubCategoryID=SubCategory.OID inner join T_WGPG on SubCategory.CategoryID=T_WGPG.OID " + WhereCondition + " order by T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,Description.Description";            
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




    }
}
