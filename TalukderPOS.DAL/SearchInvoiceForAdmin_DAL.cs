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
    public class SearchInvoiceForAdmin_DAL
    {
        SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
        SqlDataAdapter da = new SqlDataAdapter();
        SqlCommand cmd;
        String sql;
        //SqlDataReader reader;

        public DataTable GetInvoiceList(T_PROD entity)
        {
            DataTable dt = new DataTable();
            List<String> myList = new List<String>();
            string WhereCondition = string.Empty;

            if (entity.Branch != String.Empty & entity.Branch != "0")
            {
                myList.Add("T_SALES_MST.StoreID = '" + entity.Branch + "' ");
            }           
            if (entity.FromDate != String.Empty & entity.ToDate != string.Empty)
            {
                myList.Add("T_SALES_MST.IDAT between '" + entity.FromDate + "' and '" + entity.ToDate + "' ");
            }
            if (entity.DropStatus != String.Empty)
            {
                myList.Add("T_SALES_MST.DropStatus = "+ entity.DropStatus +" ");
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
            sql = "select CustomerInformation.InvoiceNo,CustomerInformation.CustomerName,CustomerInformation.Address,CustomerInformation.MobileNo,T_SALES_MST.IDAT,T_SALES_MST.DropStatus,StuffInformation.Name from T_SALES_MST inner join CustomerInformation on T_SALES_MST.InvoiceNo=CustomerInformation.InvoiceNo inner join StuffInformation on  T_SALES_MST.StuffID=StuffInformation.StuffID " + WhereCondition + " order by T_SALES_MST.SlNo ASC";                                    
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
        public DataTable GetCASHINOUT(T_PROD entity)
        {
            DataTable dt = new DataTable();
            List<String> myList = new List<String>();
            string WhereCondition = string.Empty;

            if (entity.Branch != String.Empty & entity.Branch != "0")
            {
                myList.Add("CASHINOUT.Branch = '" + entity.Branch + "' ");
            }           
            if (entity.FromDate != String.Empty & entity.ToDate != string.Empty)
            {
                myList.Add("CASHINOUT.IDAT between '" + entity.FromDate + "' and '" + entity.ToDate + "' ");
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
            sql = "select ISNULL(CONVERT(DECIMAL(10,0),SUM(CASE WHEN PAYMENTMODE <> '17' OR PAYMENTMODE IS NULL THEN CashIN ELSE 0 END)),0) AS CashIN,sum(isnull(cast(CashOUT as float),0)) as CashOUT,convert(CHAR(10),IDAT, 120) as IDAT from CASHINOUT " + WhereCondition + " group by IDAT,Branch order by IDAT DESC";                                    
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
        

        public DataTable DetailDatabase(T_PROD entity)
        {
            DataTable dt = new DataTable();
            List<String> myList = new List<String>();
            string WhereCondition = string.Empty;

            if (entity.Branch != String.Empty & entity.Branch != "0")
            {
                myList.Add("T_SALES_MST.StoreID = '" + entity.Branch + "' ");
            }
            if (entity.FromDate != String.Empty & entity.ToDate != string.Empty)
            {
                myList.Add("T_SALES_MST.IDAT between '" + entity.FromDate + "' and '" + entity.ToDate + "' ");
            }
            if (entity.DropStatus != String.Empty)
            {
                myList.Add("T_SALES_MST.DropStatus = " + entity.DropStatus + " ");
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

            sql = "Select T_SALES_MST.IDAT as Purchase_Date,case T_SALES_MST.DropStatus when 1 then 'Y' when 0 then 'N' end as DropStatus,CustomerInformation.CustomerName as Name,CustomerInformation.DateOfBirth as DOB,CustomerInformation.MobileNo as MOB,CustomerInformation.AlternativeMobileNo as ALT_MOB,CustomerInformation.Address,CustomerInformation.EmailAddress,T_WGPG.WGPG_NAME as Category,SubCategory.SubCategoryName as Model,Description.Description,T_SALES_DTL.Barcode,PaymentMode.PaymentMode,T_SALES_MST.InvoiceNo from T_SALES_MST inner join T_SALES_DTL on T_SALES_MST.InvoiceNo=T_SALES_DTL.InvoiceNo inner join CustomerInformation on T_SALES_MST.InvoiceNo=CustomerInformation.InvoiceNo inner join Description on T_SALES_DTL.DescriptionID=Description.OID inner join SubCategory on Description.SubCategoryID=SubCategory.OID inner join T_WGPG on SubCategory.CategoryID=T_WGPG.OID inner join PaymentMode on T_SALES_MST.PaymentModeID=PaymentMode.OID " + WhereCondition + " order by T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,Description.Description ";            
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

        public DataTable CustomerCare(T_PROD entity)
        {
            DataTable dt = new DataTable();
            List<String> myList = new List<String>();
            string WhereCondition = string.Empty;

            if (entity.Branch != String.Empty & entity.Branch != "0")
            {
                myList.Add("T_SALES_MST.StoreID = '" + entity.Branch + "' ");
            }
            if (entity.FromDate != String.Empty & entity.ToDate != string.Empty)
            {
                myList.Add("T_SALES_MST.IDAT between '" + entity.FromDate + "' and '" + entity.ToDate + "' ");
            }
            if (entity.PROD_WGPG.ToString() != String.Empty & entity.PROD_WGPG.ToString() != "0")
            {
                myList.Add("T_WGPG.OID = " + entity.PROD_WGPG + " ");
            }
            if (entity.PROD_SUBCATEGORY.ToString() != String.Empty & entity.PROD_SUBCATEGORY.ToString() != "0")
            {
                myList.Add("SubCategory.OID = " + entity.PROD_SUBCATEGORY + " ");
            }
            if (entity.PROD_DES.ToString() != String.Empty & entity.PROD_DES.ToString() != "0")
            {
                myList.Add("Description.OID = " + entity.PROD_DES + " ");
            }
            if (entity.ProductPriceFrom != String.Empty & entity.ProductPriceTo != string.Empty)
            {
                myList.Add("T_SALES_DTL.SalePrice between " + entity.ProductPriceFrom + " and " + entity.ProductPriceTo + " ");
            }
            myList.Add("T_SALES_MST.DropStatus = 0");

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

            sql = "Select T_SALES_MST.InvoiceNo, T_SALES_MST.IDAT as Purchase_Date,CustomerInformation.CustomerName as Name,CustomerInformation.DateOfBirth as DOB,CustomerInformation.MobileNo as MOB,CustomerInformation.AlternativeMobileNo as ALT_MOB,CustomerInformation.Address,CustomerInformation.EmailAddress,T_WGPG.WGPG_NAME as Category,SubCategory.SubCategoryName as Model,Description.Description,T_SALES_DTL.Barcode,PaymentMode.PaymentMode,T_SALES_DTL.SalePrice from T_SALES_MST inner join T_SALES_DTL on T_SALES_MST.InvoiceNo=T_SALES_DTL.InvoiceNo inner join CustomerInformation on T_SALES_MST.InvoiceNo=CustomerInformation.InvoiceNo inner join Description on T_SALES_DTL.DescriptionID=Description.OID inner join SubCategory on Description.SubCategoryID=SubCategory.OID inner join T_WGPG on SubCategory.CategoryID=T_WGPG.OID inner join PaymentMode on T_SALES_MST.PaymentModeID=PaymentMode.OID " + WhereCondition + " order by T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,Description.Description ";           
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


        public DataTable SPP_GetInvoice(string InvoiceNo)
        {
            DataTable dt = new DataTable();
            cmd = new SqlCommand("SPP_GetInvoice", dbConnect);
            cmd.Parameters.Add("@InvoiceNo", SqlDbType.VarChar, 100).Value = InvoiceNo;
            cmd.CommandType = CommandType.StoredProcedure;
            da = new SqlDataAdapter(cmd);
            da.SelectCommand.CommandTimeout = 300;
            da.Fill(dt);
            return dt;
        }




        public DataTable GetDropInvoiceList(T_PROD entity)
        {
            DataTable dt = new DataTable();
            List<String> myList = new List<String>();
            string WhereCondition = string.Empty;

            if (entity.Branch != String.Empty & entity.Branch != "0")
            {
                myList.Add("T_SALES_MST.StoreID = '" + entity.Branch + "' ");
            }
            if (entity.FromDate != String.Empty & entity.ToDate != string.Empty)
            {
                myList.Add("T_SALES_MST.IDAT between '" + entity.FromDate + "' and '" + entity.ToDate + "' ");
            }
            myList.Add("SalesReturn.Approved=1");
            myList.Add("T_SALES_MST.DropStatus=1");

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


            sql = "select SalesReturn.InvoiceNo,ShopInfo.ShopName as CCOM_NAME,DropStatus='Y',SalesReturn.Reason,CONVERT(VARCHAR(20),T_SALES_MST.IDAT,103)as InvoiceDate,CONVERT(VARCHAR(20),SalesReturn.IDAT,103)as RequestDate,CONVERT(VARCHAR(20),SalesReturn.EDAT,103)as Approveddate from SalesReturn inner join T_SALES_MST on SalesReturn.InvoiceNo=T_SALES_MST.InvoiceNo inner join ShopInfo on SalesReturn.StoreID=ShopInfo.OID " + WhereCondition + " order by ShopInfo.ShopName,T_SALES_MST.IDAT asc";
            
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
