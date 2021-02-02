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

public partial class DiscountReport : System.Web.UI.Page
{
    private string userID = string.Empty;
    private string userPassword = string.Empty;
    SalesReport_BILL BILL = new SalesReport_BILL();
    DataTable dt;

    SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
    SqlCommand cmd;
    SqlDataAdapter da;

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
        //prepare search item
        entity.Branch = ddlSearchBranch.SelectedItem.Value.ToString();
        entity.PROD_WGPG = ddlSearchProductCategory.SelectedItem.Value.ToString();
        entity.PROD_SUBCATEGORY = ddlSearchSubCategory.SelectedItem.Value.ToString();
        entity.PROD_DES = ddlSearchDescription.SelectedItem.Value.ToString();
        entity.DiscountType = ddlSearchRefType.SelectedItem.Value.ToString();
        entity.Reference = ddlSearchRef.SelectedItem.Value.ToString();        
        entity.FromDate = txtFromDate.Text;
        entity.ToDate = txtToDate.Text;
        // calling function of TalukderPOS.BILL.DiscountReportByModel(T_PROD entity)
        dt = BILL.DiscountReportByModel(entity);
        GridView1.DataSource = dt;
        GridView1.DataBind();

        var sum = dt.AsEnumerable().Sum(dr => dr.Field<Int64>("DiscountAmount"));
        lblTotal.Text = "Total:" + sum.ToString();
        Session["GridData"] = dt;
    }

    protected void cmdPreview_Click(object sender, EventArgs e)
    {        
        string webUrl = string.Empty;
        DataTable dt = (DataTable)Session["GridData"];
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    if (rbtPDF.Checked == true)
                    {
                        Session["dtsales"] = dt;
                        Session["ReportPath"] = "~/Reports/rptDiscountClaim.rpt";
                        webUrl = "../Reports/ReportView.aspx";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenWindow", "window.open('" + webUrl + "','_newtab');", true);
                    }
                    else if (rbtExcel.Checked == true)
                    {
                        Session["dtsales"] = dt;
                        Session["ReportPath"] = "~/Reports/ExcelDiscountClaim.rpt";
                        webUrl = "../Reports/ReportsViewer.aspx";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenWindow", "window.open('" + webUrl + "','_newtab');", true);
                    }
                }          
            }
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

    private void getInvoice(string invoiceno)
    {
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