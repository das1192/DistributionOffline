using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using TalukderPOS.BLL;
using TalukderPOS.BusinessObjects;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using TalukderPOS.DAL;
using AjaxControlToolkit;

//using CrystalDecisions.CrystalReports.Engine;
//using CrystalDecisions.ReportSource;
//using CrystalDecisions.Shared;

using System.Xml;
using System.Text;
using System.Activities.Statements;
/// <summary>
/// Summary description for Helper
/// </summary>
public static class Helper
{

    public enum DutyType : int
    {
        Regular = 1,
        Shift = 2
    }

    public enum LeaveApplicationApprovalStatus : int
    {
        Approved = 1,
        NotApproved = 2,
        Cancelled = 3
    }

    public enum SearchInEnum : int
    {
        EnrollId = 1,
        FirstName = 2,
        MiddleName = 3,
        LastName = 4,
        Designation = 5,
        Department = 6,
        Shift = 7,
        Grade = 8,
        SBU = 9
    }

    public enum WeekDays : int
    {
        Saturday = 1,
        Sunday = 2,
        Monday = 3,
        Tuesday = 4,
        Wednesday = 5,
        Thursday = 6,
        Friday = 7
    }

    public enum TextMatch : int
    {
        FullTextMatch = 1,
        ApproximateMatch = 2
    }

    public enum AuthorizeForEnum : int
    {
        Both = 1,
        InTime = 2,
        OutTime = 3
    }

    public static string[] AttendanceStatusArray()
    {
        string[] AttendanceStatus = new string[10];
        AttendanceStatus[0] = "LI";
        AttendanceStatus[1] = "EI";
        AttendanceStatus[2] = "EO";
        AttendanceStatus[3] = "LO";
        AttendanceStatus[4] = "OL";
        AttendanceStatus[5] = "AB";
        AttendanceStatus[6] = "OT";
        AttendanceStatus[7] = "NW";
        AttendanceStatus[8] = "RNC";
        AttendanceStatus[9] = "PO";
        return AttendanceStatus;
    }

    public static Guid GetRoleId(string roleName)
    {
        Guid roleId = Guid.Empty;
        //aspnet_RolesBLL oaspnet_RolesBLL = new aspnet_RolesBLL();
        //List<aspnet_Roles> roleList = oaspnet_RolesBLL.aspnet_Roles_GetByRoleName(roleName);
        //foreach (aspnet_Roles role in roleList)
        //{
        //    roleId = role.RoleId;
        //    break;
        //}
        return roleId;
    }

    /// <summary>
    /// Fills drop down list with values of enumaration
    /// </summary>
    /// <param name="List">Dropdownlist</param>
    /// <param name="enumType">Enumeration</param>
    public static void FillDropDownWithEnum(DropDownList List, Type enumType)
    {
        if (List == null)
        {
           throw new ArgumentNullException("List");
        }
        if (enumType == null)
        {
           throw new ArgumentNullException("enumType");
        }
        if (!enumType.IsEnum)
        {
           throw new ArgumentException("enumType must be enum type");
        }
        List.Items.Clear();
        string[] strArray = Enum.GetNames(enumType);
        foreach (string str2 in strArray)
        {
            int enumValue = (int)Enum.Parse(enumType, str2, true);
            ListItem ddlItem = new ListItem(Helper.ConvertEnum(str2), enumValue.ToString());
            List.Items.Add(ddlItem);
        }
    }

    /// <summary>
    /// Convert enum for front-end
    /// </summary>
    /// <param name="s">Input string</param>
    /// <returns>Covnerted string</returns>
    public static string ConvertEnum(string s)
    {
        string result = string.Empty;
        char[] letters = s.ToCharArray();
        foreach (char c in letters)
            if (c.ToString() != c.ToString().ToLower())
                result += " " + c.ToString();
            else
                result += c.ToString();
        return result;
    }

