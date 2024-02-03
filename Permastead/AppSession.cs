using System;
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
        
        private AppSession()
        {
            ServiceMode = ServiceMode.Local;
            GaiaService = new GaiaService();
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
