using System;
using System.Data;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TalukderPOS.BusinessObjects;
using TalukderPOS.BLL;

public partial class StuffInformation : System.Web.UI.Page
{
    private string userID = string.Empty;
    private string userPassword = string.Empty;
    private string Shop_id = string.Empty;
    StuffInformation_BILL BILL = new StuffInformation_BILL();


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

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        StuffInformation_BO entity = new StuffInformation_BO();
        entity.CCOM_OID = ddlSearchBranch.SelectedItem.Value.ToString();
        entity.CCOM_OID = Shop_id.ToString();
        gvStaffInformation.DataSource = BILL.StuffInformation_BindList(entity);
        gvStaffInformation.DataBind();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        StuffInformation_BO entity = new StuffInformation_BO();
        entity.OID = lblOID.Value.ToString();
        entity.StuffID = txtStuffID.Text;
        entity.Name = txtStuffName.Text;
        entity.CCOM_OID = ddlBranch.SelectedItem.Value.ToString();
        entity.MobileNumber = txtMobileNumber.Text;
        entity.AlternativeMobileNo = txtAlternativeMobileNo.Text;
        entity.EMailAddress = txtEmailAddress.Text;
        entity.AlternativeEMailAddress = txtAlternativeEmailAddress.Text;
        entity.ActiveStatus = "1";
        entity.IUSER = userID;
        entity.IDAT = DateTime.Today.Date.ToString();
        entity.EUSER = userID;
        entity.EDAT = DateTime.Today.Date.ToString();
        BILL.Add(entity);
        lblMessage.Text = "SAVED SUCCESSFULLY";
        Clear();        
    }



    protected void gvStaffInformation_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
    {
        Int32 Id = Convert.ToInt32(gvStaffInformation.DataKeys[e.RowIndex].Value);
        StuffInformation_BO entity = new StuffInformation_BO();
        entity.OID = Id.ToString();
        entity.ActiveStatus = "0";
        entity.EUSER = userID;
        entity.EDAT = DateTime.Today.Date.ToString();
        BILL.StuffInformation_Delete(entity);
        cmdSearch_Click(sender,e);
    }

    protected void gvStaffInformation_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
    {
        Int32 Id = Convert.ToInt32(gvStaffInformation.DataKeys[e.NewEditIndex].Value);
        List<StuffInformation_BO> list = new List<StuffInformation_BO>();
        list = BILL.GetById(Id.ToString());
        foreach (StuffInformation_BO item in list)
        {
            CDD1.SelectedValue = item.CCOM_OID;
            txtStuffID.Text = item.StuffID;
            txtStuffName.Text = item.Name;
            txtMobileNumber.Text = item.MobileNumber;
            txtAlternativeMobileNo.Text = item.AlternativeMobileNo;
            txtEmailAddress.Text = item.EMailAddress;
            txtAlternativeEmailAddress.Text = item.AlternativeEMailAddress;
        }
        lblOID.Value = Id.ToString();
        ContainerStaffInformation.ActiveTabIndex = 1;        
    }

     

    private void Clear()
    {
        lblOID.Value = string.Empty;
        txtStuffID.Text = string.Empty;
        txtStuffName.Text = string.Empty;
        txtMobileNumber.Text = string.Empty;
        txtAlternativeMobileNo.Text = string.Empty;
        txtEmailAddress.Text = string.Empty;
        txtAlternativeEmailAddress.Text = string.Empty;
    }
    

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Clear();
        lblMessage.Text = string.Empty;
        ContainerStaffInformation.ActiveTabIndex = 0;        
        cmdSearch_Click(sender, e);
    }

    


    

   


}