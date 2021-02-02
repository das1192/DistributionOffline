using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.Common;
using System.Collections.Generic;
using TalukderPOS.BusinessObjects;
using TalukderPOS.DAL;

namespace TalukderPOS.DAL
{
    public class SalesReport_DAL
    {
        SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
        SqlDataAdapter da = new SqlDataAdapter();
        SqlCommand cmd;
        String sql;
       
        //SqlDataReader reader;


        public DataTable SalesSummary(SalesReport_BO entity)
        {
            DataTable dt = new DataTable();
            if (entity.DateFrom != string.Empty & entity.DateTo != string.Empty & entity.StoreID != string.Empty)
            {
                cmd = new SqlCommand("SPP_SalesSummary", dbConnect);
                if (entity.DateFrom != string.Empty & entity.DateTo != string.Empty)
                {
                    cmd.Parameters.Add("@StoreID", SqlDbType.VarChar, 100).Value = entity.StoreID;
                    cmd.Parameters.Add("@DateFrom", SqlDbType.Date).Value = DateTime.Parse(entity.DateFrom);
                    cmd.Parameters.Add("@DateTo", SqlDbType.Date).Value = DateTime.Parse(entity.DateTo);
                }
                else
                {
                    cmd.Parameters.Add("@StoreID", SqlDbType.VarChar, 100).Value = entity.StoreID;
                    cmd.Parameters.Add("@DateFrom", SqlDbType.Date).Value = DateTime.Now.Date;
                    cmd.Parameters.Add("@DateTo", SqlDbType.Date).Value = DateTime.Now.Date;
                }
                try
                {
                    if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    da = new SqlDataAdapter(cmd);
                    da.SelectCommand.CommandTimeout = 300;
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
            }
            return dt;
        }


        public DataTable SalesItems(SalesReport_BO entity)
        {
            DataTable dt = new DataTable();
            List<String> myList = new List<String>();
            string WhereCondition = string.Empty;
            if (entity.PROD_WGPG != String.Empty & entity.PROD_WGPG != "0")
            {
                myList.Add("dbo.T_WGPG.OID = '" + entity.PROD_WGPG + "' ");
            }
            if (entity.PROD_SUBCATEGORY != String.Empty & entity.PROD_SUBCATEGORY != "0")
            {
                myList.Add("dbo.SubCategory.OID = '" + entity.PROD_SUBCATEGORY + "' ");
            }
            if (entity.PROD_DES != String.Empty & entity.PROD_DES != "0")
            {
                myList.Add("dbo.Description.OID = '" + entity.PROD_DES + "' ");
            }
            if (entity.StoreID != String.Empty & entity.StoreID != "0")
            {
                myList.Add("dbo.T_SALES_MST.StoreID = '" + entity.StoreID + "' ");
            }
            if (entity.DateFrom != String.Empty & entity.DateTo != string.Empty)
            {
                myList.Add("dbo.T_SALES_MST.IDAT between '" + entity.DateFrom + "' and '" + entity.DateTo + "' ");
            }
            else if (entity.DateFrom != String.Empty & entity.DateTo == string.Empty)
            {
                myList.Add("dbo.T_SALES_MST.IDAT between '" + entity.DateFrom + "' and '" + entity.DateFrom + "' ");
            }
            else if (entity.DateFrom == String.Empty & entity.DateTo != string.Empty)
            {
                myList.Add("dbo.T_SALES_MST.IDAT between '" + entity.DateTo + "' and '" + entity.DateTo + "' ");
            }
            myList.Add("dbo.T_SALES_MST.DropStatus=0");
            myList.Add("dbo.T_SALES_MST.DiscountReferencedBy NOT LIKE '1'");
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
        //    sql = "select dbo.ShopInfo.ShopName as CCOM_NAME, dbo.ShopInfo.CCOM_ADD1,dbo.T_WGPG.WGPG_NAME, dbo.SubCategory.SubCategoryName, dbo.Description.Description, dbo.T_SALES_MST.InvoiceNo, dbo.T_SALES_MST.SubTotal, dbo.T_SALES_MST.Discount, dbo.T_SALES_MST.NetAmount, dbo.PaymentMode.PaymentMode, dbo.T_SALES_DTL.SalePrice, dbo.T_SALES_DTL.SaleQty,ISNULL(dbo.T_SALES_DTL.DiscountAmount,0)as DiscountAmount,dbo.T_SALES_MST.IDAT from dbo.T_SALES_MST inner join dbo.T_SALES_DTL on dbo.T_SALES_DTL.InvoiceNo=dbo.T_SALES_MST.InvoiceNo inner join Description on dbo.T_SALES_DTL.DescriptionID=dbo.Description.OID inner join dbo.SubCategory on dbo.Description.SubCategoryID=dbo.SubCategory.OID inner join dbo.T_WGPG on dbo.SubCategory.CategoryID=dbo.T_WGPG.OID inner join dbo.PaymentMode on dbo.T_SALES_MST.PaymentModeID=dbo.PaymentMode.OID inner join dbo.ShopInfo on dbo.T_SALES_MST.StoreID=dbo.ShopInfo.OID left join dbo.DiscountReference on dbo.DiscountReference.OID = dbo.T_SALES_DTL.DiscountReferenceOID left join dbo.DiscountType on dbo.DiscountReference.DiscountTypeOID = dbo.DiscountType.OID " + WhereCondition + " order by dbo.ShopInfo.ShopName,dbo.T_WGPG.WGPG_NAME,dbo.SubCategory.SubCategoryName,dbo.Description.Description";

            sql = string.Format(@"
select dbo.ShopInfo.ShopName as CCOM_NAME, dbo.ShopInfo.CCOM_ADD1,dbo.T_WGPG.WGPG_NAME, dbo.SubCategory.SubCategoryName
--, dbo.Description.Description
,case when T_SALES_DTL.GiftAmount>0 then Description.Description+' (Gift)'
else Description.Description end as Description

,T_SALES_DTL.Barcode, dbo.T_SALES_MST.InvoiceNo, dbo.T_SALES_MST.SubTotal, dbo.T_SALES_MST.Discount
 ,case when T_SALES_DTL.GiftAmount>0 then 0
else 
dbo.T_SALES_MST.NetAmount end as NetAmount, dbo.PaymentMode.PaymentMode, dbo.T_SALES_DTL.SalePrice, dbo.T_SALES_DTL.SaleQty
,ISNULL(dbo.T_SALES_DTL.DiscountAmount,0)as DiscountAmount,T_SALES_MST.Remarks,dbo.T_SALES_MST.IDAT,Cst.Name 'CustomerName',Cst.Number 'MobileNo' 
from dbo.T_SALES_MST 
inner join dbo.T_SALES_DTL on dbo.T_SALES_DTL.InvoiceNo=dbo.T_SALES_MST.InvoiceNo 
inner join Description on dbo.T_SALES_DTL.DescriptionID=dbo.Description.OID 
inner join dbo.SubCategory on dbo.Description.SubCategoryID=dbo.SubCategory.OID 
inner join dbo.T_WGPG on dbo.SubCategory.CategoryID=dbo.T_WGPG.OID 
inner join dbo.PaymentMode on dbo.T_SALES_MST.PaymentModeID=dbo.PaymentMode.OID 
inner join dbo.ShopInfo on dbo.T_SALES_MST.StoreID=dbo.ShopInfo.OID 
left join Customers Cst on Cst.Id = dbo.T_SALES_MST.RetailerID
left join dbo.DiscountReference on dbo.DiscountReference.OID = dbo.T_SALES_DTL.DiscountReferenceOID 
left join dbo.DiscountType on dbo.DiscountReference.DiscountTypeOID = dbo.DiscountType.OID 
{0} 
order by dbo.ShopInfo.ShopName,dbo.T_WGPG.WGPG_NAME,dbo.SubCategory.SubCategoryName,dbo.Description.Description
", WhereCondition);
            
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
        /// <summary>
        /// edited by sagar////
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DataTable ProductMovement12(T_PROD entity)
        {
            DataTable dt = new DataTable();
            List<String> myList = new List<String>();
            string WhereCondition = string.Empty;

            if (entity.Branch != String.Empty & entity.Branch != "0")
            {
                myList.Add("StockPosting.IUSER = '" + entity.Branch + "' ");
            }


            if (entity.FromDate != String.Empty & entity.ToDate != string.Empty)
            {
                myList.Add("StockPosting.IDAT between '" + entity.FromDate + "' and '" + entity.ToDate + "' ");
            }


            myList.Add("StockPosting.Particulars = 'SES To SIS Transfer' ");



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
            sql = "select T_CCOM.CCOM_NAME,T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,Description.Description,StockPosting.Barcode,StockPosting.Remarks,StockPosting.InwardQty,StockPosting.OutwardQty,StockPosting.Particulars,CONVERT(VARCHAR(10),StockPosting.IDAT,103) AS IDAT from StockPosting inner join Description on StockPosting.DescriptionOID=Description.OID inner join SubCategory on Description.SubCategoryID=SubCategory.OID inner join T_WGPG on SubCategory.CategoryID=T_WGPG.OID inner join T_CCOM on StockPosting.BranchOID=T_CCOM.OID   " + WhereCondition + " order by StockPosting.OID ";
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
       
        public DataTable SalesReport(SalesReport_BO entity)
        {
            DataTable dt = new DataTable();
            List<String> myList = new List<String>();
            string WhereCondition = string.Empty;
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


            string[] myArray = myList.ToArray();
            string where1 = string.Join(" and ", myArray);

            if (where1 == string.Empty)
            {
                WhereCondition = string.Empty;
            }
            else
            {
                WhereCondition = "AND " + where1 + " ";
            }

            if (entity.DateFrom != string.Empty & entity.DateTo != string.Empty & entity.StoreID != string.Empty)
            {
                sql = "select ShopInfo.ShopName as CCOM_NAME,ShopInfo.CCOM_ADD1,PaymentMode.PaymentMode,T_SALES_MST.InvoiceNo,T_SALES_MST.SubTotal,T_SALES_MST.Discount,T_SALES_MST.NetAmount,T_SALES_MST.ReceiveAmount,T_SALES_MST.Remarks,T_SALES_MST.IDAT,T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,Description.Description,T_SALES_DTL.Barcode,T_SALES_DTL.SalePrice,T_SALES_DTL.SaleQty,Customers.Name 'CustomerName','(0'+Cast(Customers.Number as varchar(20))+')' as MobileNumber From T_SALES_MST,PaymentMode,ShopInfo,T_SALES_DTL,T_WGPG,SubCategory,Description,Customers where T_SALES_MST.PaymentModeID=PaymentMode.OID and T_SALES_MST.StoreID=ShopInfo.OID and T_SALES_MST.StoreID='" + entity.StoreID.ToString() + "' and T_SALES_MST.DropStatus='0' and T_SALES_MST.InvoiceNo=T_SALES_DTL.InvoiceNo and T_SALES_DTL.DescriptionID=Description.OID and T_SALES_MST.RetailerID=Customers.ID And Description.SubCategoryID =SubCategory.OID and SubCategory.CategoryID=T_WGPG.OID and dbo.T_SALES_MST.IDAT between '" + Convert.ToDateTime(entity.DateFrom.ToString()) + "' and '" + Convert.ToDateTime(entity.DateTo.ToString()) + "' " + WhereCondition + "  order by T_SALES_MST.InvoiceNo";
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
            }
            return dt;
        }


        public DataTable SalesItemsList(T_PROD entity)
        {
            DataTable dt = new DataTable();
            List<String> myList = new List<String>();
            string WhereCondition = string.Empty;

            if (entity.Branch != String.Empty & entity.Branch != "0")
            {
                myList.Add("T_SALES_MST.StoreID = '" + entity.Branch + "' ");
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
            if (entity.FromDate != String.Empty & entity.ToDate != string.Empty)
            {
                myList.Add("T_SALES_MST.IDAT between '" + entity.FromDate + "' and '" + entity.ToDate + "' ");
            }
            if (entity.Barcode != String.Empty)
            {
                myList.Add("T_SALES_DTL.Barcode = '" + entity.Barcode + "' ");
            }
            myList.Add("T_SALES_MST.DropStatus=0");
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
            sql = "select T_CCOM.CCOM_NAME,T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,Description.Description,T_SALES_DTL.Barcode,T_SALES_DTL.SaleQty,T_SALES_DTL.SalePrice,CAST(T_SALES_DTL.DiscountAmount as  bigint)as DiscountAmount,CAST((T_SALES_DTL.SaleQty * T_SALES_DTL.SalePrice) as  bigint) Total,CONVERT(VARCHAR(10),T_SALES_MST.IDAT,103) AS IDAT from T_SALES_DTL inner join T_SALES_MST on T_SALES_DTL.InvoiceNo=T_SALES_MST.InvoiceNo inner join Description on T_SALES_DTL.DescriptionID=Description.OID inner join SubCategory on Description.SubCategoryID=SubCategory.OID inner join T_WGPG on SubCategory.CategoryID=T_WGPG.OID inner join T_CCOM on T_SALES_MST.StoreID=T_CCOM.OID  " + WhereCondition + " order by T_CCOM.CCOM_NAME,T_WGPG.WGPG_NAME,SubCategory.SubCategoryName";            
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


        public DataTable SalesComboReport(SalesReport_BO entity)
        {
            DataTable dt = new DataTable();
            
            //sql = "select T_CCOM.CCOM_NAME, T_SALES_DTL.InvoiceNo,T_WGPG.WGPG_NAME,SubCategory.SubCategoryName, Description.Description,T_SALES_DTL.Barcode,T_SALES_DTL.SaleQty,T_SALES_MST.IDAT    from T_SALES_DTL,T_CCOM,T_WGPG,SubCategory, Description, T_SALES_MST  where Description.OID = T_SALES_DTL.DescriptionID and Description.SubCategoryID = SubCategory.OID and SubCategory.CategoryID = T_WGPG.OID and T_SALES_MST.InvoiceNo = T_SALES_DTL.InvoiceNo and T_SALES_MST.StoreID = T_CCOM.OID   and T_SALES_MST.StoreID = '" + entity.StoreID + "' and T_SALES_MST.IDAT between '" + entity.DateFrom + "' and '" + entity.DateTo + "' and T_SALES_MST.DropStatus=0  and T_SALES_DTL.InvoiceNo in (select T_SALES_DTL.InvoiceNo from T_SALES_DTL inner join ( select T_WGPG.OID as categoryOID,Description.OID as DescOID,Description.Description, SubCategory.SubCategoryName, T_WGPG.WGPG_NAME from T_WGPG,SubCategory,Description where Description.SubCategoryID = SubCategory.OID and SubCategory.CategoryID = T_WGPG.OID and T_WGPG.OID =111       ) as x on x.DescOID=T_SALES_DTL.DescriptionID group by T_SALES_DTL.InvoiceNo,x.categoryOID having COUNT(x.categoryOID) >= 2) ";                        
            //da = new SqlDataAdapter(sql, dbConnect);
            //try
            //{
            //    if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
            //    da.Fill(dt);
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //finally
            //{
            //    dbConnect.Close();
            //}
            
            da = new SqlDataAdapter();
            cmd = new SqlCommand("SPP_SalesComboReport", dbConnect);
            cmd.Parameters.Add("@StoreID", SqlDbType.VarChar, 100).Value = entity.StoreID;
            cmd.Parameters.Add("@DateFrom", SqlDbType.VarChar, 100).Value = entity.DateFrom;
            cmd.Parameters.Add("@DateTo", SqlDbType.VarChar, 100).Value = entity.DateTo;
            cmd.CommandType = CommandType.StoredProcedure;
            da = new SqlDataAdapter(cmd);
            da.SelectCommand.CommandTimeout = 300;
            da.Fill(dt);
            return dt;
        }

        public DataTable BarphoneComboReport(SalesReport_BO entity)
        {
            DataTable dt = new DataTable();

            da = new SqlDataAdapter();
            cmd = new SqlCommand("SPP_BarphoneComboReport", dbConnect);
            cmd.Parameters.Add("@StoreID", SqlDbType.VarChar, 100).Value = entity.StoreID;
            cmd.Parameters.Add("@DateFrom", SqlDbType.VarChar, 100).Value = entity.DateFrom;
            cmd.Parameters.Add("@DateTo", SqlDbType.VarChar, 100).Value = entity.DateTo;
            cmd.CommandType = CommandType.StoredProcedure;
            da = new SqlDataAdapter(cmd);
            da.SelectCommand.CommandTimeout = 300;
            da.Fill(dt);
            return dt;
        }

        public DataTable DiscountReportByModel(T_PROD entity)
        {
            DataTable dt = new DataTable();
            List<String> myList = new List<String>();
            string WhereCondition = string.Empty;
            
            if (entity.Branch != String.Empty & entity.Branch != "0")
            {
                myList.Add("T_SALES_MST.StoreID = '" + entity.Branch + "' ");
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
            if (entity.DiscountType.ToString() != String.Empty & entity.DiscountType.ToString() != "0")
            {
                myList.Add("DiscountType.OID = " + entity.DiscountType + " ");
            }
            if (entity.Reference.ToString() != String.Empty & entity.Reference.ToString() != "0")
            {
                myList.Add("T_SALES_DTL.DiscountReferenceOID = " + entity.Reference + " ");
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
            myList.Add("dbo.T_SALES_MST.DropStatus=0 and T_SALES_DTL.DiscountAmount >0");
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


            sql = "select T_CCOM.CCOM_NAME,T_SALES_DTL.InvoiceNo,T_WGPG.WGPG_NAME,SubCategory.SubCategoryName, Description.Description, T_SALES_DTL.Barcode,ISNULL(T_SALES_DTL.DiscountAmount,0)as DiscountAmount,DiscountReference.Reference, CONVERT(VARCHAR(10),T_SALES_MST.IDAT,103) AS IDAT from T_SALES_DTL  inner join T_SALES_MST on T_SALES_DTL.InvoiceNo=T_SALES_MST.InvoiceNo  inner join Description on T_SALES_DTL.DescriptionID=Description.OID inner join SubCategory on Description.SubCategoryID=SubCategory.OID  inner join T_WGPG on SubCategory.CategoryID=T_WGPG.OID inner join T_CCOM on T_SALES_MST.StoreID=T_CCOM.OID left join DiscountReference on T_SALES_DTL.DiscountReferenceOID = DiscountReference.OID left join DiscountType on DiscountReference.DiscountTypeOID = DiscountType.OID " + WhereCondition + " order by T_CCOM.CCOM_NAME,T_WGPG.WGPG_NAME, SubCategory.SubCategoryName, Description.Description";
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

        public DataTable SalesStockSummaryReport(T_PROD entity)
        {
            /*
             * calculate from date from noofdate previous.
             */
            int date_count = Convert.ToInt32(entity.NoOfDate) * -1;
            DateTime ToDates = Convert.ToDateTime(entity.ToDate);
            entity.FromDate = ToDates.AddDays(date_count).ToString("yyyy-MM-dd");

            string currentRow = string.Empty;
            bool addRowflag = false;
            DataRow workRow;
            int grant_total;

            DataTable dt = new DataTable();
            dt.Columns.Add("Model", typeof(String));
            for (int i = 0; i > date_count; i--)
            {
                dt.Columns.Add(ToDates.AddDays(i).ToString("yyyy-MM-dd"), typeof(String));
            }
            dt.Columns.Add("Total", typeof(String));
            dt.Columns.Add("Current_Stock", typeof(String));


            sql = "select SubCategory.OID,SubCategory.SubCategoryName from SubCategory inner join T_WGPG on SubCategory.CategoryID=T_WGPG.OID where T_WGPG.OID = " + entity.PROD_WGPG + " and SubCategory.Active=1 order by SubCategory.SubCategoryName";
            DataTable dt1 = getDataTable(sql);

            foreach (DataRow row1 in dt1.Rows)
            {
                workRow = dt.NewRow();
                workRow["Model"] = row1["SubCategoryName"].ToString();
                addRowflag = false;
                grant_total = 0;
                for (int j = 0; j > date_count; j--)
                {
                    currentRow = ToDates.AddDays(j).ToString("yyyy-MM-dd");
                    sql = "select SUM(T_SALES_DTL.SaleQty) as Total from T_SALES_DTL inner join T_SALES_MST on T_SALES_DTL.InvoiceNo=T_SALES_MST.InvoiceNo inner join Description on T_SALES_DTL.DescriptionID=Description.OID inner join SubCategory on Description.SubCategoryID=SubCategory.OID where SubCategory.OID=" + row1["OID"].ToString() + " and T_SALES_MST.StoreID='" + entity.Branch + "' and T_SALES_MST.DropStatus=0 and T_SALES_MST.IDAT between '" + currentRow + "' and '" + currentRow + "' ";
                    DataTable dt2 = getDataTable(sql);
                    
                    string sql1 = "SELECT SUM(Quantity) as CurrentStock  FROM [POSFINAL].[dbo].[StoreMasterStock] where Branch = '" + entity.Branch + "' and PROD_SUBCATEGORY = '" + row1["OID"].ToString() + "' and SaleStatus <> 1 and ActiveStatus = 1 ";                    
                    DataTable dt3 = getDataTable(sql1);
                    if (dt2 != null)
                    {
                        if (dt2.Rows.Count > 0)
                        {
                            if (string.IsNullOrEmpty(dt2.Rows[0]["Total"].ToString()))
                            {
                                workRow[currentRow] = "0";
                            }
                            else
                            {
                                workRow[currentRow] = dt2.Rows[0]["Total"].ToString();
                                grant_total += Convert.ToInt32(dt2.Rows[0]["Total"].ToString());
                                addRowflag = true;
                            }
                        }
                        else
                        {
                            workRow[currentRow] = "0";
                        }
                    }
                    else
                    {
                        workRow[currentRow] = "0";
                    }

                    if (dt3 != null)
                    {
                        if (dt3.Rows.Count > 0)
                        {
                            if (string.IsNullOrEmpty(dt3.Rows[0]["CurrentStock"].ToString()))
                            {
                                workRow["Current_Stock"] = "0";
                            }
                            else
                            {
                                workRow["Current_Stock"] = dt3.Rows[0]["CurrentStock"].ToString();
                            }
                        }
                        else
                        {
                            workRow["Current_Stock"] = "0";
                        }
                    }
                    else
                    {
                        workRow["Current_Stock"] = "0";
                    }
                }
                workRow["Total"] = grant_total.ToString();
                if (addRowflag)
                {
                    dt.Rows.Add(workRow);
                }
            }            
            return dt;
        }


        public DataTable SalesStockSummaryReport1(T_PROD entity)
        {
            /*
             * calculate from date from noofdate previous.
             */
            int date_count = Convert.ToInt32(entity.NoOfDate) * -1;
            DateTime ToDates = Convert.ToDateTime(entity.ToDate);
            entity.FromDate = ToDates.AddDays(date_count).ToString("yyyy-MM-dd");

            string FirstDate = entity.FromDate;
            string LastDate=ToDates.ToString("yyyy-MM-dd");
            DataTable dt = new DataTable();
            DataRow workRow;           
            
            dt.Columns.Add("Branch", typeof(String));
            dt.Columns.Add("Category", typeof(String));
            dt.Columns.Add("Model", typeof(String));
            dt.Columns.Add("Color", typeof(String));
            dt.Columns.Add("MRP", typeof(Int64));
            dt.Columns.Add("SaleQuantity", typeof(Int32));
            dt.Columns.Add("StockQuantity", typeof(Int32));

            dt.Columns.Add("DateFrom", typeof(String));
            dt.Columns.Add("DateTo", typeof(String));

            if (entity.Branch != String.Empty & entity.Branch != "0")
            {                
                sql = "SELECT OID,CCOM_NAME FROM T_CCOM where CCOM_ACTV=1 and OID in('" + entity.Branch + "')";
            }
            else {
                sql = "SELECT OID,CCOM_NAME FROM T_CCOM where CCOM_ACTV=1 and OID not in('CCOMxxxx01','CCOMxxxx04','CCOMxxxx06')";
            }            
            DataTable dt1 = getDataTable(sql);

            sql = "select Description.OID,T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,Description.Description,ISNULL(Description.MRP,0)MRP from Description inner join SubCategory on Description.SubCategoryID=SubCategory.OID inner join T_WGPG on SubCategory.CategoryID=T_WGPG.OID where T_WGPG.OID = " + entity.PROD_WGPG + " and T_WGPG.WGPG_ACTV=1 and SubCategory.Active=1 and Description.Active=1 and SubCategory.RunningModel='Y' order by SubCategory.SubCategoryName ";            
            DataTable dt2 = getDataTable(sql);

            foreach (DataRow row1 in dt1.Rows)
            {
                foreach (DataRow row2 in dt2.Rows)
                {
                    workRow = dt.NewRow();
                    workRow["Branch"] = row1["CCOM_NAME"].ToString();
                    workRow["Category"] = row2["WGPG_NAME"].ToString();
                    workRow["Model"] = row2["SubCategoryName"].ToString();
                    workRow["Color"] = row2["Description"].ToString();
                    workRow["MRP"] = Convert.ToInt64(row2["MRP"].ToString());

                    sql = "select ISNULL(SUM(T_SALES_DTL.SaleQty),0)Total from T_SALES_DTL inner join T_SALES_MST on T_SALES_DTL.InvoiceNo=T_SALES_MST.InvoiceNo inner join Description on T_SALES_DTL.DescriptionID=Description.OID where T_SALES_MST.StoreID='" + row1["OID"].ToString() + "' and Description.OID=" + row2["OID"].ToString() + " and T_SALES_MST.DropStatus=0 and T_SALES_MST.IDAT between '" + FirstDate + "' and '" + LastDate + "' ";
                    workRow["SaleQuantity"] = GetSigleValue(sql);

                    sql = "SELECT ISNULL(SUM(StoreMasterStock.Quantity),0)CurrentStock FROM StoreMasterStock inner join Description on StoreMasterStock.PROD_DES=Description.OID where StoreMasterStock.Branch = '" + row1["OID"].ToString() + "' and StoreMasterStock.PROD_DES = '" + row2["OID"].ToString() + "' and StoreMasterStock.SaleStatus = 0 and StoreMasterStock.ActiveStatus = 1 ";
                    workRow["StockQuantity"] = GetSigleValue(sql);

                    workRow["DateFrom"] = FirstDate;
                    workRow["DateTo"] = LastDate;
                   
                    dt.Rows.Add(workRow);
                }
            }
            
            return dt;
        }


       


        public DataTable RequistionReport(T_PROD entity)
        {
            /*
             * calculate from date from noofdate previous.
             */
            String format = "yyyy-MM-dd";
            int date_count = (Convert.ToInt32(entity.NoOfDate) * -1)+1;
            DateTime ToDates = Convert.ToDateTime(entity.ToDate);
            entity.FromDate = ToDates.AddDays(date_count).ToString(format);

            string date_search = string.Empty;
            string currentRow = string.Empty;
            bool addRowflag = false;
            DataRow workRow;

            if (entity.ToDate != string.Empty)
            {
                date_search = "dbo.T_SALES_MST.IDAT between '" + entity.FromDate + "' and '" + entity.ToDate + "' ";
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("Model", typeof(String));
            dt.Columns.Add("Current_Stock", typeof(String));
            dt.Columns.Add("Sold_Quantity", typeof(String));
            dt.Columns.Add("Requistion_Quantity", typeof(Int32));
            sql = "select SubCategory.OID,SubCategory.SubCategoryName from SubCategory inner join T_WGPG on SubCategory.CategoryID=T_WGPG.OID where T_WGPG.OID = " + entity.PROD_WGPG + " and SubCategory.Active=1 order by SubCategory.SubCategoryName";
            DataTable dt1 = getDataTable(sql);

            foreach (DataRow row1 in dt1.Rows)
            {
                workRow = dt.NewRow();
                workRow["Model"] = row1["SubCategoryName"].ToString();
                addRowflag = false;
                sql = "select SUM(T_SALES_DTL.SaleQty) as Total from T_SALES_DTL inner join T_SALES_MST on T_SALES_DTL.InvoiceNo=T_SALES_MST.InvoiceNo inner join Description on T_SALES_DTL.DescriptionID=Description.OID inner join SubCategory on Description.SubCategoryID=SubCategory.OID where SubCategory.OID=" + row1["OID"].ToString() + " and T_SALES_MST.StoreID='" + entity.Branch + "' and T_SALES_MST.DropStatus=0 and " + date_search + " ";
                string sql1 = "SELECT SUM(Quantity) as CurrentStock  FROM [POSFINAL].[dbo].[StoreMasterStock] where Branch = '" + entity.Branch + "' and PROD_SUBCATEGORY = " + row1["OID"].ToString() + " and SaleStatus <> 1 and ActiveStatus = 1 ";
                DataTable dt2 = getDataTable(sql);
                DataTable dt3 = getDataTable(sql1);
                if (dt3 != null)
                {
                    if (dt3.Rows.Count > 0)
                    {
                        if (string.IsNullOrEmpty(dt3.Rows[0]["CurrentStock"].ToString()))
                        {
                            workRow["Current_Stock"] = "0";
                        }
                        else
                        {
                            workRow["Current_Stock"] = dt3.Rows[0]["CurrentStock"].ToString();
                            addRowflag = true;
                        }
                    }
                    else
                    {
                        workRow["Current_Stock"] = "0";
                    }
                }
                if (dt2 != null)
                {
                    if (dt2.Rows.Count > 0)
                    {
                        if (string.IsNullOrEmpty(dt2.Rows[0]["Total"].ToString()))
                        {
                            workRow["Sold_Quantity"] = "0";
                        }
                        else
                        {
                            workRow["Sold_Quantity"] = dt2.Rows[0]["Total"].ToString();
                            addRowflag = true;
                        }
                    }
                    else
                    {
                        workRow["Sold_Quantity"] = "0";
                    }
                }
                if (addRowflag)
                {
                    workRow["Requistion_Quantity"] = Convert.ToInt32(workRow["Current_Stock"].ToString()) - Convert.ToInt32(workRow["Sold_Quantity"].ToString());
                    dt.Rows.Add(workRow);
                }
            }            
            return dt;
        }


        public DataTable getDataTable(string sql)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            cmd = new SqlCommand(sql, dbConnect);
            da.SelectCommand = cmd;
            cmd.CommandTimeout = 240;
            da.Fill(dt);
            return dt;
        }

        public Int32 GetSigleValue(string sql)
        {   
            //Object returnValue;
            string returnValue;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = dbConnect;
            dbConnect.Open();
            returnValue = cmd.ExecuteScalar().ToString();
            dbConnect.Close();
            return Convert.ToInt32(returnValue);
        }


        public DataTable DailyConsolidedSalesSummary(SalesReport_BO entity)
        {
            DataTable dt = new DataTable();
            List<String> myList = new List<String>();
            string WhereCondition = string.Empty;
            string WhereCondition1 = string.Empty;
            if (entity.StoreID != String.Empty & entity.StoreID != "0")
            {
                myList.Add("T_CCOM.OID = '" + entity.StoreID + "' ");
                WhereCondition1 = "T_CCOM.OID = '" + entity.StoreID + "' and ";
            }
            if (entity.DateFrom != String.Empty & entity.DateTo != string.Empty)
            {
                myList.Add("T_SALES_MST.IDAT between '" + entity.DateFrom + "' and '" + entity.DateTo + "' ");
            }
            else if (entity.DateFrom != String.Empty & entity.DateTo == string.Empty)
            {
                myList.Add("T_SALES_MST.IDAT between '" + entity.DateFrom + "' and '" + entity.DateFrom + "' ");
            }
            else if (entity.DateFrom == String.Empty & entity.DateTo != string.Empty)
            {
                myList.Add("T_SALES_MST.IDAT between '" + entity.DateTo + "' and '" + entity.DateTo + "' ");
            }
            myList.Add("T_SALES_MST.DropStatus=0");
            myList.Add("T_CCOM.CCOM_ACTV = 1");            
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
            sql = "select T_CCOM.CCOM_NAME,ISNULL(A.WGPG_NAME,'')as WGPG_NAME,ISNULL(A.SalesAmount,0)as SalesAmount,ISNULL(A.DiscountAmount,0)as DiscountAmount,A.IDAT from T_CCOM left join(select T_CCOM.CCOM_NAME,T_WGPG.WGPG_NAME,ISNULL(SUM(T_SALES_DTL.SalePrice*T_SALES_DTL.SaleQty),0)as SalesAmount,ISNULL(SUM(T_SALES_DTL.DiscountAmount),0)as DiscountAmount,T_SALES_MST.IDAT from T_CCOM left join T_SALES_MST on T_SALES_MST.StoreID=T_CCOM.OID left join T_SALES_DTL on T_SALES_MST.InvoiceNo=T_SALES_DTL.InvoiceNo left join Description on T_SALES_DTL.DescriptionID=Description.OID left join SubCategory on Description.SubCategoryID=SubCategory.OID left join T_WGPG on SubCategory.CategoryID=T_WGPG.OID " + WhereCondition + " group by T_CCOM.CCOM_NAME,T_WGPG.WGPG_NAME,T_SALES_MST.IDAT)A on A.CCOM_NAME=T_CCOM.CCOM_NAME where " + WhereCondition1 + " T_CCOM.CCOM_ACTV=1 and T_CCOM.OID <>'CCOMxxxx01' order by T_CCOM.CCOM_NAME";
            //sql = "select T_CCOM.CCOM_NAME,ISNULL(A.WGPG_NAME,'')as WGPG_NAME,ISNULL(A.SalesAmount,0)as SalesAmount,ISNULL(A.DiscountAmount,0)as DiscountAmount,A.IDAT from T_CCOM left join(select T_CCOM.CCOM_NAME,T_WGPG.WGPG_NAME,ISNULL(SUM(T_SALES_DTL.SalePrice*T_SALES_DTL.SaleQty),0)as SalesAmount,ISNULL(SUM(T_SALES_DTL.DiscountAmount),0)as DiscountAmount,T_SALES_MST.IDAT from T_CCOM left join T_SALES_MST on T_SALES_MST.StoreID=T_CCOM.OID left join T_SALES_DTL on T_SALES_MST.InvoiceNo=T_SALES_DTL.InvoiceNo left join Description on T_SALES_DTL.DescriptionID=Description.OID left join SubCategory on Description.SubCategoryID=SubCategory.OID left join T_WGPG on SubCategory.CategoryID=T_WGPG.OID " + WhereCondition + " group by T_CCOM.CCOM_NAME,T_WGPG.WGPG_NAME,T_SALES_MST.IDAT)A on A.CCOM_NAME=T_CCOM.CCOM_NAME " + WhereCondition + "  order by T_CCOM.CCOM_NAME";
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



        public DataTable ProductMovement(T_PROD entity)
        {
            DataTable dt = new DataTable();
            List<String> myList = new List<String>();
            string WhereCondition = string.Empty;

            if (entity.Branch != String.Empty & entity.Branch != "0")
            {
                myList.Add("StockPosting.BranchOID = '" + entity.Branch + "' ");
            }
            if (entity.PROD_WGPG.ToString() != String.Empty & entity.PROD_WGPG.ToString() != "0")
            {
                myList.Add("T_WGPG.OID = " + entity.PROD_WGPG + " ");
            }
            if (entity.PROD_SUBCATEGORY.ToString() != String.Empty & entity.PROD_SUBCATEGORY.ToString() != "0")
            {
                myList.Add("SubCategory.OID = " + entity.PROD_SUBCATEGORY + " ");
            }           
            if (entity.FromDate != String.Empty & entity.ToDate != string.Empty)
            {
                myList.Add("StockPosting.IDAT between '" + entity.FromDate + "' and '" + entity.ToDate + "' ");
            }
            if (entity.Barcode != String.Empty)
            {
                myList.Add("StockPosting.Barcode = '" + entity.Barcode + "' ");
            }
            if (entity.SearchOption != String.Empty)
            {
                myList.Add("StockPosting.Particulars = '" + entity.SearchOption + "' ");
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
            sql = "select T_CCOM.CCOM_NAME,T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,Description.Description,StockPosting.Barcode,StockPosting.InwardQty,StockPosting.OutwardQty,StockPosting.Particulars,CONVERT(VARCHAR(10),StockPosting.IDAT,103) AS IDAT from StockPosting inner join Description on StockPosting.DescriptionOID=Description.OID inner join SubCategory on Description.SubCategoryID=SubCategory.OID inner join T_WGPG on SubCategory.CategoryID=T_WGPG.OID inner join T_CCOM on StockPosting.BranchOID=T_CCOM.OID   " + WhereCondition + " order by StockPosting.OID ";            
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

        public DataTable DOAList(T_PROD entity)
        {
            DataTable dt = new DataTable();
            List<String> myList = new List<String>();
            string WhereCondition = string.Empty;

            if (entity.Branch != String.Empty & entity.Branch != "0")
            {
                myList.Add("T_CCOM.OID = '" + entity.Branch + "' ");
            }
            if (entity.PROD_WGPG.ToString() != String.Empty & entity.PROD_WGPG.ToString() != "0")
            {
                myList.Add("T_WGPG.OID = " + entity.PROD_WGPG + " ");
            }
            if (entity.PROD_SUBCATEGORY.ToString() != String.Empty & entity.PROD_SUBCATEGORY.ToString() != "0")
            {
                myList.Add("SubCategory.OID = " + entity.PROD_SUBCATEGORY + " ");
            }
            if (entity.FromDate != String.Empty & entity.ToDate != string.Empty)
            {
                myList.Add("DOA.IDAT between '" + entity.FromDate + "' and '" + entity.ToDate + "' ");
            }
            if (entity.Barcode != String.Empty)
            {
                myList.Add("DOA.Barcode = '" + entity.Barcode + "' ");
            }
            myList.Add("DOA.Status = '" + entity.ActiveStatus + "' ");

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
            sql = "select DOA.OID,T_CCOM.CCOM_NAME,DOA.BranchOID,T_WGPG.OID as CategoryOID,SubCategory.OID as SubcategoryOID,DOA.DescriptionOID,T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,Description.Description,DOA.Barcode,DOA.Quantity,DOA.Remarks,CONVERT(VARCHAR(10),DOA.IDAT,103) AS IDAT from DOA inner join Description on DOA.DescriptionOID=Description.OID inner join SubCategory on Description.SubCategoryID=SubCategory.OID inner join T_WGPG on SubCategory.CategoryID=T_WGPG.OID inner join T_CCOM on DOA.BranchOID=T_CCOM.OID " + WhereCondition + " order by DOA.OID ASC ";            
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
