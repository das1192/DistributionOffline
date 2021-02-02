using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TalukderPOS.BusinessObjects;
using TalukderPOS.BLL;

public partial class Pages_DailyLastBalanceBranch : System.Web.UI.Page
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
        entity.Branch = Session["StoreID"].ToString();
        entity.PROD_WGPG = ddlSearchProductCategory.SelectedItem.Value.ToString();
        entity.PROD_SUBCATEGORY = ddlSearchSubCategory.SelectedItem.Value.ToString();
        entity.PROD_DES = ddlSearchDescription.SelectedItem.Value.ToString();
        if (txtDate.Text == string.Empty)
        {
            entity.FromDate = DateTime.Today.Date.ToString();
        }
        else
        {
            entity.FromDate = txtDate.Text;
        }
        DataTable dt = BILL.DailyClosingBalance(entity);
        GridView1.DataSource = dt;
        GridView1.DataBind();
        var sum = dt.AsEnumerable().Sum(dr => dr.Field<Int32>("Quantity"));
        lblTotal.Text = "Total:" + sum.ToString();
    }





}