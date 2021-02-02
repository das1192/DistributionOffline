using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using TalukderPOS.BusinessObjects;
using TalukderPOS.DAL;

namespace TalukderPOS.BLL
{
    public class UserBLL
    {
        public UserDAL DAL = new UserDAL();


        public void Add(User entity)
        {
            try
            {
                DAL.Add(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetKEYOF()
        {
            try
            {
                return DAL.GetKEYOF();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetUserList(User entity)
        {
            try
            {
                return DAL.GetUserList(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetUserListShop(User entity)
        {
            try
            {
                return DAL.GetUserListShop(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void User_Delete(User entity)
        {
            try
            {
                DAL.User_Delete(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int User_GetMaxID()
        {
            try
            {
                return DAL.User_GetMaxID();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GenerateUserCode(string UserName)
        {
            int prevID = DAL.User_GetMaxID();
            string userCode = string.Empty;


            if (prevID != 0)
            {
                try
                {
                    userCode = (prevID + 1).ToString() + UserName.Trim().Substring(0, 3) + DateTime.Now.Year.ToString().Substring(2, 2) + DateTime.Now.DayOfYear.ToString();
                }
                catch
                {
                    userCode = "1" + UserName.Trim().Substring(0, 3) + DateTime.Now.Year.ToString().Substring(2, 2) + DateTime.Now.DayOfYear.ToString();
                }
            }
            else
            {
                userCode = "1" + UserName.Trim().Substring(0, 3) + DateTime.Now.Year.ToString().Substring(2, 2) + DateTime.Now.DayOfYear.ToString();
            }
            return userCode;
        }




        public string[] Validation(User obj)
        {
            string[] valid = new string[2];
            try
            {
                if (obj.CCOM_OID == String.Empty || obj.CCOM_OID == "0")
                {
                    valid[0] = "False";
                    valid[1] = "PLEASE SELECT A BRANCH";
                }
                else if (obj.UserFullName == String.Empty)
                {
                    valid[0] = "False";
                    valid[1] = "PLEASE INSERT USER FULL NAME";
                }
                else if (obj.UserName == String.Empty)
                {
                    valid[0] = "False";
                    valid[1] = "PLEASE INSERT USER NAME";
                }
                else if (obj.Password == String.Empty)
                {
                    valid[0] = "False";
                    valid[1] = "PLEASE INSERT PASSWORD";
                }
                else if (obj.ConfirmPassword == String.Empty)
                {
                    valid[0] = "False";
                    valid[1] = "PLEASE INSERT CONFIRM PASSWORD";
                }
                else if (obj.Password != obj.ConfirmPassword)
                {
                    valid[0] = "False";
                    valid[1] = "PLEASE CHECK PASSWORD";
                }
                else
                {
                    valid[0] = "True";
                    valid[1] = String.Empty;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return valid;
        }


        public string GetUserIDByUserNamePassword(String username, string password)
        {
            try
            {
                return DAL.GetUserIDByUserNamePassword(username, password);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable User_GetAllForDDL(string storeid)
        {
            try
            {
                return DAL.User_GetAllForDDL(storeid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public User GetById(string Id)
        {
            try
            {
                return DAL.GetById(Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string getusername(string Id)
        {
            try
            {
                return DAL.getusername(Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




    }
}
