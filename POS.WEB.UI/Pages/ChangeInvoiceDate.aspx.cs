using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Configuration;


public partial class Pages_ChangeInvoiceDate : System.Web.UI.Page
{
    private string userID = string.Empty;
    private string userPassword = string.Empty;

    SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
    SqlCommand cmd;    
    String sql;    


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
        //bool isAuthenticate = CommonBinder.CheckPageAuthentication(System.Web.HttpContext.Current.Request.Url.AbsolutePath, userID);

        //if (!isAuthenticate)
        //{
        //    Response.Redirect("~/UnAuthorizedUser.aspx");
        //}
        if (!Page.IsPostBack)
        {
            lblInvoiceNo.Text = Request.QueryString["InvoiceNo"];
            lblCurrentInvoiceDate.Text = Request.QueryString["InvoiceDate"];
        }
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (txtNewInvoiceDate.Text != string.Empty)
        {
            lblMessage.Text = string.Empty;
            if (lblInvoiceNo.Text != string.Empty)
            {
                lblMessage.Text = string.Empty;
                sql = "update T_SALES_MST set IDAT=@IDAT,EDAT=@EDAT where InvoiceNo=@InvoiceNo";
                cmd = new SqlCommand(sql, dbConnect);
                cmd.Parameters.Add("@InvoiceNo", SqlDbType.VarChar,100).Value = lblInvoiceNo.Text;
                cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = txtNewInvoiceDate.Text;
                cmd.Parameters.Add("@EDAT", SqlDbType.Date).Value = DateTime.Today.Date;
                try
                {
                    if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dbConnect.Close();
                }
                lblMessage.Text = "Saved Successfully";
            }
            else
            {
                lblMessage.Text = "Invoice No has not been selected";
            }
        }
        else {
            lblMessage.Text = "Please Select New Invoice Date";
        }

        
        
    }


}