using System;
using System.Web;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using TalukderPOS.BLL;
using TalukderPOS.BusinessObjects;

public partial class Pages_BankInfo : System.Web.UI.Page
{
    private string ShopID = string.Empty;
    private string userID = string.Empty;
    private string userPassword = string.Empty;
    BankInfoBILL BILL = new BankInfoBILL();
    //CommonDAL DAL = new CommonDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            userID = Session["UserID"].ToString();
            userPassword = Session["Password"].ToString();
            ShopID = Session["StoreID"].ToString();
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
    protected void gvBank_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
    {
        e.Cancel = true;
        Clear();
        BankInfo_BO entity = new BankInfo_BO();
        String OID = gvBank.DataKeys[e.NewEditIndex].Value.ToString();
        entity = BILL.GetById(OID);

        lblOID.Value = entity.OID;
        txtBankName.Text = entity.BankName;
        txtAccountNo.Text = entity.AccountNo;
        ddlAccountStatus.SelectedValue = entity.ActiveStatus;

        ContainerBankInfo.ActiveTabIndex = 1;
        lblMessage.Text = string.Empty;
    }


    protected void gvBank_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
    {
        BankInfo_BO entity = new BankInfo_BO();
        entity.OID = gvBank.DataKeys[e.RowIndex].Value.ToString();
        entity.EUSER = userID;
        entity.ShopID = Convert.ToInt16(ShopID);
        BILL.Delete(entity);
        BindList();
    }



    protected void btnSave_Click(object sender, EventArgs e)
    {
        BankInfo_BO entity = new BankInfo_BO();
        entity.OID = lblOID.Value.ToString();
        entity.BankName = txtBankName.Text.ToString().Trim();
        entity.AccountNo = txtAccountNo.Text.Trim();//
        entity.ShopID = Convert.ToInt32(ShopID);//
        entity.ActiveStatus = ddlAccountStatus.SelectedValue.ToString();
        //entity.ActiveStatus = "1";
        entity.IUSER = userID.ToString();
        entity.EUSER = userID.ToString();
        BILL.AddBank(entity);
        Clear();
        lblMessage.Text = "SAVED SUCCESSFULLY";
        entity = null;
        BindList();
        ContainerBankInfo.ActiveTabIndex = 0;
    }

    private void BindList()
    {
        int shopOID = Convert.ToInt32(ShopID);
        DataTable dt = BILL.BindList(shopOID);
        
        gvBank.DataSource = null;
        gvBank.DataSource = dt;
        gvBank.DataBind();
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
        txtBankName.Text = string.Empty;
        txtAccountNo.Text = string.Empty;
        ddlAccountStatus.SelectedIndex = -1;
    }
}