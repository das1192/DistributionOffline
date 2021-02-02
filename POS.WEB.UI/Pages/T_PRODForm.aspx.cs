using System;
using System.Web;
using System.Data;
using System.Linq;
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


namespace TalukderPOS.Web.UI
{
    public partial class T_PRODForm : System.Web.UI.Page
    {
        private string userID = string.Empty;
        private string userPassword = string.Empty;
        private string Shop_id = string.Empty;
        T_PRODBLL BILL = new T_PRODBLL();
        CommonDAL DAL = new CommonDAL();

        SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
        SqlDataAdapter da = new SqlDataAdapter();
        SqlCommand cmd;
        String sql;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                userID = Session["UserID"].ToString();
                userPassword = Session["Password"].ToString();
                Shop_id = Session["StoreID"].ToString();
                if (userID == "")
                {
                    Response.Redirect("~/frmLogin.aspx");
                }
                cmdSearch_Click(sender, e);

                //visible false 2 TabContainer   when  shopInfo oid=43
                if (Shop_id == "43")
                {
                    TabPanel1.Visible = false;
                    tPnlDisplay2.Visible = false;
                }
                else
                {
                    TabPanel1.Visible = true;
                    tPnlDisplay2.Visible = true;
                }
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
                LoadDDLCategory();
                LoadDDLCategoryPR();
                clearDropdownIndex();
                Session["GridData"] = null;
                dtpPurchaseDate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                CE_dtpPurchaseDate.EndDate = DateTime.Now;
            }
        }

        //==================================================
        //new by das loading purches return category
        private void LoadDDLCategoryPR()
        {
            clearDropdownIndex();
            
            string sql = string.Format(@"
select id=OID,text=WGPG_NAME from T_WGPG where WGPG_ACTV=1 AND Shop_id='{0}' order by WGPG_NAME
", Session["StoreID"].ToString());
            DataTable dt = DAL.LoadDataByQuery(sql);

            if (dt.Rows.Count > 0)
            {
                ddlProductCategoryIdRET.DataSource = dt;
                ddlProductCategoryIdRET.DataValueField = "id";
                ddlProductCategoryIdRET.DataTextField = "text";
                ddlProductCategoryIdRET.DataBind();

                ddlProductCategoryIdRET.Items.Insert(0, new ListItem("-- Select One--", String.Empty));

            }
        }

        private void LoadDDLSubCategoryPR()
        {
            string sql = string.Format(@"
select id=OID,text=SubCategoryName from SubCategory where CategoryID={0} and Active=1 and ShowOnDropdown='Y' order by SubCategoryName
", ddlProductCategoryIdRET.SelectedValue.ToString());
            DataTable dt = DAL.LoadDataByQuery(sql);

            if (dt.Rows.Count > 0)
            {
                ddlSubCategoryIdProRET.DataSource = dt;
                ddlSubCategoryIdProRET.DataValueField = "id";
                ddlSubCategoryIdProRET.DataTextField = "text";
                ddlSubCategoryIdProRET.DataBind();

                ddlSubCategoryIdProRET.Items.Insert(0, new ListItem("-- Select One--", String.Empty));

            }
        }

        private void LoadDDLDescriptionPR()
        {
            string sql = string.Format(@"
select id=OID,text=Description from Description where SubCategoryID={0} and Active=1 order by Description
", ddlSubCategoryIdProRET.SelectedValue.ToString());
            DataTable dt = DAL.LoadDataByQuery(sql);

            if (dt.Rows.Count > 0)
            {
                ddlDescriptionPrpRET.DataSource = dt;
                ddlDescriptionPrpRET.DataValueField = "id";
                ddlDescriptionPrpRET.DataTextField = "text";
                ddlDescriptionPrpRET.DataBind();

                ddlDescriptionPrpRET.Items.Insert(0, new ListItem("-- Select One--", String.Empty));

            }
        }

        private void LoadUnitCostPRF() {




         
            
                string sql = string.Format(@"
select s.ACC_STOCKID, s.CostPrice,s.Quantity
,('Cost Price:' + CONVERT(nvarchar(15), s.CostPrice)+' : Stock Qty:' +CONVERT(nvarchar(15),s.Quantity) ) CostPriceStock
from Acc_Stock s
inner join Vendor v on v.OID=s.VendorID
where s.Quantity>0 and s.Flag='Quantity' and s.Branch='{0}' and s.PROD_WGPG='{1}' and s.PROD_SUBCATEGORY='{2}' and s.PROD_DES='{3}' and s.VendorID='{4}'
order by v.Vendor_Name
", Shop_id, ddlProductCategoryIdRET.SelectedValue, ddlSubCategoryIdProRET.SelectedValue, ddlDescriptionPrpRET.SelectedValue, ddlVendorListPRF.SelectedValue);
                DataTable dt = DAL.LoadDataByQuery(sql);

                ddlUnitCostPRF.DataSource = null;
                if (dt.Rows.Count > 0)
                {
                    ddlUnitCostPRF.DataSource = dt;
                }
                ddlUnitCostPRF.DataValueField = "ACC_STOCKID";
                ddlUnitCostPRF.DataTextField = "CostPriceStock";
                ddlUnitCostPRF.DataBind();
            
            ddlUnitCostPRF.Items.Insert(0, new ListItem("...Select One...", String.Empty));
        
        
        
        
        
        
        
        }
      

        //====================== end of das 
        //new by das on index change purchase change category
        protected void ddlProductCategoryIdRET_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlProductCategoryIdRET.SelectedValue.ToString()))
            {
                LoadDDLSubCategoryPR();
            }
            else
            {
                ddlSubCategory.Items.Clear();
            }
        }
        protected void ddlSubCategoryIdProRET_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlSubCategoryIdProRET.SelectedValue.ToString()))
            {
                LoadDDLDescriptionPR();
            }
            else
            {
                ddlDescriptionPrpRET.Items.Clear();
            }
        }

        protected void ddlVendorListPRF_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlUnitCostPRF.Items.Clear();

            if (!string.IsNullOrEmpty(ddlProductCategoryIdRET.SelectedValue) && !string.IsNullOrEmpty(ddlSubCategoryIdProRET.SelectedValue)
         && !string.IsNullOrEmpty(ddlDescriptionPrpRET.SelectedValue) && !string.IsNullOrEmpty(ddlVendorListPRF.SelectedValue))
            {

                LoadUnitCostPRF();


            }
            else
            {
                ddlUnitCostPRF.Items.Clear();

            }


        }


        //=============================================end of das 






        private void LoadDDLCategory()
        {
            string sql = string.Format(@"
select id=OID,text=WGPG_NAME from T_WGPG where WGPG_ACTV=1 AND Shop_id='{0}' order by WGPG_NAME
", Session["StoreID"].ToString());
            DataTable dt = DAL.LoadDataByQuery(sql);

            if (dt.Rows.Count > 0)
            {
                ddlProductCategoryId.DataSource = dt;
                ddlProductCategoryId.DataValueField = "id";
                ddlProductCategoryId.DataTextField = "text";
                ddlProductCategoryId.DataBind();

                ddlProductCategoryId.Items.Insert(0, new ListItem("-- Select One--", String.Empty));

            }
        }
        private void LoadDDLSubCategory()
        {
            string sql = string.Format(@"
select id=OID,text=SubCategoryName from SubCategory where CategoryID={0} and Active=1 and ShowOnDropdown='Y' order by SubCategoryName
", ddlProductCategoryId.SelectedValue.ToString());
            DataTable dt = DAL.LoadDataByQuery(sql);

            if (dt.Rows.Count > 0)
            {
                ddlSubCategory.DataSource = dt;
                ddlSubCategory.DataValueField = "id";
                ddlSubCategory.DataTextField = "text";
                ddlSubCategory.DataBind();

                ddlSubCategory.Items.Insert(0, new ListItem("-- Select One--", String.Empty));

            }
        }
        private void LoadDDLDescription()
        {
            string sql = string.Format(@"
select id=OID,text=Description from Description where SubCategoryID={0} and Active=1 order by Description
", ddlSubCategory.SelectedValue.ToString());
            DataTable dt = DAL.LoadDataByQuery(sql);

            if (dt.Rows.Count > 0)
            {
                ddlDescription.DataSource = dt;
                ddlDescription.DataValueField = "id";
                ddlDescription.DataTextField = "text";
                ddlDescription.DataBind();

                ddlDescription.Items.Insert(0, new ListItem("-- Select One--", String.Empty));

            }
        }
  


        protected void ddlProductCategoryId_SelectedIndexChanged1(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlProductCategoryId.SelectedValue.ToString()))
            {
                ddlSubCategory.Items.Clear();
                LoadDDLSubCategory();
                ddlDescription.Items.Clear();
            }
            else
            {
                ddlSubCategory.Items.Clear();
                ddlDescription.Items.Clear();
            }
        }
        protected void ddlSubCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlSubCategory.SelectedValue.ToString()))
            {
                LoadDDLDescription();
            }
            else
            {
                ddlDescription.Items.Clear();
            }
        }
        protected void ddlDescription_SelectedIndexChanged(object sender, EventArgs e)
        {

            T_PROD entity = new T_PROD();
            entity.PROD_WGPG = ddlProductCategoryId.SelectedItem.Value.ToString();
            entity.PROD_DES = ddlDescription.SelectedItem.Value.ToString();
            T_PROD obj = BILL.GetBarcodeCostAndSalePrice(entity);
            txtBarcode.InnerText = obj.Barcode;
            txtPurchasePrice.Text = obj.CostPrice;
            txtSalePrice.Text = obj.SalePrice;

        }

        //***************Product List In Branch******************************************//
        protected void cmdSearch_Click(object sender, EventArgs e)
        {
            Label26.Text = string.Empty;
            T_PROD entity = new T_PROD();
            entity.Branch = Shop_id.ToString();
            entity.PROD_WGPG = ddlSearchProductCategory.SelectedItem.Value.ToString();
            entity.PROD_SUBCATEGORY = ddlSearchSubCategory.SelectedItem.Value.ToString();
            entity.PROD_DES = ddlSearchDescription.SelectedItem.Value.ToString();
            entity.Vendor_ID = ddlVendorListnew.SelectedItem.Value.ToString();
            entity.FromDate = txtDateFrom.Text;
            entity.ToDate = txtDateTo.Text;

            gvT_PROD.Visible = true;

            GridView2.Visible = false;
            entity.SearchType = "Details";
            DataTable dt = BILL.VendorStockSearch(entity);
            Session["GridData"] = dt;
            BindList();
            var sum = dt.AsEnumerable().Sum(dr => dr.Field<Int32>("Quantity"));
            lblTolQuantity.Text = "<span class='label label-primary'>Total Quantity : " + sum.ToString() + "</span>";
            var sum2 = dt.AsEnumerable().Sum(dr => dr.Field<Int32>("grandtotal"));
            lblTotgrandtotal.Text = "<span class='label label-danger'>Total Purchase Value : " + sum2.ToString() + "</span>";

        }

        protected void cmdSearch3_Click(object sender, EventArgs e)
        {
            Label26.Text = string.Empty;
            T_PROD entity = new T_PROD();
            entity.Branch = Shop_id.ToString();
            entity.PROD_WGPG = ddlSearchProductCategory3.SelectedItem.Value.ToString();
            entity.PROD_SUBCATEGORY = ddlSearchSubCategory3.SelectedItem.Value.ToString();
            entity.PROD_DES = ddlSearchDescription3.SelectedItem.Value.ToString();
            entity.FromDate = txtDateFrom2.Text;
            entity.ToDate = txtDateTo2.Text;
            entity.SearchOption = ddlSearchOption.SelectedItem.Value.ToString();
            gv_Purchase_History.Visible = true;
            DataTable dt = BILL.ProductHistorySearch(entity);
            Session["GridData2"] = dt;
            BindList3();


        }
        protected void txtSearchBarcode_TextChanged(object sender, EventArgs e)
        {
            cmdSearch_Click(sender, e);
        }
        protected void gvT_PROD_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            gvT_PROD.PageIndex = e.NewPageIndex;
            BindList();
        }

        protected void gv_Purchase_History_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            gv_Purchase_History.PageIndex = e.NewPageIndex;
            BindList3();
        }
        protected void cmdPreview_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)Session["GridData"];
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    Session["dtsales"] = dt;
                    Session["ReportPath"] = "~/Reports/rptExcelProductList.rpt";

                    string webUrl = "../Reports/ReportsViewer.aspx";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenWindow", "window.open('" + webUrl + "','_newtab');", true);
                }
            }
        }
        protected void gvT_PROD_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            if (((Label)gvT_PROD.Rows[e.NewEditIndex].FindControl("lblPROD_WGPG")).Text == "Accessories" & ((Label)gvT_PROD.Rows[e.NewEditIndex].FindControl("lblCCOM_NAME")).Text != "Head Office")
            {
                Label26.Text = "Accessories Can not be Modified !!";
                return;
            }
            else
            {
                e.Cancel = true;
                Clear();
                Int32 Id = Convert.ToInt32(gvT_PROD.DataKeys[e.NewEditIndex].Value);
                lblOID.Value = Id.ToString();
                T_PROD entity = BILL.GetProductInformation(Id.ToString());
                //CascadingDropDown4.SelectedValue = entity.PROD_WGPG.ToString();
                //CascadingDropDown5.SelectedValue = entity.PROD_SUBCATEGORY.ToString();
                //CascadingDropDown6.SelectedValue = entity.PROD_DES.ToString();
                CCD8.SelectedValue = entity.Vendor_ID.ToString();
                txtBarcode.InnerText = entity.Barcode;
                hidBarcode.Value = entity.Quantity.ToString();
                txtPurchasePrice.Text = entity.CostPrice.ToString();
                txtSalePrice.Text = entity.SalePrice.ToString();
                txtBarcode.InnerText = entity.Barcode.ToString();
                txtQuantity.Text = entity.Quantity.ToString();
                tContT_PROD.ActiveTabIndex = 1;
                cmdAdd.Visible = false;
                Session["AccessoriesEdit"] = "F";
                entity.AccessoriesEdit = "F";
                entity.OID = Id.ToString();
                entity.Action = "EditRequest";
                //BILL.MonitorStoreMasterStock(entity);

            }
        }
        protected void StockReturn_Details(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            if (e.CommandName == "Return")
            {

                Label lblOID = (Label)row.Cells[0].FindControl("lblOID");

                T_PROD entity = BILL.GetProductInformation(lblOID.Text);
                entity.AccessoriesEdit = "F";
                entity.OID = lblOID.Text;
                BILL.UpdateStoreMasterStock(entity);
                BILL.UpdatePurchaseReport(entity);


            }

        }
        protected void gvT_PROD_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            Int64 OID = Convert.ToInt32(gvT_PROD.DataKeys[e.RowIndex].Value);
            if (((Label)gvT_PROD.Rows[e.RowIndex].FindControl("lblPROD_WGPG")).Text.ToString() == "Accessories")
            {
                Label26.Text = "Accessories Can not be Deleted !!";
            }
            else
            {
                Label26.Text = string.Empty;
                txtDeleteReason.Text = string.Empty;
                lblProductID.Text = OID.ToString();
                lblBarcode.Text = ((Label)gvT_PROD.Rows[e.RowIndex].FindControl("lblBarcode")).Text;
                ModalPopupExtender1.Show();
                txtDeleteReason.Focus();
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
                T_PROD entity = new T_PROD();
                entity.OID = lblProductID.Text;
                entity.Barcode = lblBarcode.Text;
                entity.ActiveStatus = "0";
                entity.InActiveReason = txtDeleteReason.Text.Trim();
                entity.EUSER = userID;
                entity.EDAT = DateTime.Today.Date.ToString();
                BILL.SPP_DeleteProduct(entity);

                cmdSearch_Click(sender, e);
                ModalPopupExtender1.Hide();
            }
        }
        private void BindList()
        {
            gvT_PROD.DataSource = Session["GridData"];
            gvT_PROD.DataBind();
        }

        private void BindList3()
        {
            gv_Purchase_History.DataSource = Session["GridData2"];
            gv_Purchase_History.DataBind();
        }
        //***************End Product List In Branch******************************************//



        //****************************Product Entry******************************************//        
        protected void cmdAdd_Click(object sender, EventArgs e)
        {
            string[] valid = new string[2];
            T_PROD entity = new T_PROD();
            entity.PROD_WGPG = ddlProductCategoryId.SelectedItem.Value.ToString();
            entity.PROD_SUBCATEGORY = ddlSubCategory.SelectedItem.Value.ToString();
            entity.PROD_DES = ddlDescription.SelectedItem.Value.ToString();
            entity.Vendor_ID = ddlVendorList.SelectedItem.Value.ToString();

            entity.Entrymode = ddlentry.SelectedItem.Value.ToString();
            entity.Branch = Shop_id;
            entity.CostPrice = txtPurchasePrice.Text;
            entity.SalePrice = txtSalePrice.Text;
            entity.Quantity = txtQuantity.Text;

            if (ddlentry.SelectedValue == "Select Entry Mode") { ddlentry.SelectedIndex = -1; ddlentry.Focus(); return; }
            //check quantity
            if (ddlentry.SelectedValue == "2")
            {
                int qty = 0;
                if (int.TryParse(txtQuantity.Text, out qty)) { }


                if (qty == 0)
                {
                    txtQuantity.Text = string.Empty;
                    txtQuantity.Focus();
                    return;
                }

            }//



            //if (chkCash.Checked)
            //{
            //    entity.CashTrans = "1";
            //}
            //else
            //{
            //    entity.CashTrans = "2";
            //}
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
                dt.Columns.Add("VendorID");
                dt.Columns.Add("Vendor");
                dt.Columns.Add("CostPrice");
                dt.Columns.Add("SellPrice");
                dt.Columns.Add("Quantity");


                string s = txtBarcode.InnerText;
                string[] lines = s.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);

                foreach (string value in lines)
                {
                    if (string.IsNullOrEmpty(value) && ddlentry.SelectedItem.Text == "Quantity")
                    {


                        workRow = dt.NewRow();          //with qty
                        workRow["PROD_WGPG"] = ddlProductCategoryId.SelectedItem.Value;
                        workRow["Category"] = ddlProductCategoryId.SelectedItem.Text;

                        workRow["PROD_SUBCATEGORY"] = ddlSubCategory.SelectedItem.Value;
                        workRow["SubCategory"] = ddlSubCategory.SelectedItem.Text;

                        workRow["PROD_DES"] = ddlDescription.SelectedItem.Value;

                        workRow["Description"] = ddlDescription.SelectedItem.Text;
                        workRow["VendorID"] = ddlVendorList.SelectedItem.Value;
                        workRow["Vendor"] = ddlVendorList.SelectedItem.Text;

                        workRow["BarCode"] = "";



                        workRow["CostPrice"] = txtPurchasePrice.Text;
                        workRow["SellPrice"] = txtSalePrice.Text;

                        Int32 qtyAdd = 1;
                        string qtyAddd = string.IsNullOrEmpty(txtQuantity.Text) ? "1" : txtQuantity.Text;
                        if (Int32.TryParse(qtyAddd, out qtyAdd)) { }

                        workRow["Quantity"] = qtyAdd;
                        dt.Rows.Add(workRow);

                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(value) && ddlentry.SelectedItem.Text == "Barcode")
                        {
                            workRow = dt.NewRow();              //with barcode
                            workRow["PROD_WGPG"] = ddlProductCategoryId.SelectedItem.Value;
                            workRow["Category"] = ddlProductCategoryId.SelectedItem.Text;

                            workRow["PROD_SUBCATEGORY"] = ddlSubCategory.SelectedItem.Value;
                            workRow["SubCategory"] = ddlSubCategory.SelectedItem.Text;

                            workRow["PROD_DES"] = ddlDescription.SelectedItem.Value;
                            workRow["Description"] = ddlDescription.SelectedItem.Text;
                            workRow["VendorID"] = ddlVendorList.SelectedItem.Value;
                            workRow["Vendor"] = ddlVendorList.SelectedItem.Text;
                            workRow["BarCode"] = value;

                            workRow["CostPrice"] = txtPurchasePrice.Text;
                            workRow["SellPrice"] = txtSalePrice.Text;

                            workRow["Quantity"] = "1";


                            string found = BILL.FindBarcodeNew(value, Shop_id);
                            if (string.IsNullOrEmpty(found))
                            {
                                string isAdded = "no";

                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    string barcode = dt.Rows[i]["BarCode"].ToString().Trim();
                                    if (!string.IsNullOrEmpty(barcode))
                                    {
                                        if (value == barcode.Trim()) { isAdded = "yes"; break; }
                                    }

                                }

                                for (int i = 0; i < gvT_BarCode.Rows.Count; i++)
                                {
                                    string barcode = ((Label)gvT_BarCode.Rows[i].FindControl("lblBarCode")).Text;
                                    if (!string.IsNullOrEmpty(barcode))
                                    {
                                        if (value == barcode.Trim()) { isAdded = "yes"; break; }
                                    }

                                }
                                if (isAdded == "no")
                                {

                                    dt.Rows.Add(workRow);
                                }
                            }
                            else
                            {
                                string msg = "Given Barcode " + value + " Already Entered in the system";
                                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "bappskey1", "alert('" + msg + "');", true);
                            }
                        }
                    }
                }

                if (ddlProductCategoryId.SelectedItem.Value.ToString() == "111") { }
                else
                {
                    txtQuantity.Text = "1";
                }


                txtBarcode.InnerText = string.Empty;
                txtQuantity.Text = string.Empty;

                DataTable dt1 = tblGridRow();
                dt.Merge(dt1);
                gvT_BarCode.DataSource = dt;
                gvT_BarCode.DataBind();
                //int total = dt.Sum(x => Convert.ToInt32(x));
                var sum11 = dt.AsEnumerable().Sum(x => Convert.ToInt32(x["Quantity"]));
                //int sum11 = Convert.ToInt32(dt.Compute("Sum(Convert(Quantity, 'System.Int32')", ""));
                //var sum11 = dt.AsEnumerable().Sum(int.Parse(dr => dr.Field<Int32>("Quantity")));
                //int sum11 = Convert.ToInt32(dt.Compute("Sum(Quantity)", ""));
                lblPurTolQuantity.Text = "<span class='label label-primary'>Total Quantity : " + sum11.ToString() + "</span>";
                var sum22 = dt.AsEnumerable().Sum(x => (Convert.ToInt32(x["CostPrice"]) * Convert.ToInt32(x["Quantity"])));
                //var sum22 = dt.AsEnumerable().Sum(dr => dr.Field<Int32>("CostPrice"));
                lblPurTotgrandtotal.Text = "<span class='label label-danger'>Total Purchase Value : " + sum22.ToString() + "</span>";
            }
            else
            {
                lblMessage.Text = valid[1].ToString();
                lblMessage.ForeColor = Color.Red;
            }

            LoadTotalQtyTotalCost();
        }

        private void LoadTotalQtyTotalCost()
        {
            decimal totalQty = 0;
            decimal totalCost = 0;
            for (int i = 0; i < gvT_BarCode.Rows.Count; i++)
            {
                decimal Qty = 0;
                decimal Cost = 0;
                if (decimal.TryParse(((Label)gvT_BarCode.Rows[i].FindControl("lblQuantity")).Text, out Qty)) { }
                if (decimal.TryParse(((Label)gvT_BarCode.Rows[i].FindControl("lblCostPrice")).Text, out Cost)) { }

                totalQty += Qty;
                totalCost += (Qty * Cost);

            }

            lblPurTolQuantity.Text = "<span class='label label-primary'>Total Quantity : " + totalQty.ToString("0") + "</span>";

            lblPurTotgrandtotal.Text = "<span class='label label-danger'>Total Purchase Value : " + totalCost.ToString("0") + "</span>";

        }//

        protected DataTable tblGridRow()
        {
            DataTable dt = new DataTable();
            DataRow dr;

            dt.Columns.Add("PROD_WGPG");
            dt.Columns.Add("Category");

            dt.Columns.Add("PROD_SUBCATEGORY");
            dt.Columns.Add("SubCategory");

            dt.Columns.Add("PROD_DES");
            dt.Columns.Add("Description");
            dt.Columns.Add("VendorID");
            dt.Columns.Add("Vendor");
            dt.Columns.Add("BarCode");


            dt.Columns.Add("CostPrice");
            dt.Columns.Add("SellPrice");
            dt.Columns.Add("Quantity");

            for (int i = 0; i < gvT_BarCode.Rows.Count; i++)
            {
                dr = dt.NewRow();
                dr["PROD_WGPG"] = ((Label)gvT_BarCode.Rows[i].FindControl("lblPROD_WGPG")).Text;
                dr["Category"] = ((Label)gvT_BarCode.Rows[i].FindControl("lblCategory")).Text;

                dr["PROD_SUBCATEGORY"] = ((Label)gvT_BarCode.Rows[i].FindControl("lblPROD_SUBCATEGORY")).Text;
                dr["SubCategory"] = ((Label)gvT_BarCode.Rows[i].FindControl("lblSubCategory")).Text;

                dr["PROD_DES"] = ((Label)gvT_BarCode.Rows[i].FindControl("lblPROD_DES")).Text;
                dr["Description"] = ((Label)gvT_BarCode.Rows[i].FindControl("lblDescription")).Text;

                dr["VendorID"] = ((Label)gvT_BarCode.Rows[i].FindControl("lblVendorID")).Text;
                dr["Vendor"] = ((Label)gvT_BarCode.Rows[i].FindControl("lblVendor")).Text;

                dr["BarCode"] = ((Label)gvT_BarCode.Rows[i].FindControl("lblBarCode")).Text;
                dr["CostPrice"] = ((Label)gvT_BarCode.Rows[i].FindControl("lblCostPrice")).Text;
                dr["SellPrice"] = ((Label)gvT_BarCode.Rows[i].FindControl("lblSellPrice")).Text;
                dr["Quantity"] = ((Label)gvT_BarCode.Rows[i].FindControl("lblQuantity")).Text;
                dt.Rows.Add(dr);
            }
            return dt;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (gvT_BarCode.Rows.Count<1)
            {
                lblMessage.Text = "Sorry, No Product here!";
                return;
            }
            DateTime PurchaseDateTime = DateTime.Now;
            if (DateTime.TryParse(dtpPurchaseDate.Text,out PurchaseDateTime))
            {
                if (PurchaseDateTime>DateTime.Now)
                {
                    lblMessage.Text = "Please check the Date. NB: Date must be upto Todate!";
                    return;
                }
            }


            int counter = 0;
            string ponumber = "PO-" + Shop_id + DateTime.Now.ToString("yyMMddHHmmss") + " ";
            if (lblOID.Value == string.Empty)
            {
                if (gvT_BarCode.Rows.Count > 0)
                {
                    T_PROD entity = new T_PROD();

                    for (int i = 0; i < gvT_BarCode.Rows.Count; i++)
                    {
                        entity.OID = string.Empty;
                        entity.PROD_WGPG = ((Label)gvT_BarCode.Rows[i].FindControl("lblPROD_WGPG")).Text;
                        entity.PROD_SUBCATEGORY = ((Label)gvT_BarCode.Rows[i].FindControl("lblPROD_SUBCATEGORY")).Text;
                        entity.PROD_DES = ((Label)gvT_BarCode.Rows[i].FindControl("lblPROD_DES")).Text;
                        entity.Vendor_ID = ((Label)gvT_BarCode.Rows[i].FindControl("lblVendorID")).Text;
                        entity.Branch = Shop_id;

                        entity.IDAT = PurchaseDateTime.ToString(); // DateTime.Today.Date.ToString();
                        entity.EDAT = PurchaseDateTime.ToString(); // DateTime.Today.Date.ToString();
                        entity.CostPrice = ((Label)gvT_BarCode.Rows[i].FindControl("lblCostPrice")).Text;
                        entity.SalePrice = ((Label)gvT_BarCode.Rows[i].FindControl("lblSellPrice")).Text;
                        entity.IUSER = userID;
                        entity.PONUMBER = ponumber;
                        if (((Label)gvT_BarCode.Rows[i].FindControl("lblQuantity")).Text == string.Empty)
                        {
                            entity.Quantity = "0";
                        }
                        else
                        {
                            entity.Quantity = ((Label)gvT_BarCode.Rows[i].FindControl("lblQuantity")).Text;
                        }
                        if (((Label)gvT_BarCode.Rows[i].FindControl("lblBarCode")).Text == string.Empty)
                        {
                            entity.Barcode = "";
                        }
                        else
                        {
                            entity.Barcode = ((Label)gvT_BarCode.Rows[i].FindControl("lblBarCode")).Text.Trim();
                            //BILL.T_PROD_Add_Barcode(entity);
                            counter = i + 1;
                        }
                        if (entity.Barcode == "")
                        {

                        }
                        else
                        {
                            entity.Quantity = ((Label)gvT_BarCode.Rows[i].FindControl("lblQuantity")).Text;
                        }
                        //for journal
                        //entity.Narration = string.Format(@"");
                        entity.Flag = T_PROD.AddNewPurchase;
                        //BILL.T_PROD_Add(entity);// Commented By Yeasin 12-May-2019
                        BILL.AddOrEditPurchase(entity);

                        if (i==0)
                        {
                            //save in PurchaseSaleTracing
                            string sqlLoginfo = string.Format(@"
insert into PurchaseSaleTracing (Branch,UserID,Remarks,ReferenceNo,EntryDate  ,OriginalDate)
values('{0}','{1}','{2}','{3}','{4}' ,GETDATE())
", Shop_id,userID,"Purchase",ponumber,PurchaseDateTime);
                            DAL.SaveDataCRUD(sqlLoginfo);
                        }
                    }

                }

                gvT_BarCode.DataSource = null;
                gvT_BarCode.DataBind();



                Clear();

                //Response.Redirect("T_PRODForm.aspx?menuhead=102");
                // ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", " alert('Saved sucessfully'); window.location('T_PRODForm.aspx?menuhead=102');", true);
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", " alert('Saved sucessfully'); window.location('T_PRODForm.aspx?menuhead=102');", true);
                //ClientScript.RegisterStartupScript(this.GetType(), "Alert", "alert('Saved Successfully');", true);
                //Response.Redirect("T_PRODForm.aspx?menuhead=102");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "key", "ShowMessage()", true);
                // ClientScript.RegisterStartupScript(typeof(Page), "MessagePopUp", "<script   language='JavaScript'>alert('Saved sucessfully'); window.location.href = 'T_PRODForm.aspx?menuhead=102';</script>"); 
            }
            else
            {
                string[] valid = new string[2];
                T_PROD entity = new T_PROD();
                //if (chkCash.Checked)
                //{
                //    entity.CashTrans = "1";
                //}
                //else
                //{
                //    entity.CashTrans = "2";
                //}
                entity.PONUMBER = ponumber;
                entity.AccessoriesEdit = Session["AccessoriesEdit"].ToString();
                entity.OID = lblOID.Value.ToString();
                entity.OLDQUANTITY = hidBarcode.Value.ToString();
                entity.Vendor_ID = ddlVendorList.SelectedItem.Value.ToString();
                entity.PROD_WGPG = ddlProductCategoryId.SelectedItem.Value.ToString();
                entity.PROD_SUBCATEGORY = ddlSubCategory.SelectedItem.Value.ToString();
                entity.PROD_DES = ddlDescription.SelectedItem.Value.ToString();
                entity.Branch = Shop_id;
                entity.CostPrice = txtPurchasePrice.Text;
                entity.SalePrice = txtSalePrice.Text;
                entity.Barcode = txtBarcode.InnerText.ToString();
                entity.IDAT = PurchaseDateTime.ToString(); //BILL.GETIDAT(entity.OID);
                if (entity.Barcode == "")
                {
                    entity.Quantity = txtQuantity.Text.ToString();
                }
                else
                {
                    string s = txtBarcode.InnerText;
                    string[] lines = s.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);

                    entity.Quantity = lines.Length.ToString();
                }

                entity.IUSER = userID;
                entity.EUSER = userID;
                entity.EDAT = PurchaseDateTime.ToString(); //DateTime.Today.Date.ToString();
                entity.Flag = T_PROD.EditPurchase;
                valid = BILL.EditValidation(entity);
                if (valid[0].ToString() == "True")
                {
                    lblMessage.Text = valid[1].ToString();
                    //BILL.UpdateStoreMasterStock(entity); // Comment By Yeasin 11-May-2019
                    // BILL.UpdateCashINOUT(entity);
                    //BILL.DeleteTStock(entity); // Comment By Yeasin 11-May-2019
                    //BILL.T_PROD_Add(entity); // Comment By Yeasin 11-May-2019
                    //BILL.UpdateStockPosting(entity);
                    //BILL.UpdateClosingBalance(entity);
                    //-----------monitor product action-----------//
                    BILL.AddOrEditPurchase(entity); // Added By Yeasin 11-May-2019
                    entity.Action = "EditDone";
                    //BILL.MonitorStoreMasterStock(entity);
                    Clear();
                    lblMessage.Text = "Update Successfully..";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "key", "ShowMessageupdate()", true);

                }
                else
                {
                    lblMessage.Text = valid[1].ToString();
                }
            }
            cmdSearch_Click(sender, e);

        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
            cmdAdd.Visible = true;
            tContT_PROD.ActiveTabIndex = 0;


            dtpPurchaseDate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            CE_dtpPurchaseDate.EndDate = DateTime.Now;
        }
        protected void gvT_PROD_ADD_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            DataTable dt = tblGridRow();
            dt.Rows.RemoveAt(e.RowIndex);
            gvT_BarCode.DataSource = dt;
            gvT_BarCode.DataBind();

            LoadTotalQtyTotalCost();
        }
        protected DataTable tblGridRow(System.Web.UI.WebControls.GridViewDeleteEventArgs e, string PROD_WGPG)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("OID");
            dt.Columns.Add("PROD_WGPG");
            dt.Columns.Add("PROD_SUBCATEGORY");
            dt.Columns.Add("Description");
            dt.Columns.Add("Barcode");
            dt.Columns.Add("CCOM_NAME");
            dt.Columns.Add("CostPrice");
            dt.Columns.Add("SalePrice");
            dt.Columns.Add("Quantity");
            DataRow dr;
            for (int i = 0; i < gvT_PROD.Rows.Count; i++)
            {
                dr = dt.NewRow();
                dr["OID"] = ((Label)gvT_PROD.Rows[i].FindControl("lblOID")).Text;
                dr["PROD_WGPG"] = ((Label)gvT_PROD.Rows[i].FindControl("lblPROD_WGPG")).Text;
                dr["PROD_SUBCATEGORY"] = ((Label)gvT_PROD.Rows[i].FindControl("lblPROD_SUBCATEGORY")).Text;
                dr["Description"] = ((Label)gvT_PROD.Rows[i].FindControl("lblDescription")).Text;
                dr["Barcode"] = ((Label)gvT_PROD.Rows[i].FindControl("lblBarcode")).Text;
                dr["CCOM_NAME"] = ((Label)gvT_PROD.Rows[i].FindControl("lblCCOM_NAME")).Text;
                dr["CostPrice"] = ((Label)gvT_PROD.Rows[i].FindControl("lblCostPrice")).Text;
                dr["SalePrice"] = ((Label)gvT_PROD.Rows[i].FindControl("lblSalePrice")).Text;
                dr["Quantity"] = ((Label)gvT_PROD.Rows[i].FindControl("lblQuantity")).Text;
                dt.Rows.Add(dr);
            }
            dt.Rows.RemoveAt(e.RowIndex);
            return dt;
        }
        protected void ddlProductCategoryId_SelectedIndexChanged(object sender, EventArgs e)
        {



            txtBarcode.InnerText = string.Empty;
            txtPurchasePrice.Text = string.Empty;
            txtSalePrice.Text = string.Empty;


        }
        
        private void Clear()
        {

            txtBarcode.InnerText = string.Empty;
            txtPurchasePrice.Text = string.Empty;
            txtSalePrice.Text = string.Empty;
            txtQuantity.Text = string.Empty;
            //CascadingDropDown4.SelectedValue = "";
            //CascadingDropDown6.SelectedValue = "";
            //CascadingDropDown5.SelectedValue = "";
            txtUpdateSalePrice.Text = string.Empty;
            CCD8.SelectedValue = "";
            ddlentry.ClearSelection();


        }
        private void Clear2()
        {

            txtUpdateReturnPRF.Text = string.Empty;
          //  CascadingDropDown7.SelectedValue = "";
           // CascadingDropDown8.SelectedValue = "";
            
            ddlVendorListPRF.Items.Clear();
            ddlUnitCostPRF.Items.Clear();

        }
        //****************************End Product Entry******************************************//     




        //***************** Price Adjustment *************************************//
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            T_PROD entity = new T_PROD();
            string[] valid = new string[2];
            entity.Branch = Shop_id.ToString();
            entity.PROD_WGPG = ddlProductCategoryIdProAdj.SelectedItem.Value.ToString();
            entity.PROD_SUBCATEGORY = ddlSubCategoryIdProAdj.SelectedItem.Value.ToString();
            entity.PROD_DES = ddlDescriptionPrpAdj.SelectedItem.Value.ToString();
            entity.SalePrice = txtUpdateSalePrice.Text;
            valid = BILL.ValidationForPriceUpdate(entity);
            if (valid[0].ToString() == "True")
            {
                lblMessageUpdate.Text = valid[1].ToString();
                BILL.ProductCostUpdate(entity);
                lblMessageUpdate.Text = ContextConstant.UPDATE_SUCCESS;
                Clear();

                Cascadingdropdown1.SelectedValue = string.Empty;
            }
            else
            {
                lblMessageUpdate.Text = valid[1].ToString();
            }

        }
        //*****************End Price Adjustment *************************************//




        protected void ddlentry_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlentry.SelectedValue == "2")
            {
                txtBarcode.Disabled = true;
                txtQuantity.Enabled = true;
                txtBarcode.InnerText = string.Empty;
            }
            else if (ddlentry.SelectedValue == "1")
            {
                txtQuantity.Enabled = false;
                txtBarcode.Disabled = false;
                txtQuantity.Text = string.Empty;
            }
        }
        protected void btnReturn_Click(object sender, EventArgs e)
        {
            string isValid = "yes";

            string cost = string.Empty;
            string QtyLimit = string.Empty;

            string ddlData = ddlUnitCostPRF.SelectedItem.ToString().Trim();
            if (ddlData.Contains(":"))
            {
                string[] code = ddlUnitCostPRF.SelectedItem.Text.Split(':');
                if (code.Length == 4)
                {
                    cost = code[1].Trim();
                    QtyLimit = code[3].Trim();
                }
            }
            string qty = txtUpdateReturnPRF.Text;
            int Rqty = 0;
            int RLimit = 0;
            if (int.TryParse(QtyLimit, out RLimit)) { }
            if (int.TryParse(qty, out Rqty)) { }
            if (Rqty > RLimit) { isValid = "no"; }



            if (isValid == "yes")
            {
                string[] valid = new string[2];
                string Desc = ddlDescriptionPrpRET.SelectedItem.Value.ToString();
                int Vendor_ID = Convert.ToInt16(ddlVendorListPRF.SelectedValue);
                if (Desc == string.Empty)
                {
                    Label16.Text = "Please fill up all the fields";
                    Label16.ForeColor = Color.Red;
                }
                else
                {
                    T_PROD entity = BILL.GetProductInformationbydescription(Desc);
                    //string availquan = BILL.getquantityforPurchaseReturn(Desc, Vendor_ID);
                    entity.ACC_STOCKID = ddlUnitCostPRF.SelectedItem.Value.ToString();
                    entity.Vendor_ID = ddlVendorListPRF.SelectedItem.Value.ToString();
                    entity.Quantity = Rqty.ToString();  // txtUpdateReturnPRF.Text.ToString();
                    entity.CostPrice = cost;                  // txtReturnPrice.Text.ToString();
                    valid = BILL.ReturnValidation(entity);
                    if (valid[0].ToString() == "True")
                    {
                        //BILL.UpdateStoreMasterStock(entity); // Commented By Yeasin 12-May-2019
                        //BILL.UpdatePurchaseReturn(entity); // Commented By Yeasin 12-May-2019
                        //BILL.UpdatePurchaseReport(entity); // Commented By Yeasin 12-May-2019

                        BILL.PurchaseReturn(entity); // Added By Yeasin 12-May-2019
                        Label16.Text = "Purchase Return Successfull";
                        Label16.ForeColor = Color.Green;

                        Clear2();
                    }
                    else
                    {
                        Label16.Text = valid[1].ToString();
                    }
                }
            }
            else
            {
                txtUpdateReturnPRF.Text = string.Empty;
                Label16.Text = "Sorry! Not Enough Stock!";
                txtUpdateReturnPRF.Focus();
            }

            clearDropdownIndex();

        }

        //new by das to clear the dropdown Indexs

        public void clearDropdownIndex() {


            ddlProductCategoryIdRET.SelectedIndex = -1;
            ddlSubCategoryIdProRET.SelectedIndex = -1;
            ddlDescriptionPrpRET.SelectedIndex = -1;
            ddlProductCategoryIdRET_SelectedIndexChanged(null, null);
            ddlSubCategoryIdProRET_SelectedIndexChanged(null, null);
            ddlDescriptionPrpRET_SelectedIndexChanged(null, null);
        
        }

        public DataTable LoadDataByQuery(string sqlQuery)
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
                        //using (DataTable dt = new DataTable())
                        //{
                        sda.Fill(dt);
                        //}
                    }
                }
            }
            return dt;
        }


        protected void ddlDescriptionPrpRET_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlVendorListPRF.Items.Clear();
            ddlUnitCostPRF.Items.Clear();

            if (!string.IsNullOrEmpty(ddlProductCategoryIdRET.SelectedValue) && !string.IsNullOrEmpty(ddlSubCategoryIdProRET.SelectedValue)
              && !string.IsNullOrEmpty(ddlDescriptionPrpRET.SelectedValue))
            {

                //get vendor where  stock qty >0
                string sql = string.Format(@"
select distinct s.VendorID,(v.Vendor_Name+'_'+v.Vendor_mobile) VendorNameMobile,v.Vendor_Name
from Acc_Stock s
inner join Vendor v on v.OID=s.VendorID
where s.Quantity>0 and s.Flag='Quantity' and s.Branch='{0}' and s.PROD_WGPG='{1}' and s.PROD_SUBCATEGORY='{2}' and s.PROD_DES='{3}'
order by v.Vendor_Name
", Shop_id, ddlProductCategoryIdRET.SelectedValue, ddlSubCategoryIdProRET.SelectedValue, ddlDescriptionPrpRET.SelectedValue);
                DataTable dt = DAL.LoadDataByQuery(sql);

                ddlVendorListPRF.DataSource = null;
                if (dt.Rows.Count > 0)
                {
                    ddlVendorListPRF.DataSource = dt;
                }
                ddlVendorListPRF.DataValueField = "VendorID";
                ddlVendorListPRF.DataTextField = "VendorNameMobile";
                ddlVendorListPRF.DataBind();
            }
            ddlVendorListPRF.Items.Insert(0, new ListItem("...Select One...", String.Empty));
        }



       
}
}
