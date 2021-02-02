using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using TalukderPOS.BLL;
using TalukderPOS.BusinessObjects;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using System.Web.Services;

using System.Drawing;
using TalukderPOS.DAL;


public partial class DashBoard : System.Web.UI.Page
{

    //private string userID = "";
    //private string password = "";
    private string userID = "";
    private string userpassword = "";
    private string userFullName = "";
    private string UserLocation = "";
    private string pageid = "";
    string connStr = ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString;
    CommonDAL DAL = new CommonDAL();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            userID = Session["UserID"].ToString();
            userpassword = Session["Password"].ToString();
            userFullName = Session["UserFullName"].ToString();
            UserLocation = Session["CCOM_NAME"].ToString();
            string storeid = HttpContext.Current.Session["StoreID"].ToString();


        }
        catch
        {
            Response.Redirect("~/Default.aspx");
        }

        if (!IsPostBack)
        {

            Getcurrentstock();
            GetSalesStatistics();
            getchart();
            CurrentCashBalance();

        }
    }

    private void CurrentCashBalance()
    {
        // Yeasin //
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
", Session["StoreID"].ToString());
        DataTable dt = DAL.LoadDataByQuery(sql);
        if (dt.Rows.Count > 0)
        {
            gvCashBalance.DataSource = null;
            gvCashBalance.DataSource = dt;
            gvCashBalance.DataBind();
        }
        // Yeasin //
    }
    private void Getcurrentstock()
    {
        DataTable table = new DataTable();
        DataSet ds1 = new DataSet();
        DataTable dt1 = new DataTable();
        using (SqlConnection conn = new SqlConnection(connStr))
        {
            string storeid = HttpContext.Current.Session["StoreID"].ToString();
            string sql = "select TOP(10) StoreMasterStock.PROD_WGPG,T_WGPG.WGPG_NAME as PROD_WGPGNAME,SUM(StoreMasterStock.Quantity-StoreMasterStock.SaleQuantity) as sum1 from StoreMasterStock left join T_WGPG on StoreMasterStock.PROD_WGPG=T_WGPG.OID inner join Description on StoreMasterStock.PROD_DES=Description.OID where StoreMasterStock.Branch='" + storeid + "' AND Description.Active='1' group by T_WGPG.WGPG_NAME,StoreMasterStock.PROD_WGPG order by sum(StoreMasterStock.Quantity-StoreMasterStock.SaleQuantity) desc";
            SqlDataAdapter da1 = new SqlDataAdapter(sql, conn);
            da1.Fill(ds1);
            dt1 = ds1.Tables[0];
            StringBuilder tableOutput = new StringBuilder();
            tableOutput.Append("<div class='table-responsive'><table class='table'><tbody>");

            string abc = string.Format(@"haloo {0}", storeid);
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                tableOutput.Append("<tr><td>" + dt1.Rows[i]["PROD_WGPGNAME"].ToString());
                tableOutput.Append("</td>");
                tableOutput.Append("<td><a href='#' class='announce' data-toggle='modal' data-id='");
                string c1 = dt1.Rows[i]["PROD_WGPG"].ToString();
                tableOutput.Append(c1);
                tableOutput.Append("' data-cathead='");
                string c2 = dt1.Rows[i]["PROD_WGPGNAME"].ToString();
                tableOutput.Append(c2);
                tableOutput.Append("' ><span style='font-weight:bold'>");
                tableOutput.Append(dt1.Rows[i]["sum1"].ToString());

                tableOutput.Append("</span></a></td></tr>");
            }
            tableOutput.Append("</tbody></table></div>");
            tempHtmlTable2.Text = tableOutput.ToString();
        }
    }

    protected void getchart()
    {

        DataTable table = new DataTable();
        DataSet ds1 = new DataSet();
        DataTable dt1 = new DataTable();
        using (SqlConnection conn = new SqlConnection(connStr))
        {
            string storeid = HttpContext.Current.Session["StoreID"].ToString();
            string sql = "select TOP(10) T_WGPG.WGPG_NAME as PROD_WGPG,SubCategory.SubCategoryName as PROD_SUBCATEGORY,Description.Description as Description,StoreMasterStock.SaleQuantity as SaleQuantity  from StoreMasterStock left join Description on StoreMasterStock.PROD_DES=Description.OID left join SubCategory on Description.SubCategoryID=SubCategory.OID left join T_WGPG on SubCategory.CategoryID=T_WGPG.OID left join ShopInfo on StoreMasterStock.Branch=ShopInfo.OID where StoreMasterStock.Branch='" + storeid + "' AND StoreMasterStock.SaleQuantity>0 order by StoreMasterStock.SaleQuantity desc";
            sql = string.Format(@"Select Top (10) de.OID,de.Description,count(sd.Barcode) 'SaleQuantity' From T_SALES_DTL sd
Inner Join T_SALES_MST M on sd.InvoiceNo = M.InvoiceNo And M.DropStatus=0
Inner Join StoreMasterStock sm on sm.PROD_DES= sd.DescriptionID
Inner Join [Description] de on de.OID = sd.DescriptionID
Where M.StoreID='{0}' And MONTH(M.IDAT)= MONTH(GetDate()) And Year(M.IDAT) = YEAR(GETDATE())
Group By de.OID,de.Description
Order By  count(sd.Barcode) desc", storeid);
            SqlDataAdapter da1 = new SqlDataAdapter(sql, conn);
            da1.Fill(ds1);
            dt1 = ds1.Tables[0];
            StringBuilder tableOutput = new StringBuilder();
            tableOutput.Append("<div class='table-responsive'><table class='table'><tbody><tr><th>Model</th><th>Quantity</th></tr>");


            for (int i = 0; i < dt1.Rows.Count; i++)
            {


                tableOutput.Append("<tr><td>" + dt1.Rows[i]["Description"].ToString());
                tableOutput.Append("</td>");
                tableOutput.Append("<td>" + dt1.Rows[i]["SaleQuantity"].ToString());
                tableOutput.Append("</td></tr>");

            }


            tableOutput.Append("</tbody></table></div>");
            tempHtmlTable4.Text = tableOutput.ToString();





        }




    }



    [WebMethod]
    public static string GetInvoice(string id)
    {
        //SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
        //String sql;
        //SqlCommand cmd;
        //SqlDataAdapter da;

        //DataTable dtsales = new DataTable();
        //cmd = new SqlCommand("SPP_GetInvoice", dbConnect);
        //cmd.Parameters.Add("@InvoiceNo", SqlDbType.VarChar, 100).Value = id;
        //cmd.CommandType = CommandType.StoredProcedure;
        //da = new SqlDataAdapter(cmd);
        //da.SelectCommand.CommandTimeout = 300;
        //da.Fill(dtsales);

        //HttpContext.Current.Session["dtsales"] = dtsales;
        //HttpContext.Current.Session["ReportPath"] = "~/Reports/rptInvoice.rpt";
        ////Session["dtsales"] = dtsales;
        ////Session["ReportPath"] = "~/Reports/rptInvoice.rpt";

        //string webUrl = "../Reports/ReportView.aspx";
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenWindow", "window.open('" + webUrl + "','_newtab');", true);
        return "Hello";
    }




    private void GetSalesStatistics()
    {

        DataTable table = new DataTable();
        DataSet ds1 = new DataSet();
        DataTable dt1 = new DataTable();
        using (SqlConnection conn = new SqlConnection(connStr))
        {
            string storeid = HttpContext.Current.Session["StoreID"].ToString();
            string sql = "SELECT TotalSales,SalesThisMonth,SalesToday FROM SalesStatistics where SalesStatistics.StoreID='" + storeid + "' ";
            SqlDataAdapter da1 = new SqlDataAdapter(sql, conn);
            da1.Fill(ds1);
            dt1 = ds1.Tables[0];
            StringBuilder tableOutput = new StringBuilder();
            tableOutput.Append("<div class='table-responsive'><table class='table'><tbody><tr><th>Today's Sale</th><th>Current Month</th></tr>");


            for (int i = 0; i < dt1.Rows.Count; i++)
            {




                tableOutput.Append("<tr><td><a href='#' class='announce5' data-toggle='modal'>" + dt1.Rows[i]["SalesToday"].ToString());
                tableOutput.Append("</a></td>");

                tableOutput.Append("<td><a href='#' class='announce2' data-toggle='modal'");

                tableOutput.Append("' >");
                tableOutput.Append(dt1.Rows[i]["SalesThisMonth"].ToString());

                tableOutput.Append("</a>");
                //tableOutput.Append("<td>" + dt1.Rows[i]["TotalSales"].ToString());
                tableOutput.Append("</td></tr>");

            }


            tableOutput.Append("</tbody></table></div>");
            tempHtmlTable3.Text = tableOutput.ToString();





        }

    }

    [WebMethod]
    public static string SendMessage(string id)
    {
        DataTable table = new DataTable();
        DataSet ds1 = new DataSet();
        DataTable dt1 = new DataTable();
        string connStr = ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(connStr))
        {
            string storeid = HttpContext.Current.Session["StoreID"].ToString();
            //string sql = "select TOP(500) SubCategory.SubCategoryName,Description.Description,SUM(StoreMasterStock.Quantity-StoreMasterStock.SaleQuantity) as sum1,StoreMasterStock.SalePrice from StoreMasterStock inner join Description on StoreMasterStock.PROD_DES=Description.OID inner join SubCategory on Description.SubCategoryID=SubCategory.OID inner join T_WGPG on SubCategory.CategoryID=T_WGPG.OID where StoreMasterStock.Branch=" + storeid + " AND StoreMasterStock.PROD_WGPG=" + id + " group by SubCategory.SubCategoryName,Description.Description,StoreMasterStock.SalePrice";

            //string sql = "select * from (select TOP(500) SubCategory.SubCategoryName,Description.Description,SUM(StoreMasterStock.Quantity-StoreMasterStock.SaleQuantity) as sum1,StoreMasterStock.SalePrice from StoreMasterStock inner join Description on StoreMasterStock.PROD_DES=Description.OID inner join SubCategory on Description.SubCategoryID=SubCategory.OID inner join T_WGPG on SubCategory.CategoryID=T_WGPG.OID where StoreMasterStock.Branch=" + storeid + " AND StoreMasterStock.PROD_WGPG=" + id + " group by SubCategory.SubCategoryName,Description.Description,StoreMasterStock.SalePrice)s";
            string sql = "select * from (select TOP(500) SubCategory.SubCategoryName,Description.Description,SUM(StoreMasterStock.Quantity-StoreMasterStock.SaleQuantity) as sum1,StoreMasterStock.SalePrice from StoreMasterStock inner join Description on StoreMasterStock.PROD_DES=Description.OID inner join SubCategory on Description.SubCategoryID=SubCategory.OID inner join T_WGPG on SubCategory.CategoryID=T_WGPG.OID where StoreMasterStock.Branch=" + storeid + " AND StoreMasterStock.PROD_WGPG=" + id + " AND Description.Active='1' group by SubCategory.SubCategoryName,Description.Description,StoreMasterStock.SalePrice)s";
            SqlDataAdapter da1 = new SqlDataAdapter(sql, conn);
            da1.Fill(ds1);
            dt1 = ds1.Tables[0];
            StringBuilder tableOutput2 = new StringBuilder();
            tableOutput2.Append("<div class='table-responsive'><table class='table'><tbody><tr><th>Brand</th><th>Model</th><th>Stock Quantity</th><th>MRP</th></tr>");


            for (int i = 0; i < dt1.Rows.Count; i++)
            {


                // sadiq 170918
                //to set font color where qty 0

                if (dt1.Rows[i]["sum1"].ToString() != "0")
                {

                    tableOutput2.Append("<tr><td>" + dt1.Rows[i]["SubCategoryName"].ToString());
                    tableOutput2.Append("</td>");
                    tableOutput2.Append("<td>" + dt1.Rows[i]["Description"].ToString());
                    tableOutput2.Append("</td>");
                    tableOutput2.Append("<td>" + dt1.Rows[i]["sum1"].ToString());
                    tableOutput2.Append("</td>");
                    tableOutput2.Append("<td>" + dt1.Rows[i]["SalePrice"].ToString());
                    tableOutput2.Append("</td></tr>");
                }
                else
                {
                    tableOutput2.Append("<tr style='color:Red'><td>" + dt1.Rows[i]["SubCategoryName"].ToString());
                    tableOutput2.Append("</td>");
                    tableOutput2.Append("<td>" + dt1.Rows[i]["Description"].ToString());
                    tableOutput2.Append("</td>");
                    tableOutput2.Append("<td>" + dt1.Rows[i]["sum1"].ToString());
                    tableOutput2.Append("</td>");
                    tableOutput2.Append("<td>" + dt1.Rows[i]["SalePrice"].ToString());
                    tableOutput2.Append("</td></tr>");
                }

            }


            tableOutput2.Append("</tbody></table></div>");
            return tableOutput2.ToString();
        }
    }

    [WebMethod]
    public static string Totalmonthsale()
    {
        DataTable table = new DataTable();
        DataSet ds1 = new DataSet();
        DataTable dt1 = new DataTable();
        string connStr = ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(connStr))
        {
            string storeid = HttpContext.Current.Session["StoreID"].ToString();
            string ndate = DateTime.Now.ToString("yyyy-MM");
            string sql = "SELECT sum([NetAmount]) as netsum,convert(CHAR(10), IDAT, 120) as NEWDATE FROM T_SALES_MST where DiscountReferencedBy NOT LIKE '2' AND DropStatus NOT LIKE '1' AND StoreID='" + storeid + "' AND IDAT LIKE '" + ndate + "%' group by IDAT order by IDAT desc";
            SqlDataAdapter da1 = new SqlDataAdapter(sql, conn);
            da1.Fill(ds1);
            dt1 = ds1.Tables[0];
            StringBuilder tableOutput2 = new StringBuilder();
            tableOutput2.Append("<div class='table-responsive'><table class='table'><tbody><tr><th>Date</th><th>Net Amount</th></tr>");


            for (int i = 0; i < dt1.Rows.Count; i++)
            {




                tableOutput2.Append("<tr><td>" + dt1.Rows[i]["NEWDATE"].ToString());
                tableOutput2.Append("</td>");
                tableOutput2.Append("<td>" + dt1.Rows[i]["netsum"].ToString());
                tableOutput2.Append("</td></tr>");



            }


            tableOutput2.Append("</tbody></table></div>");
            return tableOutput2.ToString();
        }
    }

    [WebMethod]
    public static string Totaldailysale()
    {
        DataTable table = new DataTable();
        DataSet ds1 = new DataSet();
        DataTable dt1 = new DataTable();
        string connStr = ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(connStr))
        {
            string storeid = HttpContext.Current.Session["StoreID"].ToString();
            string ndate = DateTime.Now.ToString("yyyy-MM-dd");
            string sql = "SELECT SlNo,InvoiceNo,NetAmount FROM T_SALES_MST where DiscountReferencedBy NOT LIKE '2' AND DropStatus NOT LIKE '1' AND StoreID='" + storeid + "' AND IDAT LIKE '" + ndate + "%' order by SlNo desc";
            SqlDataAdapter da1 = new SqlDataAdapter(sql, conn);
            da1.Fill(ds1);
            dt1 = ds1.Tables[0];
            StringBuilder tableOutput2 = new StringBuilder();
            tableOutput2.Append("<div class='table-responsive'><table class='table'><tbody><tr><th>Invoice Number</th><th>Amount</th></tr>");


            for (int i = 0; i < dt1.Rows.Count; i++)
            {



                tableOutput2.Append("<tr>");
                tableOutput2.Append("<tr><td><a href='pages/GenerateInvoice.aspx?menuhead=104&val=" + dt1.Rows[i]["InvoiceNo"].ToString() + "' target='_blank' runat='server' id='anchor1'>" + dt1.Rows[i]["InvoiceNo"].ToString() + " </a></td>");

                //tableOutput2.Append("<td><a href='#' id='submit_btn' role='button' runat='server' class='btn' data-id='");

                //tableOutput2.Append("<td><asp:button ID='btnMethod' runat='server' OnClick='btncheck_Click' class='btn btn-success' Text='"); 

                //string c1 = dt1.Rows[i]["InvoiceNo"].ToString();
                //tableOutput2.Append(c1);

                //tableOutput2.Append("'></asp:button>");
                //tableOutput2.Append("</td>");
                //tableOutput2.Append("' ><span style='font-weight:bold'>");
                //tableOutput2.Append(dt1.Rows[i]["InvoiceNo"].ToString());

                //tableOutput2.Append("</span></a></td>");
                tableOutput2.Append("<td>" + dt1.Rows[i]["NetAmount"].ToString());
                tableOutput2.Append("</td></tr>");



            }


            tableOutput2.Append("</tbody></table></div>");
            return tableOutput2.ToString();
        }
    }



}
