using System.Runtime.InteropServices;

namespace AgathaSample.ServiceHost
{
    //  http://marcmelvin.com/?p=10
    public static class ConsoleHelper
    {
        [DllImport("kernel32.dll")]
        private static extern int AllocConsole();

        [DllImport("kernel32.dll")]
        private static extern int FreeConsole();

        public static void OpenConsole()
        {
            AllocConsole();
        }

        public static void CloseConsole()
        {
            FreeConsole();
        }
    }
}