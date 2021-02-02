using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TalukderPOS.BLL;
using TalukderPOS.BusinessObjects;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using TalukderPOS.DAL;
using CrystalDecisions.CrystalReports;
using CrystalDecisions.ReportSource;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;


namespace TalukderPOS.Web.UI
{
    public partial class T_SALES_DTLForm : System.Web.UI.Page
    {

        T_SALES_DTLBLL BILL = new T_SALES_DTLBLL();
        CommonDAL DAL = new CommonDAL();
        private string userID = string.Empty;
        private string userPassword = string.Empty;
        string ShopID = string.Empty;
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
                ShopID = Session["StoreID"].ToString();
                if (userID == "")
                {
                    Response.Redirect("~/frmLogin.aspx");
                }
            }
            catch
            {
                Response.Redirect("~/frmLogin.aspx");
            }

            if (string.IsNullOrEmpty(Session["StuffID"] as string))
            {
                Response.Redirect("~/Pages/ValidateSales2.aspx");
            }

            if (!Page.IsPostBack)
            {
                dtpSalesDate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                CE_dtpSalesDate.EndDate = DateTime.Now;
                gvT_SALES_DTL.DataSource = null;
                gvT_SALES_DTL.DataBind();
                txtdiscount.Text = "0";

                BindList();
                //trCardPaid.Visible = false;
                //trChange.Visible = false;
                //trBank.Visible = true;

                //trCashPaid.Visible = false;
                BindBank();
                //ddlPaymentMode_Selected();
                trCardPaid.Visible = false;
                trCashPaid.Visible = false;
                trBank.Visible = false;
                LoadDDLCategory();
                LoadDDLCustomer();
                ddlCustomerList.SelectedIndex = -1;
                ddlSearchProductCategory.SelectedIndex = -1;
                ddlCustomerList_SelectedIndexChanged(null, null);
                ddlSearchProductCategory_SelectedIndexChanged(null, null);
            }


            txtReceiveAmount.Attributes.Add("onkeypress", "return   RceamounttoSave(event,'" + btnSaveSale.ClientID + "')");

