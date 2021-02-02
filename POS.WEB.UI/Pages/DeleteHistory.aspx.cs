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

public partial class Pages_DeleteHistory : System.Web.UI.Page
{
    private string userID = string.Empty;
    private string userPassword = string.Empty;
    T_PRODBLL BILL = new T_PRODBLL();

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
        T_PROD entity = new T_PROD();
        entity.Branch = ddlSearchBranch.SelectedItem.Value.ToString();
        entity.PROD_WGPG = ddlSearchProductCategory.SelectedItem.Value.ToString();
        entity.PROD_SUBCATEGORY = ddlSearchSubCategory.SelectedItem.Value.ToString();
        entity.PROD_DES = ddlSearchDescription.SelectedItem.Value.ToString();
        entity.FromDate = txtFromDate.Text;
        entity.ToDate = txtToDate.Text;
        entity.Barcode = txtBarcode.Text;
        DataTable dt = BILL.DeleteHistory(entity);
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }

    protected void StockReturn_Details(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
    {      
        GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
        if (e.CommandName == "InvoiceNo")
        {               
            Label lblBarcode = (Label)row.Cells[0].FindControl("lblBarcode");
            Label lblBranchOID = (Label)row.Cells[0].FindControl("lblBranchOID");
            Label lblDescriptionOID = (Label)row.Cells[0].FindControl("lblDescriptionOID");

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

            sql = "insert into StockPosting(BranchOID,DescriptionOID,Barcode,InwardQty,OutwardQty,Particulars,IUSER,IDAT) values(@BranchOID,@DescriptionOID,@Barcode,@InwardQty,@OutwardQty,@Particulars,@IUSER,@IDAT)";
            cmd = new SqlCommand(sql, dbConnect);
            cmd.Parameters.Add("@BranchOID", SqlDbType.VarChar, 100).Value = lblBranchOID.Text;
            cmd.Parameters.Add("@DescriptionOID", SqlDbType.BigInt).Value = lblDescriptionOID.Text;
            cmd.Parameters.Add("@Barcode", SqlDbType.VarChar, 100).Value = lblBarcode.Text;
            cmd.Parameters.Add("@InwardQty", SqlDbType.Int).Value = 1;
            cmd.Parameters.Add("@OutwardQty", SqlDbType.Int).Value = 0;
            cmd.Parameters.Add("@Particulars", SqlDbType.VarChar, 100).Value = "Activated";
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

                
        cmdSearch_Click(sender, e);
        }        
    }



}