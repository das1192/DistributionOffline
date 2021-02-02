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


public partial class Pages_ReportAccBalanceSheet : System.Web.UI.Page
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

            dptDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            //dptToDate.Text = date.ToString("dd MMM yyyy");
        }

    }


    protected void btnShow_Click(object sender, EventArgs e)
    {
        LoadData();
        //LoadGridJournal();
    }

    private void LoadData()
    {
        h2HeadLine.InnerText = "";
        lblClosingStock.Text = "";
        lblBank.Text = "";
        lblCash.Text = "";
        lblAccountsReceivable.Text = "";

        lblProfitLoss.Text = "";
        lblAccountsPayable.Text = "";
        // total commission level text
        lblTotalCommission.Text = "";

        lblInvest.Text = "";
        lblWithdrawFB.Text = "";
        lblTotalAsset.Text = "";
        lblTotalLOE.Text = "";
        lblLoss.Text = string.Empty;


        DateTime dateDT = DateTime.Now;
        if (!string.IsNullOrEmpty(dptDate.Text)) { if (DateTime.TryParse(dptDate.Text, out dateDT)) { } }
        string ParamDate = dateDT.ToString("yyyy-MM-dd");

        #region
        //        string sql = string.Format(@"
        //select ttt.OpeningStockValue,ttt.ClosingStockValue,ttt.Purchasevalue,ttt.NetSale,ttt.DirectExpense
        //,ttt.IndirectExpense,ttt.Income,ttt.GrossProfit,ttt.NetProfit,ttt.cashAmount,ttt.BankAmount
        //,ttt.AccountsPayable,ttt.AccountsReceivable,ttt.Invest
        //
        //,TotalAsset=(
        //CONVERT(decimal(18,2),ISNULL(ttt.ClosingStockValue,0))+
        //CONVERT(decimal(18,2),ISNULL(ttt.BankAmount,0))+
        //CONVERT(decimal(18,2),ISNULL(ttt.cashAmount,0))+
        //CONVERT(decimal(18,2),ISNULL(ttt.AccountsReceivable,0))
        //)
        //,TotalLiabilityOE=(
        //CONVERT(decimal(18,2),ISNULL(ttt.NetProfit+ttt.AccountsPayable,0))+
        //CONVERT(decimal(18,2),ISNULL(ttt.Invest,0))
        //)
        //from (
        //--========================================================================Profit or Loss
        //--one date
        //select tt.OpeningStockValue,
        //tt.ClosingStockValue,tt.Purchasevalue ,tt.NetSale ,tt.DirectExpense ,tt.IndirectExpense ,tt.TilltodayPRPROT_Value as Income
        //,GrossProfit = (tt.NetSale + tt.TilltodayPRPROT_Value  -(tt.OpeningStockValue + tt.Purchasevalue -tt.ClosingStockValue) -tt.DirectExpense )
        //,NetProfit = (tt.NetSale + tt.TilltodayPRPROT_Value -(tt.OpeningStockValue + tt.Purchasevalue -tt.ClosingStockValue) -tt.DirectExpense-tt.IndirectExpense  )
        //
        //
        //
        //--================================================================================================
        //--======Bank and Cash
        //,cashAmount=(select CONVERT(decimal(18,2),ISNULL(SUM(Balance),0)) cashAmount from dbo.vw_Shopwise_Cash_Balance b where b.Branch ='{0}')
        //,BankAmount=(select CONVERT(decimal(18,2),ISNULL(SUM(Balance),0)) BankAmount from dbo.vw_Shopwise_Bank_Balance b where b.Branch ='{0}')
        //
        //
        //--A/P  A/R
        //,AccountsPayable=(
        //select ABS( CONVERT(decimal(18,2),ISNULL(SUM(j.Debit-j.Credit),0)) )AccountsPayable
        //from Acc_Journal j
        //where j.Branch='{0}' and j.Particular='A/P'   and CONVERT(date, j.IDATTIME) <= '{1}'
        //)
        //,AccountsReceivable=(
        //select CONVERT(decimal(18,2),ISNULL(SUM(j.Debit-j.Credit),0)) AccountsReceivable
        //from Acc_Journal j
        //where j.Branch='{0}' and j.Particular='A/R'   and CONVERT(date, j.IDATTIME) <= '{1}'
        //)
        //,Invest=(select ABS(  CONVERT(decimal(18,2),ISNULL(SUM(j.Debit-j.Credit),0))) Invest
        //from Acc_Journal j
        //where j.Branch='{0}' and j.Particular='Invest'   and CONVERT(date, j.IDATTIME) <= '{1}'
        //)
        //--================================================================================================
        //
        //
        //
        // from (
        //
        //------------------------------------------1
        //select 
        //OpeningStockValue = (t.TillYesterdayP_Cost-t.TillYesterdayS_Cost-t.TillYesterdayPR_Cost+t.TillYesterdaySR_Cost-t.TillYesterdayPRPROT_Cost -t.TillYesterdayPRMIS_Cost),
        //ClosingStockValue = (t.TillYesterdayP_Cost-t.TillYesterdayS_Cost-t.TillYesterdayPR_Cost+t.TillYesterdaySR_Cost+t.TilltodayP_Cost-t.TilltodayS_Cost+t.TilltodaySR_Cost-t.TilltodayPR_Cost-t.TilltodayPRPROT_Value-t.TillYesterdayPRPROT_Cost-t.TillYesterdayPRMIS_Cost-t.TilltodayPRMIS_Value),
        //Purchasevalue = (t.TilltodayP_Cost-t.TilltodayPR_Cost-t.TilltodayPRMIS_Value),
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
        //where sd.Flag='purchase' and sd.Branch='{0}' and CONVERT(date,sd.IDATTIME)< '{1}')
        //
        //,TillYesterdayPR_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.CostPrice),0)) from Acc_StockDetail sd 
        //where sd.Flag='purchase return' and sd.Branch='{0}' and CONVERT(date,sd.IDATTIME)< '{1}')
        //
        //,TillYesterdayS_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.CostPrice),0)) from Acc_StockDetail sd 
        //where sd.Flag='sale' and sd.Branch='{0}' and CONVERT(date,sd.IDATTIME)< '{1}')
        //
        //,TillYesterdaySR_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.CostPrice),0)) from Acc_StockDetail sd 
        //where sd.Flag='sale return' and sd.Branch='{0}' and CONVERT(date,sd.IDATTIME)< '{1}')
        //
        //
        //,TillYesterdayPRPROT_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.CostPrice),0)) from Acc_StockDetail sd 
        //where sd.Flag='Price Adjustment' and sd.Branch='{0}' and CONVERT(date,sd.IDATTIME)< '{1}')
        //
        //,TillYesterdayPRMIS_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.CostPrice),0)) from Acc_StockDetail sd 
        //where sd.Flag='Product Missing' and sd.Branch='{0}' and CONVERT(date,sd.IDATTIME) < '{1}')
        //
        //
        //
        //,TilltodayP_Cost=
        //(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.CostPrice),0)) from Acc_StockDetail sd 
        //where sd.Flag='purchase' and sd.Branch='{0}' and CONVERT(date,sd.IDATTIME) <= '{1}')
        //
        //,TilltodayPR_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.CostPrice),0)) from Acc_StockDetail sd 
        //where sd.Flag='purchase return' and sd.Branch='{0}' and CONVERT(date,sd.IDATTIME) <= '{1}')
        //
        //,TilltodayS_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.CostPrice),0)) from Acc_StockDetail sd 
        //where sd.Flag='sale' and sd.Branch='{0}' and CONVERT(date,sd.IDATTIME) <= '{1}')
        //
        //,TilltodaySR_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.CostPrice),0)) from Acc_StockDetail sd 
        //where sd.Flag='sale return' and sd.Branch='{0}' and CONVERT(date,sd.IDATTIME) <= '{1}')
        //
        //,TilltodayS_Value=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.SalePrice ),0)) from Acc_StockDetail sd 
        //where sd.Flag='sale' and sd.Branch='{0}' and CONVERT(date,sd.IDATTIME) <= '{1}')
        //
        //,TilltodaySR_Value=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.SalePrice ),0)) from Acc_StockDetail sd 
        //where sd.Flag='sale return' and sd.Branch='{0}' and CONVERT(date,sd.IDATTIME) <= '{1}')
        //
        //
        //,TilltodayPRPROT_Value=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.CostPrice ),0)) from Acc_StockDetail sd 
        //where sd.Flag='Price Protection' and sd.Branch='{0}' and CONVERT(date,sd.IDATTIME) <= '{1}')
        //
        //,TilltodayPRMIS_Value=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.CostPrice ),0)) from Acc_StockDetail sd 
        //where sd.Flag='Product Missing' and sd.Branch='{0}' and CONVERT(date,sd.IDATTIME) <= '{1}')
        //
        //
        //-----------------------expense
        //,DirectExpense =(
        //select CONVERT(decimal(18,2),ISNULL(SUM(dc.AMOUNT),0)) as total
        //from DailyCost dc  
        //inner join CostingHead ch on ch.OID=dc.CostingHeadID
        //where ch .ExpenseType='Direct' and dc.Shop_id='{0}' and CONVERT(date,dc.IDAT) <= '{1}'
        //)
        //
        //,IndirectExpense =(
        //select CONVERT(decimal(18,2),ISNULL(SUM(dc.AMOUNT),0)) as total
        //from DailyCost dc  
        //inner join CostingHead ch on ch.OID=dc.CostingHeadID
        //where ch .ExpenseType='Indirect' and dc.Shop_id='{0}' and CONVERT(date,dc.IDAT) <= '{1}'
        //)
        //
        //-------------------------------------0
        //
        //) t
        //-------------------------------------1
        //
        //
        //) tt
        //
        //)ttt
        //", Branch, ParamDate);
        #endregion

        string sql = string.Format(@"
select ttt.OpeningStockValue,ttt.ClosingStockValue,ttt.Purchasevalue,ttt.NetSale,ttt.DirectExpense
,ttt.IndirectExpense,ttt.Income,ttt.GrossProfit,ttt.NetProfit,ttt.cashAmount,ttt.BankAmount
,ttt.AccountsPayable,ttt.Commission,ttt.AccountsReceivable,ttt.Invest,ttt.WithdrawFromBusiness

,TotalAsset=(
CONVERT(decimal(18,2),ISNULL(ttt.ClosingStockValue,0))+
CONVERT(decimal(18,2),ISNULL(ttt.BankAmount,0))+
CONVERT(decimal(18,2),ISNULL(ttt.cashAmount,0))+
CONVERT(decimal(18,2),ISNULL(ttt.AccountsReceivable,0))
)
,TotalLiabilityOE=(
CONVERT(decimal(18,2),ISNULL(ttt.NetProfit+ttt.AccountsPayable,0))+
CONVERT(decimal(18,2),ISNULL(ttt.Invest,0))
)
,TotalLiabilityOE=(
CONVERT(decimal(18,2),ISNULL(ttt.NetProfit+ttt.AccountsPayable,0))+
CONVERT(decimal(18,2),ISNULL(ttt.WithdrawFromBusiness,0))
)
from (
--========================================================================Profit or Loss
--one date
select tt.OpeningStockValue,
tt.ClosingStockValue,tt.Purchasevalue ,tt.NetSale ,tt.DirectExpense ,tt.IndirectExpense ,tt.TilltodayPRPROT_Value as Income
,GrossProfit = (tt.NetSale + tt.TilltodayPRPROT_Value  -( tt.Purchasevalue -tt.ClosingStockValue) -tt.DirectExpense )
,NetProfit = (tt.NetSale + tt.TilltodayPRPROT_Value -( tt.Purchasevalue -tt.ClosingStockValue) -tt.DirectExpense-tt.IndirectExpense  )




--================================================================================================
--======Bank and Cash
,cashAmount=(select CONVERT(decimal(18,2),ISNULL(SUM(Balance),0)) cashAmount from dbo.vw_Shop_date_wise_cash_Balance b where b.Branch='{0}' AND IDAT<='{1}')
,BankAmount=(select CONVERT(decimal(18,2),ISNULL(SUM(Balance),0)) BankAmount from dbo.vw_Shop_date_wise_bank_Balance b where b.Branch='{0}' AND IDAT<='{1}')


--A/P  A/R T/C
,AccountsPayable=(
select ( CONVERT(decimal(18,2),ISNULL(SUM(j.Debit-j.Credit),0)) )AccountsPayable
from Acc_Journal j
where j.Branch='{0}' and j.Particular='A/P'   and CONVERT(date, j.IDATTIME) <= '{1}'
)
,AccountsReceivable=(
select CONVERT(decimal(18,2),ISNULL(SUM(j.Debit-j.Credit),0)) AccountsReceivable
from Acc_Journal j
where j.Branch='{0}' and j.Particular='A/R'   and CONVERT(date, j.IDATTIME) <= '{1}'
)
,Invest=(select (  CONVERT(decimal(18,2),ISNULL(SUM(j.Debit-j.Credit),0))) Invest
from Acc_Journal j
where j.Branch='{0}' and j.Particular='Invest'   and CONVERT(date, j.IDATTIME) <= '{1}'
)
,WithdrawFromBusiness=(select (  CONVERT(decimal(18,2),ISNULL(SUM(j.Debit-j.Credit),0))) WithdrawFromBusiness
from Acc_Journal j
where j.Branch='{0}' and j.Particular='WithdrawFromBusiness'   and CONVERT(date, j.IDATTIME) <= '{1}'
),
Commission=(SELECT 
      (SUM(j.Credit))Commission from Acc_Journal j where j.Particular='Commission' and j.Branch ='{0}' and CONVERT(date, j.IDATTIME) <= '{1}' )
--================================================================================================



 from (

------------------------------------------1
select 
OpeningStockValue = (t.TillYesterdayP_Cost-t.TillYesterdayS_Cost-t.TillYesterdayPR_Cost+t.TillYesterdaySR_Cost-t.TillYesterdayPRPROT_Cost -t.TillYesterdayPRMIS_Cost),
ClosingStockValue = (t.TillYesterdayP_Cost-t.TillYesterdayS_Cost-t.TillYesterdayPR_Cost+t.TillYesterdaySR_Cost+t.todayP_Cost-t.todayS_Cost+t.todaySR_Cost-t.todayPR_Cost-t.todayPRPROT_Value-t.TillYesterdayPRPROT_Cost-t.TillYesterdayPRMIS_Cost-t.todayPRMIS_Value),
Purchasevalue = (t.tilltodayP_Cost-t.tilltodayPR_Cost-t.tilltodayPRMIS_Value),
NetSale = (t.TilltodayS_Value-t.TilltodaySR_Value)
,t.DirectExpense,t.IndirectExpense,t.TilltodayPRPROT_Value 


from 

(


--------------------------------------0
select 
TillYesterdayP_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.CostPrice),0)) from Acc_StockDetail sd 
where sd.Flag='purchase' and sd.Branch='{0}' and CONVERT(date,sd.IDATTIME)< '{1}')

,TillYesterdayPR_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.CostPrice),0)) from Acc_StockDetail sd 
where sd.Flag='purchase return' and sd.Branch='{0}' and CONVERT(date,sd.IDATTIME)< '{1}')

