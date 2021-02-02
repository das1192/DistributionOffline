using System;
using System.Text;

namespace TalukderPOS.BusinessObjects
{
	[Serializable()]
	public class MenuPage
	{
		public int  PageId { get; set; }
		public int  MenuHeadID { get; set; }
		public string  PageName { get; set; }
		public string  URL { get; set; }
		public DateTime?  CreateDate { get; set; }
		public DateTime?  LastUpdateDate { get; set; }
		public bool  IsRemoved { get; set; }

		public MenuPage()
		{ }

		public MenuPage(int PageId,int MenuHeadID,string PageName,string URL,Nullable<DateTime> CreateDate,Nullable<DateTime> LastUpdateDate,bool IsRemoved)
		{
			this.PageId = PageId;
			this.MenuHeadID = MenuHeadID;
			this.PageName = PageName;
			this.URL = URL;
			this.CreateDate = CreateDate;
			this.LastUpdateDate = LastUpdateDate;
			this.IsRemoved = IsRemoved;
		}

		public override string ToString()
		{
			return "PageId = " + PageId.ToString() + ",MenuHeadID = " + MenuHeadID.ToString() + ",PageName = " + PageName + ",URL = " + URL + ",CreateDate = " + CreateDate.ToString() + ",LastUpdateDate = " + LastUpdateDate.ToString() + ",IsRemoved = " + IsRemoved.ToString();
		}

	}
}
