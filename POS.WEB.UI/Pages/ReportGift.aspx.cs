using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TalukderPOS.BusinessObjects;
using TalukderPOS.DAL;
using TalukderPOS.BLL;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Configuration;

public partial class Pages_ReportGift : System.Web.UI.Page
{
    private string userID = string.Empty;
    private string userPassword = string.Empty;
    SearchInvoiceForAdmin_BILL BILL = new SearchInvoiceForAdmin_BILL();
    private string Shop_id = string.Empty;
    CommonDAL DAL = new CommonDAL();

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
        }
    }


    protected void cmdProcess_Click(object sender, EventArgs e)
    {
        //param
        string param = string.Empty;
        if (!string.IsNullOrEmpty(txtFromDate.Text) && !string.IsNullOrEmpty(txtToDate.Text)) 
        {
            param += string.Format(@" and CONVERT(date,s.EDAT) between '{0}' and '{1}'",txtFromDate.Text,txtToDate.Text);
        }
        //query
        string query = string.Format(@"
select sd.InvoiceNo,ci.CustomerName,ci.MobileNo,ci.EmailAddress,ci.Address
,d.Description,d.SESPrice
,sd.DescriptionID ,ci.OID as CustomerID,CONVERT(date,s.EDAT) EDAT
from T_SALES_DTL sd 
inner join T_SALES_MST s on s.InvoiceNo=sd.InvoiceNo
inner join Description d on d.OID=sd.DescriptionID
inner join CustomerInformation ci on ci.InvoiceNo=s.InvoiceNo
where GiftAmount>0 {0}
", param);
        DataTable dt = DAL.LoadDataByQuery(query);
        //bind

        gvGift.DataSource = null;
        gvGift.DataSource = dt;
        gvGift.DataBind();
    }

}