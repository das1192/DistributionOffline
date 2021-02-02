using AjaxControlToolkit;
using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Web.Script.Services;
using System.Configuration;
using System.Data;


namespace WebApplication1
{
    [ScriptService]
    /// <summary>
    /// Summary description for DropdownWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class DropdownWebService : System.Web.Services.WebService
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
        SqlConnection conn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str1"].ConnectionString);
        SqlCommand cmd;
        string storeid = "";





        [WebMethod(EnableSession = true)]
        public CascadingDropDownNameValue[] BindCategory(string knownCategoryValues, string category)
        {
            string storeid = HttpContext.Current.Session["StoreID"].ToString();
            
            conn.Open();
            cmd = new SqlCommand("select OID,WGPG_NAME from T_WGPG where WGPG_ACTV=1 AND Shop_id=" + storeid + " order by WGPG_NAME", conn);
            cmd.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            //create list and add items in it by looping through dataset table
            List<CascadingDropDownNameValue> categorydetails = new List<CascadingDropDownNameValue>();            
            foreach (DataRow dtrow in ds.Tables[0].Rows)
            {
                string OID = dtrow["OID"].ToString();
                string WGPG_NAME = dtrow["WGPG_NAME"].ToString();
                categorydetails.Add(new CascadingDropDownNameValue(WGPG_NAME, OID));
            }
            return categorydetails.ToArray();
        }

        

        [WebMethod]
        public CascadingDropDownNameValue[] BindCategory5(string knownCategoryValues, string category)
        {
            conn.Open();
            cmd = new SqlCommand("select OID,WGPG_NAME from T_WGPG where WGPG_ACTV=1 AND OID='111' order by WGPG_NAME", conn);
            cmd.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            //create list and add items in it by looping through dataset table
            List<CascadingDropDownNameValue> categorydetails = new List<CascadingDropDownNameValue>();
            foreach (DataRow dtrow in ds.Tables[0].Rows)
            {
                string OID = dtrow["OID"].ToString();
                string WGPG_NAME = dtrow["WGPG_NAME"].ToString();
                categorydetails.Add(new CascadingDropDownNameValue(WGPG_NAME, OID));
            }
            return categorydetails.ToArray();
        }
        [WebMethod]
        public CascadingDropDownNameValue[] BindModel(string knownCategoryValues, string category)
        {
            //This method will return a StringDictionary containing the name/value pairs of the currently selected values
            StringDictionary categorydetails = AjaxControlToolkit.CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
            int CategoryID = Convert.ToInt32(categorydetails["Category"]);

            conn.Open();
            cmd = new SqlCommand("select OID,SubCategoryName from SubCategory where CategoryID=@CategoryID and Active=1 and ShowOnDropdown='Y' order by SubCategoryName", conn);
            cmd.Parameters.AddWithValue("@CategoryID", CategoryID);
            cmd.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            //create list and add items in it by looping through dataset table
            List<CascadingDropDownNameValue> modeldetails = new List<CascadingDropDownNameValue>();
            foreach (DataRow dtrow in ds.Tables[0].Rows)
            {
                string OID = dtrow["OID"].ToString();
                string SubCategoryName = dtrow["SubCategoryName"].ToString();
                modeldetails.Add(new CascadingDropDownNameValue(SubCategoryName, OID));
            }
            return modeldetails.ToArray();
        }


        [WebMethod]
        public CascadingDropDownNameValue[] BindDescription(string knownCategoryValues, string category)
        {
            //This method will return a StringDictionary containing the name/value pairs of the currently selected values
            StringDictionary modeldetails = AjaxControlToolkit.CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
            int SubCategoryID = Convert.ToInt32(modeldetails["Model"]);

            conn.Open();
            cmd = new SqlCommand("select OID,Description from Description where SubCategoryID=@SubCategoryID and Active=1 order by Description", conn);
            cmd.Parameters.AddWithValue("@SubCategoryID", SubCategoryID);
            cmd.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            //create list and add items in it by looping through dataset table
            List<CascadingDropDownNameValue> descriptiondetails = new List<CascadingDropDownNameValue>();
            foreach (DataRow dtrow in ds.Tables[0].Rows)
            {
                string OID = dtrow["OID"].ToString();
                string Description = dtrow["Description"].ToString();
                descriptiondetails.Add(new CascadingDropDownNameValue(Description, OID));
            }
            return descriptiondetails.ToArray();
        }

