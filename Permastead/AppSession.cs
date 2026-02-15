using System;
using System.Collections.Concurrent;
using Models;
using Services;

namespace Permastead
{
    public class AppSession
    {
        public static ServiceMode ServiceMode { get; set; }

        public GaiaService GaiaService;

        public ScoreBoard CurrentScoreboard = new ScoreBoard();

        public Person CurrentUser = new Person();
        
        // public ConcurrentDictionary<Guid, AlertItem> Alerts = new ConcurrentDictionary<Guid, AlertItem>();
        
        public AlertManager AlertManager = new AlertManager();
        
        private AppSession()
        {
            // if we have a connection string for a postgresql server, start up in server mode
            try
            {
                if (!string.IsNullOrEmpty(DataAccess.DataConnection.GetServerConnectionString()))
                {
                    ServiceMode = ServiceMode.Server;
                }
                else
                {
                    ServiceMode = ServiceMode.Local;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ServiceMode = ServiceMode.Local;
            }

            try
            {
                GaiaService = new GaiaService(ServiceMode);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
        }
        
        private static readonly Lazy<AppSession> lazy = new Lazy<AppSession>(() => new AppSession());
        
        public static AppSession Instance
        {
            get
            {
                return lazy.Value;
            }
        }
    }
}
