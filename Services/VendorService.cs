using DataAccess.Local;
using DataAccess;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public static class VendorService
    {
        public static List<Vendor> GetAll(ServiceMode mode)
        {
            var vendorList = new List<Vendor>();

            if (mode == ServiceMode.Local)
            {
                vendorList = VendorRepository.GetAll(DataConnection.GetLocalDataSource());
            }

            return vendorList;
        }
    }
}
