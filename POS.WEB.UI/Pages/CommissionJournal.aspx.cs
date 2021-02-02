using System;
using System.Web;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using TalukderPOS.BLL;
using TalukderPOS.DAL;
using TalukderPOS.BusinessObjects;
using System.Configuration;
using System.Data.SqlClient;

namespace TalukderPOS.Web.UI
{
    public partial class CommissionJournal : System.Web.UI.Page
    {
        private string userID = string.Empty;
        private string Shop_id = string.Empty;
        private string userPassword = string.Empty;

        Vendor_Outgoing BILL = new Vendor_Outgoing();
        CommonDAL DAL = new CommonDAL();
        T_PRODBLL BILL2 = new T_PRODBLL();
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

       
                //BindList();

                Vendor_Outgoing_BO entity = new Vendor_Outgoing_BO();
                entity.Shop_id = Shop_id.ToString();

                CurrentCashBalance();

                dptSupplierPaymentDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                // new post back mechanism copied from investment
                
            
                lblMessageForList.Text = string.Empty;
               
                DateTime date = DateTime.Now;
                var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                dptFromDate.Text = date.ToString("yyyy-MM-dd");
                dptToDate.Text = date.ToString("yyyy-MM-dd");






              // lblMsgSupplierPaymentList.Text = string.Empty;
            }
        }

        protected void cmdSearchForprice_Click(object sender, EventArgs e)
        {



            T_PROD entity = new T_PROD();
            entity.Branch = Session["StoreID"].ToString();
   
        }

        protected void gvVendorAmount_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            e.Cancel = true;
            Clear();
            Vendor_Outgoing_BO entity = new Vendor_Outgoing_BO();
           // String OID = gvVendorAmount.DataKeys[e.NewEditIndex].Value.ToString();
          //  entity = BILL.GetById(OID);
            lblOID.Value = entity.OID;
            txtAmount.Text = entity.AMOUNT;
            txtRemarks.Text = entity.Remarks;
            CCD8.SelectedValue = entity.Vendor_ID.ToString();
            ContainerProductVendor.ActiveTabIndex = 0;
            lblMessage.Text = string.Empty;
        }


        



        protected void btnSave_Click(object sender, EventArgs e)
        {
            
                Vendor_Outgoing_BO entity = new Vendor_Outgoing_BO();
                entity.OID = lblOID.Value.ToString();
                entity.AMOUNT = txtAmount.Text.ToString();
                
                //entity.PaymentFrom = ddlPamymentFrom.SelectedItem.Value.ToString();
                entity.Shop_id = Shop_id.ToString();
                entity.IUSER = userID.ToString();
                
                if (CheckBox1.Checked)
                {
                    entity.RefferenceNumber = string.Format(@"Ref_CommissionCash_{0}_{1}_{2}", entity.Shop_id, 1, DateTime.Now.ToString("yyMMddhhmmss"));
                    entity.AccountID = 1.ToString ();
                    entity.Narration = string.Format(@"CommissionByCash_{0}",txtRemarks.Text.ToString());
                    entity.Remarks = "Cash";

                }
                else
                {
                    entity.RefferenceNumber = string.Format(@"Ref_CommissionSupplierDebit_{0}_{1}_{2}", entity.Shop_id, ddlVendorList.SelectedValue.ToString(), DateTime.Now.ToString("yyMMddhhmmss"));
                    entity.AccountID = ddlVendorList.SelectedItem.Value.ToString();
                    entity.Narration = string.Format(@"CommissionBySupplierDebit_{0}", txtRemarks.Text.ToString());
                    entity.Remarks = "Supplier";
                }
                
                entity.IDAT = string.IsNullOrEmpty(dptSupplierPaymentDate.Text) ? DateTime.Now.ToString("yyyy-MM-dd") : dptSupplierPaymentDate.Text;


                BILL.AddCommission (entity);
                Clear();
                lblMessage.Text = "SAVED SUCCESSFULLY";
                entity = null;
                //BindList(entity);

                //
                //CurrentCashBalance();
            
        }

        //private void BindList()
        //{
        //    Vendor_Outgoing_BO entity = new Vendor_Outgoing_BO();
        //    entity.Shop_id = Shop_id.ToString();
        //    DataTable dt = BILL.BindList(entity);
            
           
        //}

    

        protected void gvT_Delete_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            Vendor_Outgoing_BO entity = new Vendor_Outgoing_BO();
         
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
            txtAmount.Text = string.Empty;
            txtRemarks.Text = string.Empty;
            dptSupplierPaymentDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            //ddlPamymentFrom.Items.Clear();
            ddlVendorList.SelectedIndex = -1;
            lblPayableAmount.Text = String.Empty;
        }
        protected void cmdSearchDelete_Click(object sender, EventArgs e)
        {
            Vendor_Outgoing_BO entity = new Vendor_Outgoing_BO();
            entity.Shop_id = Shop_id.ToString();

            //entity.FromDate = txtDateFromdelete.Text;
            //entity.ToDate = txtDateTodelete.Text;
            DataTable dt = BILL.BindListdelete(entity);
            Session["GridDatadelete"] = dt;
            

        }


        protected void cmdSearchSup_Click(object sender, EventArgs e)
        {
            Vendor_Outgoing_BO entity = new Vendor_Outgoing_BO();
            entity.Shop_id = Shop_id.ToString();
        
            DataTable dt = BILL.BindList(entity);

        }

        private void CurrentCashBalance()
        {
            string sql = string.Format(@"
select t.Branch,t.BankName,t.AccountNo,t.Debit,t.Credit,t.Balance 
from (
---------------------------------------------------------------------------------------
select b.Branch, BankName='Cash',AccountNo='',b.Debit,b.Credit,b.Balance 
from dbo.vw_Shopwise_Cash_Balance b where b.Branch={0}
union
select b.Branch, b.BankName,b.AccountNo,b.DebitSum Debit,b.CreditSum Credit,b.Balance 
from dbo.vw_Shopwise_Bank_Balance b where b.Branch={0}
-----------------------------------------------------------------------------------------
)t
", Shop_id);
            DataTable dt = DAL.LoadDataByQuery(sql);
            if (dt.Rows.Count > 0)
            {
                //gvCashBalance.DataSource = null;
                //gvCashBalance.DataSource = dt;
                //gvCashBalance.DataBind();
            }
        }

        //
        private void LoadDropDownPamymentFrom()
        {
            string sql = string.Format(@"
select case when t.BankName='Cash' then '1' else t.AccountID end as AccountID,(t.BankName+'_'+t.AccountNo+':'+CONVERT(nvarchar(15),t.Balance)) text
,t.BankName,t.AccountNo,t.Debit,t.Credit,t.Balance,t.Branch 
from (

select b.Branch, BankName='Cash',AccountNo='',b.Debit,b.Credit,b.Balance,AccountID=''
from dbo.vw_Shopwise_Cash_Balance b where b.Branch={0}
union
select b.Branch, b.BankName,b.AccountNo,b.DebitSum Debit,b.CreditSum Credit,b.Balance ,b.AccountID
from dbo.vw_Shopwise_Bank_Balance b where b.Branch={0}

)t
", Shop_id);
            DataTable dt = DAL.LoadDataByQuery(sql);
            if (dt.Rows.Count > 0)
            {
                //ddlPamymentFrom.DataSource = dt;
                //ddlPamymentFrom.DataTextField = "text";
                //ddlPamymentFrom.DataValueField = "AccountID";
                //ddlPamymentFrom.DataBind();

                //ddlPamymentFrom.Items.Insert(0, new ListItem("Cash", "1"));

            }
        }

        protected void ddlVendorList_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblPayableAmount.Text = "0.00";

            if (!string.IsNullOrEmpty(ddlVendorList.SelectedValue))
            {
                string sql = string.Format(@"
select sum(j.Debit),sum(j.Credit),Balance=ISNULL((sum(j.Credit-j.Debit)),0) 
from Acc_Journal j
where j.Particular='A/P' and j.Remarks='Supplier' and j.Branch='{0}' and j.AccountID={1}
", Shop_id, ddlVendorList.SelectedValue);
                DataTable dt = DAL.LoadDataByQuery(sql);

                decimal Balance = 0;
                if (decimal.TryParse(dt.Rows[0]["Balance"].ToString(), out Balance)) { }
                if (Balance >= 0)
                {
                    lblPayablelbl.Text = "Accounts Payable";
                    lblPayableAmount.Text = Balance.ToString("0.00");
                    lblPayableAmount.ForeColor = System.Drawing.Color.Red;
                    lblPayablelbl.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    lblPayablelbl.Text = "Accounts Receivable";
                    lblPayableAmount.Text = Math.Abs(Balance).ToString("0.00");
                    lblPayableAmount.ForeColor = System.Drawing.Color.Green;
                    lblPayablelbl.ForeColor = System.Drawing.Color.Green;
                }
            }


            if (!string.IsNullOrEmpty(ddlVendorList.SelectedValue.ToString()))
            {
                LoadDropDownPamymentFrom();
            }
            else
            {
                //ddlPamymentFrom.Items.Clear();
            }
        }
        protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox1.Checked)
            {
                CCD8.Enabled = false;
                lblPayableAmount.Text = "";
            }
            else
            {
                CCD8.Enabled = true;

            }
        }

        protected void btnSearchInvest_Click(object sender, EventArgs e)
        {
            BindList();
            lblMessageForList.Text = "";
        }




        private void BindList()
        {
            string param = string.Format(@" and j.Branch={0}", Shop_id );

            DateTime dFromDate = DateTime.Now;
            DateTime dToDate = DateTime.Now;

            if (DateTime.TryParse(dptFromDate.Text, out dFromDate) && DateTime.TryParse(dptToDate.Text, out dToDate))
            {
                param += string.Format(@" and CONVERT(date, j.IDATTIME) between '{0}' and '{1}'", dFromDate.ToString("yyyy-MM-dd"), dToDate.ToString("yyyy-MM-dd"));
            }
            else
            {
                param += string.Format(@" and j.IDAT between '{0}' and '{1}'", DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd"));

            }

            //int shopOID = Convert.ToInt32(ShopID);
            string sql = string.Format(@"
select j.Journal_OID,j.AccountID,j.Particular,j.RefferenceNumber,j.Debit,j.Credit,j.Narration,CONVERT(nvarchar(12),j.IDAT,106)IDAT
,j.Remarks
from Acc_Journal j 
where j.Particular='Commission' {0}

", param);

            DataTable dt = DAL.LoadDataByQuery(sql);

            gvCommissionList .DataSource = null;
            if (dt.Rows.Count > 0) { gvCommissionList.DataSource = dt; }
            gvCommissionList.DataBind();
        }
        protected void gvCommissionList_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            e.Cancel = true;
            Clear();
           
            String RefferenceNumber = gvCommissionList.DataKeys[e.RowIndex].Value.ToString();

            string sql = "DELETE FROM [dbo].[Acc_Journal] WHERE RefferenceNumber="+"'"+RefferenceNumber+"'";

            DAL.SaveDataCRUD(sql);
            lblMessageForList.Text = "DELETE Successfull !!";
            BindList();
            Clear();
        }
}
}
