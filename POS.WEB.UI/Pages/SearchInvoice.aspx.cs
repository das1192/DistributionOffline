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
using System.Configuration;
using System.Data.SqlClient;
using TalukderPOS.DAL;
public partial class Pages_SearchInvoice : System.Web.UI.Page
{
    private string userID = string.Empty;
    private string userPassword = string.Empty;
    SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
    String sql;
    SqlCommand cmd;
    SqlDataAdapter da;
    private string ShopID = string.Empty;
    CommonDAL DAL = new CommonDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            userID = Session["UserID"].ToString();
            userPassword = Session["Password"].ToString();
            ShopID = Session["StoreID"].ToString();
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
            lblMessage.Text = string.Empty;
        }

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        cmd = new SqlCommand("SPP_Invoice_List", dbConnect);
        cmd.Parameters.Add("@StoreID", SqlDbType.VarChar, 100).Value = Session["StoreID"].ToString();

        if (txtDateFrom.Text != string.Empty & txtDateTo.Text != string.Empty)
        {
            lblMessage.Text = string.Empty;
            cmd.Parameters.Add("@DateFrom", SqlDbType.Date).Value = txtDateFrom.Text;
            cmd.Parameters.Add("@DateTo", SqlDbType.Date).Value = txtDateTo.Text;
            cmd.CommandType = CommandType.StoredProcedure;
            da = new SqlDataAdapter(cmd);
            da.SelectCommand.CommandTimeout = 300;
            da.Fill(dt);
        }
        else {
            lblMessage.Text = "Please Select Invoice Date";
        }
        gvT_Issue_REQUISITION_DTL.DataSource = dt;
        gvT_Issue_REQUISITION_DTL.DataBind();
    }



    protected void btnIssue_Click(object sender, EventArgs e)
    {
        lblMessage.Text = string.Empty;
        txtCustomerName_TextChanged(sender, e);
    }



    protected void txtCustomerName_TextChanged(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        cmd = new SqlCommand("SPP_Invoice_List_Search", dbConnect);
        cmd.Parameters.Add("@StoreID", SqlDbType.VarChar, 100).Value = Session["StoreID"].ToString();

        if (txtCustomerName.Text != string.Empty)
        {
            cmd.Parameters.Add("@CustomerName", SqlDbType.VarChar, 100).Value = txtCustomerName.Text;
            cmd.Parameters.Add("@MobileNo", SqlDbType.VarChar, 100).Value = null;
            cmd.Parameters.Add("@InvoiceNo", SqlDbType.VarChar, 100).Value = null;
            cmd.Parameters.Add("@IMENo", SqlDbType.VarChar, 100).Value = null;
        }
        else if (txtMobileNo.Text != string.Empty)
        {
            cmd.Parameters.Add("@CustomerName", SqlDbType.VarChar, 100).Value = null;
            cmd.Parameters.Add("@MobileNo", SqlDbType.VarChar, 100).Value = txtMobileNo.Text;
            cmd.Parameters.Add("@InvoiceNo", SqlDbType.VarChar, 100).Value = null;
            cmd.Parameters.Add("@IMENo", SqlDbType.VarChar, 100).Value = null;
        }
        else if (txtInvoiceNo.Text != string.Empty)
        {
            cmd.Parameters.Add("@CustomerName", SqlDbType.VarChar, 100).Value = null;
            cmd.Parameters.Add("@MobileNo", SqlDbType.VarChar, 100).Value = null;
            cmd.Parameters.Add("@InvoiceNo", SqlDbType.VarChar, 100).Value = txtInvoiceNo.Text;
            cmd.Parameters.Add("@IMENo", SqlDbType.VarChar, 100).Value = null;
        }
        else if (txtIMENoSearch.Text != string.Empty)
        {
            cmd.Parameters.Add("@CustomerName", SqlDbType.VarChar, 100).Value = null;
            cmd.Parameters.Add("@MobileNo", SqlDbType.VarChar, 100).Value = null;
            cmd.Parameters.Add("@InvoiceNo", SqlDbType.VarChar, 100).Value = null;
            cmd.Parameters.Add("@IMENo", SqlDbType.VarChar, 100).Value = txtIMENoSearch.Text.Trim();
        }

        cmd.CommandType = CommandType.StoredProcedure;
        da = new SqlDataAdapter(cmd);
        da.SelectCommand.CommandTimeout = 300;
        da.Fill(dt);
        gvT_Issue_REQUISITION_DTL.DataSource = dt;
        gvT_Issue_REQUISITION_DTL.DataBind();

    }

    protected void gvT_Issue_REQUISITION_DTL_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
    {
        DataTable dtsales = new DataTable();
        cmd = new SqlCommand("SPP_GetInvoice_2", dbConnect);
        cmd.Parameters.Add("@InvoiceNo", SqlDbType.VarChar, 100).Value = ((System.Web.UI.WebControls.Label)gvT_Issue_REQUISITION_DTL.Rows[e.NewEditIndex].FindControl("lblInvoiceNo")).Text;
        cmd.CommandType = CommandType.StoredProcedure;
        da = new SqlDataAdapter(cmd);
        da.SelectCommand.CommandTimeout = 300;
        da.Fill(dtsales);


        Session["dtsales"] = dtsales;
        string sqlhalfinvoice = string.Format(@"select * from Half_Invoice where SHOP_ID='{0}' ", ShopID);
        DataTable dthalf = DAL.LoadDataByQuery(sqlhalfinvoice);
        if (dthalf.Rows.Count > 0)
        {
            if (Session["StoreID"].ToString() == "5")
            {
                Session["ReportPath"] = "~/Reports/rptInvoiceHalfDristy.rpt";
            }
            else if (Session["StoreID"].ToString() == "54" || Session["StoreID"].ToString() == "57" || Session["StoreID"].ToString() == "67")
            {
                Session["ReportPath"] = "~/Reports/rptInvoiceHalfAponJhon.rpt";
            }
            else if (Session["StoreID"].ToString() == "71")
            {
                Session["ReportPath"] = "~/Reports/rptInvoicePOSPrinter.rpt";
            }
                /*
            else if (Session["StoreID"].ToString() == "81")
            {
                Session["ReportPath"] = "~/Reports/rptInvoiceHalfHalimaIT.rpt";
            }*/
            else
            {
                Session["ReportPath"] = "~/Reports/rptInvoiceHalf.rpt";
            }
        }



        else { Session["ReportPath"] = "~/Reports/rptInvoice_NewA4.rpt"; }

        string webUrl = "../Reports/ReportView.aspx";        
        ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenWindow", "window.open('" + webUrl + "','_newtab');", true);
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


   




}