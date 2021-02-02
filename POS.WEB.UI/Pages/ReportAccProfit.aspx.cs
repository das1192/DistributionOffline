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


public partial class Pages_ReportAccProfit : System.Web.UI.Page
{
    private string userID = string.Empty;
    private string userPassword = string.Empty;
    private string Branch = string.Empty;
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
            Branch = Session["StoreID"].ToString();
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
            dptToDate.Text = date.ToString("dd MMM yyyy");
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

    protected void btnShow_Click(object sender, EventArgs e)
    {
        LoadData();
        //LoadGridJournal();
    }

    private void LoadData()
    {
        lblOpeningStockValue.Text = string.Empty;
        lblPurchaseValue.Text = string.Empty;
        lblDirectExpense.Text = string.Empty;
        lblNetSale.Text = string.Empty;
        lblCommission.Text = string.Empty;
        lblClosingStock.Text = string.Empty;
        lblIndirectExpense.Text = string.Empty;


        lblGrossProfitLabel.Text = string.Empty;
        lblGrossProfit.Text = string.Empty;

        lblGrossLossLabel.Text = string.Empty;
        lblGrossLoss.Text = string.Empty;

        lblNetProfitLabel.Text = string.Empty;
        lblNetProfit.Text = string.Empty;

        lblNetLossLabel.Text = string.Empty;
        lblNetLoss.Text = string.Empty;

        var fromdate = Convert.ToDateTime(dptFromDate.Text).ToString("yyyy-MM-dd");
        var todate = Convert.ToDateTime(dptToDate.Text).ToString("yyyy-MM-dd");   //and CONVERT(date,j.IDATTIME)

        string strParam = string.Format(@" and CONVERT(date, j.EntryDate) between '{0}' and '{1}'", fromdate, todate);
        string fromDate = dptFromDate.Text;
        string toDate = dptToDate.Text;

        #region v0
        //        string sql = string.Format(@"
//
//select tt.OpeningStockValue,
//tt.ClosingStockValue,tt.Purchasevalue ,tt.NetSale ,tt.DirectExpense ,tt.IndirectExpense ,tt.TilltodayPRPROT_Value as Income
//,GrossProfit = (tt.NetSale + tt.TilltodayPRPROT_Value  -(tt.OpeningStockValue + tt.Purchasevalue -tt.ClosingStockValue) -tt.DirectExpense )
//,NetProfit = (tt.NetSale + tt.TilltodayPRPROT_Value -(tt.OpeningStockValue + tt.Purchasevalue -tt.ClosingStockValue) -tt.DirectExpense-tt.IndirectExpense  )
//
// from (
//
//------------------------------------------1
//select 
//OpeningStockValue = (t.TillYesterdayP_Cost-t.TillYesterdayS_Cost-t.TillYesterdayPR_Cost+t.TillYesterdaySR_Cost-t.TillYesterdayPRPROT_Cost ),
//ClosingStockValue = (t.TillYesterdayP_Cost-t.TillYesterdayS_Cost-t.TillYesterdayPR_Cost+t.TillYesterdaySR_Cost+t.TilltodayP_Cost-t.TilltodayS_Cost+t.TilltodaySR_Cost-t.TilltodayPR_Cost-t.TilltodayPRPROT_Value-t.TillYesterdayPRPROT_Cost),
//Purchasevalue = (t.TilltodayP_Cost-t.TilltodayPR_Cost),
//NetSale = (t.TilltodayS_Value-t.TilltodaySR_Value)
//,t.DirectExpense,t.IndirectExpense,t.TilltodayPRPROT_Value 
//
//
//from 
//
//(
//
//
//--------------------------------------0
//select 
//TillYesterdayP_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.CostPrice),0)) from Acc_StockDetail sd 
//where sd.Flag='purchase' and CONVERT(date,sd.IDATTIME)< '{0}')
//
//,TillYesterdayPR_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.CostPrice),0)) from Acc_StockDetail sd 
//where sd.Flag='purchase return' and CONVERT(date,sd.IDATTIME)< '{0}')
//
//,TillYesterdayS_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.CostPrice),0)) from Acc_StockDetail sd 
//where sd.Flag='sale' and CONVERT(date,sd.IDATTIME)< '{0}')
//
//,TillYesterdaySR_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.CostPrice),0)) from Acc_StockDetail sd 
//where sd.Flag='sale return' and CONVERT(date,sd.IDATTIME)< '{0}')
//
//
//,TillYesterdayPRPROT_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.CostPrice),0)) from Acc_StockDetail sd 
//where sd.Flag='Price Adjustment' and CONVERT(date,sd.IDATTIME)< '{0}')
//
//,TilltodayP_Cost=
//(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.CostPrice),0)) from Acc_StockDetail sd 
//where sd.Flag='purchase' and CONVERT(date,sd.IDATTIME) between '{0}' AND '{1}')
//
//,TilltodayPR_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.CostPrice),0)) from Acc_StockDetail sd 
//where sd.Flag='purchase return' and CONVERT(date,sd.IDATTIME) between '{0}' AND '{1}')
//
//,TilltodayS_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.CostPrice),0)) from Acc_StockDetail sd 
//where sd.Flag='sale' and CONVERT(date,sd.IDATTIME) between '{0}' AND '{1}')
//
//,TilltodaySR_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.CostPrice),0)) from Acc_StockDetail sd 
//where sd.Flag='sale return' and CONVERT(date,sd.IDATTIME) between '{0}' AND '{1}')
//
//,TilltodayS_Value=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.SalePrice ),0)) from Acc_StockDetail sd 
//where sd.Flag='sale' and CONVERT(date,sd.IDATTIME) between '{0}' AND '{1}')
//
//,TilltodaySR_Value=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.SalePrice ),0)) from Acc_StockDetail sd 
//where sd.Flag='sale return' and CONVERT(date,sd.IDATTIME) between '{0}' AND '{1}')
//
//
//,TilltodayPRPROT_Value=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.CostPrice ),0)) from Acc_StockDetail sd 
//where sd.Flag='Price Adjustment' and CONVERT(date,sd.IDATTIME) between '{0}' AND '{1}')
//
//-----------------------expense
//,DirectExpense =(
//select ISNULL(SUM(dc.AMOUNT),0) as total
//from DailyCost dc  
//inner join CostingHead ch on ch.OID=dc.CostingHeadID
//where ch .ExpenseType='Direct' and CONVERT(date,dc.IDAT) between '{0}' AND '{1}'
//)
//
//,IndirectExpense =(
//select ISNULL(SUM(dc.AMOUNT),0) as total
//from DailyCost dc  
//inner join CostingHead ch on ch.OID=dc.CostingHeadID
//where ch .ExpenseType='Indirect' and CONVERT(date,dc.IDAT) between '{0}' AND '{1}'
//)
//
//-------------------------------------0
//
//) t
//-------------------------------------1
//
//
//) tt
        //", fromDate, toDate);
        #endregion
        string sql = string.Format(@"
select tt.OpeningStockValue,
tt.ClosingStockValue,tt.Purchasevalue ,tt.NetSale ,tt.DirectExpense ,tt.IndirectExpense ,tt.TilltodayPRPROT_Value as Income
,GrossProfit = (tt.NetSale +tt.Comi+ tt.TilltodayPRPROT_Value  -(tt.OpeningStockValue + tt.Purchasevalue -tt.ClosingStockValue) -tt.DirectExpense )
,NetProfit = (tt.NetSale + tt.Comi + tt.TilltodayPRPROT_Value -(tt.OpeningStockValue + tt.Purchasevalue -tt.ClosingStockValue) -tt.DirectExpense-tt.IndirectExpense  )
,tt.Comi
 from (

------------------------------------------1
select 
OpeningStockValue = (t.TillYesterdayP_Cost-t.TillYesterdayS_Cost-t.TillYesterdayPR_Cost+t.TillYesterdaySR_Cost-t.TillYesterdayPRPROT_Cost -t.TillYesterdayPRMIS_Cost),
ClosingStockValue = (t.TillYesterdayP_Cost-t.TillYesterdayS_Cost-t.TillYesterdayPR_Cost+t.TillYesterdaySR_Cost+t.TilltodayP_Cost-t.TilltodayS_Cost+t.TilltodaySR_Cost-t.TilltodayPR_Cost-t.TilltodayPRPROT_Value-t.TillYesterdayPRPROT_Cost-t.TillYesterdayPRMIS_Cost-t.TilltodayPRMIS_Value),
Purchasevalue = (t.TilltodayP_Cost-t.TilltodayPR_Cost-t.TilltodayPRMIS_Value),
NetSale = (t.TilltodayS_Value-t.TilltodaySR_Value)

---newly edited for decimal
,Comi=convert(decimal(18,2),t.Comi)
,DirectExpense=convert(decimal(18,2),t.DirectExpense)
,IndirectExpense=convert(decimal(18,2),t.IndirectExpense)
,t.TilltodayPRPROT_Value 



from 

(


--------------------------------------0
select 
TillYesterdayP_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.CostPrice),0)) from Acc_StockDetail sd 
where sd.Flag='purchase' and sd.Branch='{0}' and CONVERT(date,sd.IDATTIME)< '{1}')

----comission
,Comi = (select ISNULL(Sum(aj.Credit),0) from Acc_Journal aj   where aj.Particular='Commission' and aj.Branch = '{0}' and CONVERT(date, aj.IDATTIME) between '{1}' AND '{2}')

,TillYesterdayPR_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.CostPrice),0)) from Acc_StockDetail sd 
where sd.Flag='purchase return' and sd.Branch='{0}' and CONVERT(date,sd.IDATTIME)< '{1}')

