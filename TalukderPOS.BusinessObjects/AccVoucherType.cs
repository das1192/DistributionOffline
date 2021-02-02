using System;
using System.Text;

namespace TalukderPOS.BusinessObjects
{
	[Serializable()]
	public class AccVoucherType
	{
		public int  VoucherTypeId { get; set; }
		public string  VoucherTypeCode { get; set; }
		public string  VoucherType { get; set; }

		public AccVoucherType()
		{ }

		public AccVoucherType(int VoucherTypeId,string VoucherTypeCode,string VoucherType)
		{
			this.VoucherTypeId = VoucherTypeId;
			this.VoucherTypeCode = VoucherTypeCode;
			this.VoucherType = VoucherType;
		}

		public override string ToString()
		{
			return "VoucherTypeId = " + VoucherTypeId.ToString() + ",VoucherTypeCode = " + VoucherTypeCode + ",VoucherType = " + VoucherType;
		}

	}
}
