using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using TalukderPOS.BusinessObjects;
using TalukderPOS.BLL;

public partial class Pages_SalesReportForAdmin : System.Web.UI.Page
{
    private string userID = string.Empty;
    private string userPassword = string.Empty;   
      

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
            //txtfDate.Text = DateTime.Today.Date.ToString("yyyy-MM-dd");
            //txtReceiveDate.Text = DateTime.Today.Date.ToString("yyyy-MM-dd");
        }
    }

    protected void cmdPreview_Click(object sender, EventArgs e)
    {
        SalesReport_BO BO = new SalesReport_BO();
        SalesReport_BILL BILL = new SalesReport_BILL();

        BO.StoreID = ddlTransferTo.SelectedItem.Value.ToString();
        if (txtfDate.Text.ToString() == string.Empty)
        {
            BO.DateFrom = DateTime.Today.Date.ToString("yyyy-MM-dd");
        }
        else {
            BO.DateFrom = txtfDate.Text.ToString();
        }

        if (txtReceiveDate.Text.ToString() == string.Empty)
        {
            BO.DateTo = DateTime.Today.Date.ToString("yyyy-MM-dd");
        }
        else {
            BO.DateTo = txtReceiveDate.Text.ToString();
        }

        string webUrl = string.Empty;

        if (BO.DateFrom != string.Empty && BO.DateTo != string.Empty)
        {
            lblMessage.Text = string.Empty;
            if (rbtSalesItemsReport.Checked == true)
            {
                Session["dtsales"] = BILL.SalesItems(BO);
                Session["ReportPath"] = "~/Reports/SalesRpt.rpt";
                webUrl = "../Reports/ReportView.aspx";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenWindow", "window.open('" + webUrl + "','_newtab');", true);
            }
            else if (rbtSalesSummaryReport.Checked == true)
            {
                if (BO.StoreID == string.Empty)
                {
                    lblMessage.Text = "Please Select a Branch";
                    lblMessage.ForeColor = Color.Red;
                }
                else {
                    lblMessage.Text = string.Empty;
                    Session["dtsales"] = BILL.SalesSummary(BO);
                    Session["ReportPath"] = "~/Reports/rptSalesSummary.rpt";
                    webUrl = "../Reports/ReportView.aspx";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenWindow", "window.open('" + webUrl + "','_newtab');", true);
                }                
            }
            else if (rbtSalesDetailsReport.Checked == true)
            {
                if (BO.StoreID == string.Empty)
                {
                    lblMessage.Text = "Please Select a Branch";
                    lblMessage.ForeColor = Color.Red;
                }
                else
                {
                    lblMessage.Text = string.Empty;
                    Session["dtsales"] = BILL.SalesReport(BO);
                    Session["ReportPath"] = "~/Reports/rptSalesReport.rpt";                    
                    webUrl = "../Reports/ReportView.aspx";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenWindow", "window.open('" + webUrl + "','_newtab');", true);
                }
            }
            else if (rbtSalesComboReport.Checked == true)
            {
                if (BO.StoreID == string.Empty)
                {
                    lblMessage.Text = "Please Select a Branch";
                    lblMessage.ForeColor = Color.Red;
                }
                else
                {
                    lblMessage.Text = string.Empty;
                    Session["dtsales"] = BILL.SalesComboReport(BO);
                    Session["ReportPath"] = "~/Reports/rptComboReport.rpt";
                    webUrl = "../Reports/ReportView.aspx";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenWindow", "window.open('" + webUrl + "','_newtab');", true);
                }
            }

            else if (rbtBarphoneComboReport.Checked == true)
            {
                if (BO.StoreID == string.Empty)
                {
                    lblMessage.Text = "Please Select a Branch";
                    lblMessage.ForeColor = Color.Red;
                }
                else
                {
                    lblMessage.Text = string.Empty;
                    Session["dtsales"] = BILL.BarphoneComboReport(BO);
                    Session["ReportPath"] = "~/Reports/rptBarphoneComboReport.rpt";
                    webUrl = "../Reports/ReportView.aspx";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenWindow", "window.open('" + webUrl + "','_newtab');", true);
                }
            }

            if (rbtConsolidedSalesSummary.Checked == true)
            {
                Session["dtsales"] = BILL.DailyConsolidedSalesSummary(BO);
                Session["ReportPath"] = "~/Reports/rptConsolidedSalesSummary.rpt";
                webUrl = "../Reports/ReportView.aspx";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenWindow", "window.open('" + webUrl + "','_newtab');", true);
            }

        }
        else {
            lblMessage.Text = "Please Select a Date";
            lblMessage.ForeColor = Color.Red;
        }
    }





    protected void cmdExport_Click(object sender, EventArgs e)
    {
        SalesReport_BO BO = new SalesReport_BO();
        SalesReport_BILL BILL = new SalesReport_BILL();

        BO.StoreID = ddlTransferTo.SelectedItem.Value.ToString();
        if (txtfDate.Text.ToString() == string.Empty)
        {
            BO.DateFrom = DateTime.Today.Date.ToString("yyyy-MM-dd");
        }
        else
        {
            BO.DateFrom = txtfDate.Text.ToString();
        }

        if (txtReceiveDate.Text.ToString() == string.Empty)
        {
            BO.DateTo = DateTime.Today.Date.ToString("yyyy-MM-dd");
        }
        else
        {
            BO.DateTo = txtReceiveDate.Text.ToString();
        }

        string webUrl = string.Empty;

        if (BO.DateFrom != string.Empty && BO.DateTo != string.Empty)
        {
            lblMessage.Text = string.Empty;
            
            
            if (rbtSalesComboReport.Checked == true)
            {
                if (BO.StoreID == string.Empty)
                {
                    lblMessage.Text = "Please Select a Branch";
                    lblMessage.ForeColor = Color.Red;
                }
                else
                {
                    lblMessage.Text = string.Empty;
                    Session["dtsales"] = BILL.SalesComboReport(BO);
                    Session["ReportPath"] = "~/Reports/rptComboReport.rpt";
                    webUrl = "../Reports/ReportsViewer.aspx";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenWindow", "window.open('" + webUrl + "','_newtab');", true);
                    
                }
            }

           

            

        }
        else
        {
            lblMessage.Text = "Please Select a Date";
            lblMessage.ForeColor = Color.Red;
        }
    }
}