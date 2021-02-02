using System;
using System.Data.SqlClient;
using System.Web;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using TalukderPOS.BusinessObjects;
using System.Configuration;
using TalukderPOS.BLL;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class Pages_SalesSummary : System.Web.UI.Page
{
    private string userID = "";
    private string userPassword = "";
    string connStr = ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString;
    SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
    SqlCommand cmd;
    SqlDataAdapter da = new SqlDataAdapter();
    string sql;


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

            CommonBinder.BindTblProductCategoryList(ddlSearchProductCategory);
        }

    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        sql = "select CCOM_TEXT,CCOM_ACTV,CCOM_ADD1 from T_CCOM";
        DataTable dt = CommonBinder.getDataTable(sql);

        foreach (DataColumn col in dt.Columns){
            BoundField bfield = new BoundField();
            bfield.DataField = col.ColumnName;
            bfield.HeaderText = col.ColumnName;
            gvStockBalance.Columns.Add(bfield);
        }
        
        gvStockBalance.DataSource = dt;
        gvStockBalance.DataBind();
    }


    protected void ddlSearchProductCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        sql = "select OID,SubCategoryName as Name from SubCategory where CategoryID='" + ddlSearchProductCategory.SelectedValue + "' and Active='1' and ShowOnDropdown='Y' ";
        CommonBinder.BindDropdownList(ddlSearchSubCategory, sql);
    }


    protected void ddlSearchSubCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        sql = "select OID,Description as Name from Description where SubCategoryID='" + ddlSearchSubCategory.SelectedValue + "' and Active='1'";
        CommonBinder.BindDropdownList(ddlSearchDescription, sql);
    }



}