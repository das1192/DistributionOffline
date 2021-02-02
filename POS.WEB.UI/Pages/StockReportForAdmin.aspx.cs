using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TalukderPOS.BusinessObjects;
using TalukderPOS.BLL;


public partial class Pages_StockReportForAdmin : System.Web.UI.Page
{
    private string userID = string.Empty;
    private string userPassword = string.Empty;
    StockReportForAdmin_BILL BILL = new StockReportForAdmin_BILL();

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
        entity.Branch = ddlSearchBranch.SelectedItem.Value.ToString();
        entity.PROD_WGPG = ddlSearchProductCategory.SelectedItem.Value.ToString();
        entity.PROD_SUBCATEGORY = ddlSearchSubCategory.SelectedItem.Value.ToString();
        entity.PROD_DES = ddlSearchDescription.SelectedItem.Value.ToString();
        entity.FromDate = txtFromDate.Text;
        entity.ToDate = txtToDate.Text;

        if (rbtDetails.Checked == true)
        {
            GridView2.DataSource = null;
            GridView2.DataBind();
            GridView2.Visible = false;
            GridView1.Visible = true;
            DataTable dt = BILL.CurrentStockInBranch(entity);
            GridView1.DataSource = dt;
            GridView1.DataBind();
            Session["dtList"] = dt;
            var sum = dt.AsEnumerable().Sum(dr => dr.Field<Int32>("Quantity"));
            lblTotal.Text = "Total:" + sum.ToString();
        }
        else
        {
            GridView1.DataSource = null;
            GridView1.DataBind();
            GridView1.Visible = false;
            GridView2.Visible = true;
            DataTable dt = BILL.StockQuantitySummary(entity);
            GridView2.DataSource = dt;
            GridView2.DataBind();

            var sum = dt.AsEnumerable().Sum(dr => dr.Field<Int32>("Total"));
            lblTotal.Text = "Total:" + sum.ToString();
        }

    }

    protected void cmdExport_Click(object sender, EventArgs e)
    {

        DataTable dt = (DataTable)Session["dtList"];
        if (dt != null)
        {
            if (dt.Rows.Count > 0)
            {
                Session["dtsales"] = dt;
                Session["ReportPath"] = "~/Reports/rptCurrentStockReportForAdmin.rpt";

                string webUrl = "../Reports/ReportView.aspx";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenWindow", "window.open('" + webUrl + "','_newtab');", true);
            }
        }
    }



    protected void cmdSearchTotal_Click(object sender, EventArgs e)
    {
        T_PROD entity = new T_PROD();
        entity.Branch = ddlBranchTotal.SelectedItem.Value.ToString();
        entity.PROD_WGPG = ddlCategoryTotal.SelectedItem.Value.ToString();
        entity.PROD_SUBCATEGORY = ddlSubCategoryTotal.SelectedItem.Value.ToString();
        entity.PROD_DES = ddlDescriptionTotal.SelectedItem.Value.ToString();
        entity.FromDate = txtFromDateTotal.Text;
        entity.ToDate = txtToDateTotal.Text;
        DataTable dt = BILL.TotalStock(entity);
        GridView3.DataSource = dt;
        GridView3.DataBind();
        var sum = dt.AsEnumerable().Sum(dr => dr.Field<Int32>("Quantity"));
        lblTotalStock1.Text = "Total:" + sum.ToString();
    }



}