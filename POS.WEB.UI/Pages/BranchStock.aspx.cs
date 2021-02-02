using System;
using System.Data.SqlClient;
using System.Web;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using TalukderPOS.BusinessObjects;
using System.Configuration;
using TalukderPOS.BLL;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Drawing;

public partial class Pages_BranchStock : System.Web.UI.Page
{
    private string userID = string.Empty;
    private string userPassword = string.Empty;
    T_PRODBLL BILL = new T_PRODBLL();
    private string Shop_id = string.Empty;

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

            //visible false 2 TabContainer   when  shopInfo oid=43
            if (Shop_id == "43")
            {
                tPnlDisplay3.Visible = false;
                TabPanel1.Visible = false;
                // products with barcode
                //TabPanel4.Visible = false;

            }
            else
            {
                tPnlDisplay3.Visible = true;
                TabPanel1.Visible = true;
                // products with barcode
                //TabPanel4.Visible = false;
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
        }
    }
    protected void cmdSearchForprice_Click(object sender, EventArgs e)
    {
        StockReportForAdmin_BILL BILL2 = new StockReportForAdmin_BILL();


        T_PROD entity = new T_PROD();
        entity.Branch = Session["StoreID"].ToString();
        entity.PROD_WGPG = ddlProductCategoryForPrice.SelectedItem.Value.ToString();
        entity.PROD_SUBCATEGORY = ddlModelForPrice.SelectedItem.Value.ToString();
        entity.PROD_DES = ddlColorForPrice.SelectedItem.Value.ToString();


        DataTable dt = BILL2.TotalStock(entity);
        Session["GridData"] = dt;
        BindList1();

        //load total qty
        double Qty = 0;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            Qty += Convert.ToDouble(dt.Rows[i]["Quantity"].ToString());
        }
        lblQty.Text = "<span class='label label-primary'>Total Quantity : " + Qty.ToString() + "</span>";
    }
    protected void cmdSearchForprice2_Click(object sender, EventArgs e)
    {
        StockReportForAdmin_BILL BILL2 = new StockReportForAdmin_BILL();


        T_PROD entity = new T_PROD();
        entity.Branch = Session["StoreID"].ToString();
        entity.PROD_WGPG = ddlProductCategoryForPrice2.SelectedItem.Value.ToString();
        entity.PROD_SUBCATEGORY = ddlModelForPrice2.SelectedItem.Value.ToString();
        entity.PROD_DES = ddlColorForPrice2.SelectedItem.Value.ToString();
        entity.STOCK_TYPE = ddlstocktype.SelectedValue.ToString();

        DataTable dt = BILL2.TotalStockQuantity(entity);
        Session["GridData"] = dt;
        BindList2();
    }


    protected void StockAdjust_Details(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
    {
        GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
        int rowIndex = row.RowIndex;

        //empty
        lblRowIndexNewPrice.Text = string.Empty;
        txtNewPrice.Text = string.Empty;
        txtNewQty.Text = string.Empty;
        lblBarcode.Text = string.Empty;

        //============================================================================================
        if (e.CommandName == "cmdPriceAdjust")
        {
            string CostPrice = gvStockAdjust.DataKeys[rowIndex].Values["CostPrice"].ToString();
            string Quantity = gvStockAdjust.DataKeys[rowIndex].Values["Quantity"].ToString();

            //
            string Branch = gvStockAdjust.DataKeys[rowIndex].Values["Branch"].ToString();
            string DescriptionID = gvStockAdjust.DataKeys[rowIndex].Values["DescriptionID"].ToString();
            string ACC_STOCKID = gvStockAdjust.DataKeys[rowIndex].Values["ACCOID"].ToString();

            string vendorlist = string.Empty;
            if (ddlstocktype.SelectedValue == "Barcode")
            {
                vendorlist = string.Format(@"
select distinct s.Vendor_ID,v.Vendor_Name,(v.Vendor_Name+'_'+v.Vendor_mobile)vendorList
from T_STOCK s 
inner join Vendor v on v.OID=s.Vendor_ID
where s.Branch='{0}' and s.PROD_DES={1}
order by v.Vendor_Name
", Branch, DescriptionID);
            }
            else
            {
                vendorlist = string.Format(@"
                select distinct s.VendorID as Vendor_ID,v.Vendor_Name,(v.Vendor_Name+'_'+v.Vendor_mobile)vendorList
                from Vendor v
                inner join Acc_Stock s on s.VendorID=v.OID
                where s.ACC_STOCKID ='{0}'
                ", ACC_STOCKID);
            }
            DataTable dtVendor = GetDataTableByQuery(vendorlist);

            ddlVendorForPriceAdjust.DataSource = dtVendor;
            ddlVendorForPriceAdjust.DataTextField = "vendorList";
            ddlVendorForPriceAdjust.DataValueField = "Vendor_ID";
            ddlVendorForPriceAdjust.DataBind();

            //show popup
            lblRowIndexNewPrice.Text = rowIndex.ToString();
            txtNewPrice.Text = CostPrice;
            txtNewQty.Text = Quantity;

            txtNewQty.Enabled = true;
            if (ddlstocktype.SelectedValue == "Barcode")
            {
                txtNewQty.Enabled = false;
                lblIsBarcode.Text = "Y";
                string barcodesign = gvStockAdjust.DataKeys[rowIndex].Values["Barcode"].ToString(); ;
                lblBarcodeSign.Text = barcodesign;

                //ddlVendorForPriceAdjust.Enabled = false;
                //Label10.Enabled=false;
            }
            else
            {
                //txtNewQty.Enabled = true;
                lblIsBarcode.Text = "N";

            }
            ddlVendorForPriceAdjust.Enabled = true;
            Label10.Enabled = true;
            txtNewQty.Enabled = false;
            ModalPopupExtender1.Show();

        }
        //============================================================================================
        else if (e.CommandName == "Adjust")
        {
            Label lblOIDSMS = (Label)row.Cells[0].FindControl("lblOIDSMS");


            lblOIDSMSGR.Text = lblOIDSMS.Text;

            Label lblOIDACC = (Label)row.Cells[0].FindControl("lblOIDACC");
            lblOIDACCGR.Text = lblOIDACC.Text;
            Label lblCategory = (Label)row.Cells[0].FindControl("lblCategory");
            lblCategorynew.Text = lblCategory.Text;
            Label lblModel = (Label)row.Cells[0].FindControl("lblModel");
            lblModelnew.Text = lblModel.Text;
            Label lblDescription = (Label)row.Cells[0].FindControl("lblDescription");
            lblDescriptionnew.Text = lblDescription.Text;
            Label lblQuantity = (Label)row.Cells[0].FindControl("lblQuantity");
            lblOLDQUANTITY.Text = lblQuantity.Text;

            //sadiq
            //int rowIndex = ((sender as LinkButton).NamingContainer as GridViewRow).RowIndex;
            //int rowIndex = row.RowIndex;

            lblCategoryID.Text = gvStockAdjust.DataKeys[rowIndex].Values["CategoryID"].ToString();
            lblSubCategoryID.Text = gvStockAdjust.DataKeys[rowIndex].Values["SubCategoryID"].ToString();
            lblDescriptionID.Text = gvStockAdjust.DataKeys[rowIndex].Values["DescriptionID"].ToString();
            lblCostPrice.Text = gvStockAdjust.DataKeys[rowIndex].Values["CostPrice"].ToString();
            lblBranch.Text = gvStockAdjust.DataKeys[rowIndex].Values["Branch"].ToString();
            lblAVERAGE.Text = gvStockAdjust.DataKeys[rowIndex].Values["AVERAGE"].ToString();
            lblBarcode.Text = gvStockAdjust.DataKeys[rowIndex].Values["Barcode"].ToString();
            txtQuantity.Text = lblOLDQUANTITY.Text;
            //txtQuantity.Enabled = false;
            lblTPRODID.Text = gvStockAdjust.DataKeys[rowIndex].Values["TPRODID"].ToString();
            txtQuantity.Enabled = true;
            if (ddlstocktype.SelectedValue == "Barcode") { txtQuantity.Enabled = false; }

            ModalPopupExtender2.Show();

        }

    }

    protected void btnSaveNewPrice_Click(object sender, EventArgs e) //Modal Window Delete Product
    {
        int rowIndex = Convert.ToInt32(lblRowIndexNewPrice.Text);

        //ACCOID,CostPrice,Quantity,CategoryID,SubCategoryID,DescriptionID,Branch,AVERAGE


        string strCostPriceOld = gvStockAdjust.DataKeys[rowIndex].Values["CostPrice"].ToString();
        string strQuantityOld = gvStockAdjust.DataKeys[rowIndex].Values["Quantity"].ToString();//
        string Branch = gvStockAdjust.DataKeys[rowIndex].Values["Branch"].ToString();
        string CategoryID = gvStockAdjust.DataKeys[rowIndex].Values["CategoryID"].ToString();
        string SubCategoryID = gvStockAdjust.DataKeys[rowIndex].Values["SubCategoryID"].ToString();
        string DescriptionID = gvStockAdjust.DataKeys[rowIndex].Values["DescriptionID"].ToString();
        string ACC_STOCKID = gvStockAdjust.DataKeys[rowIndex].Values["ACCOID"].ToString();

        string strCostPriceNew = txtNewPrice.Text;
        string strQuantityNew = txtNewQty.Text;
        string isbarode = string.Empty;
        string barcodesign = string.Empty;
        barcodesign = lblBarcodeSign.Text.ToString();
        isbarode = lblIsBarcode.Text.ToString();
        decimal CostPriceOld = 0; decimal CostPriceNew = 0;
        decimal QtyOld = 0; decimal QtyNew = 0;
        decimal PriceDiff = 0; decimal QtyDiff = 0;

        if (decimal.TryParse(strCostPriceOld, out CostPriceOld)) { }
        if (decimal.TryParse(strCostPriceNew, out CostPriceNew)) { }
        if (decimal.TryParse(strQuantityOld, out QtyOld)) { }
        if (decimal.TryParse(strQuantityNew, out QtyNew)) { }

        PriceDiff = (CostPriceOld - CostPriceNew);
        if (isbarode == "Y")
        {
            QtyDiff = 1;
        }
        else
        {
            QtyDiff = (QtyOld - QtyNew);
        }

        //check validation
        string isValid = "yes";
        if (PriceDiff == 0)
        {
            isValid = "no";
            //show msg    
            string msg = "Check the New Price Please!";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + msg + "')", true);
            return;
        }
        else if (CostPriceNew > CostPriceOld)
        {
            isValid = "no";
            txtNewPrice.Text = CostPriceOld.ToString("0.00");
            txtNewPrice.Focus();
            //show msg    
            string msg = "Sorry, the New Price need to be less than recent Price!";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + msg + "')", true);
            return;
        }
        else if (QtyNew > QtyOld)
        {
            isValid = "no";
            //show msg    
            string msg = "Sorry Not Enough Stock!";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + msg + "')", true);
            return;
        }


        if (isValid == "yes")
        {
            //update and save in master
            string sqlUpdateMaster = string.Empty;
            string sqlUpdateMasterNew = string.Empty;
            if (isbarode == "Y")
            {
                sqlUpdateMaster = string.Format(@"
update Acc_Stock
set Quantity=(Quantity-{0}))
where   Branch='{1}' and PROD_WGPG='{2}' and PROD_SUBCATEGORY='{3}' and PROD_DES='{4}' and CostPrice='{5}' --and Quantity={6}
", QtyDiff, Branch, CategoryID, SubCategoryID, DescriptionID, CostPriceOld, QtyOld);

                sqlUpdateMaster = string.Format(@"
update Acc_Stock
set Quantity=(Quantity-{0})
where  ACC_STOCKID = '{1}'
", QtyDiff, ACC_STOCKID);

                //                sqlUpdateMaster = string.Format(@"
                //update Acc_Stock
                //set CostPrice='{0}'
                //where   ACC_STOCKID = '{1}'--Branch='{1}' and PROD_WGPG='{2}' and PROD_SUBCATEGORY='{3}' and PROD_DES='{4}' and CostPrice='{5}' --and Quantity={6}
                //", CostPriceNew , ACC_STOCKID );
            }
            else
            {
                sqlUpdateMaster = string.Format(@"
update Acc_Stock
set Quantity=(Quantity-{0})
where  Branch='{1}' and PROD_WGPG='{2}' and PROD_SUBCATEGORY='{3}' and PROD_DES='{4}' and CostPrice='{5}' --and Quantity={6}
", QtyNew, Branch, CategoryID, SubCategoryID, DescriptionID, CostPriceOld, QtyOld);

                sqlUpdateMaster = string.Format(@"
                update Acc_Stock
                set CostPrice='{0}'
                where   ACC_STOCKID = '{1}'
                ", CostPriceNew, ACC_STOCKID);



            }
            //insert in Master
            string availparice = string.Empty;
            string sqlselectPriceavail = string.Format(@"
Select Top(1)* from Acc_Stock 
where PROD_WGPG='{0}' AND isnull(Flag,'')='' AND PROD_SUBCATEGORY='{1}' AND PROD_DES='{2}' AND Branch='{3}' 
AND CostPrice='{4}'


update Description set SESPrice='{4}' where OID='{2}'

", CategoryID, SubCategoryID, DescriptionID, Branch
            , CostPriceNew, QtyNew, DateTime.Now.Date, DateTime.Now.ToString(), userID);

            try
            {
                SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
                if (dbConnect.State == ConnectionState.Closed)
                    dbConnect.Open();


                SqlCommand command = new SqlCommand(sqlselectPriceavail, dbConnect);
                SqlDataReader sReader;

                command.Parameters.Clear();

                sReader = command.ExecuteReader();

                while (sReader.Read())
                {
                    availparice = sReader["CostPrice"].ToString();
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            string sqlInsertMaster = string.Empty;
            if (availparice == string.Empty)
            {
                if (isbarode == "Y")
                {
                    sqlInsertMaster = string.Format(@"
insert into Acc_Stock(
PROD_WGPG, PROD_SUBCATEGORY, PROD_DES, Branch
, CostPrice, Quantity, IDAT, IDATTIME, IUSER    ,PQty, PRQty, SQty, SRQty, MQty) 
values({0},{1},{2},'{3}'    ,{4},{5},'{6}','{7}','{8}'  , {5}, '0', '0', '0', '0')
", CategoryID, SubCategoryID, DescriptionID, Branch
   , CostPriceNew, QtyNew, DateTime.Now.Date, DateTime.Now.ToString(), userID);
                }
            }
            else
            {
                if (isbarode == "Y")
                {
                    sqlInsertMaster = string.Format(@"
update Acc_Stock
set Quantity=(Quantity+{0})
where Branch='{1}' and PROD_WGPG='{2}' and PROD_SUBCATEGORY='{3}' and PROD_DES='{4}' and CostPrice='{5}' --and Quantity={6}
", QtyNew, Branch, CategoryID, SubCategoryID, DescriptionID, CostPriceNew, QtyOld);
                }
            }

            //Updated By Sagar for Price Adjust Purpose

            string sqlUpadteT_stockBarcode = string.Empty;
            string sqlUpadteT_ProdBarcode = string.Empty;
            if (isbarode == "Y")
            {
                sqlUpadteT_stockBarcode = string.Format(@"
update T_STOCK
set CostPrice={0}
where SaleStatus='0' AND Branch='{1}' and PROD_WGPG='{2}' and PROD_SUBCATEGORY='{3}' and PROD_DES='{4}' and Barcode='{5}' 
", CostPriceNew, Branch, CategoryID, SubCategoryID, DescriptionID, barcodesign);

                sqlUpadteT_ProdBarcode = string.Format(@"
update T_PROD
set CostPrice={0}
where SaleStatus='0' AND Branch='{1}' and PROD_WGPG='{2}' and PROD_SUBCATEGORY='{3}' and PROD_DES='{4}' and Barcode='{5}'
", CostPriceNew, Branch, CategoryID, SubCategoryID, DescriptionID, barcodesign);

            }

            //Updated End  By Sagar for Price Adjust Purpose






            //insert in Details
            string sqlInsertDetails = string.Format(@"
insert into Acc_StockDetail
(Po_Number, PROD_WGPG, PROD_SUBCATEGORY, PROD_DES, Branch
, Quantity, CostPrice, SalePrice, Total, Flag
, Remarks, IDAT, IUSER, IDATTIME)
values('{0}',{1},{2},{3},'{4}'  ,{5},{6},{7},{8},'{9}'  ,'{10}','{11}','{12}','{13}')
", "poNo", CategoryID, SubCategoryID, DescriptionID, Branch
, QtyNew, PriceDiff, 0, (PriceDiff * QtyNew).ToString("0.00"), "Price Protection"
, "Price Protection", DateTime.Now.Date, userID, DateTime.Now.ToString());

            string sqlInsertAmendmenttable = string.Format(@"
insert into PurchasePrice_Amendment
(PROD_WGPG, PROD_SUBCATEGORY, PROD_DES, Branch       , Amount,IUSER,IDAT,Vendor_ID)
values({0},{1},{2},'{3}'  ,{4},'{5}','{6}','{7}')
", CategoryID, SubCategoryID, DescriptionID, Branch
, (PriceDiff * QtyNew), userID, DateTime.Now.Date, ddlVendorForPriceAdjust.SelectedValue);

            //create journal
            string RefNo = string.Format(@"PA-{0}", DateTime.Now.ToString("yyyyMMddHHMMss"));
            string InsertDR = string.Format(@"
INSERT INTO Acc_Journal(AccountID,Branch,Particular,Remarks,Debit,Credit,RefferenceNumber,IDAT,IDATTIME,Narration) 
VALUES('{0}','{1}','{2}','{3}',{4}
,{5},'{6}','{7}','{8}','{9}')
"
, string.IsNullOrEmpty(ddlVendorForPriceAdjust.SelectedValue) ? "0" : ddlVendorForPriceAdjust.SelectedValue
, Branch
, "A/P", "Supplier", (PriceDiff * QtyNew).ToString("0.00")

, 0
, RefNo
, DateTime.Now.ToString(), DateTime.Now.ToString(), string.Format(@"Income from Price Protection")
);


            string InsertCR = string.Format(@"
INSERT INTO Acc_Journal(AccountID,Branch,Particular,Remarks,Debit,Credit,RefferenceNumber,IDAT,IDATTIME,Narration) 
VALUES({0},{1},'{2}','{3}',{4}
,{5},'{6}','{7}','{8}','{9}')
", 7777
, Branch
, "Income"
, "Income"
, 0

, (PriceDiff * QtyNew).ToString("0.00")
, RefNo
, DateTime.Now.ToString()
, DateTime.Now.ToString()
, string.Format(@"Income from Price Protection"));


            try
            {
                SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
                if (dbConnect.State == ConnectionState.Closed)
                    dbConnect.Open();

                SqlCommand cmdUpdateMaster = new SqlCommand(sqlUpdateMaster, dbConnect);

                SqlCommand cmdInsertDetails = new SqlCommand(sqlInsertDetails, dbConnect);
                SqlCommand cmdInsertAmendmenttable = new SqlCommand(sqlInsertAmendmenttable, dbConnect);
                SqlCommand cmdDR = new SqlCommand(InsertDR, dbConnect);
                SqlCommand cmdCR = new SqlCommand(InsertCR, dbConnect);

                if (isbarode == "Y")
                {
                    SqlCommand cmdInsertMaster = new SqlCommand(sqlInsertMaster, dbConnect);
                    SqlCommand cmdT_stock = new SqlCommand(sqlUpadteT_stockBarcode, dbConnect);
                    SqlCommand cmdT_Prod = new SqlCommand(sqlUpadteT_ProdBarcode, dbConnect);
                    cmdInsertMaster.ExecuteNonQuery();
                    cmdT_stock.ExecuteNonQuery();
                    cmdT_Prod.ExecuteNonQuery();
                }


                cmdUpdateMaster.ExecuteNonQuery();

                cmdInsertDetails.ExecuteNonQuery();//
                cmdInsertAmendmenttable.ExecuteNonQuery();//
                cmdDR.ExecuteNonQuery();
                cmdCR.ExecuteNonQuery();

                cmdSearchForprice2_Click(null, null);

                string msg = "Saved Successfully!";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + msg + "')", true);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //dbConnect.Close();
            }


        }

    }

    protected void cmdSearch3_Click(object sender, EventArgs e)
    {
        StockReportForAdmin_BILL BILL2 = new StockReportForAdmin_BILL();
        T_PROD entity = new T_PROD();
        entity.Branch = Shop_id.ToString();
        entity.PROD_WGPG = ddlSearchProductCategory3.SelectedItem.Value.ToString();
        entity.PROD_SUBCATEGORY = ddlSearchSubCategory3.SelectedItem.Value.ToString();
        entity.PROD_DES = ddlSearchDescription3.SelectedItem.Value.ToString();
        entity.FromDate = txtDateFrom2.Text;
        entity.ToDate = txtDateTo2.Text;



        gv_stockadjustment.Visible = true;

        entity.SearchType = "Details";
        DataTable dt = BILL2.StockAdjustSearch(entity);
        Session["GridData"] = dt;
        BindList3();


    }
    protected void gv_stockadjustment_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
    {
        gv_stockadjustment.PageIndex = e.NewPageIndex;
        BindList3();
    }
    private void BindList3()
    {
        gv_stockadjustment.DataSource = Session["GridData"];
        gv_stockadjustment.DataBind();
    }
    protected void btnSubmitDiscount_Click(object sender, EventArgs e) //Modal Window Delete Product
    {
        // sadiq 170918
        //to check the adjust qty
        double AdjustQuantity = 0;
        double OldQuantity = Convert.ToDouble(lblOLDQUANTITY.Text);
        if (double.TryParse(txtQuantity.Text, out AdjustQuantity))
        {
            //valid
            //stock checking
            if (AdjustQuantity > 0)
            {
                if (AdjustQuantity <= OldQuantity)
                {
                    StockReportForAdmin_BILL BILL2 = new StockReportForAdmin_BILL();
                    T_PROD entity = new T_PROD();


                    string oid = lblOIDSMSGR.Text.ToString();
                    entity = BILL2.GetProductInformation(oid);
                    //entity.OID = lblProductID.Text.ToString();
                    entity.OLDQUANTITY = lblOLDQUANTITY.Text.ToString();
                    entity.CostPrice = lblCostPrice.Text.ToString();
                    entity.Branch = Shop_id.ToString();
                    entity.IUSER = userID.ToString();
                    entity.Quantity = txtQuantity.Text.ToString();
                    entity.RunningAvg = lblAVERAGE.Text.ToString();
                    entity.OID = lblOIDSMSGR.Text.ToString();
                    entity.ACCOID = lblOIDACCGR.Text.ToString();
                    entity.STOCK_TYPE = ddlstocktype.SelectedValue.ToString();
                    entity.TPRODID = lblTPRODID.Text;
                    entity.Barcode = lblBarcode.Text.Trim();
                    //sadiq 170920

                    BILL2.AdjustStoreMasterStock(entity);

                    Response.Redirect("BranchStock.aspx?menuhead=109");
                }
                else
                {
                    //
                    string message = "Sorry, not enough stock!";
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "bappskey1", "alert('" + message + "');", true);

                    txtQuantity.Text = string.Empty;
                    return;
                }
            }
            else
            {
                string message = "Please check the quantity!";
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "bappskey1", "alert('" + message + "');", true);

                txtQuantity.Text = string.Empty;
                return;
            }
        }
        else
        {
            //invalid
            //
            string message = "Invalid quantity!";
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "bappskey1", "alert('" + message + "');", true);

            txtQuantity.Text = string.Empty;
            return;
        }
    }//


    private void BindList1()
    {
        gvProductPrice.DataSource = Session["GridData"];
        gvProductPrice.DataBind();
    }
    private void BindList2()
    {
        gvStockAdjust.DataSource = Session["GridData"];
        gvStockAdjust.DataBind();
    }
    private void BindList4()
    {
        gvProductListBP.DataSource = Session["BarcodeGridData"];
        gvProductListBP.DataBind();
    }

    protected void btnSearchBP_Click(object sender, EventArgs e)
    {
        //param
        string strParam = string.Empty;

        if (!string.IsNullOrEmpty(txtBarcodeBP.Text))
        {
            strParam += string.Format(@" and t.Barcode = '{0}' ", txtBarcodeBP.Text);
            strParam += string.Format(@" and t.SaleStatus = {0} ", ddlStatusBP.SelectedValue);
        }
        else
        {
            if (!string.IsNullOrEmpty(ddlStatusBP.SelectedValue))
            {
                strParam += string.Format(@" and t.SaleStatus = {0} ", ddlStatusBP.SelectedValue);
            }

            if (!string.IsNullOrEmpty(dptFromDateBP.Text) && !string.IsNullOrEmpty(dptToDateBP.Text))
            {
                strParam += string.Format(@" and convert(date,t.IDAT) between convert(date,'{0}') and convert(date,'{1}') ", dptFromDateBP.Text, dptToDateBP.Text);
            }
            else
            {
                dptFromDateBP.Text = string.Empty; dptToDateBP.Text = string.Empty; //when one or both date empty
                //strParam += string.Format(@" and DATEPART(MONTH, p.IDAT) = DATEPART(MONTH, getdate())");
            }
            if (!string.IsNullOrEmpty(ddlCategoryBP.SelectedValue) && ddlCategoryBP.SelectedValue != "--Please Select--")
            {
                strParam += string.Format(@" and t.PROD_WGPG = {0} ", ddlCategoryBP.SelectedValue);
            }
            if (!string.IsNullOrEmpty(ddlBrandBP.SelectedValue) && ddlBrandBP.SelectedValue != "--Please Select--")
            {
                strParam += string.Format(@" and t.PROD_SUBCATEGORY = {0} ", ddlBrandBP.SelectedValue);
            }
            if (!string.IsNullOrEmpty(ddlDescriptionBP.SelectedValue) && ddlDescriptionBP.SelectedValue != "--Please Select--")
            {
                strParam += string.Format(@" and t.PROD_DES = {0} ", ddlDescriptionBP.SelectedValue);
            }
            if (!string.IsNullOrEmpty(ddlVendorBP.SelectedValue) && ddlVendorBP.SelectedValue != "--Please Select--")
            {
                strParam += string.Format(@" and t.Vendor_ID = {0} ", ddlVendorBP.SelectedValue);
            }
        }

        strParam += string.Format(@" and t.Branch = '{0}' ", Session["StoreID"].ToString());



        //
        string sqlQuery = string.Format(@"
select ROW_NUMBER()over(order by WGPG_NAME,sc.SubCategoryName,d.Description) sl
, c.WGPG_NAME,sc.SubCategoryName, p.Barcode,d.Description 
,case  
when p.SaleStatus =1 then 'Sold' when p.SaleStatus =0 then 'Available' when p.SaleStatus =2 then 'Purchase Returned'
end as ProductStatus
,CONVERT(nvarchar(12),p.IDAT,106) IDAT,s.OID as StoreMasterStockOID,p.OID as T_PRODOID
, p.Branch, p.PROD_WGPG, p.PROD_SUBCATEGORY, p.PROD_DES,ISNULL(p.CostPrice,0) CostPrice, p.SalePrice, p.SaleStatus
,T_STOCKOID=(select top 1 q.OID
from T_STOCK q  
inner join Vendor v on v.OID =q.Vendor_ID 
where q.Branch=p.Branch and q.PROD_DES=p.PROD_DES and q.Barcode=p.Barcode
)
,SalesDate=(select top 1 convert(nvarchar(10),q.SalesDate,126)SalesDate
from T_STOCK q  
where q.Branch=p.Branch and q.PROD_DES=p.PROD_DES and q.Barcode=p.Barcode
)
,Vendor_ID=(select top 1 q.Vendor_ID
from T_STOCK q  
inner join Vendor v on v.OID =q.Vendor_ID 
where q.Branch=p.Branch and q.PROD_DES=p.PROD_DES and q.Barcode=p.Barcode
)
,Vendor_Name=(select top 1 v.Vendor_Name
from T_STOCK q  
inner join Vendor v on v.OID =q.Vendor_ID 
where q.Branch=p.Branch and q.PROD_DES=p.PROD_DES and q.Barcode=p.Barcode
)
from T_PROD p 
inner join Description d on d.OID=p.PROD_DES
inner join SubCategory sc on sc.OID=d.SubCategoryID
inner join T_WGPG c on c.OID=sc.CategoryID
--inner join T_STOCK ts on ts.Barcode =p.Barcode
inner join StoreMasterStock s 
on s.PROD_DES=p.PROD_DES and s.PROD_SUBCATEGORY=p.PROD_SUBCATEGORY and s.PROD_WGPG=p.PROD_WGPG 
and s.Branch=p.Branch  
where ISNULL(p.Barcode,'')!=''  and p.IDAT>'26 sep 2017' and p.SaleStatus in (0,1) 
{0}
order by WGPG_NAME,sc.SubCategoryName,d.Description

", strParam);

        sqlQuery = string.Format(@"
select * from (
----------------------
select ROW_NUMBER()over(order by WGPG_NAME,sc.SubCategoryName,d.Description) sl
, c.WGPG_NAME,sc.SubCategoryName, p.Barcode,d.Description 
,case  
when p.SaleStatus =1 then 'Sold' when p.SaleStatus =0 then 'Available' when p.SaleStatus =2 then 'Purchase Returned'
end as ProductStatus
,CONVERT(nvarchar(12),p.IDAT,106) IDAT,s.OID as StoreMasterStockOID,p.OID as T_PRODOID
, p.Branch, p.PROD_WGPG, p.PROD_SUBCATEGORY, p.PROD_DES,ISNULL(p.CostPrice,0) CostPrice, p.SalePrice, p.SaleStatus
,T_STOCKOID=(select top 1 q.OID
from T_STOCK q  
inner join Vendor v on v.OID =q.Vendor_ID 
where q.Branch=p.Branch and q.PROD_DES=p.PROD_DES and q.Barcode=p.Barcode And q.SaleStatus=0
)
,SalesDate=(select top 1 convert(nvarchar(10),q.SalesDate,126)SalesDate
from T_STOCK q  
where q.Branch=p.Branch and q.PROD_DES=p.PROD_DES and q.Barcode=p.Barcode 
)
,Vendor_ID=(select top 1 q.Vendor_ID
from T_STOCK q  
inner join Vendor v on v.OID =q.Vendor_ID 
where q.Branch=p.Branch and q.PROD_DES=p.PROD_DES and q.Barcode=p.Barcode
)
,Vendor_Name=(select top 1 v.Vendor_Name
from T_STOCK q  
inner join Vendor v on v.OID =q.Vendor_ID 
where q.Branch=p.Branch and q.PROD_DES=p.PROD_DES and q.Barcode=p.Barcode
)
from T_PROD p 
inner join Description d on d.OID=p.PROD_DES
inner join SubCategory sc on sc.OID=d.SubCategoryID
inner join T_WGPG c on c.OID=sc.CategoryID
--inner join T_STOCK ts on ts.Barcode =p.Barcode
inner join StoreMasterStock s 
on s.PROD_DES=p.PROD_DES and s.PROD_SUBCATEGORY=p.PROD_SUBCATEGORY and s.PROD_WGPG=p.PROD_WGPG 
and s.Branch=p.Branch  


where ISNULL(p.Barcode,'')!=''  and p.IDAT>'26 sep 2017' and p.SaleStatus in (0,1) 
) t
where 1=1  {0}
order by t.WGPG_NAME,t.SubCategoryName,t.Description
", strParam);



        DataTable dt = GetDataTableByQuery(sqlQuery);

        gvProductListBP.DataSource = null;
        gvProductListBP.DataSource = dt;
        gvProductListBP.DataBind();

        //for (int i = 0; i < gvProductListBP.Rows.Count; i++)
        //{
        //    //int rowIndex = ((sender as CheckBox).NamingContainer as GridViewRow).RowIndex;

        //    CheckBox chkPurchaseReturn = (CheckBox)gvProductListBP.Rows[i].Cells[9].FindControl("chkPurchaseReturn");
        //    Label lblQuantity2 = (Label)gvProductListBP.Rows[i].Cells[7].FindControl("lblQuantity2");

        //    if (lblQuantity2.Text == "Available") { chkPurchaseReturn.Enabled = true; } //ddlStatusBP.SelectedItem.ToString() == "Available"
        //    else { chkPurchaseReturn.Enabled = false; }

        //}


        //keep in session for paging
        Session["BarcodeGridData"] = dt;

        if (ddlStatusBP.SelectedItem.ToString() == "Available")
        {
            gvProductListBP.Columns[9].Visible = true;
        }
        else
        {
            gvProductListBP.Columns[9].Visible = false;
        }


        gvProductListBP.Enabled = true;
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


    protected void gvProductListBP_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvProductListBP.PageIndex = e.NewPageIndex;
        BindList4();

        if (ddlStatusBP.SelectedItem.ToString() == "Available")
        {
            gvProductListBP.Columns[9].Visible = true;
        }
        else
        {
            gvProductListBP.Columns[9].Visible = false;
        }
    }
    protected void btnPurchaseReturn_Click(object sender, EventArgs e)
    {
        //
        for (int i = 0; i < gvProductListBP.Rows.Count; i++)
        {
            CheckBox chkPurchaseReturn = (CheckBox)gvProductListBP.Rows[i].Cells[9].FindControl("chkPurchaseReturn");
            if (chkPurchaseReturn.Checked)
            {
                // update on three table  Branch,StoreMasterStockOID,T_PRODOID,T_STOCKOID,Barcode
                int StoreMasterStockOID = Convert.ToInt32(gvProductListBP.DataKeys[i].Values["StoreMasterStockOID"]);
                int T_STOCKOID = Convert.ToInt32(gvProductListBP.DataKeys[i].Values["T_STOCKOID"]);
                int T_PRODOID = Convert.ToInt32(gvProductListBP.DataKeys[i].Values["T_PRODOID"]);

                int PROD_DESOID = Convert.ToInt32(gvProductListBP.DataKeys[i].Values["PROD_DES"]);

                string Branch = gvProductListBP.DataKeys[i].Values["Branch"].ToString();
                string Barcode = gvProductListBP.DataKeys[i].Values["Barcode"].ToString();

                //, PROD_WGPG, PROD_SUBCATEGORY,CostPrice,Vendor_ID


                int PROD_WGPG = Convert.ToInt32(gvProductListBP.DataKeys[i].Values["PROD_WGPG"]);
                int PROD_SUBCATEGORY = Convert.ToInt32(gvProductListBP.DataKeys[i].Values["PROD_SUBCATEGORY"]);
                int Vendor_ID = Convert.ToInt32(gvProductListBP.DataKeys[i].Values["Vendor_ID"]);

                Int64 CostPrice = Convert.ToInt64(gvProductListBP.DataKeys[i].Values["CostPrice"]);

                // Commented By Yeasin 12-May-2019
                //BILL.purchaseReturn4Barcode(StoreMasterStockOID, T_STOCKOID, T_PRODOID, PROD_DESOID, Branch, Barcode, PROD_WGPG, PROD_SUBCATEGORY, Vendor_ID, CostPrice, Session["UserName"].ToString());

                // Added By Yeasin 12-May-2019
                BILL.PurchaseReturnByBarcode(StoreMasterStockOID, T_STOCKOID, T_PRODOID, PROD_DESOID, Branch, Barcode, PROD_WGPG, PROD_SUBCATEGORY, Vendor_ID, CostPrice, Session["UserName"].ToString());

                //journal
                #region debit credit

                //for journal entry
                //                string InsertDR = string.Format(@"
                //INSERT INTO Acc_Journal(AccountID,Branch,Particular,Remarks,Debit,Credit,RefferenceNumber,IDAT,IDATTIME,Narration) 
                //VALUES({0},{1},'{2}','{3}',{4}
                //,{5},{6},'{7}','{8}','{9}')
                //", Vendor_ID, Branch, "A/P", "Supplier", CostPrice
                //    , 0, "Ref-" + Barcode, DateTime.Now.ToString(), DateTime.Now.ToString(), "Purchase Return(barcode) From Supplier");

                //                string InsertCR = string.Format(@"
                //INSERT INTO Acc_Journal(AccountID,Branch,Particular,Remarks,Debit,Credit,RefferenceNumber,IDAT,IDATTIME,Narration) 
                //VALUES({0},{1},'{2}','{3}',{4}
                //,{5},{6},'{7}','{8}','{9}')
                //", PROD_DESOID, Branch, "Purchase", "Product", 0
                //, CostPrice, "Ref-" + Barcode, DateTime.Now.ToString(), DateTime.Now.ToString(), "Purchase Return(barcode) From Supplier");



                #endregion debit credit

            }

        }

        //load the grid again
        btnSearchBP_Click(null, null);


        gvProductListBP.Enabled = false;
        


    }//

    public void SaveDataByCRUD(string sqlQuery)
    {
        string constr = ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString;

        using (SqlConnection connection = new SqlConnection(constr))
        {
            SqlCommand command = new SqlCommand(sqlQuery, connection);
            command.Connection.Open();
            command.ExecuteNonQuery();
        }
    }//



   
}// //