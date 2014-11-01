using ScreenOverlayManager.Properties;
using System;
using System.IO;
using System.Windows;

namespace ScreenOverlayManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private StreamWriter LogStream;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //
            // Redirect output from console / debugging to a text file on disc
            if(Settings.Default.Debugging && !string.IsNullOrWhiteSpace(Settings.Default.LogfilePath))
            {
                LogStream = new StreamWriter
                (
                    Settings.Default.LogfilePath,
                    true
                );

                LogStream.AutoFlush = true;
                
                Console.SetOut(LogStream);

                Extender.Debugging.Debug.WriteMessage
                (
                    "Application Startup.",
                    "info"
                );
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            try
            {
                if (LogStream != null)
                {
                    Extender.Debugging.Debug.WriteMessage
                    (
                        "Application exiting.",
                        "info"
                    );

                    // HACK Waits for the write queue in Extender.Debugging.NonBlockingConsole
                    //      to finish writing, then flushes the stream before disposal.
                    System.Threading.Thread.Sleep(100);
                    LogStream.Flush();
                }
            }
            finally
            {
                LogStream.Dispose();
            }

            base.OnExit(e);
        }
    }
}