,TillYesterdayS_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.CostPrice),0)) from Acc_StockDetail sd 
where sd.Flag='sale' and sd.Branch='{0}' and CONVERT(date,sd.IDATTIME)< '{1}')

,TillYesterdaySR_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.CostPrice),0)) from Acc_StockDetail sd 
where sd.Flag='sale return' and sd.Branch='{0}' and CONVERT(date,sd.IDATTIME)< '{1}')


,TillYesterdayPRPROT_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.CostPrice),0)) from Acc_StockDetail sd 
where sd.Flag='Price Protection' and sd.Branch='{0}' and CONVERT(date,sd.IDATTIME)< '{1}')

,TillYesterdayPRMIS_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.CostPrice),0)) from Acc_StockDetail sd 
where sd.Flag='Product Missing' and sd.Branch='{0}' and CONVERT(date,sd.IDATTIME)< '{1}')



,TilltodayP_Cost=
(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.CostPrice),0)) from Acc_StockDetail sd 
where sd.Flag='purchase' and sd.Branch='{0}' and CONVERT(date,sd.IDATTIME) between '{1}' AND '{2}')

,TilltodayPR_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.CostPrice),0)) from Acc_StockDetail sd 
where sd.Flag='purchase return' and sd.Branch='{0}' and CONVERT(date,sd.IDATTIME) between '{1}' AND '{2}')

