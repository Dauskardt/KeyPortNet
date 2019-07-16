using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace KeyPortNet
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //Mutex mit eindeutigem Namen (bspw. GUID)
            Mutex mutex = new Mutex(true, "cbb99b16-cc52-47b1-b734-c0f4900ee23c");

            //Prüfung, ob Mutex schon länger aktiv ist..
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                //Mutex ist gerade gestartet..
                base.OnStartup(e);
            }
            else
            {
                //Mutex läuft bereits längere Zeit..
                MessageBox.Show("Anwendung läuft bereits!");
                //Anwendung beenden
                Environment.Exit(0);
            }

        }
    }
}
