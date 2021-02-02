using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRM.WEB.UI;
using TalukderPOS.BusinessObjects;
using TalukderPOS.BLL;
using TalukderPOS.DAL;
using System.Text;
using System.Data;
using AjaxControlToolkit;
using System.Text.RegularExpressions;


/// <summary>
/// Summary description for CommonBinder
/// </summary>
public static class CommonBinder
{
    #region Variables
    private const string DText = "--Please Select--";
    private static string ddlText = "-- Please Select --";
    private const string DefaultDropdownText = "--Please Select--";

      

    #endregion

    public static bool CheckPageAuthentication(string originalURL="" , string userID="")
    {
        MenuHeadBLL objMenuHeadBLL = new MenuHeadBLL();
        MenuPageBLL objMenuPageBLL = new MenuPageBLL();
        MenuPermissionBLL objMenuPermissionBLL = new MenuPermissionBLL();
        
        DataTable DtForIsCommonOrInternalPage = objMenuPermissionBLL.MenuPermissionCheck(userID, originalURL);


        if (DtForIsCommonOrInternalPage.Rows.Count > 0)
            return true;
        else
            return false;

    }
    public static void LoadDDLUser(DropDownList ddl)
    {
        UserBLL objUserBLL = new UserBLL();
        string storeid = HttpContext.Current.Session["StoreID"].ToString();
        DataTable dt = objUserBLL.User_GetAllForDDL(storeid);
        if (dt.Rows.Count > 0)
        {
            ddl.DataSource = dt;
            ddl.DataTextField = dt.Columns["UserName"].ToString();
            ddl.DataValueField = dt.Columns["UserId"].ToString();
            ddl.DataBind();
        }
        else
        {
            dt = new DataTable();
            dt.Columns.Add("UserName", Type.GetType("System.String"));
            dt.Columns.Add("UserId", Type.GetType("System.Int32"));
            ddl.DataSource = dt;
            ddl.DataBind();
        }
        ddl.Items.Insert(0, ddlText);
    }

    public static void BindSupplierList(DropDownList ddl)
    {
        //T_CCOMBLL obj_CCOMBLL = new T_CCOMBLL();
        //List<T_CCOM> CCOMList = obj_CCOMBLL.T_CCOM_GetAll();//obj_ProductBLL.TblProduct_GetAll();
        //T_CCOM entity = new T_CCOM();
        //entity.OID = "0";
        //entity.CCOM_NAME = DText;
        //CCOMList.Insert(0, entity);
        //ddl.DataSource = CCOMList;
        //ddl.DataTextField = "CCOM_NAME";
        //ddl.DataValueField = "OID";
        //// Others valus are not set here
        //ddl.DataBind();
    }

    public static void BindTblProductCategoryList(DropDownList ddl)
    {
        //T_WGPGBLL obj_CategoryBLL = new T_WGPGBLL();
        //List<T_WGPG> CategoryList = obj_CategoryBLL.T_WGPG_GetAll();//obj_ProductBLL.TblProduct_GetAll();
        //T_WGPG entity = new T_WGPG();
        //entity.OID = "0";
        //entity.WGPG_NAME = DText;
        //CategoryList.Insert(0, entity);
        //ddl.DataSource = CategoryList;
        //ddl.DataTextField = "WGPG_NAME";
        //ddl.DataValueField = "OID";
        //// Others valus are not set here
        //ddl.DataBind();
    }


    public static void BindTblChildCompany(DropDownList ddl)
    {
        //T_CCOMBLL obj_CompanyBLL = new T_CCOMBLL();
        //List<T_CCOM> CompanyList = obj_CompanyBLL.T_CCOM_GetAll();//obj_ProductBLL.TblProduct_GetAll();
        //T_CCOM entity = new T_CCOM();
        //entity.OID = "0";
        //entity.CCOM_NAME = DText;
        //CompanyList.Insert(0, entity);
        //ddl.DataSource = CompanyList;
        //ddl.DataTextField = "CCOM_NAME";
        //ddl.DataValueField = "OID";
        //// Others valus are not set here
        //ddl.DataBind();
    }

