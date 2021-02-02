using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using TalukderPOS.BusinessObjects;
using TalukderPOS.DAL;


namespace TalukderPOS.BLL
{
    public class NewsFeed_BILL
    {
        NewsFeed_DAL DAL = new NewsFeed_DAL();

        public void Add(NewsFeed_BO entity)
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



        public void Delete(NewsFeed_BO entity)
        {
            try
            {
                DAL.Delete(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