,TilltodayS_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.CostPrice),0)) from Acc_StockDetail sd 
where sd.Flag='sale' and sd.Branch='{0}' and CONVERT(date,sd.IDATTIME) between '{1}' AND '{2}')

,TilltodaySR_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.CostPrice),0)) from Acc_StockDetail sd 
where sd.Flag='sale return' and sd.Branch='{0}' and CONVERT(date,sd.IDATTIME) between '{1}' AND '{2}')

,TilltodayS_Value=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.SalePrice ),0)) from Acc_StockDetail sd 
where sd.Flag='sale' and sd.Branch='{0}' and CONVERT(date,sd.IDATTIME) between '{1}' AND '{2}')

,TilltodaySR_Value=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.SalePrice ),0)) from Acc_StockDetail sd 
where sd.Flag='sale return' and sd.Branch='{0}' and CONVERT(date,sd.IDATTIME) between '{1}' AND '{2}')


,TilltodayPRPROT_Value=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.CostPrice ),0)) from Acc_StockDetail sd 
where sd.Flag='Price Protection' and sd.Branch='{0}' and CONVERT(date,sd.IDATTIME) between '{1}' AND '{2}')

,TilltodayPRMIS_Value=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.CostPrice ),0)) from Acc_StockDetail sd 
where sd.Flag='Product Missing' and sd.Branch='{0}' and CONVERT(date,sd.IDATTIME) between '{1}' AND '{2}')


-----------------------expense
,DirectExpense =(
select ISNULL(SUM(dc.Balance),0) as total
from DirectExpenseView dc  

where dc.Branch='{0}' and CONVERT(date,dc.IDAT) between '{1}' AND '{2}'
)

,IndirectExpense =(
select ISNULL(SUM(dc.Balance),0) as total
from IndiectExpenseView dc  

where dc.Branch='{0}' and CONVERT(date,dc.IDAT) between '{1}' AND '{2}'
)

-------------------------------------0

) t
-------------------------------------1


) tt
", Branch, fromDate, toDate);


        DataTable dt = DAL.LoadDataByQuery(sql);
        if (dt.Rows.Count > 0)
        {
            lblFromTo.Text = string.Format(@"Statement From {0} - {1}", dptFromDate.Text, dptToDate.Text);

            lblOpeningStockValue.Text = dt.Rows[0]["OpeningStockValue"].ToString();
            lblPurchaseValue.Text = dt.Rows[0]["Purchasevalue"].ToString();
            lblDirectExpense.Text = dt.Rows[0]["DirectExpense"].ToString();
            lblNetSale.Text = dt.Rows[0]["NetSale"].ToString();
            lblClosingStock.Text = dt.Rows[0]["ClosingStockValue"].ToString();
            lblIndirectExpense.Text = dt.Rows[0]["IndirectExpense"].ToString();
            lblIncome.Text = dt.Rows[0]["Income"].ToString();
            lblCommission.Text = dt.Rows[0]["Comi"].ToString();/////////

            decimal GrossProfit = 0;
            decimal NetProfit = 0;
            if (decimal.TryParse(dt.Rows[0]["GrossProfit"].ToString(), out GrossProfit)) { }
            if (decimal.TryParse(dt.Rows[0]["NetProfit"].ToString(), out NetProfit)) { }

            if (GrossProfit >= 0)
            {
                lblGrossProfitLabel.Text = "Gross Profit";
                lblGrossProfit.Text = GrossProfit.ToString("0.00");
            }
            else
            {
                lblGrossLossLabel.Text = "Gross Loss";
                lblGrossLoss.Text = GrossProfit.ToString("0.00");
            }
            if (NetProfit >= 0)
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