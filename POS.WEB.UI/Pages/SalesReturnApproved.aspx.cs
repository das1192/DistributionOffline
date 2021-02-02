using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

public partial class Pages_SalesReturnApproved : System.Web.UI.Page
{
    private string userID = string.Empty;
    private string userPassword = string.Empty;
    SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
    SqlCommand cmd;
    SqlDataAdapter da = new SqlDataAdapter();
    string sql;
    string Shop_id;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            userID = Session["UserID"].ToString();
            userPassword = Session["Password"].ToString();
            Shop_id = Session["StoreID"].ToString();
        }
        catch
        {
            Response.Redirect("~/frmLogin.aspx");
        }
        bool isAuthenticate = CommonBinder.CheckPageAuthentication(System.Web.HttpContext.Current.Request.Url.AbsolutePath, userID);

        if (!isAuthenticate)
        {
            Response.Redirect("~/UnAuthorizedUser.aspx");
        }
        if (!Page.IsPostBack)
        {
            loadInvoiceNo();
            Label1.Text = string.Empty;
        }
    }



    protected void ddlInvoiceNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblCustomerName.Text = string.Empty;
        lblCustomerID.Text = string.Empty;
        lblPaymentMode.Text = string.Empty;
        lblPaymentModeID.Text = string.Empty;
        DataTable dt = new DataTable();

        if(ddlInvoiceNo.SelectedValue.ToString() !="0")
        {
        sql = string.Format(@"
select 
SL=''
,StoreID=sd.Branch,NetAmount=sm.NetAmount,ShopName=shp.ShopName
,PCategoryID=sd.PROD_WGPG,PCategoryName=c.WGPG_NAME
,SubCategoryID=sd.PROD_SUBCATEGORY,SubCategory=sc.SubCategoryName
,DescriptionID=sd.PROD_DES,Description=d.Description
,QtyPcs=sd.Quantity
,CostPrice=sd.CostPrice
,SalePrice=sd.SalePrice
,GiftAmount= case when sd.Remarks='Gift' then sd.SalePrice else '0' end
,sd .Discount 
,TotalSalePrice=sd.Quantity*sd.SalePrice
,Barcode=sd.Barcode
,SalesReturnDate=sm.IDAT
,sm.IDAT as SalesReturnDate
,InvoiceNo=sd.Po_Number
,cashbalance=(select isnull(Balance,0) from vw_Shopwise_Cash_Balance where Branch='{0}')
,CustomerName=(select top 1 c.CustomerName from CustomerInformation c where c.InvoiceNo=sm.InvoiceNo)
,CustomerID=(select top 1 c.OID from CustomerInformation c where c.InvoiceNo=sm.InvoiceNo)
,IDAT=sm.IDAT,IUSER=sm.IUSER
,PaymentModeID=sm.PaymentModeID,PaymentMode=pay.PaymentMode

from Acc_StockDetail sd
inner join T_SALES_MST sm on sm.InvoiceNo=sd.Po_Number
inner join PaymentMode pay on pay.OID=sm.PaymentModeID
inner join ShopInfo shp on shp.OID=sm.StoreID
inner join T_WGPG c on c.OID=sd.PROD_WGPG
inner join Description d on d.OID=sd.PROD_DES 
inner join SubCategory sc on sc.OID=sd.PROD_SUBCATEGORY 
where sm.SlNo=(select top 1 SlNo from T_SALES_MST where  T_SALES_MST.StoreID='{0}' and T_SALES_MST.InvoiceNo ='{1}' order by IDAT desc)
", Shop_id, ddlInvoiceNo.SelectedItem.Text.ToString());
        //sql = "select T_SALES_MST.StoreID,ShopInfo.ShopName,T_WGPG.OID as PCategoryID,T_WGPG.WGPG_NAME as PCategoryName,SubCategory.OID as SubCategoryID,SubCategory.SubCategoryName as SubCategory,Description.OID as DescriptionID,Description.Description,T_SALES_DTL.SaleQty as QtyPcs,T_SALES_DTL.Barcode,T_SALES_MST.IDAT as SalesReturnDate from T_SALES_MST inner join T_SALES_DTL on T_SALES_MST.InvoiceNo=T_SALES_DTL.InvoiceNo inner join Description on T_SALES_DTL.DescriptionID=Description.OID inner join SubCategory on Description.SubCategoryID= SubCategory.OID inner join T_WGPG on SubCategory.CategoryID=T_WGPG.OID inner join ShopInfo on T_SALES_MST.StoreID=ShopInfo.OID where T_SALES_MST.InvoiceNo ='" + ddlInvoiceNo.SelectedItem.Text.ToString() + "'  ";
        cmd = new SqlCommand(sql, dbConnect);
        dbConnect.Open();
        da.SelectCommand = cmd;
        da.Fill(dt);
        dbConnect.Close();

        string netamount = dt.Rows[0]["NetAmount"].ToString();
        string cashbalance = dt.Rows[0]["cashbalance"].ToString();
        if (Convert.ToDecimal(netamount) > Convert.ToDecimal(cashbalance))
        {
            gvT_Issue_REQUISITION_DTL.DataSource = null;
            gvT_Issue_REQUISITION_DTL.DataBind();
            Label1.Text = "Your Available Cash Balance is low.";
            Label1.ForeColor = Color.Red;


        }
        else
        {
            gvT_Issue_REQUISITION_DTL.DataSource = null;
            gvT_Issue_REQUISITION_DTL.DataSource = dt;
            gvT_Issue_REQUISITION_DTL.DataBind();

            //
            lblCustomerName.Text = dt.Rows[0]["CustomerName"].ToString();
            lblCustomerID.Text = dt.Rows[0]["CustomerID"].ToString();
            lblPaymentMode.Text = dt.Rows[0]["PaymentMode"].ToString();
            lblPaymentModeID.Text = dt.Rows[0]["PaymentModeID"].ToString();


            string result = string.Empty;
            string myQuery = "select Reason from SalesReturn where InvoiceNo ='" + ddlInvoiceNo.SelectedItem.Text.ToString() + "'";
            cmd = new SqlCommand(myQuery, dbConnect);
            dbConnect.Open();
            string getValue = cmd.ExecuteScalar().ToString();
            if (getValue != null)
            {
                result = getValue.ToString();
            }
            dbConnect.Close();
            txtReason.Text = result;

        }
        }



    }
    protected void ddlInvoiceNo_SelectedIndexChangedvvv0(object sender, EventArgs e)
    {
        lblCustomerName.Text = string.Empty;
        lblCustomerID.Text = string.Empty;
        lblPaymentMode.Text = string.Empty;
        lblPaymentModeID.Text = string.Empty;
        DataTable dt = new DataTable();

        sql = string.Format(@"
select T_SALES_MST.StoreID,ShopInfo.ShopName,T_WGPG.OID as PCategoryID,T_WGPG.WGPG_NAME as PCategoryName
,SubCategory.OID as SubCategoryID,SubCategory.SubCategoryName as SubCategory,Description.OID as DescriptionID
,Description.Description,T_SALES_DTL.SaleQty as QtyPcs
,T_SALES_DTL.SalePrice,(T_SALES_DTL.SaleQty*T_SALES_DTL.SalePrice) as TotalSalePrice

,T_SALES_DTL.Barcode,T_SALES_MST.IDAT as SalesReturnDate ,T_SALES_MST.InvoiceNo

,CostPrice=(
select top 1 CostPrice from Acc_StockDetail asd where asd.Po_Number=T_SALES_MST.InvoiceNo and asd.PROD_DES= Description.OID
)
,CustomerName=(select top 1 c.CustomerName from CustomerInformation c where c.InvoiceNo=T_SALES_MST.InvoiceNo)
,CustomerID=(select top 1 c.OID from CustomerInformation c where c.InvoiceNo=T_SALES_MST.InvoiceNo)
,T_SALES_MST.IDAT,T_SALES_MST.IUSER

,T_SALES_MST.PaymentModeID
,PaymentMode=(select top 1 p.PaymentMode from PaymentMode p where p.OID=T_SALES_MST.PaymentModeID)
from T_SALES_MST 
inner join T_SALES_DTL on T_SALES_MST.InvoiceNo=T_SALES_DTL.InvoiceNo 
inner join Description on T_SALES_DTL.DescriptionID=Description.OID 
inner join SubCategory on Description.SubCategoryID= SubCategory.OID 
inner join T_WGPG on SubCategory.CategoryID=T_WGPG.OID 
inner join ShopInfo on T_SALES_MST.StoreID=ShopInfo.OID 
where T_SALES_MST.InvoiceNo ='{0}' 
", ddlInvoiceNo.SelectedItem.Text.ToString());

        sql = string.Format(@"
select 
ROW_NUMBER()OVER(PARTITION BY T_SALES_DTL.DescriptionID, T_SALES_DTL.Barcode ORDER BY T_SALES_DTL.DescriptionID, T_SALES_DTL.Barcode) as SL
,T_SALES_MST.StoreID,T_SALES_MST.NetAmount,ShopInfo.ShopName,T_WGPG.OID as PCategoryID,T_WGPG.WGPG_NAME as PCategoryName
,SubCategory.OID as SubCategoryID,SubCategory.SubCategoryName as SubCategory,Description.OID as DescriptionID
,Description.Description,T_SALES_DTL.SaleQty as QtyPcs
,case 
when T_SALES_DTL .GiftAmount>0 
then (select top 1 CostPrice from Acc_StockDetail asd where asd.Po_Number=T_SALES_MST.InvoiceNo and asd.PROD_DES= Description.OID)
else T_SALES_DTL.SalePrice
end SalePrice

,T_SALES_DTL .GiftAmount
,T_SALES_DTL .DiscountAmount
,CONVERT(DECIMAL(18,0),
(T_SALES_DTL.SaleQty*
(case 
when T_SALES_DTL .GiftAmount>0 
then (select top 1 CostPrice from Acc_StockDetail asd where asd.Po_Number=T_SALES_MST.InvoiceNo and asd.PROD_DES= Description.OID)
else T_SALES_DTL.SalePrice
end)
) 
)
as TotalSalePrice

,T_SALES_DTL.Barcode,T_SALES_MST.IDAT as SalesReturnDate ,T_SALES_MST.InvoiceNo
,cashbalance = (select isnull(Balance,0) from vw_Shopwise_Cash_Balance where Branch='{1}')
,Acc_StockDetail.CostPrice
,CustomerName=(select top 1 c.CustomerName from CustomerInformation c where c.InvoiceNo=T_SALES_MST.InvoiceNo)
,CustomerID=(select top 1 c.OID from CustomerInformation c where c.InvoiceNo=T_SALES_MST.InvoiceNo)
,T_SALES_MST.IDAT,T_SALES_MST.IUSER

,T_SALES_MST.PaymentModeID
,PaymentMode=(select top 1 p.PaymentMode from PaymentMode p where p.OID=T_SALES_MST.PaymentModeID)

from (select top 1 * from T_SALES_MST where T_SALES_MST.InvoiceNo ='{0}' order by IDAT desc) T_SALES_MST
inner join T_SALES_DTL on T_SALES_MST.InvoiceNo=T_SALES_DTL.InvoiceNo 
inner join Acc_StockDetail on Acc_StockDetail.Po_Number=T_SALES_MST.InvoiceNo
inner join Description on T_SALES_DTL.DescriptionID=Description.OID 
inner join SubCategory on Description.SubCategoryID= SubCategory.OID 
inner join T_WGPG on SubCategory.CategoryID=T_WGPG.OID 
inner join ShopInfo on T_SALES_MST.StoreID=ShopInfo.OID 
where T_SALES_MST.InvoiceNo ='{0}' 
", ddlInvoiceNo.SelectedItem.Text.ToString(), Shop_id);

        //sql = "select T_SALES_MST.StoreID,ShopInfo.ShopName,T_WGPG.OID as PCategoryID,T_WGPG.WGPG_NAME as PCategoryName,SubCategory.OID as SubCategoryID,SubCategory.SubCategoryName as SubCategory,Description.OID as DescriptionID,Description.Description,T_SALES_DTL.SaleQty as QtyPcs,T_SALES_DTL.Barcode,T_SALES_MST.IDAT as SalesReturnDate from T_SALES_MST inner join T_SALES_DTL on T_SALES_MST.InvoiceNo=T_SALES_DTL.InvoiceNo inner join Description on T_SALES_DTL.DescriptionID=Description.OID inner join SubCategory on Description.SubCategoryID= SubCategory.OID inner join T_WGPG on SubCategory.CategoryID=T_WGPG.OID inner join ShopInfo on T_SALES_MST.StoreID=ShopInfo.OID where T_SALES_MST.InvoiceNo ='" + ddlInvoiceNo.SelectedItem.Text.ToString() + "'  ";
        cmd = new SqlCommand(sql, dbConnect);
        dbConnect.Open();
        da.SelectCommand = cmd;
        da.Fill(dt);
        dbConnect.Close();

        string netamount = dt.Rows[0]["NetAmount"].ToString();
        string cashbalance = dt.Rows[0]["cashbalance"].ToString();
        if (Convert.ToDecimal(netamount) > Convert.ToDecimal(cashbalance))
        {
            gvT_Issue_REQUISITION_DTL.DataSource = null;
            gvT_Issue_REQUISITION_DTL.DataBind();
            Label1.Text = "Your Available Cash Balance is low.";
            Label1.ForeColor = Color.Red;


        }
        else
        {
            gvT_Issue_REQUISITION_DTL.DataSource = null;
            gvT_Issue_REQUISITION_DTL.DataSource = dt;
            gvT_Issue_REQUISITION_DTL.DataBind();

            //
            lblCustomerName.Text = dt.Rows[0]["CustomerName"].ToString();
            lblCustomerID.Text = dt.Rows[0]["CustomerID"].ToString();
            lblPaymentMode.Text = dt.Rows[0]["PaymentMode"].ToString();
            lblPaymentModeID.Text = dt.Rows[0]["PaymentModeID"].ToString();


            string result = string.Empty;
            string myQuery = "select Reason from SalesReturn where InvoiceNo ='" + ddlInvoiceNo.SelectedItem.Text.ToString() + "'";
            cmd = new SqlCommand(myQuery, dbConnect);
            dbConnect.Open();
            string getValue = cmd.ExecuteScalar().ToString();
            if (getValue != null)
            {
                result = getValue.ToString();
            }
            dbConnect.Close();
            txtReason.Text = result;

        }




    }



    void loadInvoiceNo()
    {
        DataTable dt = new DataTable();
        ddlInvoiceNo.Items.Clear();
        Shop_id = Session["StoreID"].ToString();
        String sql = "select OID,InvoiceNo as Name from SalesReturn where Approved=0 AND StoreID='" + Shop_id + "' ";
        CommonBinder.BindDropdownList(ddlInvoiceNo, sql);
        gvT_Issue_REQUISITION_DTL.DataSource = null;
        gvT_Issue_REQUISITION_DTL.DataBind();
    }


    protected void btnCancel_Click(object sender, EventArgs e)
    {
        loadInvoiceNo();
    }


    protected void btnIssue_Click(object sender, EventArgs e)
    {
        if (Convert.ToInt16(ddlInvoiceNo.SelectedIndex) == 0)
        {
            Label1.Text = "Select a Invoice No";
            Label1.ForeColor = Color.Red;
            return;
        }
        else
        {
            try
            {
                sql = "SELECT Approved from SalesReturn where OID=" + ddlInvoiceNo.SelectedValue.ToString() + " and Approved=0 ";
                DataTable dt = CommonBinder.getDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    cmd = new SqlCommand("spSalesReturn", dbConnect);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@InvNo", SqlDbType.VarChar, 100).Value = ddlInvoiceNo.SelectedItem.Text;
                    cmd.Parameters.Add("@InvOID", SqlDbType.BigInt).Value = ddlInvoiceNo.SelectedValue.ToString();
                    cmd.Parameters.Add("@StoreId", SqlDbType.VarChar, 10).Value = Shop_id;
                    cmd.Parameters.Add("@UserId", SqlDbType.VarChar, 50).Value = userID;
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
                        //dbConnect.Close();
                    }
                    //return;
                    //------------- Previous Code --------------------//
                    //--------------------------------------------------------//
                    #region Previous Code
                    //                    sql = "update SalesReturn set Approved=@Approved,EUSER=@EUSER,EDAT=@EDAT where OID=" + ddlInvoiceNo.SelectedValue.ToString() + " ";
                    //                    cmd = new SqlCommand(sql, dbConnect);
                    //                    cmd.Parameters.Add("@Approved", SqlDbType.Int).Value = 1;
                    //                    cmd.Parameters.Add("@EUSER", SqlDbType.VarChar, 50).Value = userID;
                    //                    cmd.Parameters.Add("@EDAT", SqlDbType.Date).Value = DateTime.Now.Date;
                    //                    dbConnect.Open();
                    //                    cmd.ExecuteNonQuery();

                    //                    sql = "update T_SALES_MST set DropStatus=@DropStatus,EUSER=@EUSER,EDAT=@EDAT where InvoiceNo='" + ddlInvoiceNo.SelectedItem.Text.ToString() + "' ";
                    //                    cmd = new SqlCommand(sql, dbConnect);
                    //                    cmd.Parameters.Add("@DropStatus", SqlDbType.Int).Value = 1;
                    //                    cmd.Parameters.Add("@EUSER", SqlDbType.VarChar, 50).Value = userID;
                    //                    cmd.Parameters.Add("@EDAT", SqlDbType.Date).Value = DateTime.Now.Date;
                    //                    cmd.ExecuteNonQuery();
                    //                    //dbConnect.Close();


                    //                    sql = "delete from CASHINOUT where INVOICEID='" + ddlInvoiceNo.SelectedItem.Text.ToString() + "' ";
                    //                    cmd = new SqlCommand(sql, dbConnect);

                    //                    try
                    //                    {
                    //                        if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                    //                        cmd.ExecuteNonQuery();
                    //                    }
                    //                    catch (Exception ex)
                    //                    {
                    //                        throw ex;
                    //                    }
                    //                    finally
                    //                    {
                    //                        //dbConnect.Close();
                    //                    }

                    //                    if (gvT_Issue_REQUISITION_DTL.Rows.Count > 0)
                    //                    {
                    //                        //dbConnect.Open();
                    //                        for (int i = 0; i < gvT_Issue_REQUISITION_DTL.Rows.Count; i++)
                    //                        {
                    //                            string sqlAccStockDetails = "insert into Acc_StockDetail(Po_Number,PROD_WGPG,PROD_SUBCATEGORY,PROD_DES,Branch,Quantity,CostPrice,SalePrice,Total,Flag,Remarks,IDAT,IUSER,IDATTIME) values(@InvoiceNo,@CategoryID,@SubCategoryID,@DescriptionID,@Branch,@SaleQty,@PURCHASECOST,@SalePrice,@TOTAL,@Flag,@Remarks,@IDAT,@IUSER,@IDATTIME) ";
                    //                            SqlCommand cmdInsertAccStockDetails = new SqlCommand(sqlAccStockDetails, dbConnect);
                    //                            cmdInsertAccStockDetails.Parameters.Add("@InvoiceNo", SqlDbType.VarChar, 50).Value = ddlInvoiceNo.SelectedValue;
                    //                            cmdInsertAccStockDetails.Parameters.Add("@CategoryID", SqlDbType.BigInt).Value = Convert.ToInt64(gvT_Issue_REQUISITION_DTL.DataKeys[i].Values["PCategoryID"]);
                    //                            cmdInsertAccStockDetails.Parameters.Add("@SubCategoryID", SqlDbType.BigInt).Value = Convert.ToInt64(gvT_Issue_REQUISITION_DTL.DataKeys[i].Values["SubCategoryID"]);
                    //                            cmdInsertAccStockDetails.Parameters.Add("@DescriptionID", SqlDbType.BigInt).Value = Convert.ToInt64(gvT_Issue_REQUISITION_DTL.DataKeys[i].Values["DescriptionID"]);
                    //                            cmdInsertAccStockDetails.Parameters.Add("@Branch", SqlDbType.VarChar, 50).Value = Shop_id;
                    //                            cmdInsertAccStockDetails.Parameters.Add("@SaleQty", SqlDbType.Int).Value = Convert.ToInt32(gvT_Issue_REQUISITION_DTL.DataKeys[i].Values["QtyPcs"]);
                    //                            cmdInsertAccStockDetails.Parameters.Add("@PURCHASECOST", SqlDbType.Int).Value = Convert.ToInt32(gvT_Issue_REQUISITION_DTL.DataKeys[i].Values["CostPrice"]);
                    //                            cmdInsertAccStockDetails.Parameters.Add("@SalePrice", SqlDbType.Int).Value = Convert.ToInt32(gvT_Issue_REQUISITION_DTL.DataKeys[i].Values["SalePrice"]);          //
                    //                            cmdInsertAccStockDetails.Parameters.Add("@TOTAL", SqlDbType.Int).Value =
                    //                                Convert.ToInt32(gvT_Issue_REQUISITION_DTL.DataKeys[i].Values["CostPrice"]) *
                    //                                Convert.ToInt32(gvT_Issue_REQUISITION_DTL.DataKeys[i].Values["QtyPcs"]);
                    //                            cmdInsertAccStockDetails.Parameters.Add("@Flag", SqlDbType.VarChar, 50).Value = "Sale Return";
                    //                            cmdInsertAccStockDetails.Parameters.Add("@Remarks", SqlDbType.VarChar, 50).Value = "Sale Return";
                    //                            cmdInsertAccStockDetails.Parameters.Add("@IDAT", SqlDbType.Date).Value = gvT_Issue_REQUISITION_DTL.DataKeys[i].Values["IDAT"];
                    //                            cmdInsertAccStockDetails.Parameters.Add("@IUSER", SqlDbType.VarChar, 50).Value = gvT_Issue_REQUISITION_DTL.DataKeys[i].Values["IUSER"];
                    //                            cmdInsertAccStockDetails.Parameters.Add("@IDATTIME", SqlDbType.Date).Value = DateTime.Now;

                    //                            string sqlUpdateAccStock = string.Empty;
                    //                            if (!string.IsNullOrEmpty(gvT_Issue_REQUISITION_DTL.DataKeys[i].Values["Barcode"].ToString()))
                    //                            {
                    //                                sqlUpdateAccStock = "update Acc_Stock set Quantity = (Quantity + @Quantity) where ISNULL(Flag,'')='' and PROD_DES = @OID AND CostPrice=@PURCHASECOST";
                    //                            }
                    //                            else
                    //                            {
                    //                                //sqlUpdateAccStock = "update Acc_Stock set Quantity = (Quantity + @Quantity) where PROD_DES = @OID AND CostPrice=@PURCHASECOST AND Flag='Quantity' AND Quantity>0 order by ACC_STOCKID";

                    //                                sqlUpdateAccStock = string.Format(@"
                    //declare @stockID nvarchar(20);
                    //
                    //select top 1 @stockID = ISNULL(ACC_STOCKID,0) from Acc_Stock 
                    //where Flag ='Quantity' AND Quantity >0 
                    //and Branch=@Branch and PROD_WGPG=@CategoryID and PROD_SUBCATEGORY=@SubCategoryID and PROD_DES=@DescriptionID AND CostPrice =@PURCHASECOST 
                    //order by ACC_STOCKID asc
                    //--select @stockID
                    //
                    //IF ISNULL(@stockID,'')=''
                    //    select top 1 @stockID=ACC_STOCKID from Acc_Stock 
                    //    where Flag ='Quantity' AND Quantity =0 
                    //    and Branch=@Branch and PROD_WGPG=@CategoryID and PROD_SUBCATEGORY=@SubCategoryID and PROD_DES=@DescriptionID AND CostPrice =@PURCHASECOST  
                    //    order by ACC_STOCKID desc
                    //                                                                                                    --select @stockID
                    //
                    //update Acc_Stock set Quantity=Quantity+@Quantity,SRQty=SRQty+@Quantity where ACC_STOCKID =@stockID
                    //
                    //");
                    //                            }

                    //                            SqlCommand cmdsqlUpdateAccStock = new SqlCommand(sqlUpdateAccStock, dbConnect);
                    //                            cmdsqlUpdateAccStock.Parameters.Add("@OID", SqlDbType.BigInt).Value = Convert.ToInt64(gvT_Issue_REQUISITION_DTL.DataKeys[i].Values["DescriptionID"]);
                    //                            cmdsqlUpdateAccStock.Parameters.Add("@Quantity", SqlDbType.Int).Value = Convert.ToInt32(gvT_Issue_REQUISITION_DTL.DataKeys[i].Values["QtyPcs"]);
                    //                            cmdsqlUpdateAccStock.Parameters.Add("@PURCHASECOST", SqlDbType.Int).Value = Convert.ToInt32(gvT_Issue_REQUISITION_DTL.DataKeys[i].Values["CostPrice"]);
                    //                            cmdsqlUpdateAccStock.Parameters.Add("@CategoryID", SqlDbType.BigInt).Value = Convert.ToInt64(gvT_Issue_REQUISITION_DTL.DataKeys[i].Values["PCategoryID"]);
                    //                            cmdsqlUpdateAccStock.Parameters.Add("@SubCategoryID", SqlDbType.BigInt).Value = Convert.ToInt64(gvT_Issue_REQUISITION_DTL.DataKeys[i].Values["SubCategoryID"]);
                    //                            cmdsqlUpdateAccStock.Parameters.Add("@DescriptionID", SqlDbType.BigInt).Value = Convert.ToInt64(gvT_Issue_REQUISITION_DTL.DataKeys[i].Values["DescriptionID"]);
                    //                            cmdsqlUpdateAccStock.Parameters.Add("@Branch", SqlDbType.VarChar, 50).Value = Shop_id;
                    //                            //cmdInsertAccStockDetails.Parameters.Add("@PURCHASECOST", SqlDbType.Int).Value = Convert.ToInt32(gvT_Issue_REQUISITION_DTL.DataKeys[i].Values["CostPrice"]);


                    //                            try
                    //                            {
                    //                                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                    //                                cmdInsertAccStockDetails.ExecuteNonQuery();
                    //                                cmdsqlUpdateAccStock.ExecuteNonQuery();
                    //                            }
                    //                            catch (Exception ex)
                    //                            {
                    //                                throw ex;
                    //                            }
                    //                            finally
                    //                            {
                    //                                dbConnect.Close();
                    //                            }




                    //                            string StoreID = Session["StoreID"].ToString();

                    //                            decimal netsale1 = 0;

                    //                            //if (Convert.ToDecimal(gvT_Issue_REQUISITION_DTL.DataKeys[i].Values["Discount"]) > 0)
                    //                            //{
                    //                            //    netsale1 = (Convert.ToDecimal(gvT_Issue_REQUISITION_DTL.DataKeys[i].Values["TotalSalePrice"])) - (Convert.ToDecimal(gvT_Issue_REQUISITION_DTL.DataKeys[i].Values["DiscountAmount"]));
                    //                            //}
                    //                            //else
                    //                            //{
                    //                                netsale1 = Convert.ToDecimal(gvT_Issue_REQUISITION_DTL.DataKeys[i].Values["TotalSalePrice"]);
                    //                            //}

                    //                            string InsertDR = string.Format(@"
                    //INSERT INTO Acc_Journal(AccountID,Branch,Particular,Remarks,Debit,Credit,RefferenceNumber,IDAT,IDATTIME,Narration) 
                    //VALUES({0},{1},'{2}','{3}',{4}
                    //,{5},'{6}','{7}','{8}','{9}')
                    //", gvT_Issue_REQUISITION_DTL.DataKeys[i].Values["DescriptionID"].ToString()
                    //        , gvT_Issue_REQUISITION_DTL.DataKeys[i].Values["StoreID"].ToString()
                    //        , "Sale"
                    //        , "Product"
                    //        , gvT_Issue_REQUISITION_DTL.DataKeys[i].Values["TotalSalePrice"].ToString()

                    //        , 0
                    //        , gvT_Issue_REQUISITION_DTL.DataKeys[i].Values["InvoiceNo"].ToString()
                    //        , DateTime.Now.ToString()
                    //        , DateTime.Now.ToString()
                    //        , "Sale Return From Customer");

                    //                            string strAccountID = lblPaymentModeID.Text == "17" ? lblCustomerID.Text : "1";
                    //                            string strParticular = lblPaymentModeID.Text == "17" ? "A/R" : "Cash";
                    //                            string strRemarks = lblPaymentModeID.Text == "17" ? "Customer" : "Cash";

                    //                            string InsertCR = string.Empty;
                    //                            string InsertCR2 = string.Empty;
                    //                            if (Convert.ToDecimal(gvT_Issue_REQUISITION_DTL.DataKeys[i].Values["GiftAmount"]) == 0)
                    //                            {
                    //                                if (Convert.ToDecimal(gvT_Issue_REQUISITION_DTL.DataKeys[i].Values["DiscountAmount"]) > 0)
                    //                                {
                    //                                    string TotalSalePriceN = "-" + gvT_Issue_REQUISITION_DTL.DataKeys[i].Values["DiscountAmount"].ToString();
                    //                                    InsertCR = string.Format(@"
                    //
                    //  declare @CostingHeadID nvarchar(100)
                    //  select @CostingHeadID =(select OID from CostingHead where Shop_id ='{0}' and CostingHead ='Discount On Sales')
                    //  
                    //
                    //INSERT INTO Acc_Journal(AccountID,Branch,Particular,Remarks,Debit,Credit,RefferenceNumber,IDAT,IDATTIME,Narration) 
                    //VALUES( @CostingHeadID ,'{0}','{1}','{2}','{3}'
                    //,{4},'{5}','{6}','{7}','{8}')
                    //
                    //insert into DailyCost(Shop_id,CostingHeadID,AMOUNT,IUSER,IDAT,Remarks,ReferenceNo) values ('{0}',@CostingHeadID,{10},'{9}','{6}','Discount Returned','Discount Returned')
                    //"

                    //                                       , gvT_Issue_REQUISITION_DTL.DataKeys[i].Values["StoreID"].ToString()
                    //                                       , "Expense", strRemarks, 0

                    //                                       , gvT_Issue_REQUISITION_DTL.DataKeys[i].Values["DiscountAmount"]
                    //                                       , gvT_Issue_REQUISITION_DTL.DataKeys[i].Values["InvoiceNo"].ToString()
                    //                                       , DateTime.Now.ToString(), DateTime.Now.ToString(), "Discount Expense Credited for Sale Return From Customer", userID, TotalSalePriceN);

                    //                                    InsertCR2 = string.Format(@"
                    //INSERT INTO Acc_Journal(AccountID,Branch,Particular,Remarks,Debit,Credit,RefferenceNumber,IDAT,IDATTIME,Narration) 
                    //VALUES({0},'{1}','{2}','{3}',{4}
                    //,{5},'{6}','{7}','{8}','{9}')
                    //"
                    //                                               , strAccountID
                    //                                               , gvT_Issue_REQUISITION_DTL.DataKeys[i].Values["StoreID"].ToString()
                    //                                               , strParticular, strRemarks, 0

                    //                                               , (netsale1 - Convert.ToDecimal(gvT_Issue_REQUISITION_DTL.DataKeys[i].Values["DiscountAmount"])).ToString()
                    //                                               , gvT_Issue_REQUISITION_DTL.DataKeys[i].Values["InvoiceNo"].ToString()
                    //                                               , DateTime.Now.ToString(), DateTime.Now.ToString(), "Sale Return From Customer");
                    //                                    if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();

                    //                                    SqlCommand cmdCR2 = new SqlCommand(InsertCR2, dbConnect);

                    //                                    cmdCR2.ExecuteNonQuery();

                    //                                }
                    //                                else
                    //                                {
                    //                                    InsertCR = string.Format(@"
                    //INSERT INTO Acc_Journal(AccountID,Branch,Particular,Remarks,Debit,Credit,RefferenceNumber,IDAT,IDATTIME,Narration) 
                    //VALUES({0},'{1}','{2}','{3}',{4}
                    //,{5},'{6}','{7}','{8}','{9}')
                    //"
                    //                                               , strAccountID
                    //                                               , gvT_Issue_REQUISITION_DTL.DataKeys[i].Values["StoreID"].ToString()
                    //                                               , strParticular, strRemarks, 0

                    //                                               , gvT_Issue_REQUISITION_DTL.DataKeys[i].Values["TotalSalePrice"].ToString()
                    //                                               , gvT_Issue_REQUISITION_DTL.DataKeys[i].Values["InvoiceNo"].ToString()
                    //                                               , DateTime.Now.ToString(), DateTime.Now.ToString(), "Sale Return From Customer");
                    //                                }

                    //                            }
                    //                            else
                    //                            {
                    //                                string TotalSalePriceP = gvT_Issue_REQUISITION_DTL.DataKeys[i].Values["TotalSalePrice"].ToString();
                    //                                string TotalSalePriceN = "-" + gvT_Issue_REQUISITION_DTL.DataKeys[i].Values["TotalSalePrice"].ToString();


                    //                                InsertCR = string.Format(@"
                    //
                    //  declare @CostingHeadID nvarchar(100)
                    //  select @CostingHeadID =(select OID from CostingHead where Shop_id ='{0}' and CostingHead ='Expense For Gift')
                    //  
                    //
                    //INSERT INTO Acc_Journal(AccountID,Branch,Particular,Remarks,Debit,Credit,RefferenceNumber,IDAT,IDATTIME,Narration) 
                    //VALUES( @CostingHeadID ,'{0}','{1}','{2}','{3}'
                    //,{4},'{5}','{6}','{7}','{8}')
                    //
                    //insert into DailyCost(Shop_id,CostingHeadID,AMOUNT,IUSER,IDAT,Remarks,ReferenceNo) values ('{0}',@CostingHeadID,{10},'{9}','{6}','Gift Returned','Gift Returned')
                    //"

                    //                                       , gvT_Issue_REQUISITION_DTL.DataKeys[i].Values["StoreID"].ToString()
                    //                                       , "Expense", strRemarks, 0

                    //                                       , TotalSalePriceP
                    //                                       , gvT_Issue_REQUISITION_DTL.DataKeys[i].Values["InvoiceNo"].ToString()
                    //                                       , DateTime.Now.ToString(), DateTime.Now.ToString(), "Gift Expense Credited for Sale Return From Customer", userID, TotalSalePriceN);
                    //                            }
                    //                            try
                    //                            {
                    //                                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                    //                                SqlCommand cmdDR = new SqlCommand(InsertDR, dbConnect);
                    //                                SqlCommand cmdCR = new SqlCommand(InsertCR, dbConnect);
                    //                                //dbConnect.Open();
                    //                                cmdDR.ExecuteNonQuery();
                    //                                cmdCR.ExecuteNonQuery();
                    //                            }
                    //                            catch (Exception ex)
                    //                            {
                    //                                throw ex;
                    //                            }
                    //                            finally
                    //                            {
                    //                                // dbConnect.Close();
                    //                            }
                    //                        }
                    //                    }
                    //                    BindTemporaryItemsGrid();
                    //                    Label1.Text = "Approved Successfully";
                    //                    Label1.ForeColor = Color.Red;
                    //                    loadInvoiceNo();
                    //                    DataTable dtgrv = new DataTable();
                    //                    gvT_Issue_REQUISITION_DTL.DataSource = dtgrv;
                    //                    gvT_Issue_REQUISITION_DTL.DataBind();
                    //                    txtReason.Text = string.Empty; 
                    #endregion
                }
                else
                {
                    Label1.Text = "Already Approved";
                    Label1.ForeColor = Color.Red;
                    loadInvoiceNo();
                    DataTable dtgrv = new DataTable();
                    gvT_Issue_REQUISITION_DTL.DataSource = dtgrv;
                    gvT_Issue_REQUISITION_DTL.DataBind();
                }
            }
            catch (Exception ex)
            {

                Label1.Text = "Approved Failed";
                Label1.ForeColor = Color.Red;
            }
            finally { dbConnect.Close(); }
        }
    }



    private void BindTemporaryItemsGrid()
    {
        if (gvT_Issue_REQUISITION_DTL.Rows.Count > 0)
        {
            //dbConnect.Open();
            for (int i = 0; i < gvT_Issue_REQUISITION_DTL.Rows.Count; i++)
            {
                String StoreID = ((Label)gvT_Issue_REQUISITION_DTL.Rows[i].FindControl("lblStoreID")).Text;
                String ProductCategoryID = ((Label)gvT_Issue_REQUISITION_DTL.Rows[i].FindControl("lblCategoryId")).Text;
                String SubCategoryID = ((Label)gvT_Issue_REQUISITION_DTL.Rows[i].FindControl("lblSubCategoryID")).Text;
                String DescriptionID = ((Label)gvT_Issue_REQUISITION_DTL.Rows[i].FindControl("lblDescriptionID")).Text;
                String QUANTITY = ((Label)gvT_Issue_REQUISITION_DTL.Rows[i].FindControl("lblQtyPcs")).Text;
                String Barcode = ((Label)gvT_Issue_REQUISITION_DTL.Rows[i].FindControl("lblBarcode")).Text;


                sql = "SELECT OID from StoreMasterStock where PROD_WGPG=" + ProductCategoryID + " and PROD_SUBCATEGORY=" + SubCategoryID + " and PROD_DES=" + DescriptionID + " and Branch='" + StoreID + "' ";
                DataTable dt = CommonBinder.getDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    sql = "update StoreMasterStock set SaleQuantity=SaleQuantity - " + QUANTITY + " where OID=" + dt.Rows[0]["OID"] + " ";
                    cmd = new SqlCommand(sql, dbConnect);
                    cmd.ExecuteNonQuery();

                    sql = "INSERT INTO StockPosting(BranchOID,DescriptionOID,Barcode,InwardQty,OutwardQty,Particulars,IUSER,IDAT) VALUES(@BranchOID,@DescriptionOID,@Barcode,@InwardQty,@OutwardQty,@Particulars,@IUSER,@IDAT) ";
                    cmd = new SqlCommand(sql, dbConnect);
                    cmd.Parameters.Add("@BranchOID", SqlDbType.VarChar, 100).Value = StoreID;
                    cmd.Parameters.Add("@DescriptionOID", SqlDbType.BigInt).Value = DescriptionID;
                    cmd.Parameters.Add("@Barcode", SqlDbType.VarChar, 100).Value = Barcode;
                    cmd.Parameters.Add("@InwardQty", SqlDbType.BigInt).Value = QUANTITY;
                    cmd.Parameters.Add("@OutwardQty", SqlDbType.BigInt).Value = 0;
                    cmd.Parameters.Add("@Particulars", SqlDbType.VarChar, 100).Value = "Sale Return";
                    cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 50).Value = userID;
                    cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = DateTime.Today.Date;
                    cmd.ExecuteNonQuery();


                    //status=0    T_Stock   T_Product   :   cat   sub  des  barcode  store   date>25sep17
                    sql = string.Format(@"
update T_STOCK set SaleStatus=0 
where IDAT>'26 sep 2017' and SaleStatus='1' and PROD_WGPG={0} and PROD_SUBCATEGORY={1} and PROD_DES={2} and Branch='{3}' and Barcode='{4}'

update T_PROD set SaleStatus=0 
where IDAT>'26 sep 2017' and SaleStatus='1' and PROD_WGPG={0} and PROD_SUBCATEGORY={1} and PROD_DES={2} and Branch='{3}' and Barcode='{4}'
", ProductCategoryID, SubCategoryID, DescriptionID
     , StoreID, Barcode);
                    cmd = new SqlCommand(sql, dbConnect);
                    cmd.ExecuteNonQuery();

                }


            }
            //dbConnect.Close();
        }
    }

    protected void cmdDelete_Click(object sender, EventArgs e)
    {
        if (Convert.ToInt16(ddlInvoiceNo.SelectedIndex) == 0)
        {
            Label1.Text = "Select a Invoice No";
            Label1.ForeColor = Color.Red;
            return;
        }
        else
        {
            sql = "delete from SalesReturn where OID=" + ddlInvoiceNo.SelectedValue.ToString() + " ";
            cmd = new SqlCommand(sql, dbConnect);
            dbConnect.Open();
            cmd.ExecuteNonQuery();
            dbConnect.Close();
            Label1.Text = "Sales Return Request Deleted Successfully";
            Label1.ForeColor = Color.Red;
            loadInvoiceNo();
            gvT_Issue_REQUISITION_DTL.DataSource = null;
            gvT_Issue_REQUISITION_DTL.DataBind();
            txtReason.Text = string.Empty;
        }
    }

}