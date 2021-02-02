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
using System.Web.UI.WebControls;

public partial class Pages_Investment : System.Web.UI.Page
{
    private string ShopID = string.Empty;
    private string userID = string.Empty;
    private string userPassword = string.Empty;
    BankInfoBILL BILL = new BankInfoBILL();
    CommonDAL DAL = new CommonDAL();
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
            LoadDropDown();
            LoadDropDownPamymentFrom();
            LoadDropDownPamymentFrom2();

            CurrentCashBalance(ShopID);
            lblMessageForList.Text = string.Empty;
            dptDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DateTime date = DateTime.Now;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            dptFromDate.Text = date.ToString("yyyy-MM-dd");
            dptToDate.Text = date.ToString("yyyy-MM-dd");
        }
    }
    protected void gvBank_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
    {
        e.Cancel = true;
        Clear();


        BankInfo_BO entity = new BankInfo_BO();
        String OID = gvInvestList.DataKeys[e.NewEditIndex].Value.ToString();

        string sql = string.Format(@"
select * from Acc_Journal j where j.Journal_OID='{0}'
", OID);
        DataTable dt = DAL.LoadDataByQuery(sql);
        string RefNo = dt.Rows[0]["RefferenceNumber"].ToString();
        string Particular = dt.Rows[0]["Particular"].ToString();
        string AccountID = dt.Rows[0]["AccountID"].ToString();
        string Remarks = dt.Rows[0]["Remarks"].ToString();
        string Amount = "";
        if (dt.Rows[0]["Particular"].ToString() == "Invest") { Amount = dt.Rows[0]["Debit"].ToString(); }
        else if (dt.Rows[0]["Particular"].ToString() == "Withdraw") { Amount = dt.Rows[0]["Credit"].ToString(); }

        if (dt.Rows.Count > 0)
        {
            ddlOwnerAction.SelectedValue = Particular;
            LoadDropDownBank();
            ddlBank.SelectedValue = AccountID;
            txtAmount.Text = Amount; // dt.Rows[0]["RefferenceNumber"].ToString();
            txtRemarks.Text = Remarks;
        }


        //entity = BILL.GetById(OID);

        //lblOID.Value = entity.OID;
        ////txtBankName.Text = entity.BankName;
        //txtAmount.Text = entity.AccountNo;
        //ddlBank.SelectedValue = entity.ActiveStatus;

        ContainerBankInfo.ActiveTabIndex = 1;
        lblMessage.Text = string.Empty;
    }

    private void LoadDropDown()
    {
        LoadDropDownBank();
    }
    private void LoadDropDownBank()
    {
        ddlBank.Items.Clear();
        string sql = string.Format(@"
select OID,(BankName+':'+AccountNo)BankNameNo,AccountNo,BankName,ActiveStatus 
from BankInfo b 
where b.ActiveStatus=1 and b.ShopID={0}
order by BankName
", Convert.ToInt32(ShopID));
        DataTable dt = DAL.LoadDataByQuery(sql);
        if (dt.Rows.Count > 0)
        {
            ddlBank.DataSource = dt;
            ddlBank.DataTextField = "BankNameNo";
            ddlBank.DataValueField = "OID";
            ddlBank.DataBind();

        }
        if (ddlOwnerAction.SelectedValue.ToString() == "Invest")
        {
            ddlBank.Items.Insert(0, new ListItem("Cash", "1"));
        }
    }

    protected void gvBank_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
    {
        //e.Cancel = true;
        //Clear();

        BankInfo_BO entity = new BankInfo_BO();
        entity.OID = gvInvestList.DataKeys[e.RowIndex].Value.ToString();

        string sql = string.Format(@"select * from Acc_Journal WHERE RefferenceNumber='{0}'", entity.OID);
        DataTable dt = DAL.LoadDataByQuery(sql);

        //BILL.Delete(entity);
        if (dt.Rows.Count == 2)
        {
            string sqlDelete = string.Format(@"delete Acc_Journal WHERE RefferenceNumber='{0}'", entity.OID);
            DAL.SaveDataCRUD(sqlDelete);

            lblMessageForList.Text = "Deleted Successfully!";
        }


        entity.EUSER = userID;
        entity.ShopID = Convert.ToInt16(ShopID);
        BindList();
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
", ShopID);
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



    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(ddlBank.SelectedValue.ToString()))
        {
            if (ddlOwnerAction.SelectedValue == "Invest")
            {
                SaveInvest();
            }
            else if (ddlOwnerAction.SelectedValue == "Withdraw")
            {
                string sql = string.Format(@"
select t.Branch,t.DebitSum,t.CreditSum,t.Balance 
from (
---------------------------------------------------------------------------------------
select b.Branch, b.DebitSum,b.CreditSum,b.Balance 
from dbo.vw_Shopwise_Bank_Balance b where b.Branch={0} AND b.AccountID='{1}'

-----------------------------------------------------------------------------------------
)t
", ShopID, ddlBank.SelectedValue);
                DataTable dt = DAL.LoadDataByQuery(sql);
                if (dt.Rows.Count > 0)
                {
                    string balance = dt.Rows[0]["Balance"].ToString();
                    if (Convert.ToInt32(balance) < Convert.ToInt32(txtAmount.Text))
                    {
                        lblMessage.Text = "No Enough Balance";
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        SaveWithdraw();
                    }
                }
                else
                {
                    lblMessage.Text = "No Enough Balance";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                }

            }

            txtAmount.Text = string.Empty;
            if (lblMessage.Text == "No Enough Balance")
            {
                //ContainerBankInfo.ActiveTabIndex = 1;
                txtAmount.Focus();
            }
            else
            {
                ContainerBankInfo.ActiveTabIndex = 0;
            }

        }
        else
        {
            //MSG SELECT BANK
            ddlBank.Focus();
        }


    }


    protected void btnSave2_Click(object sender, EventArgs e)
    {
        Label3.Text = string.Empty;

        //check
        string isValid = "yes";



        string amountLimitDDL = string.Empty;
        if (ddlPamymentFrom.SelectedItem.Text.Contains(":"))
        {
            string[] code = ddlPamymentFrom.SelectedItem.Text.Split(':'); amountLimitDDL = code[1];
        }
        decimal amountLimit = 0;
        decimal amount = 0;
        if (decimal.TryParse(amountLimitDDL, out amountLimit)) { }
        if (decimal.TryParse(txtAmount2.Text, out amount)) { }
        else
        {

            txtAmount2.Text = string.Empty;
            txtAmount2.Focus();
            return;
        }

        if (amount > amountLimit)
        {
            isValid = "no";

            txtAmount2.Text = string.Empty;
            txtAmount2.Focus();
            Label3.Text = "Please Check the Amount or Limit!";
            return;
        }

        //save
        if (isValid == "yes")
        {
            DailyCost_BO entity = new DailyCost_BO();
            string newbal = "";
            int com1 = 0;
            int com2 = 0;
            //entity.OID = lblOID.Value.ToString();
            entity.AMOUNTBUSINESS = txtAmount2.Text.ToString();
            entity.Shop_id = ShopID.ToString();
            entity.RemarksBUSINESS = txtRemarksBusiness.Text.ToString();
            //entity.CostingHeadID = ddlCostingHead.SelectedItem.Value.ToString();
            //entity.Shop_id = Shop_id.ToString();
            //entity.IUSER = userID.ToString();
            //string balval = entity.Shop_id;
            //newbal = BILL.getpettycash(balval);

            //com1 = Convert.ToInt32(newbal);
            //entity.AMOUNTBUSINESS = Convert.ToInt32(entity.AMOUNTBUSINESS);
            entity.RefNoBUSINESS = string.Format(@"Ref_InvWithdraw_{0}_{1}", entity.Shop_id, DateTime.Now.ToString("yyMMddhhmmss"));
            entity.IDATBUSINESS = string.IsNullOrEmpty(dptFromDate.Text) ? DateTime.Now.ToString("yyyy-MM-dd") : dptFromDate.Text;

            //string RefNo = string.Format(@"Ref_Expense_{0}_{1}", entity.Shop_id, entity.CostingHeadID);
            // BILL.Add(entity);

            SaveInvest2(entity);

            Clear();
            Label3.Text = "SAVED SUCCESSFULLY";
            entity = null;

            LoadDropDownPamymentFrom();
            //LoadGridDailyCostList();
            CurrentCashBalance(entity);
        }
    }

    private void SaveInvest2(DailyCost_BO entity)
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
VALUES('WithdrawFromBusiness','{0}','{1}','{2}','{3}','{4}'
,'{5}','{6}','{7}','{8}')

", entity.Shop_id, "WithdrawFromBusiness", "WithdrawFromBusiness", entity.AMOUNTBUSINESS
, 0, entity.RefNoBUSINESS, entity.IDATBUSINESS, entity.IDATBUSINESS, entity.RemarksBUSINESS);
        //            , 0,entity.RefNo, DateTime.Now.ToString(), DateTime.Now.ToString(), string.Format(@"Expense for {0}", ddlCostingHead.SelectedItem));


        string InsertCR = string.Format(@"
INSERT INTO Acc_Journal(AccountID,Branch,Particular,Remarks,Debit,Credit,RefferenceNumber,IDAT,IDATTIME,Narration) 
VALUES('{0}',{1},'{2}','{3}','{4}'
,'{5}','{6}','{7}','{8}','{9}')

", CashOrBank == "Cash" ? "1" : ddlPamymentFrom.SelectedValue.ToString()
, entity.Shop_id
, CashOrBank == "Cash" ? "Cash" : "Bank"
, CashOrBank == "Cash" ? "Cash" : "Bank"
, 0
, entity.AMOUNTBUSINESS, entity.RefNoBUSINESS, entity.IDATBUSINESS, entity.IDATBUSINESS, entity.RemarksBUSINESS);


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

    private void SaveInvest()
    {
        int shopid = Convert.ToInt32(ShopID);
        string IDAT = string.IsNullOrEmpty(dptDate.Text) ? DateTime.Now.ToString("yyyy-MM-dd") : dptDate.Text;
        string CashOrBank = ddlBank.SelectedValue == "1" ? "Cash" : "Bank";
        decimal amount = 0;
        string RefNo = string.Format(@"Ref_Invest_{0}_{1}", ShopID, DateTime.Now.ToString("yyMMddhhmmss"));
        if (decimal.TryParse(txtAmount.Text, out amount)) { }

        //journal
        //string StoreID = Session["StoreID"].ToString();
        string InsertDR = string.Format(@"
INSERT INTO Acc_Journal(AccountID,Branch,Particular,Remarks,Debit,Credit,RefferenceNumber,IDAT,IDATTIME,Narration) 
VALUES('{0}',{1},'{2}','{3}',{4}
,{5},'{6}','{7}','{8}','{9}')
", ddlBank.SelectedValue.ToString(), shopid, CashOrBank, CashOrBank, amount
 , 0, RefNo, IDAT, IDAT, string.Format(@"{0} to Business in {1}. {2}", ddlOwnerAction.SelectedValue.ToString(), CashOrBank, txtRemarks.Text.Trim()));

        string InsertCR = string.Format(@"
INSERT INTO Acc_Journal(AccountID,Branch,Particular,Remarks,Debit,Credit,RefferenceNumber,IDAT,IDATTIME,Narration) 
VALUES('{0}','{1}','{2}','{3}',{4}
,{5},'{6}','{7}','{8}','{9}')
", "Invest-1", shopid, ddlOwnerAction.SelectedValue.ToString(), "Invest", 0
, amount, RefNo, IDAT, IDAT, string.Format(@"{0} to Business in {1}. {2}", ddlOwnerAction.SelectedValue.ToString(),CashOrBank, txtRemarks.Text.Trim()));

        try
        {
            DAL.SaveDataCRUD(InsertDR);
            DAL.SaveDataCRUD(InsertCR);

            lblMessage.Text = "SAVED SUCCESSFULLY !";
            Clear();
            BindList();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            // dbConnect.Close();
        }
    }
    private void SaveWithdraw()
    {
        int shopid = Convert.ToInt32(ShopID);
        string CashOrBank = ddlBank.SelectedValue == "1" ? "Cash" : "Bank";
        decimal amount = 0;
        if (decimal.TryParse(txtAmount.Text, out amount)) { }

        string RefNo = string.Format(@"Ref_Withdraw_{0}_{1}", ShopID, DateTime.Now.ToString("yyMMddhhmmss"));
        //string RefNo = string.Format(@"RefWithdraw-{0}", DateTime.Now.ToString("yyMMddhhmmss"));
        //journal
        //string StoreID = Session["StoreID"].ToString();
        string InsertDR = string.Format(@"
INSERT INTO Acc_Journal(AccountID,Branch,Particular,Remarks,Debit,Credit,RefferenceNumber,IDAT,IDATTIME,Narration) 
VALUES('{0}',{1},'{2}','{3}',{4}
,{5},'{6}','{7}','{8}','{9}')
", "1", shopid, "Cash", "Cash", amount
 , 0, RefNo, DateTime.Now.ToString(), DateTime.Now.ToString(), string.Format(@"Cash withdraw from Bank. {0}",txtRemarks.Text.Trim()));
        string InsertCR = string.Format(@"
INSERT INTO Acc_Journal(AccountID,Branch,Particular,Remarks,Debit,Credit,RefferenceNumber,IDAT,IDATTIME,Narration) 
VALUES('{0}','{1}','{2}','{3}',{4}
,{5},'{6}','{7}','{8}','{9}')
", ddlBank.SelectedValue.ToString(), shopid, CashOrBank, CashOrBank, 0
, amount, RefNo, DateTime.Now.ToString(), DateTime.Now.ToString(), string.Format(@"Cash withdraw from Bank. {0}", txtRemarks.Text.Trim()));

        try
        {
            DAL.SaveDataCRUD(InsertDR);
            DAL.SaveDataCRUD(InsertCR);

            lblMessage.Text = "SAVED SUCCESSFULLY !";
            Clear();
            BindList();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            // dbConnect.Close();
        }
    }

    private void BindList()
    {

        DateTime dFromDate = DateTime.Now;
        DateTime dToDate = DateTime.Now;
        if (DateTime.TryParse(dptFromDate.Text, out dFromDate)) { }
        if (DateTime.TryParse(dptToDate.Text, out dToDate)) { }


// string param = string.Format(@" and j.Branch={0}", ShopID);       
        //if (DateTime.TryParse(dptFromDate.Text, out dFromDate) && DateTime.TryParse(dptToDate.Text, out dToDate))
//        {
//            param += string.Format(@" and CONVERT(date, j.IDATTIME) between '{0}' and '{1}'", dFromDate.ToString("yyyy-MM-dd"), dToDate.ToString("yyyy-MM-dd"));
//        }
//        else
//        {
//            param += string.Format(@" and j.IDAT between '{0}' and '{1}'", DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd"));

//        }

//        //int shopOID = Convert.ToInt32(ShopID);
//        string sql = string.Format(@"
//select j.Journal_OID,j.AccountID,j.Particular,j.RefferenceNumber,j.Debit,j.Credit,CONVERT(nvarchar(12),j.IDAT,106)IDAT
//,case when j.Remarks='Bank' then (j.Remarks+' ('+ bi.BankName+'_'+ bi.AccountNo+')') else j.Remarks end Remarks
//,bi.BankName,bi.AccountNo
//from Acc_Journal j 
//left join BankInfo bi on CONVERT(nvarchar(50),bi.OID) =j.AccountID
//where j.Particular='Invest' {0}
//
//union all
//select j.Journal_OID,j.AccountID,j.Particular,j.RefferenceNumber,j.Debit,j.Credit,CONVERT(nvarchar(12),j.IDAT,106)IDAT
//,case when j.Remarks='Bank' then (j.Remarks+' ('+ bi.BankName+'_'+ bi.AccountNo+')') else j.Remarks end Remarks
//,bi.BankName,bi.AccountNo
//from Acc_Journal j 
//left join BankInfo bi on CONVERT(nvarchar(50),bi.OID) =j.AccountID
//where j.Particular='WithdrawFromBusiness' {0}
//", param);

      string  sql = string.Format(@"
exec dbo.spInvestmentList @branch='{0}',@ds='{1}',@de='{2}'
",ShopID,dFromDate,dToDate);
        DataTable dt = DAL.LoadDataByQuery(sql);

        gvInvestList.DataSource = null;
        if (dt.Rows.Count > 0) { gvInvestList.DataSource = dt; }
        gvInvestList.DataBind();
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
        dptDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        txtAmount2.Text = string.Empty;
        txtRemarksBusiness.Text = string.Empty;
        ddlBank.SelectedIndex = -1;
        ddlOwnerAction.SelectedIndex = -1;
    }



    private void CurrentCashBalance(object sender)
    {
        string sql = string.Format(@"
select t.Branch,t.BankName,t.AccountNo,t.Debit,t.Credit,t.Balance 
from (
---------------------------------------------------------------------------------------
select b.Branch, BankName='Cash',AccountNo='',b.Debit,b.Credit,b.Balance 
from dbo.vw_Shopwise_Cash_Balance b where b.Branch={0}

-----------------------------------------------------------------------------------------
)t
", ShopID);
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
    private void LoadDropDownPamymentFrom2()
    {
        string sql = string.Format(@"
select t.AccountID,(t.BankName+'_A/C-'+t.AccountNo+'-BDT:'+CONVERT(nvarchar(15),t.Balance)) text
,t.BankName,t.AccountNo,t.Debit,t.Credit,t.Balance,t.Branch 
from (


select b.Branch, b.BankName,b.AccountNo,b.DebitSum Debit,b.CreditSum Credit,b.Balance ,b.AccountID
from dbo.vw_Shopwise_Bank_Balance b where b.Branch={0}

)t
", ShopID);
        DataTable dt = DAL.LoadDataByQuery(sql);
        if (dt.Rows.Count > 0)
        {
            ddlPamymentFromBank.DataSource = dt;
            ddlPamymentFromBank.DataTextField = "text";
            ddlPamymentFromBank.DataValueField = "AccountID";
            ddlPamymentFromBank.DataBind();

            //ddlPamymentFrom.Items.Insert(0, new ListItem("Cash", "1"));

        }
    }
    protected void ddlOwnerAction_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadDropDownBank();
    }
    protected void btnSearchInvest_Click(object sender, EventArgs e)
    {
        BindList();
    }
    //protected void btntransfer_Click(object sender, EventArgs e)
    //{
    //   DataTable dt = new DataTable() ;
    //    string am = dt.Rows [0]["Amount"].ToString ();
    //}
    protected void TransferCash_Click(object sender, EventArgs e)
    {
        //sadiq   20 oct 2018
        lblsuccessorfail.Text = string.Empty;
        if (string.IsNullOrEmpty(ddlPamymentFromBank.SelectedValue))
        {
            ddlPamymentFromBank.Focus();
            lblsuccessorfail.Text = string.Format(@"Please, select a Bank!");
            return;
        }


        string sql = string.Format(@"
select t.Branch,t.BankName,t.AccountNo,t.Debit,t.Credit,t.Balance 
from (
---------------------------------------------------------------------------------------
select b.Branch, BankName='Cash',AccountNo='',b.Debit,b.Credit,b.Balance 
from dbo.vw_Shopwise_Cash_Balance b where b.Branch={0}

-----------------------------------------------------------------------------------------
)t
", ShopID);
        DataTable dt34 = DAL.LoadDataByQuery(sql);
        string cashbalanceam = dt34.Rows[0]["Balance"].ToString();

        int a = 0;
        if (int.TryParse(txtAmountTransfer111.Text, out a)) { }
        if (Convert.ToInt64 (cashbalanceam) < a)
        {
            lblsuccessorfail.Text = "Not Enough cash Balance";
            lblsuccessorfail.ForeColor = System.Drawing.Color.Red;
        }
        else
        {
            string RefNo = string.Format(@"Ref_CashtoBank", ShopID, DateTime.Now.ToString("yyMMddhhmmss"));
            string InsertDRTransfer = string.Format(@"
INSERT INTO Acc_Journal(AccountID,Branch,Particular,Remarks,Debit,Credit,RefferenceNumber,IDAT,IDATTIME,Narration) 
VALUES('{0}',{1},'{2}','{3}',{4}
,{5},'{6}','{7}','{8}','{9}')
", "1", ShopID, "Cash", "Cash", 0, txtAmountTransfer111.Text
 , RefNo, DateTime.Now.ToString(), DateTime.Now.ToString(), string.Format(@"Cash Transfer to Bank"));
            string InsertCRTransfer = string.Format(@"
INSERT INTO Acc_Journal(AccountID,Branch,Particular,Remarks,Debit,Credit,RefferenceNumber,IDAT,IDATTIME,Narration) 
VALUES('{0}','{1}','{2}','{3}',{4}
,{5},'{6}','{7}','{8}','{9}')
", ddlPamymentFromBank.SelectedValue.ToString(), ShopID, string.Format(@"Bank"), string.Format(@"Bank"), txtAmountTransfer111.Text
    , 0, RefNo, DateTime.Now.ToString(), DateTime.Now.ToString(), string.Format(@"Cash Transfer to Bank"));

            try
            {
                DAL.SaveDataCRUD(InsertDRTransfer);
                DAL.SaveDataCRUD(InsertCRTransfer);

                lblsuccessorfail.Text = "SAVED SUCCESSFULLY !";
                txtAmountTransfer111.Text = string.Empty;
                // BindList();
                LoadDropDownPamymentFrom();
                CurrentCashBalance(ShopID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                // dbConnect.Close();
            }
        }
    }

}