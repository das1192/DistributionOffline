using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TalukderPOS.BLL;
using TalukderPOS.BusinessObjects;
using System.Data;
using System.IO;
//using iTextSharp.text.pdf;
//using iTextSharp.text;
//using iTextSharp.text.html.simpleparser;
using System.Text;

public partial class Pages_SalesStockSummaryReport : System.Web.UI.Page
{

    private string userID = string.Empty;
    private string userPassword = string.Empty;
    SalesReport_BILL BILL = new SalesReport_BILL();



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
        T_PROD entity = new T_PROD();
        string[] valid = new string[2];
        DataTable dt = null;
        entity.Branch = ddlSearchBranch.SelectedItem.Value.ToString();
        entity.PROD_WGPG = ddlSearchProductCategory.SelectedItem.Value.ToString();
        if (entity.ToDate == string.Empty)
        {
            entity.ToDate = DateTime.Today.Date.ToString("yyyy-MM-dd");
        }
        else
        {
            entity.ToDate = txttoDate.Text;
        }
        entity.NoOfDate = ddlPrevDtCount.SelectedItem.Value.ToString();

        valid = BILL.Validation(entity);
        if (valid[0].ToString() == "True")
        {
            lblMessage.Text = valid[1].ToString();
            dt = BILL.RequistionReport(entity);
        }
        else
        {
            lblMessage.Text = valid[1].ToString();
        }

        if (dt != null && dt.Rows.Count > 0) {
            DatatableToExcel(dt);        
        }        
    }



    private void DatatableToCSV(DataTable dt)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "attachment;filename=DataTable.csv");
        Response.Charset = "";
        Response.ContentType = "application/text";


        StringBuilder sb = new StringBuilder();
        for (int k = 0; k < dt.Columns.Count; k++)
        {
            //add separator
            sb.Append(dt.Columns[k].ColumnName + ',');
        }
        //append new line
        sb.Append("\r\n");
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            for (int k = 0; k < dt.Columns.Count; k++)
            {
                //add separator
                sb.Append(dt.Rows[i][k].ToString().Replace(",", ";") + ',');
            }
            //append new line
            sb.Append("\r\n");
        }
        Response.Output.Write(sb.ToString());
        Response.Flush();
        Response.End();
    }

    //protected void ExportToPDF(DataTable dt)
    //{
    //    //Create a dummy GridView
    //    GridView GridView1 = new GridView();
    //    GridView1.AllowPaging = false;
    //    GridView1.DataSource = dt;
    //    GridView1.DataBind();

    //    Response.ContentType = "application/pdf";
    //    Response.AddHeader("content-disposition",
    //        "attachment;filename=DataTable.pdf");
    //    Response.Cache.SetCacheability(HttpCacheability.NoCache);
    //    StringWriter sw = new StringWriter();
    //    HtmlTextWriter hw = new HtmlTextWriter(sw);
    //    GridView1.RenderControl(hw);
    //    StringReader sr = new StringReader(sw.ToString());
    //    Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
    //    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
    //    PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
    //    pdfDoc.Open();
    //    htmlparser.Parse(sr);
    //    pdfDoc.Close();
    //    Response.Write(pdfDoc);
    //    Response.End();
    //}


    private void DatatableToExcel(DataTable dt)
    {
        string attachment = "attachment; filename=RequisitionReport.xls";
        Response.ClearContent();
        Response.AddHeader("content-disposition", attachment);
        Response.ContentType = "application/vnd.ms-excel";
        string tab = "";
        foreach (DataColumn dc in dt.Columns)
        {
            Response.Write(tab + dc.ColumnName);
            tab = "\t";
        }
        Response.Write("\n");
        int i;
        foreach (DataRow dr in dt.Rows)
        {
            tab = "";
            for (i = 0; i < dt.Columns.Count; i++)
            {
                Response.Write(tab + dr[i].ToString());
                tab = "\t";
            }
            Response.Write("\n");
        }
        Response.End();
    }
    
}