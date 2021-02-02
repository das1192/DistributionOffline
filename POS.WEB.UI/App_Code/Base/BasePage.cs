using System;
using System.Diagnostics;
using System.Web.Security;
using System.Text.RegularExpressions;
using System.Web.UI;

using System.Web;
using System.Web.SessionState;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;
using System.Globalization;

public class BasePage : System.Web.UI.Page
{

    #region Variables

    private static readonly Regex RegexBetweenTags = new Regex(@">(?! )\s+", RegexOptions.Compiled);
    private static readonly Regex RegexBetweenTags2 = new Regex(@">(?!u)[\s]*", RegexOptions.Compiled);

    private static readonly Regex RegexLineBreaks = new Regex(@"([\n\s])+?(?<= {2,})<", RegexOptions.Compiled);

    protected int CurrentEmployeeId = 0;

    protected int CurrentMainDivisionId = 0;

    protected int CurrentDivisionId = 0;

    protected int CurrentBranchId = 0;

    protected int CurrentDepartmentId = 0;

    protected CultureInfo CultureInfo_BD()
    {
        return new System.Globalization.CultureInfo("fr-FR");
    }

    protected CultureInfo CultureInfo_US()
    {
        return new System.Globalization.CultureInfo("en-us");
    }

    #endregion

    #region Ctor

    public BasePage()
    {

    }

    #endregion

    #region Properties

    protected bool IsAuthenticatedUser
    {
        [DebuggerStepThrough]
        get
        {
            return Page.User.Identity.IsAuthenticated;
        }
    }

    protected string CurrentUserName
    {
        [DebuggerStepThrough]
        get
        {
            return IsAuthenticatedUser ? Page.User.Identity.Name : "Anonymous";
        }
    }

    protected string CurrentUserRole
    {
        [DebuggerStepThrough]
        get
        {
            return (Roles.GetRolesForUser(CurrentUserName)).GetValue(0).ToString();
        }
    }

    protected bool IsUserAdmin
    {
        [DebuggerStepThrough()]
        get
        {
            return Page.User.IsInRole(ContextConstant.ROLE_ADMIN);
        }
    }

    protected bool IsUserEmployee
    {
        [DebuggerStepThrough()]
        get
        {
            return Page.User.IsInRole(ContextConstant.ROLE_EMPLOYEE);
        }
    }

    protected int CurrentEmployeeID
    {
        [DebuggerStepThrough]
        get
        {
            int EmployeeId = new int();//for Admin creator id is zero(0).
            try
            {
                EmployeeId = Convert.ToInt32(Page.Session[ContextConstant.EMPLOYEE_ID_OF_LOGGED_IN_USER]);
            }
            catch { }
            return EmployeeId;
        }
    }

    protected string CreatedFrom
    {
        [DebuggerStepThrough]
        get
        {
            //return Request.UserHostAddress + "," + Request.LogonUserIdentity.Name;
            return Request.UserHostAddress + "," + CurrentUserIP();
        }
    }

