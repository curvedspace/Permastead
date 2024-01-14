﻿using System;
using Models;
using Services;

namespace Permastead
{
    public class AppSession
    {
        public static ServiceMode ServiceMode { get; set; } = ServiceMode.Local;

        public GaiaService GaiaService;

        public ScoreBoard CurrentScoreboard = new ScoreBoard();

        
        private AppSession()
        {
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