        [WebMethod]
        public CascadingDropDownNameValue[] BindBranch(string knownCategoryValues, string category)
        {
            conn.Open();
            cmd = new SqlCommand("SELECT OID,CCOM_NAME FROM T_CCOM where CCOM_ACTV=1", conn);
            cmd.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            //create list and add items in it by looping through dataset table
            List<CascadingDropDownNameValue> branchdetails = new List<CascadingDropDownNameValue>();
            foreach (DataRow dtrow in ds.Tables[0].Rows)
            {
                string OID = dtrow["OID"].ToString();
                string CCOM_NAME = dtrow["CCOM_NAME"].ToString();
                branchdetails.Add(new CascadingDropDownNameValue(CCOM_NAME, OID));
            }
            return branchdetails.ToArray();
        }

        [WebMethod(EnableSession = true)]
        public CascadingDropDownNameValue[] BindShop(string knownCategoryValues, string category)
        {
            string storeid = HttpContext.Current.Session["StoreID"].ToString();
            conn.Open();
            if (storeid == "3")
            {
                cmd = new SqlCommand("SELECT OID,ShopName FROM ShopInfo where ActiveStatus=1", conn);

            }
            else
            {
                cmd = new SqlCommand("SELECT OID,ShopName FROM ShopInfo where ActiveStatus=1 AND OID=" + storeid + " ", conn);
            }
            cmd.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            //create list and add items in it by looping through dataset table
            List<CascadingDropDownNameValue> Shopdetails = new List<CascadingDropDownNameValue>();
            foreach (DataRow dtrow in ds.Tables[0].Rows)
            {
                string OID = dtrow["OID"].ToString();
                string CCOM_NAME = dtrow["ShopName"].ToString();
                Shopdetails.Add(new CascadingDropDownNameValue(CCOM_NAME, OID));
            }
            return Shopdetails.ToArray();
        }

        [WebMethod(EnableSession = true)]
        public CascadingDropDownNameValue[] BindVendor(string knownCategoryValues, string category)
        {
            string storeid = HttpContext.Current.Session["StoreID"].ToString();
            conn.Open();
            if (storeid == "3")
            {
                cmd = new SqlCommand("SELECT OID,Vendor_Name FROM Vendor where Vendor_Active=1", conn);

            }
            else
            {
                cmd = new SqlCommand("SELECT OID,Vendor_Name FROM Vendor where Vendor_Active=1 AND Shop_id=" + storeid + " ", conn);
            }
            cmd.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            //create list and add items in it by looping through dataset table
            List<CascadingDropDownNameValue> Vendordetails = new List<CascadingDropDownNameValue>();
            foreach (DataRow dtrow in ds.Tables[0].Rows)
            {
                string OID = dtrow["OID"].ToString();
                string CCOM_NAME = dtrow["Vendor_Name"].ToString();
                Vendordetails.Add(new CascadingDropDownNameValue(CCOM_NAME, OID));
            }
            return Vendordetails.ToArray();
        }
        // BINDING NEW DROPDOWN FOR THE CUSTOMER LIST

        [WebMethod(EnableSession = true)]
        public CascadingDropDownNameValue[] BindCustomer(string knownCategoryValues, string category)
        {
            string storeid = HttpContext.Current.Session["StoreID"].ToString();
            conn.Open();
           
                cmd = new SqlCommand("SELECT ID,Name,Number FROM Customers where Branch=" + storeid + " ;", conn);
            
            cmd.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            //create list and add items in it by looping through dataset table
            List<CascadingDropDownNameValue> CustomerDetails = new List<CascadingDropDownNameValue>();
            foreach (DataRow dtrow in ds.Tables[0].Rows)
            {
                string Num = dtrow["Number"].ToString();
                string Customer_Name = dtrow["Name"].ToString();
                string fullnamenumber = Customer_Name + "-"+ Num;
                string OID = dtrow["ID"].ToString();
                CustomerDetails.Add(new CascadingDropDownNameValue(fullnamenumber, OID));
            }
            return CustomerDetails.ToArray();
        }




