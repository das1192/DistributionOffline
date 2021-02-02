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
    public class StockReturn_DAL
    {
        SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
        SqlDataAdapter da = new SqlDataAdapter();
        SqlCommand cmd;
        String sql;
        SqlDataReader reader;
        string CostPrice;
        string Average;
        string SalePrice;
        string OID;


        public String Add_StockReturnMST(StockReturn_BO entity)
        {
            Int64 StockReturnID;
            sql = "insert into StockReturn_MST(StockReturnNo,FromStoreID,ToStoreID,ApprovedStatus,ReferenceBy,IUSER,IDAT,EUSER,EDAT) OUTPUT INSERTED.StockReturnID values (@StockReturnNo,@FromStoreID,@ToStoreID,@ApprovedStatus,@ReferenceBy,@IUSER,@IDAT,@EUSER,@EDAT)";
            cmd = new SqlCommand(sql, dbConnect);
            cmd.Parameters.Add("@StockReturnNo", SqlDbType.VarChar, 100).Value = entity.StockReturnNo;
            cmd.Parameters.Add("@FromStoreID", SqlDbType.VarChar, 100).Value = entity.FromStoreID;
            cmd.Parameters.Add("@ToStoreID", SqlDbType.VarChar, 100).Value = entity.ToStoreID;
            cmd.Parameters.Add("@ApprovedStatus", SqlDbType.Int).Value = Convert.ToInt16(entity.ApprovedStatus);
            cmd.Parameters.Add("@ReferenceBy", SqlDbType.VarChar, 200).Value = entity.ReferenceBy;
            cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 100).Value = entity.IUSER;
            cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = DateTime.Now.Date;
            cmd.Parameters.Add("@EUSER", SqlDbType.VarChar, 100).Value = entity.EUSER;
            cmd.Parameters.Add("@EDAT", SqlDbType.Date).Value = DateTime.Now.Date;
            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                StockReturnID = Convert.ToInt64(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbConnect.Close();
            }
            return StockReturnID.ToString();
        }


        public void Add_StockReturnDTL(StockReturn_BO entity)
        {
            sql = "insert into StockReturn_DTL(StockReturnID,DescriptionID,Barcode,RQty,faulty_stat)values(@StockReturnID,@DescriptionID,@Barcode,@RQty,@faulty_stat)";
            cmd = new SqlCommand(sql, dbConnect);
            cmd.Parameters.Add("@StockReturnID", SqlDbType.BigInt).Value = Convert.ToInt64(entity.StockReturnID);
            cmd.Parameters.Add("@DescriptionID", SqlDbType.Int).Value = Convert.ToInt64(entity.PROD_DES); 
            cmd.Parameters.Add("@Barcode", SqlDbType.VarChar,100).Value = entity.Barcode;
            cmd.Parameters.Add("@RQty", SqlDbType.Int).Value = Convert.ToInt32(entity.RQty);
            cmd.Parameters.Add("@faulty_stat", SqlDbType.VarChar, 100).Value = entity.FaultyProd;
            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                cmd.ExecuteScalar();
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




       


        public DataTable txtBarcode_TextChanged(T_PROD entity)
        {
            DataTable dt = new DataTable();
            List<String> myList = new List<String>();
            string WhereCondition = string.Empty;

            myList.Add("StoreMasterStock.Branch = '" + entity.Branch + "' ");
            myList.Add("StoreMasterStock.Barcode = '" + entity.Barcode + "' ");            
            myList.Add("StoreMasterStock.SaleStatus=0");
            myList.Add("StoreMasterStock.Quantity >0");
            myList.Add("StoreMasterStock.ActiveStatus =1");  

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
            sql = "select distinct StoreMasterStock.PROD_WGPG as PCategoryID,T_WGPG.WGPG_NAME as PCategoryName,StoreMasterStock.PROD_SUBCATEGORY as SubCategoryID,SubCategory.SubCategoryName as SubCategory,StoreMasterStock.PROD_DES as DescriptionID,Description.Description,StoreMasterStock.Barcode,1 Qty,ISNULL(Stock.StockInHand,0)StockInHand from StoreMasterStock inner join Description on StoreMasterStock.PROD_DES=Description.OID inner join SubCategory on Description.SubCategoryID=SubCategory.OID inner join T_WGPG on SubCategory.CategoryID=T_WGPG.OID left join(select StoreMasterStock.Barcode,SUM(StoreMasterStock.Quantity)StockInHand from StoreMasterStock " + WhereCondition + " group by StoreMasterStock.Barcode ) Stock on Stock.Barcode = StoreMasterStock.Barcode " + WhereCondition + " ";            
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

        public DataTable txtBarcode_TextChangedDOA(T_PROD entity)
        {
            DataTable dt = new DataTable();
            List<String> myList = new List<String>();
            string WhereCondition = string.Empty;

            myList.Add("StoreMasterStock.Branch = '" + entity.Branch + "' ");
            myList.Add("StoreMasterStock.Barcode = '" + entity.Barcode + "' ");
            myList.Add("StoreMasterStock.SaleStatus=0");
            myList.Add("StoreMasterStock.Quantity >0");
            myList.Add("StoreMasterStock.ActiveStatus =1");

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
            sql = "select distinct StoreMasterStock.PROD_WGPG as PCategoryID,T_WGPG.WGPG_NAME as PCategoryName,StoreMasterStock.PROD_SUBCATEGORY as SubCategoryID,SubCategory.SubCategoryName as SubCategory,StoreMasterStock.PROD_DES as DescriptionID,Description.Description,StoreMasterStock.Barcode,1 Qty,ISNULL(Stock.StockInHand,0)StockInHand from StoreMasterStock inner join Description on StoreMasterStock.PROD_DES=Description.OID inner join SubCategory on Description.SubCategoryID=SubCategory.OID inner join T_WGPG on SubCategory.CategoryID=T_WGPG.OID left join(select StoreMasterStock.Barcode,SUM(StoreMasterStock.Quantity)StockInHand from StoreMasterStock " + WhereCondition + " group by StoreMasterStock.Barcode ) Stock on Stock.Barcode = StoreMasterStock.Barcode " + WhereCondition + " ";
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

        public DataTable txtBarcode_TextChangedDeleteProduct(T_PROD entity)
        {
            DataTable dt = new DataTable();
            List<String> myList = new List<String>();
            string WhereCondition = string.Empty;
            myList.Add("T_WGPG.OID <> 111");
            myList.Add("StoreMasterStock.Barcode = '" + entity.Barcode + "' ");
            myList.Add("StoreMasterStock.SaleStatus=0");
            myList.Add("StoreMasterStock.Quantity >0");
            myList.Add("StoreMasterStock.ActiveStatus =1");

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
            sql = "select StoreMasterStock.OID, T_CCOM.CCOM_NAME,T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,Description.Description,StoreMasterStock.Barcode from StoreMasterStock inner join Description on StoreMasterStock.PROD_DES=Description.OID inner join SubCategory on Description.SubCategoryID=SubCategory.OID inner join T_WGPG on SubCategory.CategoryID=T_WGPG.OID inner join T_CCOM on StoreMasterStock.Branch=T_CCOM.OID " + WhereCondition + " ";            
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


        public DataTable GetStockReturnList(T_PROD entity)
        {
            DataTable dt = new DataTable();
            List<String> myList = new List<String>();
            string WhereCondition = string.Empty;

            if (entity.Branch != String.Empty & entity.Branch != "0")
            {
                myList.Add("StockReturn_MST.FromStoreID = '" + entity.Branch + "' ");
            }
            if (entity.FromDate != String.Empty & entity.ToDate != string.Empty)
            {
                myList.Add("StockReturn_MST.IDAT between '" + entity.FromDate + "' and '"+ entity.ToDate +"' ");
            }
            myList.Add("StockReturn_MST.ApprovedStatus = " + entity.SearchType + " ");

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
            sql = "select StockReturn_MST.StockReturnID,StockReturn_MST.StockReturnNo,(case StockReturn_MST.ApprovedStatus when (0) then 'N' when (1) then 'Y' end)as ApprovedStatus,C1.CCOM_NAME as FromStoreID,C2.CCOM_NAME as ToStoreID,StockReturn_MST.ReferenceBy,StockReturn_MST.IDAT from StockReturn_MST inner join T_CCOM as C2 on StockReturn_MST.ToStoreID=C2.OID inner join T_CCOM as C1 on StockReturn_MST.FromStoreID=C1.OID " + WhereCondition + " ";                                    
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

        public DataTable GetReceiveStockReturnList(T_PROD entity)
        {
            DataTable dt = new DataTable();
            List<String> myList = new List<String>();
            string WhereCondition = string.Empty;

            if (entity.Branch != String.Empty & entity.Branch != "0")
            {
                myList.Add("StockReturn_MST.ToStoreID = '" + entity.Branch + "' ");
            }
            if (entity.FromDate != String.Empty & entity.ToDate != string.Empty)
            {
                myList.Add("StockReturn_MST.IDAT between '" + entity.FromDate + "' and '" + entity.ToDate + "' ");
            }
            myList.Add("StockReturn_MST.ApprovedStatus = " + entity.SearchType+ " ");

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
            sql = "select StockReturn_MST.StockReturnID,StockReturn_MST.StockReturnNo,(case StockReturn_MST.ApprovedStatus when (0) then 'N' when (1) then 'Y' end)as ApprovedStatus,C1.CCOM_NAME as FromStoreID,C2.CCOM_NAME as ToStoreID,StockReturn_MST.ReferenceBy,StockReturn_MST.IDAT from StockReturn_MST inner join T_CCOM as C2 on StockReturn_MST.ToStoreID=C2.OID inner join T_CCOM as C1 on StockReturn_MST.FromStoreID=C1.OID " + WhereCondition + " ";
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

        public DataTable GetReceiveStockReturnListReceived(T_PROD entity)
        {
            DataTable dt = new DataTable();
            List<String> myList = new List<String>();
            string WhereCondition = string.Empty;

            if (entity.Branch != String.Empty & entity.Branch != "0")
            {
                myList.Add("StockReturn_MST.ToStoreID = '" + entity.Branch + "' ");
            }
            if (entity.FromStoreID != String.Empty & entity.FromStoreID != "0")
            {
                myList.Add("StockReturn_MST.FromStoreID = '" + entity.FromStoreID + "' ");
            }
            if (entity.Barcode != String.Empty & entity.Barcode != "0")
            {
                myList.Add("StockReturn_DTL.Barcode = '" + entity.Barcode + "' ");
            }
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
            if (entity.FromDate != String.Empty & entity.ToDate != string.Empty)
            {
                myList.Add("StockReturn_MST.IDAT between '" + entity.FromDate + "' and '" + entity.ToDate + "' ");
            }
            if (entity.FromDate != String.Empty & entity.ToDate != string.Empty)
            {
                myList.Add("StockReturn_MST.EDAT between '" + entity.FromDate + "' and '" + entity.ToDate + "' ");
            }
            myList.Add("StockReturn_MST.ApprovedStatus = " + entity.SearchType + " ");

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
            sql = "select T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,Description.Description,StockReturn_DTL.Barcode,StockReturn_DTL.RQty,(case StockReturn_MST.ApprovedStatus when (0) then 'N' when (1) then 'Y' end)as ApprovedStatus,C1.CCOM_NAME as FromStoreID,C2.CCOM_NAME as ToStoreID,StockReturn_MST.IDAT,StockReturn_MST.EDAT from StockReturn_MST inner join T_CCOM as C2 on StockReturn_MST.ToStoreID=C2.OID inner join T_CCOM as C1 on StockReturn_MST.FromStoreID=C1.OID inner join StockReturn_DTL on StockReturn_DTL.StockReturnID=StockReturn_MST.StockReturnID inner join Description on StockReturn_DTL.DescriptionID=Description.OID inner join SubCategory on Description.SubCategoryID=SubCategory.OID inner join T_WGPG on SubCategory.CategoryID=T_WGPG.OID " + WhereCondition + " order by T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,Description.Description";            
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


        public DataTable StockReturnNotReceivedDetails(T_PROD entity)
        {
            DataTable dt = new DataTable();
            List<String> myList = new List<String>();
            string WhereCondition = string.Empty;

            if (entity.FromStoreID != String.Empty & entity.FromStoreID != "0")
            {
                myList.Add("StockReturn_MST.FromStoreID = '" + entity.FromStoreID + "' ");
            }
            if (entity.Branch != String.Empty & entity.Branch != "0")
            {
                myList.Add("StockReturn_MST.ToStoreID = '" + entity.Branch + "' ");
            }    
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
            if (entity.Barcode != String.Empty & entity.Barcode != "0")
            {
                myList.Add("StockReturn_DTL.Barcode = '" + entity.Barcode + "' ");
            }
            if (entity.FromDate != String.Empty & entity.ToDate != string.Empty)
            {
                myList.Add("StockReturn_MST.IDAT between '" + entity.FromDate + "' and '" + entity.ToDate + "' ");
            }
            myList.Add("StockReturn_MST.ApprovedStatus = " + entity.SearchType + " ");

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
            sql = "select T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,Description.Description,StockReturn_DTL.Barcode,StockReturn_DTL.RQty,(case StockReturn_MST.ApprovedStatus when (0) then 'N' when (1) then 'Y' end)as ApprovedStatus,C1.CCOM_NAME as FromStoreID,C2.CCOM_NAME as ToStoreID,StockReturn_MST.IDAT from StockReturn_MST inner join T_CCOM as C2 on StockReturn_MST.ToStoreID=C2.OID inner join T_CCOM as C1 on StockReturn_MST.FromStoreID=C1.OID inner join StockReturn_DTL on StockReturn_DTL.StockReturnID=StockReturn_MST.StockReturnID inner join Description on StockReturn_DTL.DescriptionID=Description.OID inner join SubCategory on Description.SubCategoryID=SubCategory.OID inner join T_WGPG on SubCategory.CategoryID=T_WGPG.OID " + WhereCondition + " order by T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,Description.Description";
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


        public DataTable StockReturn_Detail(StockReturn_BO entity)
        {
            DataTable dt = new DataTable();
            sql = "select StockReturn_DTL.StockReturnDetailID,StockReturn_MST.StockReturnNo,T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,Description.Description,StockReturn_DTL.Barcode,StockReturn_DTL.RQty from StockReturn_MST inner join StockReturn_DTL on StockReturn_MST.StockReturnID=StockReturn_DTL.StockReturnID inner join Description on StockReturn_DTL.DescriptionID=Description.OID inner join SubCategory on Description.SubCategoryID=SubCategory.OID inner join T_WGPG on SubCategory.CategoryID=T_WGPG.OID where StockReturn_DTL.StockReturnID='" + entity.StockReturnID + "' ";            
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


        public DataTable StockReturn_Detail_ForPreview(StockReturn_BO entity)
        {
            DataTable dt = new DataTable();
            sql = "select StockReturn_MST.StockReturnID,StockReturn_MST.StockReturnNo,(case StockReturn_MST.ApprovedStatus when (0) then 'N' when (1) then 'Y' end)as ApprovedStatus,StockReturn_MST.ReferenceBy,C2.CCOM_NAME as FromStoreID,C1.CCOM_NAME as ToStoreID,StockReturn_MST.IDAT,T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,Description.Description,StockReturn_DTL.Barcode,StockReturn_DTL.RQty from StockReturn_MST inner join T_CCOM as C1 on StockReturn_MST.ToStoreID=C1.OID inner join T_CCOM as C2 on StockReturn_MST.FromStoreID=C2.OID inner join StockReturn_DTL on StockReturn_DTL.StockReturnID=StockReturn_MST.StockReturnID inner join Description on StockReturn_DTL.DescriptionID=Description.OID inner join SubCategory on Description.SubCategoryID=SubCategory.OID inner join T_WGPG on SubCategory.CategoryID=T_WGPG.OID where StockReturn_MST.StockReturnID='" + entity.StockReturnID + "' ";            
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



        public string DeleteStockReturn(StockReturn_BO entity)
        {
            Int32 ApprovedStatus = 0;
            string message = string.Empty;

            sql = "select ApprovedStatus from StockReturn_MST where StockReturnID=" + entity.StockReturnID + " ";
            cmd = new SqlCommand(sql, dbConnect);
            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ApprovedStatus = Convert.ToInt32(reader["ApprovedStatus"].ToString());
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

            if (ApprovedStatus == 1) {
                message = "Can not be Deleted";
            }
            else {
                sql = "delete from StockReturn_DTL where StockReturnID=" + entity.StockReturnID + " ";
                cmd = new SqlCommand(sql, dbConnect);               
                try
                {
                    if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                    cmd.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dbConnect.Close();
                }

                sql = "delete from StockReturn_MST where StockReturnID=" + entity.StockReturnID + " ";
                cmd = new SqlCommand(sql, dbConnect);
                try
                {
                    if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                    cmd.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dbConnect.Close();
                }
                message = "Stock Return Number Deleted Successfully";
            }

            return message;
        }


        public string DeleteStockReturnItem(string StockReturnDetailID)
        {
            string ApprovedStatus = string.Empty;
            string message = string.Empty;
            sql = "select StockReturn_MST.ApprovedStatus from StockReturn_MST inner join StockReturn_DTL on StockReturn_DTL.StockReturnID=StockReturn_MST.StockReturnID where StockReturnDetailID='" + StockReturnDetailID + "' ";
            cmd = new SqlCommand(sql, dbConnect);
            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ApprovedStatus = reader["ApprovedStatus"].ToString();
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

            if (ApprovedStatus == "1")
            {
                message = "This Item can not be deleted because The Stock return Number has been already accepted";
            }
            else
            {
                sql = "delete from StockReturn_DTL where StockReturnDetailID='" + StockReturnDetailID + "' ";
                cmd = new SqlCommand(sql, dbConnect);
                try
                {
                    if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                    cmd.ExecuteScalar();
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
           
            return message;
        }


        public void ReceiveStockReturn(StockReturn_BO entity)
        {
            CostPrice = string.Empty;
            SalePrice = string.Empty;
            OID = string.Empty;
            UpdateStockReturn_DTL(entity);
            if (entity.FaultyProd == "Faulty")
            {
                cmd = new SqlCommand("SPP_T_ProdAddForStockReturnAccessoriesFaulty", dbConnect);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@PROD_WGPG", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_WGPG);
                cmd.Parameters.Add("@PROD_SUBCATEGORY", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_SUBCATEGORY);
                cmd.Parameters.Add("@PROD_DES", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_DES);
                cmd.Parameters.Add("@Barcode", SqlDbType.VarChar, 50).Value = entity.Barcode;
                cmd.Parameters.Add("@FromStoreID", SqlDbType.VarChar, 50).Value = entity.FromStoreID;
                cmd.Parameters.Add("@ToStoreID", SqlDbType.VarChar, 50).Value = entity.ToStoreID;
                cmd.Parameters.Add("@CostPrice", SqlDbType.BigInt).Value = 0;
                cmd.Parameters.Add("@SalePrice", SqlDbType.BigInt).Value = 0;
                cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = Convert.ToInt32(entity.RQty);
                cmd.Parameters.Add("@SaleStatus", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@ActiveStatus", SqlDbType.Int).Value = 1;
                cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 50).Value = entity.IUSER;
                cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = DateTime.Now.Date;
                cmd.Parameters.Add("@TransferDate", SqlDbType.Date).Value = entity.TransferDate;
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
            if(entity.PROD_WGPG != "111") {
                cmd = new SqlCommand("SPP_StoreMasterStockStockReturn", dbConnect);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@PROD_WGPG", SqlDbType.BigInt).Value = entity.PROD_WGPG;
                cmd.Parameters.Add("@PROD_SUBCATEGORY", SqlDbType.BigInt).Value = entity.PROD_SUBCATEGORY;
                cmd.Parameters.Add("@PROD_DES", SqlDbType.BigInt).Value = entity.PROD_DES;
                cmd.Parameters.Add("@Barcode", SqlDbType.VarChar, 100).Value = entity.Barcode;
                cmd.Parameters.Add("@FromStoreID", SqlDbType.VarChar, 50).Value = entity.FromStoreID;
                cmd.Parameters.Add("@ToStoreID", SqlDbType.VarChar, 50).Value = entity.ToStoreID;
                cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 50).Value = entity.IUSER;
                cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = DateTime.Now.Date;
                cmd.Parameters.Add("@TransferDate", SqlDbType.Date).Value = entity.TransferDate;
                try
                {
                    if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                    cmd.ExecuteScalar();
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
            else {
                GetCostAndSalePrice(entity);
                GetCostAndSalePriceAVG(entity);
                if (entity.BarnchText == "Head Office")
                {
                    cmd = new SqlCommand("SPP_T_ProdAddForStockReturnAccessories", dbConnect);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@PROD_WGPG", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_WGPG);
                    cmd.Parameters.Add("@PROD_SUBCATEGORY", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_SUBCATEGORY);
                    cmd.Parameters.Add("@PROD_DES", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_DES);
                    cmd.Parameters.Add("@Barcode", SqlDbType.VarChar, 50).Value = entity.Barcode;
                    cmd.Parameters.Add("@FromStoreID", SqlDbType.VarChar, 50).Value = entity.FromStoreID;
                    cmd.Parameters.Add("@ToStoreID", SqlDbType.VarChar, 50).Value = entity.ToStoreID;
                    cmd.Parameters.Add("@CostPrice", SqlDbType.BigInt).Value = Convert.ToInt64(CostPrice);
                    cmd.Parameters.Add("@Average", SqlDbType.Decimal).Value = Convert.ToDecimal(Average);
                    cmd.Parameters.Add("@SalePrice", SqlDbType.BigInt).Value = Convert.ToInt64(SalePrice);
                    cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = Convert.ToInt32(entity.RQty);
                    cmd.Parameters.Add("@SaleStatus", SqlDbType.Int).Value = 0;
                    cmd.Parameters.Add("@ActiveStatus", SqlDbType.Int).Value = 1;
                    cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 50).Value = entity.IUSER;
                    cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = DateTime.Now.Date;
                    cmd.Parameters.Add("@TransferDate", SqlDbType.Date).Value = entity.TransferDate;
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
                else {
                    GetOID(entity);
                    GetCostAndSalePriceAVG(entity);
                    if (OID == string.Empty)
                    {                        
                        cmd = new SqlCommand("SPP_StoreMasterStockStockReturnAccessories", dbConnect);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PROD_WGPG", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_WGPG);
                        cmd.Parameters.Add("@PROD_SUBCATEGORY", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_SUBCATEGORY);
                        cmd.Parameters.Add("@PROD_DES", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_DES);
                        cmd.Parameters.Add("@Barcode", SqlDbType.VarChar, 50).Value = entity.Barcode;
                        cmd.Parameters.Add("@FromStoreID", SqlDbType.VarChar, 50).Value = entity.FromStoreID;
                        cmd.Parameters.Add("@ToStoreID", SqlDbType.VarChar, 50).Value = entity.ToStoreID;
                        cmd.Parameters.Add("@CostPrice", SqlDbType.BigInt).Value = Convert.ToInt64(CostPrice);
                        cmd.Parameters.Add("@Average", SqlDbType.Decimal).Value = Convert.ToDecimal(Average);
                        cmd.Parameters.Add("@SalePrice", SqlDbType.BigInt).Value = Convert.ToInt64(SalePrice);
                        cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = Convert.ToInt16(entity.RQty);
                        cmd.Parameters.Add("@SaleStatus", SqlDbType.Int).Value = 0;
                        cmd.Parameters.Add("@ActiveStatus", SqlDbType.Int).Value = 1;
                        cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 50).Value = entity.IUSER;
                        cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = DateTime.Now.Date;
                        cmd.Parameters.Add("@TransferDate", SqlDbType.Date).Value = entity.TransferDate;
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
                    else 
                    {
                        
                        cmd = new SqlCommand("SPP_StoreMasterStockStockReturnAccessoriesUpdate", dbConnect);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@OID", SqlDbType.BigInt).Value = OID;
                        cmd.Parameters.Add("@PROD_WGPG", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_WGPG);
                        cmd.Parameters.Add("@PROD_SUBCATEGORY", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_SUBCATEGORY);
                        cmd.Parameters.Add("@PROD_DES", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_DES);
                        cmd.Parameters.Add("@Barcode", SqlDbType.VarChar, 50).Value = entity.Barcode;
                        cmd.Parameters.Add("@FromStoreID", SqlDbType.VarChar, 50).Value = entity.FromStoreID;
                        cmd.Parameters.Add("@ToStoreID", SqlDbType.VarChar, 50).Value = entity.ToStoreID;
                        cmd.Parameters.Add("@CostPrice", SqlDbType.BigInt).Value = Convert.ToInt64(CostPrice);
                        cmd.Parameters.Add("@Average", SqlDbType.Decimal).Value = Convert.ToDecimal(Average);
                        cmd.Parameters.Add("@SalePrice", SqlDbType.BigInt).Value = Convert.ToInt64(SalePrice);
                        cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = Convert.ToInt16(entity.RQty);
                        cmd.Parameters.Add("@SaleStatus", SqlDbType.Int).Value = 0;
                        cmd.Parameters.Add("@ActiveStatus", SqlDbType.Int).Value = 1;
                        cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 50).Value = entity.IUSER;
                        cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = DateTime.Now.Date;
                        cmd.Parameters.Add("@TransferDate", SqlDbType.Date).Value = entity.TransferDate;
                        try
                        {
                            if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                            cmd.ExecuteScalar();
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

        }
        }




        private void UpdateStockReturn_DTL(StockReturn_BO entity)
        {
            sql = "update StockReturn_DTL set ToStoreID=@ToStoreID where StockReturnDetailID=@StockReturnDetailID";
            cmd = new SqlCommand(sql, dbConnect);
            cmd.Parameters.Add("@StockReturnDetailID", SqlDbType.BigInt).Value = entity.StockReturnDetailID;
            cmd.Parameters.Add("@ToStoreID", SqlDbType.VarChar, 100).Value = entity.ToStoreID;
            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                cmd.ExecuteScalar();
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

        private void GetCostAndSalePrice(StockReturn_BO entity)
        {
            sql = "SELECT CostPrice,SalePrice from StoreMasterStock where PROD_WGPG=@PROD_WGPG and PROD_SUBCATEGORY=@PROD_SUBCATEGORY and PROD_DES=@PROD_DES and Barcode=@Barcode and Branch=@Branch";
            cmd = new SqlCommand(sql, dbConnect);
            cmd.Parameters.Add("@PROD_WGPG", SqlDbType.BigInt).Value = entity.PROD_WGPG;
            cmd.Parameters.Add("@PROD_SUBCATEGORY", SqlDbType.BigInt).Value = entity.PROD_SUBCATEGORY;
            cmd.Parameters.Add("@PROD_DES", SqlDbType.BigInt).Value = entity.PROD_DES;
            cmd.Parameters.Add("@Barcode", SqlDbType.VarChar, 100).Value = entity.Barcode;
            cmd.Parameters.Add("@Branch", SqlDbType.VarChar, 50).Value = entity.FromStoreID;
            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    CostPrice = reader["CostPrice"].ToString();
                    SalePrice = reader["SalePrice"].ToString();
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
        private void GetCostAndSalePriceAVG(StockReturn_BO entity)
            {
                sql = "SELECT Average from AccessoriesMain where PROD_WGPG=@PROD_WGPG and PROD_SUBCATEGORY=@PROD_SUBCATEGORY and PROD_DES=@PROD_DES and Barcode=@Barcode";
            cmd = new SqlCommand(sql, dbConnect);
            cmd.Parameters.Add("@PROD_WGPG", SqlDbType.BigInt).Value = entity.PROD_WGPG;
            cmd.Parameters.Add("@PROD_SUBCATEGORY", SqlDbType.BigInt).Value = entity.PROD_SUBCATEGORY;
            cmd.Parameters.Add("@PROD_DES", SqlDbType.BigInt).Value = entity.PROD_DES;
            cmd.Parameters.Add("@Barcode", SqlDbType.VarChar, 100).Value = entity.Barcode;
            cmd.Parameters.Add("@Branch", SqlDbType.VarChar, 50).Value = entity.FromStoreID;
            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Average = reader["Average"].ToString();
                  
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
        private void GetOID(StockReturn_BO entity)
        {
            sql = "SELECT OID from StoreMasterStock where PROD_WGPG=@PROD_WGPG and PROD_SUBCATEGORY=@PROD_SUBCATEGORY and PROD_DES=@PROD_DES and Barcode=@Barcode and Branch=@Branch";
            cmd = new SqlCommand(sql, dbConnect);
            cmd.Parameters.Add("@PROD_WGPG", SqlDbType.BigInt).Value = entity.PROD_WGPG;
            cmd.Parameters.Add("@PROD_SUBCATEGORY", SqlDbType.BigInt).Value = entity.PROD_SUBCATEGORY;
            cmd.Parameters.Add("@PROD_DES", SqlDbType.BigInt).Value = entity.PROD_DES;
            cmd.Parameters.Add("@Barcode", SqlDbType.VarChar, 100).Value = entity.Barcode;
            cmd.Parameters.Add("@Branch", SqlDbType.VarChar, 50).Value = entity.ToStoreID;
            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    OID = reader["OID"].ToString();
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

       


     

        private void InsertStoreAccessoriesHistory(StockReturn_BO entity)
        {
            sql = "insert into StoreAccessoriesHistory(PROD_WGPG,PROD_SUBCATEGORY,PROD_DES,Branch,Barcode,CostPrice,SalePrice,Quantity,IUSER,IDAT)values(@PROD_WGPG,@PROD_SUBCATEGORY,@PROD_DES,@Branch,@Barcode,@CostPrice,@SalePrice,@QUANTITY,@IUSER,@IDAT)";
            cmd = new SqlCommand(sql, dbConnect);
            cmd.Parameters.Add("@PROD_WGPG", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_WGPG);
            cmd.Parameters.Add("@PROD_SUBCATEGORY", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_SUBCATEGORY);
            cmd.Parameters.Add("@PROD_DES", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_DES);
            cmd.Parameters.Add("@Branch", SqlDbType.VarChar, 50).Value = entity.ToStoreID;
            cmd.Parameters.Add("@Barcode", SqlDbType.VarChar, 50).Value = entity.Barcode;
            cmd.Parameters.Add("@CostPrice", SqlDbType.BigInt).Value = Convert.ToInt64(CostPrice);
            cmd.Parameters.Add("@SalePrice", SqlDbType.BigInt).Value = Convert.ToInt64(SalePrice);
            cmd.Parameters.Add("@QUANTITY", SqlDbType.Int).Value = Convert.ToInt32(entity.RQty);
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


        //private void MinusStoreMasterStockQuantity(StockReturn_BO entity)
        //{
        //    sql = "update StoreMasterStock set QUANTITY=QUANTITY - @QUANTITY where PROD_WGPG=@PROD_WGPG and PROD_SUBCATEGORY=@PROD_SUBCATEGORY and PROD_DES=@PROD_DES and Barcode=@Barcode and Branch=@Branch";
        //    cmd = new SqlCommand(sql, dbConnect);
        //    cmd.Parameters.Add("@PROD_WGPG", SqlDbType.BigInt).Value = entity.PROD_WGPG;
        //    cmd.Parameters.Add("@PROD_SUBCATEGORY", SqlDbType.BigInt).Value = entity.PROD_SUBCATEGORY;
        //    cmd.Parameters.Add("@PROD_DES", SqlDbType.BigInt).Value = entity.PROD_DES;
        //    cmd.Parameters.Add("@Barcode", SqlDbType.VarChar, 100).Value = entity.Barcode;
        //    cmd.Parameters.Add("@Branch", SqlDbType.VarChar, 50).Value = entity.FromStoreID;

        //    cmd.Parameters.Add("@QUANTITY", SqlDbType.BigInt).Value = entity.RQty;

        //    try
        //    {
        //        if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
        //        cmd.ExecuteScalar();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        dbConnect.Close();
        //    }
        //}

      




    }
}
