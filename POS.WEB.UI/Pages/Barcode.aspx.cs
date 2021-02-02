using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TalukderPOS.BusinessObjects;
using TalukderPOS.BLL;

public partial class Pages_Barcode : System.Web.UI.Page
{
    private string userID = string.Empty;
    private string userPassword = string.Empty;    
    Barcode_BILL BILL = new Barcode_BILL();

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
        String found= BILL.GetBarcode(txtBarcode.Text.ToString());
        if (found == string.Empty)
        {
            found = BILL.GetBarcodeForPROD_DES(ddlDescription.SelectedItem.Value.ToString());
            if (found == string.Empty)
            {
                if (ddlDescription.SelectedItem.Value.ToString() != string.Empty)
                {
                    Barcode_BO BO = new Barcode_BO();
                    BO.PROD_DES = ddlDescription.SelectedItem.Value.ToString();
                    BO.BARCODE = txtBarcode.Text.ToString();
                    BILL.Add(BO);
                    txtBarcode.Text = string.Empty;
                    lblmessagedtail.Text = "Saved Successfully...";
                    ddlDescription_SelectedIndexChanged(sender, e);
                }
                else
                {
                    lblmessagedtail.Text = "Select Product Description...";
                }
            }
            else {
                lblmessagedtail.Text = "Barcode Already Inserted For This Model...";    
            }
        }
        else
        {
            lblmessagedtail.Text = "This Barcode Already Inserted...";    
        }
        
    }


    protected void gvT_WGPG_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
    {
        Int32 Id = Convert.ToInt32(gvT_WGPG.DataKeys[e.RowIndex].Value);
        BILL.Delete(Id.ToString());
        ddlDescription_SelectedIndexChanged(sender,e);
        lblmessagedtail.Text = "Delete Successfully...";
    }


    protected void ddlDescription_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtBarcode.Text = BILL.GetBarcodeForPROD_DES(ddlDescription.SelectedItem.Value.ToString());
        gvT_WGPG.DataSource = BILL.Gettable(ddlDescription.SelectedItem.Value.ToString());
        gvT_WGPG.DataBind();
    }
        
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        Barcode_BO entity = new Barcode_BO();
        entity.PROD_WGPG = ddlSearchBrand.SelectedItem.Value.ToString();
        entity.PROD_SUBCATEGORY = ddlSearchModel.SelectedItem.Value.ToString();
        entity.PROD_DES = ddlSearchColor.SelectedItem.Value.ToString();
        DataTable dt = BILL.Search(entity);
        Session["GridData"] = dt;
        BindList();

    }

    private void BindList()
    {
        gvBarcode.DataSource = Session["GridData"];
        gvBarcode.DataBind();
    }

    protected void gvBarcode_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
    {
        BILL.Delete(gvBarcode.DataKeys[e.RowIndex].Value.ToString());        
        BindList();
    }
    
    

    protected void gvBarcode_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
    {
        gvBarcode.PageIndex = e.NewPageIndex;
        BindList();
    }



    protected void cmdPreview_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["GridData"];
        if (dt.Rows.Count > 0)
        {
            Session["dtsales"] = dt;
            Session["ReportPath"] = "~/Reports/rptBarcode.rpt";

            string webUrl = "../Reports/ReportView.aspx";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenWindow", "window.open('" + webUrl + "','_newtab');", true);
        }
    }



}