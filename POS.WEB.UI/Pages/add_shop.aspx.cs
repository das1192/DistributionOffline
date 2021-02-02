using System;
using System.Web;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using TalukderPOS.BLL;
using TalukderPOS.BusinessObjects;

public partial class Pages_add_shop : System.Web.UI.Page
{
    private string userID = string.Empty;
    private string userPassword = string.Empty;

    AddShopBILL BILL = new AddShopBILL();

    
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
           
                BindList();
            
        }
    }
    private void BindList()
    {
        DataTable dt = BILL.BindList();
        gvShop.DataSource = dt;
        gvShop.DataBind();
    }
    protected void gvShop_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
    {
        e.Cancel = true;
        Clear();
        AddShop_BO entity = new AddShop_BO();
        String OID = gvShop.DataKeys[e.NewEditIndex].Value.ToString();
        entity = BILL.GetById(OID);

        lblOID.Value = entity.OID;
        txtShopName.Text = entity.ShopName;
        ContainerBankInfo.ActiveTabIndex = 1;
        lblMessage.Text = string.Empty;
    }


    protected void gvShop_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
    {
        AddShop_BO entity = new AddShop_BO();
        entity.OID = gvShop.DataKeys[e.RowIndex].Value.ToString();
        entity.EUSER = userID;
        BILL.Delete(entity);
        BindList();
    }



    protected void btnSave_Click(object sender, EventArgs e)
    {
        AddShop_BO entity = new AddShop_BO();
        entity.OID = lblOID.Value.ToString();
        entity.ShopName = txtShopName.Text.ToString();
        entity.ActiveStatus = "1";
        entity.IUSER = userID.ToString();
        entity.EUSER = userID.ToString();
        BILL.Add(entity);
        Clear();
        lblMessage.Text = "SAVED SUCCESSFULLY";
        entity = null;
        BindList();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Clear();
        ContainerBankInfo.ActiveTabIndex = 0;
        lblMessage.Text = string.Empty;
    }


    private void Clear()
    {
        lblOID.Value = string.Empty;
        txtShopName.Text = string.Empty;
    }
   
    
}