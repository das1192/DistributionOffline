using System;
using System.Web;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using TalukderPOS.BLL;
using TalukderPOS.BusinessObjects;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;
using System.IO.Compression;
using Ionic.Zip;

public partial class Pages_sync : System.Web.UI.Page
{
    private string userID = string.Empty;
    private string userPassword = string.Empty;
    private string Shop_id = string.Empty;
    AddShopBILL BILL = new AddShopBILL();

    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            userID = Session["UserID"].ToString();
            userPassword = Session["Password"].ToString();
            Shop_id = Session["StoreID"].ToString();
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            //scriptManager.RegisterPostBackControl(btnSave);

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
        Page.Form.Attributes.Add("enctype", "multipart/form-data");
    }

    public string GetImage(object img)
    {
        return "data:image/jpg;base64," + Convert.ToBase64String((byte[])img);
    }

    protected void btnbackup_Click(object sender, EventArgs e)
    {
        //string[] dirs = Directory.GetFiles(@"c:\DBBACKUP\", "LITEPOSNEW*");
        //Console.WriteLine("The number of files starting with c is {0}.", dirs.Length);
        System.IO.DirectoryInfo di = new DirectoryInfo("c:/DBBACKUP");

        foreach (FileInfo file in di.GetFiles())
        {
            file.Delete();
        }

        var backupFolder = ConfigurationManager.AppSettings["BackupFolder"];

        var connectionString = ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString;
        var sqlConStrBuilder = new SqlConnectionStringBuilder(connectionString);

        // set backupfilename (you will get something like: "C:/temp/MyDatabase-2013-12-07.bak")
        var backupFileName = String.Format("{0}{1}-{2}.bak",
            backupFolder, sqlConStrBuilder.InitialCatalog,
            DateTime.Now.ToString("yyyy-MM-dd"));
        try
        {
            using (var connection = new SqlConnection(sqlConStrBuilder.ConnectionString))
            {
                var query = String.Format("BACKUP DATABASE {0} TO DISK='{1}'",
                    sqlConStrBuilder.InitialCatalog, backupFileName);

                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }

            }
           using (ZipFile zip = new ZipFile())
             {

                 zip.AddDirectory(@"C:\\DBBACKUP", "files");
                 
                 zip.Save("C:\\DBBACKUPZIP\\FINLAY_" + DateTime.Now.ToString("yyyy-MM-dd")+".zip");

             }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Back UP taken Successfull')", true);
        }
        catch
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Second Phase Problem. Contact with software provider')", true);
        }
    }

   
    protected void btnCancel_Click(object sender, EventArgs e)
    {
       
        ContainerBankInfo.ActiveTabIndex = 0;
        lblMessage.Text = string.Empty;
    }


    
   
    
}