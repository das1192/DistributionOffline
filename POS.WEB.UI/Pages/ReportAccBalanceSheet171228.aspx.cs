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


public partial class Pages_ReportAccBalanceSheet171228 : System.Web.UI.Page
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
            //LoadDDL();

            DateTime date = DateTime.Now;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            dptFromDate.Text = firstDayOfMonth.ToString("dd MMM yyyy");
            dptToDate.Text = lastDayOfMonth.ToString("dd MMM yyyy");
        }

    }

    //private void LoadDDL()
    //{
    //    LoadDDLDetailsType();
    //}
    //private void LoadDDLDetailsType()
    //{
    //    string sql = string.Format(@"select distinct j.DetailsType as id,j.DetailsType as text from AccJournal j order by j.DetailsType");
    //    DataTable dt = DAL.LoadDataByQuery(sql);

    //    if (dt.Rows.Count > 0)
    //    {
    //        ddlDetailsType.DataSource = dt;
    //        ddlDetailsType.DataValueField = "id";
    //        ddlDetailsType.DataTextField = "text";
    //        ddlDetailsType.DataBind();

    //        ddlDetailsType.Items.Insert(0, new ListItem("-- Select One--", String.Empty));

    //    }
    //}
    //private void LoadDDLDetails()
    //{
    //    if (!string.IsNullOrEmpty(ddlDetailsType.SelectedValue))
    //    {
    //        string sql = string.Format(@"select distinct j.DetailsID,j.Details from AccJournal j where j.DetailsType = '{0}' order by j.Details", ddlDetailsType.SelectedValue);
    //        DataTable dt = DAL.LoadDataByQuery(sql);
    //        if (dt.Rows.Count > 0)
    //        {
    //            ddlDetails.DataSource = dt;
    //            ddlDetails.DataValueField = "DetailsID";
    //            ddlDetails.DataTextField = "Details";
    //            ddlDetails.DataBind();

    //            ddlDetails.Items.Insert(0, new ListItem("-- Select One--", String.Empty));
    //        }
    //        else
    //        {
    //            ddlDetails.Items.Clear();
    //        }
    //    }
    //}

    protected void gvLedger_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
    {
        string sino = ((System.Web.UI.WebControls.Label)gvLedger.Rows[e.NewEditIndex].FindControl("lblPurchaseNo")).Text;

    }


    protected void gvLedger_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
    {
        gvLedger.PageIndex = e.NewPageIndex;
        LoadGridJournal();
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        LoadData();
        //LoadGridJournal();
    }

    private void LoadData()
    {

        string strParam = string.Format(@" and CONVERT(date, j.EntryDate) between '{0}' and '{1}'", dptFromDate.Text, dptToDate.Text);
        string fromDate = dptFromDate.Text;
        string toDate = dptToDate.Text;


        string sql = string.Format(@"
select tt.OpeningStockValue,
tt.ClosingStockValue,tt.Purchasevalue ,tt.NetSale ,tt.DirectExpense ,tt.IndirectExpense 
,GrossProfit = (tt.NetSale -tt.OpeningStockValue + tt.Purchasevalue -tt.ClosingStockValue -tt.DirectExpense )

,NetProfit = (tt.NetSale -tt.OpeningStockValue + tt.Purchasevalue -tt.ClosingStockValue -tt.DirectExpense-tt.IndirectExpense  )

 from (

------------------------------------------1
select 
OpeningStockValue = (t.TillYesterdayP_Cost-t.TillYesterdayS_Cost-t.TillYesterdayPR_Cost+t.TillYesterdaySR_Cost),
ClosingStockValue = (t.TillYesterdayP_Cost-t.TillYesterdayS_Cost-t.TillYesterdayPR_Cost+t.TillYesterdaySR_Cost+t.TilltodayP_Cost-t.TilltodayS_Cost+t.TilltodaySR_Cost-t.TilltodayPR_Cost),
Purchasevalue = (t.TilltodayP_Cost-t.TilltodayPR_Cost),
NetSale = (t.TilltodayS_Value-t.TilltodaySR_Value)
,t.DirectExpense,t.IndirectExpense


from 

(


--------------------------------------0
select 
TillYesterdayP_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Qty*sd.UnitCost),0)) from StockDetails sd 
where sd.Flag='purchase' and CONVERT(date,sd.EntryDate)< '{0}')

,TillYesterdayPR_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Qty*sd.UnitCost),0)) from StockDetails sd 
where sd.Flag='purchase return' and CONVERT(date,sd.EntryDate)< '{0}')

,TillYesterdayS_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Qty*sd.UnitCost),0)) from StockDetails sd 
where sd.Flag='sale' and CONVERT(date,sd.EntryDate)< '{0}')

,TillYesterdaySR_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Qty*sd.UnitCost),0)) from StockDetails sd 
where sd.Flag='sale return' and CONVERT(date,sd.EntryDate)< '{0}')