,TillYesterdayS_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.CostPrice),0)) from Acc_StockDetail sd 
where sd.Flag='sale' and sd.Branch='{0}' and CONVERT(date,sd.IDATTIME)< '{1}')

,TillYesterdaySR_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.CostPrice),0)) from Acc_StockDetail sd 
where sd.Flag='sale return' and sd.Branch='{0}' and CONVERT(date,sd.IDATTIME)< '{1}')


,TillYesterdayPRPROT_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.CostPrice),0)) from Acc_StockDetail sd 
where sd.Flag='Price Protection' and sd.Branch='{0}' and CONVERT(date,sd.IDATTIME)< '{1}')

,TillYesterdayPRMIS_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.CostPrice),0)) from Acc_StockDetail sd 
where sd.Flag='Product Missing' and sd.Branch='{0}' and CONVERT(date,sd.IDATTIME) < '{1}')



,TilltodayP_Cost=
(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.CostPrice),0)) from Acc_StockDetail sd 
where sd.Flag='purchase' and sd.Branch='{0}' and CONVERT(date,sd.IDATTIME) <= '{1}')

,TilltodayPR_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.CostPrice),0)) from Acc_StockDetail sd 
where sd.Flag='purchase return' and sd.Branch='{0}' and CONVERT(date,sd.IDATTIME) <= '{1}')

