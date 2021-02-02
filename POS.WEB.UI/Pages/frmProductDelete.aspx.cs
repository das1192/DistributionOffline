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

public partial class Pages_frmProductDelete : System.Web.UI.Page
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
                    }
                    else
                    {
                        found = reader["Barcode"].ToString();
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
                lblMessage.Text = string.Empty;
                DataTable dt = BILL.txtBarcode_TextChangedDeleteProduct(entity);
                if (dt.Rows.Count > 0)
                {
                    BindTemporaryItemsGrid(dt);
                }
                txtBarCode.Text = string.Empty;
                lblMessage.Text = string.Empty;
                txtBarCode.Focus();
            }
            else
            {
                lblMessage.Text = "Can not Delete, Product on Transit";
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
                    sameid = 1;
                }
                else
                {
                    objAddedItemDetails.OID = ((Label)gvStockReturn.Rows[i].FindControl("lblOID")).Text;
                    objAddedItemDetails.CCOM_NAME = ((Label)gvStockReturn.Rows[i].FindControl("lblCCOM_NAME")).Text;
                    objAddedItemDetails.WGPG_NAME = ((Label)gvStockReturn.Rows[i].FindControl("lblCategory")).Text;
                    objAddedItemDetails.SubCategoryName = ((Label)gvStockReturn.Rows[i].FindControl("lblSubCategory")).Text;
                    objAddedItemDetails.Description = ((Label)gvStockReturn.Rows[i].FindControl("lblDescription")).Text;
                    objAddedItemDetails.Barcode = ((Label)gvStockReturn.Rows[i].FindControl("lblBarcode")).Text;
                    listAddedItemDetails.Add(objAddedItemDetails);
                    sameid = 0;
                }
            }
        }

        if (sameid == 0)
        {
            objAddedItemDetails = new AddedItemDetailsRequisition();
            objAddedItemDetails.OID = dt.Rows[0]["OID"].ToString();
            objAddedItemDetails.CCOM_NAME = dt.Rows[0]["CCOM_NAME"].ToString();
            objAddedItemDetails.WGPG_NAME = dt.Rows[0]["WGPG_NAME"].ToString();
            objAddedItemDetails.SubCategoryName = dt.Rows[0]["SubCategoryName"].ToString();
            objAddedItemDetails.Description = dt.Rows[0]["Description"].ToString();
            objAddedItemDetails.Barcode = dt.Rows[0]["Barcode"].ToString();
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
        else
        {            
            T_PRODBLL BILL = new T_PRODBLL();           
           
            if (gvStockReturn.Rows.Count > 0)
            {
                for (int i = 0; i < gvStockReturn.Rows.Count; i++)
                {
                    T_PROD entity = new T_PROD();
                    entity.OID = ((Label)gvStockReturn.Rows[i].FindControl("lblOID")).Text;
                    entity.Barcode = ((Label)gvStockReturn.Rows[i].FindControl("lblBarcode")).Text;
                    entity.ActiveStatus = "0";
                    entity.InActiveReason = string.Empty;
                    entity.EUSER = userID;
                    entity.EDAT = DateTime.Today.Date.ToString();
                    BILL.SPP_DeleteProduct(entity);                   
                }
                lblMessage.Text = "Deleted Successfully ";
                lblMessage.ForeColor = Color.Green;
                ClearItemDetails();
            }
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        txtBarCode.Text = string.Empty;
        lblMessage.Text = string.Empty;        
        gvStockReturn.DataSource = null;
        gvStockReturn.DataBind();
    }

    private void ClearItemDetails()
    {
        txtBarCode.Text = string.Empty;        
        gvStockReturn.DataSource = null;
        gvStockReturn.DataBind();
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
        dt.Columns.Add("OID");
        dt.Columns.Add("CCOM_NAME");
        dt.Columns.Add("WGPG_NAME");
        dt.Columns.Add("SubCategoryName");
        dt.Columns.Add("Description");
        dt.Columns.Add("Barcode");
        DataRow dr;
        for (int i = 0; i < gvStockReturn.Rows.Count; i++)
        {
            dr = dt.NewRow();
            dr["OID"] = ((Label)gvStockReturn.Rows[i].FindControl("lblOID")).Text;
            dr["CCOM_NAME"] = ((Label)gvStockReturn.Rows[i].FindControl("lblCCOM_NAME")).Text;
            dr["WGPG_NAME"] = ((Label)gvStockReturn.Rows[i].FindControl("lblCategory")).Text;
            dr["SubCategoryName"] = ((Label)gvStockReturn.Rows[i].FindControl("lblSubCategory")).Text;
            dr["Description"] = ((Label)gvStockReturn.Rows[i].FindControl("lblDescription")).Text;
            dr["Barcode"] = ((Label)gvStockReturn.Rows[i].FindControl("lblBarcode")).Text;
            dt.Rows.Add(dr);
        }
        return dt;
    }
    
}