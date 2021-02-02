using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace TalukderPOS.Web.UI
{
    public partial class SalesDetailReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblSaleDate.Text= Session["SaleDate"].ToString();
                DateTime dt = Convert.ToDateTime(lblSaleDate.Text);
                lblSaleDate.Text = dt.Date.ToString("dd-MMM-yyyy");
                lblOrderNumber.Text= Session["OrderNo"].ToString();
                lblTotalAmount.Text= Session["TotalAmount"].ToString();
                lblTotalVal.Text= Session["TotalVAT"].ToString();
                lblTotalBill.Text= Session["TotalBill"].ToString();
                lblDiscountFinal.Text= Session["DiscountFinal"].ToString();
                lblNetPayable.Text= Session["NetPayable"].ToString();
                lblPayableInWord.Text= CommonBinder.NumberToCurrencyText(Convert.ToDecimal(lblNetPayable.Text));
                
                //lblMessage.Text += Request.QueryString["test_input"].ToString();

                gvSalesDetail.DataSource = Session["SaleDetail"];
                gvSalesDetail.DataBind();

                
                
            }
            catch
            {
                lblMessage2.Text = "No sale entry found.";
            }
        }

        public void getSalesDetailList()
        {
            
        }
    }
}
