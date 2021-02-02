using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TalukderPOS.BLL;
using System.Data;
using TalukderPOS.BusinessObjects;
using System.Drawing;
using TalukderPOS.BusinessObjects;


public partial class Pages_UserPageAuthorizationForm : System.Web.UI.Page
{
    MenuHeadBLL objMenuHeadBLL = new MenuHeadBLL();
    MenuPageBLL objMenuPageBLL = new MenuPageBLL();
    MenuPermissionBLL objMenuPermissionBLL = new MenuPermissionBLL();

    private string userID = string.Empty;
    private string userPassword = string.Empty;
     private string Shop_id = string.Empty;   
        
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            userID = Session["UserID"].ToString();
            userPassword = Session["Password"].ToString();
            
        }
        catch
        {
            Response.Redirect("~/Default.aspx");
        }

        if (!Page.IsPostBack)
        {
            BindMenuHead();
            CommonBinder.LoadDDLUser(ddlUser);
        }
    }

    protected void ddlUser_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlUser.SelectedIndex == 0)
        {
            BindMenuHead();
        }
        else
        {
            if (gvAllPages.Rows.Count > 0)
            {
                BindMenuHead();
                List<MenuHead> listMenuHead = objMenuHeadBLL.MenuHead_GetAllByUserPermission(ddlUser.SelectedValue, true);

                foreach (MenuHead entityParentModule in listMenuHead)
                {
                    for (int i = 0; i < gvAllPages.Rows.Count; i++)
                    {
                        if (Convert.ToInt32(gvAllPages.DataKeys[i].Value) == entityParentModule.MenuHeadID)
                        {
                            ((CheckBox)gvAllPages.Rows[i].FindControl("chkBoxHeader")).Checked = true;

                            List<MenuPage> listChildModule = objMenuPageBLL.MenuPage_GetAllByHeadUser(entityParentModule.MenuHeadID, ddlUser.SelectedValue, true);

                            foreach (MenuPage entityChildModule in listChildModule)
                            {
                                for (int j = 0; j < ((GridView)gvAllPages.Rows[i].FindControl("gvPages")).Rows.Count; j++)
                                {
                                    if (Convert.ToInt32(((GridView)gvAllPages.Rows[i].FindControl("gvPages")).DataKeys[j].Value) == entityChildModule.PageId)
                                    {
                                        ((CheckBox)((GridView)gvAllPages.Rows[i].FindControl("gvPages")).Rows[j].FindControl("chkBoxPages")).Checked = true;
                                    }
                                }
                            }

                        }
                    }
                }
            }
        }
    }

    protected void gvAllPages_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#98F5FF'");
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;");
            int headID = Convert.ToInt32(gvAllPages.DataKeys[e.Row.RowIndex].Value);
            DataTable dtPages = objMenuPageBLL.MenuPage_GetAllByHeadID(headID);
            if (dtPages.Rows.Count > 0)
            {
                ((GridView)e.Row.FindControl("gvPages")).DataSource = dtPages;
                ((GridView)e.Row.FindControl("gvPages")).DataBind();
            }
        }
    }

    protected void gvAllPages_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvAllPages.PageIndex = e.NewPageIndex;
        BindMenuHead();
    }

    protected void gvPages_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#0AD4CD'");
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;");
        }
    }



    protected void btnPermission_Click(object sender, EventArgs e)
    {
        int checke = 0;
        if (gvAllPages.Rows.Count > 0)
        {
            for (int i = 0; i < gvAllPages.Rows.Count; i++)
            {
                if (((CheckBox)gvAllPages.Rows[i].FindControl("chkBoxHeader")).Checked)
                {
                    for (int j = 0; j < ((GridView)gvAllPages.Rows[i].FindControl("gvPages")).Rows.Count; j++)
                    {
                        if (((CheckBox)((GridView)gvAllPages.Rows[i].FindControl("gvPages")).Rows[j].FindControl("chkBoxPages")).Checked)
                        {
                            checke = 1;
                            break;
                        }
                    }
                    if (checke > 0)
                    {
                        break;
                    }
                }
            }
            Save();
        }
        
    }

    private void Save()
    {
        int success = -1;
        success = objMenuPermissionBLL.MenuPermission_DeleteByUserID(ddlUser.SelectedValue);

        if (success >= 0)
        {
            success = 0;
            for (int head = 0; head < gvAllPages.Rows.Count; head++)
            {
                if (((CheckBox)gvAllPages.Rows[head].FindControl("chkBoxHeader")).Checked)
                {
                    int headID = Convert.ToInt32(gvAllPages.DataKeys[head].Value);

                    for (int page = 0; page < ((GridView)gvAllPages.Rows[head].FindControl("gvPages")).Rows.Count; page++)
                    {
                        if (((CheckBox)((GridView)gvAllPages.Rows[head].FindControl("gvPages")).Rows[page].FindControl("chkBoxPages")).Checked)
                        {
                            int pageID = Convert.ToInt32(((GridView)gvAllPages.Rows[head].FindControl("gvPages")).DataKeys[page].Value);
                            MenuPermission objMenuPermission = new MenuPermission();

                            objMenuPermission.CanView = true;
                            objMenuPermission.MenuHeadID = headID;
                            objMenuPermission.PageID = pageID;
                            objMenuPermission.UserID = ddlUser.SelectedValue;

                            success = objMenuPermissionBLL.MenuPermission_Add(objMenuPermission);
                        }

                    }
                }
            }
        }

        if (success > 0)
        {
            lblMessage.Text = ContextConstant.SAVED_SUCCESS;
            lblMessage.ForeColor = Color.Green;
            CommonBinder.LoadDDLUser(ddlUser);
            BindMenuHead();

        }
    }

    private void BindMenuHead()
    {
        List<MenuHead> listHeader = objMenuHeadBLL.MenuHead_GetAll();
        gvAllPages.DataSource = listHeader;
        gvAllPages.DataBind();
    }

}