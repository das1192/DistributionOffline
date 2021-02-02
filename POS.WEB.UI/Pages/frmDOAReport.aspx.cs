using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TalukderPOS.BusinessObjects;
using TalukderPOS.BLL;

public partial class Pages_frmDOAReport : System.Web.UI.Page
{
    private string userID = string.Empty;
    private string userPassword = string.Empty;
    SalesReport_BILL BILL = new SalesReport_BILL();

    SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
    SqlCommand cmd;
    String sql;
    

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

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        T_PROD entity = new T_PROD();
        entity.Branch = ddlSearchBranch.SelectedItem.Value.ToString();
        entity.PROD_WGPG = ddlSearchProductCategory.SelectedItem.Value.ToString();
        entity.PROD_SUBCATEGORY = ddlSearchSubCategory.SelectedItem.Value.ToString();        
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
        entity.Barcode = txtBarcode.Text;
        if (rbtActive.Checked == true) {
            entity.ActiveStatus = "Active";
        }
        else if (rbtInActive.Checked == true) {
            entity.ActiveStatus = "InActive";
        }
        else if (rbtReturn.Checked == true)
        {
            entity.ActiveStatus = "Return";            
        }       

        DataTable dt = BILL.DOAList(entity);
        GridView1.DataSource = dt;
        GridView1.DataBind();  
    }

    protected void gvT_PROD_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
    {
        if (rbtActive.Checked == true) {
            lblMessage.Text = "";
            Int64 OID = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value);
            txtDeleteReason.Text = string.Empty;
            lblProductID.Text = OID.ToString();
            ModalPopupExtender1.Show();
            txtDeleteReason.Focus();        
        }
        else if (rbtInActive.Checked == true) {
            lblMessage.Text = "Can not make Delete operation on In Active Product";
        }
        else if (rbtReturn.Checked == true) {
            lblMessage.Text = "Can not make Delete operation on Return Product";
        }
        
    }

    protected void btnSubmitDiscount_Click(object sender, EventArgs e) //Modal Window Delete Product
    {
        if (txtDeleteReason.Text == string.Empty)
        {
            lblDeleteReasonMessage.Text = "Please Type Reason For Delete";
            ModalPopupExtender1.Show();
        }
        else
        {
            lblDeleteReasonMessage.Text = string.Empty;
            sql = "update DOA set Status=@Status,Remarks=@Remarks,EUSER=@EUSER,EDAT=@EDAT where OID = @OID";
            cmd = new SqlCommand(sql, dbConnect);
            cmd.Parameters.Add("@Status", SqlDbType.VarChar,50).Value = "InActive";
            cmd.Parameters.Add("@Remarks", SqlDbType.VarChar, 200).Value = txtDeleteReason.Text.Trim();
            cmd.Parameters.Add("@OID", SqlDbType.BigInt).Value = lblProductID.Text;
            cmd.Parameters.Add("@EUSER", SqlDbType.VarChar, 50).Value = userID;
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
            ModalPopupExtender1.Hide();
            cmdSearch_Click(sender,e);
        }
    }

    protected void StockReturn_Details(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
    {
        if (rbtInActive.Checked == true)
        {
            lblMessage.Text = "Can not Make Operation on In Active Product";
        }
        else if (rbtReturn.Checked == true)
        {
            lblMessage.Text = "Can not Make Operation on Return Product";
        }
        else if (rbtActive.Checked == true)
        {
            lblMessage.Text = "";
            GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            if (e.CommandName == "InvoiceNo")
            {
                Label lblOID = (Label)row.Cells[0].FindControl("lblOID");
                Label lblBranchOID = (Label)row.Cells[0].FindControl("lblBranchOID");
                Label lblCategoryOID = (Label)row.Cells[0].FindControl("lblCategoryOID");
                Label lblSubcategoryOID = (Label)row.Cells[0].FindControl("lblSubcategoryOID");
                Label lblDescriptionOID = (Label)row.Cells[0].FindControl("lblDescriptionOID");
                Label lblBarcode = (Label)row.Cells[0].FindControl("lblBarcode");
                Label lblQuantity = (Label)row.Cells[0].FindControl("lblQuantity");

                if (lblCategoryOID.Text == "111")
                {
                    sql = "update StoreMasterStock set Quantity = Quantity + @Quantity where PROD_DES = @PROD_DES and Barcode = @Barcode and Branch=@Branch";
                    cmd = new SqlCommand(sql, dbConnect);
                    cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = lblQuantity.Text;
                    cmd.Parameters.Add("@PROD_DES", SqlDbType.BigInt).Value = lblDescriptionOID.Text;
                    cmd.Parameters.Add("@Barcode", SqlDbType.VarChar, 100).Value = lblBarcode.Text;
                    cmd.Parameters.Add("@Branch", SqlDbType.VarChar, 50).Value = lblBranchOID.Text;
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

                    sql = "update DOA set Status=@Status,Remarks=@Remarks,EUSER=@EUSER,EDAT=@EDAT where OID = @OID";
                    cmd = new SqlCommand(sql, dbConnect);
                    cmd.Parameters.Add("@Status", SqlDbType.VarChar, 50).Value = "Return";
                    cmd.Parameters.Add("@Remarks", SqlDbType.VarChar, 200).Value = String.Empty;
                    cmd.Parameters.Add("@EUSER", SqlDbType.VarChar, 50).Value = userID;
                    cmd.Parameters.Add("@EDAT", SqlDbType.Date).Value = DateTime.Now.Date;
                    cmd.Parameters.Add("@OID", SqlDbType.BigInt).Value = lblOID.Text;
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
                    cmd.Parameters.Add("@BranchOID", SqlDbType.VarChar, 100).Value = lblBranchOID.Text;
                    cmd.Parameters.Add("@DescriptionOID", SqlDbType.BigInt).Value = lblDescriptionOID.Text;
                    cmd.Parameters.Add("@Barcode", SqlDbType.VarChar, 100).Value = lblBarcode.Text;
                    cmd.Parameters.Add("@InwardQty", SqlDbType.Int).Value = lblQuantity.Text;
                    cmd.Parameters.Add("@OutwardQty", SqlDbType.Int).Value = 0;
                    cmd.Parameters.Add("@Particulars", SqlDbType.VarChar, 100).Value = "DOA";
                    cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 100).Value = userID;
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
                else
                {
                    sql = "update StoreMasterStock set ActiveStatus=@ActiveStatus,InActiveReason=@InActiveReason,EUSER=@EUSER,EDAT=@EDAT where Barcode=@Barcode";
                    cmd = new SqlCommand(sql, dbConnect);
                    cmd.Parameters.Add("@ActiveStatus", SqlDbType.Int).Value = 1;
                    cmd.Parameters.Add("@InActiveReason", SqlDbType.VarChar, 300).Value = String.Empty;
                    cmd.Parameters.Add("@EUSER", SqlDbType.VarChar, 100).Value = userID;
                    cmd.Parameters.Add("@EDAT", SqlDbType.Date).Value = DateTime.Now.Date;
                    cmd.Parameters.Add("@Barcode", SqlDbType.VarChar, 100).Value = lblBarcode.Text;
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

                    sql = "update DOA set Status=@Status,Remarks=@Remarks,EUSER=@EUSER,EDAT=@EDAT where OID = @OID";
                    cmd = new SqlCommand(sql, dbConnect);
                    cmd.Parameters.Add("@Status", SqlDbType.VarChar, 50).Value = "Return";
                    cmd.Parameters.Add("@Remarks", SqlDbType.VarChar, 200).Value = String.Empty;
                    cmd.Parameters.Add("@EUSER", SqlDbType.VarChar, 50).Value = userID;
                    cmd.Parameters.Add("@EDAT", SqlDbType.Date).Value = DateTime.Now.Date;
                    cmd.Parameters.Add("@OID", SqlDbType.BigInt).Value = lblOID.Text;
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
                    cmd.Parameters.Add("@BranchOID", SqlDbType.VarChar, 100).Value = lblBranchOID.Text;
                    cmd.Parameters.Add("@DescriptionOID", SqlDbType.BigInt).Value = lblDescriptionOID.Text;
                    cmd.Parameters.Add("@Barcode", SqlDbType.VarChar, 100).Value = lblBarcode.Text;
                    cmd.Parameters.Add("@InwardQty", SqlDbType.Int).Value = lblQuantity.Text;
                    cmd.Parameters.Add("@OutwardQty", SqlDbType.Int).Value = 0;
                    cmd.Parameters.Add("@Particulars", SqlDbType.VarChar, 100).Value = "DOA";
                    cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 100).Value = userID;
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
                cmdSearch_Click(sender, e);
            }
            
        }
    }





}