        //END OF BINDING LIST

        [WebMethod(EnableSession = true)]
        public CascadingDropDownNameValue[] BindCostingHead(string knownCategoryValues, string category)
        {
            string storeid = HttpContext.Current.Session["StoreID"].ToString();
            conn.Open();
            if (storeid == "3")
            {
                cmd = new SqlCommand("SELECT OID,CostingHead FROM CostingHead where CostingHead not in ('Product Missing','Discount On Sales','Expense For Gift')", conn);

            }
            else
            {
                //180102   cmd = new SqlCommand("SELECT OID,CostingHead FROM CostingHead where Shop_id=" + storeid + " ", conn);
                cmd = new SqlCommand("SELECT OID,CostingHead FROM CostingHead where CostingHead not in ('Product Missing','Discount On Sales','Expense For Gift') and Shop_id=" + storeid + " ", conn);
            }
            cmd.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            //create list and add items in it by looping through dataset table
            List<CascadingDropDownNameValue> Costingdetails = new List<CascadingDropDownNameValue>();
            foreach (DataRow dtrow in ds.Tables[0].Rows)
            {
                string OID = dtrow["OID"].ToString();
                string CostingHead = dtrow["CostingHead"].ToString();
                Costingdetails.Add(new CascadingDropDownNameValue(CostingHead, OID));
            }
            return Costingdetails.ToArray();
        }

        [WebMethod]
        public CascadingDropDownNameValue[] BindBranchNew(string knownCategoryValues, string category)
        {
            conn.Open();
            cmd = new SqlCommand("SELECT OID,CCOM_NAME FROM T_CCOM where CCOM_ACTV=1 and OID <>'CCOMxxxx01'", conn);
            cmd.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            //create list and add items in it by looping through dataset table
            List<CascadingDropDownNameValue> branchdetails = new List<CascadingDropDownNameValue>();
            foreach (DataRow dtrow in ds.Tables[0].Rows)
            {
                string OID = dtrow["OID"].ToString();
                string CCOM_NAME = dtrow["CCOM_NAME"].ToString();
                branchdetails.Add(new CascadingDropDownNameValue(CCOM_NAME, OID));
            }
            return branchdetails.ToArray();
        }
        [WebMethod]
        public CascadingDropDownNameValue[] BindBranch1(string knownCategoryValues, string category)
        {
            //This method will return a StringDictionary containing the name/value pairs of the currently selected values
            StringDictionary modeldetails = AjaxControlToolkit.CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
            int CategoryID = Convert.ToInt32(modeldetails["Category"]);

            conn.Open();
            if (CategoryID == 111)
            {
                cmd = new SqlCommand("SELECT OID,CCOM_NAME FROM T_CCOM where CCOM_ACTV=1 and OID='CCOMxxxx01'", conn);
            }
            else
            {
                cmd = new SqlCommand("SELECT OID,CCOM_NAME FROM T_CCOM where CCOM_ACTV=1 and OID <>'CCOMxxxx01'", conn);
            }

            cmd.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            //create list and add items in it by looping through dataset table
            List<CascadingDropDownNameValue> branchdetails = new List<CascadingDropDownNameValue>();
            foreach (DataRow dtrow in ds.Tables[0].Rows)
            {
                string OID = dtrow["OID"].ToString();
                string CCOM_NAME = dtrow["CCOM_NAME"].ToString();
                branchdetails.Add(new CascadingDropDownNameValue(CCOM_NAME, OID));
            }
            return branchdetails.ToArray();
        }

