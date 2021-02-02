using System;
using System.Data.SqlClient;
using System.Web;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using TalukderPOS.BusinessObjects;
using System.Configuration;
using TalukderPOS.BLL;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Drawing;

public partial class Pages_BranchStockSummeryProductWise : System.Web.UI.Page
{
    private string userID = string.Empty;
    private string userPassword = string.Empty;
    T_PRODBLL BILL = new T_PRODBLL();
    private string Shop_id = string.Empty;

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

            if (!IsPostBack)
            {
                DateTime date = DateTime.Now;
                var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                txtDateFrom2.Text = firstDayOfMonth.ToString("dd MMM yyyy"); //firstDayOfMonth.ToString("dd MMM yyyy");
                txtDateTo2.Text = DateTime.Now.ToString("dd MMM yyyy");
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
        }
    }


    public DataTable GetDataTableByQuery(string sqlQuery)
    {
        DataTable dt = new DataTable();
        string constr = ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand(sqlQuery))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    //using (DataTable dt = new DataTable())
                    //{
                    sda.Fill(dt);
                    //}
                }
            }
        }
        return dt;
    }

    public void SaveDataByCRUD(string sqlQuery)
    {
        string constr = ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString;

        using (SqlConnection connection = new SqlConnection(constr))
        {
            SqlCommand command = new SqlCommand(sqlQuery, connection);
            command.Connection.Open();
            command.ExecuteNonQuery();
        }
    }//


    protected void btnSearchStockSummery_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtDateFrom2.Text))
        {
            LoadGridStockSummery();
        }

    }

    private void LoadGridStockSummery()
    {
        string param = string.Empty;
        // and cat.Shop_id='' and cat.OID=0 and d.OID=1 and d.SubCategoryID=1

        if (!string.IsNullOrEmpty(Shop_id))
        {
            param += string.Format(@" and cat.Shop_id='{0}'", Shop_id);
        }
        if (!string.IsNullOrEmpty(ddlSearchProductCategory3.SelectedValue))
        {
            param += string.Format(@" and cat.OID='{0}'", ddlSearchProductCategory3.SelectedValue);
        }
        if (!string.IsNullOrEmpty(ddlSearchSubCategory3.SelectedValue))
        {
            param += string.Format(@" and d.SubCategoryID='{0}'", ddlSearchSubCategory3.SelectedValue);
        }
        if (!string.IsNullOrEmpty(ddlSearchDescription3.SelectedValue))
        {
            param += string.Format(@" and d.OID='{0}'", ddlSearchDescription3.SelectedValue);
        }

        #region
        //        string sql = string.Format(@"
//select Shop_id, CategoryName,SubcategoryName,Description,
//
//OpeningStockQty = (TillYesterdayP_Qty-TillYesterdayS_Qty-TillYesterdayPR_Qty+TillYesterdaySR_Qty-TillYesterdayPRMIS_Qty)
//,OpeningStockValue = (TillYesterdayP_Cost-TillYesterdayS_Cost-TillYesterdayPR_Cost+TillYesterdaySR_Cost-TillYesterdayPRPROT_Cost-TillYesterdayPRMIS_Cost) 
//
//,todayP_Qty
//,todayPR_Qty
//,todayS_Qty
//,todaySR_Qty
//,todayPRMIS_Qty
//
//
//,todayP_Cost
//,todayPR_Cost
//,todayS_Cost
//,todaySR_Cost
//,todayPRPROT_Value 
//,todayPRMIS_Value
//,TillYesterdayPRPROT_Cost 
//,ClosingStockQty = (TillYesterdayP_Qty-TillYesterdayS_Qty-TillYesterdayPR_Qty+TillYesterdaySR_Qty+todayP_Qty-todayS_Qty+todaySR_Qty-todayPR_Qty-TillYesterdayPRMIS_Qty-todayPRMIS_Qty)
//,ClosingStockValue = (TillYesterdayP_Cost-TillYesterdayS_Cost-TillYesterdayPR_Cost+TillYesterdaySR_Cost+todayP_Cost-todayS_Cost+todaySR_Cost-todayPR_Cost-todayPRPROT_Value- todayPRMIS_Value-TillYesterdayPRMIS_Cost)
//
//,TillYesterdayP_Qty,TillYesterdayPR_Qty,TillYesterdayS_Qty,TillYesterdaySR_Qty,TillYesterdayPRMIS_Qty
//
//,TillYesterdayP_Cost,TillYesterdayPR_Cost,TillYesterdayS_Cost,TillYesterdaySR_Cost,TillYesterdayPRMIS_Cost
//from (
//------------------
//select cat.WGPG_NAME as CategoryName ,sub.SubCategoryName as SubcategoryName ,d.Description ,cat.Shop_id 
//
//,todayP_Qty=(
//select CONVERT(decimal(18,2),ISNULL(SUM(asd.Quantity),0)) from Acc_StockDetail asd 
//where asd.PROD_SUBCATEGORY=sub.OID and asd.PROD_DES=d.OID 
//and asd.Flag='Purchase' and CONVERT(date,asd.IDATTIME)=CONVERT(date,'{0}')
//)
//
//,todayPR_Qty=(
//select CONVERT(decimal(18,2),ISNULL(SUM(asd.Quantity),0)) from Acc_StockDetail asd where asd.PROD_SUBCATEGORY=sub.OID and asd.PROD_DES=d.OID
//and asd.Flag='Purchase Return' and CONVERT(date,asd.IDATTIME)=CONVERT(date,'{0}')
//)
//
//,todayS_Qty=(
//select CONVERT(decimal(18,2),ISNULL(SUM(asd.Quantity),0)) from Acc_StockDetail asd where asd.PROD_SUBCATEGORY=sub.OID and asd.PROD_DES=d.OID
//and asd.Flag='Sale' and CONVERT(date,asd.IDATTIME)=CONVERT(date,'{0}')
//)
//
//,todaySR_Qty=
//(select CONVERT(decimal(18,2),ISNULL(SUM(asd.Quantity),0)) from Acc_StockDetail asd where asd.PROD_SUBCATEGORY=sub.OID and asd.PROD_DES=d.OID
//and asd.Flag='Sale Return' and CONVERT(date,asd.IDATTIME)=CONVERT(date,'{0}'))
//
//
//,todayPRMIS_Qty=
//(select CONVERT(decimal(18,2),ISNULL(SUM(asd.Quantity),0)) from Acc_StockDetail asd where asd.PROD_SUBCATEGORY=sub.OID and asd.PROD_DES=d.OID
//and asd.Flag='Product Missing' and CONVERT(date,asd.IDATTIME)=CONVERT(date,'{0}'))
//
//
//-------------cost
//,todayP_Cost=
//(select CONVERT(decimal(18,2),ISNULL(SUM(asd.Quantity*asd.CostPrice),0)) from Acc_StockDetail asd 
//where asd.PROD_SUBCATEGORY=sub.OID and asd.PROD_DES=d.OID
//and asd.Flag='Purchase' and CONVERT(date,asd.IDATTIME)=CONVERT(date,'{0}'))
//
//,todayPR_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(asd.Quantity*asd.CostPrice),0)) from Acc_StockDetail asd 
//where asd.PROD_SUBCATEGORY=sub.OID and asd.PROD_DES=d.OID
//and asd.Flag='Purchase Return' and CONVERT(date,asd.IDATTIME)=CONVERT(date,'{0}'))
//
//,todayS_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(asd.Quantity*asd.CostPrice),0)) from Acc_StockDetail asd 
//where asd.PROD_SUBCATEGORY=sub.OID and asd.PROD_DES=d.OID
//and asd.Flag='Sale' and CONVERT(date,asd.IDATTIME)=CONVERT(date,'{0}'))
//
//,todaySR_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(asd.Quantity*asd.CostPrice),0)) from Acc_StockDetail asd 
//where asd.PROD_SUBCATEGORY=sub.OID and asd.PROD_DES=d.OID
//and asd.Flag='Sale Return' and CONVERT(date,asd.IDATTIME)=CONVERT(date,'{0}'))
//
//,todayPRPROT_Value=(select CONVERT(decimal(18,2),ISNULL(SUM(asd.Quantity*asd.CostPrice ),0)) from Acc_StockDetail asd 
//where asd.PROD_SUBCATEGORY=sub.OID and asd.PROD_DES=d.OID AND
//asd.Flag='Price Protection' and CONVERT(date,asd.IDATTIME) =CONVERT(date,'{0}'))
//
//
//,todayPRMIS_Value=(select CONVERT(decimal(18,2),ISNULL(SUM(asd.Quantity*asd.CostPrice ),0)) from Acc_StockDetail asd 
//where asd.PROD_SUBCATEGORY=sub.OID and asd.PROD_DES=d.OID AND
//asd.Flag='Product Missing' and CONVERT(date,asd.IDATTIME) =CONVERT(date,'{0}'))
//
//------------------------------------ total till yesterday
//,TillYesterdayP_Qty=(select CONVERT(decimal(18,2),ISNULL(SUM(asd.Quantity),0)) from Acc_StockDetail asd 
//where asd.PROD_SUBCATEGORY=sub.OID and asd.PROD_DES=d.OID
//and asd.Flag='Purchase' and CONVERT(date,asd.IDATTIME)<CONVERT(date,'{0}'))
//
//,TillYesterdayPR_Qty=(select CONVERT(decimal(18,2),ISNULL(SUM(asd.Quantity),0)) from Acc_StockDetail asd 
//where asd.PROD_SUBCATEGORY=sub.OID and asd.PROD_DES=d.OID
//and asd.Flag='Purchase Return' and CONVERT(date,asd.IDATTIME)<CONVERT(date,'{0}'))
//
//,TillYesterdayS_Qty=(select CONVERT(decimal(18,2),ISNULL(SUM(asd.Quantity),0)) from Acc_StockDetail asd 
//where asd.PROD_SUBCATEGORY=sub.OID and asd.PROD_DES=d.OID
//and asd.Flag='Sale' and CONVERT(date,asd.IDATTIME)<CONVERT(date,'{0}'))
//
//,TillYesterdaySR_Qty=(select CONVERT(decimal(18,2),ISNULL(SUM(asd.Quantity),0)) from Acc_StockDetail asd 
//where asd.PROD_SUBCATEGORY=sub.OID and asd.PROD_DES=d.OID
//and asd.Flag='Sale Return' and CONVERT(date,asd.IDATTIME)<CONVERT(date,'{0}'))
//
//,TillYesterdayPRMIS_Qty=(select CONVERT(decimal(18,2),ISNULL(SUM(asd.Quantity),0)) from Acc_StockDetail asd 
//where asd.PROD_SUBCATEGORY=sub.OID and asd.PROD_DES=d.OID
//and asd.Flag='Product Missing' and CONVERT(date,asd.IDATTIME)<CONVERT(date,'{0}'))
//
//
//-------------cost
//,TillYesterdayP_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(asd.Quantity*asd.CostPrice),0)) from Acc_StockDetail asd 
//where asd.PROD_SUBCATEGORY=sub.OID and asd.PROD_DES=d.OID
//and asd.Flag='Purchase' and CONVERT(date,asd.IDATTIME)<CONVERT(date,'{0}'))
//
//,TillYesterdayPR_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(asd.Quantity*asd.CostPrice),0)) from Acc_StockDetail asd 
//where asd.PROD_SUBCATEGORY=sub.OID and asd.PROD_DES=d.OID
//and asd.Flag='Purchase Return' and CONVERT(date,asd.IDATTIME)<CONVERT(date,'{0}'))
//
//,TillYesterdayS_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(asd.Quantity*asd.CostPrice),0)) from Acc_StockDetail asd 
//where asd.PROD_SUBCATEGORY=sub.OID and asd.PROD_DES=d.OID
//and asd.Flag='Sale' and CONVERT(date,asd.IDATTIME)<CONVERT(date,'{0}'))
//
//,TillYesterdaySR_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(asd.Quantity*asd.CostPrice),0)) from Acc_StockDetail asd 
//where asd.PROD_SUBCATEGORY =sub.OID  and asd.PROD_DES=d.OID
//and asd.Flag='Sale Return' and CONVERT(date,asd.IDATTIME)<CONVERT(date,'{0}'))
//
//,TillYesterdayPRPROT_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(asd.Quantity*asd.CostPrice),0)) from Acc_StockDetail asd 
//where asd.PROD_SUBCATEGORY =sub.OID  and asd.PROD_DES=d.OID AND
//asd.Flag='Price Protection' and CONVERT(date,asd.IDATTIME)< CONVERT(date,'{0}'))
//
//
//,TillYesterdayPRMIS_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(asd.Quantity*asd.CostPrice),0)) from Acc_StockDetail asd 
//where asd.PROD_SUBCATEGORY =sub.OID  and asd.PROD_DES=d.OID AND
//asd.Flag='Product Missing' and CONVERT(date,asd.IDATTIME)< CONVERT(date,'{0}'))
//
//
//from Description d
//inner join SubCategory sub on sub.OID =d.SubCategoryID 
//left join T_WGPG cat on cat.OID =sub.CategoryID 
//
//
//where 1=1 {1}
//)t 
        //", txtDateFrom2.Text, param);
        #endregion
        string sql = string.Format(@"
declare @fdate date ='{0}'
declare @todate date ='{1}'
declare @mainshopid varchar(10) ='{3}'

----------------
select Shop_id, CategoryName,SubcategoryName,Description,
OpeningStockQty ,OpeningStockValue
,todayP_Qty
,todayPR_Qty
,todayS_Qty
,todaySR_Qty
,todayPRMIS_Qty
,todayP_Cost
,todayPR_Cost
,todayS_Cost
,todaySR_Cost
,todayPRPROT_Value 
,todayPRMIS_Value
,TillYesterdayPRPROT_Cost 
,ClosingStockQty
,ClosingStockValue
,TillYesterdayP_Qty,TillYesterdayPR_Qty,TillYesterdayS_Qty,TillYesterdaySR_Qty,TillYesterdayPRMIS_Qty

,TillYesterdayP_Cost,TillYesterdayPR_Cost,TillYesterdayS_Cost,TillYesterdaySR_Cost,TillYesterdayPRMIS_Cost
,Profit
,GiftAmount
,DiscountAmount
from
(

-------------
select Shop_id, CategoryName,SubcategoryName,Description,

OpeningStockQty = (TillYesterdayP_Qty-TillYesterdayS_Qty-TillYesterdayPR_Qty+TillYesterdaySR_Qty-TillYesterdayPRMIS_Qty)
,OpeningStockValue = (TillYesterdayP_Cost-TillYesterdayS_Cost-TillYesterdayPR_Cost+TillYesterdaySR_Cost-TillYesterdayPRPROT_Cost-TillYesterdayPRMIS_Cost) 

,todayP_Qty
,todayPR_Qty
,todayS_Qty
,todaySR_Qty
,todayPRMIS_Qty


,todayP_Cost
,todayPR_Cost
,todayS_Cost
,todaySR_Cost
,todayPRPROT_Value 
,todayPRMIS_Value
,TillYesterdayPRPROT_Cost 
,ClosingStockQty = (TillYesterdayP_Qty-TillYesterdayS_Qty-TillYesterdayPR_Qty+TillYesterdaySR_Qty+todayP_Qty-todayS_Qty+todaySR_Qty-todayPR_Qty-TillYesterdayPRMIS_Qty-todayPRMIS_Qty)
,ClosingStockValue = (TillYesterdayP_Cost-TillYesterdayS_Cost-TillYesterdayPR_Cost+TillYesterdaySR_Cost+todayP_Cost-todayS_Cost+todaySR_Cost-todayPR_Cost-todayPRPROT_Value- todayPRMIS_Value-TillYesterdayPRMIS_Cost-TillYesterdayPRPROT_Cost)




,TillYesterdayP_Qty,TillYesterdayPR_Qty,TillYesterdayS_Qty,TillYesterdaySR_Qty,TillYesterdayPRMIS_Qty

,TillYesterdayP_Cost,TillYesterdayPR_Cost,TillYesterdayS_Cost,TillYesterdaySR_Cost,TillYesterdayPRMIS_Cost

,Profit = (TilltodayS_Value -TilltodaySR_Value  -((TillYesterdayP_Cost-TillYesterdayS_Cost-TillYesterdayPR_Cost+TillYesterdaySR_Cost-TillYesterdayPRPROT_Cost-TillYesterdayPRMIS_Cost) + (todayP_Cost-todayPR_Cost)  -(TillYesterdayP_Cost-TillYesterdayS_Cost-TillYesterdayPR_Cost+TillYesterdaySR_Cost+todayP_Cost-todayS_Cost+todaySR_Cost-todayPR_Cost- todayPRMIS_Value-TillYesterdayPRMIS_Cost-TillYesterdayPRPROT_Cost)))

,GiftAmount= (select SUM(ISNULL(dc.AMOUNT,0)) from DailyCost dc where dc.Remarks='Gift' and dc.Shop_id=@mainshopid and CONVERT(date,dc.IDAT) between @fdate AND @todate)
,DiscountAmount= (select SUM(ISNULL(dc.AMOUNT,0)) from DailyCost dc where dc.Remarks='Discount' and dc.Shop_id=@mainshopid and CONVERT(date,dc.IDAT) between @fdate AND @todate)


from (
------------------
select cat.WGPG_NAME as CategoryName ,sub.SubCategoryName as SubcategoryName ,d.Description ,cat.Shop_id 

,todayP_Qty=(
select CONVERT(decimal(18,2),ISNULL(SUM(asd.Quantity),0)) from Acc_StockDetail asd 
where asd.PROD_SUBCATEGORY=sub.OID and asd.PROD_DES=d.OID 
and asd.Flag='Purchase' and CONVERT(date,asd.IDATTIME) BETWEEN CONVERT(date,@fdate) AND CONVERT(date,@todate)
)

,todayPR_Qty=(
select CONVERT(decimal(18,2),ISNULL(SUM(asd.Quantity),0)) from Acc_StockDetail asd where asd.PROD_SUBCATEGORY=sub.OID and asd.PROD_DES=d.OID
and asd.Flag='Purchase Return' and CONVERT(date,asd.IDATTIME) BETWEEN CONVERT(date,@fdate) AND CONVERT(date,@todate)
)

,todayS_Qty=(
select CONVERT(decimal(18,2),ISNULL(SUM(asd.Quantity),0)) from Acc_StockDetail asd where asd.PROD_SUBCATEGORY=sub.OID and asd.PROD_DES=d.OID
and asd.Flag='Sale' and CONVERT(date,asd.IDATTIME) BETWEEN CONVERT(date,@fdate) AND CONVERT(date,@todate)
)

,todaySR_Qty=
(select CONVERT(decimal(18,2),ISNULL(SUM(asd.Quantity),0)) from Acc_StockDetail asd where asd.PROD_SUBCATEGORY=sub.OID and asd.PROD_DES=d.OID
and asd.Flag='Sale Return' and CONVERT(date,asd.IDATTIME) BETWEEN CONVERT(date,@fdate) AND CONVERT(date,@todate)

)


,todayPRMIS_Qty=
(select CONVERT(decimal(18,2),ISNULL(SUM(asd.Quantity),0)) from Acc_StockDetail asd where asd.PROD_SUBCATEGORY=sub.OID and asd.PROD_DES=d.OID
and asd.Flag='Product Missing' and CONVERT(date,asd.IDATTIME) BETWEEN CONVERT(date,@fdate) AND CONVERT(date,@todate)
)


----------sale Value-------new Start--------------

,TilltodayS_Value=(select CONVERT(decimal(18,2),ISNULL(SUM(asd.Quantity*asd.SalePrice ),0)) from Acc_StockDetail asd 
where asd.PROD_SUBCATEGORY=sub.OID and asd.PROD_DES=d.OID and asd.Flag='sale' and CONVERT(date,asd.IDATTIME) between @fdate AND @todate)

,TilltodaySR_Value=(select CONVERT(decimal(18,2),ISNULL(SUM(asd.Quantity*asd.SalePrice ),0)) from Acc_StockDetail asd 
where asd.PROD_SUBCATEGORY=sub.OID and asd.PROD_DES=d.OID and asd.Flag='sale return' and CONVERT(date,asd.IDATTIME) between @fdate AND @todate)

----------------------new end-----------

-------------cost
,todayP_Cost=
(select CONVERT(decimal(18,2),ISNULL(SUM(asd.Quantity*asd.CostPrice),0)) from Acc_StockDetail asd 
where asd.PROD_SUBCATEGORY=sub.OID and asd.PROD_DES=d.OID
and asd.Flag='Purchase' and CONVERT(date,asd.IDATTIME) BETWEEN CONVERT(date,@fdate) AND CONVERT(date,@todate))

,todayPR_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(asd.Quantity*asd.CostPrice),0)) from Acc_StockDetail asd 
where asd.PROD_SUBCATEGORY=sub.OID and asd.PROD_DES=d.OID
and asd.Flag='Purchase Return' and CONVERT(date,asd.IDATTIME) BETWEEN CONVERT(date,@fdate) AND CONVERT(date,@todate))

,todayS_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(asd.Quantity*asd.CostPrice),0)) from Acc_StockDetail asd 
where asd.PROD_SUBCATEGORY=sub.OID and asd.PROD_DES=d.OID
and asd.Flag='Sale' and CONVERT(date,asd.IDATTIME) BETWEEN CONVERT(date,@fdate) AND CONVERT(date,@todate))

