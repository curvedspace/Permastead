using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataAccess;
using DataAccess.Local;

using Models;

namespace Services
{
    public static class QuoteService
    {
        /// <summary>
        /// Gets a random quotation from the database.
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static Quote GetRandomQuote(ServiceMode mode)
        {
            var rtnValue = new Quote();

            if (mode == ServiceMode.Local)
            {
                rtnValue = QuoteRepository.GetRandomQuote(DataConnection.GetLocalDataSource());
            }
            else
            {
                rtnValue = DataAccess.Server.QuoteRepository.GetRandomQuote(DataConnection.GetServerConnectionString());
            }
            

            return rtnValue;
        }
        
        public static DataTable GetAllQuotes(ServiceMode mode)
        {
            var rtnValue = new DataTable();
            

            if (mode == ServiceMode.Local)
            {
                rtnValue = QuoteRepository.GetAll(DataConnection.GetLocalDataSource());
            }
            else
            {
                rtnValue = DataAccess.Server.QuoteRepository.GetAll(DataConnection.GetServerConnectionString());
            }

            return rtnValue;
        }
        
        public static bool DeleteQuote(ServiceMode mode, Quote quote)
        {
            bool rtnValue;
            

            if (mode == ServiceMode.Local)
            {
                rtnValue = QuoteRepository.Delete(DataConnection.GetLocalDataSource(), quote);
            }
            else
            {
                rtnValue = DataAccess.Server.QuoteRepository.Delete(DataConnection.GetServerConnectionString(), quote);
            }

            return rtnValue;
        }
        
    }
}
