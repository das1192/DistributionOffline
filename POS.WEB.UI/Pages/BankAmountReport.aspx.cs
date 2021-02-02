using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TalukderPOS.BusinessObjects;
using TalukderPOS.BLL;
using System.IO;
using System.Text;

public partial class Pages_BankAccountReportAdmin : System.Web.UI.Page
{
    private string userID = string.Empty;
    private string userPassword = string.Empty;
    BankAccountReport_BILL BILL = new BankAccountReport_BILL();
    
    SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);    
    SqlCommand cmd;
    SqlDataAdapter da;
    DataTable dt = null;

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
        }
    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {        
        T_PROD entity = new T_PROD();
        entity.PaymentMode = ddlSearchPayment.SelectedItem.Value.ToString();
        entity.BankName = ddlSearchBank.SelectedItem.Value.ToString();
        entity.Branch = ddlSearchBranch.SelectedItem.Value.ToString();
        
        entity.FromDate = txtFromDate.Text;
        entity.ToDate = txtToDate.Text;
        dt = BILL.BankAccountReport(entity);

        if (dt != null && dt.Rows.Count > 0) {
            GridView1.DataSource = dt;
            GridView1.DataBind();
            var sum = dt.AsEnumerable().Sum(dr => dr.Field<Decimal>("amount"));
            lblTotal.Text = "Total:" + sum.ToString();
        }        
        
    }


    protected void cmdExportToExcel_Click(object sender, EventArgs e)
    {
        T_PROD entity = new T_PROD();
        entity.PaymentMode = ddlSearchPayment.SelectedItem.Value.ToString();
        entity.BankName = ddlSearchBank.SelectedItem.Value.ToString();
        entity.Branch = ddlSearchBranch.SelectedItem.Value.ToString();
        
        entity.FromDate = txtFromDate.Text;
        entity.ToDate = txtToDate.Text;
        dt = BILL.BankAccountReport(entity);

        if (dt != null && dt.Rows.Count > 0) {
            DatatableToExcel(dt);
        }

    }

    private void DatatableToExcel(DataTable dt)
    {
        string attachment = "attachment; filename=ExcelReport.xls";
        Response.ClearContent();
        Response.AddHeader("content-disposition", attachment);
        Response.ContentType = "application/vnd.ms-excel";
        string tab = "";
        foreach (DataColumn dc in dt.Columns)
        {
            Response.Write(tab + dc.ColumnName);
            tab = "\t";
        }
        Response.Write("\n");
        int i;
        foreach (DataRow dr in dt.Rows)
        {
            tab = "";
            for (i = 0; i < dt.Columns.Count; i++)
            {
                Response.Write(tab + dr[i].ToString());
                tab = "\t";
            }
            Response.Write("\n");
        }
        Response.End();
    }

      
    protected void grdCustomPagging_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "VIEW")
        {
            LinkButton lnkView = (LinkButton)e.CommandSource;
            string InvoiceNo = lnkView.CommandArgument;
            getInvoice(InvoiceNo);
        }
    }

    private void getInvoice(string invoiceno){
        DataTable dtsales = new DataTable();
        cmd = new SqlCommand("SPP_GetInvoice", dbConnect);
        cmd.Parameters.Add("@InvoiceNo", SqlDbType.VarChar, 100).Value = invoiceno;
        cmd.CommandType = CommandType.StoredProcedure;
        da = new SqlDataAdapter(cmd);
        da.SelectCommand.CommandTimeout = 300;
        da.Fill(dtsales);

        Session["dtsales"] = dtsales;
        Session["ReportPath"] = "~/Reports/rptInvoice.rpt";

        string webUrl = "../Reports/ReportView.aspx";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenWindow", "window.open('" + webUrl + "','_newtab');", true);
    }


}