using System;
using System.Web;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using TalukderPOS.BusinessObjects;
using TalukderPOS.BLL;

public partial class Pages_StockReturnReportForAdmin : System.Web.UI.Page
{
    private string userID = string.Empty;
    private string userPassword = string.Empty;
    StockReturn_BILL BILL = new StockReturn_BILL();


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
        }
    }



    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        lblMessage.Text = string.Empty;
        T_PROD entity = new T_PROD();
        entity.FromDate = txtFromDate.Text;
        entity.ToDate = txtToDate.Text;
        entity.Branch = ddlSearchBranch.SelectedValue.ToString();
        entity.SearchType = "0";
        DataTable dt = BILL.GetStockReturnList(entity);
        gvStockReturnList.DataSource = dt;
        gvStockReturnList.DataBind();
        GridView2.DataSource = null;
        GridView2.DataBind(); 
    }

    protected void StockReturn_Details(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
    {
        lblMessage.Text = string.Empty;
        GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
        if (e.CommandName == "ItemDetails")
        {
            Label lblStockReturnID = (Label)row.Cells[0].FindControl("lblStockReturnID");
            StockReturn_BO entity = new StockReturn_BO();
            entity.StockReturnID = lblStockReturnID.Text;
            DataTable dt = BILL.StockReturn_Detail(entity);
            GridView2.DataSource = dt;
            GridView2.DataBind();
        }
        if (e.CommandName == "Preview")
        {
            Label lblStockReturnID = (Label)row.Cells[0].FindControl("lblStockReturnID");
            StockReturn_BO entity = new StockReturn_BO();
            entity.StockReturnID = lblStockReturnID.Text;
            DataTable dt = BILL.StockReturn_Detail_ForPreview(entity);
            if (dt.Rows.Count > 0)
            {
                Session["dtsales"] = dt;
                Session["ReportPath"] = "~/Reports/rptStockReturn.rpt";

                string webUrl = "../Reports/ReportView.aspx";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenWindow", "window.open('" + webUrl + "','_newtab');", true);
            }
        }
        if (e.CommandName == "Delete")
        {
            Label lblApprovedStatus = (Label)row.Cells[5].FindControl("lblApprovedStatus");
            if (lblApprovedStatus.Text == "Y")
            {
                lblMessage.Text = "Can not be Deleted";
            }
            else
            {
                Label lblStockReturnID = (Label)row.Cells[1].FindControl("lblStockReturnID");
                StockReturn_BO entity = new StockReturn_BO();
                entity.StockReturnID = lblStockReturnID.Text;
                lblMessage.Text = BILL.DeleteStockReturn(entity);
                GridView2.DataSource = null;
                GridView2.DataBind();
            }
        }
    }


    protected void Button1_Click(object sender, EventArgs e)
    {
        T_PROD entity = new T_PROD();        
        entity.PROD_WGPG = ddlSearchProductCategory.SelectedItem.Value.ToString();
        entity.PROD_SUBCATEGORY = ddlSearchSubCategory.SelectedItem.Value.ToString();
        entity.PROD_DES = ddlSearchDescription.SelectedItem.Value.ToString();
        entity.FromDate = txtReceivedFromDate.Text;
        entity.ToDate = txtReceivedToDate.Text;
        entity.Barcode = txtSearchBarcode.Text;
        entity.FromStoreID = DropDownList1.SelectedItem.Value.ToString();
        entity.Branch = DropDownList2.SelectedItem.Value.ToString();
        entity.SearchType = "1";
        DataTable dt = BILL.GetReceiveStockReturnListReceived(entity);
        GridView3.DataSource = dt;
        GridView3.DataBind();
    }

    protected void cmdSearchDetails_Click(object sender, EventArgs e)
    {
        T_PROD entity = new T_PROD();
        entity.PROD_WGPG = ddlProductCategoryDetails.SelectedItem.Value.ToString();
        entity.PROD_SUBCATEGORY = ddlSubCategoryDetails.SelectedItem.Value.ToString();
        entity.PROD_DES = ddlDescriptionDetails.SelectedItem.Value.ToString();
        entity.FromDate = txtFromDateDetails.Text;
        entity.ToDate = txtToDateDetails.Text;
        entity.Barcode = txtIMEIDetails.Text;
        entity.FromStoreID = ddlTransferFromDetails.SelectedItem.Value.ToString();
        entity.Branch = ddlTransferToDetails.SelectedItem.Value.ToString();
        entity.SearchType = "0";
        DataTable dt = BILL.StockReturnNotReceivedDetails(entity);
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }
}