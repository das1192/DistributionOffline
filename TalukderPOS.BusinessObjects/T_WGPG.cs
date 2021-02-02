using System;
using System.Text;

namespace TalukderPOS.BusinessObjects
{
	[Serializable()]
	public class T_WGPG
	{
		public string  OID { get; set; }		
		public string  WGPG_NAME { get; set; }
        public string WGPG_ACTV { get; set; }
        public string IUSER { get; set; }
        public string IDAT { get; set; }
        public string EUSER { get; set; }        
        public string EDAT { get; set; }
        public string Shop_id { get; set; }
	}
}
