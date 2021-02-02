using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;

public partial class Pages_ValidateSales : System.Web.UI.Page
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
            lblMessage.Text = string.Empty;
        }
    }




    protected void btnIssue_Click(object sender, EventArgs e)
    {        
        String sql = "select StuffID from StuffInformation where StuffID='" + txtStuffID.Text.ToString() + "' and CCOM_OID='" + Session["StoreID"].ToString() + "' ";
        DataTable dt = CommonBinder.getDataTable(sql);
        if (dt.Rows.Count > 0)
        {
            Session["StuffID"] = dt.Rows[0]["StuffID"];
            Response.Redirect("~/Pages/OnCreditSales.aspx?menuhead=104");  
        }
        else {
            lblMessage.Text = "Stuff ID is not Validate";
        }
    }
    
}