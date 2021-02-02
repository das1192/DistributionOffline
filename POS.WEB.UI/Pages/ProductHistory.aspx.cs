using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TalukderPOS.BusinessObjects;
using TalukderPOS.BLL;

public partial class Pages_ProductHistory : System.Web.UI.Page
{
    private string userID = string.Empty;
    private string userPassword = string.Empty;
    T_PRODBLL BILL = new T_PRODBLL();
    
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


    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        if (ddlSearchBranch.SelectedItem.Value.ToString() == string.Empty)
        {
            lblMessage.Text = "PLEASE SELECT A BRANCH";
        }
        else if (ddlSearchSubCategory.SelectedItem.Value.ToString() == string.Empty)
        {
            lblMessage.Text = "PLEASE SELECT A MODEL";
        }
        else if (txtFromDate.Text == string.Empty & txtToDate.Text == string.Empty)
        {
            lblMessage.Text = "PLEASE SELECT DATE FROM & TO";
        }
        else {
            lblMessage.Text = string.Empty;
            T_PROD entity = new T_PROD();
            entity.Branch = ddlSearchBranch.SelectedItem.Value.ToString();
            entity.PROD_WGPG = ddlSearchProductCategory.SelectedItem.Value.ToString();
            entity.PROD_SUBCATEGORY = ddlSearchSubCategory.SelectedItem.Value.ToString();            
            entity.FromDate = txtFromDate.Text;
            entity.ToDate = txtToDate.Text;
            DataTable dt = BILL.ProductHistory(entity);
            GridView1.DataSource = dt;
            GridView1.DataBind();
            Session["GridData"] = dt;
        }        
    }




    protected void cmdPreview_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["GridData"];
        if (dt != null)
        {
            if (dt.Rows.Count > 0)
            {
                string webUrl;
                Session["dtsales"] = dt;
                if (rbtPDF.Checked == true)
                {
                    Session["ReportPath"] = "~/Reports/rptProductHistory.rpt";
                    webUrl = "../Reports/ReportView.aspx";
                }
                else {
                    Session["ReportPath"] = "~/Reports/rptProductHistoryExcel.rpt";
                    webUrl = "../Reports/ReportsViewer.aspx";
                }                

                
                ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenWindow", "window.open('" + webUrl + "','_newtab');", true);
            }
        }  
    }
}