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

public partial class Pages_ProductPrice : System.Web.UI.Page
{
    private string userID = string.Empty;
    private string userPassword = string.Empty;
    ProductPrice_BILL BILL = new ProductPrice_BILL();   

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
        ProductPrice_BO entity = new ProductPrice_BO();
        entity.OID = lblOID.Value.ToString();
        entity.CategoryOID = ddlProductCategory.SelectedItem.Value.ToString();
        entity.SubCategoryOID = ddlSubCategory.SelectedItem.Value.ToString();
        entity.DescriptionOID = ddlDescription.SelectedItem.Value.ToString();
        entity.PurchasePrice = txtCOST_PRICE.Text;
        entity.SalePrice = txtSalePrice.Text;
        entity.IUSER = userID;
        entity.EUSER = userID;

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
        ContainerProductPrice.ActiveTabIndex = 0;
    }


  

    protected void gvProductPrice_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
        {

        }
    }

    protected void gvProductPrice_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
    {
        ProductPrice_BO entity = new ProductPrice_BO();
        entity.OID = gvProductPrice.DataKeys[e.RowIndex].Value.ToString();
        entity.EUSER = userID;
        BILL.Delete(entity);
        BindList();
        ContainerProductPrice.ActiveTabIndex = 0;
    }


    protected void gvProductPrice_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
    {
        e.Cancel = true;
        Clear();
        lblMessage.Text = string.Empty;
        ProductPrice_BO entity = new ProductPrice_BO();

        String OID = gvProductPrice.DataKeys[e.NewEditIndex].Value.ToString();
        entity = BILL.GetById(OID);
        lblOID.Value = entity.OID;
        CascadingDropDown4.SelectedValue = entity.CategoryOID;
        CascadingDropDown5.SelectedValue = entity.SubCategoryOID;
        CascadingDropDown6.SelectedValue = entity.DescriptionOID;
        txtCOST_PRICE.Text = entity.PurchasePrice;
        txtSalePrice.Text = entity.SalePrice;
        ContainerProductPrice.ActiveTabIndex = 1;
    }



    protected void gvProductPrice_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
    {
        gvProductPrice.PageIndex = e.NewPageIndex;
        BindList();
    }


    private void BindList()
    {
        gvProductPrice.DataSource = Session["GridData"];
        gvProductPrice.DataBind();

    }


    private void Clear()
    {
        txtCOST_PRICE.Text = string.Empty;
        txtSalePrice.Text = string.Empty;
        lblOID.Value = string.Empty;
    }


    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        ProductPrice_BO entity = new ProductPrice_BO();
        entity.CategoryOID = ddlSearchProductCategory.SelectedItem.Value.ToString();
        entity.SubCategoryOID = ddlSearchSubCategory.SelectedItem.Value.ToString();
        entity.DescriptionOID = ddlSearchDescription.SelectedItem.Value.ToString();
        DataTable dt = BILL.BindList(entity);
        Session["GridData"] = dt;
        BindList();
    }
    
    protected void cmdPreview_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["GridData"];
        if (dt.Rows.Count > 0)
        {
            Session["dtsales"] = dt;
            Session["ReportPath"] = "~/Reports/rptProductPrice.rpt";

            string webUrl = "../Reports/ReportView.aspx";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenWindow", "window.open('" + webUrl + "','_newtab');", true);
        }
    }



}