    public static void BindInvUnitList(DropDownList ddl)
    {
        //T_UNITBLL obj_UnitBLL = new T_UNITBLL();
        //List<T_UNIT> UnitList = obj_UnitBLL.T_UNIT_GetAll(); //obj_ProductBLL.TblProduct_GetAll();
        //T_UNIT entity = new T_UNIT();
        //entity.OID = "0";
        //entity.UNIT_NAME  = DText;
        //UnitList.Insert(0, entity);
        //ddl.DataSource = UnitList;
        //ddl.DataTextField = "UNIT_NAME";
        //ddl.DataValueField = "OID";
        //// Others valus are not set here
        //ddl.DataBind();
    }

     
    public static string NumberToCurrencyText(decimal number)
    {
        // Round the value just in case the decimal value is longer than two digits
        number = decimal.Round(number, 2);

        string wordNumber = string.Empty;

        // Divide the number into the whole and fractional part strings
        string[] arrNumber = number.ToString().Split('.');

        // Get the whole number text
        //long wholePart = long.Parse(arrNumber[0]);
        //string strWholePart = NumberToText(wholePart);
        long wholePart = long.Parse(arrNumber[0]);
        string strWholePart = NumberToText(wholePart);

        // For amounts of zero dollars show 'No Dollars...' instead of 'Zero Dollars...'
        //wordNumber = (wholePart == 0 ? "No" : strWholePart) + (wholePart == 1 ? " Taka and " : " Taka and ");
        wordNumber = (wholePart == 0 ? "" : strWholePart + " Taka ");

        // If the array has more than one element then there is a fractional part otherwise there isn't
        // just add 'No Cents' to the end
        if (arrNumber.Length > 1)
        {
            // If the length of the fractional element is only 1, add a 0 so that the text returned isn't,
            // 'One', 'Two', etc but 'Ten', 'Twenty', etc.
            //long fractionPart = long.Parse((arrNumber[1].Length == 1 ? arrNumber[1] + "0" : arrNumber[1]));
            //string strFarctionPart = NumberToText(fractionPart);
            //int fractionPart = Int32.Parse((arrNumber[1].Length == 1 ? arrNumber[1] + "0" : arrNumber[1]));
            long fractionPart = long.Parse(arrNumber[1]);
            string strFarctionPart = NumberToText(fractionPart);

            //wordNumber += (fractionPart == 0 ? " No" : strFarctionPart) + (fractionPart == 1 ? " Paisa" : " Paisa");
            wordNumber += (fractionPart == 0 ? " only" : "and " + strFarctionPart + " Paisa only");
        }
        else
            wordNumber += "only";

        return wordNumber;
    }


    public static string NumberToText(long number)   // should work perfectly upto less than 1000 crore
    {
        if (number == 0) return "Zero";
        string and = "and ";
        //string and = useAnd ? "and " : ""; // deals with using 'and' separator
        //if (number == -2147483648) return "Minus Two Hundred " + and + "Fourteen Crore Seventy Four Lakh Eighty Three Thousand Six Hundred " + and +"Forty Eight";
        long[] num = new long[4];
        int first = 0;
        int u, h, t;
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        if (number < 0)
        {
            sb.Append("Minus ");
            number = -number;
        }
        string[] words0 = { "", "One ", "Two ", "Three ", "Four ", "Five ", "Six ", "Seven ", "Eight ", "Nine " };
        string[] words1 = { "Ten ", "Eleven ", "Twelve ", "Thirteen ", "Fourteen ", "Fifteen ", "Sixteen ", "Seventeen ", "Eighteen ", "Nineteen " };
        string[] words2 = { "Twenty ", "Thirty ", "Forty ", "Fifty ", "Sixty ", "Seventy ", "Eighty ", "Ninety " };
        string[] words3 = { "Thousand ", "Lakh ", "Crore " };
        num[0] = number % 1000; // units
        num[1] = number / 1000;
        num[2] = number / 100000;
        num[1] = num[1] - 100 * num[2]; // thousands
        num[3] = number / 10000000; // crores
        num[2] = num[2] - 100 * num[3]; // lakhs
        for (int i = 3; i > 0; i--)
        {
            if (num[i] != 0)
            {
                first = i;
                break;
            }
        }
        for (int i = first; i >= 0; i--)
        {
            if (num[i] == 0) continue;
            u = (int)num[i] % 10; // ones 
            t = (int)num[i] / 10;
            h = (int)num[i] / 100; // hundreds
            t = t - 10 * h; // tens
            if (h > 0) sb.Append(words0[h] + "Hundred ");

            if (u > 0 || t > 0)
            {
                //if (h > 0 || i < first) sb.Append(and);

                if (t == 0)
                    sb.Append(words0[u]);
                else if (t == 1)
                    sb.Append(words1[u]);
                else
                    sb.Append(words2[t - 2] + words0[u]);
            }
            if (i != 0) sb.Append(words3[i - 1]);
        }
        string temp = sb.ToString().TrimEnd();

        return temp;
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


    public static void BindDropdownList(DropDownList ddl,string sql)
    {
        SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
        SqlCommand cmd;
        SqlDataAdapter da = new SqlDataAdapter();
        DataTable dt = new DataTable();
        cmd = new SqlCommand(sql, dbConnect);
        da.SelectCommand = cmd;
        da.Fill(dt);
        if (dt.Rows.Count > 0) {
            ddl.DataSource = dt;
            ddl.DataTextField = "Name";
            ddl.DataValueField = "OID";
            ddl.DataBind();
        }
        else
        {
            dt = new DataTable();
            dt.Columns.Add("Name", Type.GetType("System.String"));
            dt.Columns.Add("OID", Type.GetType("System.Int32"));
            ddl.DataSource = dt;
            ddl.DataBind();
        }

        ddl.Items.Insert(0,new ListItem(ddlText, "0"));
    }

    public static DataTable getDataTable(string sql) {

        SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
        SqlCommand cmd;
        SqlDataAdapter da = new SqlDataAdapter();
        DataTable dt = new DataTable();
        cmd = new SqlCommand(sql, dbConnect);
        da.SelectCommand = cmd;
        da.Fill(dt);
        return dt;

    }


    

    

}

