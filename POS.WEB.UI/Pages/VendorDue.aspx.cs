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
using System.Drawing;

public partial class Pages_VendorDue : System.Web.UI.Page
{
    private string userID = string.Empty;
    private string userPassword = string.Empty;
    T_PRODBLL BILL = new T_PRODBLL();
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
      
    
   

    protected void cmdSearchForprice_Click(object sender, EventArgs e)
    {
        
        

        T_PROD entity = new T_PROD();
        entity.Branch = Session["StoreID"].ToString();
        entity.Vendor_ID = ddlVendorID.SelectedItem.Value.ToString();



        DataTable dt = BILL.RetailerDueReport(entity);
        Session["GridData"] = dt;
        BindList1();
    }
    
    protected void btnSubmitDiscount_Click(object sender, EventArgs e) //Modal Window Delete Product
    {
        
    }
    private void BindList1()
    {
        gvVendorDue.DataSource = Session["GridData"];
        gvVendorDue.DataBind();
    }

   
}