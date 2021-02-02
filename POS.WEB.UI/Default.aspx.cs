using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using TalukderPOS.BusinessObjects;
using TalukderPOS.BLL;
public partial class Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HttpContext.Current.Session.Abandon();
        Server.Transfer("~/frmLogin.aspx");
        
        //Response.Redirect("DashBoard.aspx");
    }
    

}