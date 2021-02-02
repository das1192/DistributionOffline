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


public partial class Pages_ReportAccJournal : System.Web.UI.Page
{
    private string userID = string.Empty;
    private string userPassword = string.Empty;
    SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
    String sql;
    SqlCommand cmd;
    SqlDataAdapter da;
    CommonDAL DAL = new CommonDAL();
    //TalukderPOS.DAL.CommonDAL DAL = new CommonDAL();

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
            DateTime date = DateTime.Now;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            dptFromDate.Text = firstDayOfMonth.ToString("dd MMM yyyy");
            dptToDate.Text = date.ToString("dd MMM yyyy");

            lblMessage.Text = string.Empty;
        }

    }

    protected string RenderPriority(object dbValue)
    {
        string strReturn = string.Empty;
        if (dbValue != null)
        {
            int intValue = Convert.ToInt16(dbValue);
            switch (intValue)
            {
                case 0:
                    strReturn = "N";
                    break;
                case 1:
                    strReturn = "Y";
                    break;
            }
        }
        return strReturn;
    }

    protected void gvJournal_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
    {
        gvJournal.PageIndex = e.NewPageIndex;
        LoadGridJournal();
    }

    protected void btnSearchPurchaseInvoice_Click(object sender, EventArgs e)
    {
        LoadGridJournal();
    }

    private void LoadGridJournal()
    {
        string strParam = string.Empty;

        var fromdate = Convert.ToDateTime(dptFromDate.Text).ToString("yyyy-MM-dd");
        var todate = Convert.ToDateTime(dptToDate.Text).ToString("yyyy-MM-dd");   //and CONVERT(date,j.IDATTIME)

        if (!string.IsNullOrEmpty(dptFromDate.Text) && !string.IsNullOrEmpty(dptToDate.Text))
        {
            strParam += string.Format(@" and CONVERT(date,j.IDATTIME) between '{0}' and '{1}'", fromdate, todate);
        }

        string sql = string.Format(@"
select Journal_OID, AccountID, Remarks, Branch, Customer_Name
,case   when ISNULL(j.Debit,0)=0 then '........'+j.Particular else j.Particular   end as  Particular
, RefferenceNumber, Debit, Credit
, IDAT, CONVERT(nvarchar(12), IDATTIME,106)IDATTIME

,case 
when j.Remarks='Product' 
then j.Narration + ' P: ' +
(select top 1 ds.Description from Description ds where ds.OID=j.AccountID)

when j.Remarks='Supplier' 
then j.Narration + ' S: ' +
(select top 1 v.Vendor_Name from Vendor v where v.OID=j.AccountID)

else j.Narration

end as Narration

from Acc_Journal j
where j.Branch='{1}' {0}
", strParam, Session["StoreID"].ToString());

        DataTable dt = DAL.LoadDataByQuery(sql);
        if (dt.Rows.Count > 0)
        {
            gvJournal.DataSource = null;
            gvJournal.DataSource = dt;
            gvJournal.DataBind();
        }
        else
        {
            gvJournal.DataSource = null;
            gvJournal.DataBind();

        }
    }
}