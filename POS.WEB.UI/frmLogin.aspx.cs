﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TalukderPOS.BLL;
using System.Drawing;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

public partial class frmLogin : System.Web.UI.Page
{
    string connStr = ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString;
    SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
    SqlCommand cmd;
    SqlDataAdapter da = new SqlDataAdapter();
    string sql;


    protected void Page_Load(object sender, EventArgs e)
    {
        //msgtr.Visible = false;
        if (Session["UserID"] != null && Session["Password"] != null)
        {
            if (System.Web.HttpContext.Current.Request.Cookies["UserID"] != null)
            {
                Session["UserID"] = null;
                System.Web.HttpContext.Current.Response.Cookies["UserID"].Expires = DateTime.Now.AddDays(-1);
            }

            if (System.Web.HttpContext.Current.Request.Cookies["Password"] != null)
            {
                Session["Password"] = null;
                System.Web.HttpContext.Current.Response.Cookies["Password"].Expires = DateTime.Now.AddDays(-1);
            }
            if (System.Web.HttpContext.Current.Request.Cookies["UserRole"] != null)
            {
                Session["UserRole"] = null;
                System.Web.HttpContext.Current.Response.Cookies["UserRole"].Expires = DateTime.Now.AddDays(-1);
            }
            if (System.Web.HttpContext.Current.Request.Cookies["UserFullName"] != null)
            {
                Session["UserName"] = null;
                System.Web.HttpContext.Current.Response.Cookies["UserFullName"].Expires = DateTime.Now.AddDays(-1);
            }
            HttpContext.Current.Session.Abandon();
        }
       

    }

    
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        lblMessageInvalid.Text = string.Empty;
        //lblMessage.Text = string.Empty;
         UserBLL objUserBLL = new UserBLL();         
         string userID = objUserBLL.GetUserIDByUserNamePassword(txtusername.Text, txtpassword.Text);

         DateTime LDate = new DateTime(2022,01,10);
         if (DateTime.Now > LDate) 
         {
             lblMessageInvalid.Text = "License Expired.. Please Contact To PlayOn24 (01673899273)";
             return;
         }
         if (userID != null && userID != string.Empty)
         {
             //sql = "SELECT [User].CCOM_OID,[User].UserFullName,[User].UserName,[User].Password,[ShopInfo].ShopName,[ShopInfo].CCOM_PREFIX FROM [User] inner join [ShopInfo] on [User].CCOM_OID=[ShopInfo].OID where [User].UserId='" + userID + "' ";
             sql = string.Format(@"
SELECT u.CCOM_OID,u.UserFullName,u.UserName,u.Password,s.ShopName,s.CCOM_PREFIX,ShopActiveStatus=s.ActiveStatus 
FROM [User] u 
inner join [ShopInfo] s on u.CCOM_OID=s.OID
where u.UserId='{0}' 
", userID);
             DataTable dtForNameAndRole = CommonBinder.getDataTable(sql);
             if (dtForNameAndRole.Rows.Count>0)
             {
                 if (dtForNameAndRole.Rows[0]["ShopActiveStatus"].ToString() != "1")
                 {
                     //msgtr.Visible = true;
                     lblMessageInvalid.Text = "Sorry, you inactived!";
                     return;
                 }
             }

             try
             {
                 Session["UserID"] = userID;
                 Session["UserName"] = dtForNameAndRole.Rows[0]["UserName"].ToString();
                 Session["Password"] = txtpassword.Text;
                 Session["UserFullName"] = dtForNameAndRole.Rows[0]["UserFullName"].ToString();
                 Session["CCOM_PREFIX"] = dtForNameAndRole.Rows[0]["CCOM_PREFIX"].ToString();
                 Session["CCOM_NAME"] = dtForNameAndRole.Rows[0]["ShopName"].ToString();
                 Session["StoreID"] = dtForNameAndRole.Rows[0]["CCOM_OID"].ToString();
             }
             catch
             {
                 Session["UserFullName"] = string.Empty;
             }
             txtpassword.Text = string.Empty;
             txtusername.Text = string.Empty;             
             Response.Redirect("DashBoard.aspx");             
         }
         else
         {
             lblMessageInvalid.Text = "Sorry, Invalid!";
             return;             
         }

    }
}