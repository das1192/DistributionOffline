using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using TalukderPOS.BusinessObjects;
using TalukderPOS.DAL;

namespace TalukderPOS.BLL
{
     public class SalesReport_BILL
    {
         SalesReport_DAL DAL = new SalesReport_DAL();

         public DataTable SalesSummary(SalesReport_BO entity)
         {
             try
             {
                 return DAL.SalesSummary(entity);
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }

         public DataTable SalesItems(SalesReport_BO entity)
         {
             try
             {
                 return DAL.SalesItems(entity);
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }

         public DataTable DailyConsolidedSalesSummary(SalesReport_BO entity)
         {
             try
             {
                 return DAL.DailyConsolidedSalesSummary(entity);
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }

         public DataTable SalesReport(SalesReport_BO entity)
         {
             try
             {
                 return DAL.SalesReport(entity);
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }
         public DataTable ProductMovement12(T_PROD entity)
         {
             try
             {
                 return DAL.ProductMovement12(entity);
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }

         public DataTable SalesItemsList(T_PROD entity)
         {
             try
             {
                 return DAL.SalesItemsList(entity);
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }

         public DataTable DiscountReportByModel(T_PROD entity)
         {
             try
             {
                 return DAL.DiscountReportByModel(entity);
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }

         public DataTable SalesComboReport(SalesReport_BO entity)
         {
             try
             {
                 return DAL.SalesComboReport(entity);
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }

         public DataTable BarphoneComboReport(SalesReport_BO entity)
         {
             try
             {
                 return DAL.BarphoneComboReport(entity);
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }

         public DataTable SalesStockSummaryReport(T_PROD entity)
         {
             try
             {
                 return DAL.SalesStockSummaryReport(entity);
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }

         public DataTable SalesStockSummaryReport1(T_PROD entity)
         {
             try
             {
                 return DAL.SalesStockSummaryReport1(entity);
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }

         public DataTable RequistionReport(T_PROD entity)
         {
             try
             {
                 return DAL.RequistionReport(entity);
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }


         public string[] Validation(T_PROD entity)
         {
             string[] valid = new string[2];
             try
             {
                 if (entity.Branch == String.Empty || entity.Branch == "0")
                 {
                     valid[0] = "False";
                     valid[1] = "PLEASE SELECT A BRANCH";
                 }
                 else if (entity.NoOfDate == string.Empty || entity.NoOfDate == "0")
                 {
                     valid[0] = "False";
                     valid[1] = "PLEASE SELECT NO. OF PREVIOUS DAYS";
                 }
                 else if (entity.PROD_WGPG == string.Empty || entity.PROD_WGPG == "0")
                 {
                     valid[0] = "False";
                     valid[1] = "PLEASE SELECT PRODUCT CATEGORY";
                 }
                 else
                 {
                     valid[0] = "True";
                     valid[1] = String.Empty;
                 }
             }
             catch (Exception ex)
             {
                 throw ex;
             }
             return valid;
         }


         public string[] Validation1(T_PROD entity)
         {
             string[] valid = new string[2];
             try
             {
               if (entity.NoOfDate == string.Empty || entity.NoOfDate == "0")
                 {
                     valid[0] = "False";
                     valid[1] = "PLEASE SELECT NO. OF PREVIOUS DAYS";
                 }
                 else if (entity.PROD_WGPG == string.Empty || entity.PROD_WGPG == "0")
                 {
                     valid[0] = "False";
                     valid[1] = "PLEASE SELECT PRODUCT CATEGORY";
                 }
                 else
                 {
                     valid[0] = "True";
                     valid[1] = String.Empty;
                 }
             }
             catch (Exception ex)
             {
                 throw ex;
             }
             return valid;
         }


         public DataTable ProductMovement(T_PROD entity)
         {
             try
             {
                 return DAL.ProductMovement(entity);
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }

         public DataTable DOAList(T_PROD entity)
         {
             try
             {
                 return DAL.DOAList(entity);
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }



    }
}
