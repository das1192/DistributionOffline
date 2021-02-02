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
using System.Configuration;
using System.Data.SqlClient;
using TalukderPOS.BusinessObjects;
using TalukderPOS.BLL;

public partial class Pages_AdjustOncredit : System.Web.UI.Page
{
    private string userID = string.Empty;
    private string userPassword = string.Empty;
    private string Shop_id = string.Empty;
    SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
    String sql;
    SqlCommand cmd;
    SqlDataAdapter da;
    T_SALES_DTLBLL BILL = new T_SALES_DTLBLL();
    CommonDAL DAL = new CommonDAL();

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
            LoadDDLCustomer();
        }

    }

    private void LoadDDLCustomer()
    {
        string sql = string.Format(@"SELECT c.ID,c.Name FROM Customers c where c.Branch ='{0}' order by c.Name", Shop_id);
        DataTable dt = DAL.LoadDataByQuery(sql);
        if (dt.Rows.Count > 0)
        {
            txtCustomerName.DataSource = dt;
            txtCustomerName.DataTextField = "Name";
            txtCustomerName.DataValueField = "ID";
            txtCustomerName.DataBind();


            txtCustomerName.Items.Insert(0, new ListItem("Customer Name", string.Empty));

        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();


        if (txtDateFrom.Text != string.Empty & txtDateTo.Text != string.Empty)
        {
            string spsql = string.Format(@"
Exec dbo.SPP_Invoice_List_Oncredit @DateFrom='{0}',@DateTo='{1}',@StoreID='{2}',@CustomerName='',@MobileNo='',@InvoiceNo=''
", txtDateFrom.Text, txtDateTo.Text, Session["StoreID"].ToString());

            dt = DAL.LoadDataByQuery(spsql);
        }
        else
        {
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




        // newly added to find out due

    }






    protected void btnIssue_Click(object sender, EventArgs e)
    {
        lblMessage.Text = string.Empty;
        txtCustomerName_TextChanged(sender, e);
    }



    protected void txtCustomerName_TextChanged(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        cmd = new SqlCommand("SPP_Invoice_List_Oncredit", dbConnect);
        cmd.Parameters.Add("@StoreID", SqlDbType.VarChar, 100).Value = Session["StoreID"].ToString();

        if (txtCustomerName.SelectedItem.Text != "Customer Name")
        {
            cmd.Parameters.Add("@CustomerName", SqlDbType.VarChar, 100).Value = txtCustomerName.SelectedItem.Text;
            cmd.Parameters.Add("@MobileNo", SqlDbType.VarChar, 100).Value = null;
            cmd.Parameters.Add("@InvoiceNo", SqlDbType.VarChar, 100).Value = null;
            cmd.Parameters.Add("@NetAmount", SqlDbType.VarChar, 100).Value = null;
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

    } // end of txtCustomerName_TextChanged

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

    // btn to preview pdf report
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        int n = ((sender as Button).NamingContainer as GridViewRow).RowIndex;
        string InvoiceNo = gvT_Issue_REQUISITION_DTL.DataKeys[n].Values["InvoiceNo"].ToString();
        DataTable dtsales = new DataTable();
        cmd = new SqlCommand("SPP_GetInvoice", dbConnect);
        cmd.Parameters.Add("@InvoiceNo", SqlDbType.VarChar, 100).Value = InvoiceNo.ToString();
        cmd.CommandType = CommandType.StoredProcedure;
        da = new SqlDataAdapter(cmd);
        da.SelectCommand.CommandTimeout = 300;
        da.Fill(dtsales);


        Session["dtsales"] = dtsales;
        Session["ReportPath"] = "~/Reports/rptInvoice.rpt";

        string webUrl = "../Reports/ReportView.aspx";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenWindow", "window.open('" + webUrl + "','_newtab');", true);



    } //  btn to preview pdf report


    // Final button to adjust
    protected void btnAdjust_Click(object sender, EventArgs e)
    {



        int n = ((sender as Button).NamingContainer as GridViewRow).RowIndex;

        //
        string InvoiceNo = gvT_Issue_REQUISITION_DTL.DataKeys[n].Values["InvoiceNo"].ToString();
        string strReceiveAmount = ((TextBox)gvT_Issue_REQUISITION_DTL.Rows[n].Cells[9].FindControl("DuePay")).Text.ToString();
        string strTotalDue = gvT_Issue_REQUISITION_DTL.DataKeys[n].Values["Due"].ToString();


        decimal DueAmount = 0;
        decimal ReceiveAmount = 0;
        if (decimal.TryParse(strTotalDue, out DueAmount)) { }
        if (decimal.TryParse(strReceiveAmount, out ReceiveAmount)) { }

        if (ReceiveAmount > DueAmount || ReceiveAmount == 0)
        {
            lblMessage.Text = "Check the amount!";
            lblMessage.ForeColor = Color.Red;
            lblMessage.BackColor = Color.Yellow;
            ////lblMessage.BorderStyle = BorderStyle.Solid;
            lblMessage.BorderColor = Color.Black;



            return;
        }
        else
        {

            T_SALES_DTL entity = BILL.GetSalesInformation(InvoiceNo);
            entity.IUSER = userID;

            entity.ReceiveAmount = Convert.ToString(Math.Round(Convert.ToDouble(entity.ReceiveAmount) + Convert.ToDouble(ReceiveAmount)));
            entity.RemainingAmount = Convert.ToString(Math.Round(Convert.ToDouble(entity.NetAmount) - Convert.ToDouble(entity.ReceiveAmount)));
            entity.CashPaid = Convert.ToString(ReceiveAmount);


            BILL.Adjust_Oncredit2(entity);

            Response.Redirect(Request.RawUrl, true);




        }


    } // end  of btnAdjust_Click





}// end of partial class