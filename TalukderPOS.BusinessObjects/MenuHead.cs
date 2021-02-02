using System;
using System.Text;

namespace TalukderPOS.BusinessObjects
{
    [Serializable()]
    public class MenuHead
    {
        public int MenuHeadID { get; set; }
        public string MenuHeadName { get; set; }
        public int Priority { get; set; }
        public string DivID { get; set; }

        public MenuHead()
        { }

        public MenuHead(int MenuHeadID, string MenuHeadName, int Priority)
        {
            this.MenuHeadID = MenuHeadID;
            this.MenuHeadName = MenuHeadName;
            this.Priority = Priority;
        }

        public override string ToString()
        {
            return "MenuHeadID = " + MenuHeadID.ToString() + ",MenuHeadName = " + MenuHeadName + ",Priority = " + Priority.ToString();
        }

    }
}
