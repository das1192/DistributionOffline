using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TalukderPOS.BusinessObjects;
using TalukderPOS.BLL;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Configuration;


public partial class Pages_SearchInvoiceForAdmin : System.Web.UI.Page
{
    private string userID = string.Empty;
    private string userPassword = string.Empty;
    SearchInvoiceForAdmin_BILL BILL = new SearchInvoiceForAdmin_BILL();


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
            cmdExport.Visible = false;
        }
    }


    protected void cmdProcess_Click(object sender, EventArgs e)
    {
        Label26.Text = string.Empty;
        T_PROD entity = new T_PROD();
        entity.Branch = ddlSearchBranch.SelectedItem.Value.ToString();
        entity.FromDate = txtFromDate.Text;
        entity.ToDate = txtToDate.Text;
        if (rbtYes.Checked == true) {
            entity.DropStatus = "1";
        }
        else if (rbtNo.Checked == true) {
            entity.DropStatus = "0";
        }
        if (rbtInvoiceList.Checked == true)
        {
            gvInvoiceList.Visible = true;
            gvDetailsInvoice.Visible = false;
            DataTable dt = BILL.GetInvoiceList(entity);
            gvInvoiceList.DataSource = dt;
            gvInvoiceList.DataBind();
            cmdExport.Visible = false;
        }
        else if (rbtInvoiceDetailDatabase.Checked == true)
        {
            gvDetailsInvoice.Visible = true;
            gvInvoiceList.Visible = false;
            DataTable dt = BILL.DetailDatabase(entity);
            gvDetailsInvoice.DataSource = dt;
            gvDetailsInvoice.DataBind();
            Session["dtList"] = dt;
            cmdExport.Visible = true;
        }
    }

    //protected void gvInvoiceList_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
    //{
    //    string InvoiceNo = ((System.Web.UI.WebControls.Label)gvInvoiceList.Rows[e.NewEditIndex].FindControl("lblInvoiceNo")).Text;
    //    DataTable dt = BILL.SPP_GetInvoice(InvoiceNo);
    //    Session["dtsales"] = dt;
    //    Session["ReportPath"] = "~/Reports/rptInvoice.rpt";
    //    string webUrl = "../Reports/ReportView.aspx";
    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenWindow", "window.open('" + webUrl + "','_newtab');", true);
    //}

    protected void StockReturn_Details(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
    {        
        GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
        if (e.CommandName == "Invoice")
        {
            Label lblInvoiceNo = (Label)row.Cells[0].FindControl("lblInvoiceNo");
            DataTable dt = BILL.SPP_GetInvoice(lblInvoiceNo.Text);
            Session["dtsales"] = dt;
            Session["ReportPath"] = "~/Reports/rptInvoice.rpt";
            string webUrl = "../Reports/ReportView.aspx";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenWindow", "window.open('" + webUrl + "','_newtab');", true);
        }

        if (e.CommandName == "InvoiceNo")
        {
            Label lblInvoiceNo = (Label)row.Cells[0].FindControl("lblInvoiceNo");
            Label lblInvoiceDate = (Label)row.Cells[0].FindControl("lblIDAT");
            Response.Redirect("~/Pages/ChangeInvoiceDate.aspx?InvoiceNo=" + lblInvoiceNo.Text + "&InvoiceDate="+ lblInvoiceDate.Text +" "); 
        }
       
       
    }


    protected void cmdExport_Click(object sender, EventArgs e)
    {
        Session["dtsales"] = Session["dtList"];
        Session["ReportPath"] = "~/Reports/rptInvoiceDetails.rpt";

        string webUrl = "../Reports/ReportsViewer.aspx";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenWindow", "window.open('" + webUrl + "','_newtab');", true);
    }


    protected string RenderPriority(object dbValue)
    {
        string strReturn = string.Empty;
        if (dbValue != null)
        {
            int intValue = Convert.ToInt16(dbValue);
            switch (intValue)
            {
                case 0:
                    strReturn = "N";
                    break;
                case 1:
                    strReturn = "Y";
                    break;
            }
        }
        return strReturn;
    }
}