,todaySR_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(asd.Quantity*asd.CostPrice),0)) from Acc_StockDetail asd 
where asd.PROD_SUBCATEGORY=sub.OID and asd.PROD_DES=d.OID
and asd.Flag='Sale Return' and CONVERT(date,asd.IDATTIME) BETWEEN CONVERT(date,@fdate) AND CONVERT(date,@todate))

,todayPRPROT_Value=(select CONVERT(decimal(18,2),ISNULL(SUM(asd.Quantity*asd.CostPrice ),0)) from Acc_StockDetail asd 
where asd.PROD_SUBCATEGORY=sub.OID and asd.PROD_DES=d.OID AND
asd.Flag='Price Protection' and CONVERT(date,asd.IDATTIME) BETWEEN CONVERT(date,@fdate) AND CONVERT(date,@todate))


,todayPRMIS_Value=(select CONVERT(decimal(18,2),ISNULL(SUM(asd.Quantity*asd.CostPrice ),0)) from Acc_StockDetail asd 
where asd.PROD_SUBCATEGORY=sub.OID and asd.PROD_DES=d.OID AND
asd.Flag='Product Missing' and CONVERT(date,asd.IDATTIME) BETWEEN CONVERT(date,@fdate) AND CONVERT(date,@todate))



------------------------------------ total till yesterday
,TillYesterdayP_Qty=(select CONVERT(decimal(18,2),ISNULL(SUM(asd.Quantity),0)) from Acc_StockDetail asd 
where asd.PROD_SUBCATEGORY=sub.OID and asd.PROD_DES=d.OID
and asd.Flag='Purchase' and CONVERT(date,asd.IDATTIME)<CONVERT(date,@todate))

