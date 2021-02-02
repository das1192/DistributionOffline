using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.Data.SqlClient;
using System.Text;

/// <summary>
/// Summary description for DataConnect
/// </summary>


namespace HRM.WEB.UI
{
    [Serializable]
    public class DataConnect
    {

        private string _dbcon = "";
        //private SUL.DBM.SQLHandaler.SQLDBManipulation InternalDbConnection;

        private SqlConnection con = null;
        private SqlCommand cmd = null;
        private SqlDataReader dr = null;

        public DataConnect()
        {
            SettingServer();

        }



        /// function for retrive Connection string
        private Boolean SettingServer()
        {
            try
            {
                _dbcon = ConfigurationManager.ConnectionStrings["DbCon"].ConnectionString.ToString();
                con = new SqlConnection(_dbcon);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Get Active Connection
        public SqlConnection GetConnection()
        {
            //if (con.State != ConnectionState.Closed)
            return con;
        }

        // Open Connection 
        public void Open()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
        }


        // Close Connection 
        public void Close()
        {
            if (con.State != ConnectionState.Closed)
            {
                con.Close();
            }
        }

        public void Dispose()
        {

            con.Dispose();
            con = null;
        }

        // Execute a SQL and return a datareader 
        public SqlDataReader Read(string strSQL)
        {
            try
            {
                cmd = new SqlCommand(strSQL, con);
                dr = cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
            }
            return dr;
        }



        // Execute a SQL and return a datatable 
        public DataTable Table(string strSQL)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da;
            try
            {
                da = new SqlDataAdapter(strSQL, con);
                da.SelectCommand.CommandTimeout = 120;

                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {

            }
            return dt;
        }


        //Load New Data in DropDownBox
        public void LoadDataInCombo(string strSQL, DropDownList dd)
        {
            try
            {
                cmd = new SqlCommand(strSQL, con);
                dr = cmd.ExecuteReader();

                dd.Items.Clear();
                while (dr.Read())
                {
                    ListItem li = new ListItem();
                    li.Text = dr.GetString(1);
                    li.Value = dr.GetValue(0).ToString();
                    dd.Items.Add(li);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                dr.Close();
            }
        }


        //Load New Data in CheckListBox
        public void LoadDataInCheckBoxList(string strSQL, CheckBoxList dd)
        {
            try
            {
                cmd = new SqlCommand(strSQL, con);
                dr = cmd.ExecuteReader();

                dd.Items.Clear();
                while (dr.Read())
                {
                    ListItem li = new ListItem();
                    li.Text = dr.GetString(1);
                    li.Value = dr.GetValue(0).ToString();
                    dd.Items.Add(li);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                dr.Close();
            }
        }

        //Add Data in DropDownBox
        public void AddDataInCombo(string strSQL, DropDownList dd)
        {
            try
            {
                cmd = new SqlCommand(strSQL, con);
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ListItem li = new ListItem();
                    li.Text = dr.GetString(1);
                    li.Value = dr.GetValue(0).ToString();
                    dd.Items.Add(li);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                dr.Close();
            }
        }

        //add data in listbox
        public void AddDataListbox(string strSQL, ListBox dd)
        {
            try
            {
                cmd = new SqlCommand(strSQL, con);
                dr = cmd.ExecuteReader();

                dd.Items.Clear();
                while (dr.Read())
                {
                    ListItem li = new ListItem();

                    li.Text = dr.GetString(1);
                    li.Value = dr.GetValue(0).ToString();
                    dd.Items.Add(li);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                dr.Close();
            }
        }


        //Get Maximum ID from a given table
        public long GetNextID(string tblName, string FieldName)
        {
            try
            {
                cmd = new SqlCommand("SELECT MAX(" + FieldName + ") FROM " + tblName + " ", con);
                long val = 0;
                val = Convert.ToInt64(cmd.ExecuteScalar());
                return val + 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 1;
            }
        }

        // Execute a SQL command 
        public void Execute(string strSQL)
        {
            try
            {
                if (dr != null && dr.IsClosed == false)
                    dr.Close();

                cmd = new SqlCommand(strSQL, con);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        /* For every transaction there will an Entry in BM_SceurityTransectionHistory
         * table. Current User ID, Current master ID, Input form name,
         * what type of event(save,edit,delete), Any kind of description
        */

        public void UserLogEntry(string user_id, string TransectionID,
            int menu_id, int Event, string Narration)
        {
            long next_id = GetNextID("BM_SecTransactionHistory", "HistoryID");
            string strSQL = null;

            strSQL = "INSERT " +
                  "     INTO " +
                  "BM_SecTransactionHistory (" +
                  "     HistoryID, " +
                  "     TransectionID, " +
                  "     TransectionTime, " +
                  "     MenuObjectid, " +
                  "     EventBM_ItemId, " +
                  "     Narration, " +
                  "     HR_UserID " +
                  ") " +
                  "VALUES (" +
                        next_id +
                        ",'" + TransectionID +
                        "','" + System.DateTime.Now +
                        "'," + menu_id +
                        ",'" + Event +
                        "','" + Narration +
                       "','" + user_id +
                        "' )";

            Execute(strSQL);

        }

        // Check whether a given value is exist in table or NOT
        public Boolean CheckValue(string tblName, string FieldName, string Value)
        {
            try
            {
                cmd = new SqlCommand("SELECT COUNT(*)as cnt FROM " + tblName + " WHERE  " + FieldName + "='" + Value + "' ", con);

                long val = 0;
                val = Convert.ToInt16(cmd.ExecuteScalar());

                if (val == 0)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        // Get a value
        public string GetValue(string tblName, string SourceFieldName, string DestinationFieldName, string Value)
        {
            string val = null;
            string strSQL = "SELECT " + DestinationFieldName + " FROM " + tblName + " WHERE  " + SourceFieldName + "='" + Value + "' ";
            try
            {
                cmd = new SqlCommand(strSQL, con);
                val = cmd.ExecuteScalar().ToString();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return val;
            }
            return val;
        }
        //
        public string GetValue(string strSQL)
        {
            string val = null;
            //  string strSQL = "SELECT " + DestinationFieldName + " FROM " + tblName + " WHERE  " + SourceFieldName + "='" + Value + "' ";
            try
            {
                cmd = new SqlCommand(strSQL, con);
                val = cmd.ExecuteScalar().ToString();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return val;
            }
            return val;
        }

        //Get BANo. Army No. Rank and Name
        public string GetOfficerShortProfile(string strHR_ArmyID)
        {
            string str;
            string val = null;

            str = "SELECT OfficerShortProfile FROM vSystem_GeneralSvcInfo WHERE HR_ARMYID='" + strHR_ArmyID + "' ";
            try
            {
                cmd = new SqlCommand(str, con);
                dr = cmd.ExecuteReader();

                dr.Read();
                val = dr.GetValue(0).ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                dr.Close();
            }
            return val;
        }


        //Get Logical ID Extention(2)Year(2)Value(6)
        public string LogicalID(string TableName, string FieldName, string Extension)
        {
            string str;
            string val = Extension;
            string tmp = null;
            string tmp1, tmp2 = null;
            string yr = null;
            int l, i;

            yr = System.DateTime.Now.Year.ToString();
            yr = yr.Substring(2, 2);


            str = "SELECT RIGHT(MAX(" + FieldName + "),6) FROM " + TableName + " WHERE  SUBSTRING(" + FieldName + ",3,2)=" + yr;
            try
            {
                cmd = new SqlCommand(str, con);
                dr = cmd.ExecuteReader();

                dr.Read();
                tmp = dr.GetValue(0).ToString();

                if (tmp == "")
                {
                    val = val + yr + "000001";
                }
                else
                {
                    tmp1 = Convert.ToInt32(tmp).ToString();
                    l = Convert.ToInt32(tmp1) + 1;

                    for (i = 0; i < 6 - (l.ToString().Length); i++)
                        tmp2 = tmp2 + "0";
                    tmp2 = tmp2 + l.ToString();

                    val = val + yr + tmp2;
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                dr.Close();
            }


            return val;
        }


        // Return Army Information
        public String GetArmyInformation(string ArmyNo)
        {
            string str = @"SELECT     RankName, Name, UnitName + '/ ' + FormationName AS UnitFormationName, CorpName, PersonalPicturePath
                            FROM         dbo.vSystem_GeneralSvcInfo where armyno='" + ArmyNo + @"'";
            StringBuilder txt = new StringBuilder();
            try
            {
                cmd = new SqlCommand(str, con);
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();

                    string ppic = null;

                    if (dr.GetValue(4).ToString() == "")
                        ppic = "Image/no_picture.jpg";
                    else
                    {
                        ppic = "PMS Project/PhotoGallery/" + dr.GetValue(4).ToString() + "";

                    }



                    string tmp = @"
                    <TR><TD colspan='2' align='center'><table border='1' cellspacing='1' style='border-collapse: collapse' bordercolor='#e1e1ea' width='72%' height='170' class='MiddleBody'>
  <tr>
    <td width='100%' colspan='3' height='19'>
    <p align='left'><b><font face='Tahoma' size='2'>Personal Information</font></b></td>
  </tr>
  <tr>
    <td width='28%' align='center' rowspan='4'>

                    <img id='v_PersonalPicturePath' src='" + ppic + @"' height='90'                                width='90' border='1' >
                             </td>
    <td width='21%' height='20'><font COLOR='#800000' face='Tahoma'>&nbsp;Rank</font></td>
    <td width='51%' height='20'>
    <font face='Tahoma'>&nbsp;" + dr.GetValue(0).ToString() + @"</font></td>
  </tr>
  <tr>
    <td width='21%' height='20'><font  COLOR='#800000' face='Tahoma'>&nbsp;Name</font></td>
    <td width='51%' height='20'><font face='Tahoma'>&nbsp;" + dr.GetValue(1).ToString() + @"</font></td>
  </tr>
  <tr>
    <td width='21%' height='20'><font  COLOR='#800000' face='Tahoma'>&nbsp;
    Unit/Formation</font></td>
    <td width='51%' height='20'><font face='Tahoma'>&nbsp;" + dr.GetValue(2).ToString() + @"</font></td>
  </tr>
  <tr>
    <td width='21%' height='20'><font  COLOR='#800000' face='Tahoma'>&nbsp;
    Crop/ Staff</font></td>
    <td width='51%' height='20'><font face='Tahoma'>&nbsp;" + dr.GetValue(3).ToString() + @"</font></td>
  </tr>
</table></td></tr>";



                    txt.Append(tmp);

                }
                else
                    txt.Append("<TR><TD colspan='2' align='center'><span style='color: #ff3300'>No Information found...</span></TD></TR>");


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                dr.Close();
            }
            return txt.ToString();
        }


    }

}