            txtdiscount.Attributes["onfocus"] = "javascript:this.select();";
            txtReceiveAmount.Attributes["onfocus"] = "javascript:this.select();";
        }

        //=============================================================================================================
        private void LoadDDLCustomer()
        {
            string sql = string.Format(@"
SELECT id=ID,text=Name+'-'+convert(nvarchar(100),Number),Number FROM Customers  where Branch='{0}' And CustomerStatus=1 order by name
", Session["StoreID"].ToString());
            DataTable dt = DAL.LoadDataByQuery(sql);

            if (dt.Rows.Count > 0)
            {
                ddlCustomerList.DataSource = dt;
                ddlCustomerList.DataValueField = "id";
                ddlCustomerList.DataTextField = "text";
                ddlCustomerList.DataBind();
                ddlCustomerList.Items.Insert(0, new ListItem("-- Select One--", String.Empty));
            }
        }

        private void LoadDDLCategory()
        {
            string sql = string.Format(@"
select id=OID,text=WGPG_NAME from T_WGPG where WGPG_ACTV=1 AND Shop_id='{0}' order by WGPG_NAME
", Session["StoreID"].ToString());
            DataTable dt = DAL.LoadDataByQuery(sql);

            if (dt.Rows.Count > 0)
            {
                ddlSearchProductCategory.DataSource = dt;
                ddlSearchProductCategory.DataValueField = "id";
                ddlSearchProductCategory.DataTextField = "text";
                ddlSearchProductCategory.DataBind();
                ddlSearchProductCategory.Items.Insert(0, new ListItem("-- Select One--", String.Empty));
            }
        }
        private void LoadDDLSubCategory()
        {
            string sql = string.Format(@"
select id=OID,text=SubCategoryName from SubCategory where CategoryID={0} and Active=1 and ShowOnDropdown='Y' order by SubCategoryName
", ddlSearchProductCategory.SelectedValue.ToString());
            DataTable dt = DAL.LoadDataByQuery(sql);

            if (dt.Rows.Count > 0)
            {
                ddlSearchSubCategory.DataSource = dt;
                ddlSearchSubCategory.DataValueField = "id";
                ddlSearchSubCategory.DataTextField = "text";
                ddlSearchSubCategory.DataBind();

                ddlSearchSubCategory.Items.Insert(0, new ListItem("-- Select One--", String.Empty));

            }
        }

        protected void ddlSearchProductCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlSearchProductCategory.SelectedValue.ToString()))
            {
                LoadDDLSubCategory();
            }
            else
            {
                ddlSearchSubCategory.Items.Clear();
            }
        }
        protected void ddlSearchSubCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlSearchSubCategory.SelectedValue) && ddlSearchSubCategory.SelectedValue != "--Please Select Model--")
            {
                txtDescription.Text = string.Empty;

                ACE_Description.ContextKey = string.Format(@" and StoreMasterStock.Branch='{0}' AND StoreMasterStock.PROD_WGPG={1} and StoreMasterStock.PROD_SUBCATEGORY={2} "
                   , Session["StoreID"].ToString(), ddlSearchProductCategory.SelectedValue, ddlSearchSubCategory.SelectedValue);

                //to focus description
                txtDescription.Enabled = true;
                txtDescription.Focus();
            }
            else
            {
                txtDescription.Enabled = false;
            }
        }//
        //==========================================================================================================
        private void BindList()
        {
            sql = "select OID,PaymentMode as Name from PaymentMode pm where pm.OID not in (16,15,19)";
            CommonBinder.BindDropdownList(ddlPaymentMode, sql);
        }

        private void BindBank()
        {
            //sql = "select OID,BankName as Name from BankInfo where ActiveStatus=1";
            sql = string.Format(@"
select OID,(BankName+'_'+AccountNo) as Name from BankInfo 
where ActiveStatus=1 and ISNULL((BankName+'_'+AccountNo),'')!='' and ShopID={0}
", ShopID);
            CommonBinder.BindDropdownList(ddlBank, sql);
        }

        private void Clear()
        {
            LoadDDLCustomer();
            ddlCustomerList.SelectedIndex = -1;
            ddlSearchProductCategory.SelectedIndex = -1;

            ddlCustomerList_SelectedIndexChanged(null, null);
            ddlSearchProductCategory_SelectedIndexChanged(null, null);

            txtAddress.Text = string.Empty;
            //txtMobileNumber.Text = string.Empty;
            TextDueAmount.Text = string.Empty;
            txtEmailAddress.Text = string.Empty;
            ddlPaymentMode.SelectedIndex = 1;
            ddlBank.SelectedIndex = 0;

            lblOID.Value = string.Empty;

            txtSubTotal.Text = "0";
            txtdiscount.Text = "0";

            txtNetAmount.Text = string.Empty;
            txtReceiveAmount.Text = "0";
            lblcashpaid.Text = "0";
            txtCardPaid.Text = "0";
            lblchange.Text = "0";

            lblMessage.Text = string.Empty;
            txtRemarks.InnerText = string.Empty;
            trBank.Visible = true;

            //
            gvT_SALES_DTL.DataSource = null;
            gvT_SALES_DTL.DataBind();


            btnSaveSale.Enabled = true;
        }





        protected void btnCancel_Click(object sender, EventArgs e)
        {
            dtpSalesDate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            CE_dtpSalesDate.EndDate = DateTime.Now;
            Clear();

        }




        protected void txtdiscount_TextChanged(object sender, EventArgs e)
        {

            LoadNetAmount();

        }



        protected void txtrceamount_TextChanged(object sender, EventArgs e)
        {
            double recamount, netamount;
            if (txtReceiveAmount.Text == string.Empty)
                recamount = 0;
            else
                recamount = Convert.ToDouble(txtReceiveAmount.Text);

            if (txtSubTotal.Text != string.Empty & txtdiscount.Text != string.Empty)
            {
                netamount = BILL.GetNetAmount(txtSubTotal.Text, txtdiscount.Text);

                txtNetAmount.Text = Convert.ToString(Math.Round(Convert.ToDecimal(netamount), 2));

                if (recamount >= Convert.ToDouble(txtNetAmount.Text))
                {
                    lblcashpaid.Text = Convert.ToInt32(Math.Round(Convert.ToDouble(txtNetAmount.Text))).ToString();
                    lblchange.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtReceiveAmount.Text) - Convert.ToDouble(txtNetAmount.Text)));
                    btnSaveSale.Focus();
                }
                else
                {
                    lblcashpaid.Text = Convert.ToInt32(Math.Round(Convert.ToDouble(txtNetAmount.Text))).ToString();
                    lblchange.Text = "0";
                    txtReceiveAmount.Focus();
                }
            }


            if (ddlPaymentMode.SelectedItem.Text == "Cash & Card" & txtReceiveAmount.Text != string.Empty & txtNetAmount.Text != string.Empty)
            {
                if (Convert.ToDouble(txtReceiveAmount.Text) > Convert.ToDouble(txtNetAmount.Text))
                {
                    lblMessage.Text = "Receive Amount can be greater than Net Amount";
                    lblcashpaid.Text = "0";
                    txtCardPaid.Text = "0";
                }
                else
                {
                    lblMessage.Text = string.Empty;
                    lblcashpaid.Text = txtReceiveAmount.Text;



                    txtCardPaid.Text = (Convert.ToDouble(txtNetAmount.Text) - Convert.ToDouble(txtReceiveAmount.Text)).ToString();

                }
                lblchange.Text = "0";
            }
            else
            {
                lblMessage.Text = string.Empty;
            }
        }

        // new to auto change field
        protected void ddlPaymentMode_Selected()
        {
            ddlPaymentMode.SelectedIndex = 4;
            txtReceiveAmount.Text = "0";
            lblcashpaid.Text = "0";
            txtCardPaid.Text = "0";
            lblchange.Text = "0";
            //ddlPaymentMode.Enabled = false;

            if (ddlPaymentMode.SelectedItem.Text == "On Credit")
            {
                //old on credit
                //ReceiveAmount.Visible = false;
                //trCardPaid.Visible = false;
                //trChange.Visible = false;
                //trBank.Visible = false;
                //trCashPaid.Visible = false;
                //ddlBank.SelectedIndex = 0;

                //new On Credit

                txtCardPaid.Enabled = false;
                lblcashpaid.Enabled = true;
                ReceiveAmount.Visible = true;
                //ddlBank.Visible = false;

                //trCardPaid.Visible = true;
                //txtCardPaid.Enabled = true;
                trChange.Visible = false;
                trBank.Visible = true;
                trCashPaid.Visible = true;
                lblcashpaid.Enabled = true;

                if (txtReceiveAmount.Text != string.Empty && txtNetAmount.Text != string.Empty)
                {
                    String tp;
                    tp = Convert.ToString(Math.Round(Convert.ToDouble(lblcashpaid.Text) + Convert.ToDouble(txtCardPaid.Text)));

                    txtReceiveAmount.Text = tp;
                }
            }
        }

        protected void ddlPaymentMode_SelectedIndexChanged(object sender, EventArgs e)
        {

            txtReceiveAmount.Text = "0";
            lblcashpaid.Text = "0";
            txtCardPaid.Text = "0";
            lblchange.Text = "0";
            trChange.Visible = false;
            if (ddlPaymentMode.SelectedIndex == 0) 
            {
                trBank.Visible = false;
                trCashPaid.Visible = false;
                trCardPaid.Visible = false;

            }

            else if (ddlPaymentMode.SelectedItem.Text == "On Credit")
            {
                //old on credit
                //ReceiveAmount.Visible = false;
                //trCardPaid.Visible = false;
                //trChange.Visible = false;
                //trBank.Visible = false;
                trCashPaid.Visible = false;
                //ddlBank.SelectedIndex = 0;
                //new On Credit
                //trCashPaid.Visible = true;
                trCardPaid.Visible = false;
                ddlBank.Enabled = true;
                ReceiveAmount.Visible = true;
                //trCardPaid.Visible = true;
                //txtCardPaid.Enabled = true;
                trBank.Visible = false;
                //trCashPaid.Visible = true;
                //lblcashpaid.Enabled = true;
                //ddlBank.Visible = false;
                if (txtReceiveAmount.Text != string.Empty && txtNetAmount.Text != string.Empty)
                {
                    String tp;
                    tp = Convert.ToString(Math.Round(Convert.ToDouble(lblcashpaid.Text) + Convert.ToDouble(txtCardPaid.Text)));
                    txtReceiveAmount.Text = tp;
                }
            }

            else if (ddlPaymentMode.SelectedItem.Text == "Cash")
            {
                trBank.Visible = false;
                ddlBank.SelectedIndex = 0;
                ddlBank.Enabled = false;
                txtCardPaid.Enabled = false;
                txtCardPaid.Text = "0";
                lblcashpaid.Text = "0";
                lblcashpaid.Enabled = true;
                //trCashPaid.Visible = true;
                trCardPaid.Visible = false;
                if (txtReceiveAmount.Text != string.Empty && txtNetAmount.Text != string.Empty)
                {
                    String tp;
                    tp = Convert.ToString(Math.Round(Convert.ToDouble(lblcashpaid.Text) + Convert.ToDouble(txtCardPaid.Text)));
                    txtReceiveAmount.Text = tp;
                }
            }
            else if (ddlPaymentMode.SelectedItem.Text == "Card")
            {
                trBank.Visible = true;
                trCashPaid.Visible = false;
                trCardPaid.Visible = false; //
                ddlBank.SelectedIndex = 0;
                ddlBank.Enabled = true;
                txtCardPaid.Enabled = true;
                txtCardPaid.Text = "0";
                lblcashpaid.Text = "0";
                lblcashpaid.Enabled = false;
                if (txtReceiveAmount.Text != string.Empty && txtNetAmount.Text != string.Empty)
                {
                    String tp;
                    tp = Convert.ToString(Math.Round(Convert.ToDouble(lblcashpaid.Text) + Convert.ToDouble(txtCardPaid.Text)));

                    txtReceiveAmount.Text = tp;
                }
            }
            else if (ddlPaymentMode.SelectedItem.Text == "Cash & Card")
            {
                trCashPaid.Visible = true;
                trCardPaid.Visible = false;//
                ddlBank.SelectedIndex = 0;
                ddlBank.Enabled = true;
                txtCardPaid.Enabled = true;
                txtCardPaid.Text = "0";
                lblcashpaid.Text = "0";
                lblcashpaid.Enabled = true;
                trBank.Visible = true;
                if (txtReceiveAmount.Text != string.Empty && txtNetAmount.Text != string.Empty)
                {
                    String tp;
                    tp = Convert.ToString(Math.Round(Convert.ToDouble(lblcashpaid.Text) + Convert.ToDouble(txtCardPaid.Text)));

                    txtReceiveAmount.Text = tp;
                }
            }
            else if (ddlPaymentMode.SelectedItem.Text == "Cash & On Credit")
            {
                trCashPaid.Visible = true;
                lblcashpaid.Text = "0";
                trBank.Visible = false;
                txtCardPaid.Text = "0";
                trCardPaid.Visible = false;
            }
        }
        protected void Qty_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox thisTextBox = (TextBox)sender;
                GridViewRow thisGridViewRow = (GridViewRow)thisTextBox.Parent.Parent;
                int row = thisGridViewRow.RowIndex;
                //int rowii = ((sender as TextBox).NamingContainer as GridViewRow).RowIndex;

                DataTable dtshow = new DataTable("dtshow");
                dtshow = (DataTable)Session["dtProperty"];
                if (thisTextBox.Text == "" || thisTextBox.Text == "0")
                {
                    thisTextBox.Text = "1";
                }
                dtshow.Rows[row]["Qty"] = thisTextBox.Text;

                if (Convert.ToInt32(dtshow.Rows[row]["Qty"].ToString()) > Convert.ToInt32(dtshow.Rows[row]["Stock"].ToString()))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "RunCode", "javascript:alert('Out Of Stock');", true);
                    thisTextBox.Text = "1";
                }
                else
                {
                    RefAmount_TextChanged(sender, e);
                }
            }
            catch
            {
            }
        }



        protected void btnSaveSale_Click(object sender, EventArgs e)
        {
            //string full2 = ddlCustomerList.SelectedItem.Text.ToString();
            //string name = full2.Substring(0, full2.IndexOf("-")).Trim();
            DateTime SalesDateTime = DateTime.Now;
            if (DateTime.TryParse(dtpSalesDate.Text, out SalesDateTime))
            {
                if (SalesDateTime > DateTime.Now)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "RunCode", "javascript:alert('Please check the Date. NB: Date must be upto Todate!');", true);
                    //lblMessage.Text = "Please check the Date. NB: Date must be upto Todate!";
                    return;
                }
            }
            
            
            string name = ddlCustomerList.SelectedItem.Text.ToString();
            ddlCustomerList.SelectedItem.Text = name;
            //btnCheckChanges_Click(null, null);

            if (Convert.ToInt16(ddlPaymentMode.SelectedIndex) == 0)
            {
                lblMessage.Text = "Please Select Payment Mode";
                lblMessage.ForeColor = Color.Red;
                return;
            }
            else
            {
                if ((Convert.ToInt16(ddlPaymentMode.SelectedItem.Value) == 12 || Convert.ToInt16(ddlPaymentMode.SelectedItem.Value) == 14 || Convert.ToInt16(ddlPaymentMode.SelectedItem.Value) == 15) && Convert.ToInt32(ddlBank.SelectedValue) < 1)
                {
                    lblMessage.Text = "Please Select Bank";
                    lblMessage.ForeColor = Color.Red;
                    return;
                }
                else
                {
                    if (txtdiscount.Text == string.Empty)
                    {
                        txtdiscount.Text = "0";
                    }

                    txtdiscount_TextChanged(sender, e);
                    
                    if (txtReceiveAmount.Text == null || txtReceiveAmount.Text == string.Empty)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "RunCode", "javascript:alert('Please Check Received Amount');", true);
                        return;
                    }
                    if (gvT_SALES_DTL.Rows.Count < 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "RunCode", "javascript:alert('Please Check Product List');", true);
                        return;
                    }

                    if (Session["StoreID"] != null & Session["StuffID"] != null)
                    {
                        //check total qty >0//Int64 totalSaleQuantity = 0;

                        string strValidQuantity = "yes";
                        for (int t = 0; t < gvT_SALES_DTL.Rows.Count; t++)
                        {
                            if (Convert.ToInt64(((TextBox)gvT_SALES_DTL.Rows[t].FindControl("txtQty")).Text) == 0)
                            {
                                strValidQuantity = "no";
                                break;
                                //totalSaleQuantity += Convert.ToInt64(((TextBox)gvT_SALES_DTL.Rows[t].FindControl("txtQty")).Text);
                            }
                        }
                        
                        if (ddlPaymentMode.SelectedItem.Text == "Cash & Card")
                        {
                            decimal CashPaid = 0;
                            decimal CardPaid = 0;

                            if (decimal.TryParse(lblcashpaid.Text, out CashPaid)) { }
                            if (decimal.TryParse(txtCardPaid.Text, out CardPaid)) { }
                            if (CashPaid == 0 || CardPaid == 0)
                            {
                                strValidQuantity = "no";
                                lblcashpaid.Focus();
                                string msg = string.Format(@"Sorry, Cash or Card amount can not be 0!");
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + msg + "')", true);
                                return;
                            }
                        }
                        if (strValidQuantity == "yes")
                        {
                            //
                            Session["InvoiceNo"] = Session["CCOM_PREFIX"].ToString() + "-" + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss");
                            //lblOID.Value = Session["CCOM_PREFIX"].ToString() + "-" + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss");
                            lblOID.Value = Session["InvoiceNo"].ToString();
                            lblcashpaid.Text = lblcashpaid.Text.Trim() == "" ? "0" : lblcashpaid.Text.Trim();
                            txtCardPaid.Text = txtCardPaid.Text.Trim() == "" ? "0" : txtCardPaid.Text.Trim();
                            T_SALES_DTL entity = new T_SALES_DTL();
                            entity.StoreID = Session["StoreID"].ToString();
                            entity.InvoiceNo = lblOID.Value.ToString();
                            entity.PaymentModeID = ddlPaymentMode.SelectedValue.ToString();
                            entity.BankInfoOID = ddlBank.SelectedValue.ToString();
                            entity.SubTotal = txtSubTotal.Text.ToString();
                            entity.Discount = txtdiscount.Text;
                            entity.DiscountReferencedBy = string.Empty;

                            entity.NetAmount = txtNetAmount.Text.ToString();
                            entity.ReceiveAmount = txtReceiveAmount.Text;
                            entity.CashPaid = lblcashpaid.Text;
                            entity.CashChange = "0";// lblchange.Text;
                            entity.Remarks = txtRemarks.InnerText;
                            entity.StuffID = Session["StuffID"].ToString();
                            entity.IUSER = userID;//userID = Session["UserID"].ToString();
                            entity.IDAT = SalesDateTime.ToString(); // DateTime.Today.Date.ToString();
                            entity.EDAT = SalesDateTime.ToString();
                           // entity.IDAT = DateTime.Today.Date.ToString();
                            //entity.EDAT = DateTime.Today.Date.ToString();        //190112
                            entity.CustomerName = ddlCustomerList.SelectedItem.Text;
                            entity.Address = txtAddress.Text.ToString();
                            entity.MobileNo = txtMobileNumber.Text.ToString();
                            entity.CustomerID = ddlCustomerList.SelectedValue;
                            entity.AlternativeMobileNo = "";
                            entity.DateOfBirth = "";
                            entity.EmailAddress = txtEmailAddress.Text;
                            entity.PreviousDue = PreviousDue.Text;
                            if (gvT_SALES_DTL.Rows.Count > 0)
                            {
                                for (int i = 0; i < gvT_SALES_DTL.Rows.Count; i++)
                                {
                                    CheckBox chkGift = (CheckBox)gvT_SALES_DTL.Rows[i].Cells[2].FindControl("chkGift");

                                    #region

                                    T_SALES_DTL entityDTL = new T_SALES_DTL();

                                    entityDTL.StoreID = Session["StoreID"].ToString();
                                    entityDTL.InvoiceNo = lblOID.Value.ToString();
                                    entityDTL.CategoryID = gvT_SALES_DTL.DataKeys[i].Values["PROD_WGPG"].ToString(); //((Label)gvT_SALES_DTL.Rows[i].FindControl("lblPROD_WGPG")).Text;
                                    entityDTL.DescriptionID = gvT_SALES_DTL.DataKeys[i].Values["DescriptionID"].ToString(); // ((Label)gvT_SALES_DTL.Rows[i].FindControl("lblDescriptionID")).Text;
                                    entityDTL.SubCategoryID = gvT_SALES_DTL.DataKeys[i].Values["PROD_SUBCATEGORY"].ToString(); // ((Label)gvT_SALES_DTL.Rows[i].FindControl("lblSubCategoryID")).Text;
                                    entityDTL.Barcode = gvT_SALES_DTL.DataKeys[i].Values["Barcode"].ToString(); //((TextBox)gvT_SALES_DTL.Rows[i].FindControl("lblBarcode")).Text;

                                    //if (chkGift.Checked)
                                    //{
                                    //    entityDTL.SalePrice = gvT_SALES_DTL.DataKeys[i].Values["SalePrice"].ToString();
                                    //}
                                    //else
                                    //{
                                    //    entityDTL.SalePrice = ((TextBox)gvT_SALES_DTL.Rows[i].FindControl("txtSalePrice")).Text;
                                    //}

                                    entityDTL.SalePrice = ((TextBox)gvT_SALES_DTL.Rows[i].FindControl("txtSalePrice")).Text;
                                    entityDTL.SaleQty = ((TextBox)gvT_SALES_DTL.Rows[i].FindControl("txtQty")).Text;
                                    entityDTL.SaleQty = (entityDTL.SaleQty == "") ? "1" : entityDTL.SaleQty;
                                    entityDTL.PaymentModeID = ddlPaymentMode.SelectedValue.ToString();
                                    entityDTL.DiscountAmount = (((TextBox)gvT_SALES_DTL.Rows[i].FindControl("txtRefAmount")).Text);
                                    entityDTL.DiscountAmount = (entityDTL.DiscountAmount == "") ? "0" : entityDTL.DiscountAmount;
                                    entityDTL.ReturnQty = "0";
                                    entityDTL.GiftAmount = "0";

                                    //if (chkGift.Checked)
                                    //{
                                    //    entityDTL.GiftAmount = (Convert.ToDecimal(entityDTL.SaleQty) * Convert.ToDecimal(entityDTL.SalePrice)).ToString("0.00");
                                    //    entityDTL.Narration = "Gift Product to Customer";
                                    //}
                                    //else
                                    //{
                                    //    entityDTL.Narration = "Sale Product to Customer";
                                    //}

                                    entityDTL.Narration = "Sale Product to Customer";


                                    entityDTL.OID = gvT_SALES_DTL.DataKeys[i].Values["OID"].ToString(); //((Label)gvT_SALES_DTL.Rows[i].FindControl("lblOID")).Text;
                                    entityDTL.CategoryID = gvT_SALES_DTL.DataKeys[i].Values["PROD_WGPG"].ToString(); //((Label)gvT_SALES_DTL.Rows[i].FindControl("lblPROD_WGPG")).Text;
                                    entityDTL.IUSER = userID;
                                    entityDTL.IDAT = SalesDateTime.ToString(); //DateTime.Today.Date.ToString();
                                    entityDTL.EDAT = SalesDateTime.ToString(); //DateTime.Today.Date.ToString();
                                 //   entityDTL.IDAT = DateTime.Today.Date.ToString();
                                  //  entityDTL.EDAT = DateTime.Today.Date.ToString();    //190110
                                    //
                                    //BILL.T_SALES_DTL_Add(entityDTL);

                                    entityDTL.CustomerName = entity.CustomerName;
                                    entityDTL.MobileNo = txtMobileNumber.Text.Trim();
                                    BILL.AddSalesDetails(entityDTL);

                                    #region Sales Details Block
                                    //update acc_stock
                                    //                                    string costpricefirst = "";
                                    //                                    string sqlAccStock = "";
                                    //                                    if (1 == 1)
                                    //                                    {
                                    //                                        //barcode
                                    //                                        if (!string.IsNullOrEmpty(entityDTL.Barcode))
                                    //                                        {
                                    //                                            sqlAccStock = string.Format(@"
                                    //select s.ACC_STOCKID,bc.CostPrice,bc.OID,s.Quantity from T_PROD bc
                                    //inner join Acc_Stock s on s.CostPrice=bc.CostPrice
                                    // where ISNULL(s.Flag,'')='' and s.Quantity>0 and bc.SaleStatus='0' and bc.Branch='{0}' and bc.Barcode='{1}'
                                    //", entityDTL.StoreID, entityDTL.Barcode);
                                    //                                            //DataTable dtBarcode = LoadDataByQuery(sqlAccStockBarcode);
                                    //                                            //if (dtBarcode.Rows.Count > 0) { costpricefirst = dtBarcode.Rows[0]["CostPrice"].ToString(); }
                                    //                                        }
                                    //                                        else
                                    //                                        {
                                    //                                            //qty
                                    //                                            sqlAccStock = string.Format(@"
                                    //select s.ACC_STOCKID,s.CostPrice, s.Quantity
                                    //from Acc_Stock s
                                    //inner join Acc_Stock scum on s.ACC_STOCKID >= scum.ACC_STOCKID
                                    //where s.Flag='Quantity' and s.Quantity>0 and s.Branch='{0}' and s.PROD_WGPG={1} and s.PROD_SUBCATEGORY={2} and s.PROD_DES={3}
                                    //group by s.ACC_STOCKID, s.Quantity,s.CostPrice
                                    //order by s.ACC_STOCKID
                                    //", entityDTL.StoreID, entityDTL.CategoryID, entityDTL.SubCategoryID, entityDTL.DescriptionID);
                                    //                                        }

                                    //                                        DataTable dt = LoadDataByQuery(sqlAccStock);
                                    //                                        string costpricepurchasehgt = dt.Rows[0]["CostPrice"].ToString();
                                    //                                        costpricefirst = costpricepurchasehgt;

                                    //                                        entityDTL.CostPrice = costpricefirst;//new
                                    //                                        entityDTL.CustomerName = entity.CustomerName;
                                    //                                        entityDTL.MobileNo = txtMobileNumber.Text.Trim();
                                    //BILL.T_SALES_DTL_Journal(entityDTL);


                                    //                                        int totaQty = Convert.ToInt32(entityDTL.SaleQty);
                                    //                                        for (int j = 0; j < dt.Rows.Count; j++)
                                    //                                        {
                                    //                                            #region
                                    //                                            int differenceQty = Convert.ToInt32(dt.Rows[j]["Quantity"]) - totaQty;
                                    //                                            int differenceQtynew = Convert.ToInt32(dt.Rows[j]["Quantity"]) - totaQty;
                                    //                                            int differenceQtynew2 = totaQty - Convert.ToInt32(dt.Rows[j]["Quantity"]);

                                    //                                            int totaQtynew = totaQty;
                                    //                                            int updateWithQty = differenceQty >= 0 ? differenceQty : 0;

                                    //                                            //restQty = differenceQty;
                                    //                                            totaQty = System.Math.Abs(differenceQty);
                                    //                                            //restQty = differenceQty;

                                    //                                            if (updateWithQty >= 0 && totaQty > 0)  //
                                    //                                            {
                                    //                                                string sqlAccStockUpdate = string.Format(@"
                                    //UPDATE Acc_Stock SET Quantity = {1},SQty=SQty+{1}  WHERE ACC_STOCKID={0}
                                    //", dt.Rows[j]["ACC_STOCKID"].ToString(), updateWithQty);

                                    //                                                SaveDataCRUD(sqlAccStockUpdate);

                                    //                                                //current row qty   
                                    //                                                //if qty>0 break
                                    //                                                string checkQty = string.Format(@"
                                    //select Quantity from Acc_Stock where ACC_STOCKID={0}", dt.Rows[j]["ACC_STOCKID"].ToString());

                                    //                                                DataTable dtQty = LoadDataByQuery(checkQty);


                                    //                                                string costpricepurchase = dt.Rows[j]["CostPrice"].ToString();
                                    //                                                string costpricepurchaseOID = dt.Rows[j]["ACC_STOCKID"].ToString();


                                    //                                                //                                                if (!string.IsNullOrEmpty(entityDTL.Barcode))
                                    //                                                //                                                {
                                    //                                                //                                                    string sqlAccStockBarcode2 = string.Format(@"
                                    //                                                //select ISNULL(p.CostPrice,0) CostPrice from T_PROD p where p.SaleStatus='0' and p.Branch='{0}' and p.Barcode='{1}'
                                    //                                                //", entityDTL.StoreID, entityDTL.Barcode);

                                    //                                                //                                                    DataTable dtBarcode2 = LoadDataByQuery(sqlAccStockBarcode2);
                                    //                                                //                                                    if (dtBarcode2.Rows.Count > 0) { costpricepurchase = dtBarcode2.Rows[0]["CostPrice"].ToString(); }
                                    //                                                //                                                }



                                    //                                                entityDTL.PURCHASECOST = costpricepurchase;
                                    //                                                entityDTL.PURCHASECOSTOID = costpricepurchaseOID;
                                    //                                                //for journal

                                    //                                                // new for no mobile no
                                    //                                                //entityDTL.MobileNo = "";
                                    //                                                //BILL.T_SALES_DTL_StockNew(entityDTL);

                                    //                                                if (Convert.ToInt32(dtQty.Rows[0]["Quantity"]) > 0)
                                    //                                                {
                                    //                                                    if (j == 0)
                                    //                                                    {
                                    //                                                        // entityDTL.SaleQty = updateWithQty.ToString();
                                    //                                                        BILL.T_SALES_DTL_StockNew(entityDTL);
                                    //                                                        entity.Barcode = string.Empty;
                                    //                                                        break;
                                    //                                                    }
                                    //                                                    else
                                    //                                                    {
                                    //                                                        entityDTL.SaleQty = totaQtynew.ToString();
                                    //                                                        BILL.T_SALES_DTL_StockNew(entityDTL);
                                    //                                                        break;
                                    //                                                    }
                                    //                                                }
                                    //                                                else
                                    //                                                {
                                    //                                                    entityDTL.SaleQty = dt.Rows[j]["Quantity"].ToString();
                                    //                                                    BILL.T_SALES_DTL_StockNew(entityDTL);
                                    //                                                }
                                    //                                            }
                                    //                                            else
                                    //                                            {
                                    //                                                string sqlAccStockUpdate = string.Format(@"
                                    //UPDATE Acc_Stock SET Quantity = {1},SQty=SQty+{1}   WHERE ACC_STOCKID={0}
                                    //", dt.Rows[j]["ACC_STOCKID"].ToString(), updateWithQty);

                                    //                                                SaveDataCRUD(sqlAccStockUpdate);

                                    //                                                //current row qty   
                                    //                                                //if qty>0 break
                                    //                                                string checkQty = string.Format(@"
                                    //select Quantity from Acc_Stock where ACC_STOCKID={0}", dt.Rows[j]["ACC_STOCKID"].ToString());

                                    //                                                DataTable dtQty = LoadDataByQuery(checkQty);


                                    //                                                string costpricepurchase = dt.Rows[j]["CostPrice"].ToString();
                                    //                                                string costpricepurchaseOID = dt.Rows[j]["ACC_STOCKID"].ToString();
                                    //                                                //                                                if (!string.IsNullOrEmpty(entityDTL.Barcode))
                                    //                                                //                                                {
                                    //                                                //                                                    string sqlAccStockBarcode2 = string.Format(@"
                                    //                                                //select ISNULL(p.CostPrice,0) CostPrice from T_PROD p where p.SaleStatus='0' and p.Branch='{0}' and p.Barcode='{1}'
                                    //                                                //", entityDTL.StoreID, entityDTL.Barcode);

                                    //                                                //                                                    DataTable dtBarcode2 = LoadDataByQuery(sqlAccStockBarcode2);
                                    //                                                //                                                    if (dtBarcode2.Rows.Count > 0) { costpricepurchase = dtBarcode2.Rows[0]["CostPrice"].ToString(); }
                                    //                                                //                                                }



                                    //                                                entityDTL.PURCHASECOST = costpricepurchase;
                                    //                                                entityDTL.PURCHASECOSTOID = costpricepurchaseOID;
                                    //                                                //for journal
                                    //                                                entityDTL.MobileNo = txtMobileNumber.Text.Trim();
                                    //                                                //entityDTL.MobileNo = "";
                                    //                                                //BILL.T_SALES_DTL_StockNew(entityDTL);


                                    //                                                entityDTL.SaleQty = dt.Rows[j]["Quantity"].ToString();
                                    //                                                BILL.T_SALES_DTL_StockNew(entityDTL);
                                    //                                                break;
                                    //                                            }
                                    //                                            #endregion
                                    //                                        }
                                    //                                    }

                                    //                                    //updateStock          
                                    //                                    if (chkGift.Checked)
                                    //                                    {
                                    //                                        //expense jounal
                                    //                                        string sqlDailyCost = string.Format(@"
                                    //select ch.OID from CostingHead ch where ch.CostingHead='Expense For Gift' and ch.Shop_id='{0}'
                                    //
                                    //", ShopID);
                                    //                                        DataTable dt = DAL.LoadDataByQuery(sqlDailyCost);
                                    //                                        if (dt.Rows.Count > 0)
                                    //                                        {
                                    //                                            entityDTL.LedgerAccID = dt.Rows[0]["OID"].ToString();
                                    //                                        }
                                    //                                        else
                                    //                                        {
                                    //                                            entityDTL.LedgerAccID = "0";
                                    //                                        }

                                    //                                        entityDTL.PassedAmount = costpricefirst; // (Convert.ToInt32(costpricefirst) * Convert.ToInt32(entityDTL.SaleQty)).ToString();
                                    //                                        //entityDTL.LedgerAccID = "4";
                                    //                                        entityDTL.LedgerAccParticular = "Expense";
                                    //                                        entityDTL.LedgerAccCusName = ddlCustomerList.SelectedItem.Text;
                                    //                                        entityDTL.LedgerAccRemarks = "Expense";
                                    //                                        entityDTL.Narration = "Expense for Gift";
                                    //                                        BILL.T_SALES_Acc_JournalForGiftDiscount(entityDTL);
                                    //                                    }

                                    //                                    //change if with barcode 
                                    //                                    if (gvT_SALES_DTL.DataKeys[i].Values["Flag"].ToString() == "Barcode")
                                    //                                    {
                                    //                                        //update the T_Prod   and   T_STOCK
                                    //                                        string cmdStr = string.Format(@"
                                    //update T_PROD    set SaleStatus=1,SalesDate='{0}'   where  SaleStatus=0 and Barcode='{1}' and Branch='{2}'
                                    //update T_STOCK   set SaleStatus=1,SalesDate='{0}'   where  SaleStatus=0 and Barcode='{1}' and Branch='{2}' 
                                    //", DateTime.Now.ToString(), gvT_SALES_DTL.DataKeys[i].Values["Barcode"].ToString(), gvT_SALES_DTL.DataKeys[i].Values["StoreID"].ToString()
                                    //);
                                    //                                        SaveDataCRUD(cmdStr);
                                    //                                    } 
                                    #endregion
                                    #endregion
                                    //}
                                }
                                // Start..Master Part //
                                #region Sales Master Part
                                //                                if (ddlPaymentMode.SelectedItem.Text == "On Credit")
                                //                                {
                                //                                    if (Convert.ToDecimal(lblcashpaid.Text) > 0)
                                //                                    {
                                //                                        entity.LedgerAccID = "1";
                                //                                        entity.LedgerAccParticular = "Cash";
                                //                                        entity.LedgerAccRemarks = "Cash";
                                //                                        entity.LedgerAccCusName = ddlCustomerList.SelectedItem .Text;
                                //                                        entity.MobileNo = txtMobileNumber.Text.ToString(); 
                                //                                        //entity.MobileNo = "txt.Mobile";
                                //                                        entity.Narration = "Cash Received from Customer";
                                //                                        entity.ReceiveAmount = lblcashpaid.Text;
                                //                                        BILL.T_SALES_Acc_Journal(entity);
                                //                                    }

                                //                                    if (Convert.ToDecimal(TextDueAmount.Text) > 0)
                                //                                    {
                                //                                        entity.RemainingAmount = TextDueAmount.Text;
                                //                                        entity.LedgerAccID = ddlCustomerList .SelectedValue;

                                //                                        //entity.MobileNo = "txt.Mobile";
                                //                                        //entity.MobileNo = "";
                                //                                        entity.MobileNo = txtMobileNumber.Text.ToString(); 
                                //                                        entity.LedgerAccParticular = "A/R";
                                //                                        entity.LedgerAccCusName = ddlCustomerList.SelectedItem.Text;
                                //                                        entity.LedgerAccRemarks = "Customer";
                                //                                        entity.Narration = "Sales on Credit";

                                //                                        BILL.T_SALES_Acc_Journal_oncr(entity);
                                //                                    }
                                //                                }
                                //                                //journal  for discount
                                //                                decimal discount = 0;
                                //                                if (decimal.TryParse(txtdiscount.Text, out discount)) { }
                                //                                if (discount > 0)
                                //                                {

                                //                                    string sqlDailyCost = string.Format(@"
                                //select ch.OID from CostingHead ch where ch.CostingHead='Discount On Sales' and ch.Shop_id='{0}'
                                //
                                //", ShopID);
                                //                                    DataTable dt = DAL.LoadDataByQuery(sqlDailyCost);
                                //                                    if (dt.Rows.Count > 0)
                                //                                    {
                                //                                        entity.LedgerAccID = dt.Rows[0]["OID"].ToString();
                                //                                    }
                                //                                    else
                                //                                    {
                                //                                        entity.LedgerAccID = "0";
                                //                                    }

                                //                                    entity.PassedAmount = entity.Discount;
                                //                                    //entity.LedgerAccID = "3";
                                //                                    entity.LedgerAccParticular = "Expense";

                                //                                    entity.LedgerAccCusName = ddlCustomerList.SelectedItem.Text;
                                //                                    entity.LedgerAccRemarks = "Expense";
                                //                                    entity.Narration = "Expense for Discount";

                                //                                    BILL.T_SALES_Acc_JournalForDiscount(entity);
                                //                                } 
                                #endregion
                                entity.BankId = "0";
                                entity.CardAmount = "0";
                                if (ddlPaymentMode.SelectedItem.Text == "Cash") 
                                {
                                    entity.CashPaid = txtNetAmount.Text;
                                }
                                if (ddlPaymentMode.SelectedItem.Text == "Card" || ddlPaymentMode.SelectedItem.Text == "Cash & Card")
                                {
                                    entity.BankId = ddlBank.SelectedValue.ToString();
                                    entity.CardAmount = ddlPaymentMode.SelectedItem.Text == "Card" ? txtNetAmount.Text : (Convert.ToInt64(txtNetAmount.Text) - Convert.ToInt64(lblcashpaid.Text)).ToString();
                                    entity.ReceiveAmount = (Convert.ToInt64(lblcashpaid.Text) + Convert.ToInt64(entity.CardAmount)).ToString();
                                }
                                if (ddlPaymentMode.SelectedItem.Text == "On Credit")
                                {
                                    entity.ReceiveAmount = (Convert.ToInt64(lblcashpaid.Text) + Convert.ToInt64(txtCardPaid.Text)).ToString();
                                    if (ddlBank.SelectedIndex > 0) 
                                    {
                                        //entity.BankId = ddlBank.SelectedValue.ToString();
                                        //entity.CardAmount = txtCardPaid.Text;
                                    }
                                }
                                BILL.AddSalesMaster(entity);
                            }

                            gvT_SALES_DTL.DataSource = null;
                            gvT_SALES_DTL.DataBind();

                            Session.Remove("InvoiceNo");
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "RunCode", "javascript:alert('Sale Successfull');", true);
                            DataTable dtsales = BILL.SPP_GetInvoice(lblOID.Value.ToString());
                            Clear();
                            Session["dtsales"] = dtsales;

                            //string sqlhalfinvoice = string.Format(@"select * from Half_Invoice where SHOP_ID='{0}'", Session["StoreID"].ToString());
                            //DataTable dthalf = DAL.LoadDataByQuery(sqlhalfinvoice);
                            //if (dthalf.Rows.Count > 0)
                            //{
                            //    if (Session["StoreID"].ToString() == "5")
                            //    {
                            //        Session["ReportPath"] = "~/Reports/rptInvoiceHalfDristy.rpt";
                            //    }
                            //    else if (Session["StoreID"].ToString() == "54" || Session["StoreID"].ToString() == "57" || Session["StoreID"].ToString() == "67")
                            //    {
                            //        Session["ReportPath"] = "~/Reports/rptInvoiceHalfAponJhon.rpt";
                            //    }
                            //    else if (Session["StoreID"].ToString() == "71")
                            //    {
                            //        Session["ReportPath"] = "~/Reports/rptInvoicePOSPrinter.rpt";
                            //    }

                            //    else
                            //    {
                            //        Session["ReportPath"] = "~/Reports/rptInvoiceHalf.rpt";
                            //    }
                            //}
                            //if (Session["StoreID"].ToString() == "59" || Session["StoreID"].ToString() == "60")
                            //{

                            //}
                            //else { 
                                Session["ReportPath"] = "~/Reports/rptInvoice_NewA4.rpt"; 
                            //}

                            //Session["ReportPath"] = "~/Reports/rptInvoice.rpt";

                            string webUrl = "../Reports/ReportView.aspx";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenWindow", "window.open('" + webUrl + "','_newtab');", true);
                            txtBarcode.Focus();
                        }
                        else
                        {
                            //show message qty 0
                            //Sorry, Not Enough Stock!
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "RunCode", "javascript:alert('Sorry, Quantity must be greater than 0!');", true);
                            txtBarcode.Focus();
                            return;
                        }
                    }
                }
            }
        }


        private void SaveDetails()
        {
            if (gvT_SALES_DTL.Rows.Count > 0)
            {
                for (int i = 0; i < gvT_SALES_DTL.Rows.Count; i++)
                {
                    T_SALES_DTL entity = new T_SALES_DTL();
                    entity.InvoiceNo = lblOID.Value.ToString();
                    entity.DescriptionID = ((Label)gvT_SALES_DTL.Rows[i].FindControl("lblDescriptionID")).Text;
                    entity.Barcode = ((Label)gvT_SALES_DTL.Rows[i].FindControl("lblBarcode")).Text;
                    entity.SalePrice = ((TextBox)gvT_SALES_DTL.Rows[i].FindControl("txtSalePrice")).Text;
                    entity.SaleQty = ((TextBox)gvT_SALES_DTL.Rows[i].FindControl("Qty")).Text;
                    entity.DiscountReferenceOID = ((DropDownList)gvT_SALES_DTL.Rows[i].FindControl("ddlRefBy")).Text;
                    entity.DiscountAmount = ((TextBox)gvT_SALES_DTL.Rows[i].FindControl("RefAmount")).Text;
                    entity.ReturnQty = "0";
                    entity.OID = ((Label)gvT_SALES_DTL.Rows[i].FindControl("lblOID")).Text;
                    entity.CategoryID = ((Label)gvT_SALES_DTL.Rows[i].FindControl("lblPROD_WGPG")).Text;
                    entity.IDAT = DateTime.Today.Date.ToString();
                    BILL.T_SALES_DTL_Add(entity);
                }
            }
        }

        protected void gvT_WGPG_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            DataTable dt = tblGridRow();
            string str = dt.Rows[e.RowIndex]["SalePrice"].ToString();
            txtSubTotal.Text = (Convert.ToInt64(txtSubTotal.Text) - Convert.ToInt64(str)).ToString();
            txtdiscount.Text = (Convert.ToInt32(txtdiscount.Text) - Convert.ToInt32(dt.Rows[e.RowIndex]["RefAmount"].ToString())).ToString();
            txtdiscount_TextChanged(sender, e);
            dt.Rows.RemoveAt(e.RowIndex);
            gvT_SALES_DTL.DataSource = dt;
            gvT_SALES_DTL.DataBind();
        }

        protected DataTable tblGridRow()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("OID");
            dt.Columns.Add("PROD_WGPG");
            dt.Columns.Add("WGPG_NAME");
            dt.Columns.Add("SubCategoryID");
            dt.Columns.Add("SubCategoryName");
            dt.Columns.Add("DescriptionID");
            dt.Columns.Add("Description");
            dt.Columns.Add("Barcode");
            dt.Columns.Add("StoreID");
            dt.Columns.Add("Stock");
            dt.Columns.Add("SalePrice");
            dt.Columns.Add("Qty");
            dt.Columns.Add("RefAmount");
            dt.Columns.Add("TotalPrice");
            DataRow dr;
            for (int i = 0; i < gvT_SALES_DTL.Rows.Count; i++)
            {
                dr = dt.NewRow();
                dr["OID"] = ((Label)gvT_SALES_DTL.Rows[i].FindControl("lblOID")).Text;
                dr["PROD_WGPG"] = ((Label)gvT_SALES_DTL.Rows[i].FindControl("lblPROD_WGPG")).Text;
                dr["WGPG_NAME"] = ((Label)gvT_SALES_DTL.Rows[i].FindControl("lblWGPG_NAME")).Text;
                dr["SubCategoryID"] = ((Label)gvT_SALES_DTL.Rows[i].FindControl("lblSubCategoryID")).Text;
                dr["SubCategoryName"] = ((Label)gvT_SALES_DTL.Rows[i].FindControl("lblSubCategoryName")).Text;

                dr["DescriptionID"] = ((Label)gvT_SALES_DTL.Rows[i].FindControl("lblDescriptionID")).Text;
                dr["Description"] = ((Label)gvT_SALES_DTL.Rows[i].FindControl("lblDescription")).Text;

                dr["StoreID"] = ((Label)gvT_SALES_DTL.Rows[i].FindControl("lblStoreID")).Text;
                dr["Stock"] = ((Label)gvT_SALES_DTL.Rows[i].FindControl("lblStock")).Text;
                dr["SalePrice"] = ((TextBox)gvT_SALES_DTL.Rows[i].FindControl("txtSalePrice")).Text;  //
                dr["Qty"] = ((TextBox)gvT_SALES_DTL.Rows[i].FindControl("Qty")).Text;
                dr["RefAmount"] = ((TextBox)gvT_SALES_DTL.Rows[i].FindControl("RefAmount")).Text;
                dr["TotalPrice"] = ((Label)gvT_SALES_DTL.Rows[i].FindControl("lblTotalPrice")).Text;
                dt.Rows.Add(dr);
            }
            return dt;
        }

        protected void RefAmount_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int n = ((sender as TextBox).NamingContainer as GridViewRow).RowIndex;

                string strMRP = string.IsNullOrEmpty(((TextBox)gvT_SALES_DTL.Rows[n].Cells[3].FindControl("txtSalePrice")).Text)
                        ? "0" : ((TextBox)gvT_SALES_DTL.Rows[n].Cells[3].FindControl("txtSalePrice")).Text;
                string strStock = string.IsNullOrEmpty(((Label)gvT_SALES_DTL.Rows[n].Cells[4].FindControl("lblStock")).Text)
                    ? "0" : ((Label)gvT_SALES_DTL.Rows[n].Cells[4].FindControl("lblStock")).Text;
                string strQty = string.IsNullOrEmpty(((TextBox)gvT_SALES_DTL.Rows[n].Cells[5].FindControl("txtQty")).Text)
                    ? "0" : ((TextBox)gvT_SALES_DTL.Rows[n].Cells[5].FindControl("txtQty")).Text;
                string strRefAmount = string.IsNullOrEmpty(((TextBox)gvT_SALES_DTL.Rows[n].Cells[7].FindControl("txtRefAmount")).Text)
                    ? "0" : ((TextBox)gvT_SALES_DTL.Rows[n].Cells[7].FindControl("txtRefAmount")).Text;
                string strNetAmount = string.IsNullOrEmpty(((Label)gvT_SALES_DTL.Rows[n].Cells[8].FindControl("lblTotalPrice")).Text)
                    ? "0" : ((Label)gvT_SALES_DTL.Rows[n].Cells[8].FindControl("lblTotalPrice")).Text;

                double MRP = 0;
                double Stock = 0;
                double Qty = 0;
                double RefAmount = 0;
                double NetAmount = 0;

                if (double.TryParse(strMRP, out MRP) && double.TryParse(strStock, out Stock)
                    && double.TryParse(strQty, out Qty) && double.TryParse(strRefAmount, out RefAmount)
                    && double.TryParse(strNetAmount, out NetAmount)
                    )
                {
                    TextBox txtMRP = (TextBox)gvT_SALES_DTL.Rows[n].Cells[3].FindControl("txtSalePrice");
                    Label lblStock = (Label)gvT_SALES_DTL.Rows[n].Cells[4].FindControl("lblStock");
                    TextBox txtQty = (TextBox)gvT_SALES_DTL.Rows[n].Cells[5].FindControl("txtQty");
                    TextBox txtRefAmount = (TextBox)gvT_SALES_DTL.Rows[n].Cells[7].FindControl("txtRefAmount");
                    Label lblNetAmount = (Label)gvT_SALES_DTL.Rows[n].Cells[8].FindControl("lblTotalPrice");

                    txtQty.Text = (Qty).ToString();
                    lblNetAmount.Text = ((MRP * Qty) - RefAmount).ToString();

                    //loadData
                    LoadData();
                }

            }
            catch (Exception ex)
            {
                //error msg
            }
            finally { }
        }


        //sadiq
        protected void txtDescription_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtDescription.Text))
            {

                if (string.IsNullOrEmpty(Session["StuffID"] as string))
                {
                    Response.Redirect("~/Pages/ValidateSales.aspx");
                }
                else
                {
                    //check in database
                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
                    string cmdstr = string.Format(@"
select
StoreMasterStock.OID,StoreMasterStock.PROD_WGPG,StoreMasterStock.PROD_SUBCATEGORY,StoreMasterStock.PROD_DES AS DescriptionID
,StoreMasterStock.Branch as StoreID,Barcode='',
StoreMasterStock.SalePrice

,(StoreMasterStock.Quantity - StoreMasterStock.SaleQuantity

-ISNULL((select COUNT (tp.OID ) from T_PROD tp 
where tp.Branch =StoreMasterStock.Branch and tp.SaleStatus ='0' and tp.PROD_DES =StoreMasterStock .PROD_DES 
group by tp.PROD_DES),0)
)as Stock,
1 Qty,(1*StoreMasterStock.SalePrice) TotalPrice,0 as RefAmount
,T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,Description.Description
,Flag='Description'

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
AND LTRIM(RTRIM(Description.Description)) = LTRIM(RTRIM('{2}'))  --AND StoreMasterStock.PROD_DES={2}
and StoreMasterStock.Branch='{3}'

", ddlSearchProductCategory.SelectedValue, ddlSearchSubCategory.SelectedValue, txtDescription.Text, Session["StoreID"]);

                    DataTable dtProductToAdd = GetDataTableByQuery(cmdstr);

                    if (dtProductToAdd.Rows.Count > 0)
                    {
                        #region check existance in gridview

                        int rowIndexExistDescriptionID = -1;
                        string IsDescriptionIDExisted = "no";
                        string newDescriptionID = dtProductToAdd.Rows[0]["DescriptionID"].ToString();
                        if (gvT_SALES_DTL.Rows.Count > 0)
                        {
                            for (int i = 0; i < gvT_SALES_DTL.Rows.Count; i++)
                            {
                                string existedDescriptionID = gvT_SALES_DTL.DataKeys[i].Values["DescriptionID"].ToString();
                                string existedFlag = gvT_SALES_DTL.DataKeys[i].Values["Flag"].ToString();
                                if (newDescriptionID == existedDescriptionID && existedFlag == "Description")
                                {
                                    IsDescriptionIDExisted = "yes";
                                    rowIndexExistDescriptionID = i;
                                    break;
                                }
                            }
                        }

                        #endregion


                        //
                        if (IsDescriptionIDExisted == "no")
                        {
                            // //get gridview data

                            //dt
                            DataTable DTGridData = new DataTable();
                            DTGridData.TableName = "gridTable";

                            #region columns
                            DTGridData.Columns.Add(new DataColumn("OID", typeof(Int64)));
                            DTGridData.Columns.Add(new DataColumn("PROD_WGPG", typeof(Int64)));
                            DTGridData.Columns.Add(new DataColumn("PROD_SUBCATEGORY", typeof(Int64)));
                            DTGridData.Columns.Add(new DataColumn("DescriptionID", typeof(Int64)));
                            DTGridData.Columns.Add(new DataColumn("StoreID", typeof(string)));//

                            DTGridData.Columns.Add(new DataColumn("Barcode", typeof(string)));

                            DTGridData.Columns.Add(new DataColumn("SalePrice", typeof(double)));
                            DTGridData.Columns.Add(new DataColumn("Stock", typeof(double)));
                            DTGridData.Columns.Add(new DataColumn("Qty", typeof(double)));
                            DTGridData.Columns.Add(new DataColumn("TotalPrice", typeof(double)));
                            DTGridData.Columns.Add(new DataColumn("RefAmount", typeof(double)));//

                            DTGridData.Columns.Add("WGPG_NAME");
                            DTGridData.Columns.Add("SubCategoryName");
                            DTGridData.Columns.Add("Description");
                            DTGridData.Columns.Add("Flag");
                            DTGridData.Columns.Add("GiftStatus");//gift


                            #endregion

                            #region get the new data row for barcode
                            DataRow DTGridDataNewRow = DTGridData.NewRow();
                            //
                            DTGridDataNewRow["OID"] = dtProductToAdd.Rows[0]["OID"].ToString();
                            DTGridDataNewRow["PROD_WGPG"] = dtProductToAdd.Rows[0]["PROD_WGPG"].ToString();
                            DTGridDataNewRow["PROD_SUBCATEGORY"] = dtProductToAdd.Rows[0]["PROD_SUBCATEGORY"].ToString();
                            DTGridDataNewRow["DescriptionID"] = dtProductToAdd.Rows[0]["DescriptionID"].ToString();
                            DTGridDataNewRow["StoreID"] = dtProductToAdd.Rows[0]["StoreID"].ToString();
                            DTGridDataNewRow["Barcode"] = dtProductToAdd.Rows[0]["Barcode"].ToString();



                            string strMRPn = string.IsNullOrEmpty(dtProductToAdd.Rows[0]["SalePrice"].ToString())
                                    ? "0" : dtProductToAdd.Rows[0]["SalePrice"].ToString();
                            string strStockn = string.IsNullOrEmpty(dtProductToAdd.Rows[0]["Stock"].ToString())
                                ? "0" : dtProductToAdd.Rows[0]["Stock"].ToString();
                            //string strQtyn = string.IsNullOrEmpty(dtProductToAdd.Rows[0]["Qty"].ToString())
                            //    ? "0" : dtProductToAdd.Rows[0]["Qty"].ToString();
                            //string strRefAmount = string.IsNullOrEmpty(dtProductToAdd.Rows[0]["RefAmount"].ToString())
                            //    ? "0" : dtProductToAdd.Rows[0]["RefAmount"].ToString();


                            double MRPn = 0; double Stockn = 0;

                            if (double.TryParse(strMRPn, out MRPn) && double.TryParse(strStockn, out Stockn))
                            {
                                DTGridDataNewRow["SalePrice"] = MRPn;
                                DTGridDataNewRow["Stock"] = Stockn;
                                DTGridDataNewRow["Qty"] = 1;
                                DTGridDataNewRow["RefAmount"] = 0;
                                DTGridDataNewRow["TotalPrice"] = MRPn;

                            }


                            DTGridDataNewRow["WGPG_NAME"] = dtProductToAdd.Rows[0]["WGPG_NAME"].ToString();
                            DTGridDataNewRow["SubCategoryName"] = dtProductToAdd.Rows[0]["SubCategoryName"].ToString();
                            DTGridDataNewRow["Description"] = dtProductToAdd.Rows[0]["Description"].ToString();
                            DTGridDataNewRow["Flag"] = dtProductToAdd.Rows[0]["Flag"].ToString();
                            DTGridDataNewRow["GiftStatus"] = "false";//gift

                            DTGridData.Rows.Add(DTGridDataNewRow);
                            #endregion

                            for (int i = 0; i < gvT_SALES_DTL.Rows.Count; i++)
                            {
                                DataRow DTGridDataOldRow = DTGridData.NewRow();
                                //
                                #region MyRegion
                                DTGridDataOldRow["OID"] = gvT_SALES_DTL.DataKeys[i].Values["OID"];
                                DTGridDataOldRow["PROD_WGPG"] = gvT_SALES_DTL.DataKeys[i].Values["PROD_WGPG"];
                                DTGridDataOldRow["PROD_SUBCATEGORY"] = gvT_SALES_DTL.DataKeys[i].Values["PROD_SUBCATEGORY"];
                                DTGridDataOldRow["DescriptionID"] = gvT_SALES_DTL.DataKeys[i].Values["DescriptionID"];
                                DTGridDataOldRow["StoreID"] = gvT_SALES_DTL.DataKeys[i].Values["StoreID"];
                                DTGridDataOldRow["Barcode"] = gvT_SALES_DTL.DataKeys[i].Values["Barcode"];

                                if (((CheckBox)gvT_SALES_DTL.Rows[i].Cells[2].FindControl("chkGift")).Checked)
                                {
                                    DTGridDataOldRow["GiftStatus"] = "true";//gift
                                }
                                else
                                {
                                    DTGridDataOldRow["GiftStatus"] = "false";//gift                                
                                }
                                string strMRP = string.IsNullOrEmpty(((TextBox)gvT_SALES_DTL.Rows[i].Cells[3].FindControl("txtSalePrice")).Text)
                                    ? "0" : ((TextBox)gvT_SALES_DTL.Rows[i].Cells[3].FindControl("txtSalePrice")).Text;
                                string strStock = string.IsNullOrEmpty(((Label)gvT_SALES_DTL.Rows[i].Cells[4].FindControl("lblStock")).Text)
                                    ? "0" : ((Label)gvT_SALES_DTL.Rows[i].Cells[4].FindControl("lblStock")).Text;
                                string strQty = string.IsNullOrEmpty(((TextBox)gvT_SALES_DTL.Rows[i].Cells[5].FindControl("txtQty")).Text)
                                    ? "0" : ((TextBox)gvT_SALES_DTL.Rows[i].Cells[5].FindControl("txtQty")).Text;
                                string strRefAmount = string.IsNullOrEmpty(((TextBox)gvT_SALES_DTL.Rows[i].Cells[7].FindControl("txtRefAmount")).Text)
                                    ? "0" : ((TextBox)gvT_SALES_DTL.Rows[i].Cells[7].FindControl("txtRefAmount")).Text;
                                string strNetAmount = string.IsNullOrEmpty(((Label)gvT_SALES_DTL.Rows[i].Cells[8].FindControl("lblTotalPrice")).Text)
                                    ? "0" : ((Label)gvT_SALES_DTL.Rows[i].Cells[8].FindControl("lblTotalPrice")).Text;

                                double MRP = 0;
                                double Stock = 0;
                                double Qty = 0;
                                double RefAmount = 0;
                                double NetAmount = 0;

                                if (double.TryParse(strMRP, out MRP) && double.TryParse(strStock, out Stock)
                                    && double.TryParse(strQty, out Qty) && double.TryParse(strRefAmount, out RefAmount)
                                    && double.TryParse(strNetAmount, out NetAmount)
                                    )
                                {
                                    DTGridDataOldRow["SalePrice"] = MRP;
                                    DTGridDataOldRow["Stock"] = Stock;
                                    DTGridDataOldRow["Qty"] = Qty;
                                    DTGridDataOldRow["TotalPrice"] = NetAmount;
                                    DTGridDataOldRow["RefAmount"] = RefAmount;

                                }

                                DTGridDataOldRow["WGPG_NAME"] = gvT_SALES_DTL.DataKeys[i].Values["WGPG_NAME"];
                                DTGridDataOldRow["SubCategoryName"] = gvT_SALES_DTL.DataKeys[i].Values["SubCategoryName"];
                                DTGridDataOldRow["Description"] = gvT_SALES_DTL.DataKeys[i].Values["Description"];
                                DTGridDataOldRow["Flag"] = gvT_SALES_DTL.DataKeys[i].Values["Flag"];
                                #endregion

                                DTGridData.Rows.Add(DTGridDataOldRow);
                            }

                            //
                            LoadGrid(DTGridData);

                            //
                            ToCalltxtSalePice_change(0);

                            //LoadData();

                            //
                            txtBarcode.Text = string.Empty;
                        }
                        else
                        {
                            //IsDescriptionIDExisted == "yes"  rowIndexExistDescriptionID = i;
                            if (rowIndexExistDescriptionID >= 0)
                            {
                                int n = rowIndexExistDescriptionID;
                                string strMRP = string.IsNullOrEmpty(((TextBox)gvT_SALES_DTL.Rows[n].Cells[3].FindControl("txtSalePrice")).Text)
                                        ? "0" : ((TextBox)gvT_SALES_DTL.Rows[n].Cells[3].FindControl("txtSalePrice")).Text;
                                string strStock = string.IsNullOrEmpty(((Label)gvT_SALES_DTL.Rows[n].Cells[4].FindControl("lblStock")).Text)
                                    ? "0" : ((Label)gvT_SALES_DTL.Rows[n].Cells[4].FindControl("lblStock")).Text;
                                string strQty = string.IsNullOrEmpty(((TextBox)gvT_SALES_DTL.Rows[n].Cells[5].FindControl("txtQty")).Text)
                                    ? "0" : ((TextBox)gvT_SALES_DTL.Rows[n].Cells[5].FindControl("txtQty")).Text;
                                string strRefAmount = string.IsNullOrEmpty(((TextBox)gvT_SALES_DTL.Rows[n].Cells[7].FindControl("txtRefAmount")).Text)
                                    ? "0" : ((TextBox)gvT_SALES_DTL.Rows[n].Cells[7].FindControl("txtRefAmount")).Text;
                                string strNetAmount = string.IsNullOrEmpty(((Label)gvT_SALES_DTL.Rows[n].Cells[8].FindControl("lblTotalPrice")).Text)
                                    ? "0" : ((Label)gvT_SALES_DTL.Rows[n].Cells[8].FindControl("lblTotalPrice")).Text;

                                double MRP = 0;
                                double Stock = 0;
                                double Qty = 0;
                                double RefAmount = 0;
                                double NetAmount = 0;

                                if (double.TryParse(strMRP, out MRP) && double.TryParse(strStock, out Stock)
                                    && double.TryParse(strQty, out Qty) && double.TryParse(strRefAmount, out RefAmount)
                                    && double.TryParse(strNetAmount, out NetAmount)
                                    )
                                {
                                    TextBox txtMRP = (TextBox)gvT_SALES_DTL.Rows[n].Cells[3].FindControl("txtSalePrice");
                                    Label lblStock = (Label)gvT_SALES_DTL.Rows[n].Cells[4].FindControl("lblStock");
                                    TextBox txtQty = (TextBox)gvT_SALES_DTL.Rows[n].Cells[5].FindControl("txtQty");
                                    TextBox txtRefAmount = (TextBox)gvT_SALES_DTL.Rows[n].Cells[7].FindControl("txtRefAmount");
                                    Label lblNetAmount = (Label)gvT_SALES_DTL.Rows[n].Cells[8].FindControl("lblTotalPrice");

                                    txtQty.Text = (Qty + 1).ToString();
                                    lblNetAmount.Text = ((MRP * (Qty + 1)) - RefAmount).ToString();

                                    //loadData
                                    LoadData();

                                    //
                                    ToCalltxtSalePice_change(rowIndexExistDescriptionID);
                                }
                            }
                        }
                    }
                    else
                    {
                        txtDescription.Text = string.Empty;
                        txtDescription.Focus();
                        return;
                    }

                    #region MyRegion
                    //txtdiscount.Text = "0";

                    //Qty_TextChanged(sender, e);
                    //txtdiscount_TextChanged(sender, e);
                    //T_PROD entity = new T_PROD();
                    //entity.PROD_WGPG = ddlSearchProductCategory.SelectedItem.Value.ToString();
                    //entity.PROD_SUBCATEGORY = ddlSearchSubCategory.SelectedItem.Value.ToString();
                    //entity.PROD_DES = id; //ddlSearchDescription.SelectedItem.Value.ToString();
                    //if (entity.PROD_DES != string.Empty)
                    //{
                    //    string StoreID = Session["StoreID"].ToString();

                    //    DataTable dt = BILL.SPP_GetProductForSale(entity);
                    //    if (dt.Rows.Count > 0)
                    //    {
                    //        //BindTemporaryItemsGrid(dt);

                    //        //
                    //        txtDescription.Text = string.Empty;
                    //    }
                    //} 
                    #endregion
                }
                //}
            }

        }

        //sadiq  101 mrp open
        protected void txtSalePrice_TextChanged(object sender, EventArgs e)
        {
            try
            {

                //
                int n = ((sender as TextBox).NamingContainer as GridViewRow).RowIndex;

                //
                string strMRP = string.IsNullOrEmpty(((TextBox)gvT_SALES_DTL.Rows[n].Cells[3].FindControl("txtSalePrice")).Text)
                        ? "0" : ((TextBox)gvT_SALES_DTL.Rows[n].Cells[3].FindControl("txtSalePrice")).Text;
                string strStock = string.IsNullOrEmpty(((Label)gvT_SALES_DTL.Rows[n].Cells[4].FindControl("lblStock")).Text)
                    ? "0" : ((Label)gvT_SALES_DTL.Rows[n].Cells[4].FindControl("lblStock")).Text;
                string strQty = string.IsNullOrEmpty(((TextBox)gvT_SALES_DTL.Rows[n].Cells[5].FindControl("txtQty")).Text)
                    ? "0" : ((TextBox)gvT_SALES_DTL.Rows[n].Cells[5].FindControl("txtQty")).Text;
                string strRefAmount = string.IsNullOrEmpty(((TextBox)gvT_SALES_DTL.Rows[n].Cells[7].FindControl("txtRefAmount")).Text)
                    ? "0" : ((TextBox)gvT_SALES_DTL.Rows[n].Cells[7].FindControl("txtRefAmount")).Text;
                string strNetAmount = string.IsNullOrEmpty(((Label)gvT_SALES_DTL.Rows[n].Cells[8].FindControl("lblTotalPrice")).Text)
                    ? "0" : ((Label)gvT_SALES_DTL.Rows[n].Cells[8].FindControl("lblTotalPrice")).Text;

                double MRP = 0;
                double Stock = 0;
                double Qty = 0;
                double RefAmount = 0;
                double NetAmount = 0;

                if (double.TryParse(strMRP, out MRP) && double.TryParse(strStock, out Stock)
                    && double.TryParse(strQty, out Qty) && double.TryParse(strRefAmount, out RefAmount)
                    && double.TryParse(strNetAmount, out NetAmount)
                    )
                {
                    //check the existing balance
                    //Int64 oldQty = Convert.ToInt64(gvT_SALES_DTL.DataKeys[n].Values["DescriptionID"]);

                    Int64 rPROD_WGPG = Convert.ToInt64(gvT_SALES_DTL.DataKeys[n].Values["PROD_WGPG"]);
                    Int64 rPROD_SUBCATEGORY = Convert.ToInt64(gvT_SALES_DTL.DataKeys[n].Values["PROD_SUBCATEGORY"]);
                    Int64 rDescriptionID = Convert.ToInt64(gvT_SALES_DTL.DataKeys[n].Values["DescriptionID"]);
                    Int64 rStock = Convert.ToInt64(gvT_SALES_DTL.DataKeys[n].Values["Stock"]);

                    Int64 newQuantity = 0;
                    for (int i = 0; i < gvT_SALES_DTL.Rows.Count; i++)
                    {
                        Int64 PROD_WGPG = Convert.ToInt64(gvT_SALES_DTL.DataKeys[i].Values["PROD_WGPG"]);
                        Int64 PROD_SUBCATEGORY = Convert.ToInt64(gvT_SALES_DTL.DataKeys[i].Values["PROD_SUBCATEGORY"]);
                        Int64 DescriptionID = Convert.ToInt64(gvT_SALES_DTL.DataKeys[i].Values["DescriptionID"]);

                        if (rPROD_WGPG == PROD_WGPG && rPROD_SUBCATEGORY == PROD_SUBCATEGORY && rDescriptionID == DescriptionID)
                        {
                            newQuantity = newQuantity + Convert.ToInt64(((TextBox)gvT_SALES_DTL.Rows[i].Cells[5].FindControl("txtQty")).Text);
                        }
                    }

                    if (rStock >= newQuantity)
                    {
                        //
                        TextBox txtMRP = (TextBox)gvT_SALES_DTL.Rows[n].Cells[3].FindControl("txtSalePrice");
                        Label lblStock = (Label)gvT_SALES_DTL.Rows[n].Cells[4].FindControl("lblStock");
                        TextBox txtQty = (TextBox)gvT_SALES_DTL.Rows[n].Cells[5].FindControl("txtQty");
                        TextBox txtRefAmount = (TextBox)gvT_SALES_DTL.Rows[n].Cells[7].FindControl("txtRefAmount");
                        Label lblNetAmount = (Label)gvT_SALES_DTL.Rows[n].Cells[8].FindControl("lblTotalPrice");

                        txtQty.Text = (Qty).ToString();
                        lblNetAmount.Text = ((MRP * Qty) - RefAmount).ToString();


                    }
                    else
                    {
                        //Sorry, Not Enough Stock!
                        if (!string.IsNullOrEmpty(((TextBox)gvT_SALES_DTL.Rows[n].Cells[6].FindControl("lblbarcode")).Text))
                        {
                            TextBox txtMRP = (TextBox)gvT_SALES_DTL.Rows[n].Cells[3].FindControl("txtSalePrice");
                            Label lblStock = (Label)gvT_SALES_DTL.Rows[n].Cells[4].FindControl("lblStock");
                            TextBox txtQty = (TextBox)gvT_SALES_DTL.Rows[n].Cells[5].FindControl("txtQty");
                            TextBox txtRefAmount = (TextBox)gvT_SALES_DTL.Rows[n].Cells[7].FindControl("txtRefAmount");
                            Label lblNetAmount = (Label)gvT_SALES_DTL.Rows[n].Cells[8].FindControl("lblTotalPrice");

                            txtQty.Text = (Qty).ToString();
                            lblNetAmount.Text = ((MRP * Qty) - RefAmount).ToString();
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "RunCode", "javascript:alert('Sorry, Not Enough Stock!');", true);

                            //invalid stock
                            TextBox txtMRP = (TextBox)gvT_SALES_DTL.Rows[n].Cells[3].FindControl("txtSalePrice");
                            Label lblStock = (Label)gvT_SALES_DTL.Rows[n].Cells[4].FindControl("lblStock");
                            TextBox txtQty = (TextBox)gvT_SALES_DTL.Rows[n].Cells[5].FindControl("txtQty");
                            TextBox txtRefAmount = (TextBox)gvT_SALES_DTL.Rows[n].Cells[7].FindControl("txtRefAmount");
                            Label lblNetAmount = (Label)gvT_SALES_DTL.Rows[n].Cells[8].FindControl("lblTotalPrice");

                            txtQty.Text = "0";
                            txtRefAmount.Text = "0";
                            lblNetAmount.Text = "0";// ((MRP * 0) - 0).ToString();
                        }
                    }
                    //loadData
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                //error msg
            }
            finally { }
        }//
        public void ToCalltxtSalePice_change(int rowIndex)
        {
            try
            {
                int n = rowIndex;
                //
                string strMRP = string.IsNullOrEmpty(((TextBox)gvT_SALES_DTL.Rows[n].Cells[3].FindControl("txtSalePrice")).Text)
                        ? "0" : ((TextBox)gvT_SALES_DTL.Rows[n].Cells[3].FindControl("txtSalePrice")).Text;
                string strStock = string.IsNullOrEmpty(((Label)gvT_SALES_DTL.Rows[n].Cells[4].FindControl("lblStock")).Text)
                    ? "0" : ((Label)gvT_SALES_DTL.Rows[n].Cells[4].FindControl("lblStock")).Text;
                string strQty = string.IsNullOrEmpty(((TextBox)gvT_SALES_DTL.Rows[n].Cells[5].FindControl("txtQty")).Text)
                    ? "0" : ((TextBox)gvT_SALES_DTL.Rows[n].Cells[5].FindControl("txtQty")).Text;
                string strRefAmount = string.IsNullOrEmpty(((TextBox)gvT_SALES_DTL.Rows[n].Cells[7].FindControl("txtRefAmount")).Text)
                    ? "0" : ((TextBox)gvT_SALES_DTL.Rows[n].Cells[7].FindControl("txtRefAmount")).Text;
                string strNetAmount = string.IsNullOrEmpty(((Label)gvT_SALES_DTL.Rows[n].Cells[8].FindControl("lblTotalPrice")).Text)
                    ? "0" : ((Label)gvT_SALES_DTL.Rows[n].Cells[8].FindControl("lblTotalPrice")).Text;

                double MRP = 0;
                double Stock = 0;
                double Qty = 0;
                double RefAmount = 0;
                double NetAmount = 0;

                if (double.TryParse(strMRP, out MRP) && double.TryParse(strStock, out Stock)
                    && double.TryParse(strQty, out Qty) && double.TryParse(strRefAmount, out RefAmount)
                    && double.TryParse(strNetAmount, out NetAmount)
                    )
                {
                    //check the existing balance
                    //Int64 oldQty = Convert.ToInt64(gvT_SALES_DTL.DataKeys[n].Values["DescriptionID"]);

                    Int64 rPROD_WGPG = Convert.ToInt64(gvT_SALES_DTL.DataKeys[n].Values["PROD_WGPG"]);
                    Int64 rPROD_SUBCATEGORY = Convert.ToInt64(gvT_SALES_DTL.DataKeys[n].Values["PROD_SUBCATEGORY"]);
                    Int64 rDescriptionID = Convert.ToInt64(gvT_SALES_DTL.DataKeys[n].Values["DescriptionID"]);
                    Int64 rStock = Convert.ToInt64(gvT_SALES_DTL.DataKeys[n].Values["Stock"]);
                    string Flag = gvT_SALES_DTL.DataKeys[n].Values["Flag"].ToString();

                    Int64 newQuantity = 0;
                    for (int i = 0; i < gvT_SALES_DTL.Rows.Count; i++)
                    {
                        Int64 PROD_WGPG = Convert.ToInt64(gvT_SALES_DTL.DataKeys[i].Values["PROD_WGPG"]);
                        Int64 PROD_SUBCATEGORY = Convert.ToInt64(gvT_SALES_DTL.DataKeys[i].Values["PROD_SUBCATEGORY"]);
                        Int64 DescriptionID = Convert.ToInt64(gvT_SALES_DTL.DataKeys[i].Values["DescriptionID"]);

                        if (rPROD_WGPG == PROD_WGPG && rPROD_SUBCATEGORY == PROD_SUBCATEGORY && rDescriptionID == DescriptionID && Flag != "Barcode")
                        {
                            newQuantity = newQuantity + Convert.ToInt64(((TextBox)gvT_SALES_DTL.Rows[i].Cells[5].FindControl("txtQty")).Text);
                        }
                    }

                    if (rStock >= newQuantity)
                    {
                        //valid qty  : less then stock
                        TextBox txtMRP = (TextBox)gvT_SALES_DTL.Rows[n].Cells[3].FindControl("txtSalePrice");
                        Label lblStock = (Label)gvT_SALES_DTL.Rows[n].Cells[4].FindControl("lblStock");
                        TextBox txtQty = (TextBox)gvT_SALES_DTL.Rows[n].Cells[5].FindControl("txtQty");
                        TextBox txtRefAmount = (TextBox)gvT_SALES_DTL.Rows[n].Cells[7].FindControl("txtRefAmount");
                        Label lblNetAmount = (Label)gvT_SALES_DTL.Rows[n].Cells[8].FindControl("lblTotalPrice");

                        txtQty.Text = (Qty).ToString();
                        lblNetAmount.Text = ((MRP * Qty) - RefAmount).ToString();


                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(((TextBox)gvT_SALES_DTL.Rows[n].Cells[6].FindControl("lblbarcode")).Text))
                        {
                            TextBox txtMRP = (TextBox)gvT_SALES_DTL.Rows[n].Cells[3].FindControl("txtSalePrice");
                            Label lblStock = (Label)gvT_SALES_DTL.Rows[n].Cells[4].FindControl("lblStock");
                            TextBox txtQty = (TextBox)gvT_SALES_DTL.Rows[n].Cells[5].FindControl("txtQty");
                            TextBox txtRefAmount = (TextBox)gvT_SALES_DTL.Rows[n].Cells[7].FindControl("txtRefAmount");
                            Label lblNetAmount = (Label)gvT_SALES_DTL.Rows[n].Cells[8].FindControl("lblTotalPrice");

                            txtQty.Text = (Qty).ToString();
                            lblNetAmount.Text = ((MRP * Qty) - RefAmount).ToString();
                        }
                        else
                        {
                            //Sorry, Not Enough Stock!
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "RunCode", "javascript:alert('Sorry, Not Enough Stock!');", true);

                            //invalid stock
                            TextBox txtMRP = (TextBox)gvT_SALES_DTL.Rows[n].Cells[3].FindControl("txtSalePrice");
                            Label lblStock = (Label)gvT_SALES_DTL.Rows[n].Cells[4].FindControl("lblStock");
                            TextBox txtQty = (TextBox)gvT_SALES_DTL.Rows[n].Cells[5].FindControl("txtQty");
                            TextBox txtRefAmount = (TextBox)gvT_SALES_DTL.Rows[n].Cells[7].FindControl("txtRefAmount");
                            Label lblNetAmount = (Label)gvT_SALES_DTL.Rows[n].Cells[8].FindControl("lblTotalPrice");

                            txtQty.Text = "0";
                            txtRefAmount.Text = "0";
                            lblNetAmount.Text = "0";// ((MRP * 0) - 0).ToString();
                        }
                    }
                    //loadData
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                //error msg
            }
            finally { }
        }
        protected void txtBarcode_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //get barcode
                string barcode = txtBarcode.Text;

                //check in database
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
                string cmdstr = string.Format(@"
select s.OID, p.PROD_WGPG, p.PROD_SUBCATEGORY, p.PROD_DES as DescriptionID, p.Branch as StoreID
, p.Barcode, s.SalePrice,1 Stock, 1 Qty,(1*p.SalePrice) TotalPrice,0 as RefAmount
,c.WGPG_NAME,sc.SubCategoryName,d.Description,Flag='Barcode', p.SaleStatus
from T_PROD p 
inner join Description d on d.OID=p.PROD_DES
inner join SubCategory sc on sc.OID=d.SubCategoryID
inner join T_WGPG c on c.OID=sc.CategoryID
inner join StoreMasterStock s 
on s.PROD_DES=p.PROD_DES and s.PROD_SUBCATEGORY=p.PROD_SUBCATEGORY and s.PROD_WGPG=p.PROD_WGPG and s.Branch=p.Branch

where SaleStatus=0 and s.Quantity >0 and (s.Quantity - s.SaleQuantity) >0
and p.Branch='{0}' and p.Barcode ='{1}'

", Session["StoreID"], barcode);

                DataTable dtProductToAdd = GetDataTableByQuery(cmdstr);
                if (dtProductToAdd.Rows.Count > 0)
                {
                    #region check existance in gridview

                    string IsBarcodeExisted = "no";
                    string newBarcode = dtProductToAdd.Rows[0]["Barcode"].ToString();
                    if (gvT_SALES_DTL.Rows.Count > 0)
                    {
                        for (int i = 0; i < gvT_SALES_DTL.Rows.Count; i++)
                        {
                            string existedBarcode = gvT_SALES_DTL.DataKeys[i].Values["Barcode"].ToString();
                            string existedFlag = gvT_SALES_DTL.DataKeys[i].Values["Flag"].ToString();
                            if (newBarcode == existedBarcode)  // && existedFlag == "Barcode"
                            {
                                IsBarcodeExisted = "yes";
                                break;
                            }
                        }
                    }

                    #endregion


                    //
                    if (IsBarcodeExisted == "no")
                    {
                        // //get gridview data

                        //dt
                        DataTable DTGridData = new DataTable();
                        DTGridData.TableName = "gridTable";

                        #region //column
                        DTGridData.Columns.Add(new DataColumn("OID", typeof(Int64)));
                        DTGridData.Columns.Add(new DataColumn("PROD_WGPG", typeof(Int64)));
                        DTGridData.Columns.Add(new DataColumn("PROD_SUBCATEGORY", typeof(Int64)));
                        DTGridData.Columns.Add(new DataColumn("DescriptionID", typeof(Int64)));
                        DTGridData.Columns.Add(new DataColumn("StoreID", typeof(string)));//

                        DTGridData.Columns.Add(new DataColumn("Barcode", typeof(string)));

                        DTGridData.Columns.Add(new DataColumn("SalePrice", typeof(double)));
                        DTGridData.Columns.Add(new DataColumn("Stock", typeof(double)));
                        DTGridData.Columns.Add(new DataColumn("Qty", typeof(double)));
                        DTGridData.Columns.Add(new DataColumn("TotalPrice", typeof(double)));
                        DTGridData.Columns.Add(new DataColumn("RefAmount", typeof(double)));//

                        DTGridData.Columns.Add("WGPG_NAME");
                        DTGridData.Columns.Add("SubCategoryName");
                        DTGridData.Columns.Add("Description");
                        DTGridData.Columns.Add("Flag");
                        DTGridData.Columns.Add("GiftStatus");//gift 
                        #endregion

                        #region get the new data row for barcode
                        DataRow DTGridDataNewRow = DTGridData.NewRow();
                        //
                        DTGridDataNewRow["OID"] = dtProductToAdd.Rows[0]["OID"].ToString();
                        DTGridDataNewRow["PROD_WGPG"] = dtProductToAdd.Rows[0]["PROD_WGPG"].ToString();
                        DTGridDataNewRow["PROD_SUBCATEGORY"] = dtProductToAdd.Rows[0]["PROD_SUBCATEGORY"].ToString();
                        DTGridDataNewRow["DescriptionID"] = dtProductToAdd.Rows[0]["DescriptionID"].ToString();
                        DTGridDataNewRow["StoreID"] = dtProductToAdd.Rows[0]["StoreID"].ToString();
                        DTGridDataNewRow["Barcode"] = dtProductToAdd.Rows[0]["Barcode"].ToString();




                        string strMRPn = string.IsNullOrEmpty(dtProductToAdd.Rows[0]["SalePrice"].ToString())
                                ? "0" : dtProductToAdd.Rows[0]["SalePrice"].ToString();
                        string strStockn = string.IsNullOrEmpty(dtProductToAdd.Rows[0]["Stock"].ToString())
                            ? "0" : dtProductToAdd.Rows[0]["Stock"].ToString();
                        //string strQtyn = string.IsNullOrEmpty(dtProductToAdd.Rows[0]["Qty"].ToString())
                        //    ? "0" : dtProductToAdd.Rows[0]["Qty"].ToString();
                        //string strRefAmount = string.IsNullOrEmpty(dtProductToAdd.Rows[0]["RefAmount"].ToString())
                        //    ? "0" : dtProductToAdd.Rows[0]["RefAmount"].ToString();


                        double MRPn = 0; double Stockn = 0;

                        if (double.TryParse(strMRPn, out MRPn) && double.TryParse(strStockn, out Stockn))
                        {
                            DTGridDataNewRow["SalePrice"] = MRPn;
                            DTGridDataNewRow["Stock"] = Stockn;
                            DTGridDataNewRow["Qty"] = 1;
                            DTGridDataNewRow["RefAmount"] = 0;
                            DTGridDataNewRow["TotalPrice"] = MRPn;

                        }


                        DTGridDataNewRow["WGPG_NAME"] = dtProductToAdd.Rows[0]["WGPG_NAME"].ToString();
                        DTGridDataNewRow["SubCategoryName"] = dtProductToAdd.Rows[0]["SubCategoryName"].ToString();
                        DTGridDataNewRow["Description"] = dtProductToAdd.Rows[0]["Description"].ToString();
                        DTGridDataNewRow["Flag"] = dtProductToAdd.Rows[0]["Flag"].ToString();
                        DTGridDataNewRow["GiftStatus"] = "false";//gift

                        DTGridData.Rows.Add(DTGridDataNewRow);
                        #endregion

                        for (int i = 0; i < gvT_SALES_DTL.Rows.Count; i++)
                        {
                            DataRow DTGridDataOldRow = DTGridData.NewRow();
                            //
                            #region MyRegion
                            DTGridDataOldRow["OID"] = gvT_SALES_DTL.DataKeys[i].Values["OID"];
                            DTGridDataOldRow["PROD_WGPG"] = gvT_SALES_DTL.DataKeys[i].Values["PROD_WGPG"];
                            DTGridDataOldRow["PROD_SUBCATEGORY"] = gvT_SALES_DTL.DataKeys[i].Values["PROD_SUBCATEGORY"];
                            DTGridDataOldRow["DescriptionID"] = gvT_SALES_DTL.DataKeys[i].Values["DescriptionID"];
                            DTGridDataOldRow["StoreID"] = gvT_SALES_DTL.DataKeys[i].Values["StoreID"];
                            DTGridDataOldRow["Barcode"] = gvT_SALES_DTL.DataKeys[i].Values["Barcode"];

                            if (((CheckBox)gvT_SALES_DTL.Rows[i].Cells[2].FindControl("chkGift")).Checked)
                            {
                                DTGridDataOldRow["GiftStatus"] = "true";//gift
                            }
                            else
                            {
                                DTGridDataOldRow["GiftStatus"] = "false";//gift                                
                            }

                            string strMRP = string.IsNullOrEmpty(((TextBox)gvT_SALES_DTL.Rows[i].Cells[3].FindControl("txtSalePrice")).Text)
                                ? "0" : ((TextBox)gvT_SALES_DTL.Rows[i].Cells[3].FindControl("txtSalePrice")).Text;
                            string strStock = string.IsNullOrEmpty(((Label)gvT_SALES_DTL.Rows[i].Cells[4].FindControl("lblStock")).Text)
                                ? "0" : ((Label)gvT_SALES_DTL.Rows[i].Cells[4].FindControl("lblStock")).Text;
                            string strQty = string.IsNullOrEmpty(((TextBox)gvT_SALES_DTL.Rows[i].Cells[5].FindControl("txtQty")).Text)
                                ? "0" : ((TextBox)gvT_SALES_DTL.Rows[i].Cells[5].FindControl("txtQty")).Text;
                            string strRefAmount = string.IsNullOrEmpty(((TextBox)gvT_SALES_DTL.Rows[i].Cells[7].FindControl("txtRefAmount")).Text)
                                ? "0" : ((TextBox)gvT_SALES_DTL.Rows[i].Cells[7].FindControl("txtRefAmount")).Text;
                            string strNetAmount = string.IsNullOrEmpty(((Label)gvT_SALES_DTL.Rows[i].Cells[8].FindControl("lblTotalPrice")).Text)
                                ? "0" : ((Label)gvT_SALES_DTL.Rows[i].Cells[8].FindControl("lblTotalPrice")).Text;

                            double MRP = 0;
                            double Stock = 0;
                            double Qty = 0;
                            double RefAmount = 0;
                            double NetAmount = 0;

                            if (double.TryParse(strMRP, out MRP) && double.TryParse(strStock, out Stock)
                                && double.TryParse(strQty, out Qty) && double.TryParse(strRefAmount, out RefAmount)
                                && double.TryParse(strNetAmount, out NetAmount)
                                )
                            {
                                DTGridDataOldRow["SalePrice"] = MRP;
                                DTGridDataOldRow["Stock"] = Stock;
                                DTGridDataOldRow["Qty"] = Qty;
                                DTGridDataOldRow["TotalPrice"] = NetAmount;
                                DTGridDataOldRow["RefAmount"] = RefAmount;

                            }

                            DTGridDataOldRow["WGPG_NAME"] = gvT_SALES_DTL.DataKeys[i].Values["WGPG_NAME"];
                            DTGridDataOldRow["SubCategoryName"] = gvT_SALES_DTL.DataKeys[i].Values["SubCategoryName"];
                            DTGridDataOldRow["Description"] = gvT_SALES_DTL.DataKeys[i].Values["Description"];
                            DTGridDataOldRow["Flag"] = gvT_SALES_DTL.DataKeys[i].Values["Flag"];
                            #endregion

                            DTGridData.Rows.Add(DTGridDataOldRow);
                        }

                        //
                        LoadGrid(DTGridData);
                        //LoadNetAmount();
                        //
                        ToCalltxtSalePice_change(0);

                        //LoadData();

                        //
                        txtBarcode.Text = string.Empty;
                        txtBarcode.Focus();
                    }
                    else
                    {
                        //show message : already added in grid
                        txtBarcode.Text = string.Empty;
                        txtBarcode.Focus();
                        //return;
                    }

                }
                else
                {
                    txtBarcode.Text = string.Empty;
                    txtBarcode.Focus();
                    //return;

                }
                
            }
            catch (Exception ex) { }
            finally { txtBarcode.Text = string.Empty; txtBarcode.Focus(); }

            #region
            ////
            //using (con)
            //{
            //    using (SqlCommand cmd = new SqlCommand(cmdstr, con))
            //    {
            //        SqlDataAdapter sda = new SqlDataAdapter(cmd);             //command.ExecuteNonQuery();
            //        DataTable dtCheck = new DataTable();
            //        sda.Fill(dtCheck);

            //        //check existance in gridview
            //        string IsBarcodeExisted = "no";
            //        string newBarcode = dtCheck.Rows[0]["Barcode"].ToString();
            //        if (gvT_SALES_DTL.Rows.Count > 0)
            //        {
            //            for (int i = 0; i < gvT_SALES_DTL.Rows.Count; i++)
            //            {
            //                string existedBarcode = ((TextBox)gvT_SALES_DTL.Rows[i].FindControl("lblbarcode")).Text;
            //                if (newBarcode == existedBarcode)
            //                {
            //                    IsBarcodeExisted = "yes";
            //                    break;
            //                }
            //            }
            //        }




            //        //
            //        if (dtCheck.Rows.Count > 0 && IsBarcodeExisted == "no")
            //        {
            //            //valid
            //            //load grid
            //            if (string.IsNullOrEmpty(Session["StuffID"] as string))
            //            {
            //                Response.Redirect("~/Pages/ValidateSales.aspx");
            //            }
            //            else
            //            {
            //                txtdiscount.Text = "0";

            //                Qty_TextChanged(sender, e);
            //                txtdiscount_TextChanged(sender, e);
            //                T_PROD entity = new T_PROD();
            //                entity.PROD_WGPG = dtCheck.Rows[0]["PROD_WGPG"].ToString(); //ddlSearchProductCategory.SelectedItem.Value.ToString();
            //                entity.PROD_SUBCATEGORY = dtCheck.Rows[0]["PROD_SUBCATEGORY"].ToString(); //ddlSearchSubCategory.SelectedItem.Value.ToString();
            //                entity.PROD_DES = dtCheck.Rows[0]["PROD_DES"].ToString(); //ddlSearchDescription.SelectedItem.Value.ToString();
            //                entity.Barcode = dtCheck.Rows[0]["Barcode"].ToString();

            //                if (entity.PROD_DES != string.Empty)
            //                {
            //                    string StoreID = Session["StoreID"].ToString();

            //                    DataTable dt = BILL.SPP_GetProductForSaleByBarcode(entity);
            //                    if (dt.Rows.Count > 0)
            //                    {
            //                        BindTemporaryItemsGrid(dt);

            //                    }
            //                }
            //            }
            //        }
            //        else
            //        {
            //            txtBarcode.Text = string.Empty;
            //            return;
            //        }
            //    }
            //}
            #endregion
        }//

        private void BindTemporaryItemsGridV0(DataTable dt)
        {
            List<AddedItemDetailsSales> listAddedItemDetails = new List<AddedItemDetailsSales>();
            AddedItemDetailsSales objAddedItemDetails;
            int sameid = 0;
            decimal subTotal = 0;
            int totalqty = 0;
            if (gvT_SALES_DTL.Rows.Count > 0)
            {
                for (int i = 0; i < gvT_SALES_DTL.Rows.Count; i++)
                {
                    objAddedItemDetails = new AddedItemDetailsSales();
                    string des = dt.Rows[0]["DescriptionID"].ToString();
                    string newdes = ((Label)gvT_SALES_DTL.Rows[i].FindControl("lblDescriptionID")).Text;
                    if (des == newdes)
                    {
                        objAddedItemDetails.OID = ((Label)gvT_SALES_DTL.Rows[i].FindControl("lblOID")).Text;
                        objAddedItemDetails.PROD_WGPG = ((Label)gvT_SALES_DTL.Rows[i].FindControl("lblPROD_WGPG")).Text;
                        objAddedItemDetails.Barcode = ((TextBox)gvT_SALES_DTL.Rows[i].FindControl("lblbarcode")).Text;

                        objAddedItemDetails.WGPG_NAME = ((Label)gvT_SALES_DTL.Rows[i].FindControl("lblWGPG_NAME")).Text;
                        objAddedItemDetails.SubCategoryID = ((Label)gvT_SALES_DTL.Rows[i].FindControl("lblSubCategoryID")).Text;
                        objAddedItemDetails.SubCategoryName = ((Label)gvT_SALES_DTL.Rows[i].FindControl("lblSubCategoryName")).Text;
                        objAddedItemDetails.StoreID = ((Label)gvT_SALES_DTL.Rows[i].FindControl("lblStoreID")).Text;
                        objAddedItemDetails.DescriptionID = ((Label)gvT_SALES_DTL.Rows[i].FindControl("lblDescriptionID")).Text;
                        objAddedItemDetails.Description = ((Label)gvT_SALES_DTL.Rows[i].FindControl("lblDescription")).Text;


                        objAddedItemDetails.Stock = Convert.ToInt32(((Label)gvT_SALES_DTL.Rows[i].FindControl("lblStock")).Text);
                        int Qty = Convert.ToInt32(((TextBox)gvT_SALES_DTL.Rows[i].FindControl("Qty")).Text);
                        objAddedItemDetails.Qty = Qty + 1;

                        if (Convert.ToInt32(((Label)gvT_SALES_DTL.Rows[i].FindControl("lblStock")).Text) < Convert.ToInt32(objAddedItemDetails.Qty))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "RunCode", "javascript:alert('Out Of Stock');", true);
                            objAddedItemDetails.Qty = Convert.ToInt32(objAddedItemDetails.Qty) - 1;
                        }

                        Qty = Convert.ToInt32(objAddedItemDetails.Qty);
                        objAddedItemDetails.SalePrice = Convert.ToDecimal(((Label)gvT_SALES_DTL.Rows[i].FindControl("lblSalePrice")).Text);

                        // product discount added here
                        objAddedItemDetails.RefAmount = Convert.ToInt32(((TextBox)gvT_SALES_DTL.Rows[i].FindControl("RefAmount")).Text);
                        // add discount to txtdiscount field.
                        txtdiscount.Text = (Convert.ToInt32(txtdiscount.Text) + Convert.ToInt32(objAddedItemDetails.RefAmount)).ToString();

                        Decimal Sprice = Convert.ToDecimal(objAddedItemDetails.SalePrice);
                        // find the total poduct price after discount
                        objAddedItemDetails.TotalPrice = Qty * Sprice - Convert.ToInt32(objAddedItemDetails.RefAmount);

                        subTotal = subTotal + Convert.ToDecimal(objAddedItemDetails.SalePrice * Qty);
                        listAddedItemDetails.Add(objAddedItemDetails);
                        sameid = 1;
                    }
                    else
                    {
                        objAddedItemDetails = new AddedItemDetailsSales();
                        objAddedItemDetails.OID = ((Label)gvT_SALES_DTL.Rows[i].FindControl("lblOID")).Text;
                        objAddedItemDetails.PROD_WGPG = ((Label)gvT_SALES_DTL.Rows[i].FindControl("lblPROD_WGPG")).Text;
                        objAddedItemDetails.WGPG_NAME = ((Label)gvT_SALES_DTL.Rows[i].FindControl("lblWGPG_NAME")).Text;
                        objAddedItemDetails.Barcode = ((TextBox)gvT_SALES_DTL.Rows[i].FindControl("lblbarcode")).Text;
                        objAddedItemDetails.SubCategoryID = ((Label)gvT_SALES_DTL.Rows[i].FindControl("lblSubCategoryID")).Text;
                        objAddedItemDetails.SubCategoryName = ((Label)gvT_SALES_DTL.Rows[i].FindControl("lblSubCategoryName")).Text;
                        objAddedItemDetails.StoreID = ((Label)gvT_SALES_DTL.Rows[i].FindControl("lblStoreID")).Text;
                        objAddedItemDetails.DescriptionID = ((Label)gvT_SALES_DTL.Rows[i].FindControl("lblDescriptionID")).Text;
                        objAddedItemDetails.Description = ((Label)gvT_SALES_DTL.Rows[i].FindControl("lblDescription")).Text;

                        objAddedItemDetails.Stock = Convert.ToInt32(((Label)gvT_SALES_DTL.Rows[i].FindControl("lblStock")).Text);

                        int Qty = Convert.ToInt32(((TextBox)gvT_SALES_DTL.Rows[i].FindControl("Qty")).Text);
                        objAddedItemDetails.Qty = Qty;
                        // product discount added here
                        objAddedItemDetails.RefAmount = Convert.ToInt32(((TextBox)gvT_SALES_DTL.Rows[i].FindControl("RefAmount")).Text);

                        objAddedItemDetails.SalePrice = Convert.ToDecimal(((Label)gvT_SALES_DTL.Rows[i].FindControl("lblSalePrice")).Text);
                        // add discount to txtdiscount field.
                        txtdiscount.Text = (Convert.ToInt32(txtdiscount.Text) + Convert.ToInt32(objAddedItemDetails.RefAmount)).ToString();
                        Decimal Sprice = Convert.ToDecimal(objAddedItemDetails.SalePrice);
                        objAddedItemDetails.TotalPrice = (Qty * Sprice) - Convert.ToInt32(objAddedItemDetails.RefAmount);

                        subTotal = subTotal + Convert.ToDecimal(objAddedItemDetails.SalePrice * Qty);
                        listAddedItemDetails.Add(objAddedItemDetails);
                    }
                    totalqty += Convert.ToInt32(objAddedItemDetails.Qty);
                }
            }
            if (sameid == 0)
            {
                objAddedItemDetails = new AddedItemDetailsSales();
                objAddedItemDetails.OID = dt.Rows[0]["OID"].ToString();
                objAddedItemDetails.PROD_WGPG = dt.Rows[0]["PROD_WGPG"].ToString();

                objAddedItemDetails.WGPG_NAME = dt.Rows[0]["WGPG_NAME"].ToString();
                objAddedItemDetails.SubCategoryName = dt.Rows[0]["SubCategoryName"].ToString();
                objAddedItemDetails.SubCategoryID = dt.Rows[0]["PROD_SUBCATEGORY"].ToString();
                objAddedItemDetails.Barcode = dt.Rows[0]["Barcode"].ToString();// 103
                objAddedItemDetails.StoreID = dt.Rows[0]["StoreID"].ToString();
                objAddedItemDetails.DescriptionID = dt.Rows[0]["DescriptionID"].ToString();
                objAddedItemDetails.Description = dt.Rows[0]["Description"].ToString();

                objAddedItemDetails.Stock = Convert.ToInt32(dt.Rows[0]["Stock"].ToString());
                objAddedItemDetails.Qty = 1;
                objAddedItemDetails.RefAmount = 0;
                objAddedItemDetails.SalePrice = Convert.ToDecimal(dt.Rows[0]["SalePrice"].ToString());
                objAddedItemDetails.TotalPrice = Convert.ToDecimal(dt.Rows[0]["TotalPrice"].ToString());
                subTotal = subTotal + Convert.ToDecimal(objAddedItemDetails.SalePrice * objAddedItemDetails.Qty);
                listAddedItemDetails.Add(objAddedItemDetails);
                sameid = 0;
                totalqty += Convert.ToInt32(objAddedItemDetails.Qty);
            }
            if (listAddedItemDetails.Count > 0)
            {
                ListtoDataTableConverter converter = new ListtoDataTableConverter();
                DataTable dtProperty = converter.ToDataTable(listAddedItemDetails);

                Session["dtProperty"] = dtProperty;
                txtSubTotal.Text = subTotal.ToString();
                if (txtSubTotal.Text != string.Empty & txtdiscount.Text != string.Empty)
                {
                    txtNetAmount.Text = ((Convert.ToInt32(txtSubTotal.Text) - Convert.ToInt32(txtdiscount.Text))).ToString();
                    //lblcashpaid.Text = txtNetAmount.Text;
                    txttotalquantity.Text = totalqty.ToString();
                }
                gvT_SALES_DTL.DataSource = listAddedItemDetails;
                gvT_SALES_DTL.DataBind();
                sameid = 0;
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
                        //using (DataTable dt = new DataTable())
                        //{
                        sda.Fill(dt);
                        //}
                    }
                }
            }
            return dt;
        }


        public void SaveDataCRUD(string sqlQuery)
        {
            string constr = ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(constr))
            {
                SqlCommand command = new SqlCommand(sqlQuery, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
            }
        }
        public void LoadGrid(DataTable dt)
        {
            gvT_SALES_DTL.DataSource = null;
            gvT_SALES_DTL.DataSource = dt;
            gvT_SALES_DTL.DataBind();

            //
            for (int i = 0; i < gvT_SALES_DTL.Rows.Count; i++)
            {

                if (gvT_SALES_DTL.DataKeys[i].Values["Flag"].ToString() == "Barcode")
                {
                    ((TextBox)gvT_SALES_DTL.Rows[i].Cells[5].FindControl("txtQty")).Enabled = false;
                }
                else
                {
                    ((TextBox)gvT_SALES_DTL.Rows[i].Cells[5].FindControl("txtQty")).Enabled = true;
                }
                //

                if (dt.Rows[i]["GiftStatus"].ToString() == "true")
                {
                    ((CheckBox)gvT_SALES_DTL.Rows[i].Cells[2].FindControl("chkGift")).Checked = true;
                }
                else
                {
                    ((CheckBox)gvT_SALES_DTL.Rows[i].Cells[2].FindControl("chkGift")).Checked = false;
                }
            }

            LoadNetAmount();


        }
        private void LoadNetAmount()
        {
            //
            decimal totalAmount = 0;
            decimal totalNetAmount = 0;
            decimal totalGiftAmount = 0;
            decimal totalDiscountAmount = 0;
            txtCardPaid.Text = "0";
            for (int i = 0; i < gvT_SALES_DTL.Rows.Count; i++)
            {

                CheckBox chkGift = (CheckBox)gvT_SALES_DTL.Rows[i].Cells[2].FindControl("chkGift");
                TextBox txtQty = (TextBox)gvT_SALES_DTL.Rows[i].Cells[5].FindControl("txtQty");
                TextBox txtSalePrice = (TextBox)gvT_SALES_DTL.Rows[i].Cells[3].FindControl("txtSalePrice");






                if (chkGift.Checked)
                {
                    //int rowIndex = ((sender as CheckBox).NamingContainer as GridViewRow).RowIndex;
                    //((TextBox)gvT_SALES_DTL.Rows[i].Cells[7].FindControl("txtRefAmount")).Enabled = false;
                    //((TextBox)gvT_SALES_DTL.Rows[i].Cells[7].FindControl("txtRefAmount")).Text = "0";
                    totalGiftAmount += (Convert.ToDecimal(txtQty.Text) * Convert.ToDecimal(txtSalePrice.Text));

                }
                else
                {
                    //((TextBox)gvT_SALES_DTL.Rows[i].Cells[7].FindControl("txtRefAmount")).Enabled = true;

                    //((TextBox)gvT_SALES_DTL.Rows[i].Cells[7].FindControl("txtRefAmount")).Text =
                    //    (Convert.ToDecimal(txtQty.Text) * Convert.ToDecimal(txtSalePrice.Text)).ToString();
                    //((TextBox)gvT_SALES_DTL.Rows[i].Cells[7].FindControl("txtRefAmount")).Text = "0";

                    //((Label)gvT_SALES_DTL.Rows[i].Cells[8].FindControl("lblTotalPrice")).Text =
                    //    ((Convert.ToDecimal(txtQty.Text) * Convert.ToDecimal(txtSalePrice.Text)) - Convert.ToDecimal(txtdiscount.Text)).ToString("0");
                }
                TextBox txtDiscountAmount = (TextBox)gvT_SALES_DTL.Rows[i].Cells[7].FindControl("txtRefAmount");
                Label lblTotalPrice = (Label)gvT_SALES_DTL.Rows[i].Cells[8].FindControl("lblTotalPrice");
                totalDiscountAmount += Convert.ToDecimal(txtDiscountAmount.Text);

                totalAmount += (Convert.ToDecimal(txtQty.Text) * Convert.ToDecimal(txtSalePrice.Text));
                //totalNetAmount += chkGift.Checked?0: Convert.ToDecimal(lblTotalPrice.Text);

            }

            txtdiscount.Text = totalDiscountAmount.ToString("0");
            txtGift.Text = totalGiftAmount.ToString("0");
            totalNetAmount = (totalAmount - totalDiscountAmount - totalGiftAmount);
            txtNetAmount.Text = (totalNetAmount - totalGiftAmount).ToString("0");
            txtNetAmount.Text = totalNetAmount.ToString("0");

            txtReceiveAmount.Text = "0";

            //if (ddlPaymentMode.SelectedItem.Text != "Cash & Card" )
            //{
                //if (ddlPaymentMode.SelectedItem.Text != "On Credit")
                //{
                //    lblcashpaid.Text = totalNetAmount.ToString("0");
                //}
            //}
            //if (ddlPaymentMode.SelectedItem.Text=="Cash")
            //{
                decimal cashPaid = 0;
                decimal cardPaid = 0;

                if (decimal.TryParse(lblcashpaid.Text, out cashPaid)) { }
                if (decimal.TryParse(txtCardPaid.Text, out cardPaid)) { }

                lblcashpaid.Text = cashPaid.ToString("0");
                txtCardPaid.Text = cashPaid.ToString("0");
                txtReceiveAmount.Text = (cardPaid + cashPaid).ToString("0");

                if (ddlPaymentMode.SelectedItem.Text == "Cash") 
                {
                    lblcashpaid.Text = totalNetAmount.ToString("0");
                    txtReceiveAmount.Text = cashPaid.ToString("0");
                }
                else if (ddlPaymentMode.SelectedItem.Text == "Card")
                {
                    txtCardPaid.Text = totalNetAmount.ToString("0");
                    txtReceiveAmount.Text = cardPaid.ToString("0");
                }
                else if (ddlPaymentMode.SelectedItem.Text == "Cash & Card")
                {
                    //txtCardPaid.Text = (totalNetAmount - cashPaid).ToString("0");
                }
                else if (ddlPaymentMode.SelectedItem.Text == "On Credit")
                {
                    txtCardPaid.Text = "0";
                    lblcashpaid.Text = "0";
                    txtReceiveAmount.Text = "0";
                }
                else if (ddlPaymentMode.SelectedItem.Text == "Cash & On Credit")
                {
                    lblcashpaid.Text = cashPaid.ToString("0");
                    txtReceiveAmount.Text = cashPaid.ToString("0");
                }
            //}
            //else
            //{
            //    if (ddlPaymentMode.SelectedItem.Text != "Cash & Card") 
            //    {
            //        if (ddlPaymentMode.SelectedItem.Text != "On Credit")
            //        {
            //            lblcashpaid.Text = totalNetAmount.ToString("0");
            //        }
            //    }
            //}
            //decimal cardPaidAmount = Convert.ToDecimal(txtCardPaid.Text);
            //if (cardPaidAmount > 0) { txtCardPaid.Text = (Convert.ToDecimal(txtReceiveAmount.Text) - Convert.ToDecimal(lblcashpaid.Text)).ToString("0"); }

            //txtCardPaid.Text = (totalNetAmount - totalDiscountAmount - totalGiftAmount - Convert.ToDecimal(lblcashpaid.Text)).ToString("0");
        }
        public void LoadData()
        {
            try
            {
                //loop to get total  and laod
                double subTotal = 0;
                double totalRefAmount = 0;
                double totalqty = 0;
                for (int n = 0; n < gvT_SALES_DTL.Rows.Count; n++)
                {
                    string strMRP = string.IsNullOrEmpty(((TextBox)gvT_SALES_DTL.Rows[n].Cells[3].FindControl("txtSalePrice")).Text)
                        ? "0" : ((TextBox)gvT_SALES_DTL.Rows[n].Cells[3].FindControl("txtSalePrice")).Text;
                    string strStock = string.IsNullOrEmpty(((Label)gvT_SALES_DTL.Rows[n].Cells[4].FindControl("lblStock")).Text)
                        ? "0" : ((Label)gvT_SALES_DTL.Rows[n].Cells[4].FindControl("lblStock")).Text;
                    string strQty = string.IsNullOrEmpty(((TextBox)gvT_SALES_DTL.Rows[n].Cells[5].FindControl("txtQty")).Text)
                        ? "0" : ((TextBox)gvT_SALES_DTL.Rows[n].Cells[5].FindControl("txtQty")).Text;
                    string strRefAmount = string.IsNullOrEmpty(((TextBox)gvT_SALES_DTL.Rows[n].Cells[7].FindControl("txtRefAmount")).Text)
                        ? "0" : ((TextBox)gvT_SALES_DTL.Rows[n].Cells[7].FindControl("txtRefAmount")).Text;
                    string strNetAmount = string.IsNullOrEmpty(((Label)gvT_SALES_DTL.Rows[n].Cells[8].FindControl("lblTotalPrice")).Text)
                        ? "0" : ((Label)gvT_SALES_DTL.Rows[n].Cells[8].FindControl("lblTotalPrice")).Text;

                    double MRP = 0;
                    double Stock = 0;
                    double Qty = 0;
                    double RefAmount = 0;
                    double NetAmount = 0;

                    if (double.TryParse(strMRP, out MRP) && double.TryParse(strStock, out Stock)
                        && double.TryParse(strQty, out Qty) && double.TryParse(strRefAmount, out RefAmount)
                        && double.TryParse(strNetAmount, out NetAmount))
                    {
                        TextBox txtMRP = (TextBox)gvT_SALES_DTL.Rows[n].Cells[3].FindControl("txtSalePrice");
                        Label lblStock = (Label)gvT_SALES_DTL.Rows[n].Cells[4].FindControl("lblStock");
                        TextBox txtQty = (TextBox)gvT_SALES_DTL.Rows[n].Cells[5].FindControl("txtQty");
                        TextBox txtRefAmount = (TextBox)gvT_SALES_DTL.Rows[n].Cells[7].FindControl("txtRefAmount");
                        Label lblNetAmount = (Label)gvT_SALES_DTL.Rows[n].Cells[8].FindControl("lblTotalPrice");

                        subTotal = subTotal + (MRP * Qty);
                        totalRefAmount = (totalRefAmount + RefAmount);

                    }
                    else
                    {
                        //show message OF ERROR
                        return;
                    }
                    totalqty += Qty;
                }//for

                txtSubTotal.Text = subTotal.ToString();
                txtdiscount.Text = totalRefAmount.ToString();
                txtNetAmount.Text = (subTotal - totalRefAmount).ToString();
                txttotalquantity.Text = totalqty.ToString();
                // newly added by das
                TextDueAmount.Text = txtNetAmount.Text;

                LoadNetAmount();
            }
            catch (Exception ex) { }
            finally
            {

            }
        }
        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int rowIndex = ((sender as LinkButton).NamingContainer as GridViewRow).RowIndex;

                //dt
                DataTable DTGridData = new DataTable();
                DTGridData.TableName = "gridTable";

                #region //column
                DTGridData.Columns.Add(new DataColumn("OID", typeof(Int64)));
                DTGridData.Columns.Add(new DataColumn("PROD_WGPG", typeof(Int64)));
                DTGridData.Columns.Add(new DataColumn("PROD_SUBCATEGORY", typeof(Int64)));
                DTGridData.Columns.Add(new DataColumn("DescriptionID", typeof(Int64)));
                DTGridData.Columns.Add(new DataColumn("StoreID", typeof(string)));//

                DTGridData.Columns.Add(new DataColumn("Barcode", typeof(string)));

                DTGridData.Columns.Add(new DataColumn("SalePrice", typeof(double)));
                DTGridData.Columns.Add(new DataColumn("Stock", typeof(double)));
                DTGridData.Columns.Add(new DataColumn("Qty", typeof(double)));
                DTGridData.Columns.Add(new DataColumn("TotalPrice", typeof(double)));
                DTGridData.Columns.Add(new DataColumn("RefAmount", typeof(double)));//

                DTGridData.Columns.Add("WGPG_NAME");
                DTGridData.Columns.Add("SubCategoryName");
                DTGridData.Columns.Add("Description");
                DTGridData.Columns.Add("Flag");
                DTGridData.Columns.Add("GiftStatus");//gift 
                #endregion

                for (int i = 0; i < gvT_SALES_DTL.Rows.Count; i++)
                {
                    if (i != rowIndex)
                    {
                        DataRow DTGridDataOldRow = DTGridData.NewRow();
                        //
                        DTGridDataOldRow["OID"] = gvT_SALES_DTL.DataKeys[i].Values["OID"];
                        DTGridDataOldRow["PROD_WGPG"] = gvT_SALES_DTL.DataKeys[i].Values["PROD_WGPG"];
                        DTGridDataOldRow["PROD_SUBCATEGORY"] = gvT_SALES_DTL.DataKeys[i].Values["PROD_SUBCATEGORY"];
                        DTGridDataOldRow["DescriptionID"] = gvT_SALES_DTL.DataKeys[i].Values["DescriptionID"];
                        DTGridDataOldRow["StoreID"] = gvT_SALES_DTL.DataKeys[i].Values["StoreID"];
                        DTGridDataOldRow["Barcode"] = gvT_SALES_DTL.DataKeys[i].Values["Barcode"];

                        string strMRP = string.IsNullOrEmpty(((TextBox)gvT_SALES_DTL.Rows[i].Cells[3].FindControl("txtSalePrice")).Text)
                                ? "0" : ((TextBox)gvT_SALES_DTL.Rows[i].Cells[3].FindControl("txtSalePrice")).Text;
                        string strStock = string.IsNullOrEmpty(((Label)gvT_SALES_DTL.Rows[i].Cells[4].FindControl("lblStock")).Text)
                            ? "0" : ((Label)gvT_SALES_DTL.Rows[i].Cells[4].FindControl("lblStock")).Text;
                        string strQty = string.IsNullOrEmpty(((TextBox)gvT_SALES_DTL.Rows[i].Cells[5].FindControl("txtQty")).Text)
                            ? "0" : ((TextBox)gvT_SALES_DTL.Rows[i].Cells[5].FindControl("txtQty")).Text;
                        string strRefAmount = string.IsNullOrEmpty(((TextBox)gvT_SALES_DTL.Rows[i].Cells[7].FindControl("txtRefAmount")).Text)
                            ? "0" : ((TextBox)gvT_SALES_DTL.Rows[i].Cells[7].FindControl("txtRefAmount")).Text;
                        string strNetAmount = string.IsNullOrEmpty(((Label)gvT_SALES_DTL.Rows[i].Cells[8].FindControl("lblTotalPrice")).Text)
                            ? "0" : ((Label)gvT_SALES_DTL.Rows[i].Cells[8].FindControl("lblTotalPrice")).Text;

                        double MRP = 0;
                        double Stock = 0;
                        double Qty = 0;
                        double RefAmount = 0;
                        double NetAmount = 0;

                        if (double.TryParse(strMRP, out MRP) && double.TryParse(strStock, out Stock)
                            && double.TryParse(strQty, out Qty) && double.TryParse(strRefAmount, out RefAmount)
                            && double.TryParse(strNetAmount, out NetAmount)
                            )
                        {
                            DTGridDataOldRow["SalePrice"] = MRP;
                            DTGridDataOldRow["Stock"] = Stock;
                            DTGridDataOldRow["Qty"] = Qty;
                            DTGridDataOldRow["TotalPrice"] = NetAmount;
                            DTGridDataOldRow["RefAmount"] = RefAmount;

                        }

                        DTGridDataOldRow["WGPG_NAME"] = gvT_SALES_DTL.DataKeys[i].Values["WGPG_NAME"];
                        DTGridDataOldRow["SubCategoryName"] = gvT_SALES_DTL.DataKeys[i].Values["SubCategoryName"];
                        DTGridDataOldRow["Description"] = gvT_SALES_DTL.DataKeys[i].Values["Description"];
                        DTGridDataOldRow["Flag"] = gvT_SALES_DTL.DataKeys[i].Values["Flag"];
                        if (((CheckBox)gvT_SALES_DTL.Rows[i].Cells[2].FindControl("chkGift")).Checked)
                        {
                            DTGridDataOldRow["GiftStatus"] = "true";//gift
                        }
                        else
                        {
                            DTGridDataOldRow["GiftStatus"] = "false";//gift                                
                        }

                        DTGridData.Rows.Add(DTGridDataOldRow);
                    }
                }

                //
                LoadGrid(DTGridData);

                //ToCalltxtSalePice_change(null, null);

                LoadData();
            }
            catch (Exception ex) { }
            finally { }
        }

        protected void chkGift_CheckedChanged(object sender, EventArgs e)
        {
            int i = ((sender as CheckBox).NamingContainer as GridViewRow).RowIndex;
            CheckBox chkGift = (CheckBox)gvT_SALES_DTL.Rows[i].Cells[2].FindControl("chkGift");
            TextBox txtQty = (TextBox)gvT_SALES_DTL.Rows[i].Cells[5].FindControl("txtQty");
            TextBox txtSalePrice = (TextBox)gvT_SALES_DTL.Rows[i].Cells[3].FindControl("txtSalePrice");

            if (chkGift.Checked)
            {
                ((TextBox)gvT_SALES_DTL.Rows[i].Cells[7].FindControl("txtRefAmount")).Enabled = false;
                ((TextBox)gvT_SALES_DTL.Rows[i].Cells[7].FindControl("txtRefAmount")).Text = "0";
                ((Label)gvT_SALES_DTL.Rows[i].Cells[8].FindControl("lblTotalPrice")).Text =
                        ((Convert.ToDecimal(txtQty.Text) * Convert.ToDecimal(txtSalePrice.Text))).ToString("0");
            }
            else
            {
                ((TextBox)gvT_SALES_DTL.Rows[i].Cells[7].FindControl("txtRefAmount")).Enabled = true;
                ((TextBox)gvT_SALES_DTL.Rows[i].Cells[7].FindControl("txtRefAmount")).Text = "0";
                ((Label)gvT_SALES_DTL.Rows[i].Cells[8].FindControl("lblTotalPrice")).Text =
                       ((Convert.ToDecimal(txtQty.Text) * Convert.ToDecimal(txtSalePrice.Text))).ToString("0");
            }



            //totalGiftAmount += (Convert.ToDecimal(txtQty.Text) * Convert.ToDecimal(txtSalePrice.Text));


            LoadNetAmount();

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
                        sda.Fill(dt);
                    }
                }
            }
            return dt;
        }//


        protected void btnCheckChanges_Click(object sender, EventArgs e)
        {

        }
        protected void lblcashpaid_TextChanged(object sender, EventArgs e)
        {
            String tp;
            
            if (ddlPaymentMode.SelectedItem.Text == "Cash & Card") 
            {
                txtCardPaid.Text = (Convert.ToDouble(txtNetAmount.Text) - Convert.ToDouble(lblcashpaid.Text)).ToString();
            }

            tp = Convert.ToString(Math.Round(Convert.ToDouble(lblcashpaid.Text) + Convert.ToDouble(txtCardPaid.Text)));

            if (Convert.ToDouble(lblcashpaid.Text) > Convert.ToDouble(txtNetAmount.Text))
            {
                lblMessage.Text = "You Can't not Enter More Than the Net Amount Please Re-Check !!";
                lblMessage.ForeColor = Color.Red;
                btnSaveSale.Enabled = false;
                return;

            }
            else
            {
                btnSaveSale.Enabled = true;
            }

            txtReceiveAmount.Text = tp;
            //if (ddlPaymentMode.SelectedItem.Text == "Cash & Card") { }

            //txtCardPaid.Text = (Convert.ToDouble(txtReceiveAmount.Text) - Convert.ToDouble(lblcashpaid.Text)).ToString();
            TextDueAmount.Text = (Convert.ToDouble(txtNetAmount.Text) - Convert.ToDouble(txtReceiveAmount.Text)).ToString();
        }

        protected void txtCardPaid_TextChanged(object sender, EventArgs e)
        {
            String tp;
            tp = Convert.ToString(Math.Round(Convert.ToDouble(lblcashpaid.Text) + Convert.ToDouble(txtCardPaid.Text)));

            txtReceiveAmount.Text = tp;

            // txtCardPaid.Text = (Convert.ToDouble(txtReceiveAmount.Text) - Convert.ToDouble(lblcashpaid.Text)).ToString();
            TextDueAmount.Text = (Convert.ToDouble(txtNetAmount.Text) - Convert.ToDouble(txtReceiveAmount.Text)).ToString();
        }



        protected void ddlCustomerList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string full = ddlCustomerList.SelectedItem.Text.ToString();

            //string[] splitString = full.Split('-');

            //string requiredString = splitString[1];

            //txtMobileNumber.Text = requiredString.ToString();
            //txtMobileNumber.Enabled = false;
            txtEmailAddress.Enabled = true;
            txtAddress.Enabled = true;
            txtMobileNumber.Enabled = true;

            if (ddlCustomerList.SelectedValue != "")
            {
                string strSql = string.Format(@"Select * From Customers Where ID={0}", ddlCustomerList.SelectedValue);
                DataTable dtCustomerDetail = LoadDataByQuery(strSql);

                if (dtCustomerDetail.Rows.Count > 0)
                {
                    txtMobileNumber.Text = dtCustomerDetail.Rows[0]["Number"].ToString();
                    txtAddress.Text = dtCustomerDetail.Rows[0]["Address"].ToString();
                    txtEmailAddress.Text = dtCustomerDetail.Rows[0]["Email"].ToString();

                    if (txtMobileNumber.Text.Trim() != "")
                    {
                        txtMobileNumber.Enabled = false;
                    }
                    if (txtAddress.Text.Trim() != "")
                    {
                        txtAddress.Enabled = false;
                    }
                    if (txtEmailAddress.Text.Trim() != "")
                    {
                        txtEmailAddress.Enabled = false;
                    }

                    string sql = string.Format(@"
                                select sum(j.Debit),sum(j.Credit),Balance=ISNULL((sum(j.Credit-j.Debit)),0) 
                                from Acc_Journal j
                                where j.Particular='A/R' and j.Remarks='Customer' and j.Branch='{0}' and j.AccountID={1}
                                ",
                                 ShopID,
                                 ddlCustomerList.SelectedValue);

                    DataTable dt = DAL.LoadDataByQuery(sql);

                    decimal Balance = 0;
                    if (decimal.TryParse(dt.Rows[0]["Balance"].ToString(), out Balance)) { }
                    if (Balance >= 0)
                    {
                        //lblPayablelbl.Text = "Accounts Payable";
                        //lblPayableAmount.Text = Balance.ToString("0.00");
                        //lblPayableAmount.ForeColor = System.Drawing.Color.Red;
                        //lblPayablelbl.ForeColor = System.Drawing.Color.Red;
                        PreviousDue.Text = Balance.ToString("0.00");
                        PreviousDue.ForeColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                       // lblPayablelbl.Text = "Accounts Receivable";
                        PreviousDue.Text = Math.Abs(Balance).ToString("0.00");
                       
                       
                    }





                }
                else
                {
                    txtMobileNumber.Text = string.Empty;
                    txtAddress.Text = string.Empty;
                    txtEmailAddress.Text = string.Empty;
                    PreviousDue.Text = string.Empty;
                }
            }
            else
            {
                txtMobileNumber.Text = string.Empty;
                txtAddress.Text = string.Empty;
                txtEmailAddress.Text = string.Empty;
                PreviousDue.Text = string.Empty;
            }

        }
        protected void ddlBank_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblcashpaid.Text = "0";
            txtCardPaid.Text = "0";
            if (ddlBank.SelectedIndex > 0)
            {
                if (ddlPaymentMode.SelectedItem.Text == "Card") 
                {
                    trCashPaid.Visible = false;
                    trCardPaid.Visible = false;
                    txtCardPaid.Text = "0";
                    txtCardPaid.Enabled = false;
                }
                else if (ddlPaymentMode.SelectedItem.Text == "Cash & Card") 
                {
                    trCashPaid.Visible = true;
                    txtCardPaid.Enabled = true;
                    trCardPaid.Visible = false;
                    txtCardPaid.Text = "0";
                    txtCardPaid.Enabled = false;
                }
                
            }
            else
            {
                if (ddlPaymentMode.SelectedItem.Text == "Cash & Card" && ddlPaymentMode.SelectedItem.Text == "Cash") 
                {
                    trCashPaid.Visible = true;
                }
               
                trCardPaid.Visible = false;
                txtCardPaid.Text = "0";
                txtCardPaid.Enabled = false;
            }
        }
    }//

}//
