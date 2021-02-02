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

public partial class Pages_frmDOA : System.Web.UI.Page
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
                        Message = "Product Can not Transfer to DOA Database, Product on Transit";
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
                            Message = "Product Already on DOA Database";
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
                DataTable dt = BILL.txtBarcode_TextChangedDOA(entity);
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
        else
        {
            T_PRODBLL BILL = new T_PRODBLL();
            if (gvStockReturn.Rows.Count > 0)
            {
                for (int i = 0; i < gvStockReturn.Rows.Count; i++)
                {
                    if (Convert.ToInt32(((TextBox)gvStockReturn.Rows[i].FindControl("lblQtyPcs")).Text) > Convert.ToInt32(((Label)gvStockReturn.Rows[i].FindControl("lblStockInHand")).Text))
                    {
                    }
                    else {
                        T_PROD entity = new T_PROD();
                        entity.PROD_WGPG = ((Label)gvStockReturn.Rows[i].FindControl("lblCategoryId")).Text;
                        entity.PROD_DES = ((Label)gvStockReturn.Rows[i].FindControl("lblDescriptionID")).Text;
                        entity.Barcode = ((Label)gvStockReturn.Rows[i].FindControl("lblBarcode")).Text;
                        entity.Quantity = ((TextBox)gvStockReturn.Rows[i].FindControl("lblQtyPcs")).Text;
                        entity.IUSER = userID;
                        entity.IDAT = DateTime.Today.Date.ToString();
                        entity.EUSER = userID;
                        entity.EDAT = DateTime.Today.Date.ToString();

                        if (entity.PROD_WGPG == "111")
                        {
                            sql = "update StoreMasterStock set Quantity = Quantity - @Quantity where PROD_DES = @PROD_DES and Barcode = @Barcode and Branch=@Branch";
                            cmd = new SqlCommand(sql, dbConnect);
                            cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = entity.Quantity;
                            cmd.Parameters.Add("@PROD_DES", SqlDbType.BigInt).Value = entity.PROD_DES;
                            cmd.Parameters.Add("@Barcode", SqlDbType.VarChar, 100).Value = entity.Barcode;
                            cmd.Parameters.Add("@Branch", SqlDbType.VarChar, 50).Value = Session["StoreID"].ToString();                            
                            try
                            {
                                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                                cmd.ExecuteScalar();
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                            finally
                            {
                                dbConnect.Close();
                            }

                            sql = "insert into DOA(DescriptionOID,BranchOID,Barcode,Quantity,Status,Remarks,IUSER,IDAT,EUSER,EDAT) values(@DescriptionOID,@BranchOID,@Barcode,@Quantity,@Status,@Remarks,@IUSER,@IDAT,@EUSER,@EDAT)";
                            cmd = new SqlCommand(sql, dbConnect);
                            cmd.Parameters.Add("@DescriptionOID", SqlDbType.BigInt).Value = entity.PROD_DES;
                            cmd.Parameters.Add("@BranchOID", SqlDbType.VarChar, 100).Value = Session["StoreID"].ToString();                            
                            cmd.Parameters.Add("@Barcode", SqlDbType.VarChar, 100).Value = entity.Barcode;
                            cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = entity.Quantity;
                            cmd.Parameters.Add("@Status", SqlDbType.VarChar,50).Value = "Active";
                            cmd.Parameters.Add("@Remarks", SqlDbType.VarChar, 200).Value = String.Empty;                            
                            cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 100).Value = entity.IUSER;
                            cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = DateTime.Now.Date;
                            cmd.Parameters.Add("@EUSER", SqlDbType.VarChar, 100).Value = entity.EUSER;
                            cmd.Parameters.Add("@EDAT", SqlDbType.Date).Value = DateTime.Now.Date;
                            try
                            {
                                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                                cmd.ExecuteScalar();
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                            finally
                            {
                                dbConnect.Close();
                            }


                            sql = "insert into StockPosting(BranchOID,DescriptionOID,Barcode,InwardQty,OutwardQty,Particulars,IUSER,IDAT) values(@BranchOID,@DescriptionOID,@Barcode,@InwardQty,@OutwardQty,@Particulars,@IUSER,@IDAT)";
                            cmd = new SqlCommand(sql, dbConnect);
                            cmd.Parameters.Add("@BranchOID", SqlDbType.VarChar, 100).Value = Session["StoreID"].ToString();
                            cmd.Parameters.Add("@DescriptionOID", SqlDbType.BigInt).Value = entity.PROD_DES;
                            cmd.Parameters.Add("@Barcode", SqlDbType.VarChar, 100).Value = entity.Barcode;
                            cmd.Parameters.Add("@InwardQty", SqlDbType.Int).Value = 0;
                            cmd.Parameters.Add("@OutwardQty", SqlDbType.Int).Value = entity.Quantity;
                            cmd.Parameters.Add("@Particulars", SqlDbType.VarChar, 100).Value = "DOA";
                            cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 100).Value = entity.EUSER;
                            cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = DateTime.Now.Date;
                            try
                            {
                                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                                cmd.ExecuteScalar();
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
                        else {
                            sql = "update StoreMasterStock set ActiveStatus=@ActiveStatus,InActiveReason=@InActiveReason,EUSER=@EUSER,EDAT=@EDAT where Barcode=@Barcode";
                            cmd = new SqlCommand(sql, dbConnect);
                            cmd.Parameters.Add("@ActiveStatus", SqlDbType.Int).Value = 0;
                            cmd.Parameters.Add("@InActiveReason", SqlDbType.VarChar, 300).Value = "DOA";                           
                            cmd.Parameters.Add("@EUSER", SqlDbType.VarChar, 100).Value = entity.EUSER;
                            cmd.Parameters.Add("@EDAT", SqlDbType.Date).Value = DateTime.Now.Date;
                            cmd.Parameters.Add("@Barcode", SqlDbType.VarChar, 100).Value = entity.Barcode;
                            try
                            {
                                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                                cmd.ExecuteScalar();
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                            finally
                            {
                                dbConnect.Close();
                            }

                            sql = "insert into DOA(DescriptionOID,BranchOID,Barcode,Quantity,Status,Remarks,IUSER,IDAT,EUSER,EDAT) values(@DescriptionOID,@BranchOID,@Barcode,@Quantity,@Status,@Remarks,@IUSER,@IDAT,@EUSER,@EDAT)";
                            cmd = new SqlCommand(sql, dbConnect);
                            cmd.Parameters.Add("@DescriptionOID", SqlDbType.BigInt).Value = entity.PROD_DES;
                            cmd.Parameters.Add("@BranchOID", SqlDbType.VarChar, 100).Value = Session["StoreID"].ToString();
                            cmd.Parameters.Add("@Barcode", SqlDbType.VarChar, 100).Value = entity.Barcode;
                            cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = entity.Quantity;
                            cmd.Parameters.Add("@Status", SqlDbType.VarChar, 50).Value = "Active";
                            cmd.Parameters.Add("@Remarks", SqlDbType.VarChar, 200).Value = String.Empty;
                            cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 100).Value = entity.IUSER;
                            cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = DateTime.Now.Date;
                            cmd.Parameters.Add("@EUSER", SqlDbType.VarChar, 100).Value = entity.EUSER;
                            cmd.Parameters.Add("@EDAT", SqlDbType.Date).Value = DateTime.Now.Date;
                            try
                            {
                                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                                cmd.ExecuteScalar();
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                            finally
                            {
                                dbConnect.Close();
                            }

                            sql = "insert into StockPosting(BranchOID,DescriptionOID,Barcode,InwardQty,OutwardQty,Particulars,IUSER,IDAT) values(@BranchOID,@DescriptionOID,@Barcode,@InwardQty,@OutwardQty,@Particulars,@IUSER,@IDAT)";
                            cmd = new SqlCommand(sql, dbConnect);
                            cmd.Parameters.Add("@BranchOID", SqlDbType.VarChar, 100).Value = Session["StoreID"].ToString();
                            cmd.Parameters.Add("@DescriptionOID", SqlDbType.BigInt).Value = entity.PROD_DES;
                            cmd.Parameters.Add("@Barcode", SqlDbType.VarChar, 100).Value = entity.Barcode;
                            cmd.Parameters.Add("@InwardQty", SqlDbType.Int).Value = 0;
                            cmd.Parameters.Add("@OutwardQty", SqlDbType.Int).Value = entity.Quantity;
                            cmd.Parameters.Add("@Particulars", SqlDbType.VarChar, 100).Value = "DOA";
                            cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 100).Value = entity.IUSER;
                            cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = DateTime.Now.Date;
                           
                            try
                            {
                                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                                cmd.ExecuteScalar();
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
                    }                    
                }
                lblMessage.Text = "DOA Transfered Successfully ";
                lblMessage.ForeColor = Color.Green;
                ClearItemDetails();
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
            dr["StockInHand"] = ((Label)gvStockReturn.Rows[i].FindControl("lblStockInHand")).Text;
            dr["QtyPcs"] = ((TextBox)gvStockReturn.Rows[i].FindControl("lblQtyPcs")).Text;
            dt.Rows.Add(dr);
        }
        return dt;
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
        gvStockReturn.DataSource = null;
        gvStockReturn.DataBind();
    }


  
}









