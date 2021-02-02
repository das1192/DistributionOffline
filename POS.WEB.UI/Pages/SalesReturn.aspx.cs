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

public partial class Pages_SalesReturn : System.Web.UI.Page
{
    private string userID = string.Empty;
    private string userPassword = string.Empty;    
    SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
    SqlCommand cmd;
    SqlDataAdapter da = new SqlDataAdapter();
    string sql;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            userID = Session["UserID"].ToString();
            userPassword = Session["Password"].ToString();
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
            Label1.Text = string.Empty;
        }

    }


    protected void txtInvoiceNo_TextChanged(object sender, EventArgs e)
    {
        Label1.Text = string.Empty;
        DataTable dt = new DataTable();
        sql = "select T_SALES_MST.StoreID,ShopInfo.ShopName,T_WGPG.OID as PCategoryID,T_WGPG.WGPG_NAME as PCategoryName,SubCategory.OID as SubCategoryID,SubCategory.SubCategoryName as SubCategory,Description.OID as DescriptionID,Description.Description,T_SALES_DTL.SaleQty as QtyPcs from T_SALES_MST inner join T_SALES_DTL on T_SALES_MST.InvoiceNo=T_SALES_DTL.InvoiceNo inner join Description on T_SALES_DTL.DescriptionID=Description.OID inner join SubCategory on Description.SubCategoryID= SubCategory.OID inner join T_WGPG on SubCategory.CategoryID=T_WGPG.OID inner join ShopInfo on T_SALES_MST.StoreID=ShopInfo.OID where T_SALES_MST.InvoiceNo='" + txtInvoiceNo.Text.ToString() + "' and T_SALES_MST.StoreID='" + Session["StoreID"].ToString() + "' and T_SALES_MST.DropStatus=0  ";

        sql = string.Format(@"
select ROW_NUMBER()OVER(PARTITION BY sd.DescriptionID, sd.Barcode ORDER BY sd.DescriptionID, sd.Barcode) as SL
,sd.Barcode
, s.StoreID,shp.ShopName,c.OID as PCategoryID,s.NetAmount
,c.WGPG_NAME as PCategoryName,sc.OID as SubCategoryID
,sc.SubCategoryName as SubCategory,d.OID as DescriptionID
,d.Description,sd.SaleQty as QtyPcs 
,cashbalance = (select isnull(Balance,0) from vw_Shopwise_Cash_Balance where Branch='{1}')
from (select top 1 * from T_SALES_MST where InvoiceNo='{0}' order by IDAT desc) s
inner join T_SALES_DTL sd on s.InvoiceNo=sd.InvoiceNo 
inner join Description d on sd.DescriptionID=d.OID 
inner join SubCategory sc on d.SubCategoryID= sc.OID 
inner join T_WGPG c on sc.CategoryID=c.OID 
inner join ShopInfo shp on s.StoreID=shp.OID 
where s.InvoiceNo='{0}' and s.StoreID='{1}' 
and s.DropStatus=0
", txtInvoiceNo.Text.ToString(),Session["StoreID"].ToString());
        
        cmd = new SqlCommand(sql, dbConnect);
        dbConnect.Open();
        da.SelectCommand = cmd;
        da.Fill(dt);
        dbConnect.Close();
        if (dt.Rows.Count > 0)
        {
            


            string netamount = dt.Rows[0]["NetAmount"].ToString();
            string cashbalance = dt.Rows[0]["cashbalance"].ToString();
            if (Convert.ToDecimal(netamount) > Convert.ToDecimal(cashbalance))
            {
                gvT_Issue_REQUISITION_DTL.DataSource = null;
                Label1.Text = "Your Available Cash Balance is low.";
                Label1.ForeColor = Color.Red;

            }
            else
            {
                gvT_Issue_REQUISITION_DTL.DataSource = null;
                gvT_Issue_REQUISITION_DTL.DataSource = dt;

            }

            gvT_Issue_REQUISITION_DTL.DataBind();
            txtReason.Focus();
        }
        else
        {
            gvT_Issue_REQUISITION_DTL.DataSource = null;
            Label1.Text = "No Such Invoice Found.";
            Label1.ForeColor = Color.Red;
            btnIssue.Enabled = false;
        }
    }


    protected void btnIssue_Click(object sender, EventArgs e)
    {
        if (txtInvoiceNo.Text.ToString() == string.Empty || gvT_Issue_REQUISITION_DTL.Rows.Count < 0 || gvT_Issue_REQUISITION_DTL.Rows.Count == 0)
        {
            Label1.Text = "Enter a Valid Invoice No";
            Label1.ForeColor = Color.Red;
            return;
        }
        else if (txtReason.Text == string.Empty) {
            Label1.Text = "Please Type Reason For Sales Return";
            Label1.ForeColor = Color.Red;
            return;
        }
        else
        {
            DataTable dt = new DataTable();
            sql = "SELECT OID from SalesReturn where InvoiceNo='" + txtInvoiceNo.Text.ToString() + "' ";
            cmd = new SqlCommand(sql, dbConnect);
            da.SelectCommand = cmd;
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                Label1.Text = "Invoice Number already Transfered to Admin for Drop Approval";
                Label1.ForeColor = Color.Red;
            }
            else
            {
                sql = "insert into SalesReturn(InvoiceNo,StoreID,Approved,Reason,IUSER,IDAT,EUSER,EDAT)values(@InvoiceNo,@StoreID,@Approved,@Reason,@IUSER,@IDAT,@EUSER,@EDAT)";
                cmd = new SqlCommand(sql, dbConnect);
                cmd.Parameters.Add("@InvoiceNo", SqlDbType.VarChar, 50).Value = txtInvoiceNo.Text.ToString();
                cmd.Parameters.Add("@StoreID", SqlDbType.VarChar, 50).Value = Session["StoreID"].ToString();
                cmd.Parameters.Add("@Approved", SqlDbType.VarChar, 1).Value = "0";
                cmd.Parameters.Add("@Reason", SqlDbType.VarChar, 300).Value = txtReason.Text;
                cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 50).Value = userID;
                cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = DateTime.Now.Date;
                cmd.Parameters.Add("@EUSER", SqlDbType.VarChar, 50).Value = userID;
                cmd.Parameters.Add("@EDAT", SqlDbType.Date).Value = DateTime.Now.Date;
                
                dbConnect.Open();
                cmd.ExecuteNonQuery();
                dbConnect.Close();
                
                Label1.Text = "Invoice Number has been Transfered to Admin for Drop Approval";
                Label1.ForeColor = Color.Red;
            }
            Clear();
        }
    }



    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Clear();

    }

    private void Clear()
    {
        txtInvoiceNo.Text = string.Empty;
        txtReason.Text = string.Empty;
        gvT_Issue_REQUISITION_DTL.DataSource = null;
        gvT_Issue_REQUISITION_DTL.DataBind();
    }
}