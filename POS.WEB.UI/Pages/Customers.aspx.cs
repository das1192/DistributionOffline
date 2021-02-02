using System;
using System.Web;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using TalukderPOS.BLL;
using TalukderPOS.DAL;
using TalukderPOS.BusinessObjects;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace TalukderPOS.Web.UI
{
    public partial class Vendor_OutgoingForm : System.Web.UI.Page
    {
        private string userID = string.Empty;
        private string Shop_id = string.Empty;
        private string userPassword = string.Empty;

        Vendor_Outgoing BILL = new Vendor_Outgoing();
        CommonDAL DAL = new CommonDAL();
        T_PRODBLL BILL2 = new T_PRODBLL();
        SqlCommand cmd;
        SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
        SqlDataAdapter da;


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

            if (!isAuthenticate)
            {
                Response.Redirect("~/UnAuthorizedUser.aspx");
            }
            if (!Page.IsPostBack)
            {
                //BindList();

                Vendor_Outgoing_BO entity = new Vendor_Outgoing_BO();
                entity.Shop_id = Shop_id.ToString();



                dptSupplierPaymentDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                // new post back mechanism copied from investment



                DateTime date = DateTime.Now;
                var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);


                // lblMsgSupplierPaymentList.Text = string.Empty;
            }

            BindList();
        }

        protected void cmdSearchForprice_Click(object sender, EventArgs e)
        {
            T_PROD entity = new T_PROD();
            entity.Branch = Session["StoreID"].ToString();

        }

        protected void gvVendorAmount_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            e.Cancel = true;
            Clear();
            Vendor_Outgoing_BO entity = new Vendor_Outgoing_BO();
            // String OID = gvVendorAmount.DataKeys[e.NewEditIndex].Value.ToString();
            //  entity = BILL.GetById(OID);
            lblOID.Value = entity.OID;
            MNumber.Text = entity.AMOUNT;
            CustomerName.Text = entity.Remarks;

            ContainerProductVendor.ActiveTabIndex = 0;
            lblMessage.Text = string.Empty;
        }

        private bool IsConverToBigInt(string OID)
        {
            int Id = 0;
            return int.TryParse(OID, out Id);
        }

        private bool IsCustomerValidForm() 
        {
            bool isValid = true;
            lblMessage.Text = string.Empty;
            if (CustomerName.Text.Trim() == "") 
            {
                isValid = false;
                lblMessage.Text = "Customer Name is Required..";
            }
            //txtAddress
            if (txtAddress.Text.Trim() == "")
            {
                isValid = false;
                lblMessage.Text = "Address is Required..";
            }
            if (MNumber.Text.Trim() == "")
            {
                isValid = false;
                lblMessage.Text = "Moblie Number is Required..";
            }
            if (MNumber.Text.Trim() != "")
            {
                int m = 0;
                if (!int.TryParse(MNumber.Text, out m)) {
                    isValid = false;
                    lblMessage.Text = "Moblie Number Must Be Number..";
                }
            }
            if (txtPhone.Text.Trim() != "")
            {
                int m = 0;
                if (!int.TryParse(txtPhone.Text, out m))
                {
                    isValid = false;
                    lblMessage.Text = "Phone Number Must Be Number..";
                }
            }
            if (txtBalance.Text.Trim() == "")
            {
                isValid = false;
                lblMessage.Text = "Opening Balance is Required..";
            }
            if (txtBalance.Text.Trim() != "")
            {
                int m = 0;
                if (!int.TryParse(txtBalance.Text, out m))
                {
                    isValid = false;
                    lblMessage.Text = "Opening Balance Must Be Number..";
                }
            }
            //txtBalance
            return isValid;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsCustomerValidForm()) return;
            Vendor_Outgoing_BO entity = new Vendor_Outgoing_BO();
            entity.OID = lblOID.Value.Trim() == "" ? "0" : lblOID.Value.Trim();
            entity.CustomerNumber = MNumber.Text.ToString();
            entity.CustomerName = CustomerName.Text.ToString();
            entity.Shop_id = Shop_id.ToString();
            string Invoice = hdfRefNo.Value.Trim() == "" ? string.Format(@"Ref_RetailerOB_{0}_{1}_{2}", entity.Shop_id, entity.OID, DateTime.Now.ToString("yyMMddhhmmss")) : hdfRefNo.Value.Trim();
            // Newly Added By Yeasin 17-Jul-2019
            entity.Telephone = txtPhone.Text;
            entity.Address = txtAddress.Text;
            entity.Remarks = txtRemarks.Text;
            entity.OpeningBalance = txtBalance.Text;
            entity.Email = txtEmail.Text;
            entity.ActiveStatus = (chkStatus.Checked ? "True" : "False");
            entity.IDAT = string.IsNullOrEmpty(dptSupplierPaymentDate.Text) ? DateTime.Now.ToString("yyyy-MM-dd") : dptSupplierPaymentDate.Text;
            entity.RefferenceNumber = Invoice;
            entity.IUSER = userID;
            //DateTime.Now.ToString("yymmssff")
            BILL.AddCustomer(entity);
            Clear();
            lblMessage.Text = "SAVED SUCCESSFULLY";
            entity = null;
            //BindList(entity);
            //
            //CurrentCashBalance();
            BindList();
        }

        protected void grdCustomerList_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            e.Cancel = true;
            Clear();
            Vendor_Outgoing_BO entity = new Vendor_Outgoing_BO();
            String OID = grdCustomerList.DataKeys[e.NewEditIndex].Value.ToString();
            entity = BILL.GetRetailerById(OID);
            lblOID.Value = entity.OID;
            CustomerName.Text = entity.CustomerName;
            txtAddress.Text = entity.Address;
            MNumber.Text = entity.CustomerNumber;
            txtEmail.Text = entity.Email;
            txtPhone.Text = entity.Telephone;
            txtBalance.Text = entity.OpeningBalance;
            txtRemarks.Text = entity.Remarks;
            chkStatus.Checked = entity.ActiveStatus == "True" ? true : false;
            hdfRefNo.Value = entity.RefferenceNumber;
            ContainerProductVendor.ActiveTabIndex = 0;
            lblMessage.Text = string.Empty;
        }

        //private void BindList()
        //{
        //    Vendor_Outgoing_BO entity = new Vendor_Outgoing_BO();
        //    entity.Shop_id = Shop_id.ToString();
        //    DataTable dt = BILL.BindList(entity);
        //}

        protected void gvT_Delete_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            Vendor_Outgoing_BO entity = new Vendor_Outgoing_BO();

        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
            ContainerProductVendor.ActiveTabIndex = 0;
            lblMessage.Text = string.Empty;
        }


        private void Clear()
        {
            lblOID.Value = string.Empty;
            MNumber.Text = string.Empty;
            CustomerName.Text = string.Empty;
            txtAddress.Text = string.Empty;
            txtBalance.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtPhone.Text = string.Empty;
            txtRemarks.Text = string.Empty;
            dptSupplierPaymentDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            hdfRefNo.Value = string.Empty;
            chkStatus.Checked = false;

        }
        protected void cmdSearchDelete_Click(object sender, EventArgs e)
        {
            Vendor_Outgoing_BO entity = new Vendor_Outgoing_BO();
            entity.Shop_id = Shop_id.ToString();

            //entity.FromDate = txtDateFromdelete.Text;
            //entity.ToDate = txtDateTodelete.Text;
            DataTable dt = BILL.BindListdelete(entity);
            Session["GridDatadelete"] = dt;


        }

        protected void cmdSearchSup_Click(object sender, EventArgs e)
        {
            Vendor_Outgoing_BO entity = new Vendor_Outgoing_BO();
            entity.Shop_id = Shop_id.ToString();
            DataTable dt = BILL.BindList(entity);
        }

        //
        private void BindList()
        {
            string param = string.Format(@" Branch={0}", Shop_id);
            string sql = string.Format(@"Select ID,Name,Address,Number,Telephone,Email,OpeningBalance,ReferenceNumber,Case When CustomerStatus =1 then 'Active' Else 'Inactive' End 'CustomerStatus',AddedDate=convert(nvarchar(12), AddedDate,106) From Customers where {0}", param);

            DataTable dt = DAL.LoadDataByQuery(sql);
            grdCustomerList.DataSource = null;
            if (dt.Rows.Count > 0) { grdCustomerList.DataSource = dt; }
            grdCustomerList.DataBind();
        }
        //lblRetailerMsg

        private bool IsReportValid() 
        {
            bool Isvalid = true;
            lblRetailerMsg.Text = string.Empty;
            if (txtfDate.Text.Trim() == "") 
            {
                Isvalid = false;
                lblRetailerMsg.Text = "From Date is Required..";
            }
            if (txtfDate.Text.Trim() != "") 
            { 
                 DateTime fDate = new DateTime();
                 if (!DateTime.TryParse(txtfDate.Text.Trim(), out fDate)) 
                 {
                     Isvalid = false;
                     lblRetailerMsg.Text = "Invalid From Date..";
                 }
            }

            if (txtReceiveDate.Text.Trim() == "")
            {
                Isvalid = false;
                lblRetailerMsg.Text = "To Date is Required..";
            }
            if (txtReceiveDate.Text.Trim() != "")
            {
                DateTime fDate = new DateTime();
                if (!DateTime.TryParse(txtReceiveDate.Text.Trim(), out fDate))
                {
                    Isvalid = false;
                    lblRetailerMsg.Text = "Invalid To Date..";
                }
            }

            return Isvalid;

        }

        protected void btnReport_Click(object sender, EventArgs e)
        {

            if (!IsReportValid()) { return; }
             
            long CustomerID =0;

            if ((ddlRetailerID.Items.Count != 0) && !String.IsNullOrEmpty(ddlRetailerID.SelectedItem.Value))
            {
                CustomerID = Convert.ToInt64(ddlRetailerID.SelectedItem.Value);
            }
          
            
            DataTable dtsales = new DataTable();
            cmd = new SqlCommand("spGetRetailerSalesDetails", dbConnect);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@FromDate", SqlDbType.Date).Value = txtfDate.Text;
            cmd.Parameters.Add("@ToDate", SqlDbType.Date).Value = txtReceiveDate.Text;
            cmd.Parameters.Add("@CustomerId", SqlDbType.BigInt).Value = CustomerID;
            cmd.Parameters.Add("@Branch", SqlDbType.VarChar, 100).Value = Session["StoreID"].ToString();
            cmd.CommandType = CommandType.StoredProcedure;
            da = new SqlDataAdapter(cmd);
            da.SelectCommand.CommandTimeout = 300;
            da.Fill(dtsales);

            if (dtsales.Rows.Count > 0) 
            {
                Session["dtsales"] = dtsales;
                Session["ReportPath"] = "~/Reports/rptRetailerSales_2.rpt";
                string webUrl = "../Reports/ReportView.aspx";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenWindow", "window.open('" + webUrl + "');", true);
                //ClientScript.RegisterStartupScript(typeof(Page), "closePage", "window.close();", true);
            }
            else
            {
                Alert.ShowMessage("No Data Found To Generate Report..");
            }
            
        }
        protected void Button1_Click(object sender, EventArgs e)
        {

        }
}
}
