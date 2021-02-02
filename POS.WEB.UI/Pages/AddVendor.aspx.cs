using System;
using System.Web;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using TalukderPOS.BLL;
using TalukderPOS.BusinessObjects;


public partial class Pages_AddVendor : System.Web.UI.Page
       {
        private string userID = string.Empty;
        private string Shop_id = string.Empty;
        private string userPassword = string.Empty;
        ProductVendorBILL BILL = new ProductVendorBILL();

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
                ProductVendor_BO entity = new ProductVendor_BO();
                entity.Shop_id = Shop_id.ToString();   
                BindList(entity);
            }
        }


        protected void gvProductVendor_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            e.Cancel = true;
            Clear();
            ProductVendor_BO entity = new ProductVendor_BO();
            String OID = gvProductVendor.DataKeys[e.NewEditIndex].Value.ToString();
            entity = BILL.GetById(OID);
            lblOID.Value = entity.OID;
            txtVendorName.Text = entity.Vendor_Name;
            txtVendorName.Enabled = false;
            txtVendorAddress.Text = entity.Vendor_Address;
            txtVendortr.Text = entity.Vendor_tr;
            txtVendormob.Text = entity.Vendor_mobile;
            ContainerProductVendor.ActiveTabIndex = 1;
            lblMessage.Text = string.Empty;

            bool s=false;
            if (entity.Vendor_Active == "1") {
                s = true;
            }
            chkStatus.Checked = s;
        }


        protected void gvProductVendor_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            ProductVendor_BO entity = new ProductVendor_BO();
            entity.OID = gvProductVendor.DataKeys[e.RowIndex].Value.ToString();
            entity.EUSER = userID;
            BILL.Delete(entity);
            BindList(entity);
        }



        protected void btnSave_Click(object sender, EventArgs e)
        {
            ProductVendor_BO entity = new ProductVendor_BO();
           // entity.ActiveStatus = (chkStatus.Checked ? "1" : "0");
            entity.OID = lblOID.Value.ToString();
            entity.Shop_id = Shop_id.ToString();           
            entity.Vendor_Name = txtVendorName.Text.ToString();
            entity.Vendor_Address = txtVendorAddress.Text.ToString();
            entity.Vendor_mobile = txtVendormob.Text.ToString();
            entity.Vendor_tr = txtVendortr.Text.ToString();
            entity.Vendor_Active = (chkStatus.Checked ? "1" : "0");
            entity.IUSER = userID.ToString();
            entity.EUSER = userID.ToString();
            BILL.Add(entity);
            Clear();
            lblMessage.Text = "SAVED SUCCESSFULLY";
            entity = null;
            BindList(entity);
        }

        private void BindList(object sender)
        {
             
            ProductVendor_BO entity = new ProductVendor_BO();
            entity.Shop_id = Shop_id.ToString(); 
            DataTable dt = BILL.BindList(entity);
            gvProductVendor.DataSource = dt;
            gvProductVendor.DataBind();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
            ContainerProductVendor.ActiveTabIndex = 0;
            lblMessage.Text = string.Empty;
        }


        private void Clear()
        {
            lblOID.Value = string.Empty;
            txtVendorName.Text = string.Empty;
            txtVendorAddress.Text = string.Empty;
            txtVendortr.Text = string.Empty;
            txtVendormob.Text = string.Empty;
        }


       
    }

