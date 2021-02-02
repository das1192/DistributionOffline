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

public partial class Pages_frmChangePassword : System.Web.UI.Page
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

        bool isAuthenticate = CommonBinder.CheckPageAuthentication(System.Web.HttpContext.Current.Request.Url.AbsolutePath, userID);
        if (!isAuthenticate)
        {
            Response.Redirect("~/UnAuthorizedUser.aspx");
        }
        if (!Page.IsPostBack)
        {
            lblUserName.Text = Session["UserName"].ToString();
            lblCurrentPassword.Text = Session["Password"].ToString();
        }
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (txtNewPassword.Text != string.Empty)
        {
            lblMessage.Text = string.Empty;
            sql = "update [User] set [User].Password = @Password,[User].ConfirmPassword = @ConfirmPassword where [User].UserId = @UserId ";
            cmd = new SqlCommand(sql, dbConnect);
            cmd.Parameters.Add("@Password", SqlDbType.NVarChar, 50).Value = txtNewPassword.Text;
            cmd.Parameters.Add("@ConfirmPassword", SqlDbType.NVarChar, 50).Value = txtNewPassword.Text;
            cmd.Parameters.Add("@UserId", SqlDbType.NVarChar, 50).Value = userID;           
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
            lblMessage.Text = "Please type new password";
        }
    }



}


