using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using TalukderPOS.BusinessObjects;
using TalukderPOS.DAL;

namespace TalukderPOS.BLL
{
     public class BankAccountReport_BILL
    {
         BankInfoDAL DAL = new BankInfoDAL();

         public DataTable BankAccountReport(T_PROD entity)
         {
             try
             {
                 return DAL.BankAccountReport(entity);
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }
    }
}
