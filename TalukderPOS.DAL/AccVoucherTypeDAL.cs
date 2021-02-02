using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using TalukderPOS.BusinessObjects;
using System.Data.SqlClient;

namespace TalukderPOS.DAL
{
	public class AccVoucherTypeDAL
	{
		public AccVoucherTypeDAL()
		{
			DbProviderHelper.GetConnection();
		}
		private static void BuildEntity(DbDataReader oDbDataReader, AccVoucherType oAccVoucherType)
		{
					oAccVoucherType.VoucherTypeId = Convert.ToInt32(oDbDataReader["VoucherTypeId"]);
					oAccVoucherType.VoucherTypeCode = Convert.ToString(oDbDataReader["VoucherTypeCode"]);
					oAccVoucherType.VoucherType = Convert.ToString(oDbDataReader["VoucherType"]);
		}

        public List<AccVoucherType> AccVoucherType_GetAll()
		{
			try
			{
				List<AccVoucherType> lstAccVoucherType = new List<AccVoucherType>();
				
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("AccVoucherType_GetAll",CommandType.StoredProcedure);

				DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
				while (oDbDataReader.Read())
				{
				
                    AccVoucherType oAccVoucherType = new AccVoucherType();
				    BuildEntity(oDbDataReader, oAccVoucherType);
					lstAccVoucherType.Add(oAccVoucherType);
				}
				oDbDataReader.Close();
				return lstAccVoucherType;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public List<AccVoucherType> AccVoucherType_GetDynamic(string WhereCondition,string OrderByExpression)
		{
			try
			{
				List<AccVoucherType> lstAccVoucherType = new List<AccVoucherType>();
				DbCommand oDbCommand = DbProviderHelper.CreateCommand("AccVoucherType_GetDynamic",CommandType.StoredProcedure);
				AddParameter(oDbCommand, "@WhereCondition", DbType.String, WhereCondition);
				AddParameter(oDbCommand, "@OrderByExpression", DbType.String, OrderByExpression);
				DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
				while (oDbDataReader.Read())
				{
					AccVoucherType oAccVoucherType = new AccVoucherType();
				BuildEntity(oDbDataReader, oAccVoucherType);
					lstAccVoucherType.Add(oAccVoucherType);
				}
				oDbDataReader.Close();
				return lstAccVoucherType;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public List<AccVoucherType> AccVoucherType_GetPaged(int StartRowIndex, int RowPerPage, string WhereClause, string SortColumn, string SortOrder)
		{
			try
			{
				List<AccVoucherType> lstAccVoucherType = new List<AccVoucherType>();
				DbCommand oDbCommand = DbProviderHelper.CreateCommand("AccVoucherType_GetPaged",CommandType.StoredProcedure);
				AddParameter(oDbCommand, "@StartRowIndex", DbType.Int32, StartRowIndex);
				AddParameter(oDbCommand, "@RowPerPage", DbType.Int32, RowPerPage);
				AddParameter(oDbCommand, "@WhereClause", DbType.String, WhereClause);
				AddParameter(oDbCommand, "@SortColumn", DbType.String, SortColumn);
				AddParameter(oDbCommand, "@SortOrder", DbType.String, SortOrder);
				DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
				while (oDbDataReader.Read())
				{
					AccVoucherType oAccVoucherType = new AccVoucherType();
				BuildEntity(oDbDataReader, oAccVoucherType);
					lstAccVoucherType.Add(oAccVoucherType);
				}
				oDbDataReader.Close();
				return lstAccVoucherType;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public AccVoucherType AccVoucherType_GetById(int VoucherTypeId)
		{
			try
			{
				AccVoucherType oAccVoucherType = new AccVoucherType();
				DbCommand oDbCommand = DbProviderHelper.CreateCommand("AccVoucherType_GetById",CommandType.StoredProcedure);
				AddParameter(oDbCommand, "@VoucherTypeId",DbType.Int32,VoucherTypeId);
				DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
				while (oDbDataReader.Read())
				{
				BuildEntity(oDbDataReader, oAccVoucherType);
				}
				oDbDataReader.Close();
				return oAccVoucherType;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		private void AddParameter(DbCommand oDbCommand, string parameterName, DbType dbType, object value)
		{
			 oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter(parameterName,dbType,value));
		}
		public int Add(AccVoucherType _AccVoucherType)
		{
			try
			{
				DbCommand oDbCommand = DbProviderHelper.CreateCommand("AccVoucherType_Create",CommandType.StoredProcedure);
				AddParameter(oDbCommand, "@VoucherTypeCode",DbType.String, _AccVoucherType.VoucherTypeCode);
				AddParameter(oDbCommand, "@VoucherType",DbType.String, _AccVoucherType.VoucherType);

				return Convert.ToInt32(DbProviderHelper.ExecuteScalar(oDbCommand));
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public int Update(AccVoucherType _AccVoucherType)
		{

			try
			{
				DbCommand oDbCommand = DbProviderHelper.CreateCommand("AccVoucherType_Update",CommandType.StoredProcedure);
				AddParameter(oDbCommand, "@VoucherTypeCode",DbType.String, _AccVoucherType.VoucherTypeCode);
				AddParameter(oDbCommand, "@VoucherType",DbType.String, _AccVoucherType.VoucherType);
				AddParameter(oDbCommand, "@VoucherTypeId",DbType.Int32, _AccVoucherType.VoucherTypeId);
				return DbProviderHelper.ExecuteNonQuery(oDbCommand);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public int Delete(int VoucherTypeId)
		{

			try
			{
				DbCommand oDbCommand = DbProviderHelper.CreateCommand("AccVoucherType_DeleteById",CommandType.StoredProcedure);
				AddParameter(oDbCommand, "@VoucherTypeId",DbType.Int32,VoucherTypeId);
				return DbProviderHelper.ExecuteNonQuery(oDbCommand);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}



        public DataTable TblSalesOrder_For_DeliveryGetAll(string UserID)
        {

            DataTable dt_L = new DataTable();
              try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("TblSalesOrder_For_DeliveryGetAll", CommandType.StoredProcedure);
               AddParameter(oDbCommand, "@UserID", DbType.String, UserID);
            
            //SqlConnection conn = new SqlConnection(connStr);
            //conn.Open();
            //SqlCommand dCmd = new SqlCommand("uspSelectSearchModel", conn);
            //dCmd.CommandType = CommandType.StoredProcedure;
         
                //dCmd.Parameters.AddWithValue("@ItemCode", ItemCode);
                //dCmd.Parameters.AddWithValue("@ItemName", ItemName);

               // DbProviderHelper.ExecuteNonQuery(oDbCommand);

                DbDataAdapter adapter = DbProviderHelper.CreateDataAdapter(oDbCommand);
                adapter.Fill(dt_L);
                 
           
            }
              catch (Exception ex)
              {
                  throw ex;
              }
              finally
              {
                  dt_L.Dispose();
              }
              return dt_L;
        }
        public DataTable DelivaryOrderList_Get_C_M_G_Name_ID(string UserID)
        {

            DataTable dt_L = new DataTable();
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("DelivaryOrderList_Get_C_M_G_Name_ID", CommandType.StoredProcedure);
                AddParameter(oDbCommand, "@UserID", DbType.String, UserID);

                //SqlConnection conn = new SqlConnection(connStr);
                //conn.Open();
                //SqlCommand dCmd = new SqlCommand("uspSelectSearchModel", conn);
                //dCmd.CommandType = CommandType.StoredProcedure;

                //dCmd.Parameters.AddWithValue("@ItemCode", ItemCode);
                //dCmd.Parameters.AddWithValue("@ItemName", ItemName);

                // DbProviderHelper.ExecuteNonQuery(oDbCommand);

                DbDataAdapter adapter = DbProviderHelper.CreateDataAdapter(oDbCommand);
                adapter.Fill(dt_L);


            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt_L.Dispose();
            }
            return dt_L;
        }

        public DataTable SaleOrder_Get_C_M_G_Name_ID(string UserID)
        {

            DataTable dt_L = new DataTable();
              try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("SaleOrder_Get_C_M_G_Name_ID", CommandType.StoredProcedure);
               AddParameter(oDbCommand, "@UserID", DbType.String, UserID);
            
            //SqlConnection conn = new SqlConnection(connStr);
            //conn.Open();
            //SqlCommand dCmd = new SqlCommand("uspSelectSearchModel", conn);
            //dCmd.CommandType = CommandType.StoredProcedure;
         
                //dCmd.Parameters.AddWithValue("@ItemCode", ItemCode);
                //dCmd.Parameters.AddWithValue("@ItemName", ItemName);

               // DbProviderHelper.ExecuteNonQuery(oDbCommand);

                DbDataAdapter adapter = DbProviderHelper.CreateDataAdapter(oDbCommand);
                adapter.Fill(dt_L);
                 
           
            }
              catch (Exception ex)
              {
                  throw ex;
              }
              finally
              {
                  dt_L.Dispose();
              }
              return dt_L;
        }

        public static DataTable CreateDataTable(string pStoreProcedure)
        {
            DataTable table = new DataTable();
            try
            {
                DbCommand command = DbProviderHelper.CreateCommand(pStoreProcedure, CommandType.StoredProcedure);
                DbDataAdapter adapter = DbProviderHelper.CreateDataAdapter(command);
                adapter.Fill(table);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                table.Dispose();
            }
            return table;
        }


        public static DataTable GetDataTable(string psql)
        {
            string conString = DbProviderHelper.GetConnectionStrings();
            SqlConnection sqlCon = new SqlConnection(conString);
            SqlCommand cmd;
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            cmd = new SqlCommand(psql, sqlCon);
            try
            {
                cmd.CommandTimeout = 30;
                da.SelectCommand = cmd;
                sqlCon.Open();
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Dispose();
                sqlCon.Close();
            }
        }

        public static string GetUserCompanyCode(string pUserId)
        {

            try {
               return GetDataTable("Select COM_ID from [User] Where UserId='" + pUserId + "'").Rows[0]["COM_ID"].ToString();
            
            }
            catch (Exception ex)
            {
                throw ex;
            }
        
        }

	}
}