    /// <summary>
    /// Verifies that a string is in valid e-mail format
    /// </summary>
    /// <param name="Email">Email to verify</param>
    /// <returns>true if the string is a valid e-mail address and false if it's not</returns>
    public static bool IsValidEmail(string Email)
    {
        return Regex.IsMatch(Email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
    }

    public static string ConvertMinuteToTimeText(string _Minutes)
    {
        string Hours = string.Empty;
        int mins = new int();
        int hours = new int();
        if (_Minutes != "&nbsp;" && !string.IsNullOrEmpty(_Minutes))
        {
            mins = Convert.ToInt32(_Minutes.Trim());
            if (mins > 0)
            {
                hours = mins / 60;
                mins = mins % 60;
                Hours = (hours < 10 ? "0" + hours.ToString() : hours.ToString()) + ":" + (mins < 10 ? "0" + mins.ToString() : mins.ToString());
            }
        }
        return Hours;
    }

    public static string GetException(Exception ex)
    {
        string msg = string.Empty;
        if (ex is System.Data.Common.DbException)
        {
            System.Data.Common.DbException dbEx;
            dbEx = (System.Data.Common.DbException)ex;
            if (dbEx.ErrorCode == -2146232060)
            {
                msg = "Can't delete data. There is some relavant data in other tables";
            }
            else
            {
                msg = dbEx.Message;
            }
        }
        else
        {
            msg = ex.Message;
        }

        return msg;
    }

    public static void ShowMessage(Label lbl, string msg, bool IsError)
    {
        lbl.Text = msg;
        if (IsError)
        {
            lbl.ForeColor = System.Drawing.Color.Red;
        }
        else
        {
            lbl.ForeColor = System.Drawing.Color.Green;
        }
    }

    public static string ConvertDateStringFormat(string date, bool IsToLocal)
    {
        string year = string.Empty;
        string month = string.Empty;
        string day = string.Empty;
        int index = 0;
        char sep = new char();
        sep = date.Contains("/") ? '/' : '-';
        if (IsToLocal)
        {
            //Get Month
            index = date.IndexOf(sep);
            month = date.Substring(0, index);
            date = date.Substring(index + 1);

            //Get Day
            index = date.IndexOf(sep);
            day = date.Substring(0, index);
            date = date.Substring(index + 1);

            //Get Year
            if (date.IndexOf(" ") > 0)
            {
                year = date.Substring(0, date.IndexOf(" "));
            }
            else
            {
                year = date.Substring(0);
            }
            return day + sep + month + sep + year;
        }
        else
        {
            //Get Day
            index = date.IndexOf(sep);
            day = date.Substring(0, index);
            date = date.Substring(index + 1);

            //Get Month
            index = date.IndexOf(sep);
            month = date.Substring(0, index);
            date = date.Substring(index + 1);

            //Get Year
            if (date.IndexOf(" ") > 0)
            {
                year = date.Substring(0, date.IndexOf(" "));
            }
            else
            {
                year = date.Substring(0);
            }
            return month + sep + day + sep + year;
        }
    }

    //public static void SetDataBaseLogonForReport(ReportDocument oReportDocument)
    //{
    //    System.Data.Common.DbConnection connection = (DbProviderHelper.GetConnection());
    //    String connStr = connection.ConnectionString;

    //    string[] connValues = connStr.Split(';');
    //    string[] serverName = connValues[0].Split('=');
    //    string[] dbName = connValues[1].Split('=');
    //    string[] userID = connValues[2].Split('=');
    //    string[] password = connValues[3].Split('=');
    //    System.Data.SqlClient.SqlConnectionStringBuilder SConn = new System.Data.SqlClient.SqlConnectionStringBuilder(connection.ConnectionString);
    //    //oReportDocument.SetDatabaseLogon(SConn.UserID, SConn.Password, SConn.DataSource, SConn.InitialCatalog);
    //    oReportDocument.SetDatabaseLogon(userID[1], password[1], serverName[1], dbName[1]);

    //}

    public static Guid GetNewGuid()
    {
        return Guid.NewGuid();
    }

    //public static bool Rank_Update(int CurrentRank, int NewRank, object PKValue, string PKField, string Table)
    //{
    //    UtilsBLL util = new UtilsBLL();
    //    return util.UpdateRank(CurrentRank, NewRank, PKValue, PKField, Table);
    //}

    //public static int Rank_GetNew(string Table)
    //{
    //    UtilsBLL util = new UtilsBLL();
    //    return util.GetNewRank(Table);
    //}

    public static void SetPageTitle(string Title, System.Web.UI.Page page)
    {
        page.Title = Title + ContextConstant.APPLICATION_NAME;
    }

    public static void SelectDropDownValue(DropDownList ddl, string value)
    {
        int itemCount = ddl.Items.Count;
        try
        {
            if (itemCount > 0)
                ddl.SelectedValue = value;
        }
        catch
        {
            if (itemCount > 0)
            {
                ddl.SelectedValue = "0";
            }
            else
            {
                ddl.SelectedIndex = 0;
            }
        }
    }

    /// <summary>
    /// Selects item
    /// </summary>
    /// <param name="List">List</param>
    /// <param name="Value">Value to select</param>
    public static void SelectListItem(DropDownList List, object Value)
    {
        if (List.Items.Count != 0)
        {
            ListItem selectedItem = List.SelectedItem;
            if (selectedItem != null)
                selectedItem.Selected = false;
            if (Value != null)
            {
                selectedItem = List.Items.FindByValue(Value.ToString());
                if (selectedItem != null)
                    selectedItem.Selected = true;
            }
        }
    }


    public static void Export(string type, GridView gv, Page page)
    {
        page.Response.Clear();
        if (type == "excel")
        {
            page.Response.AddHeader("content-disposition", "attachment;filename=EmployeeList.xls");
            page.Response.ContentType = "application/vnd.xls";
        }
        else if (type == "word")
        {
            page.Response.AddHeader("content-disposition", "attachment;filename=EmployeeList.doc");
            page.Response.ContentType = "application/msword";
        }
        page.Response.Charset = String.Empty;
        System.IO.StringWriter sw = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter hw = new HtmlTextWriter(sw);
        gv.RenderControl(hw);
        page.Response.Write(sw.ToString());
        page.Response.End();
    }

    public static void Redirect(string url, string target, string windowFeatures)
    {
        HttpContext context = HttpContext.Current;
        if ((String.IsNullOrEmpty(target) ||
            target.Equals("_self", StringComparison.OrdinalIgnoreCase)) &&
            String.IsNullOrEmpty(windowFeatures))
        {
            context.Response.Redirect(url);
        }
        else
        {
            Page page = (Page)context.Handler;
            if (page == null)
            {
                throw new InvalidOperationException(
                    "Cannot redirect to new window outside Page context.");
            }

            url = page.ResolveClientUrl(url);
            string script;
            if (!String.IsNullOrEmpty(windowFeatures))
            {
                script = @"window.open(""{0}"", ""{1}"", ""{2}"");";
            }
            else
            {
                script = @"window.open(""{0}"", ""{1}"");";
            }
            script = String.Format(script, url, target, windowFeatures);
            ScriptManager.RegisterStartupScript(page,
                typeof(Page),
                "Redirect",
                script,
                true);
        }
    }

    /// <summary>
    /// Write XML to response
    /// </summary>
    /// <param name="xml">XML</param>
    /// <param name="Filename">Filename</param>
    public static void WriteResponseXML(string xml, string Filename)
    {
        if (!String.IsNullOrEmpty(xml))
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(xml);
            ((XmlDeclaration)document.FirstChild).Encoding = "utf-8";
            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            response.Charset = "utf-8";
            response.ContentType = "text/xml";
            response.AddHeader("content-disposition", string.Format("attachment; filename={0}", Filename));
            response.BinaryWrite(Encoding.UTF8.GetBytes(document.InnerXml));
            response.End();
        }
    }

