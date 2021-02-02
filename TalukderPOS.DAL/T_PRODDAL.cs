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
    public class T_PRODDAL
    {
        SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
        SqlCommand cmd;
        SqlCommand cmd12;
        SqlDataAdapter da = new SqlDataAdapter();
        String sql, sqlg;
        int lid;
        SqlDataReader reader;
        string total11;
        string total12;
        decimal total13;
        decimal total14;
        decimal total15;
        decimal total16;
        string totq11;
        string totq12;
        decimal totq13;
        decimal totq14;
        decimal totq15;
        decimal totq16;

        public DataTable StoreMasterStockSearch(T_PROD entity)
        {
            DataTable dt = new DataTable();
            List<String> myList = new List<String>();
            string WhereCondition = string.Empty;

            if (entity.Branch != String.Empty & entity.Branch != "0")
            {
                myList.Add("StoreMasterStock.Branch = '" + entity.Branch + "' ");
            }
            if (entity.PROD_WGPG.ToString() != String.Empty & entity.PROD_WGPG.ToString() != "0")
            {
                myList.Add("StoreMasterStock.PROD_WGPG = " + entity.PROD_WGPG + " ");
            }
            if (entity.PROD_SUBCATEGORY.ToString() != String.Empty & entity.PROD_SUBCATEGORY.ToString() != "0")
            {
                myList.Add("StoreMasterStock.PROD_SUBCATEGORY = " + entity.PROD_SUBCATEGORY + " ");
            }
            if (entity.PROD_DES.ToString() != String.Empty & entity.PROD_DES.ToString() != "0")
            {
                myList.Add("StoreMasterStock.PROD_DES = " + entity.PROD_DES + " ");
            }

            if (entity.FromDate != String.Empty & entity.ToDate != String.Empty)
            {
                myList.Add("StoreMasterStock.IDAT between '" + entity.FromDate + "' and '" + entity.ToDate + "' ");
            }
            myList.Add("StoreMasterStock.ActiveStatus=1");
            myList.Add("StoreMasterStock.Quantity >0");
            myList.Add("StoreMasterStock.SaleQuantity=0");

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

            if (entity.SearchType == "Details")
            {
                sql = "select StoreMasterStock.OID,StoreMasterStock.Branch,ShopInfo.ShopName,T_WGPG.WGPG_NAME as PROD_WGPG,SubCategory.SubCategoryName as PROD_SUBCATEGORY,Description.Description,StoreMasterStock.Barcode,StoreMasterStock.CostPrice,StoreMasterStock.SalePrice,StoreMasterStock.Quantity,CONVERT(VARCHAR(10),StoreMasterStock.IDAT,103) AS IDAT,DATEDIFF(DAY, StoreMasterStock.IDAT, GETDATE()) AS Ageing  from StoreMasterStock left join Description on StoreMasterStock.PROD_DES=Description.OID left join SubCategory on Description.SubCategoryID=SubCategory.OID left join T_WGPG on SubCategory.CategoryID=T_WGPG.OID left join ShopInfo on StoreMasterStock.Branch=ShopInfo.OID " + WhereCondition + "  order by StoreMasterStock.Branch,T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,Description.Description,DATEDIFF(DAY, StoreMasterStock.IDAT, GETDATE()) desc";
            }
            else
            {
                sql = "select ShopInfo.ShopName,T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,Description.Description,SUM(StoreMasterStock.Quantity) as Qty from StoreMasterStock inner join Description on StoreMasterStock.PROD_DES=Description.OID inner join SubCategory on Description.SubCategoryID=SubCategory.OID inner join T_WGPG on SubCategory.CategoryID=T_WGPG.OID inner join ShopInfo on StoreMasterStock.Branch=ShopInfo.OID " + WhereCondition + " Group by ShopInfo.ShopName,T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,Description.Description order by ShopInfo.ShopName,T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,Description.Description";
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
        public string getquantityforPurchaseReturn(string description, int Vendor_ID)
        {
            string TOTALAVAILQUANTITY = string.Empty;
            sql = string.Format(@"

declare @totTOTALAVAILQUANTITY int =0;
declare @totAvailable4Barcode int =0;
declare @totalFinal int =0;

set @totTOTALAVAILQUANTITY = (select TOTALAVAILQUANTITY from STOCKAVERAGEVALUE where PROD_DES={0} )

set @totAvailable4Barcode =(
select COUNT(*) from (
-------
select p.Barcode,d.Description , p.Branch, p.SaleStatus,q.Vendor_ID ,v.Vendor_Name 

from T_PROD p 
inner join Description d on d.OID=p.PROD_DES
inner join SubCategory sc on sc.OID=d.SubCategoryID
inner join T_WGPG c on c.OID=sc.CategoryID
inner join StoreMasterStock s 
on s.PROD_DES=p.PROD_DES and s.PROD_SUBCATEGORY=p.PROD_SUBCATEGORY and s.PROD_WGPG=p.PROD_WGPG and s.Branch=p.Branch
inner join T_STOCK q on q.Branch = p.Branch AND q.PROD_DES =p.PROD_DES and q.Barcode =p.Barcode 
inner join Vendor v on v.OID =q.Vendor_ID    

where ISNULL(p.Barcode,'')!=''  and p.IDAT>'26 sep 2017' and p.SaleStatus = 0 
and p.PROD_DES = {0}  and q.Vendor_ID = {1}  
-----
)s
)

set @totalFinal=@totTOTALAVAILQUANTITY-@totAvailable4Barcode

select @totalFinal as TOTALAVAILQUANTITY
", description, Vendor_ID);
            //select TOTALAVAILQUANTITY from STOCKAVERAGEVALUE where PROD_DES='" + description + "' ";

            cmd = new SqlCommand(sql, dbConnect);
            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    TOTALAVAILQUANTITY = reader["TOTALAVAILQUANTITY"].ToString();
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
            return TOTALAVAILQUANTITY;
        }
        public string getquantity(string description)
        {
            string TOTALAVAILQUANTITY = string.Empty;
            sql = "select TOTALAVAILQUANTITY from STOCKAVERAGEVALUE where PROD_DES='" + description + "' ";

            cmd = new SqlCommand(sql, dbConnect);
            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    TOTALAVAILQUANTITY = reader["TOTALAVAILQUANTITY"].ToString();
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
            return TOTALAVAILQUANTITY;
        }
        public DataTable ProductHistorySearch(T_PROD entity)
        {
            DataTable dt = new DataTable();
            List<String> myList = new List<String>();
            string WhereCondition = string.Empty;

            if (entity.Branch != String.Empty & entity.Branch != "0")
            {
                myList.Add("  ps.Branch = '" + entity.Branch + "' ");
            }
            if (entity.PROD_WGPG.ToString() != String.Empty & entity.PROD_WGPG.ToString() != "0")
            {
                myList.Add("  ps.PROD_WGPG = " + entity.PROD_WGPG + " ");
            }
            if (entity.PROD_SUBCATEGORY.ToString() != String.Empty & entity.PROD_SUBCATEGORY.ToString() != "0")
            {
                myList.Add("  ps.PROD_SUBCATEGORY = " + entity.PROD_SUBCATEGORY + " ");
            }
            if (entity.PROD_DES.ToString() != String.Empty & entity.PROD_DES.ToString() != "0")
            {
                myList.Add("  ps.PROD_DES = " + entity.PROD_DES + " ");
            }

            if (entity.FromDate != String.Empty & entity.ToDate != String.Empty)
            {
                myList.Add("  ps.IDAT between '" + entity.FromDate + "' and '" + entity.ToDate + "' ");
            }
            if (entity.SearchOption != String.Empty)
            {
                if (entity.SearchOption == "Purchase Return")
                {
                    myList.Add("  ps.SaleStatus = '2' ");
                }
                else
                {
                   // myList.Add("  ps.Particular = '" + entity.SearchOption + "' ");

                    myList.Add("  ps.SaleStatus in ('0','1') ");
                }
            }



            string[] myArray = myList.ToArray();
            string where = string.Join(" and ", myArray);

            if (where == string.Empty)
            {
                WhereCondition = string.Empty;
            }
            else
            {
                WhereCondition = " and " + where + " ";
            }

           // sql = "select Purchase_Report.OID,Purchase_Report.Branch,Purchase_Report.Particular,Vendor.Vendor_Name,ShopInfo.ShopName,T_WGPG.WGPG_NAME as PROD_WGPG,SubCategory.SubCategoryName as PROD_SUBCATEGORY,Description.Description,Purchase_Report.CostPrice,Purchase_Report.Quantity,CONVERT(VARCHAR(10),Purchase_Report.IDAT,103) AS IDAT,DATEDIFF(DAY, Purchase_Report.IDAT, GETDATE()) AS Ageing  from Purchase_Report left join Vendor on Purchase_Report.Vendor_ID=Vendor.OID left join Description on Purchase_Report.PROD_DES=Description.OID left join SubCategory on Description.SubCategoryID=SubCategory.OID left join T_WGPG on SubCategory.CategoryID=T_WGPG.OID left join ShopInfo on Purchase_Report.Branch=ShopInfo.OID " + WhereCondition + "  order by Purchase_Report.Branch,T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,Description.Description, Purchase_Report.OID desc";

            if (entity.SearchOption == "Purchase Return")
            {
                sql = string.Format(@"
select top 4000 ps.OID,ps.Branch,Particular='Purchase Return',v.Vendor_Name,shp.ShopName
,PROD_WGPG=c.WGPG_NAME,PROD_SUBCATEGORY=sc.SubCategoryName,Description=d.Description +':'+ ps.Barcode,ps.Barcode
,CostPrice=d.SESPrice,ps.Quantity,IDAT=CONVERT(VARCHAR(10),ps.IDAT,103),Ageing=DATEDIFF(DAY, ps.IDAT, GETDATE()), ps.Po_Number 

from T_STOCK ps 
inner join Vendor v on v.OID=ps.Vendor_ID
inner join ShopInfo shp on shp.OID=ps.Branch
inner join Description d on d.OID=ps.PROD_DES
inner join SubCategory sc on sc.OID=ps.PROD_SUBCATEGORY
inner join T_WGPG c on c.OID=ps.PROD_WGPG and ps.Branch=c.Shop_id
where ps.Quantity=1  and ISNULL(ps.Barcode,'')<>''  {0} 

order by ps.Branch,c.WGPG_NAME,sc.SubCategoryName,d.Description, ps.OID desc
", WhereCondition);
            }
            else if (entity.SearchOption == "Purchase")
            {
                                sql = string.Format(@"
select top 4000 ps.OID,ps.Branch,Particular='Purchase',v.Vendor_Name,shp.ShopName
,PROD_WGPG=c.WGPG_NAME,PROD_SUBCATEGORY=sc.SubCategoryName,Description=d.Description +':'+ ps.Barcode,ps.Barcode
,CostPrice=d.SESPrice,ps.Quantity,IDAT=CONVERT(VARCHAR(10),ps.IDAT,103),Ageing=DATEDIFF(DAY, ps.IDAT, GETDATE()), ps.Po_Number 

from T_STOCK ps 
inner join Vendor v on v.OID=ps.Vendor_ID
inner join ShopInfo shp on shp.OID=ps.Branch
inner join Description d on d.OID=ps.PROD_DES
inner join SubCategory sc on sc.OID=ps.PROD_SUBCATEGORY
inner join T_WGPG c on c.OID=ps.PROD_WGPG and ps.Branch=c.Shop_id
where ps.Quantity=1 and ISNULL(ps.Barcode,'')<>''  {0} 

order by ps.Branch,c.WGPG_NAME,sc.SubCategoryName,d.Description, ps.OID desc
", WhereCondition);
                
                
                
                
                
                
                }
            else
            {
                sql = string.Format(@"
select top 4000 ps.OID,ps.Branch,ps.Particular,v.Vendor_Name,shp.ShopName
,c.WGPG_NAME as PROD_WGPG,sc.SubCategoryName as PROD_SUBCATEGORY,d.Description
,ps.CostPrice,ps.Quantity,CONVERT(VARCHAR(10),ps.IDAT,103) AS IDAT
,DATEDIFF(DAY, ps.IDAT, GETDATE()) AS Ageing, Po_Number='' 

from Purchase_Report ps
left join Vendor v on ps.Vendor_ID=v.OID 
left join Description d on ps.PROD_DES=d.OID 
left join SubCategory sc on d.SubCategoryID=sc.OID 
left join T_WGPG c on sc.CategoryID=c.OID 
left join ShopInfo shp on ps.Branch=shp.OID 

where 1=1 {0} 

order by ps.Branch,c.WGPG_NAME,sc.SubCategoryName,d.Description, ps.OID desc
", WhereCondition);
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
        public DataTable VendorStockSearch(T_PROD entity)
        {
            DataTable dt = new DataTable();
            List<String> myList = new List<String>();
            string WhereCondition = string.Empty;

            if (entity.Branch != String.Empty & entity.Branch != "0")
            {
                myList.Add("T_STOCK.Branch = '" + entity.Branch + "' ");
            }
            if (entity.PROD_WGPG.ToString() != String.Empty & entity.PROD_WGPG.ToString() != "0")
            {
                myList.Add("T_STOCK.PROD_WGPG = " + entity.PROD_WGPG + " ");
            }
            if (entity.PROD_SUBCATEGORY.ToString() != String.Empty & entity.PROD_SUBCATEGORY.ToString() != "0")
            {
                myList.Add("T_STOCK.PROD_SUBCATEGORY = " + entity.PROD_SUBCATEGORY + " ");
            }
            if (entity.PROD_DES.ToString() != String.Empty & entity.PROD_DES.ToString() != "0")
            {
                myList.Add("T_STOCK.PROD_DES = " + entity.PROD_DES + " ");
            }

            if (entity.FromDate != String.Empty & entity.ToDate != String.Empty)
            {
                myList.Add("T_STOCK.IDAT between '" + entity.FromDate + "' and '" + entity.ToDate + "' ");
            }

            if (entity.Vendor_ID != String.Empty & entity.Vendor_ID != String.Empty)
            {
                myList.Add("T_STOCK.Vendor_ID = " + entity.Vendor_ID + " ");
            }

            myList.Add("T_STOCK.Quantity >0");


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

            if (entity.SearchType == "Details")
            {
                sql = "select T_STOCK.OID,T_STOCK.Branch,Vendor.Vendor_Name,ShopInfo.ShopName,T_WGPG.WGPG_NAME as PROD_WGPG,SubCategory.SubCategoryName as PROD_SUBCATEGORY,Description.Description,T_STOCK.CostPrice,T_STOCK.Barcode,T_STOCK.Quantity,CAST((T_STOCK.CostPrice * T_STOCK.Quantity) AS INT) as grandtotal,CONVERT(VARCHAR(10),T_STOCK.IDAT,103) AS IDAT,DATEDIFF(DAY, T_STOCK.IDAT, GETDATE()) AS Ageing  from T_STOCK left join Vendor on T_STOCK.Vendor_ID=Vendor.OID left join Description on T_STOCK.PROD_DES=Description.OID left join SubCategory on Description.SubCategoryID=SubCategory.OID left join T_WGPG on SubCategory.CategoryID=T_WGPG.OID left join ShopInfo on T_STOCK.Branch=ShopInfo.OID " + WhereCondition + "  order by T_STOCK.OID desc";
            }
            else
            {
                sql = "select ShopInfo.ShopName,T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,Description.Description,SUM(StoreMasterStock.Quantity) as Qty from StoreMasterStock inner join Description on StoreMasterStock.PROD_DES=Description.OID inner join SubCategory on Description.SubCategoryID=SubCategory.OID inner join T_WGPG on SubCategory.CategoryID=T_WGPG.OID inner join ShopInfo on StoreMasterStock.Branch=ShopInfo.OID " + WhereCondition + " Group by ShopInfo.ShopName,T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,Description.Description order by ShopInfo.ShopName,T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,Description.Description";
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
        public DataTable VendorDueReport(T_PROD entity)
        {
            DataTable dt = new DataTable();
            List<String> myList = new List<String>();
            string WhereCondition = string.Empty;

            if (entity.Vendor_ID != String.Empty & entity.Vendor_ID != "0")
            {
                myList.Add("Vendor_Incoming_View.Vendor_ID = '" + entity.Vendor_ID + "' ");
            }

            myList.Add("Vendor_Incoming_View.Branch = '" + entity.Branch + "' ");

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


            sql = "SELECT Vendor_Incoming_View.incomingsum1,isnull(Vendor_Outgoing_View.sum2,'0') as Outgoingsum,isnull(PurchaseReturnView.ReturnSum,'0') as returnsum,isnull(PurchasePrice_AmendmentView.sumamendment,'0') as sumamendment,(isnull(Vendor_Incoming_View.incomingsum1,'0')-isnull(Vendor_Outgoing_View.sum2,'0')-isnull(PurchaseReturnView.ReturnSum,'0')-isnull(PurchasePrice_AmendmentView.sumamendment,'0')) as Balance,Vendor.Vendor_Name, Vendor_Incoming_View.Branch FROM Vendor_Incoming_View inner join Vendor on Vendor_Incoming_View.Vendor_ID = Vendor.OID LEFT OUTER JOIN Vendor_Outgoing_View ON Vendor_Incoming_View.Vendor_ID = Vendor_Outgoing_View.Vendor_ID LEFT OUTER JOIN PurchaseReturnView ON Vendor_Incoming_View.Vendor_ID = PurchaseReturnView.Vendor_ID LEFT OUTER JOIN PurchasePrice_AmendmentView ON Vendor_Incoming_View.Vendor_ID = PurchasePrice_AmendmentView.Vendor_ID " + WhereCondition + "GROUP BY Vendor.Vendor_Name,Vendor_Incoming_View.Branch,Vendor_Incoming_View.incomingsum1,Vendor_Outgoing_View.sum2,PurchaseReturnView.ReturnSum,PurchasePrice_AmendmentView.sumamendment";

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
        public void Add(T_PROD entity)
        {


            cmd = new SqlCommand("SPP_StoreMasterStockAdd", dbConnect);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@OID", SqlDbType.VarChar, 50).Value = entity.OID;
            cmd.Parameters.Add("@PROD_WGPG", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_WGPG);
            cmd.Parameters.Add("@PROD_SUBCATEGORY", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_SUBCATEGORY);
            cmd.Parameters.Add("@PROD_DES", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_DES);
            cmd.Parameters.Add("@Vendor_ID", SqlDbType.BigInt).Value = Convert.ToInt64(entity.Vendor_ID);
            cmd.Parameters.Add("@Branch", SqlDbType.VarChar, 50).Value = entity.Branch;
            cmd.Parameters.Add("@Barcode", SqlDbType.VarChar, 50).Value = entity.Barcode;
            cmd.Parameters.Add("@CostPrice", SqlDbType.BigInt).Value = Convert.ToInt64(entity.CostPrice);
            cmd.Parameters.Add("@SalePrice", SqlDbType.BigInt).Value = Convert.ToInt64(entity.SalePrice);
            cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = Convert.ToInt32(entity.Quantity);
            cmd.Parameters.Add("@SaleStatus", SqlDbType.Int).Value = 0;
            cmd.Parameters.Add("@ActiveStatus", SqlDbType.Int).Value = 1;
            cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 50).Value = entity.IUSER;
            cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = entity.IDAT;
            //cmd.Parameters.Add("@CashTrans", SqlDbType.VarChar, 50).Value = entity.CashTrans;
            cmd.Parameters.Add("@EDAT", SqlDbType.Date).Value = entity.EDAT;
            cmd.Parameters.Add("@PONUMBER", SqlDbType.VarChar, 100).Value = entity.PONUMBER;
            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                cmd.ExecuteNonQuery();
                //MonitorStoreMasterStockForAdd(entity);
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

        public void Add_Barcode(T_PROD entity)
        {

            sql = "INSERT INTO T_PROD(PROD_WGPG,PROD_SUBCATEGORY,PROD_DES,Branch,Barcode,CostPrice,SalePrice,Quantity,SaleStatus,IUSER,IDAT) VALUES(@PROD_WGPG,@PROD_SUBCATEGORY,@PROD_DES,@Branch,@Barcode,@CostPrice,@SalePrice,@Quantity,@SaleStatus,@IUSER,@IDAT)";
            cmd = new SqlCommand(sql, dbConnect);


            cmd.Parameters.Add("@PROD_WGPG", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_WGPG);
            cmd.Parameters.Add("@PROD_SUBCATEGORY", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_SUBCATEGORY);
            cmd.Parameters.Add("@PROD_DES", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_DES);
            cmd.Parameters.Add("@Branch", SqlDbType.VarChar, 50).Value = entity.Branch;
            cmd.Parameters.Add("@Barcode", SqlDbType.VarChar, 50).Value = entity.Barcode;
            cmd.Parameters.Add("@CostPrice", SqlDbType.BigInt).Value = Convert.ToInt64(entity.CostPrice);
            cmd.Parameters.Add("@SalePrice", SqlDbType.BigInt).Value = Convert.ToInt64(entity.SalePrice);
            cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = Convert.ToInt32(entity.Quantity);
            cmd.Parameters.Add("@SaleStatus", SqlDbType.Int).Value = 0;
            cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 50).Value = entity.IUSER;
            cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = entity.IDAT; // DateTime.Now.Date;


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

            sql = "INSERT INTO StockPosting(BranchOID,DescriptionOID,Barcode,InwardQty,OutwardQty,Particulars,IUSER,IDAT) VALUES(@Branch,@PROD_DES,@Barcode,@Quantity,@OutwardQty,@Particulars,@IUSER,@IDAT)";
            cmd = new SqlCommand(sql, dbConnect);


            cmd.Parameters.Add("@Branch", SqlDbType.VarChar, 50).Value = entity.Branch;
            cmd.Parameters.Add("@PROD_DES", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_DES);
            cmd.Parameters.Add("@Barcode", SqlDbType.VarChar, 50).Value = entity.Barcode;
            cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = Convert.ToInt32(entity.Quantity);
            cmd.Parameters.Add("@OutwardQty", SqlDbType.Int).Value = 0;
            cmd.Parameters.Add("@Particulars", SqlDbType.VarChar, 50).Value = "Stock In";
            cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 50).Value = entity.IUSER;
            cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = entity.IDAT; //DateTime.Now.Date;


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

        // Added By Yeasin 11-May-2019

        public void AddOrEditPurchase(T_PROD entity)
        {
            cmd = new SqlCommand("SPP_AddOrEditPurchase", dbConnect);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@OID", SqlDbType.VarChar, 50).Value = entity.OID;
            cmd.Parameters.Add("@PROD_WGPG", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_WGPG);
            cmd.Parameters.Add("@PROD_SUBCATEGORY", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_SUBCATEGORY);
            cmd.Parameters.Add("@PROD_DES", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_DES);
            cmd.Parameters.Add("@Vendor_ID", SqlDbType.BigInt).Value = Convert.ToInt64(entity.Vendor_ID);
            cmd.Parameters.Add("@Branch", SqlDbType.VarChar, 50).Value = entity.Branch;
            cmd.Parameters.Add("@Barcode", SqlDbType.VarChar, 50).Value = entity.Barcode;
            cmd.Parameters.Add("@CostPrice", SqlDbType.BigInt).Value = Convert.ToInt64(entity.CostPrice);
            cmd.Parameters.Add("@SalePrice", SqlDbType.BigInt).Value = Convert.ToInt64(entity.SalePrice);
            cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = Convert.ToInt32(entity.Quantity);
            cmd.Parameters.Add("@SaleStatus", SqlDbType.Int).Value = 0;
            cmd.Parameters.Add("@ActiveStatus", SqlDbType.Int).Value = 1;
            cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 50).Value = entity.IUSER;
            cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = entity.IDAT;
            //cmd.Parameters.Add("@CashTrans", SqlDbType.VarChar, 50).Value = entity.CashTrans;
            cmd.Parameters.Add("@EDAT", SqlDbType.Date).Value = entity.EDAT;
            cmd.Parameters.Add("@PONUMBER", SqlDbType.VarChar, 100).Value = entity.PONUMBER;
            cmd.Parameters.Add("@Flag", SqlDbType.VarChar, 50).Value = entity.Flag;
            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                cmd.ExecuteNonQuery();
                //MonitorStoreMasterStockForAdd(entity);
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

        public void PurchaseReturn(T_PROD entity)
        {
            int totalpurret = 0;
            string prnumber = "PR-" + entity.Branch + DateTime.Now.ToString("yyMMddHHmmss") + " ";
            totalpurret = Convert.ToInt32(entity.Quantity) * Convert.ToInt32(entity.CostPrice);
            int totalamountdebit = Convert.ToInt32(entity.Quantity) * Convert.ToInt32(entity.CostPrice);

            cmd = new SqlCommand("SPP_PurchaseReturn_Quantity", dbConnect);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@PROD_WGPG", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_WGPG);
            cmd.Parameters.Add("@PROD_SUBCATEGORY", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_SUBCATEGORY);
            cmd.Parameters.Add("@PROD_DES", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_DES);
            cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = Convert.ToInt32(entity.Quantity);
            cmd.Parameters.Add("@Branch", SqlDbType.VarChar, 50).Value = entity.Branch;
            cmd.Parameters.Add("@CostPrice", SqlDbType.VarChar, 50).Value = entity.CostPrice;
            cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 50).Value = entity.IUSER;
            cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = DateTime.Now.Date;
            cmd.Parameters.Add("@Vendor_ID", SqlDbType.Int).Value = Convert.ToInt32(entity.Vendor_ID);
            cmd.Parameters.Add("@prnumber", SqlDbType.VarChar, 100).Value = prnumber;
            cmd.Parameters.Add("@Total", SqlDbType.BigInt).Value = totalpurret;
            cmd.Parameters.Add("@IDATTIME", SqlDbType.DateTime).Value = DateTime.Now;
            cmd.Parameters.Add("@ACC_STOCKID", SqlDbType.BigInt).Value = Convert.ToInt64(entity.ACC_STOCKID);
            cmd.Parameters.Add("@Debit", SqlDbType.BigInt).Value = totalamountdebit;
            cmd.Parameters.Add("@Credit", SqlDbType.BigInt).Value = totalamountdebit;
            cmd.Parameters.Add("@StockID", SqlDbType.BigInt).Value = Convert.ToInt64(entity.OID);

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

        //--------------------------//

        public void UpdatePurchaseReturn(T_PROD entity)
        {


            sql = "INSERT INTO Purchase_Return(PROD_WGPG,PROD_SUBCATEGORY,PROD_DES,Branch,CostPrice,Quantity,IUSER,IDAT,Vendor_ID) VALUES(@PROD_WGPG,@PROD_SUBCATEGORY,@PROD_DES,@Branch,@CostPrice,@Quantity,@IUSER,@IDAT,@Vendor_ID)";
            cmd = new SqlCommand(sql, dbConnect);


            cmd.Parameters.Add("@PROD_WGPG", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_WGPG);
            cmd.Parameters.Add("@PROD_SUBCATEGORY", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_SUBCATEGORY);
            cmd.Parameters.Add("@PROD_DES", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_DES);
            cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = Convert.ToInt32(entity.Quantity);
            cmd.Parameters.Add("@Vendor_ID", SqlDbType.Int).Value = Convert.ToInt32(entity.Vendor_ID);
            cmd.Parameters.Add("@Branch", SqlDbType.VarChar, 50).Value = entity.Branch;
            cmd.Parameters.Add("@CostPrice", SqlDbType.VarChar, 50).Value = entity.CostPrice;


            cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 50).Value = entity.IUSER;
            cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = DateTime.Now.Date;

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
            /////Insert into ACC STOCK DETAIL//////////////
            int totalpurret = 0;
            string prnumber = "PR-" + entity.Branch + DateTime.Now.ToString("yyMMddHHmmss") + " ";
            totalpurret = Convert.ToInt32(entity.Quantity) * Convert.ToInt32(entity.CostPrice);
            sql = "INSERT INTO Acc_StockDetail(Po_Number,PROD_WGPG,PROD_SUBCATEGORY,PROD_DES,Branch,Quantity,CostPrice,Total,Flag,Remarks,IDAT,IUSER,IDATTIME) VALUES(@prnumber,@PROD_WGPG,@PROD_SUBCATEGORY,@PROD_DES,@Branch,@Quantity,@CostPrice,@Total,@Flag,@Remarks,@IDAT,@IUSER,@IDATTIME)";
            cmd = new SqlCommand(sql, dbConnect);

            cmd.Parameters.Add("@prnumber", SqlDbType.VarChar, 50).Value = prnumber;
            cmd.Parameters.Add("@PROD_WGPG", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_WGPG);
            cmd.Parameters.Add("@PROD_SUBCATEGORY", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_SUBCATEGORY);
            cmd.Parameters.Add("@PROD_DES", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_DES);
            cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = Convert.ToInt32(entity.Quantity);
            cmd.Parameters.Add("@Branch", SqlDbType.VarChar, 50).Value = entity.Branch;
            cmd.Parameters.Add("@CostPrice", SqlDbType.VarChar, 50).Value = entity.CostPrice;
            cmd.Parameters.Add("@Total", SqlDbType.Int).Value = totalpurret;
            cmd.Parameters.Add("@Flag", SqlDbType.VarChar, 50).Value = "Purchase Return";
            cmd.Parameters.Add("@Remarks", SqlDbType.VarChar, 50).Value = "Purchase Return";
            cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 50).Value = entity.IUSER;
            cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = DateTime.Now.Date;
            cmd.Parameters.Add("@IDATTIME", SqlDbType.DateTime).Value = DateTime.Now;
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
            ////////////Stock Update////////////////////
            //sql = "update Acc_Stock set Quantity=Quantity-@Quantity where PROD_WGPG=@PROD_WGPG AND PROD_SUBCATEGORY=@PROD_SUBCATEGORY AND PROD_DES=@PROD_DES and CostPrice= @CostPrice";
            sql = "update Acc_Stock set Quantity=Quantity-@Quantity , PRQty=PRQty+@Quantity  where ACC_STOCKID=@ACC_STOCKID";
            cmd = new SqlCommand(sql, dbConnect);

            cmd.Parameters.Add("@ACC_STOCKID", SqlDbType.BigInt).Value = Convert.ToInt64(entity.ACC_STOCKID);
            cmd.Parameters.Add("@PROD_WGPG", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_WGPG);
            cmd.Parameters.Add("@PROD_SUBCATEGORY", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_SUBCATEGORY);
            cmd.Parameters.Add("@PROD_DES", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_DES);
            cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = Convert.ToInt32(entity.Quantity);
            cmd.Parameters.Add("@CostPrice", SqlDbType.Int).Value = Convert.ToInt32(entity.CostPrice);
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


            //sql = "update Acc_Stock set Quantity=Quantity-@Quantity where PROD_WGPG=@PROD_WGPG AND PROD_SUBCATEGORY=@PROD_SUBCATEGORY AND PROD_DES=@PROD_DES";
            //cmd = new SqlCommand(sql, dbConnect);

            //cmd.Parameters.Add("@PROD_WGPG", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_WGPG);
            //cmd.Parameters.Add("@PROD_SUBCATEGORY", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_SUBCATEGORY);
            //cmd.Parameters.Add("@PROD_DES", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_DES);
            //cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = Convert.ToInt32(entity.Quantity);




            //try
            //{
            //    if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
            //    cmd.ExecuteNonQuery();
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //finally
            //{
            //    dbConnect.Close();
            //}



            /////Journal Entry////////////////

            int totalamountdebit = Convert.ToInt32(entity.Quantity) * Convert.ToInt32(entity.CostPrice);
            #region debit
            sql = "INSERT INTO Acc_Journal(AccountID,Branch,Particular,Remarks,Debit,Credit,RefferenceNumber,IDAT,IDATTIME,Narration) VALUES(@Vendor_ID,@Branch,@Particular,@Remarks,@Debit,@Credit,@RefferenceNumber,@IDAT,@IDATTIME,@Narration)";
            cmd = new SqlCommand(sql, dbConnect);


            cmd.Parameters.Add("@Vendor_ID", SqlDbType.BigInt).Value = Convert.ToInt32(entity.Vendor_ID);
            cmd.Parameters.Add("@Branch", SqlDbType.VarChar, 50).Value = entity.Branch;
            cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "A/P";
            cmd.Parameters.Add("@Remarks", SqlDbType.VarChar, 50).Value = "Supplier";
            cmd.Parameters.Add("@Narration", SqlDbType.VarChar, 500).Value = "Purchase Return To Supplier";
            cmd.Parameters.Add("@Debit", SqlDbType.BigInt).Value = totalamountdebit;
            cmd.Parameters.Add("@Credit", SqlDbType.BigInt).Value = 0;
            cmd.Parameters.Add("@RefferenceNumber", SqlDbType.VarChar, 50).Value = "";
            cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = DateTime.Now.Date;
            cmd.Parameters.Add("@IDATTIME", SqlDbType.DateTime).Value = DateTime.Now;
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
            #endregion debit

            #region credit
            sql = "INSERT INTO Acc_Journal(AccountID,Branch,Particular,Remarks,Debit,Credit,RefferenceNumber,IDAT,IDATTIME,Narration) VALUES(@PROD_DES,@Branch,@Particular,@Remarks,@Debit,@Credit,@RefferenceNumber,@IDAT,@IDATTIME,@Narration)";
            cmd = new SqlCommand(sql, dbConnect);


            cmd.Parameters.Add("@PROD_DES", SqlDbType.BigInt).Value = Convert.ToInt32(entity.PROD_DES);
            cmd.Parameters.Add("@Branch", SqlDbType.VarChar, 50).Value = entity.Branch;
            cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "Purchase";
            cmd.Parameters.Add("@Remarks", SqlDbType.VarChar, 50).Value = "Product";
            cmd.Parameters.Add("@Narration", SqlDbType.VarChar, 500).Value = "Purchase Return To Supplier";
            cmd.Parameters.Add("@Debit", SqlDbType.BigInt).Value = 0;
            cmd.Parameters.Add("@Credit", SqlDbType.BigInt).Value = totalamountdebit;
            cmd.Parameters.Add("@RefferenceNumber", SqlDbType.VarChar, 50).Value = "";
            cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = DateTime.Now.Date;
            cmd.Parameters.Add("@IDATTIME", SqlDbType.DateTime).Value = DateTime.Now;
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
            #endregion credit
        }


        public void Add_Purchase_Amendment(T_PROD entity)
        {


            sql = "INSERT INTO PurchasePrice_Amendment(PROD_WGPG,PROD_SUBCATEGORY,PROD_DES,Branch,Amount,IUSER,IDAT,Vendor_ID) VALUES(@PROD_WGPG,@PROD_SUBCATEGORY,@PROD_DES,@Branch,@CostPrice,@IUSER,@IDAT,@Vendor_ID)";
            cmd = new SqlCommand(sql, dbConnect);


            cmd.Parameters.Add("@PROD_WGPG", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_WGPG);
            cmd.Parameters.Add("@PROD_SUBCATEGORY", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_SUBCATEGORY);
            cmd.Parameters.Add("@PROD_DES", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_DES);

            cmd.Parameters.Add("@Vendor_ID", SqlDbType.Int).Value = Convert.ToInt32(entity.Vendor_ID);
            cmd.Parameters.Add("@Branch", SqlDbType.VarChar, 50).Value = entity.Branch;
            cmd.Parameters.Add("@CostPrice", SqlDbType.VarChar, 50).Value = entity.CostPrice;


            cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 50).Value = entity.IUSER;
            cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = DateTime.Now.Date;

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

        public void UpdateStoreMasterStock(T_PROD entity)
        {

            sql = "update StoreMasterStock set Quantity=Quantity-@Quantity where PROD_WGPG=@PROD_WGPG AND PROD_SUBCATEGORY=@PROD_SUBCATEGORY AND PROD_DES=@PROD_DES";
            cmd = new SqlCommand(sql, dbConnect);

            cmd.Parameters.Add("@PROD_WGPG", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_WGPG);
            cmd.Parameters.Add("@PROD_SUBCATEGORY", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_SUBCATEGORY);
            cmd.Parameters.Add("@PROD_DES", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_DES);
            cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = Convert.ToInt32(entity.Quantity);




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
        public void UpdateCashINOUT(T_PROD entity)
        {
            int total;
            total = Convert.ToInt32(entity.CostPrice) * Convert.ToInt32(entity.Quantity);
            sql = "update CASHINOUT set PURCHASE=@PURCHASE,Vendor_ID=@Vendor_ID,IUSER=@IUSER,IDAT=@IDAT where PURCHASEOID=@OID";
            cmd = new SqlCommand(sql, dbConnect);

            cmd.Parameters.Add("@OID", SqlDbType.BigInt).Value = Convert.ToInt64(entity.OID);
            cmd.Parameters.Add("@Vendor_ID", SqlDbType.BigInt).Value = Convert.ToInt64(entity.Vendor_ID);
            cmd.Parameters.Add("@PURCHASE", SqlDbType.BigInt).Value = total;
            cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 50).Value = entity.IUSER;
            cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = DateTime.Now.Date;




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
        public void DeleteTStock(T_PROD entity)
        {

            sql = "delete from T_STOCK where OID=" + entity.OID + " ";
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

            sql = "delete from Vendor_Incoming where StockID=" + entity.OID + " ";
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

        public void UpdatePurchaseReport(T_PROD entity)
        {
            int total = 0;
            sql = "INSERT INTO Purchase_Report(PROD_WGPG,PROD_SUBCATEGORY,PROD_DES,Vendor_ID,Branch,CostPrice,Quantity,TOTAL,Particular,IUSER,IDAT,StockID) VALUES(@PROD_WGPG,@PROD_SUBCATEGORY,@PROD_DES,@Vendor_ID,@Branch,@CostPrice,@Quantity,@TOTAL,@Particular,@IUSER,@IDAT,@StockID)";
            cmd = new SqlCommand(sql, dbConnect);
            total = Convert.ToInt32(entity.CostPrice) * Convert.ToInt32(entity.Quantity);

            cmd.Parameters.Add("@PROD_WGPG", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_WGPG);
            cmd.Parameters.Add("@PROD_SUBCATEGORY", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_SUBCATEGORY);
            cmd.Parameters.Add("@PROD_DES", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_DES);
            cmd.Parameters.Add("@Vendor_ID", SqlDbType.BigInt).Value = Convert.ToInt64(entity.Vendor_ID);
            cmd.Parameters.Add("@Branch", SqlDbType.BigInt).Value = Convert.ToInt64(entity.Branch);
            cmd.Parameters.Add("@CostPrice", SqlDbType.BigInt).Value = Convert.ToInt64(entity.CostPrice);
            cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = Convert.ToInt32(entity.Quantity);
            cmd.Parameters.Add("@TOTAL", SqlDbType.Int).Value = total;
            cmd.Parameters.Add("@Particular", SqlDbType.VarChar, 50).Value = "Purchase Return";
            cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 50).Value = entity.IUSER;
            cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = DateTime.Now.Date;
            cmd.Parameters.Add("@StockID", SqlDbType.BigInt).Value = Convert.ToInt64(entity.OID);

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


        public void MonitorStoreMasterStock(T_PROD entity)
        {
            if (entity.AccessoriesEdit == "F")
            {

                sqlg = "SELECT TOP 1 * FROM StoreMasterStock ORDER BY OID DESC";
                cmd = new SqlCommand(sqlg, dbConnect);
                try
                {
                    if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {

                        lid = Convert.ToInt32(reader["OID"]);

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

                sql = "INSERT INTO MonitorsStoreMasterStock(LID,PROD_WGPG,PROD_SUBCATEGORY,PROD_DES,Branch,Barcode,CostPrice,SalePrice,Quantity,SaleStatus,ActiveStatus,IUSER,IDAT,EUSER,EDAT,Action) values(@LID,@PROD_WGPG,@PROD_SUBCATEGORY,@PROD_DES,@Branch,@Barcode,@CostPrice,@SalePrice,@Quantity,@SaleStatus,@ActiveStatus,@IUSER,@IDAT,@EUSER,@EDAT,@Action)";
                cmd = new SqlCommand(sql, dbConnect);

                cmd.Parameters.Add("@LID", SqlDbType.BigInt).Value = Convert.ToInt64(entity.OID);
                cmd.Parameters.Add("@PROD_WGPG", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_WGPG);
                cmd.Parameters.Add("@PROD_SUBCATEGORY", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_SUBCATEGORY);
                cmd.Parameters.Add("@PROD_DES", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_DES);
                cmd.Parameters.Add("@Branch", SqlDbType.VarChar, 50).Value = entity.Branch;
                cmd.Parameters.Add("@Barcode", SqlDbType.VarChar, 100).Value = entity.Barcode;
                cmd.Parameters.Add("@CostPrice", SqlDbType.BigInt).Value = Convert.ToInt64(entity.CostPrice);
                cmd.Parameters.Add("@SalePrice", SqlDbType.BigInt).Value = Convert.ToInt64(entity.SalePrice);
                cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = Convert.ToInt32(entity.Quantity);
                cmd.Parameters.Add("@SaleStatus", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@ActiveStatus", SqlDbType.Int).Value = 1;
                cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 50).Value = entity.IUSER;
                cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = DateTime.Now.Date;
                cmd.Parameters.Add("@EUSER", SqlDbType.VarChar, 50).Value = entity.EUSER;
                cmd.Parameters.Add("@EDAT", SqlDbType.Date).Value = DateTime.Now.ToString("yyyy-M-d");
                cmd.Parameters.Add("@Action", SqlDbType.VarChar, 50).Value = entity.Action;
            }
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



        public void MonitorStoreMasterStockForAdd(T_PROD entity)
        {


            sqlg = "SELECT TOP 1 * FROM StoreMasterStock ORDER BY OID DESC";
            cmd = new SqlCommand(sqlg, dbConnect);
            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    lid = Convert.ToInt32(reader["OID"]);

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

            sql = "INSERT INTO MonitorsStoreMasterStock(LID,PROD_WGPG,PROD_SUBCATEGORY,PROD_DES,Branch,Barcode,CostPrice,SalePrice,Quantity,SaleStatus,ActiveStatus,IUSER,IDAT,Action) values(@LID,@PROD_WGPG,@PROD_SUBCATEGORY,@PROD_DES,@Branch,@Barcode,@CostPrice,@SalePrice,@Quantity,@SaleStatus,@ActiveStatus,@IUSER,@IDAT,@Action)";
            cmd = new SqlCommand(sql, dbConnect);

            cmd.Parameters.Add("@LID", SqlDbType.BigInt).Value = lid;
            cmd.Parameters.Add("@PROD_WGPG", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_WGPG);
            cmd.Parameters.Add("@PROD_SUBCATEGORY", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_SUBCATEGORY);
            cmd.Parameters.Add("@PROD_DES", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_DES);
            cmd.Parameters.Add("@Branch", SqlDbType.VarChar, 50).Value = entity.Branch;
            cmd.Parameters.Add("@Barcode", SqlDbType.VarChar, 50).Value = entity.Barcode;
            cmd.Parameters.Add("@CostPrice", SqlDbType.BigInt).Value = Convert.ToInt64(entity.CostPrice);
            cmd.Parameters.Add("@SalePrice", SqlDbType.BigInt).Value = Convert.ToInt64(entity.SalePrice);
            cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = Convert.ToInt32(entity.Quantity);
            cmd.Parameters.Add("@SaleStatus", SqlDbType.Int).Value = 0;
            cmd.Parameters.Add("@ActiveStatus", SqlDbType.Int).Value = 1;
            cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 50).Value = entity.IUSER;
            cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = DateTime.Now.Date;
            cmd.Parameters.Add("@Action", SqlDbType.VarChar, 50).Value = "AddDone";

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




        public void UpdateStockPosting(T_PROD entity)
        {
            if (entity.AccessoriesEdit == "F")
            {
                sql = "update StockPosting set DescriptionOID=@PROD_DES,Barcode=@Barcode where Barcode=@OLDBARCODE";
                cmd = new SqlCommand(sql, dbConnect);

                cmd.Parameters.Add("@PROD_DES", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_DES);
                //cmd.Parameters.Add("@Branch", SqlDbType.VarChar, 50).Value = entity.Branch;
                cmd.Parameters.Add("@Barcode", SqlDbType.VarChar, 100).Value = entity.Barcode;
                //cmd.Parameters.Add("@OLDBARCODE", SqlDbType.VarChar, 100).Value = entity.OLDBARCODE;

            }
            else if (entity.AccessoriesEdit == "T")
            {

            }
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
        public void UpdateClosingBalance(T_PROD entity)
        {
            if (entity.AccessoriesEdit == "F")
            {
                sql = "update ClosingBalance set DescriptionOID=@PROD_DES,Barcode=@Barcode,CostPrice=@CostPrice,SalePrice=@SalePrice where Barcode=@OLDBARCODE";
                cmd = new SqlCommand(sql, dbConnect);

                cmd.Parameters.Add("@PROD_DES", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_DES);
                //cmd.Parameters.Add("@Branch", SqlDbType.VarChar, 50).Value = entity.Branch;
                cmd.Parameters.Add("@Barcode", SqlDbType.VarChar, 100).Value = entity.Barcode;
                //cmd.Parameters.Add("@OLDBARCODE", SqlDbType.VarChar, 100).Value = entity.OLDBARCODE;
                cmd.Parameters.Add("@CostPrice", SqlDbType.BigInt).Value = Convert.ToInt64(entity.CostPrice);
                cmd.Parameters.Add("@SalePrice", SqlDbType.BigInt).Value = Convert.ToInt64(entity.SalePrice);
            }
            else if (entity.AccessoriesEdit == "T")
            {

            }
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
        public T_PROD GetBarcodeCostAndSalePrice(T_PROD obj)
        {
            T_PROD entity = new T_PROD();




            //Get Purchase & Sale Price//
            sql = "select SESPrice,MRP from Description where OID=" + obj.PROD_DES + " and Active=1";
            cmd = new SqlCommand(sql, dbConnect);
            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    if (string.IsNullOrEmpty(reader["SESPrice"].ToString()))
                    {
                        entity.CostPrice = string.Empty;
                    }
                    else
                    {
                        entity.CostPrice = reader["SESPrice"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["MRP"].ToString()))
                    {
                        entity.SalePrice = string.Empty;
                    }
                    else
                    {
                        entity.SalePrice = reader["MRP"].ToString();
                    }
                }
                else
                {
                    entity.CostPrice = string.Empty;
                    entity.SalePrice = string.Empty;
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

        public void ProductCostUpdate(T_PROD entity)
        {
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            List<String> myList = new List<String>();
            string WhereCondition = string.Empty;

            if (entity.Branch != String.Empty & entity.Branch != "0")
            {
                myList.Add("StoreMasterStock.Branch = '" + entity.Branch + "' ");
            }
            if (entity.PROD_WGPG != String.Empty & entity.PROD_WGPG != "0")
            {
                myList.Add("StoreMasterStock.PROD_WGPG = " + entity.PROD_WGPG + " ");
            }
            if (entity.PROD_SUBCATEGORY != String.Empty & entity.PROD_SUBCATEGORY != "0")
            {
                myList.Add("StoreMasterStock.PROD_SUBCATEGORY = " + entity.PROD_SUBCATEGORY + " ");
            }
            if (entity.PROD_DES != String.Empty & entity.PROD_DES != "0")
            {
                myList.Add("StoreMasterStock.PROD_DES = " + entity.PROD_DES + " ");
            }

            myList.Add("StoreMasterStock.Quantity >0");
            myList.Add("StoreMasterStock.ActiveStatus=1");

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

            sql = "SELECT StoreMasterStock.OID,PROD_DES,Branch from StoreMasterStock " + WhereCondition + "  ";
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

            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    // Commented By Yeasin 12-May-2019 //
                    //sql = "update StoreMasterStock set SalePrice=@SalePrice where OID=@OID";
                    //cmd = new SqlCommand(sql, dbConnect);
                    //cmd.Parameters.Add("@OID", SqlDbType.BigInt).Value = Convert.ToInt64(row["OID"].ToString());
                    //cmd.Parameters.Add("@SalePrice", SqlDbType.BigInt).Value = Convert.ToInt64(entity.SalePrice);

                    // Commented By Yeasin 12-May-2019 //
//                    string sqlUpdateDescription = string.Format(@" update Description set MRP='{1}' where OID='{0}' 
//", row["PROD_DES"].ToString(), Convert.ToInt64(entity.SalePrice));
//                    SqlCommand cmdUpdateDescription = new SqlCommand(sqlUpdateDescription, dbConnect);

                    // Added By Yeasin 12-May-2019 //
                    cmd = new SqlCommand("spUpdateProductPrice", dbConnect);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@StockId", SqlDbType.BigInt).Value = Convert.ToInt64(row["OID"].ToString());
                    cmd.Parameters.Add("@DesId", SqlDbType.BigInt).Value = Convert.ToInt64(row["PROD_DES"].ToString());
                    cmd.Parameters.Add("@TProdId", SqlDbType.BigInt).Value = 0;
                    cmd.Parameters.Add("@SalePrice", SqlDbType.BigInt).Value = Convert.ToInt64(entity.SalePrice);

                    try
                    {
                        if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                        cmd.ExecuteNonQuery();
                        // Commented By Yeasin 12-May-2019 //
                        //cmdUpdateDescription.ExecuteNonQuery();
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


            if (entity.PROD_WGPG.ToString() == "111")
            {
                List<String> myList1 = new List<String>();
                if (entity.PROD_WGPG != String.Empty & entity.PROD_WGPG != "0")
                {
                    myList1.Add("T_PROD.PROD_WGPG = " + entity.PROD_WGPG + " ");
                }
                if (entity.PROD_SUBCATEGORY != String.Empty & entity.PROD_SUBCATEGORY != "0")
                {
                    myList1.Add("T_PROD.PROD_SUBCATEGORY = " + entity.PROD_SUBCATEGORY + " ");
                }
                if (entity.PROD_DES != String.Empty & entity.PROD_DES != "0")
                {
                    myList1.Add("T_PROD.PROD_DES = " + entity.PROD_DES + " ");
                }
                myList.Add("T_PROD.Quantity >0");

                string[] myArray1 = myList1.ToArray();
                string where1 = string.Join(" and ", myArray1);

                if (where1 == string.Empty)
                {
                    WhereCondition = string.Empty;
                }
                else
                {
                    WhereCondition = "where " + where1 + " ";
                }


                sql = "SELECT T_PROD.OID from T_PROD " + WhereCondition + " ";
                cmd = new SqlCommand(sql, dbConnect);
                try
                {
                    if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                    da.SelectCommand = cmd;
                    da.Fill(dt1);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dbConnect.Close();
                }

                if (dt1 != null)
                {
                    foreach (DataRow row in dt1.Rows)
                    {
                        // Commented By Yeasin 12-May-2019
                        //sql = "update T_PROD set SalePrice=@SalePrice where OID=@OID";
                        //cmd = new SqlCommand(sql, dbConnect);
                        //cmd.Parameters.Add("@OID", SqlDbType.BigInt).Value = Convert.ToInt64(row["OID"].ToString());
                        //cmd.Parameters.Add("@SalePrice", SqlDbType.BigInt).Value = Convert.ToInt64(entity.SalePrice);

                        // Added By Yeasin 12-May-2019 //
                        cmd = new SqlCommand("spUpdateProductPrice", dbConnect);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@StockId", SqlDbType.BigInt).Value = 0;
                        cmd.Parameters.Add("@DesId", SqlDbType.BigInt).Value = 0;
                        cmd.Parameters.Add("@TProdId", SqlDbType.BigInt).Value = Convert.ToInt64(row["OID"].ToString());
                        cmd.Parameters.Add("@SalePrice", SqlDbType.BigInt).Value = Convert.ToInt64(entity.SalePrice);
                        cmd.Parameters.Add("@Flag", SqlDbType.VarChar, 20).Value = "T_Prod";

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
            }


            /*
            //*****************Update Product Price Table****************************
            String found = string.Empty;
            sql = "select OID from ProductPrice where DescriptionOID=@DescriptionOID";
            cmd = new SqlCommand(sql, dbConnect);
            cmd.Parameters.Add("@DescriptionOID", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_DES);
            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (string.IsNullOrEmpty(reader["OID"].ToString()))
                    {
                        found = string.Empty;
                    }
                    else
                    {
                        found = reader["OID"].ToString();
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

            if (found != string.Empty) {
                sql = "update ProductPrice set SalePrice=@SalePrice where DescriptionOID=@DescriptionOID and Active=1";
                cmd = new SqlCommand(sql, dbConnect);
                cmd.Parameters.Add("@DescriptionOID", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_DES);
                cmd.Parameters.Add("@SalePrice", SqlDbType.BigInt).Value = Convert.ToInt64(entity.SalePrice);
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
            */

        }

        public String AccessoriesStockInHand(T_PROD entity)
        {
            String Qty = "0";
            sql = "select sum(Quantity) as Quantity from T_PROD where PROD_WGPG=" + entity.PROD_WGPG + " and PROD_SUBCATEGORY=" + entity.PROD_SUBCATEGORY + " and PROD_DES=" + entity.PROD_DES + " and Quantity >0 ";
            cmd = new SqlCommand(sql, dbConnect);
            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (string.IsNullOrEmpty(reader["Quantity"].ToString()))
                    {
                        Qty = "0";
                    }
                    else
                    {
                        Qty = reader["Quantity"].ToString();
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

            return Qty;
        }


        public String GETIDAT(String OID)
        {
            String IDAT = "";
            sql = "select IDAT from T_STOCK where OID=" + OID + " ";
            cmd = new SqlCommand(sql, dbConnect);
            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (string.IsNullOrEmpty(reader["IDAT"].ToString()))
                    {
                        IDAT = "";
                    }
                    else
                    {
                        IDAT = reader["IDAT"].ToString();
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

            return IDAT;
        }

        public void AccessoriesStockOut(T_PROD entity)
        {
            sql = "select Quantity,TOTAL from AccessoriesMain where PROD_WGPG=" + entity.PROD_WGPG + " AND PROD_SUBCATEGORY= " + entity.PROD_SUBCATEGORY + " AND PROD_DES= " + entity.PROD_DES + "  AND AccStatus= 1 AND Branch='CCOMxxxx01'";
            cmd = new SqlCommand(sql, dbConnect);
            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    totq11 = reader["Quantity"].ToString();
                    total11 = reader["TOTAL"].ToString();



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
            sql = "select SUM(Quantity) as TOTQ,SUM(TOTAL) as TOTAL from ClosingBalanceAccessories where PROD_WGPG=" + entity.PROD_WGPG + " AND PROD_SUBCATEGORY= " + entity.PROD_SUBCATEGORY + " AND PROD_DES= " + entity.PROD_DES + "  AND AccStatus= 1 AND Branch='CCOMxxxx01'";
            cmd = new SqlCommand(sql, dbConnect);
            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (string.IsNullOrEmpty(reader["TOTQ"].ToString()))
                    {
                        totq12 = "0";
                        total12 = "0";
                    }
                    else
                    {
                        totq12 = reader["TOTQ"].ToString();
                        total12 = reader["TOTAL"].ToString();
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

            total13 = Convert.ToDecimal(total11);
            total14 = Convert.ToDecimal(total12);
            totq13 = Convert.ToDecimal(totq11);
            totq14 = Convert.ToDecimal(totq12);
            totq15 = totq13 + totq14;
            total15 = total13 + total14;

            total16 = total15 / totq15;


            cmd = new SqlCommand("SPP_Accessories_Stockout", dbConnect);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@x", SqlDbType.BigInt, 50).Value = entity.x;
            cmd.Parameters.Add("@y", SqlDbType.Decimal, 50).Value = total16;
            cmd.Parameters.Add("@PROD_WGPG", SqlDbType.VarChar, 50).Value = entity.PROD_WGPG;
            cmd.Parameters.Add("@PROD_SUBCATEGORY", SqlDbType.VarChar, 50).Value = entity.PROD_SUBCATEGORY;
            cmd.Parameters.Add("@PROD_DES", SqlDbType.VarChar, 50).Value = entity.PROD_DES;
            cmd.Parameters.Add("@Branch", SqlDbType.VarChar, 100).Value = entity.Branch;
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
        }

        public void SPP_DeleteProduct(T_PROD entity)
        {
            cmd = new SqlCommand("SPP_StoreMasterStockProductDelete", dbConnect);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@OID", SqlDbType.BigInt).Value = entity.OID;
            cmd.Parameters.Add("@Barcode", SqlDbType.VarChar, 100).Value = entity.Barcode;
            cmd.Parameters.Add("@ActiveStatus", SqlDbType.Int).Value = entity.ActiveStatus;
            cmd.Parameters.Add("@InActiveReason", SqlDbType.VarChar, 300).Value = entity.InActiveReason;
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

        public T_PROD GetProductInformation(String OID)
        {
            T_PROD entity = new T_PROD();
            sql = "select T_STOCK.PROD_WGPG,T_STOCK.PROD_SUBCATEGORY,T_STOCK.PROD_DES,T_STOCK.Vendor_ID,T_STOCK.Branch,T_STOCK.CostPrice,T_STOCK.SalePrice,T_STOCK.Quantity,T_STOCK.IUSER from T_STOCK where T_STOCK.OID=" + OID + " ";
            cmd = new SqlCommand(sql, dbConnect);
            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (string.IsNullOrEmpty(reader["PROD_WGPG"].ToString()))
                    {
                        entity.PROD_WGPG = string.Empty;
                    }
                    else
                    {
                        entity.PROD_WGPG = reader["PROD_WGPG"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["PROD_SUBCATEGORY"].ToString()))
                    {
                        entity.PROD_SUBCATEGORY = string.Empty;
                    }
                    else
                    {
                        entity.PROD_SUBCATEGORY = reader["PROD_SUBCATEGORY"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["PROD_DES"].ToString()))
                    {
                        entity.PROD_DES = string.Empty;
                    }
                    else
                    {
                        entity.PROD_DES = reader["PROD_DES"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["Branch"].ToString()))
                    {
                        entity.Branch = string.Empty;
                    }
                    else
                    {
                        entity.Branch = reader["Branch"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["Vendor_ID"].ToString()))
                    {
                        entity.Vendor_ID = string.Empty;
                    }
                    else
                    {
                        entity.Vendor_ID = reader["Vendor_ID"].ToString();
                    }

                    entity.Barcode = string.Empty;


                    if (string.IsNullOrEmpty(reader["CostPrice"].ToString()))
                    {
                        entity.CostPrice = string.Empty;
                    }
                    else
                    {
                        entity.CostPrice = reader["CostPrice"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["SalePrice"].ToString()))
                    {
                        entity.SalePrice = string.Empty;
                    }
                    else
                    {
                        entity.SalePrice = reader["SalePrice"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["Quantity"].ToString()))
                    {
                        entity.Quantity = string.Empty;
                        entity.OLDQUANTITY = string.Empty;
                    }
                    else
                    {
                        entity.Quantity = reader["Quantity"].ToString();
                        entity.OLDQUANTITY = reader["Quantity"].ToString();
                    }

                    if (string.IsNullOrEmpty(reader["IUSER"].ToString()))
                    {
                        entity.IUSER = string.Empty;
                    }
                    else
                    {
                        entity.IUSER = reader["IUSER"].ToString();
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

        public T_PROD GetProductInformationbydescription(String description)
        {
            T_PROD entity = new T_PROD();
            sql = "select TOP(1)* from Vendor_Incoming where Vendor_Incoming.PROD_DES='" + description + "' order by Vendor_Incoming.OID DESC";
            cmd = new SqlCommand(sql, dbConnect);
            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (string.IsNullOrEmpty(reader["PROD_WGPG"].ToString()))
                    {
                        entity.PROD_WGPG = string.Empty;
                    }
                    else
                    {
                        entity.PROD_WGPG = reader["PROD_WGPG"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["OID"].ToString()))
                    {
                        entity.OID = string.Empty;
                    }
                    else
                    {
                        entity.OID = reader["OID"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["PROD_SUBCATEGORY"].ToString()))
                    {
                        entity.PROD_SUBCATEGORY = string.Empty;
                    }
                    else
                    {
                        entity.PROD_SUBCATEGORY = reader["PROD_SUBCATEGORY"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["CostPrice"].ToString()))
                    {
                        entity.CostPrice = string.Empty;
                    }
                    else
                    {
                        entity.CostPrice = reader["CostPrice"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["PROD_DES"].ToString()))
                    {
                        entity.PROD_DES = string.Empty;
                    }
                    else
                    {
                        entity.PROD_DES = reader["PROD_DES"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["Branch"].ToString()))
                    {
                        entity.Branch = string.Empty;
                    }
                    else
                    {
                        entity.Branch = reader["Branch"].ToString();
                    }





                    //if (string.IsNullOrEmpty(reader["CostPrice"].ToString()))
                    //{
                    //    entity.CostPrice = string.Empty;
                    //}
                    //else
                    //{
                    //    entity.CostPrice = reader["CostPrice"].ToString();
                    //}


                    if (string.IsNullOrEmpty(reader["IUSER"].ToString()))
                    {
                        entity.IUSER = string.Empty;
                    }
                    else
                    {
                        entity.IUSER = reader["IUSER"].ToString();
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

        public T_PROD GetAccessoriesProductInformation(String OID)
        {
            T_PROD entity = new T_PROD();
            sql = "select T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,Description.Description,T_PROD.CostPrice,T_PROD.SalePrice,T_PROD.Quantity from T_PROD inner join Description on T_PROD.PROD_DES=Description.OID inner join SubCategory on Description.SubCategoryID=SubCategory.OID inner join T_WGPG on SubCategory.CategoryID=T_WGPG.OID where T_PROD.OID=" + OID + " ";
            cmd = new SqlCommand(sql, dbConnect);
            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (string.IsNullOrEmpty(reader["WGPG_NAME"].ToString()))
                    {
                        entity.PROD_WGPG = string.Empty;
                    }
                    else
                    {
                        entity.PROD_WGPG = reader["WGPG_NAME"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["SubCategoryName"].ToString()))
                    {
                        entity.PROD_SUBCATEGORY = string.Empty;
                    }
                    else
                    {
                        entity.PROD_SUBCATEGORY = reader["SubCategoryName"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["Description"].ToString()))
                    {
                        entity.PROD_DES = string.Empty;
                    }
                    else
                    {
                        entity.PROD_DES = reader["Description"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["CostPrice"].ToString()))
                    {
                        entity.CostPrice = "0";
                    }
                    else
                    {
                        entity.CostPrice = reader["CostPrice"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["SalePrice"].ToString()))
                    {
                        entity.SalePrice = "0";
                    }
                    else
                    {
                        entity.SalePrice = reader["SalePrice"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["Quantity"].ToString()))
                    {
                        entity.Quantity = "0";
                    }
                    else
                    {
                        entity.Quantity = reader["Quantity"].ToString();
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


        //Search on T_PROD For Accessories
        public DataTable SPP_T_PROD_SearchByDate(T_PROD entity)
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
            if (entity.FromDate != String.Empty & entity.ToDate != string.Empty)
            {
                myList.Add("T_PROD.IDAT between '" + entity.FromDate + "' and '" + entity.ToDate + "' ");
            }
            myList.Add("T_WGPG.OID=111");
            myList.Add("T_PROD.Quantity >0 ");

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
            sql = "select T_PROD.OID,T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,Description.Description,T_CCOM.CCOM_NAME,T_PROD.Barcode,T_PROD.CostPrice,T_PROD.SalePrice,T_PROD.Quantity,CONVERT(VARCHAR(10),T_PROD.IDAT,103)IDAT from T_PROD inner join Description on T_PROD.PROD_DES=Description.OID inner join SubCategory on Description.SubCategoryID=SubCategory.OID inner join T_WGPG on SubCategory.CategoryID=T_WGPG.OID inner join T_CCOM on T_PROD.Branch=T_CCOM.OID " + WhereCondition + " order by T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,Description.Description";

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
        public DataTable SearchAccessoriesStockInHeadOffice(T_PROD entity)
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
            string where = string.Join(" and ", myArray);

            if (where == string.Empty)
            {
                WhereCondition = string.Empty;
            }
            else
            {
                WhereCondition = "where " + where + " ";
            }
            sql = "select T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,Description.Description,T_CCOM.CCOM_NAME,T_PROD.Barcode,SUM(T_PROD.Quantity)Quantity from T_PROD inner join Description on T_PROD.PROD_DES= Description.OID inner join SubCategory on Description.SubCategoryID=SubCategory.OID inner join T_WGPG on SubCategory.CategoryID=T_WGPG.OID inner join T_CCOM on T_PROD.Branch=T_CCOM.OID " + WhereCondition + " group by T_PROD.PROD_WGPG,SubCategory.SubCategoryName,Description.Description,T_WGPG.WGPG_NAME,T_CCOM.CCOM_NAME,T_PROD.Barcode HAVING SUM(T_PROD.Quantity)>0";
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
        //End







        //public DataTable MobileSummary()
        //{
        //gvStockBalance.Visible = false;
        //GridView1.Visible = true;
        //Label2.Visible = true;
        //int j, k;
        //int flag = 2;
        //int sum = 0;

        //sql = "select distinct StoreMasterStock.PROD_WGPG,T_WGPG.WGPG_NAME,StoreMasterStock.PROD_SUBCATEGORY,SubCategory.SubCategoryName from StoreMasterStock,T_WGPG,SubCategory where StoreMasterStock.PROD_WGPG=T_WGPG.OID and StoreMasterStock.PROD_SUBCATEGORY=SubCategory.OID and T_WGPG.WGPG_NAME='Mobile' and StoreMasterStock.Branch='" + Session["StoreID"].ToString() + "' and StoreMasterStock.SaleStatus=0 ";
        //DataTable dt1 = CommonBinder.getDataTable(sql);
        //dt1.Columns.Add("Black", typeof(Int32)); //4
        //dt1.Columns.Add("White", typeof(Int32));//5
        //dt1.Columns.Add("Red", typeof(Int32)); //6
        //dt1.Columns.Add("Blue", typeof(Int32));  //7
        //dt1.Columns.Add("Gray", typeof(Int32));  //8
        //dt1.Columns.Add("Silver", typeof(Int32));  //9        
        //dt1.Columns.Add("Total", typeof(Int32));  //11

        //for (int i = 0; i < dt1.Rows.Count; i++)
        //{
        //    sql = "select Description.Description as Status,SUM(StoreMasterStock.Quantity) as Total from StoreMasterStock,Description where StoreMasterStock.PROD_DES=Description.OID and StoreMasterStock.PROD_WGPG='" + dt1.Rows[i]["PROD_WGPG"] + "' and StoreMasterStock.PROD_SUBCATEGORY='" + dt1.Rows[i]["PROD_SUBCATEGORY"] + "' and StoreMasterStock.Branch='" + Session["StoreID"].ToString() + "' and StoreMasterStock.SaleStatus=0  group by Description.Description ";
        //    DataTable dt2 = CommonBinder.getDataTable(sql);

        //    for (k = 4; k < 10; k++)
        //    {
        //        if (dt2.Rows.Count > 0)
        //        {
        //            for (j = 0; j < dt2.Rows.Count; j++)
        //            {
        //                if (dt1.Columns[k].ColumnName.ToString() == dt2.Rows[j]["Status"].ToString())
        //                {
        //                    dt1.Rows[i][dt1.Columns[k].ColumnName.ToString()] = dt2.Rows[j]["Total"].ToString();
        //                    flag = 1;
        //                    break;
        //                }
        //                else
        //                {
        //                    flag = 0;
        //                }

        //                if (flag == 0)
        //                {
        //                    dt1.Rows[i][dt1.Columns[k].ColumnName.ToString()] = "0";
        //                }

        //            }
        //        }
        //        else
        //        {
        //            dt1.Rows[i][dt1.Columns[k].ColumnName.ToString()] = "0";
        //        }
        //    }
        //    dt1.Rows[i]["Total"] = Convert.ToInt32(dt1.Rows[i]["Black"].ToString()) + Convert.ToInt32(dt1.Rows[i]["White"].ToString()) + Convert.ToInt32(dt1.Rows[i]["Red"].ToString()) + Convert.ToInt32(dt1.Rows[i]["Blue"].ToString()) + Convert.ToInt32(dt1.Rows[i]["Gray"].ToString()) + Convert.ToInt32(dt1.Rows[i]["Silver"].ToString());

        //}
        //foreach (DataRow row in dt1.Rows) // Loop over the rows.
        //{
        //    sum = sum + Convert.ToInt32(row["Total"].ToString());
        //    Label2.Text = "Total: " + sum.ToString();
        //}

        //GridView1.DataSource = dt1;
        //GridView1.DataBind();
        //Session["GridData"] = dt1;
        //Session["ReportPage"] = 2;
        //}








        public DataTable SearchDateWiseBranchStock(T_PROD entity)
        {
            DataTable dt = new DataTable();
            if (entity.FromDate != string.Empty & entity.ToDate != string.Empty & entity.Branch != string.Empty)
            {
                cmd = new SqlCommand("SPP_DateWiseStoreStockHistory", dbConnect);
                cmd.Parameters.Add("@DateFrom", SqlDbType.Date).Value = Convert.ToDateTime(entity.FromDate);
                cmd.Parameters.Add("@DateTo", SqlDbType.Date).Value = Convert.ToDateTime(entity.ToDate);
                cmd.Parameters.Add("@Branch", SqlDbType.VarChar, 50).Value = entity.Branch;
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 300;
                da.Fill(dt);
            }
            return dt;
        }

        public String FindBarcode(String Barcode)
        {
            String found = string.Empty;
            sql = "select StoreMasterStock.OID from StoreMasterStock where StoreMasterStock.Barcode='" + Barcode + "' ";
            // sql = "select T_STOCK.OID from T_STOCK where T_STOCK.Barcode='" + Barcode + "' ";
            cmd = new SqlCommand(sql, dbConnect);
            try
            {
                if (dbConnect.State == ConnectionState.Closed)
                    dbConnect.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (string.IsNullOrEmpty(reader["OID"].ToString()))
                    {
                        found = string.Empty;
                    }
                    else
                    {
                        found = reader["OID"].ToString();
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

            return found;
        }
        public String FindBarcodeNew(String Barcode, String Shop_id)
        {
            String found = string.Empty;
            //sql = "select StoreMasterStock.OID from StoreMasterStock where StoreMasterStock.Barcode='" + Barcode + "' ";
            sql = "select T_STOCK.OID from T_STOCK where T_STOCK.SaleStatus in(0) AND T_STOCK.Barcode='" + Barcode + "' AND T_STOCK.Branch='" + Shop_id + "' AND T_STOCK.IDAT >'26 sep 2017' ";
            cmd = new SqlCommand(sql, dbConnect);
            try
            {
                if (dbConnect.State == ConnectionState.Closed)
                    dbConnect.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (string.IsNullOrEmpty(reader["OID"].ToString()))
                    {
                        found = string.Empty;
                    }
                    else
                    {
                        found = reader["OID"].ToString();
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

            return found;
        }
        public DataTable AccessoriesStockOutReport(T_PROD entity)
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
            if (entity.PROD_DES.ToString() != String.Empty & entity.PROD_DES.ToString() != "0")
            {
                myList.Add("Description.OID = " + entity.PROD_DES + " ");
            }
            if (entity.FromDate != String.Empty & entity.ToDate != string.Empty)
            {
                myList.Add("StoreAccessoriesHistory.IDAT between '" + entity.FromDate + "' and '" + entity.ToDate + "' ");
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
            sql = "select T_CCOM.CCOM_NAME,T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,Description.Description,StoreAccessoriesHistory.Barcode,StoreAccessoriesHistory.Quantity,CONVERT(VARCHAR(10),StoreAccessoriesHistory.IDAT,103) AS IDAT from StoreAccessoriesHistory inner join Description on StoreAccessoriesHistory.PROD_DES=Description.OID inner join SubCategory on Description.SubCategoryID=SubCategory.OID inner join T_WGPG on SubCategory.CategoryID=T_WGPG.OID inner join T_CCOM on StoreAccessoriesHistory.Branch=T_CCOM.OID  " + WhereCondition + " order by T_CCOM.CCOM_NAME,T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,Description.Description,StoreAccessoriesHistory.IDAT ";
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



        //Product History
        public DataTable OpeningBalance(T_PROD entity)
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
            if (entity.IDAT != String.Empty)
            {
                myList.Add("StockPosting.IDAT < '" + entity.FromDate + "' ");
            }
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
            sql = "select T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,SUM(StockPosting.InwardQty) - SUM(StockPosting.OutwardQty) as InwardsQty,OutwardsQty=0 from StockPosting left join Description on StockPosting.DescriptionOID=Description.OID left join SubCategory on Description.SubCategoryID=SubCategory.OID left join T_WGPG on SubCategory.CategoryID=T_WGPG.OID left join T_CCOM on StockPosting.BranchOID=T_CCOM.OID " + WhereCondition + " group by T_WGPG.WGPG_NAME,SubCategory.SubCategoryName ";
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


            //DataSet ds = new DataSet();
            ////Purchase
            //DataTable dt1 = new DataTable();
            //List<String> myList1 = new List<String>();
            //string WhereCondition1 = string.Empty;

            //if (entity.Branch != String.Empty & entity.Branch != "0")
            //{
            //    myList1.Add("T_CCOM.OID = '" + entity.Branch + "' ");
            //}
            //if (entity.PROD_WGPG.ToString() != String.Empty & entity.PROD_WGPG.ToString() != "0")
            //{
            //    myList1.Add("T_WGPG.OID = " + entity.PROD_WGPG + " ");
            //}
            //if (entity.PROD_SUBCATEGORY.ToString() != String.Empty & entity.PROD_SUBCATEGORY.ToString() != "0")
            //{
            //    myList1.Add("SubCategory.OID = " + entity.PROD_SUBCATEGORY + " ");
            //}           
            //if (entity.IDAT != String.Empty)
            //{
            //    myList1.Add("StoreMasterStock.IDAT < '" + entity.IDAT + "' ");
            //}            
            //myList1.Add("StoreMasterStock.ActiveStatus=1");
            //string[] myArray1 = myList1.ToArray();
            //string where1 = string.Join(" and ", myArray1);

            //if (where1 == string.Empty)
            //{
            //    WhereCondition1 = string.Empty;
            //}
            //else
            //{
            //    WhereCondition1 = "where " + where1 + " ";
            //}
            //sql = "select T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,COUNT(StoreMasterStock.Barcode) as InwardsQty from StoreMasterStock left join Description on StoreMasterStock.PROD_DES=Description.OID left join SubCategory on Description.SubCategoryID=SubCategory.OID left join T_WGPG on SubCategory.CategoryID=T_WGPG.OID left join T_CCOM on StoreMasterStock.Branch=T_CCOM.OID " + WhereCondition1 + " group by T_WGPG.WGPG_NAME,SubCategory.SubCategoryName  ";
            //cmd = new SqlCommand(sql, dbConnect);
            //try
            //{
            //    if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
            //    da.SelectCommand = cmd;
            //    da.Fill(dt1);
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //finally
            //{
            //    dbConnect.Close();
            //}
            //ds.Tables.Add(dt1);


            ////Sale
            //DataTable dt2 = new DataTable();
            //List<String> myList2 = new List<String>();
            //string WhereCondition2 = string.Empty;

            //if (entity.Branch != String.Empty & entity.Branch != "0")
            //{
            //    myList2.Add("T_CCOM.OID = '" + entity.Branch + "' ");
            //}
            //if (entity.PROD_WGPG.ToString() != String.Empty & entity.PROD_WGPG.ToString() != "0")
            //{
            //    myList2.Add("T_WGPG.OID = " + entity.PROD_WGPG + " ");
            //}
            //if (entity.PROD_SUBCATEGORY.ToString() != String.Empty & entity.PROD_SUBCATEGORY.ToString() != "0")
            //{
            //    myList2.Add("SubCategory.OID = " + entity.PROD_SUBCATEGORY + " ");
            //}
            //if (entity.IDAT != String.Empty)
            //{
            //    myList2.Add("T_SALES_MST.IDAT < '" + entity.IDAT + "' ");
            //}
            //myList2.Add("T_SALES_MST.DropStatus=0");
            //string[] myArray2 = myList2.ToArray();
            //string where2 = string.Join(" and ", myArray2);

            //if (where2 == string.Empty)
            //{
            //    WhereCondition2 = string.Empty;
            //}
            //else
            //{
            //    WhereCondition2 = "where " + where2 + " ";
            //}
            //sql = "select T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,SUM(T_SALES_DTL.SaleQty) as OutwardsQty from T_SALES_DTL inner join T_SALES_MST on T_SALES_DTL.InvoiceNo=T_SALES_MST.InvoiceNo inner join Description on T_SALES_DTL.DescriptionID=Description.OID inner join SubCategory on Description.SubCategoryID=SubCategory.OID inner join T_WGPG on SubCategory.CategoryID=T_WGPG.OID inner join T_CCOM on T_SALES_MST.StoreID=T_CCOM.OID " + WhereCondition2 + "  group by T_WGPG.WGPG_NAME,SubCategory.SubCategoryName  ";
            //cmd = new SqlCommand(sql, dbConnect);
            //try
            //{
            //    if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
            //    da.SelectCommand = cmd;
            //    da.Fill(dt2);
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //finally
            //{
            //    dbConnect.Close();
            //}

            //ds.Tables.Add(dt2);


            //return ds;

        }
        public DataTable ProductHistory(T_PROD entity)
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
            if (entity.IDAT != String.Empty)
            {
                myList.Add("StockPosting.IDAT between '" + entity.FromDate + "' and '" + entity.ToDate + "' ");
            }
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
            sql = "select T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,StockPosting.Particulars,StockPosting.InwardQty,StockPosting.OutwardQty,CONVERT(VARCHAR(10),StockPosting.IDAT,103)IDAT from StockPosting left join Description on StockPosting.DescriptionOID=Description.OID left join SubCategory on Description.SubCategoryID=SubCategory.OID left join T_WGPG on SubCategory.CategoryID=T_WGPG.OID left join T_CCOM on StockPosting.BranchOID=T_CCOM.OID " + WhereCondition + " order by StockPosting.OID ASC ";

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
        public DataTable ProductHistoryPurchase(T_PROD entity)
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
            if (entity.IDAT != String.Empty)
            {
                myList.Add("StoreMasterStock.IDAT ='" + entity.IDAT + "' ");
            }
            myList.Add("StoreMasterStock.ActiveStatus=1");
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
            sql = "select T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,COUNT(StoreMasterStock.Barcode) as InwardsQty,CONVERT(VARCHAR(10),StoreMasterStock.IDAT,103) AS IDAT from StoreMasterStock inner join Description on StoreMasterStock.PROD_DES=Description.OID inner join SubCategory on Description.SubCategoryID=SubCategory.OID inner join T_WGPG on SubCategory.CategoryID=T_WGPG.OID inner join T_CCOM on StoreMasterStock.Branch=T_CCOM.OID " + WhereCondition + " group by T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,CONVERT(VARCHAR(10),StoreMasterStock.IDAT,103)";
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
        public DataTable ProductHistorySale(T_PROD entity)
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
            if (entity.IDAT != String.Empty)
            {
                myList.Add("T_SALES_MST.IDAT ='" + entity.IDAT + "' ");
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
            sql = "select T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,SUM(T_SALES_DTL.SaleQty) as OutwardsQty,CONVERT(VARCHAR(10),T_SALES_MST.IDAT,103) AS IDAT from T_SALES_DTL inner join T_SALES_MST on T_SALES_DTL.InvoiceNo=T_SALES_MST.InvoiceNo inner join Description on T_SALES_DTL.DescriptionID=Description.OID inner join SubCategory on Description.SubCategoryID=SubCategory.OID inner join T_WGPG on SubCategory.CategoryID=T_WGPG.OID inner join T_CCOM on T_SALES_MST.StoreID=T_CCOM.OID " + WhereCondition + "  group by T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,T_SALES_MST.IDAT  ";
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
        public DataTable ProductHistoryStockTransfer(T_PROD entity)
        {
            DataTable dt = new DataTable();
            List<String> myList = new List<String>();
            string WhereCondition = string.Empty;

            if (entity.Branch != String.Empty & entity.Branch != "0")
            {
                myList.Add("StockReturn_MST.FromStoreID = '" + entity.Branch + "' ");
            }
            if (entity.PROD_WGPG.ToString() != String.Empty & entity.PROD_WGPG.ToString() != "0")
            {
                myList.Add("T_WGPG.OID = " + entity.PROD_WGPG + " ");
            }
            if (entity.PROD_SUBCATEGORY.ToString() != String.Empty & entity.PROD_SUBCATEGORY.ToString() != "0")
            {
                myList.Add("SubCategory.OID = " + entity.PROD_SUBCATEGORY + " ");
            }
            if (entity.IDAT != String.Empty)
            {
                myList.Add("StockReturn_MST.IDAT ='" + entity.IDAT + "' ");
            }
            myList.Add("StockReturn_MST.ApprovedStatus=1");
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
            sql = "select T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,COUNT(StockReturn_DTL.Barcode) as OutwardsQty,CONVERT(VARCHAR(10),StockReturn_MST.IDAT,103) AS IDAT from StockReturn_DTL inner join StockReturn_MST on StockReturn_DTL.StockReturnID=StockReturn_MST.StockReturnID left join Description on StockReturn_DTL.DescriptionID=Description.OID left join SubCategory on Description.SubCategoryID=SubCategory.OID left join T_WGPG on SubCategory.CategoryID=T_WGPG.OID inner join T_CCOM on StockReturn_MST.FromStoreID=T_CCOM.OID " + WhereCondition + " group by T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,StockReturn_MST.IDAT   ";
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
        public DataTable ProductHistoryStockReceive(T_PROD entity)
        {
            DataTable dt = new DataTable();
            List<String> myList = new List<String>();
            string WhereCondition = string.Empty;

            if (entity.Branch != String.Empty & entity.Branch != "0")
            {
                myList.Add("StockReturn_MST.ToStoreID = '" + entity.Branch + "' ");
            }
            if (entity.PROD_WGPG.ToString() != String.Empty & entity.PROD_WGPG.ToString() != "0")
            {
                myList.Add("T_WGPG.OID = " + entity.PROD_WGPG + " ");
            }
            if (entity.PROD_SUBCATEGORY.ToString() != String.Empty & entity.PROD_SUBCATEGORY.ToString() != "0")
            {
                myList.Add("SubCategory.OID = " + entity.PROD_SUBCATEGORY + " ");
            }
            if (entity.IDAT != String.Empty)
            {
                myList.Add("StockReturn_MST.EDAT ='" + entity.IDAT + "' ");
            }
            myList.Add("StockReturn_MST.ApprovedStatus=1");
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
            sql = "select T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,COUNT(StockReturn_DTL.Barcode) as InwardsQty,CONVERT(VARCHAR(10),StockReturn_MST.IDAT,103) AS IDAT from StockReturn_DTL inner join StockReturn_MST on StockReturn_DTL.StockReturnID=StockReturn_MST.StockReturnID left join Description on StockReturn_DTL.DescriptionID=Description.OID left join SubCategory on Description.SubCategoryID=SubCategory.OID left join T_WGPG on SubCategory.CategoryID=T_WGPG.OID inner join T_CCOM on StockReturn_MST.ToStoreID=T_CCOM.OID " + WhereCondition + " group by  T_WGPG.WGPG_NAME, SubCategory.SubCategoryName, StockReturn_MST.IDAT   ";
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
        public DataTable ProductHistoryStockReturn(T_PROD entity)
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
            if (entity.PROD_DES.ToString() != String.Empty & entity.PROD_DES.ToString() != "0")
            {
                myList.Add("Description.OID = " + entity.PROD_DES + " ");
            }
            if (entity.IDAT != String.Empty)
            {
                myList.Add("StockReturn_MST.IDAT ='" + entity.IDAT + "' ");
            }
            myList.Add("StockReturn_MST.ApprovedStatus=1");
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
            sql = "select T_CCOM.CCOM_NAME,T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,Description.Description,COUNT(StockReturn_DTL.RQty) as OutwardsQty,SUM(StoreMasterStock.SalePrice) as OutwardsValue,CONVERT(VARCHAR(10),StockReturn_MST.IDAT,103) AS IDAT from StockReturn_DTL inner join StockReturn_MST on StockReturn_DTL.StockReturnID=StockReturn_MST.StockReturnID inner join Description on StockReturn_DTL.DescriptionID=Description.OID inner join SubCategory on Description.SubCategoryID=SubCategory.OID inner join T_WGPG on SubCategory.CategoryID=T_WGPG.OID inner join T_CCOM on StockReturn_MST.FromStoreID=T_CCOM.OID inner join StoreMasterStock on StockReturn_DTL.Barcode=StoreMasterStock.Barcode " + WhereCondition + " group by T_CCOM.CCOM_NAME,T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,Description.Description,StockReturn_MST.IDAT order by T_CCOM.CCOM_NAME,T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,Description.Description ";
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
        //End Product History 




        public DataTable DeleteHistory(T_PROD entity)
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
            if (entity.PROD_DES.ToString() != String.Empty & entity.PROD_DES.ToString() != "0")
            {
                myList.Add("Description.OID = " + entity.PROD_DES + " ");
            }
            if (entity.FromDate != String.Empty & entity.ToDate != String.Empty)
            {
                myList.Add("StoreMasterStock.EDAT between '" + entity.FromDate + "' and '" + entity.ToDate + "' ");
            }
            if (entity.Barcode != String.Empty)
            {
                myList.Add("StoreMasterStock.Barcode = '" + entity.Barcode + "' ");
            }
            myList.Add("StoreMasterStock.ActiveStatus = 0");

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
            sql = "select StoreMasterStock.Branch as BranchOID,StoreMasterStock.PROD_DES as DescriptionOID,T_CCOM.CCOM_NAME,T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,Description.Description,StoreMasterStock.Barcode,CASE StoreMasterStock.ActiveStatus WHEN 1 THEN 'Y' Else 'N' END as ActiveStatus,StoreMasterStock.InActiveReason,[User].UserFullName,CONVERT(VARCHAR(10),StoreMasterStock.EDAT,103) AS EDAT from StoreMasterStock inner join T_CCOM on StoreMasterStock.Branch=T_CCOM.OID inner join Description on StoreMasterStock.PROD_DES=Description.OID inner join SubCategory on Description.SubCategoryID=SubCategory.OID inner join T_WGPG on SubCategory.CategoryID=T_WGPG.OID inner join [User] on StoreMasterStock.EUSER=[User].UserId " + WhereCondition + "  ";
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

        public void purchaseReturn4BarcodeD(int StoreMasterStockOID, int T_STOCKOID, int T_PRODOID, int PROD_DESOID, string Branch, string Barcode, int PROD_WGPG, int PROD_SUBCATEGORY, int Vendor_ID, Int64 CostPrice, string IUSER)
        {

            string sqlCommand = string.Format(@"
EXECUTE [dbo].[SPP_purchaseReturn4Barcode] 
@StoreMasterStockOID ={0}, @T_STOCKOID ={1}, @T_PRODOID ={2},@PROD_DES={3}, @Branch='{4}', @Barcode='{5}',@PROD_WGPG={6},@PROD_SUBCATEGORY={7},@Vendor_ID={8},@CostPrice={9},@IUSER='{10}',@IDAT='{11}'
", StoreMasterStockOID, T_STOCKOID, T_PRODOID, PROD_DESOID, Branch, Barcode, PROD_WGPG, PROD_SUBCATEGORY, Vendor_ID, CostPrice, IUSER, DateTime.Now.ToString());


            //for journal entry
            string InsertDR = string.Format(@"
INSERT INTO Acc_Journal(AccountID,Branch,Particular,Remarks,Debit,Credit,RefferenceNumber,IDAT,IDATTIME,Narration) 
VALUES({0},{1},'{2}','{3}',{4}
,{5},'{6}','{7}','{8}','{9}')
", Vendor_ID, Branch, "A/P", "Supplier", CostPrice
, 0, string.Format(@"Ref-{0}", Barcode), DateTime.Now.ToString(), DateTime.Now.ToString(), "Purchase Return(barcode) From Supplier");

            string InsertCR = string.Format(@"
INSERT INTO Acc_Journal(AccountID,Branch,Particular,Remarks,Debit,Credit,RefferenceNumber,IDAT,IDATTIME,Narration) 
VALUES({0},{1},'{2}','{3}',{4}
,{5},'{6}','{7}','{8}','{9}')
", PROD_DESOID, Branch, "Purchase", "Product", 0
, CostPrice, string.Format(@"Ref-{0}", Barcode), DateTime.Now.ToString(), DateTime.Now.ToString(), "Purchase Return(barcode) From Supplier");


            cmd = new SqlCommand(sqlCommand, dbConnect);
            SqlCommand cmd1 = new SqlCommand(InsertDR, dbConnect);
            SqlCommand cmd2 = new SqlCommand(InsertCR, dbConnect);
            //cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                if (dbConnect.State == ConnectionState.Closed)
                {
                    dbConnect.Open();
                    cmd.ExecuteNonQuery();
                    cmd1.ExecuteNonQuery();
                    cmd2.ExecuteNonQuery();
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

        // Added By Yeasin 12-May-2019
        public void PurchaseReturnByBarcode(int StoreMasterStockOID, int T_STOCKOID, int T_PRODOID, int PROD_DESOID, string Branch, string Barcode, int PROD_WGPG, int PROD_SUBCATEGORY, int Vendor_ID, Int64 CostPrice, string IUSER)
        {
            cmd = new SqlCommand("SPP_PurchaseReturnByBarcode", dbConnect);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@StoreMasterStockOID", SqlDbType.BigInt).Value = StoreMasterStockOID;
            cmd.Parameters.Add("@T_STOCKOID", SqlDbType.BigInt).Value = T_STOCKOID;
            cmd.Parameters.Add("@T_PRODOID", SqlDbType.BigInt).Value = T_PRODOID;
            cmd.Parameters.Add("@PROD_DES", SqlDbType.BigInt).Value = PROD_DESOID;
            cmd.Parameters.Add("@Branch", SqlDbType.NVarChar, 100).Value = Branch;
            cmd.Parameters.Add("@Barcode", SqlDbType.VarChar, 100).Value = Barcode;
            cmd.Parameters.Add("@PROD_WGPG", SqlDbType.BigInt).Value = PROD_WGPG;
            cmd.Parameters.Add("@PROD_SUBCATEGORY", SqlDbType.BigInt).Value = PROD_SUBCATEGORY;
            cmd.Parameters.Add("@Vendor_ID", SqlDbType.Int).Value = Vendor_ID;
            cmd.Parameters.Add("@CostPrice", SqlDbType.BigInt).Value = CostPrice;
            cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 50).Value = IUSER;
            cmd.Parameters.Add("@IDAT", SqlDbType.Date, 50).Value = DateTime.Now.Date;

            try
            {
                if (dbConnect.State == ConnectionState.Closed)
                {
                    dbConnect.Open();
                    cmd.ExecuteNonQuery();
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

        // Added By Yeasin 23-May-2019
        // Added By Yeasin 23-May-2019
        public DataTable RetailerDueReport(T_PROD entity)
        {
            DataTable dt = new DataTable();
            List<String> myList = new List<String>();
            string WhereCondition = string.Empty;

            if (entity.Vendor_ID != String.Empty && entity.Vendor_ID != "0")
            {
                myList.Add("C.ID = '" + entity.Vendor_ID + "' ");
            }

            myList.Add("SI.OID = '" + entity.Branch + "' ");

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


            //            sql = string.Format(@"Select C.ID,C.Name,
            //ISNULL((Select Sum(ISNULL(D.SalePrice,0)-ISNULL(D.Discount,0)) From T_SALES_MST M Inner Join Acc_StockDetail D On M.InvoiceNo = D.Po_Number Where M.RetailerID=C.ID And D.Flag='Sale'),0)'TotalSaleAmount',
            //ISNULL((Select Sum(ISNULL(D.SalePrice,0)-ISNULL(D.Discount,0)) From T_SALES_MST M Inner Join Acc_StockDetail D On M.InvoiceNo = D.Po_Number Where M.RetailerID=C.ID And D.Flag='Sale Return'),0)'TotalSaleReturn',ISNULL((Select (SUM(Debit)-SUM(Credit)) From Acc_Journal Where Particular='A/R' And AccountID=Cast(C.ID as nvarchar(50))),0) 'TotalDueAmount',ISNULL((Select SUM(ISNULL(CashAmount,0))+SUM(ISNULL(CardAmount,0)) From RetailerPayments Where RetailerID=C.ID) ,0)'TotalPayment',Case When ((ISNULL((Select (SUM(Debit)-SUM(Credit)) From Acc_Journal Where Particular='A/R' And AccountID=Cast(C.ID as nvarchar(50))),0) -ISNULL((Select SUM(ISNULL(CashAmount,0))+SUM(ISNULL(CardAmount,0)) From RetailerPayments Where RetailerID=C.ID) ,0))) <0 then 0 else (ISNULL((Select (SUM(Debit)-SUM(Credit)) From Acc_Journal Where Particular='A/R' And AccountID=Cast(C.ID as nvarchar(50))),0) -ISNULL((Select SUM(ISNULL(CashAmount,0))+SUM(ISNULL(CardAmount,0)) From RetailerPayments Where RetailerID=C.ID) ,0)) End
            //'RemainDueAmount'
            //From ShopInfo SI 
            //Inner Join Customers C On SI.OID = C.Branch
            //{0}", WhereCondition);

            sql = string.Format(@"Select C.ID,C.Name,ISNULL((Select Sum(ISNULL(D.SalePrice,0)-ISNULL(D.Discount,0)) From T_SALES_MST M 
Inner Join Acc_StockDetail D On M.InvoiceNo = D.Po_Number Where M.RetailerID=C.ID And D.Flag='Sale'),0)'TotalSaleAmount',
ISNULL((Select Sum(ISNULL(D.SalePrice,0)-ISNULL(D.Discount,0)) From T_SALES_MST M 
Inner Join Acc_StockDetail D On M.InvoiceNo = D.Po_Number
 Where M.RetailerID=C.ID And D.Flag='Sale Return'),0)'TotalSaleReturn',
ISNULL((Select (SUM(Debit)-SUM(Credit)) From Acc_Journal 
Where Particular='A/R' And AccountID=Cast(C.ID as nvarchar(50))),0) 'TotalDueAmount',
ISNULL((Select SUM(ISNULL(CashAmount,0))+SUM(ISNULL(CardAmount,0)) 
From RetailerPayments Where RetailerID=C.ID) ,0)'TotalPayment',
Case When ((ISNULL((Select (SUM(Debit)-SUM(Credit)) 
From Acc_Journal Where Particular='A/R' 
And AccountID=Cast(C.ID as nvarchar(50))),0)-ISNULL((Select SUM(ISNULL(CashAmount,0))+SUM(ISNULL(CardAmount,0)) 
From RetailerPayments Where RetailerID=C.ID) ,0))) <0 then 0 else (ISNULL((Select (SUM(Debit)-SUM(Credit)) 
From Acc_Journal Where Particular='A/R' And AccountID=Cast(C.ID as nvarchar(50))),0) -ISNULL((Select SUM(ISNULL(CashAmount,0))+SUM(ISNULL(CardAmount,0)) 
From RetailerPayments Where RetailerID=C.ID) ,0)) End 'RemainDueAmount',
ISNULL((Select SUM(Debit) 
From Acc_Journal Where RefferenceNumber IN (Select InvoiceNo From T_SALES_MST Where RetailerID=C.ID) And Remarks='Cash'),0) 'TotalCashReceived',
sl.ImageByte 
From ShopInfo SI 
Inner Join Customers C On SI.OID = C.Branch
Left Join Shop_Logo sl ON SI.OID = sl.Shop_id
{0}", WhereCondition);




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
