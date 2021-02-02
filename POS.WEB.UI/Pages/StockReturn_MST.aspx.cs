using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using TalukderPOS.BusinessObjects;
using TalukderPOS.BLL;

public partial class Pages_StockReturn_MST : System.Web.UI.Page
{
    private string userID = string.Empty;
    private string userPassword = string.Empty;
    StockReturn_BILL BILL = new StockReturn_BILL();

    SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
    SqlCommand cmd;    
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
            txtBarCode.Focus();
        }
    }

    protected void txtBarcode_TextChanged(object sender, EventArgs e)
    {
        T_PROD entity = new T_PROD();
        entity.Barcode = txtBarCode.Text;
        entity.Branch = Session["StoreID"].ToString();

        if (txtBarCode.Text != string.Empty)
        {
            String found = string.Empty;
            String Message = string.Empty;

            sql = "select Barcode from V_OnTransitProduct where Barcode = '" + entity.Barcode + "' ";
            cmd = new SqlCommand(sql, dbConnect);
            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (string.IsNullOrEmpty(reader["Barcode"].ToString()))
                    {
                        found = string.Empty;
                        Message = string.Empty;
                    }
                    else
                    {
                        found = reader["Barcode"].ToString();
                        Message = "Can not Transfer, Product on Transit";
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbConnect.Close();
            }

            if (string.IsNullOrEmpty(found))
            {
                sql = "select Barcode from V_DOA where Barcode = '" + entity.Barcode + "' ";
                cmd = new SqlCommand(sql, dbConnect);
                try
                {
                    if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (string.IsNullOrEmpty(reader["Barcode"].ToString()))
                        {
                            found = string.Empty;
                            Message = string.Empty;
                        }
                        else
                        {
                            found = reader["Barcode"].ToString();
                            Message = "Can Not Transfer, Product Already on DOA Database";
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dbConnect.Close();
                }
            }

            if (string.IsNullOrEmpty(found))
            {
                lblMessage.Text = string.Empty;
                DataTable dt = BILL.txtBarcode_TextChanged(entity);
                if (dt.Rows.Count > 0)
                {
                    BindTemporaryItemsGrid(dt);
                }
                txtBarCode.Text = string.Empty;
                lblMessage.Text = string.Empty;
                txtBarCode.Focus();
            }
            else {
                lblMessage.Text = Message;
                txtBarCode.Text = string.Empty;                
                txtBarCode.Focus();
            }
        }
    }


    private void BindTemporaryItemsGrid(DataTable dt)
    {
        List<AddedItemDetailsRequisition> listAddedItemDetails = new List<AddedItemDetailsRequisition>();
        AddedItemDetailsRequisition objAddedItemDetails;
        int sameid = 0;

        if (gvStockReturn.Rows.Count > 0)
        {
            for (int i = 0; i < gvStockReturn.Rows.Count; i++)
            {
                objAddedItemDetails = new AddedItemDetailsRequisition();
                string dtbarID = dt.Rows[0]["Barcode"].ToString();
                string BarID = ((Label)gvStockReturn.Rows[i].FindControl("lblBarcode")).Text;
                if (dtbarID == BarID)
                {
                    objAddedItemDetails.PCategoryID = ((Label)gvStockReturn.Rows[i].FindControl("lblCategoryId")).Text;
                    objAddedItemDetails.PCategoryName = ((Label)gvStockReturn.Rows[i].FindControl("lblCategory")).Text;
                    objAddedItemDetails.SubCategoryID = ((Label)gvStockReturn.Rows[i].FindControl("lblSubCategoryID")).Text;
                    objAddedItemDetails.SubCategory = ((Label)gvStockReturn.Rows[i].FindControl("lblSubCategory")).Text;
                    objAddedItemDetails.DescriptionID = ((Label)gvStockReturn.Rows[i].FindControl("lblDescriptionID")).Text;
                    objAddedItemDetails.Description = ((Label)gvStockReturn.Rows[i].FindControl("lblDescription")).Text;
                    objAddedItemDetails.Barcode = ((Label)gvStockReturn.Rows[i].FindControl("lblBarcode")).Text;
                    objAddedItemDetails.StockInHand = ((Label)gvStockReturn.Rows[i].FindControl("lblStockInHand")).Text;
                    int Qty = Convert.ToInt32(((TextBox)gvStockReturn.Rows[i].FindControl("lblQtyPcs")).Text);                    
                    objAddedItemDetails.QtyPcs = Qty + 1;
                    if (((Label)gvStockReturn.Rows[i].FindControl("lblCategoryId")).Text != "111")
                    {
                        objAddedItemDetails.QtyPcs = Convert.ToInt32(objAddedItemDetails.QtyPcs) - 1;
                    }
                    Qty = Convert.ToInt32(objAddedItemDetails.QtyPcs);
                    listAddedItemDetails.Add(objAddedItemDetails);
                    sameid = 1;
                }
                else
                {
                    objAddedItemDetails.PCategoryID = ((Label)gvStockReturn.Rows[i].FindControl("lblCategoryId")).Text;
                    objAddedItemDetails.PCategoryName = ((Label)gvStockReturn.Rows[i].FindControl("lblCategory")).Text;
                    objAddedItemDetails.SubCategoryID = ((Label)gvStockReturn.Rows[i].FindControl("lblSubCategoryID")).Text;
                    objAddedItemDetails.SubCategory = ((Label)gvStockReturn.Rows[i].FindControl("lblSubCategory")).Text;
                    objAddedItemDetails.DescriptionID = ((Label)gvStockReturn.Rows[i].FindControl("lblDescriptionID")).Text;
                    objAddedItemDetails.Description = ((Label)gvStockReturn.Rows[i].FindControl("lblDescription")).Text;
                    objAddedItemDetails.Barcode = ((Label)gvStockReturn.Rows[i].FindControl("lblBarcode")).Text;
                    objAddedItemDetails.StockInHand = ((Label)gvStockReturn.Rows[i].FindControl("lblStockInHand")).Text;
                    int Qty = Convert.ToInt32(((TextBox)gvStockReturn.Rows[i].FindControl("lblQtyPcs")).Text);
                    if (((Label)gvStockReturn.Rows[i].FindControl("lblCategoryId")).Text != "111")
                    {
                        objAddedItemDetails.QtyPcs = Convert.ToInt32(objAddedItemDetails.QtyPcs) - 1;
                    }

                    objAddedItemDetails.QtyPcs = Qty;
                    Qty = Convert.ToInt32(objAddedItemDetails.QtyPcs);
                    listAddedItemDetails.Add(objAddedItemDetails);
                }
            }
        }

        if (sameid == 0)
        {
            objAddedItemDetails = new AddedItemDetailsRequisition();
            objAddedItemDetails.PCategoryID = dt.Rows[0]["PCategoryID"].ToString();
            objAddedItemDetails.PCategoryName = dt.Rows[0]["PCategoryName"].ToString();
            objAddedItemDetails.SubCategoryID = dt.Rows[0]["SubCategoryID"].ToString();
            objAddedItemDetails.SubCategory = dt.Rows[0]["SubCategory"].ToString();
            objAddedItemDetails.DescriptionID = dt.Rows[0]["DescriptionID"].ToString();
            objAddedItemDetails.Description = dt.Rows[0]["Description"].ToString();
            objAddedItemDetails.Barcode = dt.Rows[0]["Barcode"].ToString();
            objAddedItemDetails.StockInHand = dt.Rows[0]["StockInHand"].ToString();
            objAddedItemDetails.QtyPcs = 1;
            listAddedItemDetails.Add(objAddedItemDetails);
            sameid = 0;
        }

        if (listAddedItemDetails.Count > 0)
        {
            ListtoDataTableConverter converter = new ListtoDataTableConverter();
            DataTable dtProperty = converter.ToDataTable(listAddedItemDetails);
            gvStockReturn.DataSource = listAddedItemDetails;
            gvStockReturn.DataBind();
            sameid = 0;
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (gvStockReturn.Rows.Count < 0 || gvStockReturn.Rows.Count == 0)
        {
            lblMessage.Text = "Sorry! Please Add Product";
            lblMessage.ForeColor = Color.Red;
            return;
        }
        else if (ddlSearchBranch.SelectedItem.Value.ToString() == string.Empty || ddlSearchBranch.SelectedItem.Value.ToString() == "0")
        {
            lblMessage.Text = "Sorry! Please Select a Branch";
            lblMessage.ForeColor = Color.Red;
            return;
        }
        else
        {
            StockReturn_BO entity = new StockReturn_BO();
            entity.StockReturnNo = "SR-" + Session["CCOM_PREFIX"].ToString() + "-" + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss");
            entity.FromStoreID = Session["StoreID"].ToString();
            entity.ToStoreID = ddlSearchBranch.SelectedItem.Value.ToString();
            entity.ApprovedStatus = "0";
            entity.ReferenceBy = txtRemarks.InnerText;
            entity.IUSER = userID;
            entity.EUSER = userID;
            String StockReturnID = BILL.Add_StockReturnMST(entity);
            if (gvStockReturn.Rows.Count > 0)
            {
                for (int i = 0; i < gvStockReturn.Rows.Count; i++)
                {
                    StockReturn_BO entity1 = new StockReturn_BO();
                    if (CheckBox1.Checked)
                    {
                        entity1.FaultyProd = "Faulty";

                    }
                    else
                    {
                        entity1.FaultyProd = "";
                    }
                    entity1.StockReturnID = StockReturnID;
                    entity1.PROD_DES = ((Label)gvStockReturn.Rows[i].FindControl("lblDescriptionID")).Text;
                    entity1.Barcode = ((Label)gvStockReturn.Rows[i].FindControl("lblBarcode")).Text;
                    entity1.RQty = ((TextBox)gvStockReturn.Rows[i].FindControl("lblQtyPcs")).Text;
                    BILL.Add_StockReturnDTL(entity1);
                }
                lblMessage.Text = "Saved successfully ";
                lblMessage.ForeColor = Color.Green;
                ClearItemDetails();
            }
            
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        txtBarCode.Text = string.Empty;
        lblOID.Value = string.Empty;
        lblMessage.Text = string.Empty;
        txtRemarks.InnerText = string.Empty;
        gvStockReturn.DataSource = null;
        gvStockReturn.DataBind();
    }
    private void ClearItemDetails()
    {
        lblOID.Value = string.Empty;
        txtRemarks.InnerText = string.Empty;
        CDD1.SelectedValue = string.Empty;
        gvStockReturn.DataSource = null;
        gvStockReturn.DataBind();
    }

  


    


    protected void Button1_Click(object sender, EventArgs e)
    {
        T_PROD entity = new T_PROD();
        entity.FromDate = txtFromDate.Text;
        entity.ToDate = txtToDate.Text;
        entity.Branch = Session["StoreID"].ToString();
        if (rbtNotReceived.Checked == true)
        {
            entity.SearchType = "0";
        }
        else
        {
            entity.SearchType = "1";
        }
        DataTable dt = BILL.GetStockReturnList(entity);
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
            Label lblStockReturnID = (Label)row.Cells[1].FindControl("lblStockReturnID");
            StockReturn_BO entity = new StockReturn_BO();
            entity.StockReturnID = lblStockReturnID.Text;
            DataTable dt = BILL.StockReturn_Detail(entity);
            GridView2.DataSource = dt;
            GridView2.DataBind();
        }
        if (e.CommandName == "Preview")
        {
            Label lblStockReturnID = (Label)row.Cells[1].FindControl("lblStockReturnID");
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

        if (e.CommandName == "Delete")
        {            
            Label lblApprovedStatus = (Label)row.Cells[5].FindControl("lblApprovedStatus");
            if (lblApprovedStatus.Text == "Y")
            {
                lblMessage1.Text = "Can not be Deleted";
            }
            else {
                Label lblStockReturnID = (Label)row.Cells[1].FindControl("lblStockReturnID");
                StockReturn_BO entity = new StockReturn_BO();
                entity.StockReturnID = lblStockReturnID.Text;
                lblMessage1.Text = BILL.DeleteStockReturn(entity);
                GridView2.DataSource = null;
                GridView2.DataBind();
            }            
        }
    }
    protected void gvStockReturn_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
    {
        DataTable dt = tblGridRow();
        dt.Rows.RemoveAt(e.RowIndex);
        gvStockReturn.DataSource = dt;
        gvStockReturn.DataBind();
    }
    protected DataTable tblGridRow()
    {
        DataTable dt = new DataTable();        
        dt.Columns.Add("PCategoryID");
        dt.Columns.Add("PCategoryName");
        dt.Columns.Add("SubCategoryID");
        dt.Columns.Add("SubCategory");
        dt.Columns.Add("DescriptionID");
        dt.Columns.Add("Description");
        dt.Columns.Add("Barcode");
        dt.Columns.Add("StockInHand");
        dt.Columns.Add("QtyPcs");
        DataRow dr;
        for (int i = 0; i < gvStockReturn.Rows.Count; i++)
        {
            dr = dt.NewRow();            
            dr["PCategoryID"] = ((Label)gvStockReturn.Rows[i].FindControl("lblCategoryId")).Text;
            dr["PCategoryName"] = ((Label)gvStockReturn.Rows[i].FindControl("lblCategory")).Text;
            dr["SubCategoryID"] = ((Label)gvStockReturn.Rows[i].FindControl("lblSubCategoryID")).Text;
            dr["SubCategory"] = ((Label)gvStockReturn.Rows[i].FindControl("lblSubCategory")).Text;
            dr["DescriptionID"] = ((Label)gvStockReturn.Rows[i].FindControl("lblDescriptionID")).Text;
            dr["Description"] = ((Label)gvStockReturn.Rows[i].FindControl("lblDescription")).Text;
            dr["Barcode"] = ((Label)gvStockReturn.Rows[i].FindControl("lblBarcode")).Text;
            dr["StockInHand"] = ((TextBox)gvStockReturn.Rows[i].FindControl("lblStockInHand")).Text;
            dr["QtyPcs"] = ((TextBox)gvStockReturn.Rows[i].FindControl("lblQtyPcs")).Text;
            dt.Rows.Add(dr);
        }
        return dt;
    }
    protected void GridView2_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
    {
        string StockReturnDetailID = GridView2.DataKeys[e.RowIndex].Value.ToString();
        lblMessage1.Text = BILL.DeleteStockReturnItem(StockReturnDetailID);
        GridView2.DataSource = null;
        GridView2.DataBind();
    }

    

    

   



   
}
