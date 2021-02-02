using System;
using System.Web;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using TalukderPOS.BusinessObjects;
using TalukderPOS.BLL;


public partial class Reports_SalesReport : System.Web.UI.Page
{    
    private string userID = "";
    private string userPassword = "";
    SalesReport_BILL BILL = new SalesReport_BILL();

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

    }


    protected void btnAdd_Click(object sender, EventArgs e)
    {        
        SalesReport_BO BO = new SalesReport_BO();        
        BO.StoreID = Session["StoreID"].ToString();
        string webUrl = string.Empty;
        if (txtfDate.Text != string.Empty && txtReceiveDate.Text != string.Empty & BO.StoreID !=string.Empty)
        {
            BO.DateFrom = txtfDate.Text.ToString();
            BO.DateTo = txtReceiveDate.Text.ToString();
            BO.PROD_WGPG = ddlSearchProductCategory.SelectedItem.Value.ToString();
            BO.PROD_SUBCATEGORY = ddlSearchSubCategory.SelectedItem.Value.ToString();
            BO.PROD_DES = ddlSearchDescription.SelectedItem.Value.ToString();
            DataTable dtsales =   BILL.SalesItems(BO);
            Session["dtsales"] = dtsales;
            

            if (rbtPDF.Checked == true)
            {
                Session["ReportPath"] = "~/Reports/SalesRpt.rpt";
                webUrl = "../Reports/ReportView.aspx";
            }
            else {
                Session["ReportPath"] = "~/Reports/SalesRptExcel.rpt";
                webUrl = "../Reports/ReportsViewer.aspx";
            }            
            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenWindow", "window.open('" + webUrl + "','_newtab');", true);
        }
    }




    protected void cmdSummary_Click(object sender, EventArgs e)
    {
        SalesReport_BO BO = new SalesReport_BO();        
        BO.StoreID = Session["StoreID"].ToString();
        if (txtfDate.Text != string.Empty & txtReceiveDate.Text != string.Empty & BO.StoreID != string.Empty)
        {                
            BO.DateFrom = txtfDate.Text.ToString();
            BO.DateTo = txtReceiveDate.Text.ToString();
            BO.PROD_WGPG = ddlSearchProductCategory.SelectedItem.Value.ToString();
            BO.PROD_SUBCATEGORY = ddlSearchSubCategory.SelectedItem.Value.ToString();
            BO.PROD_DES = ddlSearchDescription.SelectedItem.Value.ToString();

            Session["dtsales"] = BILL.SalesSummary(BO);
            Session["ReportPath"] = "~/Reports/rptSalesSummary.rpt";

            string webUrl = "../Reports/ReportView.aspx";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenWindow", "window.open('" + webUrl + "','_newtab');", true);
        }        
      
    }


    protected void cmdOK_Click(object sender, EventArgs e)
    {
        SalesReport_BO BO = new SalesReport_BO();        
        BO.StoreID = Session["StoreID"].ToString();
        if (txtfDate.Text != string.Empty && txtReceiveDate.Text != string.Empty & BO.StoreID != string.Empty)
        {
            BO.DateFrom = txtfDate.Text.ToString();
            BO.DateTo = txtReceiveDate.Text.ToString();
            BO.PROD_WGPG = ddlSearchProductCategory.SelectedItem.Value.ToString();
            BO.PROD_SUBCATEGORY = ddlSearchSubCategory.SelectedItem.Value.ToString();
            BO.PROD_DES = ddlSearchDescription.SelectedItem.Value.ToString();

            Session["dtsales"] = BILL.SalesReport(BO);
            Session["ReportPath"] = "~/Reports/rptSalesReport.rpt";

            string webUrl = "../Reports/ReportView.aspx";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenWindow", "window.open('" + webUrl + "','_newtab');", true);
        }
    }




}