        [WebMethod]
        public CascadingDropDownNameValue[] BindBranch2(string knownCategoryValues, string category)
        {
            //This method will return a StringDictionary containing the name/value pairs of the currently selected values
            StringDictionary modeldetails = AjaxControlToolkit.CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
            int CategoryID = Convert.ToInt32(modeldetails["Category"]);

            conn.Open();
            if (CategoryID == 111)
            {
                cmd = new SqlCommand("SELECT OID,CCOM_NAME FROM T_CCOM where CCOM_ACTV=1 and OID <>'CCOMxxxx01'", conn);
            }

            cmd.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            //create list and add items in it by looping through dataset table
            List<CascadingDropDownNameValue> branchdetails = new List<CascadingDropDownNameValue>();
            foreach (DataRow dtrow in ds.Tables[0].Rows)
            {
                string OID = dtrow["OID"].ToString();
                string CCOM_NAME = dtrow["CCOM_NAME"].ToString();
                branchdetails.Add(new CascadingDropDownNameValue(CCOM_NAME, OID));
            }
            return branchdetails.ToArray();
        }


        [WebMethod]
        public CascadingDropDownNameValue[] BindCategory1(string knownCategoryValues, string category)
        {
            conn.Open();
            cmd = new SqlCommand("select OID,WGPG_NAME from T_WGPG where WGPG_ACTV=1 and OID=111 order by WGPG_NAME", conn);
            cmd.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            //create list and add items in it by looping through dataset table
            List<CascadingDropDownNameValue> categorydetails = new List<CascadingDropDownNameValue>();
            foreach (DataRow dtrow in ds.Tables[0].Rows)
            {
                string OID = dtrow["OID"].ToString();
                string WGPG_NAME = dtrow["WGPG_NAME"].ToString();
                categorydetails.Add(new CascadingDropDownNameValue(WGPG_NAME, OID));
            }
            return categorydetails.ToArray();
        }


        [WebMethod]
        public CascadingDropDownNameValue[] BindCategory2(string knownCategoryValues, string category)
        {
            conn.Open();
            cmd = new SqlCommand("select OID,WGPG_NAME from T_WGPG where WGPG_ACTV=1 and OID <>111 order by WGPG_NAME", conn);
            cmd.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            //create list and add items in it by looping through dataset table
            List<CascadingDropDownNameValue> categorydetails = new List<CascadingDropDownNameValue>();
            foreach (DataRow dtrow in ds.Tables[0].Rows)
            {
                string OID = dtrow["OID"].ToString();
                string WGPG_NAME = dtrow["WGPG_NAME"].ToString();
                categorydetails.Add(new CascadingDropDownNameValue(WGPG_NAME, OID));
            }
            return categorydetails.ToArray();
        }


        [WebMethod]
        public CascadingDropDownNameValue[] BindDiscountType(string knownCategoryValues, string category)
        {
            conn.Open();
            cmd = new SqlCommand("select OID,DiscountType from DiscountType where ActiveStatus=1 order by DiscountType", conn);
            cmd.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            //create list and add items in it by looping through dataset table
            List<CascadingDropDownNameValue> DiscountTypedetails = new List<CascadingDropDownNameValue>();
            foreach (DataRow dtrow in ds.Tables[0].Rows)
            {
                string OID = dtrow["OID"].ToString();
                string DiscountType = dtrow["DiscountType"].ToString();
                DiscountTypedetails.Add(new CascadingDropDownNameValue(DiscountType, OID));
            }
            return DiscountTypedetails.ToArray();
        }

        [WebMethod]
        public CascadingDropDownNameValue[] Bindbank(string knownCategoryValues, string category)
        {
            conn.Open();
            cmd = new SqlCommand("select OID,BankName from BankInfo where ActiveStatus=1", conn);
            cmd.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            //create list and add items in it by looping through dataset table
            List<CascadingDropDownNameValue> bankdetails = new List<CascadingDropDownNameValue>();
            foreach (DataRow dtrow in ds.Tables[0].Rows)
            {
                string OID = dtrow["OID"].ToString();
                string BankName = dtrow["BankName"].ToString();
                bankdetails.Add(new CascadingDropDownNameValue(BankName, OID));
            }
            return bankdetails.ToArray();
        }

