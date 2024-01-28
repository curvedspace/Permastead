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
    }
}
