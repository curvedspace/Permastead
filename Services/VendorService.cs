using DataAccess.Local;
using DataAccess;
using Models;

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
            else
            {
                vendorList = DataAccess.Server.VendorRepository.GetAll(DataConnection.GetServerConnectionString());
            }

            return vendorList;
        }
        
        public static bool CommitRecord(ServiceMode mode, Vendor? vendor)
        {
            bool rtnValue = false;
        
            if (vendor != null)
            {
                if (vendor.Id > 0)
                {
                    if (mode == ServiceMode.Local)
                    {
                        VendorRepository.Update(vendor);
                    }
                    else
                    {
                        DataAccess.Server.VendorRepository.Update(vendor);
                    }
                }
                else
                {
                    // insert new record
                    if (mode == ServiceMode.Local)
                    {
                        VendorRepository.Insert(vendor);
                    }
                    else
                    {
                        DataAccess.Server.VendorRepository.Insert(vendor);
                    }
                }
            }

            return rtnValue;
        }
    }
}