,TillYesterdayPR_Qty=(select CONVERT(decimal(18,2),ISNULL(SUM(asd.Quantity),0)) from Acc_StockDetail asd 
where asd.PROD_SUBCATEGORY=sub.OID and asd.PROD_DES=d.OID
and asd.Flag='Purchase Return' and CONVERT(date,asd.IDATTIME)<CONVERT(date,@todate))

,TillYesterdayS_Qty=(select CONVERT(decimal(18,2),ISNULL(SUM(asd.Quantity),0)) from Acc_StockDetail asd 
where asd.PROD_SUBCATEGORY=sub.OID and asd.PROD_DES=d.OID
and asd.Flag='Sale' and CONVERT(date,asd.IDATTIME)<CONVERT(date,@todate))

,TillYesterdaySR_Qty=(select CONVERT(decimal(18,2),ISNULL(SUM(asd.Quantity),0)) from Acc_StockDetail asd 
where asd.PROD_SUBCATEGORY=sub.OID and asd.PROD_DES=d.OID
and asd.Flag='Sale Return' and CONVERT(date,asd.IDATTIME)<CONVERT(date,@todate))

,TillYesterdayPRMIS_Qty=(select CONVERT(decimal(18,2),ISNULL(SUM(asd.Quantity),0)) from Acc_StockDetail asd 
where asd.PROD_SUBCATEGORY=sub.OID and asd.PROD_DES=d.OID
and asd.Flag='Product Missing' and CONVERT(date,asd.IDATTIME)<CONVERT(date,@todate))


