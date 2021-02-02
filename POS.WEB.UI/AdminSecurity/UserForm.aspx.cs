using System;
using System.Web;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using TalukderPOS.BLL;
using TalukderPOS.BusinessObjects;
using System.Drawing;
using System.Web.UI.WebControls;
using TalukderPOS.DAL;

namespace SALESANDINVENTORY.Web.UI
{
    public partial class UserForm : System.Web.UI.Page
    {
        private string userID = string.Empty;
        private string password = string.Empty;
        UserBLL BILL = new UserBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                userID = Session["UserID"].ToString();
                password = Session["Password"].ToString();

            }
            catch
            {
                Response.Redirect("~/Default.aspx");
            }
            if (!Page.IsPostBack)
            {                
            }
        }

      
        protected void btnSave_Click(object sender, EventArgs e)
        {
            User entity = new User();
            string[] valid = new string[2];

            entity.Id = lblOID.Value;
            entity.CCOM_OID = ddlBranch.SelectedItem.Value.ToString();
            entity.UserId = txtUserId.Text.ToString();
            entity.UserName = txtUserName.Text.Trim();
            entity.Password = txtPassword.Text.Trim();
            entity.ConfirmPassword = txtConfirmPassword.Text.Trim();
            entity.UserFullName = txtFullName.Text;           
            entity.ActiveStatus = "1";
            entity.IUSER = userID;
            entity.IDAT = DateTime.Today.Date.ToString();
            entity.EUSER = userID;
            entity.EDAT = DateTime.Today.Date.ToString();

            valid = BILL.Validation(entity);
            if (valid[0].ToString() == "True")
            {
                lblMessage.Text = valid[1].ToString();
                BILL.Add(entity);
                Clear();
                cmdSearch_Click(sender,e);
                lblMessage.Text = ContextConstant.SAVED_SUCCESS;
                lblMessage.ForeColor = Color.Green;
            }
            else
            {
                lblMessage.Text = valid[1].ToString();
            }
        }


        protected void gvUser_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            e.Cancel = true;
            Clear();
            User entity = new User();
            String Id = gvUser.DataKeys[e.NewEditIndex].Value.ToString();
            entity = BILL.GetById(Id);

            lblOID.Value = entity.Id;
            txtUserId.Text = entity.UserId;            
            CDD1.SelectedValue = entity.CCOM_OID;
            txtFullName.Text = entity.UserFullName;
            txtUserName.Text = entity.UserName;
            txtPassword.Text = entity.Password;
            txtConfirmPassword.Text = entity.ConfirmPassword;
            ContainerSystemUser.ActiveTabIndex = 1;
            lblMessage.Text = string.Empty;
        }

        protected void gvUser_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            User entity = new User();
            Int32 Id = Convert.ToInt32(gvUser.DataKeys[e.RowIndex].Value);
            entity.Id = Id.ToString();
            entity.ActiveStatus = "0";
            entity.EUSER = userID;
            entity.EDAT = DateTime.Today.Date.ToString();

            BILL.User_Delete(entity);
            cmdSearch_Click(sender, e);
        }

              

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        protected void txtUserName_TextChanged(object sender, EventArgs e)
        {            
            txtUserId.Text = BILL.GenerateUserCode(txtUserName.Text);
            txtPassword.Focus();
            lblMessage.Text = string.Empty;          
        }

        private void Clear()
        {
            lblOID.Value = string.Empty;
            txtFullName.Text = string.Empty;
            txtUserId.Text = string.Empty;
            txtUserName.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtConfirmPassword.Text = string.Empty;
            CDD1.SelectedValue = string.Empty;
        }




        protected void cmdSearch_Click(object sender, EventArgs e)
        {
            User entity = new User();
            entity.CCOM_OID = ddlSearchBranch.SelectedItem.Value.ToString();

            DataTable dt = BILL.GetUserList(entity);
            gvUser.DataSource = dt;
            gvUser.DataBind();
        }

}
}
