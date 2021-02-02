using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using TalukderPOS.BusinessObjects;
using System.Configuration;

namespace TalukderPOS.DAL
{
    public class ProductTransferToSIS_DAL
    {
        SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
        SqlConnection dbConnect1 = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str1"].ConnectionString);
        SqlCommand cmd;
        SqlDataAdapter da = new SqlDataAdapter();
        String sql;
        SqlDataReader reader;        

        public void Add(T_PROD entity)
        {
            //Insert Into SIS
            sql = "INSERT INTO StoreMasterStock(PROD_WGPG,PROD_SUBCATEGORY,PROD_DES,Branch,Barcode,CostPrice,SalePrice,Quantity,SaleStatus,ActiveStatus,IUSER,IDAT) values(@PROD_WGPG,@PROD_SUBCATEGORY,@PROD_DES,@Branch,@Barcode,@CostPrice,@SalePrice,@Quantity,@SaleStatus,@ActiveStatus,@IUSER,@IDAT)";
            cmd = new SqlCommand(sql, dbConnect1);
            cmd.Parameters.Add("@PROD_WGPG", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_WGPG);
            cmd.Parameters.Add("@PROD_SUBCATEGORY", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_SUBCATEGORY);
            cmd.Parameters.Add("@PROD_DES", SqlDbType.BigInt).Value = Convert.ToInt64(entity.PROD_DES);
            cmd.Parameters.Add("@Branch", SqlDbType.VarChar, 50).Value = entity.Branch;
            cmd.Parameters.Add("@Barcode", SqlDbType.VarChar, 100).Value = entity.Barcode;
            cmd.Parameters.Add("@CostPrice", SqlDbType.BigInt).Value = Convert.ToInt64(entity.CostPrice);
            cmd.Parameters.Add("@SalePrice", SqlDbType.BigInt).Value = Convert.ToInt64(entity.SalePrice);
            cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = Convert.ToInt32(entity.Quantity);
            cmd.Parameters.Add("@SaleStatus", SqlDbType.Int).Value = 0;
            cmd.Parameters.Add("@ActiveStatus", SqlDbType.Int).Value = 1;
            cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 50).Value = entity.IUSER;
            cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = DateTime.Now.Date;
            try
            {
                if (dbConnect1.State == ConnectionState.Closed) dbConnect1.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbConnect1.Close();
            }


            //Update SES
            string BranchOID = SenderBranch(entity.Barcode);
            string DescriptionOID = SenderPROD_DES(entity.Barcode);
            sql = "INSERT INTO StockPosting(BranchOID,DescriptionOID,Barcode,InwardQty,OutwardQty,Particulars,Remarks,IUSER,IDAT) values(@BranchOID,@DescriptionOID,@Barcode,@InwardQty,@OutwardQty,@Particulars,@Remarks,@IUSER,@IDAT)";
            cmd = new SqlCommand(sql, dbConnect);
            cmd.Parameters.AddWithValue("@BranchOID", BranchOID);
            cmd.Parameters.AddWithValue("@DescriptionOID", DescriptionOID);
            cmd.Parameters.AddWithValue("@Barcode", entity.Barcode);
            cmd.Parameters.AddWithValue("@InwardQty", 0);
            cmd.Parameters.AddWithValue("@OutwardQty", 1);
            cmd.Parameters.AddWithValue("@Particulars", "SES To SIS Transfer");
            cmd.Parameters.AddWithValue("@Remarks", entity.BranchText);
            cmd.Parameters.AddWithValue("@IUSER", entity.IUSER);
            cmd.Parameters.AddWithValue("@IDAT", DateTime.Now.Date);
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

            sql = "delete from StoreMasterStock where Barcode=@Barcode";
            cmd = new SqlCommand(sql, dbConnect);
            cmd.Parameters.Add("@Barcode", SqlDbType.VarChar, 100).Value = entity.Barcode;
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

        public String SenderBranch(String Barcode)
        {
            String BranchOID = string.Empty;
            sql = "select StoreMasterStock.Branch from StoreMasterStock where StoreMasterStock.Barcode = '" + Barcode + "' ";
            cmd = new SqlCommand(sql, dbConnect);
            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (string.IsNullOrEmpty(reader["Branch"].ToString()))
                    {
                        BranchOID = string.Empty;
                    }
                    else
                    {
                        BranchOID = reader["Branch"].ToString();
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

            return BranchOID;
        }
        public String SenderPROD_DES(String Barcode)
        {
            String PROD_DES = string.Empty;
            sql = "select StoreMasterStock.PROD_DES from StoreMasterStock where StoreMasterStock.Barcode = '" + Barcode + "' ";
            cmd = new SqlCommand(sql, dbConnect);
            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (string.IsNullOrEmpty(reader["PROD_DES"].ToString()))
                    {
                        PROD_DES = string.Empty;
                    }
                    else
                    {
                        PROD_DES = reader["PROD_DES"].ToString();
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

            return PROD_DES;
        }

        public T_PROD FindBarcode(String Barcode)
        {
            T_PROD entity = new T_PROD();
            sql = "select StoreMasterStock.OID,StoreMasterStock.CostPrice,StoreMasterStock.SalePrice from StoreMasterStock where StoreMasterStock.Barcode='" + Barcode + "' and StoreMasterStock.SaleStatus=0 and StoreMasterStock.Quantity=1 ";
            cmd = new SqlCommand(sql, dbConnect);
            try
            {
                if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (string.IsNullOrEmpty(reader["OID"].ToString()))
                    {
                        entity.OID = string.Empty;
                    }
                    else
                    {
                        entity.OID = reader["OID"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["CostPrice"].ToString()))
                    {
                        entity.CostPrice = string.Empty;
                    }
                    else
                    {
                        entity.CostPrice = reader["CostPrice"].ToString();
                    }
                    if (string.IsNullOrEmpty(reader["SalePrice"].ToString()))
                    {
                        entity.SalePrice = string.Empty;
                    }
                    else
                    {
                        entity.SalePrice = reader["SalePrice"].ToString();
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
            return entity;
        }

        public String CheckProductonSIS(String Barcode)
        {
            String found = string.Empty;
            sql = "select StoreMasterStock.OID from StoreMasterStock where StoreMasterStock.Barcode='" + Barcode + "' ";
            cmd = new SqlCommand(sql, dbConnect1);
            try
            {
                if (dbConnect1.State == ConnectionState.Closed) dbConnect1.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (string.IsNullOrEmpty(reader["OID"].ToString()))
                    {
                        found = string.Empty;
                    }
                    else
                    {
                        found = reader["OID"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbConnect1.Close();
            }

            return found;
        }


    }
}
