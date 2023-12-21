using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Services;

namespace Permastead
{
    public static class AppSession
    {
        public static ServiceMode ServiceMode { get; set; } = ServiceMode.Local;

        public static GaiaService GaiaService { get; } = new GaiaService();

    }
}
