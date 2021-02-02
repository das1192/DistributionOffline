using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using TalukderPOS.BusinessObjects;

namespace TalukderPOS.DAL
{
    public class MenuHeadDAL
    {
        public MenuHeadDAL()
        {
            DbProviderHelper.GetConnection();
        }
        private static void BuildEntity(DbDataReader oDbDataReader, MenuHead oMenuHead)
        {
            if (oDbDataReader["MenuHeadID"] != DBNull.Value)
                oMenuHead.MenuHeadID = Convert.ToInt32(oDbDataReader["MenuHeadID"]);
            if (oDbDataReader["MenuHeadName"] != DBNull.Value)
                oMenuHead.MenuHeadName = Convert.ToString(oDbDataReader["MenuHeadName"]);
            if (oDbDataReader["Priority"] != DBNull.Value)
                oMenuHead.Priority = Convert.ToInt32(oDbDataReader["Priority"]);
            if (oDbDataReader["DivID"] != DBNull.Value)
                oMenuHead.DivID = Convert.ToString(oDbDataReader["DivID"]);
        }
        public List<MenuHead> MenuHead_GetAll()
        {
            DbDataReader oDbDataReader = null;
            try
            {
                List<MenuHead> lstMenuHead = new List<MenuHead>();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("MenuHead_GetAll", CommandType.StoredProcedure);
                oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                    MenuHead oMenuHead = new MenuHead();
                    BuildEntity(oDbDataReader, oMenuHead);
                    lstMenuHead.Add(oMenuHead);
                }
                oDbDataReader.Close();
                return lstMenuHead;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oDbDataReader.Dispose();
            }
        }
        public List<MenuHead> MenuHead_GetDynamic(string WhereCondition, string OrderByExpression)
        {
            DbDataReader oDbDataReader = null;
            try
            {
                List<MenuHead> lstMenuHead = new List<MenuHead>();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("MenuHead_GetDynamic", CommandType.StoredProcedure);
                AddParameter(oDbCommand, "@WhereCondition", DbType.String, WhereCondition);
                AddParameter(oDbCommand, "@OrderByExpression", DbType.String, OrderByExpression);
                oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                    MenuHead oMenuHead = new MenuHead();
                    BuildEntity(oDbDataReader, oMenuHead);
                    lstMenuHead.Add(oMenuHead);
                }
                oDbDataReader.Close();
                return lstMenuHead;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oDbDataReader.Dispose();
            }
        }
        public List<MenuHead> MenuHead_GetPaged(int StartRowIndex, int RowPerPage, string WhereClause, string SortColumn, string SortOrder)
        {
            DbDataReader oDbDataReader = null;
            try
            {
                List<MenuHead> lstMenuHead = new List<MenuHead>();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("MenuHead_GetPaged", CommandType.StoredProcedure);
                AddParameter(oDbCommand, "@StartRowIndex", DbType.Int32, StartRowIndex);
                AddParameter(oDbCommand, "@RowPerPage", DbType.Int32, RowPerPage);
                AddParameter(oDbCommand, "@WhereClause", DbType.String, WhereClause);
                AddParameter(oDbCommand, "@SortColumn", DbType.String, SortColumn);
                AddParameter(oDbCommand, "@SortOrder", DbType.String, SortOrder);
                oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                    MenuHead oMenuHead = new MenuHead();
                    BuildEntity(oDbDataReader, oMenuHead);
                    lstMenuHead.Add(oMenuHead);
                }
                oDbDataReader.Close();
                return lstMenuHead;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oDbDataReader.Dispose();
            }
        }
        public MenuHead MenuHead_GetById(int MenuHeadID)
        {
            DbDataReader oDbDataReader = null;
            try
            {
                MenuHead oMenuHead = new MenuHead();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("MenuHead_GetById", CommandType.StoredProcedure);
                AddParameter(oDbCommand, "@MenuHeadID", DbType.Int32, MenuHeadID);
                oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                    BuildEntity(oDbDataReader, oMenuHead);
                }
                oDbDataReader.Close();
                return oMenuHead;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oDbDataReader.Dispose();
            }
        }
        private void AddParameter(DbCommand oDbCommand, string parameterName, DbType dbType, object value)
        {
            oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter(parameterName, dbType, value));
        }
        public int Add(MenuHead _MenuHead)
        {
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("MenuHead_Create", CommandType.StoredProcedure);
                AddParameter(oDbCommand, "@MenuHeadName", DbType.String, _MenuHead.MenuHeadName);
                AddParameter(oDbCommand, "@Priority", DbType.Int32, _MenuHead.Priority);

                return Convert.ToInt32(DbProviderHelper.ExecuteScalar(oDbCommand));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int Update(MenuHead _MenuHead)
        {

            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("MenuHead_Update", CommandType.StoredProcedure);
                AddParameter(oDbCommand, "@MenuHeadName", DbType.String, _MenuHead.MenuHeadName);
                AddParameter(oDbCommand, "@Priority", DbType.Int32, _MenuHead.Priority);
                AddParameter(oDbCommand, "@MenuHeadID", DbType.Int32, _MenuHead.MenuHeadID);
                return DbProviderHelper.ExecuteNonQuery(oDbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int Delete(int MenuHeadID)
        {

            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("MenuHead_DeleteById", CommandType.StoredProcedure);
                AddParameter(oDbCommand, "@MenuHeadID", DbType.Int32, MenuHeadID);
                return DbProviderHelper.ExecuteNonQuery(oDbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<MenuHead> MenuHead_GetAllByUserPermission(string userID, bool permission)
        {
            DbDataReader oDbDataReader = null;
            try
            {

                List<MenuHead> lstMenuHead = new List<MenuHead>();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("MenuHead_GetAllByUserPermission", CommandType.StoredProcedure);
                AddParameter(oDbCommand, "@UserID", DbType.String, userID);
                AddParameter(oDbCommand, "@Permission", DbType.Boolean, permission);

                oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                    MenuHead oMenuHead = new MenuHead();
                    BuildEntity(oDbDataReader, oMenuHead);
                    lstMenuHead.Add(oMenuHead);
                }
                oDbDataReader.Close();
                return lstMenuHead;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oDbDataReader.Dispose();
            }
        }
    }
}
