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

public partial class Pages_CustomerCare : System.Web.UI.Page
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
            Session["dtList"] = null;
        }
    }
    protected void cmdProcess_Click(object sender, EventArgs e)
    {
        T_PROD entity = new T_PROD();
        entity.Branch = Session["StoreID"].ToString();
        entity.PROD_WGPG = ddlSearchProductCategory.SelectedItem.Value.ToString();
        entity.PROD_SUBCATEGORY = ddlSearchSubCategory.SelectedItem.Value.ToString();
        entity.PROD_DES = ddlSearchDescription.SelectedItem.Value.ToString();
        entity.FromDate = txtFromDate.Text;
        entity.ToDate = txtToDate.Text;
        entity.ProductPriceFrom = txtProductPriceFrom.Text;
        entity.ProductPriceTo = txtProductPriceTo.Text;

        DataTable dt = BILL.CustomerCare(entity);
        gvDetailsInvoice.DataSource = dt;
        gvDetailsInvoice.DataBind();
        Session["dtList"] = dt;              
    }

    protected void cmdExport_Click(object sender, EventArgs e)
    {   
        DataTable dt = (DataTable)Session["dtList"];
        if (dt != null)
        {
            if (dt.Rows.Count > 0)
            {
                string webUrl;
                Session["dtsales"] = dt;
                webUrl = "../Reports/ReportsViewer.aspx";
                Session["ReportPath"] = "~/Reports/rptCustomerCare.rpt";                
                ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenWindow", "window.open('" + webUrl + "','_newtab');", true);
            }   
        }
    } 





    


   


}