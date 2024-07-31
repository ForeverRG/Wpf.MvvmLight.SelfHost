﻿using Microsoft.Owin.Hosting;

namespace Wpf.MvvmLight.SelfHost.Api
{
    public class Program
    {
        public static void Main()
        {
            string baseAddress = "http://localhost:9000/";
            // Start OWIN host
            WebApp.Start<Startup>(url: baseAddress);
        }
    }
}