        [WebMethod]
        public CascadingDropDownNameValue[] BindPaymentMode(string knownCategoryValues, string category)
        {
            conn.Open();
            cmd = new SqlCommand("select OID,PaymentMode from PaymentMode where OID <> 11 and OID <> 13", conn);
            cmd.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            //create list and add items in it by looping through dataset table
            List<CascadingDropDownNameValue> PaymentModedetails = new List<CascadingDropDownNameValue>();
            foreach (DataRow dtrow in ds.Tables[0].Rows)
            {
                string OID = dtrow["OID"].ToString();
                string PaymentMode = dtrow["PaymentMode"].ToString();
                PaymentModedetails.Add(new CascadingDropDownNameValue(PaymentMode, OID));
            }
            return PaymentModedetails.ToArray();
        }
        
        [WebMethod]
        public CascadingDropDownNameValue[] BindReference(string knownCategoryValues, string category)
        {
            //This method will return a StringDictionary containing the name/value pairs of the currently selected values
            StringDictionary categorydetails = AjaxControlToolkit.CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
            int DiscountTypeOID = Convert.ToInt32(categorydetails["RefType"]);
            conn.Open();
            cmd = new SqlCommand("select OID,Reference from DiscountReference where DiscountTypeOID=@DiscountTypeOID and ActiveStatus=1 order by Reference", conn);
            cmd.Parameters.AddWithValue("@DiscountTypeOID", DiscountTypeOID);
            cmd.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            //create list and add items in it by looping through dataset table
            List<CascadingDropDownNameValue> PaymentModedetails = new List<CascadingDropDownNameValue>();
            foreach (DataRow dtrow in ds.Tables[0].Rows)
            {
                string OID = dtrow["OID"].ToString();
                string PaymentMode = dtrow["Reference"].ToString();
                PaymentModedetails.Add(new CascadingDropDownNameValue(PaymentMode, OID));
            }
            return PaymentModedetails.ToArray();
        }

        [WebMethod]
        public CascadingDropDownNameValue[] BindDays(string knownCategoryValues, string category)
        {
            int i;
            List<CascadingDropDownNameValue> totalDayCount = new List<CascadingDropDownNameValue>();
            for (i = 1; i < 31; i++)
            {
                string days = i.ToString();
                string sDays = i.ToString();
                totalDayCount.Add(new CascadingDropDownNameValue(sDays, days));
            }
            return totalDayCount.ToArray();
        }



        //************* SIS Method ********************//
        [WebMethod]
        public CascadingDropDownNameValue[] BindCategorySIS(string knownCategoryValues, string category)
        {
            conn1.Open();
            cmd = new SqlCommand("select OID,WGPG_NAME from T_WGPG where WGPG_ACTV=1 and OID not in(111,119,121,122,124,125,126,127,128,129,130) order by OID", conn1);
            cmd.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn1.Close();
            //create list and add items in it by looping through dataset table
            List<CascadingDropDownNameValue> categorydetails = new List<CascadingDropDownNameValue>();
            foreach (DataRow dtrow in ds.Tables[0].Rows)
            {
                string OID = dtrow["OID"].ToString();
                string WGPG_NAME = dtrow["WGPG_NAME"].ToString();
                categorydetails.Add(new CascadingDropDownNameValue(WGPG_NAME, OID));
            }
            return categorydetails.ToArray();
        }


        [WebMethod]
        public CascadingDropDownNameValue[] BindModelSIS(string knownCategoryValues, string category)
        {
            //This method will return a StringDictionary containing the name/value pairs of the currently selected values
            StringDictionary categorydetails = AjaxControlToolkit.CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
            int CategoryID = Convert.ToInt32(categorydetails["Category"]);

            conn1.Open();
            cmd = new SqlCommand("select OID,SubCategoryName from SubCategory where CategoryID=@CategoryID and Active=1 order by SubCategoryName", conn1);
            cmd.Parameters.AddWithValue("@CategoryID", CategoryID);
            cmd.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn1.Close();
            //create list and add items in it by looping through dataset table
            List<CascadingDropDownNameValue> modeldetails = new List<CascadingDropDownNameValue>();
            foreach (DataRow dtrow in ds.Tables[0].Rows)
            {
                string OID = dtrow["OID"].ToString();
                string SubCategoryName = dtrow["SubCategoryName"].ToString();
                modeldetails.Add(new CascadingDropDownNameValue(SubCategoryName, OID));
            }
            return modeldetails.ToArray();
        }


