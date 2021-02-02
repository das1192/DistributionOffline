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
    public class StockReportForAdmin_DAL
    {
        SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
        SqlDataAdapter da = new SqlDataAdapter();
        SqlCommand cmd;
        String sql;
        SqlDataReader reader;
        //SqlDataReader reader;

        public DataTable CurrentStockInBranch(T_PROD entity)
        {
            DataTable dt = new DataTable();
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
            if (entity.FromDate != String.Empty & entity.ToDate != string.Empty)
            {
                myList.Add("StoreMasterStock.IDAT between '" + entity.FromDate + "' and '" + entity.ToDate + "'  ");
            }

            myList.Add("StoreMasterStock.Quantity > 0");
            myList.Add("StoreMasterStock.SaleStatus=0 ");
            myList.Add("StoreMasterStock.ActiveStatus=1 ");

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


            sql = "select T_CCOM.CCOM_NAME as Branch,T_WGPG.WGPG_NAME as PROD_WGPG,SubCategory.SubCategoryName as PROD_SUBCATEGORY,Description.Description,sum(StoreMasterStock.Quantity)as Quantity from StoreMasterStock inner join Description on StoreMasterStock.PROD_DES=Description.OID inner join SubCategory on Description.SubCategoryID=SubCategory.OID inner join T_WGPG on SubCategory.CategoryID=T_WGPG.OID inner join T_CCOM on StoreMasterStock.Branch=T_CCOM.OID " + WhereCondition + " group by T_CCOM.CCOM_NAME,T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,Description.Description order by T_CCOM.CCOM_NAME,T_WGPG.WGPG_NAME,SubCategory.SubCategoryName  ";
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

        public T_PROD GetProductInformation(String OID)
        {
            T_PROD entity = new T_PROD();
            sql = "select StoreMasterStock.PROD_WGPG,StoreMasterStock.PROD_SUBCATEGORY,StoreMasterStock.SaleQuantity,StoreMasterStock.PROD_DES from StoreMasterStock where StoreMasterStock.OID=" + OID + " ";
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
                    if (string.IsNullOrEmpty(reader["SaleQuantity"].ToString()))
                    {
                        entity.SaleQuantity = string.Empty;
                    }
                    else
                    {
                        entity.SaleQuantity = reader["SaleQuantity"].ToString();
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

        public void AdjustStoreMasterStock(T_PROD entity)
        {
            //
            sql = "update Acc_Stock set Quantity=Quantity-@Quantity,MQty=MQty+@Quantity where ACC_STOCKID=@OID";
                cmd = new SqlCommand(sql, dbConnect);

                cmd.Parameters.Add("@OID", SqlDbType.Int).Value = Convert.ToInt32(entity.ACCOID);
                cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = Convert.ToInt32(entity.Quantity);
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

            // Insert Into Stock Details//////////////

            string InsertSTD = string.Format(@"
Insert into Acc_StockDetail 
(Po_Number, PROD_WGPG, PROD_SUBCATEGORY, PROD_DES, Branch
, Quantity, CostPrice, Total, Flag
, Remarks, IDAT, IUSER, IDATTIME)

values('{0}',{1},{2},{3},'{4}'   
,{5},{6},{7},'{8}'   
,'{9}','{10}','{11}','{12}')

", "PO-MISSING"
 , entity.PROD_WGPG, entity.PROD_SUBCATEGORY, entity.PROD_DES, entity.Branch

 , entity.Quantity, entity.CostPrice 
 , (Convert.ToInt32(entity.Quantity) * Convert.ToInt32(entity.CostPrice)), "Product Missing"

 , "Product Missing", DateTime.Now, entity.IUSER, DateTime.Now.ToString()
 );
            SqlCommand cmdInsertSTD = new SqlCommand(InsertSTD, dbConnect);


            /////////////////////-------------/////////////
            //
            sql = "update StoreMasterStock set Quantity=Quantity-@Quantity where OID=@OID";
            cmd = new SqlCommand(sql, dbConnect);

            cmd.Parameters.Add("@OID", SqlDbType.Int).Value = Convert.ToInt32(entity.OID);
            cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = Convert.ToInt32(entity.Quantity);
            cmd.Parameters.Add("@SaleQuantity", SqlDbType.Int).Value = Convert.ToInt32(entity.SaleQuantity);


            try
            {
                if (dbConnect.State == ConnectionState.Closed)
                    dbConnect.Open();

                cmdInsertSTD.ExecuteNonQuery();
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

            //
            sql = "INSERT INTO Stock_Adjust(PROD_WGPG,PROD_SUBCATEGORY,PROD_DES,Branch,OldQuantity,NewQuantity,RunningAvg,Rel_ID,IUSER,IDAT) VALUES(@PROD_WGPG,@PROD_SUBCATEGORY,@PROD_DES,@Branch,@OldQuantity,@NewQuantity,@RunningAvg,@Rel_ID,@IUSER,@IDAT)";
            cmd = new SqlCommand(sql, dbConnect);


            cmd.Parameters.Add("@PROD_WGPG", SqlDbType.BigInt).Value = Convert.ToInt32(entity.PROD_WGPG);
            cmd.Parameters.Add("@PROD_SUBCATEGORY", SqlDbType.BigInt).Value = Convert.ToInt32(entity.PROD_SUBCATEGORY);
            cmd.Parameters.Add("@PROD_DES", SqlDbType.BigInt).Value = Convert.ToInt32(entity.PROD_DES);
            cmd.Parameters.Add("@Branch", SqlDbType.VarChar, 50).Value = entity.Branch;           
            cmd.Parameters.Add("@OldQuantity", SqlDbType.BigInt).Value = Convert.ToInt64(entity.OLDQUANTITY);
            cmd.Parameters.Add("@NewQuantity", SqlDbType.Int).Value = Convert.ToInt32(entity.Quantity);
            //sadiq RunningAvg 170920
            cmd.Parameters.Add("@RunningAvg", SqlDbType.Decimal).Value = Convert.ToDecimal(entity.RunningAvg);
            cmd.Parameters.Add("@Rel_ID", SqlDbType.Int).Value = Convert.ToInt32(entity.OID);
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


            //=================================journal

            int amount  = (Convert.ToInt32(entity.Quantity) * Convert.ToInt32(entity.CostPrice));

            string sqlTPRODUpdate = string.Empty;
            if(entity.STOCK_TYPE=="Barcode")
            {
                amount = (1 * Convert.ToInt32(entity.CostPrice));

                sqlTPRODUpdate = string.Format(@"
update T_PROD set SaleStatus='5' 
where SaleStatus='0' and Branch='{0}' and PROD_WGPG='{1}' and PROD_SUBCATEGORY='{2}' and PROD_DES='{3}' and Barcode='{4}'
",entity.Branch,entity.PROD_WGPG,entity.PROD_SUBCATEGORY,entity.PROD_DES,entity.Barcode);
            }

            // insert in DailyCost
            string sqlDailyCost =string.Format(@"
declare @IDProductMissing NVARCHAR(10)
select @IDProductMissing=(select ch.OID from CostingHead ch where ch.CostingHead='Product Missing' and ch.Shop_id={0})
--select @IDProductMissing

insert into DailyCost(Shop_id,CostingHeadID,AMOUNT,IUSER,IDAT,Remarks) 
values ({0},@IDProductMissing,{1},'{2}','{3}','{4}')
", entity.Branch, amount
 , entity.IUSER, DateTime.Now.Date, "Loss for Missing Product");



            //journal
            string InsertDR = string.Format(@"
declare @IDProductMissing NVARCHAR(10)
select @IDProductMissing=(select ch.OID from CostingHead ch where ch.CostingHead='Product Missing' and ch.Shop_id={0})
--select @IDProductMissing

INSERT INTO Acc_Journal(AccountID,Branch,Particular,Remarks,Debit,Credit,RefferenceNumber,IDAT,IDATTIME,Narration) 
VALUES(@IDProductMissing,'{0}','{1}','{2}',{3}
,{4},'{5}','{6}','{7}','{8}')

", entity.Branch
, "Expense", "Expense"
, amount

, 0
, "Ref-Expense"
, DateTime.Now.ToString(), DateTime.Now.ToString(), string.Format(@"Loss for Missing Product")
);


            string InsertCR = string.Format(@"
INSERT INTO Acc_Journal(AccountID,Branch,Particular,Remarks,Debit,Credit,RefferenceNumber,IDAT,IDATTIME,Narration) 
VALUES({0},{1},'{2}','{3}',{4}
,{5},'{6}','{7}','{8}','{9}')
", entity.PROD_DES
, entity.Branch
, "Purchase"
, "Product"
, 0

, amount
, "Ref-Expense"
, DateTime.Now.ToString()
, DateTime.Now.ToString()
, string.Format(@"Loss for Missing Product"));


            try
            {
                //SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                SqlCommand cmdDR = new SqlCommand(InsertDR, dbConnect);
                SqlCommand cmdCR = new SqlCommand(InsertCR, dbConnect);
                SqlCommand cmdsDailyCost = new SqlCommand(sqlDailyCost, dbConnect);
                SqlCommand cmdTPRODUpdate = new SqlCommand(sqlTPRODUpdate, dbConnect);


                if (!string.IsNullOrEmpty(entity.Barcode)) { cmdTPRODUpdate.ExecuteNonQuery(); }
                cmdsDailyCost.ExecuteNonQuery();
                cmdDR.ExecuteNonQuery();
                cmdCR.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //dbConnect.Close();
            }



        }

        public DataTable TotalStock(T_PROD entity)
        {
            DataTable dt = new DataTable();
            List<String> myList = new List<String>();
            string WhereCondition = string.Empty;

            
            //if (entity.PROD_WGPG != String.Empty & entity.PROD_WGPG != "0")
            //{
            //    myList.Add("T_WGPG.OID = " + entity.PROD_WGPG + " ");
            //}
            //if (entity.PROD_SUBCATEGORY != String.Empty & entity.PROD_SUBCATEGORY != "0")
            //{
            //    myList.Add("SubCategory.OID = " + entity.PROD_SUBCATEGORY + " ");
            //}
            //if (entity.PROD_DES != String.Empty & entity.PROD_DES != "0")
            //{
            //    myList.Add("Description.OID = " + entity.PROD_DES + " ");
            //}

            //if (entity.Branch != String.Empty & entity.Branch != "0")
            //{
            //    myList.Add("StoreMasterStock.Branch = " + entity.Branch + " ");
            //}

            if (entity.PROD_WGPG != String.Empty & entity.PROD_WGPG != "0")
            {
                myList.Add("c.OID = " + entity.PROD_WGPG + " ");
            }
            if (entity.PROD_SUBCATEGORY != String.Empty & entity.PROD_SUBCATEGORY != "0")
            {
                myList.Add("sc.OID = " + entity.PROD_SUBCATEGORY + " ");
            }
            if (entity.PROD_DES != String.Empty & entity.PROD_DES != "0")
            {
                myList.Add("d.OID = " + entity.PROD_DES + " ");
            }

            if (entity.Branch != String.Empty & entity.Branch != "0")
            {
                myList.Add("s.Branch = " + entity.Branch + " ");
            }
            //where c.OID = 210  and sc.OID = 553  and s.Branch = 20 

            string[] myArray = myList.ToArray();
            string where1 = string.Join(" and ", myArray);

            if (where1 == string.Empty)
            {
                WhereCondition = string.Empty;
            }
            else
            {
                WhereCondition = "where " + where1 + " AND s.Quantity>0";
            }

           
                //sql = "select StoreMasterStock.OID,T_WGPG.WGPG_NAME as PROD_WGPG,SubCategory.SubCategoryName as PROD_SUBCATEGORY,Description.Description,(StoreMasterStock.Quantity-StoreMasterStock.SaleQuantity) as Quantity from StoreMasterStock inner join Description on StoreMasterStock.PROD_DES=Description.OID inner join SubCategory on Description.SubCategoryID=SubCategory.OID inner join T_WGPG on SubCategory.CategoryID=T_WGPG.OID " + WhereCondition + " group by StoreMasterStock.OID,T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,Description.Description,StoreMasterStock.SaleQuantity,StoreMasterStock.Quantity order by T_WGPG.WGPG_NAME,SubCategory.SubCategoryName  ";

                sql = string.Format(@"

select sms.OID AS SMSOID,s.ACC_STOCKID as ACCOID ,c.WGPG_NAME as PROD_WGPG
,sc.SubCategoryName as PROD_SUBCATEGORY
,d.Description,(s.Quantity) as Quantity ,s.CostPrice ,
c.OID as CategoryID,sc.OID as SubCategoryID,d.OID AS DescriptionID,s.Branch

,AVERAGE = (select 

convert(decimal(18,2),
(SUM(Quantity * CostPrice ))/convert(decimal(18,2),(SUM(Quantity)))
)

from Acc_Stock where Quantity >0 AND Acc_Stock.PROD_DES = d.OID group by Acc_Stock.PROD_DES
)
,s.Flag
from Acc_Stock s 
inner join Description d on s.PROD_DES=d.OID 
inner join StoreMasterStock sms on sms.PROD_DES= d.OID 
inner join SubCategory sc on d.SubCategoryID=sc.OID 
inner join T_WGPG c on sc.CategoryID=c.OID 

 {0}  --where c.OID = 210  and sc.OID = 553  and s.Branch = 20 
 
 group by sms.OID,s.Flag,s.ACC_STOCKID,c.WGPG_NAME,sc.SubCategoryName,d.Description,s.Quantity,s.CostPrice,c.OID,sc.OID,d.OID,s.Branch
--group by s.ACC_STOCKID,c.WGPG_NAME,sc.SubCategoryName,d.Description,s.Quantity ,s.CostPrice,c.OID,sc.OID,d.OID,s.Branch
order by s.Flag, c.WGPG_NAME,sc.SubCategoryName 
", WhereCondition);

            
//    select s.OID,c.WGPG_NAME as PROD_WGPG,sc.SubCategoryName as PROD_SUBCATEGORY
//,d.Description,(s.Quantity-s.SaleQuantity) as Quantity 
//,c.OID as CategoryID,sc.OID as SubCategoryID,d.OID AS DescriptionID,s.Branch,sva.AVERAGE
//from StoreMasterStock s 
//inner join Description d on s.PROD_DES=d.OID 
//inner join SubCategory sc on d.SubCategoryID=sc.OID 
//inner join T_WGPG c on sc.CategoryID=c.OID 
//left join STOCKAVERAGEVALUE sva 
//on sva.PROD_DES=d.OID and sva.PROD_SUBCATEGORY=sc.OID and sva.PROD_WGPG=c.OID
//and sva.Branch=s.Branch
// {0}  --where c.OID = 210  and sc.OID = 553  and s.Branch = 20   
//group by s.OID,c.WGPG_NAME,sc.SubCategoryName,d.Description,s.SaleQuantity,s.Quantity ,c.OID,sc.OID,d.OID,s.Branch,sva.AVERAGE
//order by c.WGPG_NAME,sc.SubCategoryName          
            
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
        public DataTable TotalStockQuantity(T_PROD entity)
        {
            DataTable dt = new DataTable();
            List<String> myList = new List<String>();
            string WhereCondition = string.Empty;


            if (entity.PROD_WGPG != String.Empty & entity.PROD_WGPG != "0")
            {
                myList.Add("c.OID = " + entity.PROD_WGPG + " ");
            }
            if (entity.PROD_SUBCATEGORY != String.Empty & entity.PROD_SUBCATEGORY != "0")
            {
                myList.Add("sc.OID = " + entity.PROD_SUBCATEGORY + " ");
            }
            if (entity.PROD_DES != String.Empty & entity.PROD_DES != "0")
            {
                myList.Add("d.OID = " + entity.PROD_DES + " ");
            }

            if (entity.Branch != String.Empty & entity.Branch != "0")
            {
                myList.Add("s.Branch = " + entity.Branch + " ");
            }
            //where c.OID = 210  and sc.OID = 553  and s.Branch = 20 

            string[] myArray = myList.ToArray();
            string where1 = string.Join(" and ", myArray);

            if (where1 == string.Empty)
            {
                WhereCondition = string.Empty;
            }
            else
            {
                WhereCondition = where1 + " AND s.Quantity>0";
            }
            if(entity.STOCK_TYPE=="Quantity")
            {
sql = string.Format(@"

select TPRODID='', sms.OID AS SMSOID,s.ACC_STOCKID as ACCOID ,c.WGPG_NAME as PROD_WGPG
,sc.SubCategoryName as PROD_SUBCATEGORY
,d.Description,'' as Barcode
,s.Quantity as Quantity
,s.CostPrice 
,c.OID as CategoryID,sc.OID as SubCategoryID,d.OID AS DescriptionID,s.Branch

,AVERAGE = 
(select convert(decimal(18,2),(SUM(Quantity * CostPrice ))/convert(decimal(18,2),(SUM(Quantity))))
from Acc_Stock where Flag='Quantity' and Quantity >0 AND Acc_Stock.PROD_DES = d.OID group by Acc_Stock.PROD_DES
)

from Acc_Stock s 
inner join Description d on s.PROD_DES=d.OID 
inner join StoreMasterStock sms on sms.PROD_DES= d.OID 
inner join SubCategory sc on d.SubCategoryID=sc.OID 
inner join T_WGPG c on sc.CategoryID=c.OID 

where s.Flag='Quantity' and s.Quantity>0 and {0} 
 
group by sms.OID,s.ACC_STOCKID,c.WGPG_NAME,sc.SubCategoryName,d.Description
,s.Quantity,s.CostPrice,c.OID,sc.OID,d.OID,s.Branch
order by s.ACC_STOCKID
", WhereCondition);
            }
            if (entity.STOCK_TYPE == "Barcode")
            {
                sql = string.Format(@"
select s.OID TPRODID,sms.OID AS SMSOID,acc.ACC_STOCKID  as ACCOID ,c.WGPG_NAME as PROD_WGPG
,sc.SubCategoryName as PROD_SUBCATEGORY
,(s.Barcode+' : '+d.Description) as Description,s.Barcode
,s.Quantity
,s.CostPrice ,
c.OID as CategoryID,sc.OID as SubCategoryID,d.OID AS DescriptionID,s.Branch

,AVERAGE = s.CostPrice

from T_PROD s 
inner join Description d on s.PROD_DES=d.OID 
inner join StoreMasterStock sms on sms.PROD_DES= d.OID 
inner join SubCategory sc on d.SubCategoryID=sc.OID 
inner join T_WGPG c on sc.CategoryID=c.OID 
inner join Acc_Stock acc on acc.PROD_DES =s.PROD_DES AND acc.CostPrice = s.CostPrice and ISNULL(acc.Flag,'') ='' and acc.Quantity>0

where s.SaleStatus=0 AND {0} 
 
 group by s.OID,sms.OID,c.WGPG_NAME,sc.SubCategoryName,d.Description,s.Quantity,s.CostPrice
 ,c.OID,sc.OID,d.OID,s.Branch,s.Barcode,acc.ACC_STOCKID 
order by acc.ACC_STOCKID
", WhereCondition);
            }
            


            //    select s.OID,c.WGPG_NAME as PROD_WGPG,sc.SubCategoryName as PROD_SUBCATEGORY
            //,d.Description,(s.Quantity-s.SaleQuantity) as Quantity 
            //,c.OID as CategoryID,sc.OID as SubCategoryID,d.OID AS DescriptionID,s.Branch,sva.AVERAGE
            //from StoreMasterStock s 
            //inner join Description d on s.PROD_DES=d.OID 
            //inner join SubCategory sc on d.SubCategoryID=sc.OID 
            //inner join T_WGPG c on sc.CategoryID=c.OID 
            //left join STOCKAVERAGEVALUE sva 
            //on sva.PROD_DES=d.OID and sva.PROD_SUBCATEGORY=sc.OID and sva.PROD_WGPG=c.OID
            //and sva.Branch=s.Branch
            // {0}  --where c.OID = 210  and sc.OID = 553  and s.Branch = 20   
            //group by s.OID,c.WGPG_NAME,sc.SubCategoryName,d.Description,s.SaleQuantity,s.Quantity ,c.OID,sc.OID,d.OID,s.Branch,sva.AVERAGE
            //order by c.WGPG_NAME,sc.SubCategoryName          

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
        public DataTable TotalFaultyStock(T_PROD entity)
        {
            DataTable dt = new DataTable();
            List<String> myList = new List<String>();
            string WhereCondition = string.Empty;
            if (entity.FromDate != String.Empty & entity.ToDate != string.Empty)
            {
                myList.Add("T_PROD_Faulty.IDAT between '" + entity.FromDate + "' and '" + entity.ToDate + "' ");
            }

            myList.Add("T_PROD_Faulty.Quantity > 0");
            myList.Add("T_PROD_Faulty.SaleStatus=0 ");
            

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

            sql = "select T_CCOM.CCOM_NAME as Branch,T_WGPG.WGPG_NAME as PROD_WGPG,SubCategory.SubCategoryName as PROD_SUBCATEGORY,Description.Description,sum(T_PROD_Faulty.Quantity)as Quantity,CONVERT(VARCHAR(10),T_PROD_Faulty.IDAT,103) AS IDAT from T_PROD_Faulty inner join Description on T_PROD_Faulty.PROD_DES=Description.OID inner join SubCategory on Description.SubCategoryID=SubCategory.OID inner join T_WGPG on SubCategory.CategoryID=T_WGPG.OID  inner join T_CCOM on T_PROD_Faulty.FromBranch=T_CCOM.OID " + WhereCondition + " group by T_PROD_Faulty.IDAT,T_CCOM.CCOM_NAME,T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,Description.Description order by T_WGPG.WGPG_NAME,SubCategory.SubCategoryName";
            

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
        public DataTable CurrentStockByBranch(T_PROD entity)
        {
            DataTable dt = new DataTable();
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
            if (entity.FromDate != String.Empty & entity.ToDate != string.Empty)
            {
                myList.Add("StoreMasterStock.IDAT between '" + entity.FromDate + "' and '" + entity.ToDate + "' ");
            }

            myList.Add("StoreMasterStock.Quantity > 0");
            myList.Add("StoreMasterStock.SaleStatus=0 ");
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
            sql = "select T_CCOM.CCOM_NAME,T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,Description.Description,StoreMasterStock.Barcode,StoreMasterStock.SalePrice,StoreMasterStock.Quantity,CONVERT(VARCHAR(10),StoreMasterStock.IDAT,103) AS IDAT,DATEDIFF(DAY, StoreMasterStock.IDAT, GETDATE()) AS Ageing from StoreMasterStock inner join Description on StoreMasterStock.PROD_DES=Description.OID inner join SubCategory on Description.SubCategoryID=SubCategory.OID inner join T_WGPG on SubCategory.CategoryID=T_WGPG.OID inner join T_CCOM on StoreMasterStock.Branch=T_CCOM.OID " + WhereCondition + " order by T_CCOM.CCOM_NAME,T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,Description.Description,DATEDIFF(DAY, StoreMasterStock.IDAT, GETDATE()) desc ";            
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
        public DataTable CurrentStockValue(T_PROD entity)
        {
            DataTable dt = new DataTable();
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
            sql = "select StoreMasterStock.OID,T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,Description.Description,(StoreMasterStock.Quantity-StoreMasterStock.SaleQuantity) as Quantity,STOCKAVERAGEVALUE.AVERAGE,CONVERT(INT,ISNULL((STOCKAVERAGEVALUE.TOTALAVAILQUANTITY*STOCKAVERAGEVALUE.AVERAGE),'0')) as Stockvalue from StoreMasterStock inner join Description on StoreMasterStock.PROD_DES=Description.OID inner join SubCategory on Description.SubCategoryID=SubCategory.OID inner join T_WGPG on SubCategory.CategoryID=T_WGPG.OID inner join STOCKAVERAGEVALUE on StoreMasterStock.PROD_DES = STOCKAVERAGEVALUE.PROD_DES " + WhereCondition + " order by T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,Description.Description,StoreMasterStock.OID desc ";
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

        public DataTable StockQuantitySummary(T_PROD entity)
        {
            List<String> myList = new List<String>();
            string WhereCondition = string.Empty;
            string where = string.Empty;
            string[] myArray;           


            //Get Color
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
            myList.Add("Description.Active='1'");
            myList.Add("T_WGPG.OID <> 111");

            myArray = myList.ToArray();
            where = string.Join(" and ", myArray);

            if (where == string.Empty)
            {
                WhereCondition = string.Empty;
            }
            else
            {
                WhereCondition = "where " + where + " ";
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("Model", typeof(string));            
            DataRow workRow;

            String sql = "SELECT distinct Description.Description FROM Description inner join SubCategory on Description.SubCategoryID=SubCategory.OID inner join T_WGPG on SubCategory.CategoryID=T_WGPG.OID " + WhereCondition + " ";            
            DataTable dt1 = getDataTable(sql);
            foreach (DataRow row in dt1.Rows)
            {
                dt.Columns.Add(row["Description"].ToString(), typeof(string));
            }
            dt.Columns.Add("Total", typeof(Int32));



            //Get Model
            WhereCondition = string.Empty;
            where = string.Empty;            
            myArray = null;
            myList = null;
            myList = new List<String>();
            if (entity.PROD_WGPG != String.Empty & entity.PROD_WGPG != "0")
            {
                myList.Add("T_WGPG.OID = " + entity.PROD_WGPG + " ");
            }
            if (entity.PROD_SUBCATEGORY != String.Empty & entity.PROD_SUBCATEGORY != "0")
            {
                myList.Add("SubCategory.OID = " + entity.PROD_SUBCATEGORY + " ");
            }                       
            myList.Add("SubCategory.Active='1'");
            myList.Add("T_WGPG.OID <> 111 ");

            myArray = myList.ToArray();
            where = string.Join(" and ", myArray);

            if (where == string.Empty)
            {
                WhereCondition = string.Empty;
            }
            else
            {
                WhereCondition = "where " + where + " ";
            }

            sql = "select SubCategory.OID,SubCategory.SubCategoryName from SubCategory inner join T_WGPG on SubCategory.CategoryID=T_WGPG.OID " + WhereCondition + " order by SubCategory.SubCategoryName";
            DataTable dt2 = getDataTable(sql);





            
            foreach (DataRow row2 in dt2.Rows)
            {
                workRow = dt.NewRow();
                workRow["Total"] = 0;
                foreach (DataRow row1 in dt1.Rows)
                {
                    sql = "select SUM(StoreMasterStock.Quantity)as Total from StoreMasterStock inner join Description on StoreMasterStock.PROD_DES=Description.OID inner join SubCategory on Description.SubCategoryID=SubCategory.OID inner join T_WGPG on SubCategory.CategoryID=T_WGPG.OID where StoreMasterStock.PROD_SUBCATEGORY=" + row2["OID"].ToString() + " and Description.Description='" + row1["Description"].ToString() + "' and StoreMasterStock.SaleStatus=0 and StoreMasterStock.Quantity >0 and StoreMasterStock.ActiveStatus=1 and StoreMasterStock.Branch='" + entity.Branch + "' ";                    
                    DataTable dt3 = getDataTable(sql);

                    if (dt3 != null)
                    {
                        if (dt3.Rows.Count > 0)
                        {                            
                            workRow["Model"] = row2["SubCategoryName"].ToString();
                            if (string.IsNullOrEmpty(dt3.Rows[0]["Total"].ToString()))
                            {
                                workRow[row1["Description"].ToString()] = "0";
                            }
                            else
                            {
                                workRow[row1["Description"].ToString()] = dt3.Rows[0]["Total"].ToString();
                            }
                            workRow["Total"] = Convert.ToInt32(workRow["Total"]) + Convert.ToInt32(workRow[row1["Description"].ToString()]);
                        }
                        else
                        {                            
                            workRow["Model"] = row2["SubCategoryName"].ToString();
                            workRow[row1["Description"].ToString()] = "0";
                            workRow["Total"] = Convert.ToInt32(workRow["Total"]) + Convert.ToInt32(workRow[row1["Description"].ToString()]);
                        }
                    }
                    else if (dt3 == null)
                    {                        
                        workRow["Model"] = row2["SubCategoryName"].ToString();
                        workRow[row1["Description"].ToString()] = "0";
                        workRow["Total"] = Convert.ToInt32(workRow["Total"]) + Convert.ToInt32(workRow[row1["Description"].ToString()]);
                    }
                }

                if (Convert.ToInt32(workRow["Total"]) != 0)
                {
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
            da.Fill(dt);
            return dt;
        }

        public DataTable StockAdjustSearch(T_PROD entity)
        {
            DataTable dt = new DataTable();
            List<String> myList = new List<String>();
            string WhereCondition = string.Empty;

            if (entity.Branch != String.Empty & entity.Branch != "0")
            {
                myList.Add("Stock_Adjust.Branch = '" + entity.Branch + "' ");
            }
            if (entity.PROD_WGPG.ToString() != String.Empty & entity.PROD_WGPG.ToString() != "0")
            {
                myList.Add("Stock_Adjust.PROD_WGPG = " + entity.PROD_WGPG + " ");
            }
            if (entity.PROD_SUBCATEGORY.ToString() != String.Empty & entity.PROD_SUBCATEGORY.ToString() != "0")
            {
                myList.Add("Stock_Adjust.PROD_SUBCATEGORY = " + entity.PROD_SUBCATEGORY + " ");
            }
            if (entity.PROD_DES.ToString() != String.Empty & entity.PROD_DES.ToString() != "0")
            {
                myList.Add("Stock_Adjust.PROD_DES = " + entity.PROD_DES + " ");
            }

            if (entity.FromDate != String.Empty & entity.ToDate != String.Empty)
            {
                myList.Add("Stock_Adjust.IDAT between '" + entity.FromDate + "' and '" + entity.ToDate + "' ");
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

            if (entity.SearchType == "Details")
            {
                sql = "select Stock_Adjust.OID,Stock_Adjust.Branch,ShopInfo.ShopName,T_WGPG.WGPG_NAME as PROD_WGPG,SubCategory.SubCategoryName as PROD_SUBCATEGORY,Description.Description,Stock_Adjust.OldQuantity,Stock_Adjust.NewQuantity,CONVERT(VARCHAR(10),Stock_Adjust.IDAT,103) AS IDAT from Stock_Adjust left join Description on Stock_Adjust.PROD_DES=Description.OID left join SubCategory on Description.SubCategoryID=SubCategory.OID left join T_WGPG on SubCategory.CategoryID=T_WGPG.OID left join ShopInfo on Stock_Adjust.Branch=ShopInfo.OID " + WhereCondition + "  order by Stock_Adjust.Branch,T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,Description.Description,Stock_Adjust.OID desc";
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

        public DataTable GetDetailData(T_PROD entity)
        {
            DataTable dt = new DataTable();
            List<String> myList = new List<String>();
            string WhereCondition = string.Empty;

            if (entity.Branch != String.Empty & entity.Branch != "0")
            {
                myList.Add("StoreMasterStock.Branch = '" + entity.Branch + "' ");
            }

            if (entity.PROD_WGPG != String.Empty & entity.PROD_WGPG != "0")
            {
                myList.Add("StoreMasterStock.PROD_WGPG = '" + entity.PROD_WGPG + "' ");
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


            sql = "select SubCategory.SubCategoryName,Description.Description,SUM(StoreMasterStock.Quantity-StoreMasterStock.SaleQuantity) as sum1 from StoreMasterStock inner join Description on StoreMasterStock.PROD_DES=Description.OID inner join SubCategory on Description.SubCategoryID=SubCategory.OID inner join T_WGPG on SubCategory.CategoryID=T_WGPG.OID " + WhereCondition + " group by SubCategory.SubCategoryName,Description.Description";
            

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

        public DataTable AccessoriesStockInHistory(T_PROD entity)
        {
            DataTable dt = new DataTable();
            List<String> myList = new List<String>();
            string WhereCondition = string.Empty;

            if (entity.Branch != String.Empty & entity.Branch != "0")
            {
                myList.Add("StoreAccessoriesHistory.Branch = '" + entity.Branch + "' ");
            }
            if (entity.PROD_WGPG != String.Empty & entity.PROD_WGPG != "0")
            {
                myList.Add("StoreAccessoriesHistory.PROD_WGPG = " + entity.PROD_WGPG + " ");
            }
            if (entity.PROD_SUBCATEGORY != String.Empty & entity.PROD_SUBCATEGORY != "0")
            {
                myList.Add("StoreAccessoriesHistory.PROD_SUBCATEGORY = " + entity.PROD_SUBCATEGORY + " ");
            }
            if (entity.PROD_DES != String.Empty & entity.PROD_DES != "0")
            {
                myList.Add("StoreAccessoriesHistory.PROD_DES = " + entity.PROD_DES + " ");
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
            sql = "select T_CCOM.CCOM_NAME,T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,Description.Description,StoreAccessoriesHistory.Barcode,StoreAccessoriesHistory.Quantity,CONVERT(VARCHAR(10),StoreAccessoriesHistory.IDAT,103) AS IDAT from StoreAccessoriesHistory inner join Description on StoreAccessoriesHistory.PROD_DES=Description.OID inner join SubCategory on Description.SubCategoryID=SubCategory.OID inner join T_WGPG on SubCategory.CategoryID=T_WGPG.OID inner join T_CCOM on StoreAccessoriesHistory.Branch=T_CCOM.OID " + WhereCondition + " order by T_CCOM.CCOM_NAME,T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,Description.Description ";            
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



        public DataTable DailyClosingBalance(T_PROD entity)
        {
            DataTable dt = new DataTable();
            List<String> myList = new List<String>();
            string WhereCondition = string.Empty;

            if (entity.Branch != String.Empty & entity.Branch != "0")
            {
                myList.Add("T_CCOM.OID = '" + entity.Branch + "' ");
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
            if (entity.FromDate != String.Empty)
            {
                myList.Add("ClosingBalance.IDAT = '" + entity.FromDate + "' ");
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
            sql = "SELECT T_CCOM.CCOM_NAME,T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,Description.Description,CONVERT(int,SUM(ClosingBalance.Quantity)) Quantity,CONVERT(int,SUM(ClosingBalance.CostPrice)) as Closing from ClosingBalance inner join Description on ClosingBalance.DescriptionOID=Description.OID inner join SubCategory on Description.SubCategoryID=SubCategory.OID inner join T_WGPG on SubCategory.CategoryID=T_WGPG.OID inner join T_CCOM on ClosingBalance.T_CCOMOID=T_CCOM.OID " + WhereCondition + " group by T_CCOM.CCOM_NAME,T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,Description.Description order by T_CCOM.CCOM_NAME,T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,Description.Description ";
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
