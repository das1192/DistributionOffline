using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TalukderPOS.BLL;
using TalukderPOS.BusinessObjects;

public partial class Pages_DiscountReference : System.Web.UI.Page
{
    private string userID = string.Empty;
    private string userPassword = string.Empty;
    DiscountReference_BILL BILL = new DiscountReference_BILL();   

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
    protected void btnSave_Click(object sender, EventArgs e)
    {
        DiscountReference_BO entity = new DiscountReference_BO();
        entity.OID = lblOID.Value.ToString();
        entity.DiscountTypeOID = ddlDiscountType.SelectedItem.Value.ToString();
        entity.Reference = txtReference.Text;
        entity.Email = txtEmail.Text.Trim();
        entity.ActiveStatus = "1";
        entity.IUSER = userID;
        entity.IDAT = DateTime.Today.Date.ToString();
        entity.EUSER = userID;
        entity.EDAT = DateTime.Today.Date.ToString();

        string[] valid = BILL.Validation(entity);
        if (valid[0].ToString() == "True")
        {
            lblMessage.Text = valid[1].ToString();
            BILL.Add(entity);
            lblMessage.Text = "SAVED SUCCESSFULLY";
            Clear();
        }
        else
        {
            lblMessage.Text = valid[1].ToString();
        }
    }


    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Clear();
        lblMessage.Text = string.Empty;
        ContainerDiscountReference.ActiveTabIndex = 0;
    }


  

    protected void gvModel_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
    {
        DiscountReference_BO entity = new DiscountReference_BO();
        entity.OID = gvModel.DataKeys[e.RowIndex].Value.ToString();
        entity.EUSER = userID;
        BILL.Delete(entity);
        cmdSearch_Click(sender, e);
    }


    protected void gvModel_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
    {
        e.Cancel = true;
        Clear();
        lblMessage.Text = string.Empty;
        DiscountReference_BO entity = new DiscountReference_BO();
        String OID = gvModel.DataKeys[e.NewEditIndex].Value.ToString();
        entity = BILL.GetById(OID);

        lblOID.Value = entity.OID;
        CAS_ddlDiscountType.SelectedValue = entity.DiscountTypeOID;
        txtReference.Text = entity.Reference;
        txtEmail.Text = entity.Email;
        ContainerDiscountReference.ActiveTabIndex = 1;
    }



    protected void gvModel_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
    {
        gvModel.PageIndex = e.NewPageIndex;
        BindList();
    }


    private void BindList()
    {
        gvModel.DataSource = Session["GridData"];
        gvModel.DataBind();

    }


    private void Clear()
    {
        lblOID.Value = string.Empty;
        txtReference.Text = string.Empty;
        txtEmail.Text = string.Empty;
        
    }


    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        DiscountReference_BO entity = new DiscountReference_BO();
        entity.DiscountTypeOID = ddlSearchDiscountType.SelectedItem.Value.ToString();
        DataTable dt = BILL.BindList(entity);
        Session["GridData"] = dt;
        BindList();
    }
}