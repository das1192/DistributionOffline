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

namespace TalukderPOS.Web.UI
{
    public partial class Vendor_OutgoingForm : System.Web.UI.Page
    {
        private string userID = string.Empty;
        private string Shop_id = string.Empty;
        private string userPassword = string.Empty;

        Vendor_Outgoing BILL = new Vendor_Outgoing();
        CommonDAL DAL = new CommonDAL();
        T_PRODBLL BILL2 = new T_PRODBLL();

        SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
        SqlDataAdapter da = new SqlDataAdapter();
        SqlCommand cmd;
        String sql;

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
                Vendor_Outgoing_BO entity = new Vendor_Outgoing_BO();
                entity.Shop_id = Shop_id.ToString();

                CurrentCashBalance();

                dptSupplierPaymentDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

                lblMsgSupplierPaymentList.Text = string.Empty;
                LoadDDLVendor();
            }
        }

        //new by das loading vendor list
        private void LoadDDLVendor()
        {


            string sql = string.Format(@"SELECT id= OID,text=Vendor_Name FROM Vendor where Vendor_Active=1 AND  Shop_id='{0}' ", Session["StoreID"].ToString());
            DataTable dt = DAL.LoadDataByQuery(sql);

            if (dt.Rows.Count > 0)
            {
                ddlVendorList.DataSource = dt;
                ddlVendorList.DataValueField = "id";
                ddlVendorList.DataTextField = "text";
                ddlVendorList.DataBind();

                ddlVendorList.Items.Insert(0, new ListItem("-- Select One--", String.Empty));

            }
        }













        protected void cmdSearchForprice_Click(object sender, EventArgs e)
        {



            T_PROD entity = new T_PROD();
            entity.Branch = Session["StoreID"].ToString();
            entity.Vendor_ID = ddlVendorID.SelectedItem.Value.ToString();



            DataTable dt = BILL2.RetailerDueReport(entity);
            Session["GridData"] = dt;
            BindList1();
        }
        private void BindList1()
        {
            gvVendorDue.DataSource = Session["GridData"];
            gvVendorDue.DataBind();
        }
        protected void gvVendorAmount_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            e.Cancel = true;
            Clear();
            Vendor_Outgoing_BO entity = new Vendor_Outgoing_BO();
            String OID = gvVendorAmount.DataKeys[e.NewEditIndex].Value.ToString();
            entity = BILL.GetById(OID);
            lblOID.Value = entity.OID;
            txtAmount.Text = entity.AMOUNT;
            txtRemarks.Text = entity.Remarks;
            ddlVendorList.SelectedValue = entity.Vendor_ID.ToString();   //.SelectedValue.ToString() = entity.Vendor_ID.ToString();
            ContainerProductVendor.ActiveTabIndex = 0;
            lblMessage.Text = string.Empty;
        }


        protected void gvVendorAmount_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            Vendor_Outgoing_BO entity = new Vendor_Outgoing_BO();
            entity.OID = gvVendorAmount.DataKeys[e.RowIndex].Value.ToString();


            string sql = string.Format(@"
select t.rowNumber from (

select Journal_OID as rowNumber from Acc_Journal where RefferenceNumber='{0}'
union
select OID as rowNumber from Vendor_Outgoing where ReferenceNo='{0}'
union
select  OID as rowNumber from CASHINOUT where ReferenceNo='{0}'

)t", entity.OID);
            DataTable dt = DAL.LoadDataByQuery(sql);

            if (dt.Rows.Count == 4)
            {
                string sqlDelete = string.Format(@"
delete Acc_Journal where RefferenceNumber='{0}'
delete Vendor_Outgoing where ReferenceNo='{0}'
delete CASHINOUT where ReferenceNo='{0}'
", entity.OID);
                DAL.SaveDataCRUD(sqlDelete);

                lblMsgSupplierPaymentList.Text = "Deleted Successfully!";
            }



            //entity = BILL.GetById(entity.OID);
            //entity.Shop_id = Shop_id.ToString();
            //entity.IUSER = userID;



            //BILL.Delete(entity);
            BindList(entity); //

            CurrentCashBalance();
        }



        protected void btnSave_Click(object sender, EventArgs e)
        {
            string isValid = "yes";
            if (string.IsNullOrEmpty(ddlVendorList.SelectedValue.ToString()))
            {
                isValid = "no";
                ddlVendorList.Focus();
                lblMessage.Text = "Please Select the Vendor!";
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









            if (isValid == "yes")
            {
                //check DR  CR




                //
                Vendor_Outgoing_BO entity = new Vendor_Outgoing_BO();
                entity.OID = lblOID.Value.ToString();
                entity.AMOUNT = txtAmount.Text.ToString();
                entity.Vendor_ID = ddlVendorList.SelectedItem.Value.ToString();
                entity.PaymentFrom = ddlPamymentFrom.SelectedItem.Value.ToString();
                entity.Shop_id = Shop_id.ToString();
                entity.IUSER = userID.ToString();
                entity.Remarks = txtRemarks.Text.ToString();

                entity.RefferenceNumber = string.Format(@"Ref_SupplierPayment_{0}_{1}_{2}", entity.Shop_id, ddlVendorList.SelectedValue.ToString(), DateTime.Now.ToString("yyMMddhhmmss"));
                entity.IDAT = string.IsNullOrEmpty(dptSupplierPaymentDate.Text) ? DateTime.Now.ToString("yyyy-MM-dd") : dptSupplierPaymentDate.Text;


                BILL.Add(entity);
                Clear();
                lblMessage.Text = "SAVED SUCCESSFULLY";
                entity = null;
                BindList(entity);

                //
                CurrentCashBalance();
            }
        }

        private void BindList(object sender)
        {
            Vendor_Outgoing_BO entity = new Vendor_Outgoing_BO();
            entity.Shop_id = Shop_id.ToString();
            DataTable dt = BILL.BindList(entity);

            gvVendorAmount.DataSource = null;
            gvVendorAmount.DataSource = dt;
            gvVendorAmount.DataBind();
        }
        protected void gvT_Delete_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            Vendor_Outgoing_BO entity = new Vendor_Outgoing_BO();
            gvT_Delete.PageIndex = e.NewPageIndex;

            BindList2();
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
            ddlPamymentFrom.Items.Clear();
            ddlVendorList.SelectedIndex = -1;
        }
        protected void cmdSearchDelete_Click(object sender, EventArgs e)
        {
            Vendor_Outgoing_BO entity = new Vendor_Outgoing_BO();
            entity.Shop_id = Shop_id.ToString();

            entity.FromDate = txtDateFromdelete.Text;
            entity.ToDate = txtDateTodelete.Text;
            DataTable dt = BILL.BindListdelete(entity);
            Session["GridDatadelete"] = dt;
            BindList2();

        }
        private void BindList2()
        {
            gvT_Delete.DataSource = Session["GridDatadelete"];
            gvT_Delete.DataBind();
        }

        protected void cmdSearchSup_Click(object sender, EventArgs e)
        {
            Vendor_Outgoing_BO entity = new Vendor_Outgoing_BO();
            entity.Shop_id = Shop_id.ToString();
            entity.Vendor_ID = ddlVendorListnew.SelectedItem.Value.ToString();
            entity.FromDate = FromdateSup.Text;
            entity.ToDate = TodateSup.Text;
            DataTable dt = BILL.BindList(entity);

            gvVendorAmount.DataSource = dt;
            gvVendorAmount.DataBind();
        }
        //102 supplier transaction
        protected void btnSearchSupplierTransaction_Click(object sender, EventArgs e)
        {
            //param
            string strParam = string.Empty;
            if (!string.IsNullOrEmpty(dtFromDateST.Text) && !string.IsNullOrEmpty(dtToDateST.Text))
            {
                strParam += string.Format(@" and ss.IDAT between '{0}' and '{1}' ", dtFromDateST.Text, dtToDateST.Text);
            }
            else
            {
                dtFromDateST.Text = string.Empty;
                dtToDateST.Text = string.Empty;
                //when one or both date empty
                strParam += string.Format(@" and DATEPART(MONTH, ss.IDAT) = DATEPART(MONTH, getdate())");
            }
            if (!string.IsNullOrEmpty(ddlParticularST.SelectedValue) && ddlParticularST.SelectedValue != "--Please Select--")
            {
                strParam += string.Format(@" and ss.Particular = '{0}' ", ddlParticularST.SelectedValue);
            }
            if (!string.IsNullOrEmpty(ddlVendorST.SelectedValue) && ddlVendorST.SelectedValue != "--Please Select--")
            {
                strParam += string.Format(@" and ss.Vendor_ID = '{0}' ", ddlVendorST.SelectedValue);
            }
            strParam += string.Format(@" and ss.Branch = '{0}' ", Session["StoreID"].ToString());

            //con  sqlStatement
            string constr = ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString;

            string query = string.Format(@"
select ss.IDAT,ss.Branch,ss.Vendor_ID,v.Vendor_Name
,ss.Particular,ss.Total 
,case 
when Particular='Payment'  then ''
when Particular='Purchase Amendment'  then ''
else CONVERT(nvarchar(150),TotalQty )
end as TotalQty
from(

select s.IDAT, s.Branch,s.Vendor_ID,Particular='Purchase',SUM(ISNULL(s.CostPrice,0)*isnull(s.Quantity,0)) as Total
,SUM(isnull(s.Quantity,0)) as TotalQty
from Purchase_Report s where s.Particular='Purchase'
GROUP BY IDAT, s.Branch,s.Vendor_ID

union all
select pa.IDAT, pa.Branch,pa.Vendor_ID,Particular='Purchase Amendment', SUM(ISNULL(pa.Amount,0)) as Total
,'' as TotalQty
from PurchasePrice_Amendment pa
group by pa.IDAT, pa.Branch,pa.Vendor_ID

union all
select pr.IDAT,pr.Branch,pr.Vendor_ID,Particular='Purchase Return',SUM(ISNULL(pr.CostPrice,0)*isnull(pr.Quantity,0))as Total
,SUM(isnull(pr.Quantity,0)) as TotalQty
from Purchase_Return pr
group by pr.IDAT,pr.Branch,pr.Vendor_ID

union all
select vo.IDAT,vo.Shop_id as Branch,vo.Vendor_ID,Particular='Payment' 
,SUM(CONVERT(decimal(18,2),Replace(AMOUNT,CHAR(9),''))) as Total   --to remove tab in amount   oid 105
,'' as TotalQty
from Vendor_Outgoing vo
group by vo.IDAT,vo.Shop_id,vo.Vendor_ID
)ss
inner join Vendor v on v.OID=ss.Vendor_ID

where 1=1 {0} 
order by IDAT desc

", strParam);

            #region MyRegion
            //            string query = string.Format(@"
            //select ss.IDAT,ss.Branch,ss.Vendor_ID,v.Vendor_Name
            //,ss.Particular,ss.Total 
            //from(
            //
            //select s.IDAT, s.Branch,s.Vendor_ID,Particular='Purchase',SUM(ISNULL(s.CostPrice,0)*isnull(s.Quantity,0)) as Total
            //from T_STOCK s
            //GROUP BY IDAT, s.Branch,s.Vendor_ID
            //
            //union all
            //select pa.IDAT, pa.Branch,pa.Vendor_ID,Particular='Purchase Amendment', SUM(ISNULL(pa.Amount,0)) as Total
            //from PurchasePrice_Amendment pa
            //group by pa.IDAT, pa.Branch,pa.Vendor_ID
            //
            //union all
            //select pr.IDAT,pr.Branch,pr.Vendor_ID,Particular='Purchase Return',SUM(ISNULL(pr.CostPrice,0)*isnull(pr.Quantity,0))as Total
            //from Purchase_Return pr
            //group by pr.IDAT,pr.Branch,pr.Vendor_ID
            //
            //union all
            //select vo.IDAT,vo.Shop_id as Branch,vo.Vendor_ID,Particular='Payment' 
            //,SUM(CONVERT(decimal(18,2),Replace(AMOUNT,CHAR(9),''))) as Total   --to remove tab in amount   oid 105
            //from Vendor_Outgoing vo
            //group by vo.IDAT,vo.Shop_id,vo.Vendor_ID
            //)ss
            //inner join Vendor v on v.OID=ss.Vendor_ID
            //
            //where 1=1 {0} 
            //order by IDAT desc
            //
            //", strParam); 
            #endregion
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            gvSupplierTransaction.DataSource = null;
                            gvSupplierTransaction.DataSource = dt;
                            gvSupplierTransaction.DataBind();

                            //to load totQty and totGrand
                            double grandtotal = 0;

                            double totalPurchase = 0;
                            double totalPurchaseAmendment = 0;
                            double totalPurchaseReturn = 0;
                            double totalPayment = 0;

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                grandtotal += Convert.ToDouble(dt.Rows[i]["Total"].ToString());

                                if (dt.Rows[i]["Particular"].ToString() == "Purchase")
                                { totalPurchase += Convert.ToDouble(dt.Rows[i]["Total"].ToString()); }

                                else if (dt.Rows[i]["Particular"].ToString() == "Purchase Amendment")
                                { totalPurchaseAmendment += Convert.ToDouble(dt.Rows[i]["Total"].ToString()); }

                                else if (dt.Rows[i]["Particular"].ToString() == "Purchase Return")
                                { totalPurchaseReturn += Convert.ToDouble(dt.Rows[i]["Total"].ToString()); }

                                else if (dt.Rows[i]["Particular"].ToString() == "Payment")
                                { totalPayment += Convert.ToDouble(dt.Rows[i]["Total"].ToString()); }

                            }

                            //show total on a label
                            lblTotal.Text = "<span class='label label-success'>Total</span>";

                            lblPurchase.Text = "<span class='label label-primary'>Purchase : " + totalPurchase.ToString("0.00") + "</span>";
                            lblPurchaseAmendment.Text = "<span class='label label-warning'>Purchase Amendment : " + totalPurchaseAmendment.ToString("0.00") + "</span>";
                            lblPurchaseReturn.Text = "<span class='label label-warning'>Purchase Return : " + totalPurchaseReturn.ToString("0.00") + "</span>";
                            lblPayment.Text = "<span class='label label-warning'>Payment : " + totalPayment.ToString("0.00") + "</span>";

                            string balance = (totalPurchase - (totalPurchaseAmendment + totalPurchaseReturn + totalPayment)).ToString("0.00");
                            lblBalance.Text = "<span class='label label-success'>Balance : " + balance + "</span>";
                            //lblGrandTotalST.Text = "<span class='label label-danger'>Total Amount : " + Convert.ToDecimal(grandtotal).ToString("#,##0.00") + "</span>";

                        }
                    }
                }
            }
        }

        //payment
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
                gvCashBalance.DataSource = null;
                gvCashBalance.DataSource = dt;
                gvCashBalance.DataBind();
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
                ddlPamymentFrom.DataSource = dt;
                ddlPamymentFrom.DataTextField = "text";
                ddlPamymentFrom.DataValueField = "AccountID";
                ddlPamymentFrom.DataBind();

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
                    lblPayableAmount.ForeColor = System.Drawing.Color.Red ;
                    lblPayablelbl.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    lblPayablelbl.Text = "Accounts Receivable";
                    lblPayableAmount.Text = Math .Abs ( Balance).ToString("0.00");
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
                ddlPamymentFrom.Items.Clear();
            }
        }
    }
}
