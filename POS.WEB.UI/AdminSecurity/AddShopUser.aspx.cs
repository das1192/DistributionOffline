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

    public partial class AdminSecurity_AddShopUser : System.Web.UI.Page
    {
        private string userID = string.Empty;
        private string password = string.Empty;
        private string Shop_id = string.Empty;
        UserBLL BILL = new UserBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                userID = Session["UserID"].ToString();
                password = Session["Password"].ToString();
                string storeid = HttpContext.Current.Session["StoreID"].ToString();
                Shop_id = Session["StoreID"].ToString();
                if (Shop_id != "3")
                {
                    NEW.SelectedValue = Shop_id;
                    
                }
                cmdSearch_Click(sender, e);
            }
            catch
            {
                Response.Redirect("~/Default.aspx");
            }
            bool isAuthenticate = CommonBinder.CheckPageAuthentication(System.Web.HttpContext.Current.Request.Url.AbsolutePath, userID);

            if (!isAuthenticate)
            {
                Response.Redirect("~/UnAuthorizedUser.aspx");
            }
            if (!Page.IsPostBack)
            {
                lblMessage.Text = string.Empty;
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
            NEW.SelectedValue = entity.CCOM_OID;
            txtFullName.Text = entity.UserFullName;
            txtUserName.Text = entity.UserName;
            txtPassword.Text = entity.Password;
            txtConfirmPassword.Text = entity.ConfirmPassword;
            txtMobileNumber.Text = entity.MobileNumber;
            txtnationalid.Text = entity.nid;
            txtEmailAddress.Text = entity.EmailID;
            txtaddress.Text = entity.address;
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


        protected void btnSave_Click(object sender, EventArgs e)
        {
            User entity = new User();

            
            string[] valid = new string[2];
            StuffInformation_BILL BILL2 = new StuffInformation_BILL();
            Random rnd = new Random();
            int myRandomNo = rnd.Next(10000, 99999);
            
            entity.Id = lblOID.Value;
            entity.CCOM_OID = Shop_id ;
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
            entity.EmailID = txtEmailAddress.Text.Trim();
            entity.MobileNumber = txtMobileNumber.Text.Trim();
            entity.AlternativeMobileNo = txtAlternativeMobileNo.Text.Trim();
            
            string newuser = txtUserName.Text.Trim();


            StuffInformation_BO entity2 = new StuffInformation_BO();
            entity2.OID = lblOID.Value;
            entity2.StuffID = myRandomNo.ToString();
            entity2.Name = txtFullName.Text;
            entity2.CCOM_OID = Shop_id ;
            entity2.MobileNumber = txtMobileNumber.Text;
            entity2.AlternativeMobileNo = txtAlternativeMobileNo.Text.Trim();
            entity2.EMailAddress = txtEmailAddress.Text;
            entity2.AlternativeEMailAddress = string.Empty;
            entity2.ActiveStatus = "1";
            entity2.IUSER = userID;
            entity2.IDAT = DateTime.Today.Date.ToString();
            entity2.EUSER = userID;
            entity2.EDAT = DateTime.Today.Date.ToString();
            entity2.address = txtaddress.Text.Trim();
            entity2.nid = txtnationalid.Text.Trim();
            
            string avail = BILL.getusername(newuser);
            if (avail != String.Empty & avail != "0")
            {
                lblMessage.Text = "Username Already Exist. Please Try Other Username";
                lblMessage.ForeColor = Color.Red;
            }
            else
            {
                valid = BILL.Validation(entity);
                if (valid[0].ToString() == "True")
                {
                    lblMessage.Text = valid[1].ToString();
                    BILL.Add(entity);
                    int maxid = BILL.User_GetMaxID();
                    entity2.UserMaxID = maxid.ToString(); 
                    BILL2.Add(entity2);
                    Clear();

                    lblMessage.Text = ContextConstant.SAVED_SUCCESS;
                    lblMessage.ForeColor = Color.Green;
                }
                else
                {
                    lblMessage.Text = valid[1].ToString();
                }
            }
        }
        private void Clear()
        {
            lblOID.Value = string.Empty;
            txtFullName.Text = string.Empty;
            txtUserId.Text = string.Empty;
            txtUserName.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtConfirmPassword.Text = string.Empty;
            NEW.SelectedValue = string.Empty;
        }
       
        protected void cmdSearch_Click(object sender, EventArgs e)
        {
            User entity = new User();
            
            entity.CCOM_OID = Shop_id.ToString();
            DataTable dt = BILL.GetUserListShop(entity);
            gvUser.DataSource = dt;
            gvUser.DataBind();
        }


       
        protected void txtUserName_TextChanged1(object sender, EventArgs e)
        {
            txtUserId.Text = BILL.GenerateUserCode(txtUserName.Text);
            txtPassword.Focus();
            lblMessage.Text = string.Empty;
        }
}
