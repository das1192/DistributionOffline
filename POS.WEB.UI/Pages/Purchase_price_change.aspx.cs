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


namespace TalukderPOS.Web.UI
{
    public partial class Purchase_price_change : System.Web.UI.Page
    {
        private string userID = string.Empty;
        private string userPassword = string.Empty;
        private string Shop_id = string.Empty;
        T_PRODBLL BILL = new T_PRODBLL();
        

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
                Session["GridData"] = null;
            }            
        }

    
        private void Clear2()
        {

                    
            CascadingDropDown7.SelectedValue = "";
            CascadingDropDown8.SelectedValue = "";
            CascadingDropDown9.SelectedValue = "";
            CascadingDropDown13.SelectedValue = "";
          


        }
        
        protected void btnChange_Click(object sender, EventArgs e)
        {

            string[] valid = new string[2];
                string Desc = ddlDescriptionPrpRET.SelectedItem.Value.ToString();
                if (Desc == string.Empty)
                {
                    Label16.Text = "Please fill up all the fields";
                    Label16.ForeColor = Color.Red;
                }
                else
                {
                    T_PROD entity = BILL.GetProductInformationbydescription(Desc);
                    string availquan = BILL.getquantity(Desc);
                    entity.Vendor_ID = ddlVendorList2.SelectedItem.Value.ToString();
                    
                    entity.CostPrice = txtReturnPrice.Text.ToString();
                    valid = BILL.ReturnValidation2(entity);
                    if (valid[0].ToString() == "True")
                    {
                        int qtyold = Convert.ToInt32(availquan);
                       
                            BILL.Add_Purchase_Amendment(entity);
                          
                            Label16.Text = "Purchase Amendment Successfull";
                            Label16.ForeColor = Color.Green;
                        
                        Clear2();
                    }
                    else
                    {
                        Label16.Text = valid[1].ToString();
                    }
                }
            
        }
}
}
