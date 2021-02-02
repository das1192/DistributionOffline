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
using TalukderPOS.BusinessObjects;
using TalukderPOS.BLL;

public partial class Pages_AdjustOncreditOld : System.Web.UI.Page
{
    private string userID = string.Empty;
    private string userPassword = string.Empty;
    SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
    String sql;
    SqlCommand cmd;
    SqlDataAdapter da;
    T_SALES_DTLBLL BILL = new T_SALES_DTLBLL();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            userID = Session["UserID"].ToString();
            userPassword = Session["Password"].ToString();
            //BindList();
            if (userID == "")
            {
                Response.Redirect("~/frmLogin.aspx");
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
            BindList();
            lblMessage.Text = string.Empty;
        }

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        cmd = new SqlCommand("SPP_Invoice_List_Oncredit_Old", dbConnect);
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

    private void BindList()
    {
        DataTable dt = new DataTable();
        cmd = new SqlCommand("SPP_Invoice_List_Oncreditlatest", dbConnect);
        cmd.Parameters.Add("@StoreID", SqlDbType.VarChar, 100).Value = Session["StoreID"].ToString();
        cmd.CommandType = CommandType.StoredProcedure;
        da = new SqlDataAdapter(cmd);
        da.SelectCommand.CommandTimeout = 300;
        da.Fill(dt);
        gvT_Issue_REQUISITION_DTL.DataSource = dt;
        gvT_Issue_REQUISITION_DTL.DataBind();
    }

    protected void btnIssue_Click(object sender, EventArgs e)
    {
        lblMessage.Text = string.Empty;
        txtCustomerName_TextChanged(sender, e);
    }

    protected void adjust_oncredit(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
    {
        GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
        if (e.CommandName == "Adjust")
        {

            
            
            Label lblOID2 = (Label)row.Cells[0].FindControl("lblInvoiceNo");
            string invno = lblOID2.Text.ToString();
            T_SALES_DTL entity = BILL.GetSalesInformation(invno);
            entity.IUSER = userID;
            BILL.Adjust_Oncredit(entity);
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", " alert('Sucessful');", true);
            //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert", "alert('Successful');", true);
            Response.Redirect(Request.RawUrl, true);
            
          
        }
    }

    protected void txtCustomerName_TextChanged(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        cmd = new SqlCommand("SPP_Invoice_List_Oncredit", dbConnect);
        cmd.Parameters.Add("@StoreID", SqlDbType.VarChar, 100).Value = Session["StoreID"].ToString();

        if (txtCustomerName.Text != string.Empty)
        {
            cmd.Parameters.Add("@CustomerName", SqlDbType.VarChar, 100).Value = txtCustomerName.Text;
            cmd.Parameters.Add("@MobileNo", SqlDbType.VarChar, 100).Value = null;
            cmd.Parameters.Add("@InvoiceNo", SqlDbType.VarChar, 100).Value = null;
        }
        else if (txtMobileNo.Text != string.Empty)
        {
            cmd.Parameters.Add("@CustomerName", SqlDbType.VarChar, 100).Value = null;
            cmd.Parameters.Add("@MobileNo", SqlDbType.VarChar, 100).Value = txtMobileNo.Text;
            cmd.Parameters.Add("@InvoiceNo", SqlDbType.VarChar, 100).Value = null;

        }
        else if (txtInvoiceNo.Text != string.Empty)
        {
            cmd.Parameters.Add("@CustomerName", SqlDbType.VarChar, 100).Value = null;
            cmd.Parameters.Add("@MobileNo", SqlDbType.VarChar, 100).Value = null;
            cmd.Parameters.Add("@InvoiceNo", SqlDbType.VarChar, 100).Value = txtInvoiceNo.Text;
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
        cmd = new SqlCommand("SPP_GetInvoice", dbConnect);
        cmd.Parameters.Add("@InvoiceNo", SqlDbType.VarChar, 100).Value = ((System.Web.UI.WebControls.Label)gvT_Issue_REQUISITION_DTL.Rows[e.NewEditIndex].FindControl("lblInvoiceNo")).Text;
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