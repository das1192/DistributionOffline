using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using TalukderPOS.BusinessObjects;
using System.Text.RegularExpressions;

namespace TalukderPOS.DAL
{
    public class MenuPermissionDAL
    {
        public MenuPermissionDAL()
        {
            DbProviderHelper.GetConnection();
        }
        private static void BuildEntity(DbDataReader oDbDataReader, MenuPermission oMenuPermission)
        {
            oMenuPermission.GrantID = Convert.ToInt32(oDbDataReader["GrantID"]);
            oMenuPermission.UserID = Convert.ToString(oDbDataReader["UserID"]);
            oMenuPermission.MenuHeadID = Convert.ToInt32(oDbDataReader["MenuHeadID"]);
            oMenuPermission.PageID = Convert.ToInt32(oDbDataReader["PageID"]);
            oMenuPermission.CanView = Convert.ToBoolean(oDbDataReader["CanView"]);
        }
        public List<MenuPermission> MenuPermission_GetAll()
        {
            try
            {
                List<MenuPermission> lstMenuPermission = new List<MenuPermission>();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("MenuPermission_GetAll", CommandType.StoredProcedure);
                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                    MenuPermission oMenuPermission = new MenuPermission();
                    BuildEntity(oDbDataReader, oMenuPermission);
                    lstMenuPermission.Add(oMenuPermission);
                }
                oDbDataReader.Close();
                return lstMenuPermission;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<MenuPermission> MenuPermission_GetDynamic(string WhereCondition, string OrderByExpression)
        {
            try
            {
                List<MenuPermission> lstMenuPermission = new List<MenuPermission>();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("MenuPermission_GetDynamic", CommandType.StoredProcedure);
                AddParameter(oDbCommand, "@WhereCondition", DbType.String, WhereCondition);
                AddParameter(oDbCommand, "@OrderByExpression", DbType.String, OrderByExpression);
                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                    MenuPermission oMenuPermission = new MenuPermission();
                    BuildEntity(oDbDataReader, oMenuPermission);
                    lstMenuPermission.Add(oMenuPermission);
                }
                oDbDataReader.Close();
                return lstMenuPermission;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<MenuPermission> MenuPermission_GetPaged(int StartRowIndex, int RowPerPage, string WhereClause, string SortColumn, string SortOrder)
        {
            try
            {
                List<MenuPermission> lstMenuPermission = new List<MenuPermission>();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("MenuPermission_GetPaged", CommandType.StoredProcedure);
                AddParameter(oDbCommand, "@StartRowIndex", DbType.Int32, StartRowIndex);
                AddParameter(oDbCommand, "@RowPerPage", DbType.Int32, RowPerPage);
                AddParameter(oDbCommand, "@WhereClause", DbType.String, WhereClause);
                AddParameter(oDbCommand, "@SortColumn", DbType.String, SortColumn);
                AddParameter(oDbCommand, "@SortOrder", DbType.String, SortOrder);
                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                    MenuPermission oMenuPermission = new MenuPermission();
                    BuildEntity(oDbDataReader, oMenuPermission);
                    lstMenuPermission.Add(oMenuPermission);
                }
                oDbDataReader.Close();
                return lstMenuPermission;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public MenuPermission MenuPermission_GetById(int GrantID)
        {
            try
            {
                MenuPermission oMenuPermission = new MenuPermission();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("MenuPermission_GetById", CommandType.StoredProcedure);
                AddParameter(oDbCommand, "@GrantID", DbType.Int32, GrantID);
                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                    BuildEntity(oDbDataReader, oMenuPermission);
                }
                oDbDataReader.Close();
                return oMenuPermission;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void AddParameter(DbCommand oDbCommand, string parameterName, DbType dbType, object value)
        {
            oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter(parameterName, dbType, value));
        }
        public int Add(MenuPermission _MenuPermission)
        {
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("MenuPermission_Create", CommandType.StoredProcedure);
                AddParameter(oDbCommand, "@UserID", DbType.String, _MenuPermission.UserID);
                AddParameter(oDbCommand, "@MenuHeadID", DbType.Int32, _MenuPermission.MenuHeadID);
                AddParameter(oDbCommand, "@PageID", DbType.Int32, _MenuPermission.PageID);
                AddParameter(oDbCommand, "@CanView", DbType.Boolean, _MenuPermission.CanView);

                return Convert.ToInt32(DbProviderHelper.ExecuteScalar(oDbCommand));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int Update(MenuPermission _MenuPermission)
        {

            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("MenuPermission_Update", CommandType.StoredProcedure);
                AddParameter(oDbCommand, "@UserID", DbType.String, _MenuPermission.UserID);
                AddParameter(oDbCommand, "@MenuHeadID", DbType.Int32, _MenuPermission.MenuHeadID);
                AddParameter(oDbCommand, "@PageID", DbType.Int32, _MenuPermission.PageID);
                AddParameter(oDbCommand, "@CanView", DbType.Boolean, _MenuPermission.CanView);
                AddParameter(oDbCommand, "@GrantID", DbType.Int32, _MenuPermission.GrantID);
                return DbProviderHelper.ExecuteNonQuery(oDbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int Delete(int GrantID)
        {

            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("MenuPermission_DeleteById", CommandType.StoredProcedure);
                AddParameter(oDbCommand, "@GrantID", DbType.Int32, GrantID);
                return DbProviderHelper.ExecuteNonQuery(oDbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int MenuPermission_UpdateUserID(string userID)
        {
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("MenuPermission_UpdateUserID", CommandType.StoredProcedure);
                AddParameter(oDbCommand, "@UserID", DbType.String, userID);
                return DbProviderHelper.ExecuteNonQuery(oDbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int MenuPermission_DeleteByUserID(string userID)
        {
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("MenuPermission_DeleteByUserID", CommandType.StoredProcedure);
                AddParameter(oDbCommand, "@UserID", DbType.String, userID);
                return DbProviderHelper.ExecuteNonQuery(oDbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable MenuPermission_IsPageAuthorized(string userID, string requestedURL)
        {

            DataTable dt = null;
            DbDataReader oDbDataReader = null;
            try
            {
                dt = new DataTable();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("MenuPermission_IsPageAuthorizedByUserandUrl", CommandType.StoredProcedure);
                AddParameter(oDbCommand, "@UserID", DbType.String, userID);
                AddParameter(oDbCommand, "@RequestedURL", DbType.String, requestedURL);

                oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                dt.Load(oDbDataReader);
                oDbDataReader.Close();
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                dt.Dispose();
                oDbDataReader.Dispose();
            }
        }
        public DataTable MenuPermissionCheck(string UserID="" ,string URL="")
        {
            string devMode = System.Configuration.ConfigurationManager.AppSettings["devMode"].ToString();
            DataTable dt = null;
            DbDataReader oDbDataReader = null;
            string devModeString = "/POS.WEB.UI/";
            
            if (devMode == "1")
            {
                URL = Regex.Split(URL, devModeString)[1].ToString();
            }
            else
            {
                int urllength = URL.Length;
                URL = URL.Substring(1, urllength-1);
            }
            try
            {
                dt = new DataTable();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("MenuPermissionCheck", CommandType.StoredProcedure);
                AddParameter(oDbCommand, "@UserID", DbType.String, UserID);
                AddParameter(oDbCommand, "@URL", DbType.String, URL);

                oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                dt.Load(oDbDataReader);
                oDbDataReader.Close();
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                dt.Dispose();
                oDbDataReader.Dispose();
            }
        }

        public DataTable CommonAndInternalPages_IsExist(string requestedURL)
        {
            DataTable dt = null;
            DbDataReader oDbDataReader = null;
            try
            {
                dt = new DataTable();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("CommonAndInternalPages_IsExist", CommandType.StoredProcedure);

                AddParameter(oDbCommand, "@RequestedURL", DbType.String, requestedURL);

                oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                dt.Load(oDbDataReader);
                oDbDataReader.Close();
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                dt.Dispose();
                oDbDataReader.Dispose();
            }
        }
    }
}