        [WebMethod]
        public CascadingDropDownNameValue[] BindDescriptionSIS(string knownCategoryValues, string category)
        {
            //This method will return a StringDictionary containing the name/value pairs of the currently selected values
            StringDictionary modeldetails = AjaxControlToolkit.CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
            int SubCategoryID = Convert.ToInt32(modeldetails["Model"]);

            conn1.Open();
            cmd = new SqlCommand("select OID,Description from Description where SubCategoryID=@SubCategoryID and Active=1 order by Description", conn1);
            cmd.Parameters.AddWithValue("@SubCategoryID", SubCategoryID);
            cmd.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn1.Close();
            //create list and add items in it by looping through dataset table
            List<CascadingDropDownNameValue> descriptiondetails = new List<CascadingDropDownNameValue>();
            foreach (DataRow dtrow in ds.Tables[0].Rows)
            {
                string OID = dtrow["OID"].ToString();
                string Description = dtrow["Description"].ToString();
                descriptiondetails.Add(new CascadingDropDownNameValue(Description, OID));
            }
            return descriptiondetails.ToArray();
        }

        [WebMethod]
        public CascadingDropDownNameValue[] BindBranchSIS(string knownCategoryValues, string category)
        {
            conn1.Open();
            cmd = new SqlCommand("SELECT OID,CCOM_NAME FROM T_CCOM where CCOM_ACTV=1 and OID not in('CCOMxxxx01')", conn1);
            cmd.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn1.Close();
            //create list and add items in it by looping through dataset table
            List<CascadingDropDownNameValue> branchdetails = new List<CascadingDropDownNameValue>();
            foreach (DataRow dtrow in ds.Tables[0].Rows)
            {
                string OID = dtrow["OID"].ToString();
                string CCOM_NAME = dtrow["CCOM_NAME"].ToString();
                branchdetails.Add(new CascadingDropDownNameValue(CCOM_NAME, OID));
            }
            return branchdetails.ToArray();
        }
        //************* END Method *******************//

