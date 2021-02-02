using System;
using System.Web;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using TalukderPOS.BLL;
using TalukderPOS.BusinessObjects;

public partial class Pages_DiscountType : System.Web.UI.Page
{
    private string userID = string.Empty;
    private string userPassword = string.Empty;
    DiscountTypeBILL BILL = new DiscountTypeBILL();

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


    protected void gvDiscountType_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
    {
        e.Cancel = true;
        Clear();
        DiscountType_BO entity = new DiscountType_BO();
        String OID = gvDiscountType.DataKeys[e.NewEditIndex].Value.ToString();
        entity = BILL.GetById(OID);
        lblOID.Value = entity.OID;
        txtDiscountType.Text = entity.DiscountType;
        ContainerDiscountType.ActiveTabIndex = 1;
        lblMessage.Text = string.Empty;
    }


    protected void gvDiscountType_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
    {
        DiscountType_BO entity = new DiscountType_BO();
        entity.OID = gvDiscountType.DataKeys[e.RowIndex].Value.ToString();
        entity.EUSER = userID;
        BILL.Delete(entity);
        BindList();
    }



    protected void btnSave_Click(object sender, EventArgs e)
    {
        DiscountType_BO entity = new DiscountType_BO();
        entity.OID = lblOID.Value.ToString();
        entity.DiscountType = txtDiscountType.Text.Trim();
        entity.ActiveStatus = "1";
        entity.IUSER = userID.ToString();
        entity.IDAT = DateTime.Today.Date.ToString();
        entity.EUSER = userID.ToString();
        entity.EDAT = DateTime.Today.Date.ToString();
        BILL.Add(entity);
        Clear();
        lblMessage.Text = "SAVED SUCCESSFULLY";
        entity = null;
        BindList();
    }

    private void BindList()
    {
        DataTable dt = BILL.BindList();
        gvDiscountType.DataSource = dt;
        gvDiscountType.DataBind();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Clear();
        ContainerDiscountType.ActiveTabIndex = 0;
        lblMessage.Text = string.Empty;
    }


    private void Clear()
    {
        lblOID.Value = string.Empty;
        txtDiscountType.Text = string.Empty;
    }
}