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

public partial class Pages_uploaddata : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
      
      
        if (fileupload.HasFile)
        {


            string filename = Path.GetFileName(fileupload.FileName);
            fileupload.SaveAs(Server.MapPath("ALLDBBACKUP/") + filename);
            

            lblMessage.Text = "SAVED SUCCESSFULLY";
           
        }
        else
        {
            lblMessage.Text = "NO FILE SELECTED";
        }
      
    
    }
}