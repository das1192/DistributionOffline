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

public partial class Pages_Cashinout : System.Web.UI.Page
{
    private string userID = string.Empty;
    private string userPassword = string.Empty;
    SearchInvoiceForAdmin_BILL BILL = new SearchInvoiceForAdmin_BILL();
    private string Shop_id = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            userID = Session["UserID"].ToString();
            userPassword = Session["Password"].ToString();
            Shop_id = Session["StoreID"].ToString();
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


    protected void cmdProcess_Click(object sender, EventArgs e)
    {        
        T_PROD entity = new T_PROD();
        entity.Branch = Shop_id.ToString();
        entity.FromDate = txtFromDate.Text;
        entity.ToDate = txtToDate.Text;

        DataTable dt = BILL.GetCASHINOUT(entity);
        gvCashINOUT.DataSource = dt;
        gvCashINOUT.DataBind();
    }

   






}