using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using TalukderPOS.BusinessObjects;
using TalukderPOS.DAL;

namespace TalukderPOS.BLL
{
    public class T_PRODBLL
    {
        T_PRODDAL DAL = new T_PRODDAL();


        public DataTable StoreMasterStockSearch(T_PROD entity)
        {
            try
            {
                return DAL.StoreMasterStockSearch(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable VendorStockSearch(T_PROD entity)
        {
            try
            {
                return DAL.VendorStockSearch(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable ProductHistorySearch(T_PROD entity)
        {
            try
            {
                return DAL.ProductHistorySearch(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void T_PROD_Add_Barcode(T_PROD entity)
        {
            try
            {
                DAL.Add_Barcode(entity);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void T_PROD_Add(T_PROD entity)
        {
            try
            {
                DAL.Add(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Yeasin --11-May-2019
        public void AddOrEditPurchase(T_PROD entity)
        {
            try
            {
                DAL.AddOrEditPurchase(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void PurchaseReturn(T_PROD entity)
        {
            try
            {
                DAL.PurchaseReturn(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //-------------------------

        public T_PROD GetBarcodeCostAndSalePrice(T_PROD entity)
        {
            try
            {
                return DAL.GetBarcodeCostAndSalePrice(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public string[] AddValidation(T_PROD obj)
        {
            string[] valid = new string[2];
            try
            {
                if (obj.PROD_WGPG.ToString() == String.Empty || obj.PROD_WGPG.ToString() == "0")
                {
                    valid[0] = "False";
                    valid[1] = "PLEASE SELECT A PRODUCT CATEGORY";
                }
                else if (obj.PROD_SUBCATEGORY.ToString() == String.Empty || obj.PROD_SUBCATEGORY.ToString() == "0")
                {
                    valid[0] = "False";
                    valid[1] = "PLEASE SELECT A MODEL";
                }
                else if (obj.Entrymode.ToString() == String.Empty || obj.Entrymode.ToString() == "0")
                {
                    valid[0] = "False";
                    valid[1] = "PLEASE SELECT A ENTRY MODE";
                }
                else if (obj.PROD_DES.ToString() == String.Empty || obj.PROD_DES.ToString() == "0")
                {
                    valid[0] = "False";
                    valid[1] = "PLEASE SELECT A COLOR/DESCRIPTION";
                }
                else if (obj.Branch.ToString() == String.Empty || obj.Branch.ToString() == "0")
                {
                    valid[0] = "False";
                    valid[1] = "PLEASE SELECT A BRANCH";
                }
                else if (obj.CostPrice.ToString() == String.Empty)
                {
                    valid[0] = "False";
                    valid[1] = "PLEASE ENTER COST PRICE";
                }
                else if (obj.SalePrice.ToString() == String.Empty)
                {
                    valid[0] = "False";
                    valid[1] = "PLEASE ENTER SALE PRICE";
                }
                else if (obj.PROD_WGPG.ToString() == "111" & obj.Quantity.ToString() == string.Empty)
                {
                    valid[0] = "False";
                    valid[1] = "PLEASE ENTER QUANTITY";
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

        public string[] ReturnValidation2(T_PROD obj)
        {
            string[] valid = new string[2];
            try
            {
                if (obj.PROD_WGPG.ToString() == String.Empty || obj.PROD_WGPG.ToString() == "0")
                {
                    valid[0] = "False";
                    valid[1] = "PLEASE SELECT A PRODUCT CATEGORY";
                }
                else if (obj.PROD_SUBCATEGORY.ToString() == String.Empty || obj.PROD_SUBCATEGORY.ToString() == "0")
                {
                    valid[0] = "False";
                    valid[1] = "PLEASE SELECT A MODEL";
                }

                else if (obj.PROD_DES.ToString() == String.Empty || obj.PROD_DES.ToString() == "0")
                {
                    valid[0] = "False";
                    valid[1] = "PLEASE SELECT A COLOR/DESCRIPTION";
                }

                else if (obj.CostPrice.ToString() == String.Empty)
                {
                    valid[0] = "False";
                    valid[1] = "PLEASE ENTER RETURN COST PRICE";
                }

                else if (obj.Vendor_ID.ToString() == "0" & obj.Vendor_ID.ToString() == string.Empty)
                {
                    valid[0] = "False";
                    valid[1] = "PLEASE ENTER VENDOR NAME";
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

        public string[] ReturnValidation(T_PROD obj)
        {
            string[] valid = new string[2];
            try
            {
                if (obj.PROD_WGPG.ToString() == String.Empty || obj.PROD_WGPG.ToString() == "0")
                {
                    valid[0] = "False";
                    valid[1] = "PLEASE SELECT A PRODUCT CATEGORY";
                }
                else if (obj.PROD_SUBCATEGORY.ToString() == String.Empty || obj.PROD_SUBCATEGORY.ToString() == "0")
                {
                    valid[0] = "False";
                    valid[1] = "PLEASE SELECT A MODEL";
                }
               
                else if (obj.PROD_DES.ToString() == String.Empty || obj.PROD_DES.ToString() == "0")
                {
                    valid[0] = "False";
                    valid[1] = "PLEASE SELECT A COLOR/DESCRIPTION";
                }
               
                else if (obj.CostPrice.ToString() == String.Empty)
                {
                    valid[0] = "False";
                    valid[1] = "PLEASE ENTER RETURN COST PRICE";
                }
                else if (obj.Quantity.ToString() == String.Empty)
                {
                    valid[0] = "False";
                    valid[1] = "PLEASE ENTER QUANTITY";
                }
                else if (obj.Vendor_ID.ToString() == "0" & obj.Vendor_ID.ToString() == string.Empty)
                {
                    valid[0] = "False";
                    valid[1] = "PLEASE ENTER VENDOR NAME";
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
        public void UpdateStoreMasterStock(T_PROD entity)
        {
            try
            {
                DAL.UpdateStoreMasterStock(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Add_Purchase_Amendment(T_PROD entity)
        {
            try
            {
                DAL.Add_Purchase_Amendment(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteTStock(T_PROD entity)
        {
            try
            {
                DAL.DeleteTStock(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateCashINOUT(T_PROD entity)
        {
            try
            {
                DAL.UpdateCashINOUT(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdatePurchaseReturn(T_PROD entity)
        {
            try
            {
                DAL.UpdatePurchaseReturn(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdatePurchaseReport(T_PROD entity)
        {
            try
            {
                DAL.UpdatePurchaseReport(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void MonitorStoreMasterStock(T_PROD entity)
        {
            try
            {
                DAL.MonitorStoreMasterStock(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateStockPosting(T_PROD entity)
        {
            try
            {
                DAL.UpdateStockPosting(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateClosingBalance(T_PROD entity)
        {
            try
            {
                DAL.UpdateClosingBalance(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ProductCostUpdate(T_PROD entity)
        {
            try
            {
                DAL.ProductCostUpdate(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String AccessoriesStockInHand(T_PROD entity)
        {
            try
            {
                return DAL.AccessoriesStockInHand(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public String GETIDAT(String OID)
        {
            try
            {
                return DAL.GETIDAT(OID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AccessoriesStockOut(T_PROD entity)
        {
            try
            {
                DAL.AccessoriesStockOut(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SPP_DeleteProduct(T_PROD entity)
        {
            try
            {
                DAL.SPP_DeleteProduct(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public T_PROD GetProductInformation(String OID)
        {
            try
            {
                return DAL.GetProductInformation(OID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public T_PROD GetProductInformationbydescription(String description)
        {
            try
            {
                return DAL.GetProductInformationbydescription(description);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string getquantity(String description)
        {
            try
            {
                return DAL.getquantity(description);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string getquantityforPurchaseReturn(String description, int Vendor_ID)
        {
            try
            {
                return DAL.getquantityforPurchaseReturn(description,Vendor_ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public T_PROD GetAccessoriesProductInformation(String OID)
        {
            try
            {
                return DAL.GetAccessoriesProductInformation(OID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable SearchAccessoriesStockInHeadOffice(T_PROD entity)
        {
            try
            {
                return DAL.SearchAccessoriesStockInHeadOffice(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable VendorDueReport(T_PROD entity)
        {
            try
            {
                return DAL.VendorDueReport(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable SPP_T_PROD_SearchByDate(T_PROD entity)
        {
            try
            {
                return DAL.SPP_T_PROD_SearchByDate(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

      





        public string[] ValidationForPriceUpdate(T_PROD obj)
        {
            string[] valid = new string[2];
            try
            {
               if (obj.SalePrice == String.Empty)
                {
                    valid[0] = "False";
                    valid[1] = "PLEASE ENTER UPDATE SALE PRICE";
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


        public String FindBarcode(String Barcode)
        {
            try
            {
                return DAL.FindBarcode(Barcode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public String FindBarcodeNew(String Barcode, String Shop_id)
        {
            try
            {
                return DAL.FindBarcodeNew(Barcode, Shop_id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SearchDateWiseBranchStock(T_PROD entity)
        {
            try
            {
                return DAL.SearchDateWiseBranchStock(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable AccessoriesStockOutReport(T_PROD entity)
        {
            try
            {
                return DAL.AccessoriesStockOutReport(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable ProductHistory(T_PROD entity)
        {
            DataTable dt = new DataTable();        
            dt.Columns.Add("WGPG_NAME", typeof(string));
            dt.Columns.Add("SubCategoryName", typeof(string));            
            dt.Columns.Add("Type", typeof(string));
            dt.Columns.Add("InwardsQty", typeof(Int32));            
            dt.Columns.Add("OutwardsQty", typeof(Int32));            
            dt.Columns.Add("ClosingQty", typeof(Int32));            
            dt.Columns.Add("IDAT");
            DataRow workRow;

            Int32 ClosingQty = 0;            

            try
            {
                DataTable dt1 = DAL.OpeningBalance(entity);
                if (dt1 != null)
                {
                    if (dt1.Rows.Count > 0)
                    {
                        workRow = dt.NewRow();
                        workRow["WGPG_NAME"] = dt1.Rows[0]["WGPG_NAME"];
                        workRow["SubCategoryName"] = dt1.Rows[0]["SubCategoryName"];
                        workRow["Type"] = "Opening Balance";
                        workRow["InwardsQty"] = dt1.Rows[0]["InwardsQty"];
                        workRow["OutwardsQty"] = dt1.Rows[0]["OutwardsQty"];
                        ClosingQty = ClosingQty + Convert.ToInt32(dt1.Rows[0]["InwardsQty"]) - Convert.ToInt32(dt1.Rows[0]["OutwardsQty"]);
                        workRow["ClosingQty"] = ClosingQty;
                        workRow["IDAT"] = string.Empty;
                        dt.Rows.Add(workRow);
                    }
                }


                DataTable dt2 = DAL.ProductHistory(entity);
                if (dt2 != null)
                {
                    if (dt2.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt2.Rows)
                        {
                            workRow = dt.NewRow();
                            workRow["WGPG_NAME"] = row["WGPG_NAME"].ToString();
                            workRow["SubCategoryName"] = row["SubCategoryName"].ToString();
                            workRow["Type"] = row["Particulars"].ToString();
                            workRow["InwardsQty"] = row["InwardQty"].ToString();
                            workRow["OutwardsQty"] = row["OutwardQty"].ToString();
                            ClosingQty = ClosingQty + Convert.ToInt32(row["InwardQty"]) - Convert.ToInt32(row["OutwardQty"]);
                            workRow["ClosingQty"] = ClosingQty;
                            workRow["IDAT"] = row["IDAT"].ToString();
                            dt.Rows.Add(workRow);
                        }
                    }
                }       

                #region OpeningBalance   

                //Opening Balance
                //entity.IDAT = entity.FromDate;
                //DataSet ds = DAL.OpeningBalance(entity);
                //dt1 = ds.Tables[0];
                //dt2=ds.Tables[1];

                //if (dt1 != null)
                //{
                //    if (dt1.Rows.Count > 0)
                //    {                                                  
                //        workRow = dt.NewRow();                                                    
                //        workRow["WGPG_NAME"] = dt1.Rows[0]["WGPG_NAME"];
                //        workRow["SubCategoryName"] = dt1.Rows[0]["SubCategoryName"];                            
                //        workRow["Type"] = "Opening Balance";
                //        if (dt2 != null)
                //        {
                //            if (dt2.Rows.Count > 0)
                //            {
                //                workRow["InwardsQty"] = Convert.ToInt32(dt1.Rows[0]["InwardsQty"]) - Convert.ToInt32(dt2.Rows[0]["OutwardsQty"]);                                
                //            }
                //            else {
                //                workRow["InwardsQty"] = Convert.ToInt32(dt1.Rows[0]["InwardsQty"]);                                
                //            }                        
                //        }
                //        else
                //        {
                //            workRow["InwardsQty"] = Convert.ToInt32(dt1.Rows[0]["InwardsQty"]);                            
                //        }                        
                //        workRow["OutwardsQty"] = 0;
                //        ClosingQty = Convert.ToInt32(workRow["InwardsQty"]);                        
                //        workRow["ClosingQty"] = ClosingQty;                        
                //        workRow["IDAT"] = string.Empty;
                //        dt.Rows.Add(workRow);

                //    }
                //}
                #endregion

                //DateTime start = DateTime.Parse(entity.FromDate);
                //DateTime end = DateTime.Parse(entity.ToDate);
                //for (DateTime counter = start; counter <= end; counter = counter.AddDays(1))
                //{
                //    entity.IDAT = counter.ToString("yyyy-MM-dd");

                //    //Purchase 
                //    dt1 = null;
                //    dt1 = DAL.ProductHistoryPurchase(entity);

                //    if (dt1 != null)
                //    {
                //        if (dt1.Rows.Count > 0)
                //        {
                //            foreach (DataRow row in dt1.Rows)
                //            {
                //                workRow = dt.NewRow();
                //                workRow["WGPG_NAME"] = row["WGPG_NAME"].ToString();
                //                workRow["SubCategoryName"] = row["SubCategoryName"].ToString();
                //                workRow["Type"] = "Stock IN";
                //                workRow["InwardsQty"] = Convert.ToInt32(row["InwardsQty"]);
                //                workRow["OutwardsQty"] = 0;
                //                ClosingQty = ClosingQty + Convert.ToInt32(row["InwardsQty"]);
                //                workRow["ClosingQty"] = ClosingQty;
                //                workRow["IDAT"] = row["IDAT"].ToString();
                //                dt.Rows.Add(workRow);
                //            }
                //        }
                //    }                                     


                //    //Sale
                //    dt1 = null;
                //    dt1 = DAL.ProductHistorySale(entity);
                //    if (dt1 != null)
                //    {
                //        if (dt1.Rows.Count > 0)
                //        {
                //            foreach (DataRow row in dt1.Rows)
                //            {
                //                workRow = dt.NewRow();
                //                workRow["WGPG_NAME"] = row["WGPG_NAME"].ToString();
                //                workRow["SubCategoryName"] = row["SubCategoryName"].ToString();
                //                workRow["Type"] = "Sale";
                //                workRow["InwardsQty"] = 0;
                //                workRow["OutwardsQty"] = Convert.ToInt32(row["OutwardsQty"]);
                //                ClosingQty = ClosingQty - Convert.ToInt32(row["OutwardsQty"]);
                //                workRow["ClosingQty"] = ClosingQty;
                //                workRow["IDAT"] = row["IDAT"].ToString();
                //                dt.Rows.Add(workRow);
                //            }
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }


        public DataTable DeleteHistory(T_PROD entity)
        {
            try
            {
                return DAL.DeleteHistory(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public string[] EditValidation(T_PROD obj)
        {
            string[] valid = new string[2];
            try
            {
                if (obj.PROD_WGPG.ToString() == String.Empty || obj.PROD_WGPG.ToString() == "0")
                {
                    valid[0] = "False";
                    valid[1] = "PLEASE SELECT A PRODUCT CATEGORY";
                }
                else if (obj.PROD_SUBCATEGORY.ToString() == String.Empty || obj.PROD_SUBCATEGORY.ToString() == "0")
                {
                    valid[0] = "False";
                    valid[1] = "PLEASE SELECT A MODEL";
                }
                else if (obj.PROD_DES.ToString() == String.Empty || obj.PROD_DES.ToString() == "0")
                {
                    valid[0] = "False";
                    valid[1] = "PLEASE SELECT A COLOR/DESCRIPTION";
                }
                else if (obj.Branch.ToString() == String.Empty || obj.Branch.ToString() == "0")
                {
                    valid[0] = "False";
                    valid[1] = "PLEASE SELECT A BRANCH";
                }
                else if (obj.Vendor_ID.ToString() == String.Empty || obj.Vendor_ID.ToString() == "0")
                {
                    valid[0] = "False";
                    valid[1] = "PLEASE SELECT A Vendor";
                }
                else if (obj.CostPrice.ToString() == String.Empty)
                {
                    valid[0] = "False";
                    valid[1] = "PLEASE ENTER COST PRICE";
                }
                else if (obj.SalePrice.ToString() == String.Empty)
                {
                    valid[0] = "False";
                    valid[1] = "PLEASE ENTER SALE PRICE";
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

        public void purchaseReturn4Barcode(int StoreMasterStockOID, int T_STOCKOID, int T_PRODOID, int PROD_DESOID, string Branch, string Barcode, int PROD_WGPG, int PROD_SUBCATEGORY, int Vendor_ID, Int64 CostPrice, string IUSER)
        {
            
            try
            {
                DAL.purchaseReturn4BarcodeD(StoreMasterStockOID, T_STOCKOID, T_PRODOID, PROD_DESOID, Branch, Barcode, PROD_WGPG, PROD_SUBCATEGORY, Vendor_ID, CostPrice, IUSER);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Added By Yeasin 12-May-2019
        public void PurchaseReturnByBarcode(int StoreMasterStockOID, int T_STOCKOID, int T_PRODOID, int PROD_DESOID, string Branch, string Barcode, int PROD_WGPG, int PROD_SUBCATEGORY, int Vendor_ID, Int64 CostPrice, string IUSER)
        {

            try
            {
                DAL.PurchaseReturnByBarcode(StoreMasterStockOID, T_STOCKOID, T_PRODOID, PROD_DESOID, Branch, Barcode, PROD_WGPG, PROD_SUBCATEGORY, Vendor_ID, CostPrice, IUSER);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        // Added By Yeasin Ahmed 23-Jul-2019
        public DataTable RetailerDueReport(T_PROD entity)
        {
            try
            {
                return DAL.RetailerDueReport(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
