using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TalukderPOS.BLL;
using TalukderPOS.BusinessObjects;

public partial class Pages_Description : System.Web.UI.Page
{
    private string userID = string.Empty;
    private string userPassword = string.Empty;
    Color_BLL BILL = new Color_BLL();
    private string Shop_id = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            userID = Session["UserID"].ToString();
            userPassword = Session["Password"].ToString();
            Shop_id = Session["StoreID"].ToString();
        }
        catch
        {
            Response.Redirect("~/frmLogin.aspx");
        }
        bool isAuthenticate = CommonBinder.CheckPageAuthentication(System.Web.HttpContext.Current.Request.Url.AbsolutePath, userID);
        //isAuthenticate = true;
        if (!isAuthenticate)
        {
            Response.Redirect("~/UnAuthorizedUser.aspx");
        }
        if (!Page.IsPostBack)
        {
        }
    }


    //V0 171002
    //protected void btnSave_Click(object sender, EventArgs e)
    //{
    //    Color_BO entity = new Color_BO();
    //    entity.OID = lblOID.Value.ToString();
    //    entity.CategoryID = ddlProductCategoryId.SelectedItem.Value.ToString();
    //    entity.SubCategoryID = ddlSubCategory.SelectedItem.Value.ToString();
    //    entity.Description = txtDescription.Text;
    //    entity.SESPrice = txtSESPrice.Text;
    //    entity.MRP = txtMRP.Text;
    //    entity.Active = "1";
    //    entity.IUSER = userID;
    //    entity.EUSER = userID;
    //    string[] valid = BILL.Validation(entity);
    //    if (valid[0].ToString() == "True")
    //    {
    //        lblMessage.Text = valid[1].ToString();
    //        BILL.Add(entity);
    //        lblMessage.Text = "SAVED SUCCESSFULLY";
    //        Clear();
    //        cmdSearch_Click(sender, e);
    //    }
    //    else
    //    {
    //        lblMessage.Text = valid[1].ToString();
    //    }
    //}
    protected void btnSave_Click(object sender, EventArgs e)
    {
        //check the name exist!

        string cmdstr = string.Empty;

        string abc = lblOID.Value;

        if (!string.IsNullOrEmpty(abc))
        {
            cmdstr = string.Format(@"
select *

from StoreMasterStock,T_WGPG,SubCategory,Description
Where
StoreMasterStock.PROD_DES=Description.OID AND 
StoreMasterStock.Quantity >0 and
(StoreMasterStock.Quantity - StoreMasterStock.SaleQuantity) >0 and
StoreMasterStock.ActiveStatus=1 and
StoreMasterStock.PROD_WGPG=T_WGPG.OID and
StoreMasterStock.PROD_SUBCATEGORY=SubCategory.OID
--AND UPPER(LTRIM(RTRIM(Description.Description))) = LTRIM(RTRIM('{0}'))
and StoreMasterStock.Branch='{1}' AND Description.OID = '{2}'

", txtDescription.Text.ToUpper(), Session["StoreID"], abc);
        }

        else
        {




             cmdstr = string.Format(@"
select Description.*
from StoreMasterStock,T_WGPG,SubCategory,Description
Where
StoreMasterStock.PROD_DES=Description.OID AND 
StoreMasterStock.Quantity >0 and
(StoreMasterStock.Quantity - StoreMasterStock.SaleQuantity) >0 and
StoreMasterStock.ActiveStatus=1 and
StoreMasterStock.PROD_WGPG=T_WGPG.OID and
StoreMasterStock.PROD_SUBCATEGORY=SubCategory.OID
AND StoreMasterStock.PROD_WGPG={0}
and StoreMasterStock.PROD_SUBCATEGORY={1} 
AND UPPER(LTRIM(RTRIM(Description.Description))) = LTRIM(RTRIM('{2}'))
and StoreMasterStock.Branch='{3}'
", ddlProductCategoryId.SelectedValue, ddlSubCategory.SelectedValue, txtDescription.Text.ToUpper(), Session["StoreID"]
     );
        }
        DataTable dt = GetDataTableByQuery(cmdstr);

        if (dt.Rows.Count == 0)
        {
            //
            Color_BO entity = new Color_BO();
            entity.OID = lblOID.Value.ToString();
            entity.CategoryID = ddlProductCategoryId.SelectedItem.Value.ToString();
            entity.SubCategoryID = ddlSubCategory.SelectedItem.Value.ToString();
            entity.Description = txtDescription.Text;
            entity.SESPrice = txtSESPrice.Text;
            entity.MRP = txtMRP.Text;
            entity.Active = "1";
            entity.IUSER = userID;
            entity.EUSER = userID;
            string[] valid = BILL.Validation(entity);
            if (valid[0].ToString() == "True")
            {
                lblMessage.Text = valid[1].ToString();
                BILL.Add(entity);
                lblMessage.Text = "SAVED SUCCESSFULLY";
                Clear();
                cmdSearch_Click(sender, e);
            }
            else
            {
                lblMessage.Text = valid[1].ToString();
            }
        }
        else
        {
            Color_BO entity = new Color_BO();
            entity.OID = lblOID.Value.ToString();
            //entity.CategoryID = dt.Rows[0]["PROD_WGPG"].ToString ();
            //entity.SubCategoryID = dt.Rows[0]["PROD_SUBCATEGORY"].ToString();
            entity.Description = txtDescription.Text;
            entity.SESPrice = txtSESPrice.Text;
            entity.MRP = txtMRP.Text;
            
           
            entity.Active = "1";
            entity.IUSER = userID;
            entity.EUSER = userID;

            string[] valid = BILL.Validation(entity);
            if (valid[0].ToString() == "True")
            {
                lblMessage.Text = valid[1].ToString();
                BILL.Add(entity);
                lblMessage.Text = "UPDATED SUCCESSFULLY";
                Clear();
                cmdSearch_Click(sender, e);

            }
            else
            {
                lblMessage.Text = valid[1].ToString();

            }
        }
    }

    public DataTable GetDataTableByQuery(string sqlQuery)
    {
        DataTable dt = new DataTable();
        string constr = ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand(sqlQuery))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;

                    sda.Fill(dt);
                }
            }
        }
        return dt;
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Clear();
        lblMessage.Text = string.Empty;
        ContainerColor.ActiveTabIndex = 0;
    }


    protected void gvColor_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
    {
        Color_BO entity = new Color_BO();
        entity.OID = gvColor.DataKeys[e.RowIndex].Value.ToString();
        entity.EUSER = userID;
        BILL.Delete(entity);
        cmdSearch_Click(sender, e);
    }

    protected void gvColor_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
    {
        e.Cancel = true;
        Clear();
        lblMessage.Text = string.Empty;
        Color_BO entity = new Color_BO();
        String OID = gvColor.DataKeys[e.NewEditIndex].Value.ToString();
        entity = BILL.GetById(OID);
        lblOID.Value = entity.OID;
        CAS_ddlProductCategoryId.SelectedValue = entity.CategoryID;
        CAS_ddlSubCategory.SelectedValue = entity.SubCategoryID;
        txtDescription.Text = entity.Description;
        txtSESPrice.Text = entity.SESPrice;
        txtMRP.Text = entity.MRP;



        ContainerColor.ActiveTabIndex = 1;

        //disable 
        btnSubmit.Enabled = false;
        Button2.Enabled = false;
        CAS_ddlProductCategoryId.Enabled = false;
        CAS_ddlSubCategory.Enabled = false;
        //txtDescription.Enabled = false;
    }



    protected void gvColor_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
    {
        gvColor.PageIndex = e.NewPageIndex;
        BindList();

        for (int i = 0; i < gvColor.Rows.Count; i++)
        {
            LinkButton lnkbtnInactive = (LinkButton)gvColor.Rows[i].Cells[8].FindControl("lnkDelete");
            LinkButton lnkbtnActive = (LinkButton)gvColor.Rows[i].Cells[8].FindControl("lnkbtnActive");
            if (ddlStatus.SelectedItem.Value.ToString() == "1")
            {
                lnkbtnInactive.Visible = true; lnkbtnActive.Visible = false;
            }
            else if (ddlStatus.SelectedItem.Value.ToString() == "0")
            {
                lnkbtnInactive.Visible = false; lnkbtnActive.Visible = true;
            }
            else
            {
                lnkbtnInactive.Visible = false; lnkbtnActive.Visible = false;
            }
        }
    }



    private void BindList()
    {
        gvColor.DataSource = Session["GridData"];
        gvColor.DataBind();
    }


    private void Clear()
    {
        lblOID.Value = string.Empty;
        ddlProductCategoryId.SelectedIndex = -1;
        ddlSubCategory.SelectedIndex = -1;

        txtDescription.Text = string.Empty;
        txtSESPrice.Text = string.Empty;
        txtMRP.Text = string.Empty;

        //disable 
        ddlProductCategoryId.Enabled = true;
        ddlSubCategory.Enabled = true;
        txtDescription.Enabled = true;

        //ContainerColor.ActiveTabIndex = 0;
    }


    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        Color_BO entity = new Color_BO();
        entity.CategoryID = ddlSearchProductCategory.SelectedItem.Value.ToString();
        entity.SubCategoryID = ddlSearchProductSubCategory.SelectedItem.Value.ToString();
        entity.Shop_id = Shop_id.ToString();

        // sadiq 170917
        entity.Active = ddlStatus.SelectedItem.Value.ToString();

        DataTable dt = BILL.BindList(entity);
        Session["GridData"] = dt;
        BindList();

        // sadiq 170918
        //to show hide Deactive option
        //TextBox txtAQty = (TextBox)gv.Rows[i].Cells[8].FindControl("txtApprvdQnt");

        for (int i = 0; i < gvColor.Rows.Count; i++)
        {
            LinkButton lnkbtnInactive = (LinkButton)gvColor.Rows[i].Cells[8].FindControl("lnkDelete");
            LinkButton lnkbtnActive = (LinkButton)gvColor.Rows[i].Cells[8].FindControl("lnkbtnActive");
            if (ddlStatus.SelectedItem.Value.ToString() == "1")
            {
                lnkbtnInactive.Visible = true; lnkbtnActive.Visible = false;
            }
            else if (ddlStatus.SelectedItem.Value.ToString() == "0")
            {
                lnkbtnInactive.Visible = false; lnkbtnActive.Visible = true;
            }
            else
            {
                lnkbtnInactive.Visible = false; lnkbtnActive.Visible = false;
            }
        }

        //if (ddlStatus.SelectedItem.Value.ToString() == "1")
        //{
        //    gvColor.Columns[8].Visible = true;
        //    for (int i = 0; i < gvColor.Rows.Count; i++)
        //    {
        //        gvColor.Rows[i].Cells[8].Visible = true;
        //    }
        //}
        //else
        //{
        //    gvColor.Columns[8].Visible = false;
        //    for (int i = 0; i < gvColor.Rows.Count; i++)
        //    {
        //        gvColor.Rows[i].Cells[8].Visible = false;
        //    }
        //}
    }






    protected void cmdExport_Click(object sender, EventArgs e)
    {
        Session["dtsales"] = Session["GridData"];
        Session["ReportPath"] = "~/Reports/rptDescription.rpt";

        string webUrl = "../Reports/ReportsViewer.aspx";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenWindow", "window.open('" + webUrl + "','_newtab');", true);
    }






    protected void addnew_Click(object sender, EventArgs e)
    {
        //ModalPopupExtender1.Show();
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", true);
    }
    protected void addnewsub_Click(object sender, EventArgs e)
    {
        Model_BO entity = new Model_BO();
        string catidnew = ddlProductCategoryId.SelectedItem.Value.ToString();
        CascadingDropDownsub244.SelectedValue = catidnew;
        //ModalPopupExtender1.Show();
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal2", "$('#myModal2').modal();", true);

    }
    protected void btncatSave_Click(object sender, EventArgs e)
    {
        //ModalPopupExtender1.Show();
        T_WGPGBLL BILL2 = new T_WGPGBLL();
        T_WGPG entity = new T_WGPG();

        entity.WGPG_NAME = txtCategoryName.Text.ToString();
        if (entity.WGPG_NAME == string.Empty)
        {
           // ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "bappskey1", "alert('Category Name Can not be empty');", true);
            //RegisterStartupScript(this, GetType(), "Close Modal Popup", "Closepopup();", true);
            Response.Redirect("Description.aspx?menuhead=101");
            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", true);
        }
        else
        {
            entity.WGPG_ACTV = "1";
            entity.Shop_id = Shop_id.ToString();
            entity.IUSER = userID.ToString();
            entity.EUSER = userID.ToString();
            BILL2.Add(entity);
            Response.Redirect("Description.aspx?menuhead=101");
        }

    }

    protected void btnSavesub_Click(object sender, EventArgs e)
    {
        //ModalPopupExtender1.Show();
        Model_BO entity = new Model_BO();
        Model_BLL BILL23 = new Model_BLL();
        entity.CategoryID = CascadingDropDownsub2.SelectedItem.Value.ToString();
        entity.SubCategoryName = txtSubCategory22.Text;
        if (entity.CategoryID == string.Empty || entity.SubCategoryName == string.Empty)
        {
            Response.Redirect("Description.aspx?menuhead=101");
        }
        else
        {
            entity.Active = "1";
            entity.ShowOnDropdown = "Y";
            entity.RunningModel = "Y";
            entity.IUSER = userID;
            entity.EUSER = userID;
            BILL23.Add(entity);
            Response.Redirect("Description.aspx?menuhead=101");
        }
        
    }
    //protected void ddlSearchProductSubCategory_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    if (!string.IsNullOrEmpty(ddlSearchProductSubCategory.SelectedValue))
    //    {
    //        ddlStatus.Enabled = true;
    //    }
    //    else { ddlStatus.Enabled = false; }
    //}


    protected void Button4_Click(object sender, EventArgs e)
    {
        LoadGrid();
    }
    public void LoadGrid()
    {
        //param
        string strpram = string.Empty;
        if (!string.IsNullOrEmpty(ddlSearchProductCategory.SelectedItem.Value))
        {
            strpram += string.Format(@" and T_WGPG.OID = {0}", ddlSearchProductCategory.SelectedItem.Value);
        }
        if (!string.IsNullOrEmpty(Shop_id.ToString()))
        {
            strpram += string.Format(@" and T_WGPG.Shop_id =  {0}", Shop_id.ToString());
        }
        if (!string.IsNullOrEmpty(ddlSearchProductSubCategory.SelectedItem.Value))
        {
            strpram += string.Format(@" and SubCategory.OID =  {0}", ddlSearchProductSubCategory.SelectedItem.Value.ToString());
        }
        if (ddlStatus.SelectedValue != "--Please Select--")
        {
            strpram += string.Format(@" and Description.Active =  '{0}'", ddlStatus.SelectedValue.ToString());
        }
        strpram += string.Format(@" and T_WGPG.WGPG_ACTV='1' and SubCategory.Active='1'");


        //
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
        string cmdstr = string.Format(@"
select  Description.OID,T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,Description.Description,Description.SESPrice,Description.MRP 
from Description inner join SubCategory on Description.SubCategoryID=SubCategory.OID inner join T_WGPG on SubCategory.CategoryID=T_WGPG.OID 
where 1=1 {0}
order by T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,Description.Description

", strpram);
        using (con)
        {
            using (SqlCommand cmd = new SqlCommand(cmdstr, con))
            {
                SqlDataAdapter sda = new SqlDataAdapter(cmd);             //command.ExecuteNonQuery();
                DataTable dt = new DataTable();
                sda.Fill(dt);

                //
                gvColor.DataSource = null;
                gvColor.DataSource = dt;
                gvColor.DataBind();

                //
                if (ddlStatus.SelectedItem.Value.ToString() == "1")
                {
                    gvColor.Columns[8].Visible = true;
                    for (int i = 0; i < gvColor.Rows.Count; i++)
                    {
                        gvColor.Rows[i].Cells[8].Visible = true;
                    }
                }
                else
                {
                    gvColor.Columns[8].Visible = false;
                    for (int i = 0; i < gvColor.Rows.Count; i++)
                    {
                        gvColor.Rows[i].Cells[8].Visible = false;
                    }
                }
            }
        }//
    }//

    protected void lnkbtnActive_Click(object sender, EventArgs e)
    {
        //lnkbtnActive



        //
        Color_BO entity = new Color_BO();
        int rowIndex = ((sender as LinkButton).NamingContainer as GridViewRow).RowIndex;
        entity.OID = gvColor.DataKeys[rowIndex].Value.ToString();
        entity.EUSER = userID;
        BILL.ActiveDescription(entity); // BILL.Delete(entity);
        cmdSearch_Click(sender, e);
    }
    
}