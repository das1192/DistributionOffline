using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using TalukderPOS.BusinessObjects;
using TalukderPOS.DAL;

namespace TalukderPOS.BLL
{
	public class AccVoucherTypeBLL
	{
		public AccVoucherTypeDAL  AccVoucherTypeDAL { get; set; }

		public AccVoucherTypeBLL()
		{
			AccVoucherTypeDAL = new AccVoucherTypeDAL();
		}

		public List<AccVoucherType> AccVoucherType_GetAll()
		{
			try
			{
				return AccVoucherTypeDAL.AccVoucherType_GetAll();
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
		public List<AccVoucherType> AccVoucherType_GetDynamic(string WhereCondition,string OrderByExpression)
		{
			try
			{
				return AccVoucherTypeDAL.AccVoucherType_GetDynamic(WhereCondition, OrderByExpression);
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
		public List<AccVoucherType> AccVoucherType_GetPaged(int StartRowIndex, int RowPerPage, string WhereClause, string SortColumn, string SortOrder)
		{
			try
			{
				return AccVoucherTypeDAL.AccVoucherType_GetPaged(StartRowIndex, RowPerPage, WhereClause, SortColumn, SortOrder);
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
		public AccVoucherType AccVoucherType_GetById(int VoucherTypeId)
		{
			try
			{
				return AccVoucherTypeDAL.AccVoucherType_GetById(VoucherTypeId);
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
		public int AccVoucherType_Add(AccVoucherType _AccVoucherType)
		{
			try
			{
				return AccVoucherTypeDAL.Add(_AccVoucherType);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public int AccVoucherType_Update(AccVoucherType _AccVoucherType)
		{
			try
			{
				return AccVoucherTypeDAL.Update(_AccVoucherType);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public int AccVoucherType_Delete(int VoucherTypeId)
		{
			try
			{
				return AccVoucherTypeDAL.Delete(VoucherTypeId);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public List<AccVoucherType> DeserializeAccVoucherTypes(string Path)
		{
			try
			{
				return GenericXmlSerializer<List<AccVoucherType>>.Deserialize(Path);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public void SerializeAccVoucherTypes(string Path, List<AccVoucherType> AccVoucherTypes)
		{
			try
			{
				GenericXmlSerializer<List<AccVoucherType>>.Serialize(AccVoucherTypes, Path);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