    public static string CurrentUserIP()
    {
        return HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];
    }


    protected bool CanCreate { get; private set; }

    protected bool CanRead { get; private set; }

    protected bool CanDelete { get; private set; }

    protected bool CanUpdate { get; private set; }

    protected void SetPageTitle(string title)
    {
        Page.Title = title + ContextConstant.APPLICATION_NAME;
    }

    protected DateTime Now
    {
        [DebuggerStepThrough]
        get
        {
            return DateTime.Now;
        }
    }

    protected DateTime MinimumDate
    {
        [DebuggerStepThrough]
        get
        {
            return Convert.ToDateTime(ContextConstant.MINIMUM_DATE);
        }
    }

    #endregion

    #region Methods

    public static string RemoveWhitespaceFromHtml(string html)
    {

        html = RegexBetweenTags2.Replace(html, ">");
        //html = RegexLineBreaks.Replace(html, "<");
        return html.Trim();

    }

    //protected override void Render(HtmlTextWriter writer)
    //{

    //using (HtmlTextWriter htmlwriter = new HtmlTextWriter(new System.IO.StringWriter()))
    //{

    //    base.Render(htmlwriter);

    //    string html = htmlwriter.InnerWriter.ToString();


    //    // Trim the whitespace from the 'html' variable
    //    html = RemoveWhitespaceFromHtml(html);

    //    writer.Write(html);

    //}
    //Optimizer optimizer = new Optimizer();
    //optimizer.Compress(this.Context);

    //}

    private void RedirectToLoginPage()
    {
        int req = Request.RawUrl.Split('/').Length;
        if (req == 3)
        {
            Response.Redirect(ContextConstant.LOGIN_PAGE);
        }
        else if (req == 4)
        {
            Response.Redirect("../" + ContextConstant.LOGIN_PAGE);
        }
        else if (req == 5)
        {
            Response.Redirect("../../" + ContextConstant.LOGIN_PAGE);
        }
    }

    //public static int GetCreatorId(Page page)
    //{
    //    int creatorId = new int();//for Admin creator id is zero(0).
    //    try
    //    {

    //        return Convert.ToInt32(page.Session[ContextConstant.EMPLOYEE_ID_OF_LOGGED_IN_USER]);
    //    }
    //    catch { }
    //    return creatorId;
    //}

    public void ClearControls(ControlCollection tabsControlsCollection)
    {

        string type = string.Empty;
        foreach (Control tabControl in tabsControlsCollection)
        {
            type = tabControl.GetType().Name.ToString();
            if (type.Trim() == "Control")
            {
                string typesCol = string.Empty;
                foreach (Control control in tabControl.Controls)
                {
                    type = control.GetType().Name.ToString();
                    switch (type)
                    {
                        case "TextBox":
                            TextBox txtBox = (TextBox)control;
                            txtBox.Text = string.Empty;
                            break;
                        case "CheckBox":
                            CheckBox chk = (CheckBox)control;
                            chk.Checked = false;
                            break;
                        case "RadioButton":
                            RadioButton rdo = (RadioButton)control;
                            rdo.Checked = false;
                            break;
                        case "DropDownList":
                            DropDownList ddl = (DropDownList)control;
                            ddl.SelectedIndex = 0;
                            break;
                        case "HiddenField":
                            HiddenField hfField = (HiddenField)control;
                            hfField.Value = string.Empty;
                            break;
                        default:
                            break;
                    }
                    typesCol += type + Environment.NewLine;
                }
                typesCol = typesCol + "";
            }
        }
    }


    #endregion

    #region Events

    protected override void OnPreInit(EventArgs e)
    {
        base.OnPreInit(e);
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
    }

    protected override void OnLoad(EventArgs e)
    {
        if (!Context.User.Identity.IsAuthenticated)
        {
            //int req = Request.RawUrl.Split('/').Length;
            //if (req == 3)
            //{
            //    Response.Redirect("LogIn.aspx");
            //}
            //else if (req == 4)
            //{
            //    Response.Redirect("~/LogIn.aspx");
            //}
            RedirectToLoginPage();
        }
        base.OnLoad(e);
    }

    protected override void OnPreLoad(EventArgs e)
    {
        if (!Context.User.Identity.IsAuthenticated)
        {
            RedirectToLoginPage();
        }
        else
        {
            try
            {
                CurrentEmployeeId = Convert.ToInt32(Session[ContextConstant.EMPLOYEE_ID_OF_LOGGED_IN_USER]);
                CurrentMainDivisionId = Convert.ToInt32(Session[ContextConstant.MAIN_DIVISION_ID_OF_LOGGED_IN_USER]);
                CurrentDivisionId = Convert.ToInt32(Session[ContextConstant.DIVISION_ID_OF_LOGGED_IN_USER]);
                CurrentDepartmentId = Convert.ToInt32(Session[ContextConstant.DEPARTMENT_ID_OF_LOGGED_IN_USER]);

                Session[ContextConstant.EMPLOYEE_ID_OF_LOGGED_IN_USER] = CurrentEmployeeId.ToString();
                Session[ContextConstant.MAIN_DIVISION_ID_OF_LOGGED_IN_USER] = CurrentMainDivisionId.ToString();
                Session[ContextConstant.DIVISION_ID_OF_LOGGED_IN_USER] = CurrentDivisionId.ToString();
                Session[ContextConstant.DEPARTMENT_ID_OF_LOGGED_IN_USER] = CurrentDepartmentId.ToString();
            }
            catch (Exception ex)
            {
                if (ex is FormatException)
                {
                    if (Response.IsClientConnected)
                    {
                        //RedirectToLoginPage();
                    }
                }
            }

        }
        //Response.Cache.SetExpires(Now.AddMinutes(30));
        //Response.Cache.SetCacheability(HttpCacheability.Public);
        base.OnPreLoad(e);
    }

    #endregion

}