,TilltodayS_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.CostPrice),0)) from Acc_StockDetail sd 
where sd.Flag='sale' and sd.Branch='{0}' and CONVERT(date,sd.IDATTIME) <= '{1}')

,TilltodaySR_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.CostPrice),0)) from Acc_StockDetail sd 
where sd.Flag='sale return' and sd.Branch='{0}' and CONVERT(date,sd.IDATTIME) <= '{1}')

,TilltodayS_Value=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.SalePrice ),0)) from Acc_StockDetail sd 
where sd.Flag='sale' and sd.Branch='{0}' and CONVERT(date,sd.IDATTIME) <= '{1}')

,TilltodaySR_Value=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.SalePrice ),0)) from Acc_StockDetail sd 
where sd.Flag='sale return' and sd.Branch='{0}' and CONVERT(date,sd.IDATTIME) <= '{1}')


,TilltodayPRPROT_Value=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.CostPrice ),0)) from Acc_StockDetail sd 
where sd.Flag='Price Protection' and sd.Branch='{0}' and CONVERT(date,sd.IDATTIME) <= '{1}')

,TilltodayPRMIS_Value=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.CostPrice ),0)) from Acc_StockDetail sd 
where sd.Flag='Product Missing' and sd.Branch='{0}' and CONVERT(date,sd.IDATTIME) <= '{1}')




