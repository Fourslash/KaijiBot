﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


using System.Diagnostics;


namespace KaijiBot
{
    class Program
    {
        static void Main(string[] args)
        {
            var connector = new Proxy.ProcessConnector();
            var prc = connector.Connect("chrome");
            var ts = new Proxy.GameProxy(prc);
            var gameEventEmmiter = new Game.EventEmitter(ts);
            var table = new Game.Table(gameEventEmmiter);
            while (true) ;

        }       
    }
}