-------------cost
,TillYesterdayP_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(asd.Quantity*asd.CostPrice),0)) from Acc_StockDetail asd 
where asd.PROD_SUBCATEGORY=sub.OID and asd.PROD_DES=d.OID
and asd.Flag='Purchase' and CONVERT(date,asd.IDATTIME)<CONVERT(date,@todate))

,TillYesterdayPR_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(asd.Quantity*asd.CostPrice),0)) from Acc_StockDetail asd 
where asd.PROD_SUBCATEGORY=sub.OID and asd.PROD_DES=d.OID
and asd.Flag='Purchase Return' and CONVERT(date,asd.IDATTIME)<CONVERT(date,@todate))

,TillYesterdayS_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(asd.Quantity*asd.CostPrice),0)) from Acc_StockDetail asd 
where asd.PROD_SUBCATEGORY=sub.OID and asd.PROD_DES=d.OID
and asd.Flag='Sale' and CONVERT(date,asd.IDATTIME)<CONVERT(date,@todate))

,TillYesterdaySR_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(asd.Quantity*asd.CostPrice),0)) from Acc_StockDetail asd 
where asd.PROD_SUBCATEGORY =sub.OID  and asd.PROD_DES=d.OID
and asd.Flag='Sale Return' and CONVERT(date,asd.IDATTIME)<CONVERT(date,@todate))

