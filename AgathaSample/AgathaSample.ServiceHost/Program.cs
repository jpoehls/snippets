using System;
using System.ServiceProcess;

namespace AgathaSample.ServiceHost
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            if (!Environment.UserInteractive)
            {
                var ServicesToRun = new ServiceBase[]
                                        {
                                            new WcfRequestProcessorService()
                                        };
                ServiceBase.Run(ServicesToRun);
            }
            else
            {
                ConsoleHelper.OpenConsole();
                Console.WriteLine("Service host started. Press ENTER to stop service.");

                var service = new WcfRequestProcessorService();
                service.Start();
                Console.ReadLine();
                service.Shutdown();

                ConsoleHelper.CloseConsole();
            }
        }
    }
}