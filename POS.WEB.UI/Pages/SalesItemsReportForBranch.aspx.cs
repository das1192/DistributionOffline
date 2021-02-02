using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TalukderPOS.BusinessObjects;
using TalukderPOS.BLL;

public partial class Pages_SalesItemsReportForBranch : System.Web.UI.Page
{
    private string userID = string.Empty;
    private string userPassword = string.Empty;
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

        if (!Page.IsPostBack)
        {
        }
    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        T_PROD entity = new T_PROD();
        entity.Branch = Session["StoreID"].ToString();
        entity.PROD_WGPG = ddlSearchProductCategory.SelectedItem.Value.ToString();
        entity.PROD_SUBCATEGORY = ddlSearchSubCategory.SelectedItem.Value.ToString();
        entity.PROD_DES = ddlSearchDescription.SelectedItem.Value.ToString();
        entity.FromDate = txtFromDate.Text;
        entity.ToDate = txtToDate.Text;
        entity.Barcode = txtBarcode.Text;
        DataTable dt = BILL.SalesItemsList(entity);
        Session["GridData"] = dt;
        GridView1.DataSource = dt;
        GridView1.DataBind();   
    }


    protected void cmdPreview_Click(object sender, EventArgs e)
    {
        string webUrl = string.Empty;
        DataTable dt = (DataTable)Session["GridData"];
        if (dt != null)
        {
            if (dt.Rows.Count > 0)
            {               
                Session["dtsales"] = dt;
                Session["ReportPath"] = "~/Reports/rptExcelSalesItemReport.rpt";
                webUrl = "../Reports/ReportsViewer.aspx";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenWindow", "window.open('" + webUrl + "','_newtab');", true);                
            }
        }
    }




}