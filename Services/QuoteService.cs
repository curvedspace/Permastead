using System;
using System.Collections.Generic;
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
            var userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            if (mode == ServiceMode.Local)
            {
                rtnValue = QuoteRepository.GetRandomQuote(DataConnection.GetLocalDataSource());
            }
            else
            {

            }
            

            return rtnValue;
        }
    }
}
