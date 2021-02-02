using System;
using System.Text;

namespace TalukderPOS.BusinessObjects
{
	[Serializable()]
	public class MenuPermission
	{
		public int  GrantID { get; set; }
		public string  UserID { get; set; }
		public int  MenuHeadID { get; set; }
		public int  PageID { get; set; }
		public bool  CanView { get; set; }

		public MenuPermission()
		{ }

		public MenuPermission(int GrantID,string UserID,int MenuHeadID,int PageID,bool CanView)
		{
			this.GrantID = GrantID;
			this.UserID = UserID;
			this.MenuHeadID = MenuHeadID;
			this.PageID = PageID;
			this.CanView = CanView;
		}

		public override string ToString()
		{
			return "GrantID = " + GrantID.ToString() + ",UserID = " + UserID + ",MenuHeadID = " + MenuHeadID.ToString() + ",PageID = " + PageID.ToString() + ",CanView = " + CanView.ToString();
		}

	}
}
