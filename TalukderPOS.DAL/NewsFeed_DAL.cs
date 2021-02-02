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
    public class NewsFeed_DAL
    {
        SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
        SqlDataAdapter da = new SqlDataAdapter();
        SqlCommand cmd;
        String sql;

        public void Add(NewsFeed_BO entity)
        {
            if (string.IsNullOrEmpty(entity.OID))
            {
                sql = "insert into NewsFeed(ToDate,FromDate,BranchOID,Message,ActiveStatus,IUSER,IDAT,EUSER,EDAT) values (@ToDate,@FromDate,@BranchOID,@Message,@ActiveStatus,@IUSER,@IDAT,@EUSER,@EDAT)";
                cmd = new SqlCommand(sql, dbConnect);
                cmd.Parameters.Add("@IUSER", SqlDbType.VarChar, 100).Value = entity.IUSER;
                cmd.Parameters.Add("@IDAT", SqlDbType.Date).Value = entity.IDAT;
            }
            else
            {
                sql = "update NewsFeed set ToDate=@ToDate,FromDate=@FromDate,BranchOID=@BranchOID, Message=@Message, ActiveStatus=@ActiveStatus, EUSER=@EUSER,EDAT=@EDAT where OID=" + entity.OID + " ";
                cmd = new SqlCommand(sql, dbConnect);
            }
            cmd.Parameters.Add("@FromDate", SqlDbType.Date).Value = entity.FromDate;
            cmd.Parameters.Add("@ToDate", SqlDbType.Date).Value = entity.ToDate;
            cmd.Parameters.Add("@BranchOID", SqlDbType.VarChar,100).Value = entity.BranchOID;
            cmd.Parameters.Add("@Message", SqlDbType.Text).Value = entity.Message;
            cmd.Parameters.Add("@ActiveStatus", SqlDbType.Int).Value = entity.ActiveStatus;
            cmd.Parameters.Add("@EUSER", SqlDbType.VarChar, 100).Value = entity.EUSER;
            cmd.Parameters.Add("@EDAT", SqlDbType.Date).Value = entity.EDAT;
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



        public void Delete(NewsFeed_BO entity)
        {
            sql = "update NewsFeed set STATUS=@STATUS, EUSER=@EUSER,EDAT=@EDAT where OID=" + entity.OID + " ";
           cmd = new SqlCommand(sql, dbConnect);
           cmd.Parameters.Add("@STATUS", SqlDbType.Int).Value = 0;
           cmd.Parameters.Add("@EUSER", SqlDbType.VarChar, 100).Value = entity.EUSER;
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
}
