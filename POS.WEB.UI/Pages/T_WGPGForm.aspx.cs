using System;
using System.Web;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using TalukderPOS.BLL;
using TalukderPOS.BusinessObjects;

namespace TalukderPOS.Web.UI
{
	public partial class T_WGPGForm : System .Web.UI .Page 
	{
        private string userID = string.Empty;
        private string Shop_id = string.Empty;
        private string userPassword = string.Empty;
        T_WGPGBLL BILL = new T_WGPGBLL();

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
                T_WGPG entity = new T_WGPG();
                entity.Shop_id = Shop_id.ToString();
                BindList(entity);
            }
        }

       
        protected void gvProductCategory_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            e.Cancel = true;
            Clear();
            T_WGPG entity = new T_WGPG();
            String OID = gvProductCategory.DataKeys[e.NewEditIndex].Value.ToString();
            entity = BILL.GetById(OID);
            lblOID.Value = entity.OID;
            txtCategoryName.Text = entity.WGPG_NAME;
            ContainerProductCategory.ActiveTabIndex = 1;
            lblMessage.Text = string.Empty;
        }


        protected void gvProductCategory_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            T_WGPG entity = new T_WGPG();
            entity.OID = gvProductCategory.DataKeys[e.RowIndex].Value.ToString();
            entity.Shop_id = Shop_id.ToString(); 
            entity.EUSER = userID;
            BILL.Delete(entity);
            BindList(entity);
        }

        

        protected void btnSave_Click(object sender, EventArgs e)
        {
            T_WGPG entity = new T_WGPG();
            entity.OID = lblOID.Value.ToString();
            entity.WGPG_NAME = txtCategoryName.Text.ToString();
            entity.WGPG_ACTV = "1";
            entity.Shop_id = Shop_id.ToString(); 
            entity.IUSER = userID.ToString();
            entity.EUSER = userID.ToString();
            string getcatname = BILL.getcatname(entity.WGPG_NAME, entity.Shop_id);
            if (getcatname != String.Empty & getcatname != "0")
            {
                Clear();
                lblMessage.Text = "Given Category Name Already Exist In the shop";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                
            }
            else
            {
                BILL.Add(entity);
                Clear();
                lblMessage.Text = "SAVED SUCCESSFULLY";
                lblMessage.ForeColor = System.Drawing.Color.Green;
            }
            
            entity = null;
            BindList(entity);
        }

        private void BindList(object sender)
        {
            T_WGPG entity = new T_WGPG();
            entity.Shop_id = Shop_id.ToString();
            DataTable dt = BILL.BindList(entity);
            
            gvProductCategory.DataSource = dt;
            gvProductCategory.DataBind();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
            ContainerProductCategory.ActiveTabIndex = 0;
            lblMessage.Text = string.Empty;
        }


        private void Clear()
        {
            lblOID.Value = string.Empty;
            txtCategoryName.Text = string.Empty;
        }
    }
}
