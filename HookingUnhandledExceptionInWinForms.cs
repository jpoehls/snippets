public class Program
{
	public static void Main()
	{
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
	}

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {          
            var ex = e.ExceptionObject as Exception;
            HandleUnhandledException(ex, e.IsTerminating);
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            HandleUnhandledException(e.Exception, false);
        }

        /// <summary>
        /// Logs and displays the unhandled exception message.
        /// Exits the application if needed.
        /// </summary>
        /// <param name="ex">Exception that was thrown.</param>
        /// <param name="isTerminating">Indicates whether the common language runtime is terminating.</param>
        private static void HandleUnhandledException(Exception ex, bool isTerminating)
        {
            var errorHResult = System.Runtime.InteropServices.Marshal.GetHRForException(ex);
            if (errorHResult != 0)
            {
                string hexValue = string.Format("{0:x}", errorHResult);
                Log.Fatal("HResult: " + hexValue, ex);
            }
            else
            {
                Log.Fatal(ex);
            }

            try
            {
		MessageBox.Show(ex.ToString());
                Environment.Exit(0);
            }
            catch
            {
                //  re-throwing here could re-enter this handler
                //  causing a potentially endless loop
                Log.Fatal(ex);
            }
        }
}