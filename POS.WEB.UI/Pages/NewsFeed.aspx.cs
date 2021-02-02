using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TalukderPOS.BusinessObjects;
using TalukderPOS.BLL;

public partial class Pages_NewsFeed : System.Web.UI.Page
{

    private string userID = string.Empty;
    private string userPassword = string.Empty;
    NewsFeed_BILL BILL = new NewsFeed_BILL();
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

    protected void cmdSave_Click(object sender, EventArgs e)
    {
        NewsFeed_BO entity = new NewsFeed_BO();
        entity.OID = lblOID.Value.ToString();
        if (Convert.ToDateTime(txtFromDate.Text) > Convert.ToDateTime(txtToDate.Text))
        {
            lblMessage.Text = "To Date must be greater than From Date";
        }
        else
        {
            entity.FromDate = txtFromDate.Text;
            entity.ToDate = txtToDate.Text;           
            entity.BranchOID = ddlSearchBranch.SelectedItem.Value.ToString();
            entity.Message = txtNews.InnerText.Trim();
            entity.ActiveStatus = "1";
            entity.IUSER = userID.Trim();
            entity.IDAT = DateTime.Today.Date.ToString();
            entity.EUSER = userID.Trim();
            entity.EDAT = DateTime.Today.Date.ToString();
            BILL.Add(entity);
            lblMessage.Text = ContextConstant.SAVED_SUCCESS;
            Clear();
        }
    }

    private void Clear()
    {
        lblOID.Value = string.Empty;
        txtFromDate.Text = string.Empty;
        txtToDate.Text = string.Empty;
        txtNews.InnerText = "";
        Cascadingdropdown1.SelectedValue = string.Empty;
    }

    protected void cmdCancel_Click(object sender, EventArgs e)
    {
        Clear();
        lblMessage.Text = string.Empty;
        
    }
}