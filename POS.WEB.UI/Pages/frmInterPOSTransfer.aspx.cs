using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using TalukderPOS.BLL;
using TalukderPOS.BusinessObjects;
using TalukderPOS.DAL;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Drawing;

public partial class Pages_frmInterPOSTransfer : System.Web.UI.Page
{
    private string userID = string.Empty;
    private string userPassword = string.Empty;
    ProductTransferToSIS_BILL BILL = new ProductTransferToSIS_BILL();
    SalesReport_BILL BILL12 = new SalesReport_BILL();
    SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
    SqlCommand cmd;
    SqlDataAdapter da = new SqlDataAdapter();
    String sql;
    SqlDataReader reader;    

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
    protected void cmdAdd_Click(object sender, EventArgs e)
    {
        string[] valid = new string[2];
        T_PROD entity = new T_PROD();
        entity.PROD_WGPG = ddlProductCategoryId.SelectedItem.Value.ToString();
        entity.PROD_SUBCATEGORY = ddlSubCategory.SelectedItem.Value.ToString();
        entity.PROD_DES = ddlDescription.SelectedItem.Value.ToString();
        entity.Branch = ddlBranch.SelectedItem.Value.ToString();        

        valid = BILL.AddValidation(entity);
        if (valid[0].ToString() == "True")
        {
            lblMessage.Text = valid[1].ToString();
            DataTable dt = new DataTable();
            DataRow workRow;
            dt.Columns.Add("PROD_WGPG");
            dt.Columns.Add("Category");
            dt.Columns.Add("PROD_SUBCATEGORY");
            dt.Columns.Add("SubCategory");
            dt.Columns.Add("PROD_DES");
            dt.Columns.Add("Description");
            dt.Columns.Add("BarCode");
            dt.Columns.Add("BranchID");
            dt.Columns.Add("Branch");
            dt.Columns.Add("CostPrice");
            dt.Columns.Add("SellPrice");

            string s = txtBarcode.InnerText;
            string[] lines = s.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);

            foreach (string value in lines)
            {
                if (! string.IsNullOrEmpty(value)) { 
                    T_PROD entity1 = BILL.FindBarcode(value);
                    if (! string.IsNullOrEmpty(entity1.OID))
                    {
                        workRow = dt.NewRow();
                        workRow["PROD_WGPG"] = ddlProductCategoryId.SelectedItem.Value;
                        workRow["Category"] = ddlProductCategoryId.SelectedItem.Text;

                        workRow["PROD_SUBCATEGORY"] = ddlSubCategory.SelectedItem.Value;
                        workRow["SubCategory"] = ddlSubCategory.SelectedItem.Text;

                        workRow["PROD_DES"] = ddlDescription.SelectedItem.Value;
                        workRow["Description"] = ddlDescription.SelectedItem.Text;
                        workRow["BarCode"] = value.Trim();
                        workRow["BranchID"] = ddlBranch.SelectedItem.Value;
                        workRow["Branch"] = ddlBranch.SelectedItem.Text;
                        workRow["CostPrice"] = entity1.CostPrice;
                        workRow["SellPrice"] = entity1.SalePrice;
                        dt.Rows.Add(workRow);
                    }      
                }
            }
            txtBarcode.InnerText = string.Empty;
            gvT_BarCode.DataSource = dt;
            gvT_BarCode.DataBind();
        }
        else
        {
            lblMessage.Text = valid[1].ToString();
            lblMessage.ForeColor = Color.Red;
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {        
        if (gvT_BarCode.Rows.Count > 0)
        {
            for (int i = 0; i < gvT_BarCode.Rows.Count; i++)
            {
                T_PROD entity = new T_PROD();
                entity.PROD_WGPG = ((Label)gvT_BarCode.Rows[i].FindControl("lblPROD_WGPG")).Text;
                entity.PROD_SUBCATEGORY = ((Label)gvT_BarCode.Rows[i].FindControl("lblPROD_SUBCATEGORY")).Text;
                entity.PROD_DES = ((Label)gvT_BarCode.Rows[i].FindControl("lblPROD_DES")).Text;
                entity.Barcode = ((Label)gvT_BarCode.Rows[i].FindControl("lblBarCode")).Text;
                entity.Branch = ((Label)gvT_BarCode.Rows[i].FindControl("lblBranchID")).Text;
                entity.BranchText = ((Label)gvT_BarCode.Rows[i].FindControl("lblBranch")).Text;                
                entity.CostPrice = ((Label)gvT_BarCode.Rows[i].FindControl("lblCostPrice")).Text;
                entity.SalePrice = ((Label)gvT_BarCode.Rows[i].FindControl("lblSellPrice")).Text;                
                entity.Quantity = "1";
                entity.IUSER = userID;
                entity.IDAT = DateTime.Today.Date.ToString();
                entity.EUSER = userID;
                entity.EDAT = DateTime.Today.Date.ToString();
                string found = BILL.CheckProductonSIS(entity.Barcode);
                if (string.IsNullOrEmpty(found))
                {
                    BILL.T_PROD_Add(entity);      
              

                }                
            }
        }
        gvT_BarCode.DataSource = null;
        gvT_BarCode.DataBind();
        lblMessage.Text = "Product Transfered Successfully";            
    }






    protected void cmdSISSearch_Click(object sender, EventArgs e)
    {
        T_PROD entity = new T_PROD();
        
        if (txtFromDate.Text == string.Empty)
        {
            entity.FromDate = DateTime.Today.Date.ToString();
        }
        else
        {
            entity.FromDate = txtFromDate.Text;
        }
        if (txtToDate.Text == string.Empty)
        {
            entity.ToDate = DateTime.Today.Date.ToString();
        }
        else
        {
            entity.ToDate = txtToDate.Text;
        }
        entity.Branch = userID;
        DataTable dt = BILL12.ProductMovement12(entity);
        GridViewsislist.DataSource = dt;
        GridViewsislist.DataBind();  
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        T_PROD entity = new T_PROD();
        string webUrl = string.Empty;
        if (txtFromDate.Text != string.Empty && txtToDate.Text != string.Empty)
        {
            if (txtFromDate.Text == string.Empty)
            {
                entity.FromDate = DateTime.Today.Date.ToString();
            }
            else
            {
                entity.FromDate = txtFromDate.Text;
            }
            if (txtToDate.Text == string.Empty)
            {
                entity.ToDate = DateTime.Today.Date.ToString();
            }
            else
            {
                entity.ToDate = txtToDate.Text;
            }
            entity.Branch = userID;
            Session["dtsales"] = BILL12.ProductMovement12(entity);
            Session["ReportPath"] = "~/Reports/rptTransfertoSIS.rpt";
                webUrl = "../Reports/ReportView.aspx";
            
            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenWindow", "window.open('" + webUrl + "','_newtab');", true);
        }
    }
}