,TilltodayP_Cost=
(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Qty*sd.UnitCost),0)) from StockDetails sd 
where sd.Flag='purchase' and CONVERT(date,sd.EntryDate) between '{0}' AND '{1}')

,TilltodayPR_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Qty*sd.UnitCost),0)) from StockDetails sd 
where sd.Flag='purchase return' and CONVERT(date,sd.EntryDate) between '{0}' AND '{1}')

,TilltodayS_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Qty*sd.UnitCost),0)) from StockDetails sd 
where sd.Flag='sale' and CONVERT(date,sd.EntryDate) between '{0}' AND '{1}')

,TilltodaySR_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Qty*sd.UnitCost),0)) from StockDetails sd 
where sd.Flag='sale return' and CONVERT(date,sd.EntryDate) between '{0}' AND '{1}')

,TilltodayS_Value=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Qty*sd.UnitSale ),0)) from StockDetails sd 
where sd.Flag='sale' and CONVERT(date,sd.EntryDate) between '{0}' AND '{1}')

,TilltodaySR_Value=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Qty*sd.UnitSale ),0)) from StockDetails sd 
where sd.Flag='sale return' and CONVERT(date,sd.EntryDate) between '{0}' AND '{1}')

-----------------------expense
,DirectExpense =(
select ISNULL(SUM(e.ExpenseAmount),0) totalExpenseAmount
from AccExpense e inner join AccExpenseHead eh on eh.ExpenseHeadID=e.ExpenseHeadID

where eh.ExpenseType='Direct'  and CONVERT(date,e.EntryDate) between '{0}' AND '{1}'
)
,IndirectExpense =(
select ISNULL(SUM(e.ExpenseAmount),0) totalExpenseAmount
from AccExpense e inner join AccExpenseHead eh on eh.ExpenseHeadID=e.ExpenseHeadID

where eh.ExpenseType='Indirect'  and CONVERT(date,e.EntryDate) between '{0}' AND '{1}'
)
-------------------------------------0

) t
-------------------------------------1


) tt
", fromDate,toDate);
        DataTable dt = DAL.LoadDataByQuery(sql);
        if (dt.Rows.Count > 0)
        {
            lblOpeningStockValue.Text = dt.Rows[0]["OpeningStockValue"].ToString();
            lblPurchaseValue.Text = dt.Rows[0]["Purchasevalue"].ToString();
            lblDirectExpense.Text = dt.Rows[0]["DirectExpense"].ToString();
            lblNetSale.Text = dt.Rows[0]["NetSale"].ToString();
            lblClosingStock.Text = dt.Rows[0]["ClosingStockValue"].ToString();
            lblIndirectExpense.Text = dt.Rows[0]["IndirectExpense"].ToString();

            decimal GrossProfit = 0;
            decimal NetProfit = 0;
            if (decimal.TryParse(dt.Rows[0]["GrossProfit"].ToString(),out GrossProfit)) { }
            if (decimal.TryParse(dt.Rows[0]["NetProfit"].ToString(), out NetProfit)) { }
            
            if(GrossProfit>0)
            {
                lblGrossProfitLabel.Text = "Gross Profit";
                lblGrossProfit.Text = GrossProfit.ToString("0.00");
            }
            else
            {
                lblGrossLossLabel.Text = "Gross Loss";
                lblGrossLoss.Text = GrossProfit.ToString("0.00");
            }
            if (NetProfit > 0)
            {
                lblNetProfitLabel.Text = "Net Profit";
                lblNetProfit.Text = NetProfit.ToString("0.00");
            }
            else
            {
                lblNetLossLabel.Text = "Net Loss";
                lblNetLoss.Text = NetProfit.ToString("0.00");
            }

            //lblGrossProfit.Text = dt.Rows[0]["GrossProfit"].ToString();
            //lblGrossProfit.Text = dt.Rows[0]["ClosingStockValue"].ToString();
//OpeningStockValue	ClosingStockValue	Purchasevalue	NetSale	DirectExpense	IndirectExpense	GrossProfit	NetProfit
        }

    }



    private void LoadGridJournal()
    {
        string strParam = string.Empty;

        if (!string.IsNullOrEmpty(dptFromDate.Text) && !string.IsNullOrEmpty(dptToDate.Text))
        {
            strParam += string.Format(@" and CONVERT(date, j.EntryDate) between '{0}' and '{1}'", dptFromDate.Text, dptToDate.Text);
        }


        string sql = string.Format(@"
select j.Details,CONVERT(nvarchar(12),j.EntryDate,106)EntryDate,j.DetailsID,j.Narration,j.JournalID,j.JournalCode
, j.DrAmount,j.CrAmount,Balance=(j.DrAmount-j.CrAmount)
from AccJournal j
inner join [User] u on u.Id=j.EntryUserID
where 1=1
 {0}
", strParam);
        DataTable dt = DAL.LoadDataByQuery(sql);
        if (dt.Rows.Count > 0)
        {

            gvLedger.DataSource = null;
            gvLedger.DataSource = dt;
            gvLedger.DataBind();

            //get total
            decimal totalDr = 0; decimal totalCr = 0; decimal totalBalance = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                totalDr += Convert.ToDecimal(dt.Rows[i]["DrAmount"].ToString());
                totalCr += Convert.ToDecimal(dt.Rows[i]["CrAmount"].ToString());
                totalBalance += Convert.ToDecimal(dt.Rows[i]["Balance"].ToString());
            }
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
order by j.Details
");
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

    string sqlq = string.Format(@"

select tt.OpeningStockValue,
tt.ClosingStockValue,tt.Purchasevalue ,tt.NetSale ,tt.DirectExpense ,tt.IndirectExpense 
,GrossProfit = (tt.NetSale -tt.OpeningStockValue + tt.Purchasevalue -tt.ClosingStockValue -tt.DirectExpense )

,NetProfit = (tt.NetSale -tt.OpeningStockValue + tt.Purchasevalue -tt.ClosingStockValue -tt.DirectExpense-tt.IndirectExpense  )

 from (

------------------------------------------1
select 
OpeningStockValue = (t.TillYesterdayP_Cost-t.TillYesterdayS_Cost-t.TillYesterdayPR_Cost+t.TillYesterdaySR_Cost),
ClosingStockValue = (t.TillYesterdayP_Cost-t.TillYesterdayS_Cost-t.TillYesterdayPR_Cost+t.TillYesterdaySR_Cost+t.TilltodayP_Cost-t.TilltodayS_Cost+t.TilltodaySR_Cost-t.TilltodayPR_Cost),
Purchasevalue = (t.TilltodayP_Cost-t.TilltodayPR_Cost),
NetSale = (t.TilltodayS_Value-t.TilltodaySR_Value)
,t.DirectExpense,t.IndirectExpense


from 

(


--------------------------------------0
select 
TillYesterdayP_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Qty*sd.UnitCost),0)) from StockDetails sd 
where sd.Flag='purchase' and CONVERT(date,sd.EntryDate)< '01-Nov-2017')

,TillYesterdayPR_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Qty*sd.UnitCost),0)) from StockDetails sd 
where sd.Flag='purchase return' and CONVERT(date,sd.EntryDate)< '01-Nov-2017')