    /// <summary>
    /// Write PDF file to response
    /// </summary>
    /// <param name="filePath">File napathme</param>
    /// <param name="targetFileName">Target file name</param>
    /// <remarks>For BeatyStore project</remarks>
    public static void WriteResponsePDF(string filePath, string targetFileName)
    {
        if (!String.IsNullOrEmpty(filePath))
        {
            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            response.Charset = "utf-8";
            response.ContentType = "text/pdf";
            response.AddHeader("content-disposition", string.Format("attachment; filename={0}", targetFileName));
            response.BinaryWrite(File.ReadAllBytes(filePath));
            response.End();
        }
    }



    #region Genarel
    public static string GetStringValue(object value)
    {
        if (value == null)
            return String.Empty;
        else
            return (String)value;
    }
    public static int GetIntegerValue(object value)
    {
        if (value == null)
            return 0;
        else
            return int.Parse(value.ToString());
    }
    public static Decimal GetDecemalValue(object value)
    {
        if (value == null)
            return 0;
        else
            return Convert.ToDecimal(value.ToString());
    }

    #endregion

    //public static int Rank_GetNew()
    //{
    //    throw new NotImplementedException();
    //}
}

public static class Alert
{

    /// <summary>
    /// Shows a client-side JavaScript alert in the browser.
    /// </summary>
    /// <param name="message">The message to appear in the alert.</param>
    public static void ShowMessage(string message)
    {
        // Cleans the message to allow single quotation marks
        string cleanMessage = message.Replace("'", "\'");
        string script = "<script type=\"text/javascript\">alert('" + cleanMessage + "');</script>";

        // Gets the executing web page
        Page page = HttpContext.Current.CurrentHandler as Page;

        // Checks if the handler is a Page and that the script isn't allready on the Page
        if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("alert"))
        {
            page.ClientScript.RegisterClientScriptBlock(typeof(Alert), "alert", script);
        }

