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
using System.Web.UI;

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
                LoadDropDown();
            }
        }

        //new by das loading vendor list
        private void LoadDropDown()
        {
            //string sql = string.Format(@"SELECT id= OID,text=Vendor_Name FROM Vendor where Vendor_Active=1 AND  Shop_id='{0}' ", Session["StoreID"].ToString());
            string sql = string.Format(@"Select ID,Name+' (0'+Cast(Number as varchar(50))+')' as text From Customers Where CustomerStatus=1 And Branch='{0}'", Session["StoreID"].ToString());
            DataTable dt = DAL.LoadDataByQuery(sql);

            if (dt.Rows.Count > 0)
            {
                ddlRetailerList.DataSource = dt;
                ddlRetailerList.DataValueField = "id";
                ddlRetailerList.DataTextField = "text";
                ddlRetailerList.DataBind();
                ddlRetailerList.Items.Insert(0, new ListItem("-- Select One--", String.Empty));
            }

            dt = new DataTable();

            sql = string.Format(@"Select OID,PaymentMode From PaymentMode Where OID IN(11,12,14,19)");
            dt = DAL.LoadDataByQuery(sql);
            if (dt.Rows.Count > 0)
            {
                ddlPaymentMode.DataSource = dt;
                ddlPaymentMode.DataValueField = "OID";
                ddlPaymentMode.DataTextField = "PaymentMode";
                ddlPaymentMode.DataBind();
                ddlPaymentMode.Items.Insert(0, new ListItem("-- Select One--", String.Empty));
            }

            // Bank
            dt = new DataTable();

            sql = string.Format(@"Select OID,BankName=(BankName+' ('+AccountNo+')') From BankInfo Where ActiveStatus=1 And ShopID='62'");
            dt = DAL.LoadDataByQuery(sql);
            if (dt.Rows.Count > 0)
            {
                ddlBank.DataSource = dt;
                ddlBank.DataValueField = "OID";
                ddlBank.DataTextField = "BankName";
                ddlBank.DataBind();
                ddlBank.Items.Insert(0, new ListItem("-- Select One--", String.Empty));
            }
        }

        protected void cmdSearchForprice_Click(object sender, EventArgs e)
        {
            T_PROD entity = new T_PROD();
            entity.Branch = Session["StoreID"].ToString();
            entity.Vendor_ID = ddlRetailerID.SelectedItem.Value.ToString();

            DataTable dt = BILL2.RetailerDueReport(entity);
            Session["GridData"] = dt;
            BindList1();
        }

        protected void Print_Click(object sender, EventArgs e)
        {
            T_PROD entity = new T_PROD();
            entity.Branch = Session["StoreID"].ToString();
            entity.Vendor_ID = ddlRetailerID.SelectedItem.Value.ToString();

            DataTable dt = BILL2.RetailerDueReport(entity);
            Session["dtsales"] = dt;
            Session["ReportPath"] = "~/Reports/rptRetailerPayment.rpt";
            string webUrl = "../Reports/ReportView.aspx";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenWindow", "window.open('" + webUrl + "');", true);


        }

        private void BindList1()
        {
            grdRetailerDue.DataSource = Session["GridData"];
            grdRetailerDue.DataBind();
        }

        protected void grdRetailerAmt_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            e.Cancel = true;
            Clear();
            Vendor_Outgoing_BO entity = new Vendor_Outgoing_BO();
            String OID = grdRetailerAmt.DataKeys[e.NewEditIndex].Values["OID"].ToString();
            entity = BILL.RetailerPaymentById(OID);

            if (entity != null) 
            {
                lblOID.Value = entity.OID;
                ddlRetailerID.SelectedValue = entity.Vendor_ID;
                hdfRefNo.Value = entity.RefferenceNumber;
                dptSupplierPaymentDate.Text = entity.IDAT;
                ddlRetailerList.SelectedValue = entity.Vendor_ID.ToString();
                txtRemarks.Text = entity.Remarks;
                ddlPaymentMode.SelectedValue = entity.PaymentModeID;

                txtAmount.Text = entity.AMOUNT;
                txtBankAmount.Text = entity.CardAmt;
                   
                ContainerProductVendor.ActiveTabIndex = 0;
                lblMessage.Text = string.Empty;
                ddlPaymentMode_SelectedIndexChanged(this, new EventArgs());
            }
        }


        protected void grdRetailerAmt_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            Vendor_Outgoing_BO entity = new Vendor_Outgoing_BO();
            entity.OID = grdRetailerAmt.DataKeys[e.RowIndex].Values["OID"].ToString();
            entity.RefferenceNumber = grdRetailerAmt.DataKeys[e.RowIndex].Values["ReferenceNo"].ToString();


            string sql = string.Format(@"
select t.rowNumber from (

select Journal_OID as rowNumber from Acc_Journal where RefferenceNumber='{0}'
union
select OID as rowNumber from RetailerPayments where ReferenceNo='{0}'
union
select  OID as rowNumber from CASHINOUT where INVOICEID='{0}'

)t", entity.RefferenceNumber);
            DataTable dt = DAL.LoadDataByQuery(sql);

            if (dt.Rows.Count ==4)
            {
                string sqlDelete = string.Format(@"
delete Acc_Journal where RefferenceNumber='{0}'
delete RetailerPayments where ReferenceNo='{0}'
delete CASHINOUT where INVOICEID='{0}'
", entity.RefferenceNumber);
                DAL.SaveDataCRUD(sqlDelete);

                lblMsgSupplierPaymentList.Text = "Deleted Successfully!";
            }

            cmdSearchSup_Click(this, new EventArgs());
        }

        private bool IsValidRetailerPayment() 
        {
            bool isValid = true;
            txtAmount.Text = txtAmount.Text.Trim() == "" ? "0" : txtAmount.Text.Trim();
            txtBankAmount.Text = txtBankAmount.Text.Trim() == "" ? "0" : txtBankAmount.Text.Trim();
            lblPayableAmount.Text = lblPayableAmount.Text.Trim() == "" ? "0" : lblPayableAmount.Text.Trim();

            if (string.IsNullOrEmpty(ddlRetailerList.SelectedValue.ToString()))
            {
                isValid = false;
                ddlRetailerList.Focus();
                lblMessage.Text = "Please Select the Retailer!";
                
            }
            
            if (lblPayablelbl.Text.Trim() == "Accounts Payable")
            {
                if (ddlPaymentMode.SelectedItem.Text != "Pay To Retailer") 
                {
                    isValid = false;
                    lblMessage.Text = "Please Select Payment mode of type (Pay To Retailer)";
                }
            }
            if (lblPayableAmount.Text == "" || lblPayableAmount.Text == "0" || lblPayableAmount.Text == "0.00") {
                isValid = false;
                lblMessage.Text = "Payable amount can not be zero/empty..!";
            }

            //if (lblPayablelbl.Text.Trim() != "Accounts Payable")
            //{
                if ((Convert.ToInt32(txtAmount.Text) + Convert.ToInt32(txtBankAmount.Text)) > Convert.ToDecimal(lblPayableAmount.Text))
                {
                    isValid = false;
                    lblMessage.Text = "Payment Can not be greater than Due Amount!";
                } 
           // }
                if (lblPayablelbl.Text.Trim() != "Accounts Payable") 
                {
                    if (ddlPaymentMode.SelectedItem.Text == "Pay To Retailer") 
                    {
                        isValid = false;
                        lblMessage.Text = "Invalid Payment mode..!";
                    }
                }

            if (ddlPaymentMode.SelectedIndex <= 0)
            {
                isValid = false;
                lblMessage.Text = "Please select a Payment Mode..!";
            }
            if (ddlPaymentMode.SelectedItem.Text == "Cash" || ddlPaymentMode.SelectedItem.Text == "Pay To Retailer")
            {
                if (txtAmount.Text == "" || txtAmount.Text == "0" || Convert.ToInt32(txtAmount.Text) <= 0)
                {
                    isValid = false;
                    lblMessage.Text = "Please Enter Cash Amount..!";
                }
            }
            if (ddlPaymentMode.SelectedItem.Text == "Card")
            {
                if (txtBankAmount.Text == "" || txtBankAmount.Text == "0" || Convert.ToInt32(txtBankAmount.Text) <= 0)
                {
                    isValid = false;
                    lblMessage.Text = "Please Enter Card Amount..!";
                }
            }

            if (ddlPaymentMode.SelectedItem.Text == "Cash & Card")
            {
                if (txtAmount.Text == "" || txtAmount.Text == "0" || Convert.ToInt32(txtAmount.Text) <= 0 || txtBankAmount.Text == "" || txtBankAmount.Text == "0" || Convert.ToInt32(txtBankAmount.Text) <= 0)
                {
                    isValid = false;
                    lblMessage.Text = "Please Enter Cash And Card Amount..!";
                }
            }
            if (isValid) 
            {
                btnSave.Enabled = false;
            }

            return isValid;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (IsValidRetailerPayment())
            {
                Vendor_Outgoing_BO entity = new Vendor_Outgoing_BO();
                entity.AMOUNT = (txtAmount.Text.Trim() == "" ? "0.00" : txtAmount.Text.Trim());
                entity.Vendor_ID = ddlRetailerList.SelectedValue; //Customer
                entity.Shop_id = Shop_id.ToString();
                entity.IUSER = userID.ToString();
                entity.Remarks = txtRemarks.Text.ToString();
                if (lblOID.Value.Trim() == "") 
                {
                    entity.RefferenceNumber = string.Format(@"Ref_RetailerPayment_{0}_{1}_{2}", entity.Shop_id, ddlRetailerList.SelectedValue.ToString(), DateTime.Now.ToString("yyMMddhhmmss"));
                }
                else
                {
                    entity.RefferenceNumber = hdfRefNo.Value;
                }
                
                entity.IDAT = string.IsNullOrEmpty(dptSupplierPaymentDate.Text) ? DateTime.Now.ToString("yyyy-MM-dd") : dptSupplierPaymentDate.Text;
                
                // New 
                entity.PaymentModeID = ddlPaymentMode.SelectedValue;
                entity.BankId = (ddlBank.SelectedIndex <=0 ? "0" : ddlBank.SelectedValue);
                entity.CardAmt = (txtBankAmount.Text.Trim() == "" ? "0.00" : txtBankAmount.Text.Trim());

                entity.PaymentId = (lblOID.Value.Trim() == "" ? "0" : lblOID.Value.Trim());
                entity.IUSER = Session["UserID"].ToString();

                BILL.AddRetailerPaymentAdj(entity);
                Clear();
                lblMessage.Text = "SAVED SUCCESSFULLY";
                btnSave.Visible = true;
                btnSave.Enabled = true;
            }
        }

        private void BindList(object sender)
        {
            Vendor_Outgoing_BO entity = new Vendor_Outgoing_BO();
            entity.Shop_id = Shop_id.ToString();
            DataTable dt = BILL.BindList(entity);

            grdRetailerAmt.DataSource = null;
            grdRetailerAmt.DataSource = dt;
            grdRetailerAmt.DataBind();
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
            ddlRetailerList.SelectedIndex = -1;
            ddlPaymentMode.SelectedIndex = 0;
            txtBankAmount.Text = string.Empty;
            if (ddlBank.SelectedIndex >=0){
            ddlBank.SelectedIndex = -1;
            }
            
            
            lblPayablelbl.Text = "";
            hdfRefNo.Value = "";
            lblPayableAmount.Text = "0.00";
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
            entity.Vendor_ID = ddlRetailerListnew.SelectedItem.Value.ToString();
            entity.FromDate = FromdateSup.Text;
            entity.ToDate = TodateSup.Text;
            DataTable dt = BILL.RetailerPaymentHistory(entity);

            grdRetailerAmt.DataSource = dt;
            grdRetailerAmt.DataBind();
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
        protected void ddlRetailerList_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblPayableAmount.Text = "0.00";

            if (!string.IsNullOrEmpty(ddlRetailerList.SelectedValue))
            {
                string sql = string.Format(@"
select sum(j.Debit),sum(j.Credit),Balance=ISNULL((sum(j.Credit-j.Debit)),0) 
from Acc_Journal j
where j.Particular='A/R' and j.Remarks='Customer' and j.Branch='{0}' and j.AccountID={1}
", Shop_id, ddlRetailerList.SelectedValue);
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

        }

        protected void ddlPaymentMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPaymentMode.SelectedItem.Text == "Card" || ddlPaymentMode.SelectedItem.Text == "Cash & Card") 
            {
                trBank.Visible = true;
                txtAmount.Text = "0";
                trCashAmt.Visible = true;
                txtBankAmount.Text = "0";
                trBankAmt.Visible = true;
                if (ddlPaymentMode.SelectedItem.Text == "Card")
                {
                    txtAmount.Text = "0";
                    trCashAmt.Visible = false;
                }
            }
            else if (ddlPaymentMode.SelectedItem.Text == "Cash" || ddlPaymentMode.SelectedItem.Text == "Pay To Retailer")
            {
                trBank.Visible = false;
                txtAmount.Text = "0";
                trCashAmt.Visible = true;
                txtBankAmount.Text = "0";
                trBankAmt.Visible = false;
            }
        }
}
}
