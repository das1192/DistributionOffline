<%@ WebHandler Language="C#" Class="ImageUploadInDB" %>

using System;  
using System.Collections.Generic;  
using System.Linq;
using System.Data; 
using System.Web;  
using System.Data.SqlClient;
using System.Configuration; 


public class ImageUploadInDB : IHttpHandler {
    SqlConnection dbConnect = new SqlConnection(ConfigurationManager.ConnectionStrings["conn_str"].ConnectionString);
    SqlDataAdapter da = new SqlDataAdapter();
  
    String sql;
    
    public void ProcessRequest (HttpContext context) {
        string imageid = context.Request.QueryString["Id"];
        //SqlConnection connection = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=CMS;Integrated Security=True");
        try
        {
            
            if (dbConnect.State == ConnectionState.Closed) dbConnect.Open();
            {
               
                sql = "select ImageByte from Shop_Logo where OID=" + imageid;

                SqlCommand command = new SqlCommand(sql, dbConnect);
                SqlDataReader dr = command.ExecuteReader();
                if (dr.Read())
                {
                    if (!(dr[0] is DBNull))
                    {
                        context.Response.BinaryWrite((Byte[])dr[0]);
                        dbConnect.Close();
                        context.Response.End();
                    }
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
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}