        //if (page != null && !page.IsStartupScriptRegistered("alertMessage"))
        //{
        //    //page.RegisterStartupScript("alertMessage", script);
        //    page.RegisterClientScriptBlock("alertmessage", script);
        //}

        //if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("Alert"))
        //{
        //    //page.ClientScript.RegisterClientScriptBlock(typeof(Alert), "Alert", script);
        //    ScriptManager.RegisterClientScriptBlock(lbl, typeof(Alert), "Alert", script, true);
        //}


    }

    public static void ShowMessage(string message, Page page)
    {
        string script = "<script language=\"javascript\"  type=\"text/javascript\">alert('" + message + "');</script>";
        ScriptManager.RegisterStartupScript(page, page.GetType(), "AlertMessage", script, false);
    }

    public static void ShowMessage(Label label, string message, bool isError)
    {

        string strInterval = string.Empty;
        if (message == string.Empty) return;
        try
        {
            AlwaysVisibleControlExtender avce = new AlwaysVisibleControlExtender();
            avce.TargetControlID = label.ID;
            avce.ScrollEffectDuration = .1f;
            label.Parent.Controls.Add(avce);
            label.ForeColor = System.Drawing.Color.Black;

            //string webRoot = ConfigurationManager.AppSettings["webRoot"].ToString();

            label.Text = "<div id=\"divParent\" style=\"width: 100%; padding: 5px;\">";
            label.Text += "<div style=\"float: left; width: 80%; text-align: left;\">";
            //label.Text += "<img src=\"" + webRoot + "Images/messagebox_info.png\" />";
            label.Text += "  " + "" + message + "";
            label.Text += "</div>";

            label.Text += "<div style=\"float: right; width: 18%; text-align: right; padding-right: 10px; white-space: nowrap\">";
            //label.Text += " <a href=\"javascript:void(0)\" onclick=\"javascript:hideMeClick('" + label.ClientID + "')\" title=\"Hide Me\">Hide Me</a></div>";
            label.Text += " <a href=\"javascript:void(0)\" onclick=\"javascript:function hideMeClick(){document.getElementById('divParent').style.visibility = \'hidden\';"
                       + ")\" title=\"Hide Me\">Hide Me</a></div>";
            label.Text += "</div>";
            strInterval = "3000";//ConfigurationManager.AppSettings["AlertInterval"].ToString();
            ScriptManager.RegisterClientScriptBlock(label, typeof(Alert), "Alert",
               "window.setTimeout(function(){hideMeClick(){document.getElementById('divParent').style.visibility = \'hidden\';}, '" + strInterval + "')", true);

            if (isError)
            {
                label.ForeColor = System.Drawing.Color.Red;
                label.Font.Bold = true;
            }

            label.CssClass = "CommonAlertMessage";
        }
        catch (Exception)
        {
            label.ForeColor = System.Drawing.Color.DodgerBlue;

            label.Text = message;
            if (isError)
            {
                label.ForeColor = System.Drawing.Color.Red;
                label.Font.Bold = true;
            }
        }
    }





}
