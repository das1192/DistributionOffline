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


public partial class Pages_Product_SubCategory : System.Web.UI.Page
{
    private string userID = string.Empty;
    private string userPassword = string.Empty;
    Model_BLL BILL = new Model_BLL();
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

    
    protected void btnSave_Click(object sender, EventArgs e)
    {
        Model_BO entity = new Model_BO();
        entity.OID = lblOID.Value.ToString();
        entity.CategoryID = ddlProductCategoryId.SelectedItem.Value.ToString();
        entity.SubCategoryName = txtSubCategory.Text;
        entity.Active = "1";
        entity.ShowOnDropdown = "Y";
        entity.RunningModel = "Y";
        entity.IUSER = userID;
        entity.EUSER = userID;

        string[] valid = BILL.Validation(entity);
        if (valid[0].ToString() == "True")
        {
            lblMessage.Text = valid[1].ToString();
            BILL.Add(entity);
            cmdSearch_Click(sender, e);
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
        ContainerModel.ActiveTabIndex = 0;
    }


    

    protected void gvModel_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
    {       
        Model_BO entity = new Model_BO();
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
        Model_BO entity = new Model_BO();

        String OID = gvModel.DataKeys[e.NewEditIndex].Value.ToString();
        entity = BILL.GetById(OID);
        lblOID.Value = entity.OID;
        CAS_ddlProductCategoryId.SelectedValue = entity.CategoryID;
        txtSubCategory.Text = entity.SubCategoryName;
       
        ContainerModel.ActiveTabIndex = 1;
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
        txtSubCategory.Text = string.Empty;
        lblOID.Value = string.Empty;
    }


    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        Model_BO entity = new Model_BO();
        entity.CategoryID = ddlSearchProductCategory.SelectedItem.Value.ToString();
        entity.ShowOnDropdown = "Y";
        entity.RunningModel = "Y";
        entity.Shop_id = Shop_id.ToString();
        DataTable dt = BILL.BindList(entity);
        Session["GridData"] = dt;
        BindList();
    }




  






   




}