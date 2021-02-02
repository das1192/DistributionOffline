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

public partial class Reports_ReportView : System.Web.UI.Page
{
    string connStr = ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString;
    ReportDocument report = new ReportDocument();

    protected void Page_Load(object sender, EventArgs e)
    {
        string ReportPath = Session["ReportPath"].ToString();
        DataTable dt = (DataTable)Session["dtsales"];
        report.Load(MapPath(ReportPath));
        report.SetDatabaseLogon("admin", "admin", @"210.4.67.245", "LITEPOSNEW");
        report.SetDataSource(dt);
        report.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "Report");

        CrystalReportViewer1.ReportSource = report;
        CrystalReportViewer1.DataBind();
        CrystalReportViewer1.RefreshReport();
        //CrystaReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
        //report.PrintToPrinter(1, false, 0, 0);
        //Session["dtsales"] = null;        
    }

    protected void Page_Unload(object sender, EventArgs e)
    {
        if (this.report != null)
        {
            this.report.Close();
            this.report.Dispose();
        }
    }




}