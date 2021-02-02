using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TalukderPOS.DAL;
public partial class Pages_SearchInvoiceForReturn : System.Web.UI.Page
{
    private string userID = string.Empty;
    private string userPassword = string.Empty;
    SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
    String sql;
    SqlCommand cmd;
    SqlDataAdapter da;
    private string ShopID = string.Empty;
    CommonDAL DAL = new CommonDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            userID = Session["UserID"].ToString();
            userPassword = Session["Password"].ToString();
            ShopID = Session["StoreID"].ToString();
            ace_txtCustomerName.ContextKey = ShopID;
            ace_txtInvoiceNo.ContextKey = ShopID;
            hdfCustomerId.Value = string.Empty;
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
            lblMessage.Text = string.Empty;
        }

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        cmd = new SqlCommand("SPP_Invoice_List", dbConnect);
        cmd.Parameters.Add("@StoreID", SqlDbType.VarChar, 100).Value = Session["StoreID"].ToString();

        if (txtDateFrom.Text != string.Empty & txtDateTo.Text != string.Empty)
        {
            lblMessage.Text = string.Empty;
            cmd.Parameters.Add("@DateFrom", SqlDbType.Date).Value = txtDateFrom.Text;
            cmd.Parameters.Add("@DateTo", SqlDbType.Date).Value = txtDateTo.Text;
            cmd.CommandType = CommandType.StoredProcedure;
            da = new SqlDataAdapter(cmd);
            da.SelectCommand.CommandTimeout = 300;
            da.Fill(dt);
        }
        else {
            lblMessage.Text = "Please Select Invoice Date";
        }
        gvT_Issue_REQUISITION_DTL.DataSource = dt;
        gvT_Issue_REQUISITION_DTL.DataBind();
    }

    private string Param() 
    {
        string Param = string.Empty;
        if (!string.IsNullOrEmpty(hdfCustomerId.Value)) 
        {
            Param += string.Format(@" And C.ID={0}",hdfCustomerId.Value);
        }
        if (!string.IsNullOrEmpty(txtInvoiceNo.Text))
        {
            Param += string.Format(@" And SM.InvoiceNo='{0}'", txtInvoiceNo.Text);
        }
        if (!string.IsNullOrEmpty(txtIMENoSearch.Text))
        {
            Param += string.Format(@" And SD.Barcode='{0}'", txtIMENoSearch.Text);
        }
        return Param;
    }


    protected void txtCustomerName_TextChanged(object sender, EventArgs e)
    {
        //DataTable dt = new DataTable();
        //cmd = new SqlCommand("SPP_Invoice_List_Search", dbConnect);
        //cmd.Parameters.Add("@StoreID", SqlDbType.VarChar, 100).Value = Session["StoreID"].ToString();

        //if (txtCustomerName.Text != string.Empty)
        //{
        //    cmd.Parameters.Add("@CustomerName", SqlDbType.VarChar, 100).Value = txtCustomerName.Text;
        //    cmd.Parameters.Add("@MobileNo", SqlDbType.VarChar, 100).Value = null;
        //    cmd.Parameters.Add("@InvoiceNo", SqlDbType.VarChar, 100).Value = null;
        //    cmd.Parameters.Add("@IMENo", SqlDbType.VarChar, 100).Value = null;
        //}
        //else if (txtMobileNo.Text != string.Empty)
        //{
        //    cmd.Parameters.Add("@CustomerName", SqlDbType.VarChar, 100).Value = null;
        //    cmd.Parameters.Add("@MobileNo", SqlDbType.VarChar, 100).Value = txtMobileNo.Text;
        //    cmd.Parameters.Add("@InvoiceNo", SqlDbType.VarChar, 100).Value = null;
        //    cmd.Parameters.Add("@IMENo", SqlDbType.VarChar, 100).Value = null;
        //}
        //else if (txtInvoiceNo.Text != string.Empty)
        //{
        //    cmd.Parameters.Add("@CustomerName", SqlDbType.VarChar, 100).Value = null;
        //    cmd.Parameters.Add("@MobileNo", SqlDbType.VarChar, 100).Value = null;
        //    cmd.Parameters.Add("@InvoiceNo", SqlDbType.VarChar, 100).Value = txtInvoiceNo.Text;
        //    cmd.Parameters.Add("@IMENo", SqlDbType.VarChar, 100).Value = null;
        //}
        //else if (txtIMENoSearch.Text != string.Empty)
        //{
        //    cmd.Parameters.Add("@CustomerName", SqlDbType.VarChar, 100).Value = null;
        //    cmd.Parameters.Add("@MobileNo", SqlDbType.VarChar, 100).Value = null;
        //    cmd.Parameters.Add("@InvoiceNo", SqlDbType.VarChar, 100).Value = null;
        //    cmd.Parameters.Add("@IMENo", SqlDbType.VarChar, 100).Value = txtIMENoSearch.Text.Trim();
        //}

        //cmd.CommandType = CommandType.StoredProcedure;
        //da = new SqlDataAdapter(cmd);
        //da.SelectCommand.CommandTimeout = 300;
        //da.Fill(dt);
        //gvT_Issue_REQUISITION_DTL.DataSource = dt;
        //gvT_Issue_REQUISITION_DTL.DataBind();
        hdfCustomerId.Value = string.Empty;
        if (txtCustomerName.Text.Contains(":") && txtCustomerName.Text.Contains("(")) 
        {
            string txt = txtCustomerName.Text;
            hdfCustomerId.Value = txt.Substring(0, txt.IndexOf(":"));
            txtCustomerName.Text = txt.Substring(txt.IndexOf(":") + 1, (txt.IndexOf("(") - txt.IndexOf(":"))-2);
            txtMobileNo.Text = txt.Substring(txt.IndexOf("(") + 1,11);
        }
        else
        {
            txtCustomerName.Text = string.Empty;
        }
    }
    private bool IsCheckAble() 
    {
        int count = 0;
        bool IsValid = false;
        for (int i = 0; i < gvT_Issue_REQUISITION_DTL.Rows.Count; i++)
        {
            CheckBox chkbx = (CheckBox)gvT_Issue_REQUISITION_DTL.Rows[i].FindControl("chkReturn");
            if (chkbx != null)
            {
                if (chkbx.Checked)
                { 
                    count= count+1;
                    IsValid = true;
                    break;
                }
            }
        }
        return IsValid;
    }
    protected void btnReturn_Cliked(object sender, EventArgs e) 
    {
        // Return Code Here..
        //.........
        if (!IsCheckAble()) 
        {
            Alert.ShowMessage("Please Select At Least One Item..");
            return;
        }

        Button btnReturn=(Button)gvT_Issue_REQUISITION_DTL.HeaderRow.FindControl("btnReturn");
        btnReturn.Enabled = false;

        for (int i = 0; i < gvT_Issue_REQUISITION_DTL.Rows.Count; i++)
        {
            CheckBox chkbx = (CheckBox)gvT_Issue_REQUISITION_DTL.Rows[i].FindControl("chkReturn");
            if (chkbx != null) 
            {
                if (chkbx.Checked) 
                {
                    long SlNo = Convert.ToInt64(gvT_Issue_REQUISITION_DTL.DataKeys[i].Values["SlNo"].ToString());
                    string InvoiceNo = gvT_Issue_REQUISITION_DTL.DataKeys[i].Values["InvoiceNo"].ToString();
                    string Barcode = gvT_Issue_REQUISITION_DTL.DataKeys[i].Values["Barcode"].ToString();
                    string IUSER = Session["UserID"].ToString();
                    string Branch = Session["StoreID"].ToString();

                    cmd = new SqlCommand("spInvoiceProductReturn", dbConnect);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@SlNo", SqlDbType.BigInt).Value = SlNo;
                    cmd.Parameters.Add("@Branch", SqlDbType.NVarChar, 100).Value = Branch;
                    cmd.Parameters.Add("@InvoiceNo", SqlDbType.NVarChar, 100).Value = InvoiceNo;
                    cmd.Parameters.Add("@Barcode", SqlDbType.NVarChar, 100).Value = Barcode;
                    cmd.Parameters.Add("@IUSER", SqlDbType.NVarChar, 100).Value = userID;

                    dbConnect.Open();
                    cmd.ExecuteNonQuery();
                    dbConnect.Close();
                }
            }
        }
        gvT_Issue_REQUISITION_DTL.DataSource = null;
        gvT_Issue_REQUISITION_DTL.DataBind();
        Session["DTReturn"] = null;
        
    }

    protected void gvT_Issue_REQUISITION_DTL_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
    {
        DataTable dtsales = new DataTable();
        cmd = new SqlCommand("SPP_GetInvoice", dbConnect);
        cmd.Parameters.Add("@InvoiceNo", SqlDbType.VarChar, 100).Value = ((System.Web.UI.WebControls.Label)gvT_Issue_REQUISITION_DTL.Rows[e.NewEditIndex].FindControl("lblInvoiceNo")).Text;
        cmd.CommandType = CommandType.StoredProcedure;
        da = new SqlDataAdapter(cmd);
        da.SelectCommand.CommandTimeout = 300;
        da.Fill(dtsales);


        Session["dtsales"] = dtsales;
        string sqlhalfinvoice = string.Format(@"select * from Half_Invoice where SHOP_ID='{0}' ", ShopID);
        DataTable dthalf = DAL.LoadDataByQuery(sqlhalfinvoice);
        if (dthalf.Rows.Count > 0)
        {
            if (Session["StoreID"].ToString() == "5")
            {
                Session["ReportPath"] = "~/Reports/rptInvoiceHalfDristy.rpt";
            }
            else if (Session["StoreID"].ToString() == "54" || Session["StoreID"].ToString() == "57" || Session["StoreID"].ToString() == "67")
            {
                Session["ReportPath"] = "~/Reports/rptInvoiceHalfAponJhon.rpt";
            }
            else if (Session["StoreID"].ToString() == "71")
            {
                Session["ReportPath"] = "~/Reports/rptInvoicePOSPrinter.rpt";
            }
            /*
            else if (Session["StoreID"].ToString() == "81")
            {
                Session["ReportPath"] = "~/Reports/rptInvoiceHalfHalimaIT.rpt";
            }*/
            else
            {
                Session["ReportPath"] = "~/Reports/rptInvoiceHalf.rpt";
            }
        }



        else { Session["ReportPath"] = "~/Reports/rptInvoice.rpt"; }

        string webUrl = "../Reports/ReportView.aspx";        
        ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenWindow", "window.open('" + webUrl + "','_newtab');", true);
    }
    
    protected string RenderPriority(object dbValue)
    {
        string strReturn = string.Empty;
        if (dbValue != null)
        {
            int intValue = Convert.ToInt16(dbValue);
            switch (intValue)
            {
                case 0:
                    strReturn = "N";
                    break;
                case 1:
                    strReturn = "Y";
                    break;
            }
        }
        return strReturn;
    }

    protected void chkAll_CheckedChanged(object sender, EventArgs e) 
    {
        CheckBox chkAll = (CheckBox)gvT_Issue_REQUISITION_DTL.HeaderRow.FindControl("chkAll");   
        if(chkAll.Checked)
        {
            if (gvT_Issue_REQUISITION_DTL.Rows.Count > 0) 
            {
                for (int i = 0; i < gvT_Issue_REQUISITION_DTL.Rows.Count; i++)
			    {
                    CheckBox chkReturn = (CheckBox)gvT_Issue_REQUISITION_DTL.Rows[i].FindControl("chkReturn");
                    chkReturn.Checked = true;
			    }
            }
        }
        else
        {
            if (gvT_Issue_REQUISITION_DTL.Rows.Count > 0)
            {
                for (int i = 0; i < gvT_Issue_REQUISITION_DTL.Rows.Count; i++)
                {
                    CheckBox chkReturn = (CheckBox)gvT_Issue_REQUISITION_DTL.Rows[i].FindControl("chkReturn");
                    chkReturn.Checked = false;
                }
            }
        }
    }

    protected void txtIMENoSearch_TextChanged(object sender, EventArgs e)
    {
        string strSql = string.Empty;
        DataRow dr = null;
        if (txtIMENoSearch.Text.Trim() != "") 
        {
            DataTable DTReturn = new DataTable();
            if (Session["DTReturn"] != null)
            {
                DTReturn = (DataTable)Session["DTReturn"];
            }
            strSql = string.Format(@"Select SD.SlNo,SM.InvoiceNo,C.Name 'RetailerName',C.Address,'0'+Cast(C.Number as nvarchar) 'MobileNo'
        ,Convert(nvarchar(15),SM.IDAT,106) 'InvoiceDate',SD.Barcode,Des.Description 
        From T_SALES_DTL SD
        Inner Join T_SALES_MST SM On SD.InvoiceNo = SM.InvoiceNo
        Inner Join Description Des On Des.OID = SD.DescriptionID 
        Inner Join Customers C On SM.RetailerID = C.ID Where 1=1 And SM.DropStatus=0 And SD.Barcode='{0}'", txtIMENoSearch.Text.Trim());
            DataTable dt = new DataTable();
            cmd = new SqlCommand(strSql, dbConnect);
            cmd.CommandType = CommandType.Text;
            da = new SqlDataAdapter(cmd);
            da.SelectCommand.CommandTimeout = 300;
            da.Fill(dt);

            if (gvT_Issue_REQUISITION_DTL.Rows.Count > 0)
            {
                if (DTReturn == null || DTReturn.Rows.Count ==0)
                {
                    gvT_Issue_REQUISITION_DTL.DataSource = dt;
                    gvT_Issue_REQUISITION_DTL.DataBind();
                    Session["DTReturn"] = dt;
                }
                else 
                {
                    if (dt.Rows.Count > 0) 
                    {
                        dr = DTReturn.NewRow();
                        dr["SlNo"] = dt.Rows[0]["SlNo"].ToString();
                        dr["InvoiceNo"] = dt.Rows[0]["InvoiceNo"].ToString();
                        dr["RetailerName"] = dt.Rows[0]["RetailerName"].ToString();
                        dr["Address"] = dt.Rows[0]["Address"].ToString();
                        dr["MobileNo"] = dt.Rows[0]["MobileNo"].ToString();
                        dr["InvoiceDate"] = dt.Rows[0]["InvoiceDate"].ToString();
                        dr["Barcode"] = dt.Rows[0]["Barcode"].ToString();
                        dr["Description"] = dt.Rows[0]["Description"].ToString();
                        DTReturn.Rows.Add(dr);
                        gvT_Issue_REQUISITION_DTL.DataSource = DTReturn;
                        gvT_Issue_REQUISITION_DTL.DataBind();
                        Session["DTReturn"] = DTReturn;
                    }
                }
            }
            else
            {
                gvT_Issue_REQUISITION_DTL.DataSource = dt;
                gvT_Issue_REQUISITION_DTL.DataBind();
                Session["DTReturn"] = dt;
            }
        }
        txtIMENoSearch.Focus();
        txtIMENoSearch.Text = string.Empty;
    }

    protected void btnIssue_Click(object sender, EventArgs e)
    {
        lblMessage.Text = string.Empty;
        //txtCustomerName_TextChanged(sender, e);
        string strSql = string.Format(@"
        Select SD.SlNo,SM.InvoiceNo,C.Name 'RetailerName',C.Address,'0'+Cast(C.Number as nvarchar) 'MobileNo'
        ,Convert(nvarchar(15),SM.IDAT,106) 'InvoiceDate',SD.Barcode,Des.Description 
        From T_SALES_DTL SD
        Inner Join T_SALES_MST SM On SD.InvoiceNo = SM.InvoiceNo
        Inner Join Description Des On Des.OID = SD.DescriptionID 
        Inner Join Customers C On SM.RetailerID = C.ID Where 1=1 And SM.DropStatus=0 {0} ", Param());

        DataTable dt = new DataTable();
        cmd = new SqlCommand(strSql, dbConnect);
        cmd.CommandType = CommandType.Text;
        da = new SqlDataAdapter(cmd);
        da.SelectCommand.CommandTimeout = 300;
        da.Fill(dt);
        Session["DTReturn"] = dt;
        gvT_Issue_REQUISITION_DTL.DataSource = dt;
        gvT_Issue_REQUISITION_DTL.DataBind();
    }

    protected void btnReport_Click(object sender, EventArgs e)
    {
        string WhereCondition = string.Empty;

        WhereCondition += string.Format(@" And SM.StoreID='{0}'", Session["StoreID"].ToString());

        if (hdfCustomerId.Value.Trim() != "") 
        {
            WhereCondition += string.Format(@" And SM.RetailerID='{0}'", hdfCustomerId.Value);
        }
        if (txtInvoiceNo.Text.Trim() != "") 
        {
            WhereCondition += string.Format(@" And SM.InvoiceNo='{0}'", txtInvoiceNo.Text.Trim());
        }
        if (txtIMENoSearch.Text.Trim() != "") 
        {
            WhereCondition += string.Format(@" And SDD.Barcode='{0}'", txtIMENoSearch.Text.Trim());
        }

        if (dptFromDate.Text.Trim() != "" && dptToDate.Text.Trim() != "") 
        {
            WhereCondition += string.Format(@" And SDD.RemovedDate Between '{0}' And '{1}'", txtDateFrom.Text.Trim(), txtDateTo.Text.Trim());
        }

        //else if (txtDateFrom.Text.Trim() != "")
        //{
        //    WhereCondition += string.Format(@" And SDD.RemovedDate ='{0}'", txtDateFrom.Text.Trim());
        //}
        //else if (txtDateTo.Text.Trim() != "")
        //{
        //    WhereCondition += string.Format(@" And SDD.RemovedDate ='{0}'", txtDateFrom.Text.Trim(), txtDateTo.Text.Trim(), txtDateTo.Text.Trim());
        //}
        string fromDate = dptFromDate.Text.Trim() == "" ? "CAST(NULL as Date)" : string.Format(@"CAST('{0}' as Date)", dptFromDate.Text.Trim());
        string toDate = dptToDate.Text.Trim() == "" ? "CAST(NULL as Date)" : string.Format(@"CAST('{0}' as Date)", dptToDate.Text.Trim());

        string strSql = string.Format(@"SELECT C.Name,'0'+Cast(C.Number as varchar(20)) as Number,C.Address,C.Email,SM.RetailerID,SDD.Barcode,P.CostPrice,P.SalePrice 'Prod_SalePrice',ISNULL(SDD.DiscountAmount,0) as DiscountAmount,ISNULL(SDD.GiftAmount,0) as GiftAmount,SDD.IDAT,SDD.RemovedDate,SDD.DescriptionID
,SDD.SalePrice,SM.StoreID,S.SubCategoryName,Cat.WGPG_NAME 'CategoryName',SM.InvoiceNo,D.Description, {1} as FormDate, {2} as ToDate
FROM T_SALES_DTL_Delete SDD
INNER JOIN T_SALES_MST SM ON SDD.InvoiceNo = SM.InvoiceNo
LEFT JOIN Customers C On C.ID = SM.RetailerID
LEFT JOIN T_PROD P On SDD.Barcode = P.Barcode
LEFT JOIN SubCategory S On S.OID = P.PROD_WGPG 
LEFT JOIN T_WGPG Cat On Cat.OID = S.OID
LEFT JOIN Description D On D.OID =P.PROD_DES
WHERE 1= 1 {0}", WhereCondition, fromDate, toDate);

        DataTable dt = DAL.LoadDataByQuery(strSql);
        if (dt.Rows.Count > 0)
        {
            // Report Works....
            Session["dtsales"] = dt;
            Session["ReportPath"] = "~/Reports/rptSalesReturnDetails.rpt";

            string webUrl = "../Reports/ReportView.aspx";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenWindow", "window.open('" + webUrl + "','_newtab');", true);
        }

    }
}