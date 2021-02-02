using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TalukderPOS.BusinessObjects;
using TalukderPOS.BLL;

public partial class Pages_StockReportFaultyForAdmin : System.Web.UI.Page
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
        entity.FromDate = txtFromDate.Text;
        entity.ToDate = txtToDate.Text;
        GridView1.Visible = true;
        DataTable dt = BILL.TotalFaultyStock(entity);
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }
}