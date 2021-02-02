using System;
using System.Web;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using TalukderPOS.BLL;
using TalukderPOS.DAL;
using TalukderPOS.BusinessObjects;


public partial class Pages_AddCostingHead : System.Web.UI.Page
       {
        private string userID = string.Empty;
        private string Shop_id = string.Empty;
        private string userPassword = string.Empty;
        CostingHeadBILL BILL = new CostingHeadBILL();
        CommonDAL DAL = new CommonDAL();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                userID = Session["UserID"].ToString();
                userPassword = Session["Password"].ToString();
                Shop_id = Session["StoreID"].ToString();
                if (userID == "")
                {
                    Response.Redirect("~/frmLogin.aspx");
                }
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
                CostingHead_BO entity = new CostingHead_BO();
                entity.Shop_id = Shop_id.ToString();   
                BindList(entity);
            }
        }


        protected void gvCostingHead_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            e.Cancel = true;
            Clear();
            CostingHead_BO entity = new CostingHead_BO();
            String OID = gvCostingHead.DataKeys[e.NewEditIndex].Value.ToString();

            string sql = string.Format(@"select OID,ExpenseType,CostingHead 
from CostingHead where Shop_id={0} and OID={1}", Shop_id,OID);

            DataTable dt = DAL.LoadDataByQuery(sql);

            lblOID.Value = dt.Rows[0]["OID"].ToString();
            txtCostingHead.Text = dt.Rows[0]["CostingHead"].ToString();
            ddlExType.SelectedValue = dt.Rows[0]["ExpenseType"].ToString();
            
            //entity = BILL.GetById(OID);
            ContainerCostingHead.ActiveTabIndex = 1;
            lblMessage.Text = string.Empty;
        }


        protected void gvCostingHead_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            CostingHead_BO entity = new CostingHead_BO();
            entity.OID = gvCostingHead.DataKeys[e.RowIndex].Value.ToString();
          
            BILL.Delete(entity);
            BindList(entity);
        }



        protected void btnSave_Click(object sender, EventArgs e)
        {
            CostingHead_BO entity = new CostingHead_BO();
            entity.OID = lblOID.Value.ToString();
            entity.Shop_id = Shop_id.ToString();
            entity.CostingHead = txtCostingHead.Text.ToString();
            entity.ExpenseType = ddlExType.SelectedValue;
            entity.IUSER = userID.ToString();            
            BILL.Add(entity);
            Clear();
            lblMessage.Text = "SAVED SUCCESSFULLY";
            entity = null;
            BindList(entity);
            ContainerCostingHead.ActiveTabIndex = 0;
        }

        private void BindList(object sender)
        {

            string sql = string.Format(@"select OID,ExpenseType,CostingHead 
from CostingHead where CostingHead not in ('Product Missing','Discount On Sales','Expense For Gift') and Shop_id={0}", Shop_id);
            
            DataTable dt = DAL.LoadDataByQuery(sql);
            //CostingHead_BO entity = new CostingHead_BO();
            //entity.Shop_id = Shop_id.ToString(); 
            //DataTable dt = BILL.BindList(entity);
            gvCostingHead.DataSource = null;
            if(dt.Rows.Count>0){ gvCostingHead.DataSource = dt;}
            gvCostingHead.DataBind();


            ////to disable link btn
            //for (int i = 0; i < gvCostingHead.Rows.Count; i++)
            //{
            //    CommandField lnkDelete = (CommandField)gvDailyCost.Rows[i].Cells[7].FindControl("lnkDelete");
            //    if (dt.Rows[i]["CostingHead"].ToString() == "Product Missing" ||
            //        dt.Rows[i]["CostingHead"].ToString() == "Discount On Sales" ||
            //        dt.Rows[i]["CostingHead"].ToString() == "Expense For Gift")
            //    {
            //        lnkDelete.Visible = false;
            //    }
            //    else
            //    {
            //        lnkDelete.Visible = true;
            //    }
            //}

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
            ContainerCostingHead.ActiveTabIndex = 0;
            lblMessage.Text = string.Empty;
        }


        private void Clear()
        {
            lblOID.Value = string.Empty;
            txtCostingHead.Text = string.Empty;
            ddlExType.SelectedValue = "Indirect";
        }


       
    }

