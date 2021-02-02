using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using TalukderPOS.BusinessObjects;
using TalukderPOS.BLL;

public partial class Pages_DashBoardAdmin : System.Web.UI.Page
{
    private string userID = string.Empty;
    private string userPassword = string.Empty;
    SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
    SqlCommand cmd;
    SqlDataAdapter da = new SqlDataAdapter();
    
    

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
        //bool isAuthenticate = CommonBinder.CheckPageAuthentication(System.Web.HttpContext.Current.Request.Url.AbsolutePath, userID);
        //if (!isAuthenticate)
        //{
        //    Response.Redirect("~/UnAuthorizedUser.aspx");
        //}
        if (!Page.IsPostBack)
        {
            loadPurchaseReturnList();
            loadSalesReturnList();
            loadPurchaseOrderList();
        }
    }

    void loadPurchaseReturnList()
    {
        DataTable dt = new DataTable();
        String sql = "select BRANCH.NAME,(cast(BRANCH.PREFIX AS Varchar) +'-'+ cast(PurchaseReturnMST.OID AS Varchar) )as PurchaseReturnNo,(case PurchaseReturnMST.ApprovedStatus when (1) then 'Y' when (0) then 'N' end) as ApprovedStatus,CONVERT(VARCHAR(10),PurchaseReturnMST.IDAT,103) AS IDAT from PurchaseReturnMST inner join BRANCH on PurchaseReturnMST.BranchOID=BRANCH.OID where PurchaseReturnMST.ApprovedStatus=0";                
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
            gvPurchaseReturnList.DataSource = dt;
            gvPurchaseReturnList.DataBind();
        }
        else {
            gvPurchaseReturnList.DataSource = null;
            gvPurchaseReturnList.DataBind();
        }
 
        
    }


    void loadSalesReturnList()
    {
        DataTable dt = new DataTable();
        String sql = "select BRANCH.NAME,(cast(BRANCH.PREFIX AS Varchar) +'-'+ cast(SalesReturnMST.OID AS Varchar) )as SalesReturnNo,SalesReturnMST.ApprovedStatus,CONVERT(VARCHAR(10),SalesReturnMST.IDAT,103) AS IDAT from SalesReturnMST inner join BRANCH on SalesReturnMST.BranchOID=BRANCH.OID where SalesReturnMST.ApprovedStatus='N' ";
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
            gvSalesReturnList.DataSource = dt;
            gvSalesReturnList.DataBind();
        }
        else
        {
            gvSalesReturnList.DataSource = null;
            gvSalesReturnList.DataBind();
        }       
    }

    void loadPurchaseOrderList()
    {
        DataTable dt = new DataTable();
        String sql = "select BRANCH.NAME,(cast(BRANCH.PREFIX AS Varchar) +'-'+ cast(PurchaseOrderMST.OID AS Varchar) )as PurchaseOrderNo,PurchaseOrderMST.ApprovedStatus,CONVERT(VARCHAR(10),PurchaseOrderMST.IDAT,103) AS IDAT from PurchaseOrderMST inner join BRANCH on PurchaseOrderMST.BranchOID=BRANCH.OID where PurchaseOrderMST.ApprovedStatus='N' ";        
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
            gvPurchaseOrderList.DataSource = dt;
            gvPurchaseOrderList.DataBind();
        }
        else
        {
            gvPurchaseOrderList.DataSource = null;
            gvPurchaseOrderList.DataBind();
        }
    }


}