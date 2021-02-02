using System;
using System.Web;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using TalukderPOS.BLL;
using TalukderPOS.DAL;
using TalukderPOS.BusinessObjects;

public partial class Pages_add_shopPayment : System.Web.UI.Page
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
            LoadGrid();

            LoadDateFirstTime();

            ////Form tabl
            //dtpFrom.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            //txtMonthNo.Text = "1";  //set default month no 1
            //txtMonthNo_TextChanged(null, null);

        }
    }
    private void LoadDateFirstTime()
    {
        ////
        //string sql = string.Format(@"select s.OId,s.MonthlyFee from ShopInfo s where s.OID='{0}'", ShopID);
        //DataTable dtLocal = DAL.LoadDataByQuery(sql);

        //if (dtLocal.Rows.Count>0)
        //{
        //    lblMonthlyFee.Text = string.Format(@"{0}", dtLocal.Rows[0]["MonthlyFee"].ToString());
        //}

        ///////////////////////////////
        DateTime dt = DateTime.Now;
        DateTime dtBegin = dt.AddDays(-(dt.Day - 1));
        DateTime dtEnd = dtBegin.AddMonths(1).AddDays(-1);

        //
        lblMessage.Text = string.Empty;
        dtpFrom.Text = dtBegin.ToString("dd-MMM-yyyy");
        dtpTo.Text = dtEnd.ToString("dd-MMM-yyyy");
        
        
        txtMonthNo.Text = "1";

        lblTotalFee.Text = (Convert.ToDecimal(lblMonthlyFee.Text) * 1).ToString();
    }

    private void LoadGrid()
    {
        string sql = string.Format(@"
select 
p.OId,EntryDate=CONVERT(nvarchar(12),p.IDATETIME,106),p.StatusPayment--,s.CCOM_PREFIX
,p.MonthYear,p.MonthNo,s.MonthlyFee,p.TotalFee,P.TransactionID,p.Remarks
from ShopInfo s
left join ShopPayment p on p.Branch=s.OID
where s.OID='{0}'
order by p.StatusPayment
", ShopID);

        DataTable dt=DAL.LoadDataByQuery(sql);

        gvShopPayment.DataSource = null;
        if (dt.Rows.Count>0)
        {
            gvShopPayment.DataSource = dt;
        }
        gvShopPayment.DataBind();

        //
        lblMonthlyFee.Text=dt.Rows[0]["MonthlyFee"].ToString();


        //
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (gvShopPayment.DataKeys[i].Values["StatusPayment"].ToString() == "Approved")
            {
                gvShopPayment.Rows[i].Cells[1].ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                gvShopPayment.Rows[i].Cells[1].ForeColor = System.Drawing.Color.Red;
            }

        }

    }



    protected void dtpFrom_TextChanged(object sender, EventArgs e)
    {
        //
        GetToMonth();

    }

    protected void txtMonthNo_TextChanged(object sender, EventArgs e)
    {
        //
        GetToMonth();

    }

    private void GetToMonth()
    {

        //DateTime dt = Convert.ToDateTime(dtpFrom.Text);
        //DateTime dtBegin = dt.AddDays(-(dt.Day - 1));
        //DateTime dtEnd = dtBegin.AddMonths(1).AddDays(-1);

        //
        lblMessage.Text = string.Empty;
        
        //
        int MonthNo = 0;
        if (int.TryParse(txtMonthNo.Text, out MonthNo)) { } 
        else {
            lblMessage.Text = "Please check Month No";
            txtMonthNo.Focus(); 
            return; 
        }
        

        DateTime dtFrom = Convert.ToDateTime(dtpFrom.Text);

       
        //if (DateTime.TryParse(dtpFrom.Text, out DateFrom)) 
        //{
        //    lblFromDate.Text = DateFrom.ToString();
        //} 
        //else {
        //    lblMessage.Text = "Please Check Month!";
        //    dtpFrom.Focus(); 
        //    return; 
        //}
        
        
        DateTime dtTo = dtFrom.AddDays(1).AddMonths(MonthNo - 1).AddDays(-1);   //DateTime nextMonth = date.AddDays(1).AddMonths(1).AddDays(-1);

        dtpTo.Text = dtTo.ToString("dd-MMM-yyyy");

        dtpTo.Text = dtTo.AddMonths(1).AddDays(-1).ToString("dd-MMM-yyyy");     //dtTo.AddMonths(1).AddDays(-1);

        lblTotalFee.Text = string.Format(@"{0}", (MonthNo * Convert.ToDecimal(lblMonthlyFee.Text)));
    }


    protected void btnSavePayment_Click(object sender, EventArgs e)
    {
        lblMessage.Text = string.Empty;
        if (string.IsNullOrEmpty(txtTransactionID.Text))
        {
            lblMessage.Text = "Transaction id!";
            txtTransactionID.Focus();
            return;
        }
        if (string.IsNullOrEmpty(lblTotalFee.Text))
        {
            lblMessage.Text = "Month!";
            txtMonthNo.Focus();
            return;
        }
        HttpRequest currentRequest = HttpContext.Current.Request;
        String clientIP = currentRequest.ServerVariables["REMOTE_ADDR"];


        DateTime dtFrom = Convert.ToDateTime(dtpFrom.Text);
        DateTime dtTo = Convert.ToDateTime(dtpTo.Text);
        int MonthNo = Convert.ToInt16(txtMonthNo.Text);

        string abc = string.Empty;
        //
        for (int i = 0; i < MonthNo; i++)
        {
            DateTime CurrentFirstDate = dtFrom;
            DateTime CurrentLastDate = CurrentFirstDate.AddMonths(1).AddDays(-1);


            //save
            string sql = string.Format(@"
insert into ShopPayment
(
Branch,MonthYear,Year,Month,DateFrom
,DateTo,MonthlyFee,MonthNo,TotalFee,TransactionID
,Remarks,StatusPayment,IUSER,IPAddress ,IDAT
,IDATETIME
)
values('{0}','{1}','{2}','{3}','{4}',    
'{5}','{6}','{7}','{8}','{9}',    
'{10}','{11}','{12}','{13}', convert(date,getdate()), getdate()
)
", ShopID, CurrentFirstDate.ToString("MMM-yyyy"), CurrentFirstDate.ToString("yyyy"), CurrentFirstDate.ToString("MM"), CurrentFirstDate.ToString()
, CurrentLastDate.ToString(), lblMonthlyFee.Text, "1", lblMonthlyFee.Text, txtTransactionID.Text.Trim()
, txtRemarks.Text.Trim(), "Entered", userID, clientIP
);
            DAL.SaveDataCRUD(sql);

            dtFrom = CurrentFirstDate.AddMonths(1);
        }
        
        lblMessage.Text="Saved Successfully";

        //
        LoadGrid();

        //
        txtTransactionID.Text = string.Empty;
        txtRemarks.Text = string.Empty;

        tabcon.ActiveTabIndex = 0;
    }
}