using System;
using System.Web;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using TalukderPOS.BLL;
using TalukderPOS.BusinessObjects;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;

public partial class Pages_shop_logo : System.Web.UI.Page
{
    private string userID = string.Empty;
    private string userPassword = string.Empty;
    private string Shop_id = string.Empty;
    AddShopBILL BILL = new AddShopBILL();

    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            userID = Session["UserID"].ToString();
            userPassword = Session["Password"].ToString();
            Shop_id = Session["StoreID"].ToString();
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            scriptManager.RegisterPostBackControl(btnSave);

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

            BindList(Shop_id);  
            
        }
        Page.Form.Attributes.Add("enctype", "multipart/form-data");
    }

    public string GetImage(object img)
    {
        return "data:image/jpg;base64," + Convert.ToBase64String((byte[])img);
    }

    protected void ShopLOGO_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
    {
        AddShop_BO entity = new AddShop_BO();
        entity.OID = gvShopLOGO.DataKeys[e.RowIndex].Value.ToString();
        entity.EUSER = userID;
        BILL.DeleteLogo(entity);
        BindList(Shop_id);
    }

    private void BindList(string Shop_id)
    {
        DataTable dt = BILL.BindListLogo(Shop_id);
        gvShopLOGO.DataSource = dt;
        gvShopLOGO.DataBind();
        byte[] bytes;
        string base64String = string.Empty;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["ImageByte"])))
            {


                bytes = (byte[])dt.Rows[i]["ImageByte"];
                base64String = Convert.ToBase64String(bytes, 0, bytes.Length);

                Image img = (Image)gvShopLOGO.Rows[i].FindControl("ImgBookPic");
                img.ImageUrl = "data:image/jpeg;base64," + base64String;

            }
        }
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        AddShop_BO entity = new AddShop_BO();
        entity.OID = lblOID.Value.ToString();
        entity.Shop_id = Shop_id.ToString();
        if (fileupload.HasFile)
        {


            string fileName = string.Empty;
            string filePath = string.Empty;
            string getPath = string.Empty;
            string pathToStore = string.Empty;
            string finalPathToStore = string.Empty;

            Byte[] bytes;
            FileStream fs;
            BinaryReader br;

            fileName = fileupload.FileName;
            filePath = Server.MapPath("ShopImage/" + System.Guid.NewGuid() + fileName);
            fileupload.SaveAs(filePath);




            fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
         

            br = new BinaryReader(fs);
            bytes = br.ReadBytes(Convert.ToInt32(fs.Length));


            br.Close();
            fs.Close();

          
            entity.imgarray = bytes;
            entity.ActiveStatus = "1";
            entity.IUSER = userID.ToString();
            entity.EUSER = userID.ToString();
            BILL.AddLogo(entity);

            lblMessage.Text = "SAVED SUCCESSFULLY";
            entity = null;
            BindList(Shop_id);
        }
        else
        {
            lblMessage.Text = "NO FILE SELECTED";
        }
      
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
       
        ContainerBankInfo.ActiveTabIndex = 0;
        lblMessage.Text = string.Empty;
    }


    
   
    
}