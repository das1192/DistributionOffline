using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using Ext.Net;
//using SALESANDINVENTORY.BusinessObjects;
//using SALESANDINVENTORY.BLL;
using System.Data;
using TalukderPOS.BLL;
using TalukderPOS.BusinessObjects;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;


public partial class MasterPage : System.Web.UI.MasterPage
{
    //MenuHeadBLL objMenuHeadBLL = new MenuHeadBLL();
    //MenuPageBLL objMenuPageBLL = new MenuPageBLL();
    //MenuPermissionBLL objMenuPermissionBLL = new MenuPermissionBLL();

    private string userID = "";
    private string userpassword = "";
    private string userFullName = "";
    private string UserLocation = "";
    private string ShopCode = "";
    private string pageid = "";
    string connStr = ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString;
    SqlCommand cmd;
    String sql;
    SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            userID = Session["UserID"].ToString();
            userpassword = Session["Password"].ToString();
            userFullName = Session["UserFullName"].ToString();
            UserLocation = Session["CCOM_NAME"].ToString();
            ShopCode = Session["CCOM_PREFIX"].ToString();
            string storeid = HttpContext.Current.Session["StoreID"].ToString();
            
            
        }
        catch

        {
            Response.Redirect("~/Default.aspx");
        }

        if (!IsPostBack)
        {
            string pageid2 = "";
            
            string sPath = System.Web.HttpContext.Current.Request.Url.AbsolutePath;
            string basepath = HttpContext.Current.Request.Url.LocalPath;
            lblUser.Text = "<span class='label label-primary' >Shop Code : " + ShopCode + "</span>  <span class='label label-success' >User : " + userFullName + "</span>  <span class='label label-info'>" + UserLocation +
                 "</span> ";
            System.IO.FileInfo oInfo = new System.IO.FileInfo(sPath);
            string sRet = oInfo.Name;


            if (sRet == "DashBoard.aspx" || sRet == "SendMessage")
            {

            }
            else
            {
                pageid2 = Request.QueryString["menuhead"].ToString();
                GetMenuData2();
               
            }
            GetImage();
        }
    }
    private void GetImage()
    {
        string storeid = HttpContext.Current.Session["StoreID"].ToString();
        sql = "select Top(1) OID,ImageByte from Shop_Logo where Shop_id =" + storeid + " ";
        
        try
        {
            //SqlDataAdapter da12 = new SqlDataAdapter(sql, dbConnect);
            if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
            SqlCommand cmd = new SqlCommand(sql, dbConnect);
            SqlDataReader dr = cmd.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                byte[] picData = dr["ImageByte"] as byte[] ?? null;
                string imageBase64Data = Convert.ToBase64String(picData);
                string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);
                Image1.ImageUrl = imageDataURL;
            }
            else
            {
                Image1.ImageUrl = "Images/Samsung.jpg";
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

    }
    private void GetMenuData2()
    {

       DataTable table = new DataTable();
        DataSet ds1 = new DataSet();
        DataTable dt1 = new DataTable();

        DataTable table12 = new DataTable();
        DataSet ds12 = new DataSet();
        DataTable dt12 = new DataTable();

        string activeURL = ".." + HttpContext.Current.Request.Url.AbsolutePath;
        string pageid2 = Request.QueryString["menuhead"].ToString();
        StringBuilder tableOutput = new StringBuilder();
        tableOutput.Append("<div id='mySidenav' class='sidenav'><a href='javascript:void(0)' class='closebtn' onclick='closeNav()'>&times;</a>");




        using (SqlConnection conn2 = new SqlConnection(connStr))
        {

            string sql12 = "SELECT DISTINCT dbo.MenuHead.MenuHeadID, dbo.MenuHead.MenuHeadName, dbo.MenuHead.Priority, dbo.MenuPermission.UserID, dbo.MenuPermission.CanView";
            sql12 = sql12 + " FROM dbo.MenuHead INNER JOIN dbo.MenuPage ON dbo.MenuHead.MenuHeadID = dbo.MenuPage.MenuHeadID INNER JOIN";
            sql12 = sql12 + " dbo.MenuPermission ON dbo.MenuHead.MenuHeadID = dbo.MenuPermission.MenuHeadID AND dbo.MenuPage.PageId = dbo.MenuPermission.PageID";
            sql12 = sql12 + " WHERE  (dbo.MenuPermission.UserID = '" + userID + "')  ";

            SqlDataAdapter da12 = new SqlDataAdapter(sql12, conn2);
            da12.Fill(ds12);
            dt12 = ds12.Tables[0];
            // write the sql statement to execute
            //Create Stringbuilder, so we can append the iterated results from our DataTable.
            

            for (int i = 0; i < dt12.Rows.Count; i++)
            {

                tableOutput.Append("<a class='menuitem submenuheader' runat='server' href='#' ");
                tableOutput.Append(">" + dt12.Rows[i]["MenuHeadName"].ToString());
                tableOutput.Append("</a>");
                tempHtmlTable.Text = tableOutput.ToString();

                string sql33 = "SELECT DISTINCT dbo.MenuHead.MenuHeadID, dbo.MenuHead.MenuHeadName, dbo.MenuHead.Priority, dbo.MenuPage.PageId, dbo.MenuPage.PageName, dbo.MenuPage.URL,";
                sql33 = sql33 + " dbo.MenuPermission.UserID, dbo.MenuPermission.CanView FROM dbo.MenuHead INNER JOIN  dbo.MenuPage ON dbo.MenuHead.MenuHeadID = dbo.MenuPage.MenuHeadID INNER JOIN ";
                sql33 = sql33 + " dbo.MenuPermission ON dbo.MenuHead.MenuHeadID = dbo.MenuPermission.MenuHeadID AND dbo.MenuPage.PageId = dbo.MenuPermission.PageID WHERE     (dbo.MenuPermission.UserID = '" + dt12.Rows[i]["UserID"].ToString() + "') and dbo.MenuHead.MenuHeadID = '" + dt12.Rows[i]["MenuHeadID"].ToString() + "'";
                //string abc = dt12.Rows[i]["MenuHeadID"].ToString();

                // instantiate the command object to fire

                using (SqlCommand cmd12 = new SqlCommand(sql33, conn2))
                {

                    // get the adapter object and attach the command object to it

                    using (SqlDataAdapter ad12 = new SqlDataAdapter(cmd12))
                    {
                        // fire Fill method to fetch the data and fill into DataTable
                        ad12.Fill(table12);

                    }

                }


                tableOutput.Append("<div class='submenu' runat='server' > <ul class='mainul'> ");
                string devMode = System.Configuration.ConfigurationManager.AppSettings["devMode"].ToString();
                string devModeURLString = "../POS.WEB.UI/";
                string deployMode = "../";
                for (int j = 0; j < table12.Rows.Count; j++)
                {

                    if (activeURL == table12.Rows[j]["Url"].ToString())
                    {
                        //tableOutput.Append("<li><a id='abc" + j + "' onclick='abcd(" + j + ")' runat='server' ID=" + table.Rows[j]["PageId"].ToString());
                        tableOutput.Append("<li><a class='active' runat='server' ID=" + table12.Rows[j]["PageId"].ToString());
                    }
                    else
                    {
                        tableOutput.Append("<li><a runat='server' ID=" + table12.Rows[j]["PageId"].ToString());
                    }
                    if (devMode == "1")
                    {
                        string mainurl = table12.Rows[j]["Url"].ToString();
                       

                        tableOutput.Append(" href=../../" + devModeURLString + table12.Rows[j]["Url"].ToString());
                        tableOutput.Append("?menuhead=" + table12.Rows[j]["MenuHeadID"].ToString());
                    }
                    else
                    {
                       
                        tableOutput.Append(" href=../../" + deployMode + table12.Rows[j]["Url"].ToString());
                        tableOutput.Append("?menuhead=" + table12.Rows[j]["MenuHeadID"].ToString());
                    }
                    
                    tableOutput.Append(" >" + table12.Rows[j]["PageName"].ToString());
                    tableOutput.Append("</a></li>");
                    //Label1.Text = tableOutput.ToString();
                }
                tableOutput.Append("</ul></div>");
                table12.Clear();
            }
            
        }




        tableOutput.Append("</div>");
        //tableOutput.Append("<nav class='navbar navbar-inverse'>"
        //    +"'<div class='container-fluid'><div class='navbar-header'>"
        //    +"<button type='button' class='navbar-toggle' data-toggle='collapse' data-target='#myNavbar'>"
        //    +"<span class='icon-bar'></span>"
        //    +"<span class='icon-bar'></span>"
        //    +"<span class='icon-bar'></span>"
        //    +"</button></div><div class='collapse navbar-collapse' id='myNavbar'><ul class='nav navbar-nav'>");
          tableOutput.Append("<div class='topnav' id='myTopnav'>");


          tableOutput.Append("<a href='../DashBoard.aspx'class='myactive '><div class='myicon'><i class='fa fa-home'></i></div></a>");
        using (SqlConnection conn = new SqlConnection(connStr))
        {

            string sql = "SELECT DISTINCT dbo.MenuHead.MenuHeadID, dbo.MenuHead.MenuHeadName, dbo.MenuHead.Priority, dbo.MenuPage.PageId, dbo.MenuPage.PageName, dbo.MenuPage.URL,";
            sql = sql + " dbo.MenuPermission.UserID, dbo.MenuPermission.CanView FROM dbo.MenuHead INNER JOIN  dbo.MenuPage ON dbo.MenuHead.MenuHeadID = dbo.MenuPage.MenuHeadID INNER JOIN ";
            sql = sql + " dbo.MenuPermission ON dbo.MenuHead.MenuHeadID = dbo.MenuPermission.MenuHeadID AND dbo.MenuPage.PageId = dbo.MenuPermission.PageID WHERE     (dbo.MenuPermission.UserID = '" + userID + "') and dbo.MenuHead.MenuHeadID = '" + pageid2 + "'";

            SqlDataAdapter da1 = new SqlDataAdapter(sql, conn);
            da1.Fill(ds1);
            dt1 = ds1.Tables[0];
            // write the sql statement to execute
            //Create Stringbuilder, so we can append the iterated results from our DataTable.

           
            
            for (int i = 0; i < dt1.Rows.Count; i++)
            {

                
                string devMode = System.Configuration.ConfigurationManager.AppSettings["devMode"].ToString();
                string devModeURLString = "../POS.WEB.UI/";
                string deployMode = "../";

                string cc = activeURL.Replace("../POS.WEB.UI/", "");
                string bb = dt1.Rows[i]["Url"].ToString();
              
                if (devMode == "0")
                {
                    cc = cc.Replace("../", "");
                }

                if (cc == dt1.Rows[i]["Url"].ToString())
                    {
                        //tableOutput.Append("<li><a id='abc" + j + "' onclick='abcd(" + j + ")' runat='server' ID=" + table.Rows[j]["PageId"].ToString());
                        tableOutput.Append("<a class='active'  runat='server' ID=" + dt1.Rows[i]["PageId"].ToString());
                    }
                    else
                    {
                        tableOutput.Append("<a runat='server' ID=" + dt1.Rows[i]["PageId"].ToString());
                    }
                    if (devMode == "1")
                    {
                        tableOutput.Append(" href=../../" + devModeURLString + dt1.Rows[i]["Url"].ToString()  + "?menuhead=" + dt1.Rows[i]["MenuHeadID"].ToString());
                    }
                    else
                    {
                        tableOutput.Append(" href=../../" + deployMode + dt1.Rows[i]["Url"].ToString() + "?menuhead=" + dt1.Rows[i]["MenuHeadID"].ToString());
                    }

                    tableOutput.Append(" >" + dt1.Rows[i]["PageName"].ToString());
                    tableOutput.Append("</a>");

                
            }
            //tableOutput.Append("</ul><ul class='nav navbar-nav navbar-right'><li><a href='../DashBoard.aspx'><span class='glyphicon glyphicon-log-in'></span> Dashboard</a></li><li><a href='../default.aspx'><span class='glyphicon glyphicon-log-in'></span> Logout</a></li></ul></div></div></nav>");
           // tableOutput.Append("</ul>");
          //old  tableOutput.Append("<ul class='nav navbar-nav navbar-right'><li><span style='color:#fff;font-size:23px;cursor:pointer' onclick='openNav()'>&#9776;</span></li><li><a href='../DashBoard.aspx'><span class='glyphicon glyphicon-log-in'></span> Dashboard</a></li><li><a href='../default.aspx'><span class='glyphicon glyphicon-log-in'></span> Logout</a></li></ul>");
            tableOutput.Append("<div class='mynav'><a href='javascript:void(0);' onclick='openNav()' class='myactive2'> <div class='myicon'><i class='fa fa-sort-alpha-asc'></i></div></a></div>");
            tableOutput.Append("<div class='mynav'><a href='../default.aspx'  class='myactive3'> <div class='myicon'><i class='fa fa-power-off'></i></div></a></div>");  
         
         
tableOutput.Append("<a href='javascript:void(0);' class='icon' onclick='myFunction()'><i class='fa fa-bars'></i></a>");        
tableOutput.Append("</div>");
            tempHtmlTable.Text = tableOutput.ToString();
        }
    }
    protected void btnBackUp_Click(object sender, EventArgs e)
    {
        DateTime d = DateTime.Now;
        string dd = d.Day.ToString()+"-"+ d.Month.ToString()+"-"+d.Year.ToString();

        //string aaa = @"Data Source=DESKTOP-D1J1P2N;Integrated Security=True;Initial Catalog=LITEPOSONLINEACC2";
        SqlConnection con = new SqlConnection(connStr);
        //con.ConnectionString = ConfigurationManager.ConnectionStrings["BackupCatalogDBSoft.Properties.Settings."+dbname+"ConnectionString"].ToString();

        con.Open();
        //string str = "USE LITEPOSONLINEACC2; ";
        string str1 = "USE LITEPOSONLINEACC2; BACKUP DATABASE LITEPOSONLINEACC2" +
            " TO DISK = 'D:\\Database\\LITEPOSONLINEACC2_" + dd +
            ".Bak' WITH FORMAT,MEDIANAME = 'MM_SQLServerBackups',NAME = 'Full Backup of LITEPOSONLINEACC2';";
        //SqlCommand cmd1 = new SqlCommand(str, con);
        SqlCommand cmd2 = new SqlCommand(str1, con);
        //cmd1.ExecuteNonQuery();
        cmd2.ExecuteNonQuery();
        Alert.ShowMessage("Back Up is Successfull.");
        con.Close();
    }
}






