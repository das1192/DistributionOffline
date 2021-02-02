using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using TalukderPOS.BusinessObjects;

namespace TalukderPOS.DAL
{
	public class MenuPageDAL
	{
		public MenuPageDAL()
		{
			DbProviderHelper.GetConnection();
		}
        private static void BuildEntity(DbDataReader oDbDataReader, MenuPage oMenuPage)
        {
            if (oDbDataReader["PageId"] != DBNull.Value)
                oMenuPage.PageId = Convert.ToInt32(oDbDataReader["PageId"]);
            if (oDbDataReader["MenuHeadID"] != DBNull.Value)
                oMenuPage.MenuHeadID = Convert.ToInt32(oDbDataReader["MenuHeadID"]);
            if (oDbDataReader["PageName"] != DBNull.Value)
                oMenuPage.PageName = Convert.ToString(oDbDataReader["PageName"]);
            if (oDbDataReader["URL"] != DBNull.Value)
                oMenuPage.URL = Convert.ToString(oDbDataReader["URL"]);

            if (oDbDataReader["CreateDate"] != DBNull.Value)
                oMenuPage.CreateDate = Convert.ToDateTime(oDbDataReader["CreateDate"]);

            if (oDbDataReader["LastUpdateDate"] != DBNull.Value)
                oMenuPage.LastUpdateDate = Convert.ToDateTime(oDbDataReader["LastUpdateDate"]);
            if (oDbDataReader["IsRemoved"] != DBNull.Value)
                oMenuPage.IsRemoved = Convert.ToBoolean(oDbDataReader["IsRemoved"]);
        }
		public List<MenuPage> MenuPage_GetAll()
		{
			try
			{
				List<MenuPage> lstMenuPage = new List<MenuPage>();
				DbCommand oDbCommand = DbProviderHelper.CreateCommand("MenuPage_GetAll",CommandType.StoredProcedure);
				DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
				while (oDbDataReader.Read())
				{
					MenuPage oMenuPage = new MenuPage();
				BuildEntity(oDbDataReader, oMenuPage);
					lstMenuPage.Add(oMenuPage);
				}
				oDbDataReader.Close();
				return lstMenuPage;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public List<MenuPage> MenuPage_GetDynamic(string WhereCondition,string OrderByExpression)
		{
			try
			{
				List<MenuPage> lstMenuPage = new List<MenuPage>();
				DbCommand oDbCommand = DbProviderHelper.CreateCommand("MenuPage_GetDynamic",CommandType.StoredProcedure);
				AddParameter(oDbCommand, "@WhereCondition", DbType.String, WhereCondition);
				AddParameter(oDbCommand, "@OrderByExpression", DbType.String, OrderByExpression);
				DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
				while (oDbDataReader.Read())
				{
					MenuPage oMenuPage = new MenuPage();
				BuildEntity(oDbDataReader, oMenuPage);
					lstMenuPage.Add(oMenuPage);
				}
				oDbDataReader.Close();
				return lstMenuPage;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public List<MenuPage> MenuPage_GetPaged(int StartRowIndex, int RowPerPage, string WhereClause, string SortColumn, string SortOrder)
		{
			try
			{
				List<MenuPage> lstMenuPage = new List<MenuPage>();
				DbCommand oDbCommand = DbProviderHelper.CreateCommand("MenuPage_GetPaged",CommandType.StoredProcedure);
				AddParameter(oDbCommand, "@StartRowIndex", DbType.Int32, StartRowIndex);
				AddParameter(oDbCommand, "@RowPerPage", DbType.Int32, RowPerPage);
				AddParameter(oDbCommand, "@WhereClause", DbType.String, WhereClause);
				AddParameter(oDbCommand, "@SortColumn", DbType.String, SortColumn);
				AddParameter(oDbCommand, "@SortOrder", DbType.String, SortOrder);
				DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
				while (oDbDataReader.Read())
				{
					MenuPage oMenuPage = new MenuPage();
				BuildEntity(oDbDataReader, oMenuPage);
					lstMenuPage.Add(oMenuPage);
				}
				oDbDataReader.Close();
				return lstMenuPage;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public MenuPage MenuPage_GetById(int PageId)
		{
			try
			{
				MenuPage oMenuPage = new MenuPage();
				DbCommand oDbCommand = DbProviderHelper.CreateCommand("MenuPage_GetById",CommandType.StoredProcedure);
				AddParameter(oDbCommand, "@PageId",DbType.Int32,PageId);
				DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
				while (oDbDataReader.Read())
				{
				BuildEntity(oDbDataReader, oMenuPage);
				}
				oDbDataReader.Close();
				return oMenuPage;
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
		public int Add(MenuPage _MenuPage)
		{
			try
			{
				DbCommand oDbCommand = DbProviderHelper.CreateCommand("MenuPage_Create",CommandType.StoredProcedure);
				AddParameter(oDbCommand, "@MenuHeadID",DbType.Int32, _MenuPage.MenuHeadID);
				AddParameter(oDbCommand, "@PageName",DbType.String, _MenuPage.PageName);
				AddParameter(oDbCommand, "@URL",DbType.String, _MenuPage.URL);
				if (_MenuPage.CreateDate.HasValue)
					AddParameter(oDbCommand, "@CreateDate",DbType.DateTime, _MenuPage.CreateDate);
				else
					AddParameter(oDbCommand, "@CreateDate",DbType.DateTime,DBNull.Value);
				if (_MenuPage.LastUpdateDate.HasValue)
					AddParameter(oDbCommand, "@LastUpdateDate",DbType.DateTime, _MenuPage.LastUpdateDate);
				else
					AddParameter(oDbCommand, "@LastUpdateDate",DbType.DateTime,DBNull.Value);
				AddParameter(oDbCommand, "@IsRemoved",DbType.Int32, _MenuPage.IsRemoved);

				return Convert.ToInt32(DbProviderHelper.ExecuteScalar(oDbCommand));
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public int Update(MenuPage _MenuPage)
		{

			try
			{
				DbCommand oDbCommand = DbProviderHelper.CreateCommand("MenuPage_Update",CommandType.StoredProcedure);
				AddParameter(oDbCommand, "@MenuHeadID",DbType.Int32, _MenuPage.MenuHeadID);
				AddParameter(oDbCommand, "@PageName",DbType.String, _MenuPage.PageName);
				AddParameter(oDbCommand, "@URL",DbType.String, _MenuPage.URL);
				if (_MenuPage.CreateDate.HasValue)
					AddParameter(oDbCommand, "@CreateDate",DbType.DateTime, _MenuPage.CreateDate);
				else
					AddParameter(oDbCommand, "@CreateDate",DbType.DateTime,DBNull.Value);
				if (_MenuPage.LastUpdateDate.HasValue)
					AddParameter(oDbCommand, "@LastUpdateDate",DbType.DateTime, _MenuPage.LastUpdateDate);
				else
					AddParameter(oDbCommand, "@LastUpdateDate",DbType.DateTime,DBNull.Value);
				AddParameter(oDbCommand, "@IsRemoved",DbType.Int32, _MenuPage.IsRemoved);
				AddParameter(oDbCommand, "@PageId",DbType.Int32, _MenuPage.PageId);
				return DbProviderHelper.ExecuteNonQuery(oDbCommand);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public int Delete(int PageId)
		{

			try
			{
				DbCommand oDbCommand = DbProviderHelper.CreateCommand("MenuPage_DeleteById",CommandType.StoredProcedure);
				AddParameter(oDbCommand, "@PageId",DbType.Int32,PageId);
				return DbProviderHelper.ExecuteNonQuery(oDbCommand);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

        public List<MenuPage> MenuPage_GetAllByHeadUser(int headID, string userID, bool permission)
        {
            try
            {
                List<MenuPage> lstMenuPage = new List<MenuPage>();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("MenuPage_GetAllByHeadUser", CommandType.StoredProcedure);
                AddParameter(oDbCommand, "@HeadID", DbType.Int32, headID);
                AddParameter(oDbCommand, "@UserID", DbType.String, userID);
                AddParameter(oDbCommand, "@Permission", DbType.Boolean, permission);
                
                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                    MenuPage oMenuPage = new MenuPage();
                    BuildEntity(oDbDataReader, oMenuPage);
                    lstMenuPage.Add(oMenuPage);
                }
                oDbDataReader.Close();
                return lstMenuPage;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable MenuPage_GetAllByHeadID(int headID)
        {

            DataTable dtPages = null;
            DbDataReader oDbDataReader = null;
            try
            {
                dtPages = new DataTable();

                DbCommand oDbCommand = DbProviderHelper.CreateCommand("MenuPage_GetAllByHeadID", CommandType.StoredProcedure);

                AddParameter(oDbCommand, "@HeadID", DbType.Int32, headID);

                oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                dtPages.Load(oDbDataReader);
                oDbDataReader.Close();
                return dtPages;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                dtPages.Dispose();
                oDbDataReader.Dispose();
            }
        }
    }
}
