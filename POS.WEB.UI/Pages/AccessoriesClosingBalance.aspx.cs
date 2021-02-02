using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class Pages_AccessoriesClosingBalance : System.Web.UI.Page
{
    private string userID = string.Empty;
    private string userPassword = string.Empty;

    SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
    SqlCommand cmd;
    SqlDataAdapter da = new SqlDataAdapter();

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
        if (ddlSearchBranch.SelectedItem.Value.ToString() == string.Empty || ddlSearchBranch.SelectedItem.Value.ToString() == "0")
        {
            lblMessage.Text = "Please select a Branch";
        }
        else {
            lblMessage.Text = string.Empty;
            DataTable dt = new DataTable();            
            cmd = new SqlCommand("SPP_AccessoriesClosingBalance", dbConnect);
            cmd.Parameters.Add("@Branch", SqlDbType.VarChar, 100).Value = ddlSearchBranch.SelectedItem.Value.ToString();
            if (txtFromDate.Text == string.Empty)
            {
                cmd.Parameters.Add("@DateFrom", SqlDbType.Date).Value = DateTime.Today.Date;
            }
            else
            {
                cmd.Parameters.Add("@DateFrom", SqlDbType.Date).Value = txtFromDate.Text;
            }
            if (txtToDate.Text == string.Empty)
            {
                cmd.Parameters.Add("@DateTo", SqlDbType.Date).Value = DateTime.Today.Date;
            }
            else
            {
                cmd.Parameters.Add("@DateTo", SqlDbType.Date).Value = txtToDate.Text;
            }

            cmd.CommandType = CommandType.StoredProcedure;
            da = new SqlDataAdapter(cmd);
            da.SelectCommand.CommandTimeout = 300;
            da.Fill(dt);            
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        
    }



  
}