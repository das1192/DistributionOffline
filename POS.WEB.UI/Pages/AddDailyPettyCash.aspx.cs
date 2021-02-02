using System;
using System.Web;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using TalukderPOS.BLL;
using TalukderPOS.BusinessObjects;


public partial class Pages_AddDailyPettyCash : System.Web.UI.Page
       {
        private string userID = string.Empty;
        private string Shop_id = string.Empty;
        private string userPassword = string.Empty;
        PettyCashBILL BILL = new PettyCashBILL();
        DailyCost BILL2 = new DailyCost();
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
                DailyPettyCash_BO entity = new DailyPettyCash_BO();
                entity.Shop_id = Shop_id.ToString();   
                BindList(entity);
            }
        }


        protected void gvPettyCash_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            e.Cancel = true;
            Clear();
            DailyPettyCash_BO entity = new DailyPettyCash_BO();
            String OID = gvPettyCash.DataKeys[e.NewEditIndex].Value.ToString();
            entity = BILL.GetById(OID);
            lblOID.Value = entity.OID;
            txtPettyCash.Text = entity.Amount;
            ContainerPettyCash.ActiveTabIndex = 1;
            lblMessage.Text = string.Empty;
        }


        protected void gvPettyCash_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            DailyPettyCash_BO entity = new DailyPettyCash_BO();
            entity.OID = gvPettyCash.DataKeys[e.RowIndex].Value.ToString();
          
            BILL.Delete(entity);
            BindList(entity);
        }



        protected void btnSave_Click(object sender, EventArgs e)
        {
            DailyPettyCash_BO entity = new DailyPettyCash_BO();
            entity.OID = lblOID.Value.ToString();
            
            entity.Shop_id = Shop_id.ToString();
            entity.Amount = txtPettyCash.Text.ToString();           
            entity.IUSER = userID.ToString();
            string newbal = "";
            string balval = entity.Shop_id;
            newbal = BILL2.getpettycash(balval);
            entity.CURBALANCE = newbal;
            BILL.Add(entity);
            Clear();
            lblMessage.Text = "SAVED SUCCESSFULLY";
            entity = null;
            BindList(entity);
        }

        private void BindList(object sender)
        {

            DailyPettyCash_BO entity = new DailyPettyCash_BO();
            entity.Shop_id = Shop_id.ToString(); 
            DataTable dt = BILL.BindList(entity);
            gvPettyCash.DataSource = dt;
            gvPettyCash.DataBind();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
            ContainerPettyCash.ActiveTabIndex = 0;
            lblMessage.Text = string.Empty;
        }


        private void Clear()
        {
            lblOID.Value = string.Empty;
            txtPettyCash.Text = string.Empty;
        }


       
    }

