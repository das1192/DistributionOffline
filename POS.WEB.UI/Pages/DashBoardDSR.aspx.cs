using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using TalukderPOS.BusinessObjects;
using TalukderPOS.BLL;

public partial class Pages_DashBoardAdmin : System.Web.UI.Page
{
    private string userID = string.Empty;
    private string userPassword = string.Empty; 
    private string branchOID = string.Empty;
    SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
    SqlCommand cmd;
    SqlDataAdapter da = new SqlDataAdapter();
    SqlDataReader reader;
    

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            userID = Session["UserID"].ToString();
            userPassword = Session["Password"].ToString();
            branchOID = Session["StoreID"].ToString();
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
            loadNewsFeed();
        }
    }


    void loadNewsFeed()
    {
        NewsFeed_BO newsFeedObj;
        List<NewsFeed_BO> newsFeedList = new List<NewsFeed_BO>();        
        string todayDate = DateTime.Today.Date.ToString();
        String sql = "select NewsFeed.Message,NewsFeed.BranchOID from NewsFeed where NewsFeed.FromDate >= '" + todayDate + "' and NewsFeed.ToDate <='" + todayDate + "' AND ActiveStatus=1 order by NewsFeed.OID DESC";
        cmd = new SqlCommand(sql, dbConnect);
        try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    newsFeedObj = new NewsFeed_BO();
                    if (reader["BranchOID"].ToString() == string.Empty || reader["BranchOID"].ToString() == branchOID)
                    {
                        newsFeedObj.Message = reader["Message"].ToString();
                        newsFeedList.Add(newsFeedObj);
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbConnect.Close();
            }
            gvNewsFeed.DataSource = newsFeedList;
            gvNewsFeed.DataBind();
    }


  



   


}