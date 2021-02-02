using System;
using System.Web;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using TalukderPOS.BLL;
using TalukderPOS.DAL;
using TalukderPOS.BusinessObjects;
using System.Web.UI.WebControls;

public partial class Pages_add_shopStatus : System.Web.UI.Page
{
    private string ShopID = string.Empty;
    private string userID = string.Empty;
    private string userPassword = string.Empty;

    AddShopBILL BILL = new AddShopBILL();
    CommonDAL DAL = new CommonDAL();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ShopID = Session["StoreID"].ToString();
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
            LoadDDLShopName();
            LoadGridShopStatus();

        }
    }
    private void LoadGridShopStatus()
    {
        string sql = string.Format(@"
select s.OID, s.ShopName,ShopCode=s.CCOM_PREFIX,s.ShopMobile,ShopAddress=s.CCOM_ADD1
,ActiveStatus=case when s.ActiveStatus=1 then 'Active' else 'Inactive' end
from ShopInfo s
order by s.ActiveStatus,s.ShopName
");
        DataTable dt = DAL.LoadDataByQuery(sql);

        gvShop.DataSource = null;
        if (dt.Rows.Count > 0)
        {
            gvShop.DataSource = dt;
        }
        gvShop.DataBind();

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            string activeStatus = gvShop.DataKeys[i].Values["ActiveStatus"].ToString();
            Button b = (Button)gvShop.Rows[i].Cells[7].FindControl("btnActiveInactive");
            if (activeStatus == "Active")
            {
                b.Text = "Inactive";
                b.ForeColor = System.Drawing.Color.Red;
            }
            else if (activeStatus == "Inactive")
            {
                b.Text = "Active";
                b.ForeColor = System.Drawing.Color.Green;

            }
            //.Enabled = true;

            //CheckBox chkGift = (CheckBox)gvT_SALES_DTL.Rows[i].Cells[2].FindControl("chkGift");
        }
    }
    private void LoadGridShopPayment()
    {
        string strParam = string.Empty;
        if (!string.IsNullOrEmpty(ddlShopName.SelectedValue.ToString()))
        {
            strParam = string.Format(@" and s.OID='{0}' ", ddlShopName.SelectedValue.ToString());
        }

        string sql = string.Format(@"

select 
p.OId,s.ShopName,EntryDate=CONVERT(nvarchar(12),p.IDATETIME,106),p.StatusPayment--,s.CCOM_PREFIX
,p.MonthYear,p.MonthNo,s.MonthlyFee,p.TotalFee,p.Remarks,p.TransactionID
from ShopInfo s
left join ShopPayment p on p.Branch=s.OID
where s.OID='{0}' {1}
order by p.StatusPayment desc,s.ShopName 
", ShopID, strParam);

        DataTable dt = DAL.LoadDataByQuery(sql);

        gvShopPayment.DataSource = null;
        if (dt.Rows.Count > 0)
        {
            gvShopPayment.DataSource = dt;
        }
        gvShopPayment.DataBind();

        //
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            string StatusPayment = gvShopPayment.DataKeys[i].Values["StatusPayment"].ToString();
            Button b = (Button)gvShopPayment.Rows[i].Cells[9].FindControl("btnPaymentApproved");
            if (StatusPayment == "Entered")
            {
                gvShopPayment.Rows[i].Cells[2].ForeColor = System.Drawing.Color.Red;
                b.Visible = true;
                b.ForeColor = System.Drawing.Color.Red;
            }

        }
    }



    protected void btnActiveInactive_Click(object sender, EventArgs e)
    {
        int i = ((sender as Button).NamingContainer as GridViewRow).RowIndex;

        int OID = Convert.ToInt32(gvShop.DataKeys[i].Values["OID"]);
        Button b = (Button)gvShop.Rows[i].Cells[7].FindControl("btnActiveInactive");

        string sql = string.Format(@"update ShopInfo set ActiveStatus='{1}'  where OID='{0}'", OID, b.Text == "Active" ? "1" : "0");
        DAL.SaveDataCRUD(sql);


        LoadGridShopStatus();

        lblMessage.Text = string.Format(@"Successfully {0}d", b.Text);
    }
    protected void btnShopPayment_Click(object sender, EventArgs e)
    {

        LoadGridShopPayment();

    }
    private void LoadDDLShopName()
    {
        string sql = string.Format(@"select s.OID,s.ShopName from ShopInfo s order by s.ShopName");
        DataTable dt = DAL.LoadDataByQuery(sql);

        if (dt.Rows.Count > 0)
        {
            ddlShopName.DataSource = dt;
            ddlShopName.DataValueField = "OID";
            ddlShopName.DataTextField = "ShopName";
            ddlShopName.DataBind();

            ddlShopName.Items.Insert(0, new ListItem("-- Select One--", String.Empty));

        }
    }
    protected void btnPaymentApproved_Click(object sender, EventArgs e)
    {
        int i = ((sender as Button).NamingContainer as GridViewRow).RowIndex;

        int OID = Convert.ToInt32(gvShopPayment.DataKeys[i].Values["OID"]);
        string StatusPayment = gvShopPayment.DataKeys[i].Values["StatusPayment"].ToString();

        string sql = string.Format(@"update ShopPayment set StatusPayment='Approved'  where OID='{0}'", OID);
        DAL.SaveDataCRUD(sql);

        //
        LoadGridShopPayment();

        lblMessage.Text = string.Format(@"Successfully Approved.");
    }
}