        // sadiq 170919
        //GetOrderNoOSCode OSD
        [WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public string[] GetDescription(string prefixText, string contextKey)
        {
            //string a= Session["UserID"].ToString();
                

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
            string sql = string.Empty;
            if (!string.IsNullOrEmpty(contextKey))
            {
                sql = string.Format(@"
select
StoreMasterStock.OID,StoreMasterStock.PROD_WGPG,StoreMasterStock.PROD_SUBCATEGORY,StoreMasterStock.PROD_DES AS DescriptionID
,StoreMasterStock.Branch as StoreID,Barcode='',
StoreMasterStock.SalePrice

,(StoreMasterStock.Quantity 
- StoreMasterStock.SaleQuantity
-ISNULL((select COUNT (tp.OID ) from T_PROD tp 
where tp.Branch =StoreMasterStock.Branch and tp.SaleStatus ='0' and tp.PROD_DES =StoreMasterStock .PROD_DES 
group by tp.PROD_DES),0)
)as Stock
,1 Qty,(1*StoreMasterStock.SalePrice) TotalPrice,0 as RefAmount
,T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,Description.Description
,Flag='Description'

from StoreMasterStock,T_WGPG,SubCategory,Description

Where
StoreMasterStock.PROD_DES=Description.OID AND 
StoreMasterStock.Quantity >0 and
(StoreMasterStock.Quantity - StoreMasterStock.SaleQuantity) >0 and
StoreMasterStock.ActiveStatus=1 and
StoreMasterStock.PROD_WGPG=T_WGPG.OID and
StoreMasterStock.PROD_SUBCATEGORY=SubCategory.OID 
and (StoreMasterStock.Quantity 
- StoreMasterStock.SaleQuantity
-ISNULL((select COUNT (tp.OID ) from T_PROD tp 
where tp.Branch =StoreMasterStock.Branch and tp.SaleStatus ='0' and tp.PROD_DES =StoreMasterStock .PROD_DES 
group by tp.PROD_DES),0))>0

{0} 
AND Description.Description like '%{1}%'
", contextKey, prefixText);

                #region OLD
                //                sql = string.Format(@"
//select t.Description from (
//
//select sms.OID AS SMSOID,s.ACC_STOCKID as ACCOID ,c.WGPG_NAME as PROD_WGPG
//,sc.SubCategoryName as PROD_SUBCATEGORY
//,d.Description,'' as Barcode
//,(s.Quantity-
//isnull(
//(select SUM(tp.Quantity) from T_PROD tp 
//where tp.SaleStatus ='0' AND tp.CostPrice = s.CostPrice          --where tp.SaleStatus ='0'
//and tp.Branch=s.Branch and tp.PROD_WGPG=c.OID and tp.PROD_SUBCATEGORY =sc.OID and tp.PROD_DES=d.OID 
//)
//,0)
//)  as Quantity
//,s.CostPrice ,
//c.OID as CategoryID,sc.OID as SubCategoryID,d.OID AS DescriptionID,s.Branch
//
//,AVERAGE = (select 
//
//convert(decimal(18,2),
//(SUM(Quantity * CostPrice ))/convert(decimal(18,2),(SUM(Quantity)))
//)
//
//from Acc_Stock where Quantity >0 AND Acc_Stock.PROD_DES = d.OID group by Acc_Stock.PROD_DES
//)
//
//from Acc_Stock s 
//inner join Description d on s.PROD_DES=d.OID 
//inner join StoreMasterStock sms on sms.PROD_DES= d.OID 
//inner join SubCategory sc on d.SubCategoryID=sc.OID 
//inner join T_WGPG c on sc.CategoryID=c.OID 
//
//where  (s.Quantity-
//isnull(
//(select SUM(tp.Quantity) from T_PROD tp 
//where tp.SaleStatus ='0' AND tp.CostPrice = s.CostPrice          --where tp.SaleStatus ='0'
//and tp.Branch=s.Branch and tp.PROD_WGPG=c.OID and tp.PROD_SUBCATEGORY =sc.OID and tp.PROD_DES=d.OID 
//)
//,0)
//)>0  
// {0}  and d.Description like '%{1}%'
// 
// group by sms.OID,s.ACC_STOCKID,c.WGPG_NAME,sc.SubCategoryName,d.Description,s.Quantity,s.CostPrice,c.OID,sc.OID,d.OID,s.Branch
//)t
//order by t.Description
//", contextKey, prefixText);


                //                sql = string.Format(@"
                //select c.WGPG_NAME,sc.SubCategoryName,(d.Description +':' +CONVERT(nvarchar(100),d.OID)) as Description,d.OID as id
                //,ROW_NUMBER()over(partition by sc.SubCategoryName order by d.Description) SL--,d.OID,d.SubCategoryID,sc.CategoryID
                //from [Description] d
                //inner join SubCategory sc on sc.OID=d.SubCategoryID
                //inner join T_WGPG c on c.OID=sc.CategoryID
                //inner join [User] u on u.UserId=d.IUSER
                //where d.Description like '%{0}%'  {1}
                //
                //order by d.Description
                //", prefixText, contextKey); 
                #endregion

            }
            SqlDataAdapter da = new SqlDataAdapter(sql, con);
            DataTable DTLocal = new DataTable();
            da.Fill(DTLocal);
            string[] items = new string[DTLocal.Rows.Count];
            int i = 0;
            foreach (DataRow dr in DTLocal.Rows)
            {
                items.SetValue(dr["Description"].ToString(), i);
                i++;
            }
            return items;
        }//

        // sadiq 170919
        //GetOrderNoOSCode OSD
        [WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public string[] GetBarcode(string prefixText, string contextKey)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
            string sql = string.Empty;
            if (contextKey == null || contextKey == "")
            {
                sql = string.Format(@"
select Barcode, (p.Barcode + ':' + d.Description + ':' + CONVERT(nvarchar(100),p.OID)) as listItem --, p.* 
from T_PROD p 
inner join Description d on d.OID=p.PROD_DES
where ISNULL(p.Barcode,'') !='' and SaleStatus=0 and p.Branch='{0}' and p.Barcode like '%{1}%'
order by p.Barcode
", Session["StoreID"].ToString(), prefixText);
            }
            else
            {
//                sql = string.Format(@"
//select Barcode, (p.Barcode + ':' + d.Description + ':' + CONVERT(nvarchar(100),p.OID)) as listItem --, p.* 
//from T_PROD p 
//inner join Description d on d.OID=p.PROD_DES
//where ISNULL(p.Barcode,'') !='' and SaleStatus=0 and p.Branch='{0}' and p.Barcode like '%{1}%'
//order by p.Barcode
//", Session["StoreID"].ToString(), prefixText);

            }
            SqlDataAdapter da = new SqlDataAdapter(sql, con);
            DataTable DTLocal = new DataTable();
            da.Fill(DTLocal);
            string[] items = new string[DTLocal.Rows.Count];
            int i = 0;
            foreach (DataRow dr in DTLocal.Rows)
            {
                items.SetValue(dr["listItem"].ToString(), i);
                i++;
            }
            return items;
        }//

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public string[]  GetRetailerNames(string prefixText,string contextKey)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
            string sql = string.Empty;
            
                sql = string.Format(@"

Select Cast(ID as nvarchar(10))+':'+Name+' (0'+Cast(Number as nvarchar(15))+')' as Text From Customers Where CustomerStatus=1 And Branch='{0}'
And Name Like @prefixText
", contextKey);
                //Session["StoreID"].ToString()
            

            SqlDataAdapter da = new SqlDataAdapter(sql, con);
            da.SelectCommand.Parameters.AddWithValue("@prefixText", "%" + prefixText + "%");
            
            DataTable DTLocal = new DataTable();
            da.Fill(DTLocal);
            string[] items = new string[DTLocal.Rows.Count];
            int i = 0;
            foreach (DataRow dr in DTLocal.Rows)
            {
                items.SetValue(dr["Text"].ToString(), i);
                i++;
            }
            return items;
        }//

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public string[] GetInvoices(string prefixText, string contextKey)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
            string sql = string.Empty;

            sql = string.Format(@"Select InvoiceNo From T_SALES_MST Where DropStatus=0 And StoreID='{0}' And InvoiceNo Like @prefixText", contextKey);

            SqlDataAdapter da = new SqlDataAdapter(sql, con);
            da.SelectCommand.Parameters.AddWithValue("@prefixText", "%" + prefixText + "%");

            DataTable DTLocal = new DataTable();
            da.Fill(DTLocal);
            string[] items = new string[DTLocal.Rows.Count];
            int i = 0;
            foreach (DataRow dr in DTLocal.Rows)
            {
                items.SetValue(dr["InvoiceNo"].ToString(), i);
                i++;
            }
            return items;
        }//

        [WebMethod(EnableSession = true)]
        public CascadingDropDownNameValue[] BindRetailer(string knownCategoryValues, string category)
        {
            string storeid = HttpContext.Current.Session["StoreID"].ToString();
            conn.Open();
            if (storeid == "3")
            {
                cmd = new SqlCommand(string.Format(@"Select ID,(Name+' (0'+Cast(Number as varchar(50))+')') as Name from Customers 
Where CustomerStatus=1 Order By Name Desc"), conn);

            }
            else
            {
                cmd = new SqlCommand(string.Format(@"Select ID,(Name+' (0'+Cast(Number as varchar(50))+')') as Name from Customers 
Where CustomerStatus=1 And Branch=" + storeid + " Order By Name Desc"), conn);
            }
            cmd.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            //create list and add items in it by looping through dataset table
            List<CascadingDropDownNameValue> Retailerdetails = new List<CascadingDropDownNameValue>();
            foreach (DataRow dtrow in ds.Tables[0].Rows)
            {
                string OID = dtrow["ID"].ToString();
                string CCOM_NAME = dtrow["Name"].ToString();
                Retailerdetails.Add(new CascadingDropDownNameValue(CCOM_NAME, OID));
            }
            return Retailerdetails.ToArray();
        }

        //^^^^^^^^^^^^^^
    }
}
