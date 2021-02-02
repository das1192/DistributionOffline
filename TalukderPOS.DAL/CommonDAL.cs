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
using System.Web;

namespace TalukderPOS.DAL
{
    public class CommonDAL
    {

        public DataTable LoadDataByQuery(string sqlQuery)
        {
            DataTable dt = new DataTable();
            string constr = ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(sqlQuery))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        sda.Fill(dt);
                    }
                }
            }
            return dt;
        }//

      

        public void SaveDataCRUD(string sqlQuery)
        {
            string constr = ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(constr))
            {
                SqlCommand command = new SqlCommand(sqlQuery, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
            }
        }

        
        //
    }
}