,TillYesterdayS_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Qty*sd.UnitCost),0)) from StockDetails sd 
where sd.Flag='sale' and CONVERT(date,sd.EntryDate)< '01-Nov-2017')

,TillYesterdaySR_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Qty*sd.UnitCost),0)) from StockDetails sd 
where sd.Flag='sale return' and CONVERT(date,sd.EntryDate)< '01-Nov-2017')


,TilltodayP_Cost=
(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Qty*sd.UnitCost),0)) from StockDetails sd 
where sd.Flag='purchase' and CONVERT(date,sd.EntryDate) between '01-Nov-2017' AND '20-Nov-2017')

,TilltodayPR_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Qty*sd.UnitCost),0)) from StockDetails sd 
where sd.Flag='purchase return' and CONVERT(date,sd.EntryDate) between '01-Nov-2017' AND '20-Nov-2017')

,TilltodayS_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Qty*sd.UnitCost),0)) from StockDetails sd 
where sd.Flag='sale' and CONVERT(date,sd.EntryDate) between '01-Nov-2017' AND '20-Nov-2017')

,TilltodaySR_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Qty*sd.UnitCost),0)) from StockDetails sd 
where sd.Flag='sale return' and CONVERT(date,sd.EntryDate) between '01-Nov-2017' AND '20-Nov-2017')

,TilltodayS_Value=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Qty*sd.UnitSale ),0)) from StockDetails sd 
where sd.Flag='sale' and CONVERT(date,sd.EntryDate) between '01-Nov-2017' AND '20-Nov-2017')

,TilltodaySR_Value=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Qty*sd.UnitSale ),0)) from StockDetails sd 
where sd.Flag='sale return' and CONVERT(date,sd.EntryDate) between '01-Nov-2017' AND '20-Nov-2017')

-----------------------expense
,DirectExpense =(
select ISNULL(SUM(e.ExpenseAmount),0) totalExpenseAmount
from AccExpense e inner join AccExpenseHead eh on eh.ExpenseHeadID=e.ExpenseHeadID

where eh.ExpenseType='Direct'  and CONVERT(date,e.EntryDate) between '01-Nov-2017' AND '20-Nov-2017'
)
,IndirectExpense =(
select ISNULL(SUM(e.ExpenseAmount),0) totalExpenseAmount
from AccExpense e inner join AccExpenseHead eh on eh.ExpenseHeadID=e.ExpenseHeadID

where eh.ExpenseType='Indirect'  and CONVERT(date,e.EntryDate) between '01-Nov-2017' AND '20-Nov-2017'
)
-------------------------------------0

) t
-------------------------------------1


) tt

");


}