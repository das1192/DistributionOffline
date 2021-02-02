using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TalukderPOS.DAL;
using TalukderPOS.BLL;
using TalukderPOS.BusinessObjects;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;


public partial class Pages_ReportAccLedger : System.Web.UI.Page
{
    private string userID = string.Empty;
    private string userPassword = string.Empty;
    SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
    String sql;
    SqlCommand cmd;
    SqlDataAdapter da;
    CommonDAL DAL = new CommonDAL();


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            userID = Session["UserID"].ToString();
            userPassword = Session["Password"].ToString();
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
            //lblMessage.Text = string.Empty;
            LoadDDL();

            DateTime date = DateTime.Now;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            dptFromDate.Text = firstDayOfMonth.ToString("dd MMM yyyy");
            dptToDate.Text = date.ToString("dd MMM yyyy");
        }

    }

    private void LoadDDL()
    {
        LoadDDLDetailsType();
    }
    private void LoadDDLDetailsType()
    {
        string sql = string.Format(@"
select distinct J.Particular as id,j.Particular as text 
from Acc_Journal j 
where j.Branch='{0}'
order by j.Particular
", Session["StoreID"].ToString());
        DataTable dt = DAL.LoadDataByQuery(sql);

        if (dt.Rows.Count > 0)
        {
            ddlDetailsType.DataSource = dt;
            ddlDetailsType.DataValueField = "id";
            ddlDetailsType.DataTextField = "text";
            ddlDetailsType.DataBind();

            ddlDetailsType.Items.Insert(0, new ListItem("-- Select One--", String.Empty));

        }
    }
    private void LoadDDLDetails()
    {
        if (!string.IsNullOrEmpty(ddlDetailsType.SelectedValue))
        {
            string sql = string.Format(@"
select distinct t.ParticularRemarks,t.AccountID from
(
select j.AccountID,j.Particular,j.Remarks
,case
when j.Remarks='Supplier' then (select top 1 (v.Vendor_Name+'_'+v.Vendor_mobile) 
from Vendor v where v.OID=j.AccountID)

when j.Remarks='Customer' then (select top 1 (c.Name+'_'+CONVERT(nvarchar(150),c.Number)) 
from Customers c where c.ID=j.AccountID)

when j.Remarks='Expense' then (select top 1 (cs.ExpenseType+'_'+cs.CostingHead) 
from CostingHead cs where cs.OID=j.AccountID
)
when j.Remarks='Bank' then (select top 1 (BI.BankName+'_'+BI.AccountNo) 
from BankInfo BI where BI.OID=j.AccountID
)
when j.Remarks='Bank(Card)' then (select top 1 (BI.BankName+'_'+BI.AccountNo) 
from BankInfo BI where BI.OID=j.AccountID
)
when j.Remarks='Product' then (select top 1 (d.Description) 
from Description d where d.OID=j.AccountID)

end as ParticularRemarks
from Acc_Journal j 
where j.Particular = '{0}' and j.Branch='{1}'
)t
where t.ParticularRemarks is not null
order by t.ParticularRemarks
", ddlDetailsType.SelectedValue, Session["StoreID"].ToString());
            DataTable dt = DAL.LoadDataByQuery(sql);
            if (dt.Rows.Count > 0)
            {
                ddlDetails.DataSource = dt;
                ddlDetails.DataValueField = "AccountID";
                ddlDetails.DataTextField = "ParticularRemarks";
                ddlDetails.DataBind();

                ddlDetails.Items.Insert(0, new ListItem("-- Select One--", String.Empty));
            }
            else
            {
                ddlDetails.Items.Clear();
            }
        }
    }

    protected void gvLedger_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
    {
        string sino = ((System.Web.UI.WebControls.Label)gvLedger.Rows[e.NewEditIndex].FindControl("lblPurchaseNo")).Text;

    }


    protected void gvLedger_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
    {
        gvLedger.PageIndex = e.NewPageIndex;
        LoadGridJournal();
    }

    protected void btnSearchPurchaseInvoice_Click(object sender, EventArgs e)
    {
        LoadGridJournal();
    }

    private void LoadGridJournal()
    {
//        string strParam = string.Empty;

//        var fromdate = Convert.ToDateTime(dptFromDate.Text).ToString("yyyy-MM-dd");
//        var todate = Convert.ToDateTime(dptToDate.Text).ToString("yyyy-MM-dd");   //and CONVERT(date,j.IDATTIME)

//        if (!string.IsNullOrEmpty(dptFromDate.Text) && !string.IsNullOrEmpty(dptToDate.Text))
//        {
//            strParam += string.Format(@" and CONVERT(date, j.IDATTIME) between '{0}' and '{1}'", fromdate, todate);
//        }
//        if (!string.IsNullOrEmpty(ddlDetails.SelectedValue))
//        {
//            strParam += string.Format(@" and j.AccountID={0}", ddlDetails.SelectedValue);
//        }
//        if (!string.IsNullOrEmpty(ddlDetailsType.SelectedValue))
//        {
//            strParam += string.Format(@" and j.Particular='{0}'", ddlDetailsType.SelectedValue);
//        }


//        string sql = string.Format(@"
//select j.Particular,CONVERT(nvarchar(12),j.IDATTIME,106)EntryDate,j.AccountID
//,case 
//when j.Remarks='Product' 
//then j.Narration + ' P: ' +
//(select top 1 ds.Description from Description ds where ds.OID=j.AccountID)
//
//when j.Remarks='Supplier' 
//then j.Narration + ' S: ' +
//(select top 1 v.Vendor_Name from Vendor v where v.OID=j.AccountID)
//
//else j.Narration
//
//end as Narration
//,j.RefferenceNumber
//, j.Debit,j.Credit,Balance=(j.Debit-j.Credit)
//from Acc_Journal j
//where j.Branch='{1}'
// {0}
//", strParam, Session["StoreID"].ToString());
        string strParam = string.Empty;

        //var fromdate = Convert.ToDateTime(dptFromDate.Text).ToString("yyyy-MM-dd");
        //var todate = Convert.ToDateTime(dptToDate.Text).ToString("yyyy-MM-dd");   //and CONVERT(date,j.IDATTIME)

        //if (!string.IsNullOrEmpty(dptFromDate.Text) && !string.IsNullOrEmpty(dptToDate.Text))
        //{
        //    strParam += string.Format(@" and CONVERT(date, j.IDATTIME) between '{0}' and '{1}'", fromdate, todate);
        //}
        //if (!string.IsNullOrEmpty(ddlDetails.SelectedValue))
        //{
        //    strParam += string.Format(@" and j.AccountID={0}", ddlDetails.SelectedValue);
        //}
        //if (!string.IsNullOrEmpty(ddlDetailsType.SelectedValue))
        //{
        //    strParam += string.Format(@" and j.Particular='{0}'", ddlDetailsType.SelectedValue);
        //}

        string fromDate = dptFromDate.Text == "" ? DateTime.Now.ToString("dd-MMM-yyyy") : Convert.ToDateTime(dptFromDate.Text).ToString("dd-MMM-yyyy");
        string toDate = dptToDate.Text == "" ? DateTime.Now.ToString("dd-MMM-yyyy") : Convert.ToDateTime(dptToDate.Text).ToString("dd-MMM-yyyy");
        string accName = ddlDetails.SelectedIndex <= 0 ? "" : ddlDetails.SelectedItem.Value;
        string accType= ddlDetailsType.Items.Count == 0 ? "" : (ddlDetailsType.SelectedIndex <= 0 ? "" : ddlDetailsType.SelectedItem.Text);


        //        string sql = string.Format(@"
        //select Row_Number() Over(Order by Journal_OID asc) 'SLNo',j.Particular,CONVERT(nvarchar(12),j.IDATTIME,106)EntryDate,j.AccountID
        //,case 
        //when j.Remarks='Product' 
        //then j.Narration + ' P: ' +
        //(select top 1 ds.Description from Description ds where ds.OID=j.AccountID)
        //
        //when j.Remarks='Supplier' 
        //then j.Narration + ' S: ' +
        //(select top 1 v.Vendor_Name from Vendor v where v.OID=j.AccountID)
        //
        //else j.Narration
        //
        //end as Narration
        //,j.RefferenceNumber
        //, j.Debit,j.Credit,Balance=(j.Debit-j.Credit),(Select Sum(Debit-Credit) From Acc_Journal Where Branch='{1}' {0}) 'TotalBalance',S.ShopName,S.CCOM_ADD1,S.ShopMobile,S.emailweb 'Email','{2}' as FromDate,'{3}' as ToDate,'{4}' as AccountType,'{5}' as AccountName
        //from Acc_Journal j
        //Left Join ShopInfo S On S.OID = j.Branch
        //where j.Branch='{1}' {0}
        //", strParam, Session["StoreID"].ToString(),fromdate,todate,accType,accName);

        sql = string.Format(@"Exec spGetAccountsLedger @ParticularType='{0}',@ParticularName='{1}',@FromDate='{2}',@ToDate='{3}',@Branch='{4}'", accType, accName, fromDate, toDate, Session["StoreID"].ToString());
        DataTable dt = DAL.LoadDataByQuery(sql);
        if (dt.Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(ddlDetailsType.SelectedValue))
            {
                lblDetailsHead.Text = ddlDetailsType.SelectedItem.ToString();
            }
            else if (!string.IsNullOrEmpty(ddlDetails.SelectedValue))
            {
                lblDetailsHead.Text = ddlDetails.SelectedItem.ToString();
            }

            gvLedger.DataSource = null;
            gvLedger.DataSource = dt;
            gvLedger.DataBind();

            //get total
            decimal totalDr = 0; decimal totalCr = 0; decimal totalBalance = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                totalDr += Convert.ToDecimal(dt.Rows[i]["Debit"].ToString());
                totalCr += Convert.ToDecimal(dt.Rows[i]["Credit"].ToString());
                //totalBalance += Convert.ToDecimal(dt.Rows[i]["Balance"].ToString());
               
            }
            totalBalance = (totalDr - totalCr);
            ((Label)gvLedger.FooterRow.FindControl("lblTotalDrAmount")).Text = totalDr.ToString("0.00");
            ((Label)gvLedger.FooterRow.FindControl("lblTotalCrAmount")).Text = totalCr.ToString("0.00");
            ((Label)gvLedger.FooterRow.FindControl("lblTotalBalance")).Text = totalBalance.ToString("0.00");
        }
        else
        {
            lblDetailsHead.Text = string.Empty;

            gvLedger.DataSource = null;
            gvLedger.DataBind();

        }
    }
    protected void ddlDetailsType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(ddlDetailsType.SelectedValue))
        {
            LoadDDLDetails();
        }
        else
        {
            ddlDetails.Items.Clear();
        }
    }


    protected void txtDetails_TextChanged(object sender, EventArgs e)
    {


    }

    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetExpenseHeadEID(string prefixText, int count)
    {
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = string.Format(@"
select distinct j.DetailsID,j.Details from AccJournal j 
order by j.Details");
                cmd.Connection = conn;
                conn.Open();
                List<string> customers = new List<string>();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        customers.Add(sdr["Details"].ToString());
                    }
                }
                conn.Close();
                return customers;
            }
        }
    }

    protected void btnLedgerReport_Click(object sender, EventArgs e)
    {
        string strParam = string.Empty;

        //var fromdate = Convert.ToDateTime(dptFromDate.Text).ToString("yyyy-MM-dd");
        //var todate = Convert.ToDateTime(dptToDate.Text).ToString("yyyy-MM-dd");   //and CONVERT(date,j.IDATTIME)

        //if (!string.IsNullOrEmpty(dptFromDate.Text) && !string.IsNullOrEmpty(dptToDate.Text))
        //{
        //    strParam += string.Format(@" and CONVERT(date, j.IDATTIME) between '{0}' and '{1}'", fromdate, todate);
        //}
        //if (!string.IsNullOrEmpty(ddlDetails.SelectedValue))
        //{
        //    strParam += string.Format(@" and j.AccountID={0}", ddlDetails.SelectedValue);
        //}
        //if (!string.IsNullOrEmpty(ddlDetailsType.SelectedValue))
        //{
        //    strParam += string.Format(@" and j.Particular='{0}'", ddlDetailsType.SelectedValue);
        //}

        string fromDate = dptFromDate.Text == "" ? DateTime.Now.ToString("dd-MMM-yyyy") : Convert.ToDateTime(dptFromDate.Text).ToString("dd-MMM-yyyy");
        string toDate = dptToDate.Text == "" ? DateTime.Now.ToString("dd-MMM-yyyy") : Convert.ToDateTime(dptToDate.Text).ToString("dd-MMM-yyyy");
        string accName = ddlDetails.SelectedIndex <= 0 ? "" : ddlDetails.SelectedItem.Value;
        string accType = ddlDetailsType.Items.Count == 0 ? "" : (ddlDetailsType.SelectedIndex <= 0 ? "" : ddlDetailsType.SelectedItem.Text);

//        string sql = string.Format(@"
//select Row_Number() Over(Order by Journal_OID asc) 'SLNo',j.Particular,CONVERT(nvarchar(12),j.IDATTIME,106)EntryDate,j.AccountID
//,case 
//when j.Remarks='Product' 
//then j.Narration + ' P: ' +
//(select top 1 ds.Description from Description ds where ds.OID=j.AccountID)
//
//when j.Remarks='Supplier' 
//then j.Narration + ' S: ' +
//(select top 1 v.Vendor_Name from Vendor v where v.OID=j.AccountID)
//
//else j.Narration
//
//end as Narration
//,j.RefferenceNumber
//, j.Debit,j.Credit,Balance=(j.Debit-j.Credit),(Select Sum(Debit-Credit) From Acc_Journal Where Branch='{1}' {0}) 'TotalBalance',S.ShopName,S.CCOM_ADD1,S.ShopMobile,S.emailweb 'Email','{2}' as FromDate,'{3}' as ToDate,'{4}' as AccountType,'{5}' as AccountName
//from Acc_Journal j
//Left Join ShopInfo S On S.OID = j.Branch
//where j.Branch='{1}' {0}
//", strParam, Session["StoreID"].ToString(),fromdate,todate,accType,accName);

        sql = string.Format(@"Exec spGetAccountsLedger @ParticularType='{0}',@ParticularName='{1}',@FromDate='{2}',@ToDate='{3}',@Branch='{4}'", accType, accName, fromDate, toDate, Session["StoreID"].ToString());

        DataTable dt = DAL.LoadDataByQuery(sql);
        if (dt.Rows.Count > 0) 
        { 
            // Report Works....
            Session["dtsales"] = dt;
            Session["ReportPath"] = "~/Reports/rptAccountLedger.rpt";

            string webUrl = "../Reports/ReportView.aspx";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenWindow", "window.open('" + webUrl + "','_newtab');", true);
        }
    }
}