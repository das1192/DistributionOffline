using System;
using System.Web;
using System.Data;
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

public partial class Pages_frmAccessoriesEdit : System.Web.UI.Page
{
    T_PRODBLL BILL = new T_PRODBLL();    

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack) 
            {
                lblOID.Value = Request.QueryString["OID"].ToString();
                T_PROD entity = BILL.GetAccessoriesProductInformation(lblOID.Value);                
                lblModel.Text = entity.PROD_SUBCATEGORY;
                lblDescription.Text = entity.PROD_DES;
                txtCostPrice.Text = entity.CostPrice;
                txtSalePrice.Text = entity.SalePrice;
                txtQuantity.Text = entity.Quantity;                
            }
    }


    protected void Button2_Click(object sender, EventArgs e)
    {
        if (txtCostPrice.Text == string.Empty) {
            lblMessage.Text = "Cost Price Can not be empty";
        }
        else if (txtSalePrice.Text == string.Empty) {
            lblMessage.Text = "Sale Price Can not be empty";
        }
        else if (txtQuantity.Text == string.Empty) {
            lblMessage.Text = "Quantity Can not be empty";
        }
        else {
            lblMessage.Text = string.Empty;
            T_PROD entity = new T_PROD();
            entity.OID = lblOID.Value;
            entity.AccessoriesEdit = "T";
            if (txtCostPrice.Text == string.Empty)
            {
                entity.CostPrice = "0";
            }
            else
            {
                entity.CostPrice = txtCostPrice.Text;
            }

            if (txtSalePrice.Text == string.Empty)
            {
                entity.SalePrice = "0";
            }
            else
            {
                entity.SalePrice = txtSalePrice.Text;
            }

            if (txtQuantity.Text == string.Empty)
            {
                entity.Quantity = "0";
            }
            else
            {
                entity.Quantity = txtQuantity.Text;
            }
            BILL.UpdateStoreMasterStock(entity);
            lblMessage.Text = "Save Successfully";
        }
    }





}