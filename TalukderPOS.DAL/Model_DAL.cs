using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.Common;
using System.Collections.Generic;
using TalukderPOS.BusinessObjects;
using System.Globalization;
using System.Security.Permissions;
using System.Threading;


namespace TalukderPOS.DAL
{
    public class Model_DAL
    {
        SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
        SqlDataAdapter da = new SqlDataAdapter();
        SqlCommand cmd;
        String sql;
        SqlDataReader reader;

        public void Add(Model_BO obj)
        {
            if (string.IsNullOrEmpty(obj.OID))
            {
                sql = "insert into SubCategory(CategoryID,SubCategoryName,Active,ShowOnDropdown,RunningModel,IUSER,IDAT,EUSER,EDAT) values (@CategoryID,@SubCategoryName,@Active,@ShowOnDropdown,@RunningModel,@IUSER,@IDAT,@EUSER,@EDAT)";
                cmd = new SqlCommand(sql, dbConnect);                
                cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 100).Value = obj.IUSER;
                cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = DateTime.Today.Date;
            }
            else
            {
                sql = "update SubCategory set CategoryID=@CategoryID,SubCategoryName=@SubCategoryName,Active=@Active,ShowOnDropdown=@ShowOnDropdown,RunningModel=@RunningModel,EUSER=@EUSER,EDAT=@EDAT where OID=" + obj.OID + " ";
                cmd = new SqlCommand(sql, dbConnect);                
            }
            cmd.Parameters.Add("@CategoryID", SqlDbType.BigInt).Value = Convert.ToInt64(obj.CategoryID);
            cmd.Parameters.Add("@SubCategoryName", SqlDbType.VarChar, 100).Value = obj.SubCategoryName;
            cmd.Parameters.Add("@Active", SqlDbType.VarChar, 1).Value = obj.Active;
            cmd.Parameters.Add("@ShowOnDropdown", SqlDbType.VarChar, 1).Value = obj.ShowOnDropdown;
            cmd.Parameters.Add("@RunningModel", SqlDbType.VarChar, 1).Value = obj.RunningModel;
            cmd.Parameters.Add("@EUSER", SqlDbType.VarChar, 100).Value = obj.EUSER;
            cmd.Parameters.Add("@EDAT", SqlDbType.Date).Value = DateTime.Today.Date;
            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbConnect.Close();
            }
        }



        public void Delete(Model_BO obj)
        {
            string found = string.Empty;

            //Check On T_PROD Table
            sql = "select T_PROD.OID from T_PROD where T_PROD.PROD_SUBCATEGORY=" + obj.OID + " and T_PROD.Quantity >0 ";
            cmd = new SqlCommand(sql, dbConnect);
            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (string.IsNullOrEmpty(reader["OID"].ToString()))
                        {
                            found = string.Empty;
                        }
                        else
                        {
                            found = reader["OID"].ToString();
                            break;
                        }
                    }
                }
                else
                {
                    found = string.Empty;
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbConnect.Close();
            }


            //Check On StoreMasterStock Table
            if (string.IsNullOrEmpty(found))
            {
                sql = "select StoreMasterStock.OID from StoreMasterStock where StoreMasterStock.PROD_SUBCATEGORY=" + obj.OID + " and StoreMasterStock.Quantity >0 and StoreMasterStock.SaleStatus=0 and StoreMasterStock.ActiveStatus=1";
                cmd = new SqlCommand(sql, dbConnect);
                try
                {
                    if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                    reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            if (string.IsNullOrEmpty(reader["OID"].ToString()))
                            {
                                found = string.Empty;
                            }
                            else
                            {
                                found = reader["OID"].ToString();
                                break;
                            }
                        }
                    }
                    else
                    {
                        found = string.Empty;
                    }
                   
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dbConnect.Close();
                }
            }



            if (string.IsNullOrEmpty(found))
            {
                sql = "update SubCategory set Active=@Active,EUSER=@EUSER,EDAT=@EDAT where OID=" + obj.OID + " ";
                cmd = new SqlCommand(sql, dbConnect);
                cmd.Parameters.Add("@Active", SqlDbType.VarChar, 1).Value = "0";
                cmd.Parameters.Add("@EUSER", SqlDbType.VarChar, 100).Value = obj.EUSER;
                cmd.Parameters.Add("@EDAT", SqlDbType.Date).Value = DateTime.Today.Date;
                try
                {
                    if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dbConnect.Close();
                }
            }
        }


        public DataTable BindList(Model_BO obj)
        {
            DataTable dt = new DataTable();
            List<String> myList = new List<String>();
            string WhereCondition = string.Empty;

            if (obj.CategoryID != String.Empty & obj.CategoryID != "0")
            {
                myList.Add("SubCategory.CategoryID = " + obj.CategoryID + " ");
            }
            if (obj.ShowOnDropdown != String.Empty & obj.ShowOnDropdown != "0")
            {
                myList.Add("SubCategory.ShowOnDropdown = '" + obj.ShowOnDropdown + "' ");
            }
            if (obj.RunningModel != String.Empty & obj.RunningModel != "0")
            {
                myList.Add("SubCategory.RunningModel = '" + obj.RunningModel + "' ");
            }
            if (obj.Shop_id != String.Empty & obj.Shop_id != "0")
            {
                myList.Add("T_WGPG.Shop_id = '" + obj.Shop_id + "' ");
            }

            myList.Add("SubCategory.Active = '1'");
            myList.Add("T_WGPG.WGPG_ACTV = '1'");

            string[] myArray = myList.ToArray();
            string where = string.Join(" and ", myArray);

            if (where == string.Empty)
            {
                WhereCondition = string.Empty;
            }
            else
            {
                WhereCondition = "where " + where + " ";
            }
            sql = "select SubCategory.OID,T_WGPG.WGPG_NAME,SubCategory.SubCategoryName,SubCategory.ShowOnDropdown,SubCategory.RunningModel from SubCategory left join T_WGPG on SubCategory.CategoryID=T_WGPG.OID " + WhereCondition + " order by T_WGPG.WGPG_NAME,SubCategory.SubCategoryName";
            cmd = new SqlCommand(sql, dbConnect);
            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                da.SelectCommand = cmd;
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbConnect.Close();
            }
            return dt;
        }



        public Model_BO GetById(string OID)
        {
            Model_BO obj = new Model_BO();
            sql = "select OID,CategoryID,SubCategoryName,ShowOnDropdown,RunningModel from SubCategory where OID=" + OID + " ";
            cmd = new SqlCommand(sql, dbConnect);
            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (string.IsNullOrEmpty(reader["OID"].ToString()))
                    {
                        obj.OID = string.Empty;
                    }
                    else
                    {
                        obj.OID = reader["OID"].ToString();
                    }

                    if (string.IsNullOrEmpty(reader["CategoryID"].ToString()))
                    {
                        obj.CategoryID = string.Empty;
                    }
                    else
                    {
                        obj.CategoryID = reader["CategoryID"].ToString();
                    }

                    if (string.IsNullOrEmpty(reader["SubCategoryName"].ToString()))
                    {
                        obj.SubCategoryName = string.Empty;
                    }
                    else
                    {
                        obj.SubCategoryName = reader["SubCategoryName"].ToString();
                    }

                    if (string.IsNullOrEmpty(reader["ShowOnDropdown"].ToString()))
                    {
                        obj.ShowOnDropdown = string.Empty;
                    }
                    else
                    {
                        obj.ShowOnDropdown = reader["ShowOnDropdown"].ToString();
                    }

                    if (string.IsNullOrEmpty(reader["RunningModel"].ToString()))
                    {
                        obj.RunningModel = string.Empty;
                    }
                    else
                    {
                        obj.RunningModel = reader["RunningModel"].ToString();
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbConnect.Close();
            }
            return obj;
        }


      



    }
}
