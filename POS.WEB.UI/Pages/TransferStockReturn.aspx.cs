using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using TalukderPOS.BusinessObjects;
using TalukderPOS.BLL;


public partial class Pages_TransferStockReturn : System.Web.UI.Page
{
    private string userID = string.Empty;
    private string userPassword = string.Empty;
    SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
    SqlCommand cmd;
    SqlDataAdapter da = new SqlDataAdapter();
    string sql;
    StockReturn_BILL BILL = new StockReturn_BILL();

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
            loadReqNo();
            Label1.Text = string.Empty;
        }
    }

    private void loadReqNo()
    {
        ddlStockReturnNo.Items.Clear();
        sql = "SELECT StockReturn_MST.StockReturnID as OID,StockReturn_MST.StockReturnNo as Name FROM StockReturn_MST where StockReturn_MST.ApprovedStatus=0 and StockReturn_MST.ToStoreID='" + Session["StoreID"].ToString() + "'";
        CommonBinder.BindDropdownList(ddlStockReturnNo, sql);
        gvStockReturnDetails.DataSource = null;
        gvStockReturnDetails.DataBind();
    }

    protected void ddlStockReturnNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlStockReturnNo.SelectedIndex == 0)
        {
            gvStockReturnDetails.DataSource = null;
            gvStockReturnDetails.DataBind();
            Label1.Text = string.Empty;
        }
        else
        {
            Label1.Text = string.Empty;
            Int32 Id = Convert.ToInt32(ddlStockReturnNo.SelectedItem.Value);
            DataTable dt = new DataTable();
            sql = "SELECT C1.OID,C1.CCOM_NAME as FromStoreID,StockReturn_MST.StockReturnID,StockReturn_MST.ApprovedStatus,StockReturn_DTL.faulty_stat,StockReturn_DTL.StockReturnDetailID,StockReturn_DTL.Barcode,T_WGPG.OID AS PCategoryID,T_WGPG.WGPG_NAME AS PCategoryName,SubCategory.OID AS SubCategoryID,SubCategory.SubCategoryName AS SubCategory,Description.OID AS DescriptionID,Description.Description,StockReturn_DTL.RQty AS QtyPcs,C2.OID as ToStoreIDOID,C2.CCOM_NAME as ToStoreID,StockReturn_MST.IDAT as TransferDate FROM StockReturn_MST inner join StockReturn_DTL on StockReturn_MST.StockReturnID=StockReturn_DTL.StockReturnID inner join Description on StockReturn_DTL.DescriptionID=Description.OID inner join SubCategory on Description.SubCategoryID=SubCategory.OID inner join T_WGPG on SubCategory.CategoryID=T_WGPG.OID inner join T_CCOM as C1 on StockReturn_MST.FromStoreID=C1.OID inner join T_CCOM as C2 on StockReturn_MST.ToStoreID=C2.OID WHERE StockReturn_DTL.StockReturnID=" + Id + "  ";
            cmd = new SqlCommand(sql, dbConnect);
            dbConnect.Open();
            da.SelectCommand = cmd;
            da.Fill(dt);
            dbConnect.Close();
            gvStockReturnDetails.DataSource = dt;
            gvStockReturnDetails.DataBind();
        }
    }


    protected void btnIssue_Click(object sender, EventArgs e)
    {
        if (Convert.ToInt16(ddlStockReturnNo.SelectedIndex) == 0 || ddlStockReturnNo.SelectedItem.Value.ToString() == string.Empty)
        {
            Label1.Text = "Select A Stock Return No";
            Label1.ForeColor = Color.Red;
        }
        else
        {
            try
            {
                DataTable dt = new DataTable();
                sql = "SELECT ApprovedStatus from StockReturn_MST where StockReturnID='" + ddlStockReturnNo.SelectedValue + "' and ApprovedStatus=0";
                cmd = new SqlCommand(sql, dbConnect);
                da.SelectCommand = cmd;
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    sql = "update StockReturn_MST set ApprovedStatus=@ApprovedStatus,EUSER=@EUSER,EDAT=@EDAT where StockReturnID='" + ddlStockReturnNo.SelectedValue + "'";
                    cmd = new SqlCommand(sql, dbConnect);
                    cmd.Parameters.Add("@ApprovedStatus", SqlDbType.Int).Value = 1;
                    cmd.Parameters.Add("@EUSER", SqlDbType.VarChar, 50).Value = userID;
                    cmd.Parameters.Add("@EDAT", SqlDbType.Date).Value = DateTime.Now.Date;
                    dbConnect.Open();
                    cmd.ExecuteNonQuery();
                    dbConnect.Close();
                    ReceiveStockReturn();

                    Label1.Text = "Product Received Successfully";
                    Label1.ForeColor = Color.Red;
                    loadReqNo();
                    gvStockReturnDetails.DataSource = null;
                    gvStockReturnDetails.DataBind();
                }
                else
                {
                    Label1.Text = "Already Received";
                    Label1.ForeColor = Color.Red;
                    loadReqNo();
                    gvStockReturnDetails.DataSource = null;
                    gvStockReturnDetails.DataBind();
                }
            }
            catch
            {
                Label1.Text = "Product Received Failed";
                Label1.ForeColor = Color.Red;
            }
        }
    }


    private void ReceiveStockReturn()
    {
        if (gvStockReturnDetails.Rows.Count > 0)
        {
            for (int i = 0; i < gvStockReturnDetails.Rows.Count; i++)
            {
                StockReturn_BO entity = new StockReturn_BO();
                entity.StockReturnDetailID = ((Label)gvStockReturnDetails.Rows[i].FindControl("lblStockReturnDetailID")).Text;
                entity.FromStoreID = ((Label)gvStockReturnDetails.Rows[i].FindControl("lblCCOM_CODE")).Text;
                entity.ToStoreID = ((Label)gvStockReturnDetails.Rows[i].FindControl("lblToStoreIDOID")).Text;
                entity.BarnchText = ((Label)gvStockReturnDetails.Rows[i].FindControl("lblToStoreID")).Text;
                entity.PROD_WGPG = ((Label)gvStockReturnDetails.Rows[i].FindControl("lblCategoryId")).Text;
                entity.PROD_SUBCATEGORY = ((Label)gvStockReturnDetails.Rows[i].FindControl("lblSubCategoryID")).Text;
                entity.PROD_DES = ((Label)gvStockReturnDetails.Rows[i].FindControl("lblDescriptionID")).Text;
                entity.Barcode = ((Label)gvStockReturnDetails.Rows[i].FindControl("lblBarcode")).Text;
                entity.RQty = ((Label)gvStockReturnDetails.Rows[i].FindControl("lblQtyPcs")).Text;
                entity.FaultyProd = ((Label)gvStockReturnDetails.Rows[i].FindControl("lblFaultyStat")).Text;
                entity.IUSER = userID;
                entity.TransferDate = ((Label)gvStockReturnDetails.Rows[i].FindControl("lblTransferDate")).Text;
                BILL.ReceiveStockReturn(entity);
            }
        }
    }

    
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        T_PROD entity = new T_PROD();
        entity.FromDate = txtFromDate.Text;
        entity.ToDate = txtToDate.Text;
        entity.Branch = Session["StoreID"].ToString();
        entity.SearchType = "0";
        DataTable dt = BILL.GetReceiveStockReturnList(entity);
        gvStockReturnList.DataSource = dt;
        gvStockReturnList.DataBind();
        GridView2.DataSource = null;
        GridView2.DataBind();
    }

    protected void StockReturn_Details(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
    {
        GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
        lblMessage1.Text = string.Empty;
        if (e.CommandName == "ItemDetails")
        {
            Label lblStockReturnID = (Label)row.Cells[0].FindControl("lblStockReturnID");
            StockReturn_BO entity = new StockReturn_BO();
            entity.StockReturnID = lblStockReturnID.Text;
            DataTable dt = BILL.StockReturn_Detail(entity);
            GridView2.DataSource = dt;
            GridView2.DataBind();
        }
        if (e.CommandName == "Preview")
        {
            Label lblStockReturnID = (Label)row.Cells[0].FindControl("lblStockReturnID");
            StockReturn_BO entity = new StockReturn_BO();
            entity.StockReturnID = lblStockReturnID.Text;
            DataTable dt = BILL.StockReturn_Detail_ForPreview(entity);
            if (dt.Rows.Count > 0)
            {
                Session["dtsales"] = dt;
                Session["ReportPath"] = "~/Reports/rptStockReturn.rpt";

                string webUrl = "../Reports/ReportView.aspx";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenWindow", "window.open('" + webUrl + "','_newtab');", true);
            }
        }
    }



    protected void Button1_Click(object sender, EventArgs e)
    {
        T_PROD entity = new T_PROD();
        entity.Branch = Session["StoreID"].ToString();
        entity.PROD_WGPG = ddlSearchProductCategory.SelectedItem.Value.ToString();
        entity.PROD_SUBCATEGORY = ddlSearchSubCategory.SelectedItem.Value.ToString();
        entity.PROD_DES = ddlSearchDescription.SelectedItem.Value.ToString();
        entity.FromDate = txtReceivedFromDate.Text;
        entity.ToDate = txtReceivedToDate.Text;
        entity.Barcode = txtSearchBarcode.Text;
        entity.FromStoreID = ddlSearchBranch.SelectedItem.Value.ToString();
        entity.SearchType = "1";
        DataTable dt = BILL.GetReceiveStockReturnListReceived(entity);
        GridView3.DataSource = dt;
        GridView3.DataBind();
    }
}