,todayP_Cost=
(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.CostPrice),0)) from Acc_StockDetail sd 
where sd.Flag='purchase' and sd.Branch='{0}' and CONVERT(date,sd.IDATTIME) = '{1}')

,todayPR_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.CostPrice),0)) from Acc_StockDetail sd 
where sd.Flag='purchase return' and sd.Branch='{0}' and CONVERT(date,sd.IDATTIME) = '{1}')

,todayS_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.CostPrice),0)) from Acc_StockDetail sd 
where sd.Flag='sale' and sd.Branch='{0}' and CONVERT(date,sd.IDATTIME) = '{1}')

,todaySR_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.CostPrice),0)) from Acc_StockDetail sd 
where sd.Flag='sale return' and sd.Branch='{0}' and CONVERT(date,sd.IDATTIME) = '{1}')

,todayS_Value=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.SalePrice ),0)) from Acc_StockDetail sd 
where sd.Flag='sale' and sd.Branch='{0}' and CONVERT(date,sd.IDATTIME) = '{1}')

,todaySR_Value=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.SalePrice ),0)) from Acc_StockDetail sd 
where sd.Flag='sale return' and sd.Branch='{0}' and CONVERT(date,sd.IDATTIME) = '{1}')


,todayPRPROT_Value=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.CostPrice ),0)) from Acc_StockDetail sd 
where sd.Flag='Price Protection' and sd.Branch='{0}' and CONVERT(date,sd.IDATTIME) = '{1}')

