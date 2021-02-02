using System;
using System.Web;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using TalukderPOS.BLL;
using TalukderPOS.DAL;
using TalukderPOS.BusinessObjects;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TalukderPOS.Web.UI
{
    public partial class DailyCostForm : System.Web.UI.Page
    {
        private string userID = string.Empty;
        private string Shop_id = string.Empty;
        private string userPassword = string.Empty;

        CommonDAL DAL = new CommonDAL();
        DailyCost BILL = new DailyCost();


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
                DailyCost_BO entity = new DailyCost_BO();
                entity.Shop_id = Shop_id.ToString();
                LoadGridDailyCostList();
                CurrentCashBalance(entity);

                dptFromDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                LoadDropDownPamymentFrom();
            }
        }

        private void LoadDropDownPamymentFrom()
        {
            string sql = string.Format(@"
select t.AccountID,(t.BankName+'_'+t.AccountNo+':'+CONVERT(nvarchar(15),t.Balance)) text
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
                ddlPamymentFrom.DataSource = dt;
                ddlPamymentFrom.DataTextField = "text";
                ddlPamymentFrom.DataValueField = "AccountID";
                ddlPamymentFrom.DataBind();

                //ddlPamymentFrom.Items.Insert(0, new ListItem("Cash", "1"));

            }
        }

        protected void gvDailyCost_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            e.Cancel = true;
            Clear();
            DailyCost_BO entity = new DailyCost_BO();
            String OID = gvDailyCost.DataKeys[e.NewEditIndex].Value.ToString();
            entity = BILL.GetById(OID);
            lblOID.Value = entity.OID;
            txtAmount.Text = entity.AMOUNT;
            CCD8.SelectedValue = entity.CostingHeadID.ToString();
            ContainerDailyCost.ActiveTabIndex = 1;
            lblMessage.Text = string.Empty;
            CurrentCashBalance(entity);
        }

        protected void gvT_PROD_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            gvDailyCost.PageIndex = e.NewPageIndex;
            DailyCost_BO entity = new DailyCost_BO();
            entity.Shop_id = Shop_id.ToString();
            LoadGridDailyCostList();
        }
        protected void gvT_CashFlow_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            gvT_CashFlow.PageIndex = e.NewPageIndex;
            BindList2();
        }
        protected void gvDailyCost_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            DailyCost_BO entity = new DailyCost_BO();
            string newbal = "";
            entity.OID = gvDailyCost.DataKeys[e.RowIndex].Value.ToString();




            entity.Shop_id = Shop_id.ToString();
            entity.IUSER = userID;
            string balval = entity.Shop_id;
            newbal = BILL.getpettycash(balval);
            entity.CURBALANCE = newbal;

            string sql = string.Format(@"
select Journal_OID as rowNumber from Acc_Journal where RefferenceNumber=(select ReferenceNo from DailyCost where OID={0})
union
select OID as rowNumber from DailyCost where OID={0}
",entity.OID);
            DataTable dt = DAL.LoadDataByQuery(sql);

            if (dt.Rows.Count == 3)
            {
                string sqlDelete = string.Format(@"
delete Acc_Journal where RefferenceNumber=(select ReferenceNo from DailyCost where OID={0})
delete DailyCost where OID={0}
", entity.OID);
                DAL.SaveDataCRUD(sqlDelete);
                //BILL.Delete(entity);
            }

            LoadDropDownPamymentFrom();
            LoadGridDailyCostList();
            CurrentCashBalance(entity);
        }



        protected void btnSave_Click(object sender, EventArgs e)
        {
            lblMessage.Text = string.Empty;

            //check
            string isValid = "yes";

            if(string.IsNullOrEmpty(ddlCostingHead.SelectedValue))
            {
                ddlCostingHead.Focus();
                lblMessage.Text = "Please Select Costing Head!";
                return;
            }

            string amountLimitDDL = string.Empty;
            if (ddlPamymentFrom.SelectedItem.Text.Contains(":")) 
            { 
                string[] code = ddlPamymentFrom.SelectedItem.Text.Split(':'); amountLimitDDL = code[1]; 
            }
            decimal amountLimit = 0;
            decimal amount = 0;
            if (decimal.TryParse(amountLimitDDL, out amountLimit)) { }
            if (decimal.TryParse(txtAmount.Text, out amount)) { }

            if (amount > amountLimit) 
            { 
                isValid = "no";

                txtAmount.Text = string.Empty;
                txtAmount.Focus();
                lblMessage.Text = "Please Check the Amount or Limit!";
                return;
            }

            //save
            if (isValid == "yes")
            {
                DailyCost_BO entity = new DailyCost_BO();
                string newbal = "";
                int com1 = 0;
                int com2 = 0;
                entity.OID = lblOID.Value.ToString();
                entity.AMOUNT = txtAmount.Text.ToString();
                entity.Remarks = txtremarks.Text.ToString();
                entity.CostingHeadID = ddlCostingHead.SelectedItem.Value.ToString();
                entity.Shop_id = Shop_id.ToString();
                entity.IUSER = userID.ToString();
                string balval = entity.Shop_id;
                newbal = BILL.getpettycash(balval);
                entity.CURBALANCE = newbal;
                com1 = Convert.ToInt32(newbal);
                com2 = Convert.ToInt32(entity.AMOUNT);
                entity.RefNo = string.Format(@"Ref_Expense_{0}_{1}_{2}", entity.Shop_id, entity.CostingHeadID,DateTime.Now.ToString("yyMMddhhmmss"));
                entity.IDAT = DateTime.Now.ToString("yyyy-MM-dd"); // string.IsNullOrEmpty(dptFromDate.Text) ? DateTime.Now.ToString("yyyy-MM-dd") : dptFromDate.Text;

                //string RefNo = string.Format(@"Ref_Expense_{0}_{1}", entity.Shop_id, entity.CostingHeadID);
                BILL.Add(entity);

                CreateJournal(entity);

                Clear();
                lblMessage.Text = "SAVED SUCCESSFULLY";
                entity = null;

                LoadDropDownPamymentFrom();
                LoadGridDailyCostList();
                CurrentCashBalance(entity);
            }
        }

        private void CreateJournal(DailyCost_BO entity)
        {
            //journal
            //string StoreID = Session["StoreID"].ToString();
            string CashOrBank = string.Empty;
            if (ddlPamymentFrom.SelectedItem.Text.Contains("_")) 
            { 
                string[] code = ddlPamymentFrom.SelectedItem.Text.Split('_'); 
                CashOrBank = code[0]; 
            }


            //string RefNo = string.Format(@"Ref_Expense_{0}_{1}", entity.Shop_id, entity.CostingHeadID);

            string InsertDR = string.Format(@"
INSERT INTO Acc_Journal(AccountID,Branch,Particular,Remarks,Debit,Credit,RefferenceNumber,IDAT,IDATTIME,Narration) 
VALUES('{0}','{1}','{2}','{3}',{4}
,{5},'{6}','{7}','{8}','{9}')

", ddlCostingHead.SelectedValue, entity.Shop_id, "Expense", "Expense", entity.AMOUNT
 , 0, entity.RefNo, entity.IDAT, entity.IDAT, string.Format(@"Expense for {0}", ddlCostingHead.SelectedItem));
//            , 0,entity.RefNo, DateTime.Now.ToString(), DateTime.Now.ToString(), string.Format(@"Expense for {0}", ddlCostingHead.SelectedItem));


            string InsertCR = string.Format(@"
INSERT INTO Acc_Journal(AccountID,Branch,Particular,Remarks,Debit,Credit,RefferenceNumber,IDAT,IDATTIME,Narration) 
VALUES('{0}',{1},'{2}','{3}',{4}
,{5},'{6}','{7}','{8}','{9}')

", CashOrBank == "Cash" ? "1" : ddlPamymentFrom.SelectedValue.ToString()
 , entity.Shop_id
 , CashOrBank == "Cash" ? "Cash" : "Bank"
 , CashOrBank == "Cash" ? "Cash" : "Bank"
 , 0
 , entity.AMOUNT, entity.RefNo, entity.IDAT, entity.IDAT, string.Format(@"Expense for {0}", ddlCostingHead.SelectedItem));


            try
            {
                SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                SqlCommand cmdDR = new SqlCommand(InsertDR, dbConnect);
                SqlCommand cmdCR = new SqlCommand(InsertCR, dbConnect);
                cmdDR.ExecuteNonQuery();
                cmdCR.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //dbConnect.Close();
            }
        }

        private void LoadGridDailyCostList()
        {
            string param=string.Empty;
            //shopid
            param += string.Format(@" and dc.Shop_id='{0}'",Shop_id);
            
            //date

            DateTime dFromDate = DateTime.Now;
            DateTime dToDate = DateTime.Now;
            
            if (DateTime.TryParse(dptFromDateDCL.Text, out dFromDate) && DateTime.TryParse(dptToDateDCL.Text, out dToDate))
            {
                param += string.Format(@" and convert(date, dc.IDAT) between '{0}' and '{1}'", dFromDate.ToString("yyyy-MM-dd"), dToDate.ToString("yyyy-MM-dd"));
            }
            else
            {
                param += string.Format(@" and convert(date, dc.IDAT) between '{0}' and '{1}'", dFromDate.ToString("yyyy-MM-dd"), dToDate.ToString("yyyy-MM-dd"));
            }

           string sql =string.Format(@"
Select dc.OID,dc.Remarks,ch.CostingHead
,dc.AMOUNT,convert(CHAR(10), dc.IDAT, 120) as NEWDATE ,convert(date, dc.IDAT) as NEWDATE 
from DailyCost dc
inner join CostingHead ch on dc.CostingHeadID=ch.OID 
where dc.AMOUNT>0  {0} 
and ch.CostingHead not in ('Expense For Gift','Discount On Sales')
order by dc.OID desc
", param);
            
            //DailyCost_BO entity = new DailyCost_BO();
            //entity.Shop_id = Shop_id.ToString();
            DataTable dt = DAL.LoadDataByQuery(sql);

            gvDailyCost.DataSource = null;
            if (dt.Rows.Count > 0) { gvDailyCost.DataSource = dt; }
            gvDailyCost.DataBind();

            //to disable link btn
            for (int i = 0; i < gvDailyCost.Rows.Count; i++)
            {
                LinkButton lnkDelete = (LinkButton)gvDailyCost.Rows[i].Cells[7].FindControl("lnkDelete");
                if (dt.Rows[i]["CostingHead"].ToString() == "Product Missing" ||
                    dt.Rows[i]["CostingHead"].ToString() == "Discount On Sales" ||
                    dt.Rows[i]["CostingHead"].ToString() == "Expense For Gift")
                {
                    lnkDelete.Visible = false;
                }
                else
                {
                    lnkDelete.Visible = true;
                }
            }

        }
        private void CurrentCashBalance(object sender)
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
                gvCashBalance.DataSource = null;
                gvCashBalance.DataSource = dt;
                gvCashBalance.DataBind();
            }
            else
            {
                gvCashBalance.DataSource = null;
                gvCashBalance.DataBind();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
            ContainerDailyCost.ActiveTabIndex = 0;
            lblMessage.Text = string.Empty;
        }


        private void Clear()
        {
            dptFromDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            lblOID.Value = string.Empty;
            txtAmount.Text = string.Empty;
            txtremarks.Text = string.Empty;
            dptFromDate.Text = string.Empty;                                                                                                                                                             
            ddlCostingHead.SelectedIndex = -1;
            ddlPamymentFrom.SelectedIndex = -1;
        }
        protected void cmdSearchCashflow_Click(object sender, EventArgs e)
        {

            DailyCost_BO entity = new DailyCost_BO();
            entity.Shop_id = Shop_id.ToString();

            entity.FromDate = txtDateFrom.Text;
            entity.ToDate = txtDateTo.Text;


            if (rbtDetails.Checked == true)
            {
                gvT_CashFlow.Visible = true;

                DataTable dt = BILL.CashFlowSearch(entity);
                Session["GridData"] = dt;
                BindList2();
            }

        }
        private void BindList2()
        {
            gvT_CashFlow.DataSource = Session["GridData"];
            gvT_CashFlow.DataBind();
        }

        protected void btnSearchDailyCost_Click(object sender, EventArgs e)
        {
            LoadGridDailyCostList();
        }
}
}
