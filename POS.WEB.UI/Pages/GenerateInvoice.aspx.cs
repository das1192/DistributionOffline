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
public partial class Pages_GenerateInvoice : System.Web.UI.Page
{
    private string userID = string.Empty;
    private string userPassword = string.Empty;
    SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
    String sql;
    private string ShopID = string.Empty;
    SqlCommand cmd;
    SqlDataAdapter da;
    CommonDAL DAL = new CommonDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            userID = Session["UserID"].ToString();
            userPassword = Session["Password"].ToString();
            GenerateNewInvoice();
            
        }
        catch
        {
            Response.Redirect("~/frmLogin.aspx");
        }
       
        if (!Page.IsPostBack)
        {
            
        }

    }



    protected void GenerateNewInvoice()
    {
        string val = "";
        val = Request.QueryString["val"].ToString();
        DataTable dtsales = new DataTable();
        cmd = new SqlCommand("SPP_GetInvoice_2", dbConnect);
        cmd.Parameters.Add("@InvoiceNo", SqlDbType.VarChar, 100).Value = val;
        cmd.CommandType = CommandType.StoredProcedure;
        da = new SqlDataAdapter(cmd);
        da.SelectCommand.CommandTimeout = 300;
        da.Fill(dtsales);

        Session["dtsales"] = dtsales;
        //Session["ReportPath"] = "~/Reports/rptInvoice.rpt";
        //if (Session["StoreID"].ToString() == "59" || Session["StoreID"].ToString() == "60")
        //{
        //    Session["ReportPath"] = "~/Reports/rptInvoiceHalf.rpt";
        //}
        //else { Session["ReportPath"] = "~/Reports/rptInvoice.rpt"; }
//        ShopID = Session["StoreID"].ToString();
//        string sqlhalfinvoice = string.Format(@"
//select * from Half_Invoice where SHOP_ID='{0}'
//
//", ShopID);
//        DataTable dthalf = DAL.LoadDataByQuery(sqlhalfinvoice);
//        if (dthalf.Rows.Count > 0)
//        { 
//            if (Session["StoreID"].ToString() == "5")
//            {
//                Session["ReportPath"] = "~/Reports/rptInvoiceHalfDristy.rpt";
//            }
//            else if (Session["StoreID"].ToString() == "54" || Session["StoreID"].ToString() == "57" || Session["StoreID"].ToString() == "67")
//            {
//                Session["ReportPath"] = "~/Reports/rptInvoiceHalfAponJhon.rpt";
//            }
//            else if (Session["StoreID"].ToString() == "71")
//            {
//                Session["ReportPath"] = "~/Reports/rptInvoicePOSPrinter.rpt";
//            }
//            else
//            {
//                Session["ReportPath"] = "~/Reports/rptInvoiceHalf.rpt";
//            }
//            //Session["ReportPath"] = "~/Reports/rptInvoiceHalf.rpt";
//        }

        //else { 
        Session["ReportPath"] = "~/Reports/rptInvoice_NewA4.rpt";
        //Session["ReportPath"] = "~/Reports/rptInvoice.rpt"; 
        //}







        string webUrl = "../Reports/ReportView.aspx"; 
     
        ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenWindow", "window.open('" + webUrl + "');", true);
        ClientScript.RegisterStartupScript(typeof(Page), "closePage", "window.close();", true);
    }
    


}