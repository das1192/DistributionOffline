﻿using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TalukderPOS.BusinessObjects;
using TalukderPOS.BLL;

public partial class Pages_SaleItemsReportForAdmin : System.Web.UI.Page
{
    private string userID = string.Empty;
    private string userPassword = string.Empty;
    SalesReport_BILL BILL = new SalesReport_BILL();

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
        DataTable dt = BILL.SalesItemsList(entity);
        GridView1.DataSource = dt;
        GridView1.DataBind();

        var TotalSalesAmount = dt.AsEnumerable().Sum(dr => dr.Field<Int64>("Total"));
        lblTotalSalesAmount.Text = "Total Sales Amount : " + TotalSalesAmount.ToString();

        var TotalDiscount = dt.AsEnumerable().Sum(dr => dr.Field<Int64>("DiscountAmount"));
        lblTotalDiscount.Text = "Total Discount Amount : " + TotalDiscount.ToString();

        lblTotalNetAmount.Text = (Convert.ToInt64(TotalSalesAmount) - Convert.ToInt64(TotalDiscount)).ToString();
        
        
        
        
    }
}