,TillYesterdayPRPROT_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(asd.Quantity*asd.CostPrice),0)) from Acc_StockDetail asd 
where asd.PROD_SUBCATEGORY =sub.OID  and asd.PROD_DES=d.OID AND
asd.Flag='Price Protection' and CONVERT(date,asd.IDATTIME)< CONVERT(date,@todate))


,TillYesterdayPRMIS_Cost=(select CONVERT(decimal(18,2),ISNULL(SUM(asd.Quantity*asd.CostPrice),0)) from Acc_StockDetail asd 
where asd.PROD_SUBCATEGORY =sub.OID  and asd.PROD_DES=d.OID AND
asd.Flag='Product Missing' and CONVERT(date,asd.IDATTIME)< CONVERT(date,@todate))


from Description d
inner join SubCategory sub on sub.OID =d.SubCategoryID 
left join T_WGPG cat on cat.OID =sub.CategoryID 


where 1=1  {2}
)t )y  where y.Profit >'0' or y.Profit <'0'
", txtDateFrom2.Text,txtDateTo2.Text, param,Shop_id );
        DataTable dt = GetDataTableByQuery(sql);

        if (dt.Rows.Count > 0)
        {
            gvStockSummery.DataSource = null;
            gvStockSummery.DataSource = dt;
            gvStockSummery.DataBind();


            //color
            gvStockSummery.Columns[4].ItemStyle.BackColor = System.Drawing.Color.Violet;
            gvStockSummery.Columns[11].ItemStyle.BackColor = System.Drawing.Color.YellowGreen;//
            gvStockSummery.Columns[12].ItemStyle.BackColor = System.Drawing.Color.Violet;
            gvStockSummery.Columns[19].ItemStyle.BackColor = System.Drawing.Color.YellowGreen;//
            gvStockSummery.Columns[20].ItemStyle.BackColor = System.Drawing.Color.Bisque;


            //get Total
            decimal TotalOpeningStockQty = 0;
            decimal TotalClosingStockQty = 0;
            decimal TotalOpeningStockValue = 0;
            decimal TotalClosingStockValue = 0;
            decimal totalProfit = 0;

            for (int i = 0; i < gvStockSummery.Rows.Count; i++)
            {
                decimal OpeningStockQty = 0;
                decimal ClosingStockQty = 0;
                decimal OpeningStockValue = 0;
                decimal ClosingStockValue = 0;
                decimal Profit = 0;

                //OpeningStockQty,ClosingStockQty,OpeningStockValue,ClosingStockValue  
                if (decimal.TryParse(gvStockSummery.DataKeys[i].Values["OpeningStockQty"].ToString(), out OpeningStockQty)) { }
                if (decimal.TryParse(gvStockSummery.DataKeys[i].Values["ClosingStockQty"].ToString(), out ClosingStockQty)) { }
                if (decimal.TryParse(gvStockSummery.DataKeys[i].Values["OpeningStockValue"].ToString(), out OpeningStockValue)) { }
                if (decimal.TryParse(gvStockSummery.DataKeys[i].Values["ClosingStockValue"].ToString(), out ClosingStockValue)) { }
                if (decimal.TryParse(gvStockSummery.DataKeys[i].Values["Profit"].ToString(), out Profit)) { }

                TotalOpeningStockQty += OpeningStockQty;
                TotalClosingStockQty += ClosingStockQty;
                TotalOpeningStockValue += OpeningStockValue;
                TotalClosingStockValue += ClosingStockValue;
                totalProfit += Profit;
            }
            gvStockSummery.FooterRow.Cells[4].Text = TotalOpeningStockQty.ToString("0.00");
            gvStockSummery.FooterRow.Cells[11].Text = TotalClosingStockQty.ToString("0.00");
            gvStockSummery.FooterRow.Cells[12].Text = TotalOpeningStockValue.ToString("0.00");
            gvStockSummery.FooterRow.Cells[19].Text = TotalClosingStockValue.ToString("0.00");
            gvStockSummery.FooterRow.Cells[20].Text = totalProfit.ToString("0.00");

            
                lblTotalGiftAmount.Text = string.Format(@"Total Gift Amount : {0}", dt.Rows[0]["GiftAmount"]);
                lblTotalDiscountAmount.Text = string.Format(@"Total Discount Amount : {0}", dt.Rows[0]["DiscountAmount"]);
            
        }
        else
        {
            gvStockSummery.DataSource = null;
            gvStockSummery.DataBind();
        }

    }

    protected void gvStockSummery_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
    {
        gvStockSummery.PageIndex = e.NewPageIndex;

        LoadGridStockSummery();
    }


}// //