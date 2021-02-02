using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TalukderPOS.BusinessObjects;
using TalukderPOS.BLL;

public partial class Pages_StockReportBranch : System.Web.UI.Page
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
    protected void gv_Purchase_History_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        BindList3();
    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        T_PROD entity = new T_PROD();
        entity.Branch = Session["StoreID"].ToString();
        entity.PROD_WGPG = ddlSearchProductCategory.SelectedItem.Value.ToString();
        entity.PROD_SUBCATEGORY = ddlSearchSubCategory.SelectedItem.Value.ToString();
        entity.PROD_DES = ddlSearchDescription.SelectedItem.Value.ToString();

        GridView1.Visible = true;
        DataTable dt = BILL.CurrentStockValue(entity);
        Session["GridData"] = dt;
        BindList3();
        
        var sum = dt.AsEnumerable().Sum(dr => dr.Field<Int32>("Quantity"));
        lblTotalQuantity.Text = "<span class='label label-primary'>Total Quantity : " + sum.ToString() + "</span>";
        var sum2 = dt.AsEnumerable().Sum(dr => dr.Field<Int32>("Stockvalue"));
        lblTotal.Text = "<span class='label label-danger'>Total Stock Value : " + sum2.ToString() + "</span>";


    }
    private void BindList3()
    {
        GridView1.DataSource = Session["GridData"];
        GridView1.DataBind();
    }




}