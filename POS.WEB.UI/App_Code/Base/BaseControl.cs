using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Diagnostics;

/// <summary>
/// Summary description for BaseControl
/// </summary>
/// 
namespace SALESANDINVENTORY.Web.UI
{
    public class BaseControl : System.Web.UI.UserControl
    {
        protected int CurrentEmployeeID
        {
            [DebuggerStepThrough]
            get
            {
                int EmployeeId = new int();//for Admin creator id is zero(0).
                try
                {
                    EmployeeId = Convert.ToInt32(Page.Session[ContextConstant.EMPLOYEE_ID_OF_LOGGED_IN_USER]);
                }
                catch { }
                return EmployeeId;
            }
        }

        #region Methods

       
        protected void InsertToCache(string key, object value, double seconds)
        {
            Cache.Insert(key, value, null, DateTime.Now.AddSeconds(seconds), Cache.NoSlidingExpiration);
        }

        #endregion
    }
}
