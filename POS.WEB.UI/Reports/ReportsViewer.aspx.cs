using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Configuration;


public partial class Reports_ReportsViewer : System.Web.UI.Page
{

    string connStr = ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString;
    ReportDocument report = new ReportDocument();

    protected void Page_Load(object sender, EventArgs e)
    {
        string R = Session["ReportPath"].ToString();
        report.Load(Server.MapPath(R));
        //report.SetDatabaseLogon("admin", "admin", @"210.4.67.245", "LITEPOSNEW");
        report.SetDataSource(Session["dtsales"]);
        report.ExportToHttpResponse(ExportFormatType.ExcelWorkbook, Response, false, "Report");

        CrystalReportViewer1.ReportSource = report;
        CrystalReportViewer1.DataBind();
        CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
    }
       
    
       
    
    
    //protected void imgbtnsale_Click(object sender, ImageClickEventArgs e)
    //{
    //    report.PrintToPrinter(1, false, 0, 0);
    //}
 
}