,todayPRMIS_Value=(select CONVERT(decimal(18,2),ISNULL(SUM(sd.Quantity*sd.CostPrice ),0)) from Acc_StockDetail sd 
where sd.Flag='Product Missing' and sd.Branch='{0}' and CONVERT(date,sd.IDATTIME) = '{1}')



-----------------------expense
,DirectExpense =(
select CONVERT(decimal(18,2),ISNULL(SUM(dc.Balance),0)) as total
from DirectExpenseView dc  

where dc.Branch='{0}' and CONVERT(date,dc.IDAT) <= '{1}'
)

,IndirectExpense =(
select CONVERT(decimal(18,2),ISNULL(SUM(dc.Balance),0)) as total
from IndiectExpenseView dc  

where dc.Branch='{0}' and CONVERT(date,dc.IDAT) <= '{1}'
)

-------------------------------------0

) t
-------------------------------------1


) tt

)ttt

", Branch, ParamDate);

        DataTable dt = DAL.LoadDataByQuery(sql);
        if (dt.Rows.Count > 0)
        {
            h2HeadLine.InnerText = string.Format(@"Balance Sheet on {0}", dptDate.Text);

            //OpeningStockValue	ClosingStockValue	Purchasevalue	NetSale	DirectExpense	IndirectExpense	
            //Income	GrossProfit	NetProfit	cashAmount	BankAmount	AccountsPayable	AccountsReceivable	Invest

            lblClosingStock.Text = dt.Rows[0]["ClosingStockValue"].ToString();
            lblBank.Text = dt.Rows[0]["BankAmount"].ToString();
            lblCash.Text = dt.Rows[0]["cashAmount"].ToString();
            lblAccountsReceivable.Text = dt.Rows[0]["AccountsReceivable"].ToString();


            decimal AccPayable = 0;
            if (decimal.TryParse((dt.Rows[0]["AccountsPayable"].ToString()), out AccPayable)) { }
            lblAccountsPayable.Text = ((-1)*(AccPayable)).ToString("0.00");
            
            // try out for Commission update
            decimal TotalCommissions = 0;
            if (decimal.TryParse((dt.Rows[0]["Commission"].ToString()), out TotalCommissions)) { }
            lblTotalCommission.Text = ( (TotalCommissions)).ToString("0.00");

            decimal Invest = 0;
            if (decimal.TryParse((dt.Rows[0]["Invest"].ToString()), out Invest)) { }
            lblInvest.Text = ((-1) * (Invest)).ToString("0.00");

            decimal WithdrawFromBusiness = 0;
            if (decimal.TryParse((dt.Rows[0]["WithdrawFromBusiness"].ToString()), out WithdrawFromBusiness)) { }
            lblWithdrawFB.Text = ( (WithdrawFromBusiness)).ToString("0.00");
            
            
            
            
            lblTotalAsset.Text = dt.Rows[0]["TotalAsset"].ToString();
            lblTotalLOE.Text = dt.Rows[0]["TotalLiabilityOE"].ToString();
            decimal ProfitOrLoss = 0;
            if (decimal.TryParse((dt.Rows[0]["NetProfit"].ToString()), out ProfitOrLoss)) { }
            if (ProfitOrLoss < 0)
            {
                lblProfitLoss.Text = "0.00";
                lblLoss.Text = Math.Abs(ProfitOrLoss).ToString("0.00");
            }
            else
            {
                lblProfitLoss.Text = ProfitOrLoss.ToString("0.00");
                lblLoss.Text = "0.00";
            }

           
        }

        //
        decimal ClosingStock = 0;
        decimal Bank = 0;
        decimal Cash = 0;
        decimal AccountsReceivable2 = 0;
        decimal Loss = 0;

        decimal Profit = 0;
        decimal AccountsPayable = 0;
        decimal TotalComi = 0;
        decimal InvestCapital = 0;
        decimal WithdrawFromBusinessnew = 0;

        if (decimal.TryParse(lblClosingStock.Text, out ClosingStock)) { }
        if (decimal.TryParse(lblBank.Text, out Bank)) { }
        if (decimal.TryParse(lblCash.Text, out Cash)) { }
        if (decimal.TryParse(lblAccountsReceivable.Text, out AccountsReceivable2)) { }
        if (decimal.TryParse(lblLoss.Text, out Loss)) { }

        if (decimal.TryParse(lblProfitLoss.Text, out Profit)) { }
        if (decimal.TryParse(lblWithdrawFB.Text, out WithdrawFromBusinessnew)) { }
        if (decimal.TryParse(lblTotalCommission .Text, out TotalComi)) { }
        if (decimal.TryParse(lblAccountsPayable.Text, out AccountsPayable))
        {
            if (AccountsPayable < 0)
            {
                AccountsReceivable2 = AccountsPayable * (-1) + Convert.ToInt32(dt.Rows[0]["AccountsReceivable"]); AccountsPayable = 0;


            }
            else { AccountsReceivable2 = Convert.ToInt32(dt.Rows[0]["AccountsReceivable"]); }
        }
        lblAccountsPayable.Text = AccountsPayable.ToString("0.00");
        lblAccountsReceivable.Text = AccountsReceivable2.ToString("0.00");
        if (decimal.TryParse(lblInvest.Text, out InvestCapital)) { }
       
        lblTotalAsset.Text = (ClosingStock + Bank + Cash + AccountsReceivable2 + Loss).ToString("0.00");
       lblTotalLOE.Text= (Profit + AccountsPayable + TotalComi + InvestCapital - WithdrawFromBusinessnew).ToString("0.00");
        
    }

}