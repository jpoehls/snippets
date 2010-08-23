using System;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Web;
using Agatha.Ninject;
using Agatha.ServiceLayer;
using AgathaSample.Services;

namespace AgathaSample.WebServiceHost
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            var c = new ServiceLayerConfiguration(
                Assembly.Load("AgathaSample.Services"),
                Assembly.Load("AgathaSample.Common"),
                new Container(KernelContainer.Kernel))
                        {
                            SecurityExceptionType = typeof (SecurityException)
                        };
            c.Initialize();
        }

        protected void Session_Start(object sender, EventArgs e)
        {
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        protected void Application_Error(object sender, EventArgs e)
        {
        }

        protected void Session_End(object sender, EventArgs e)
        {
        }

        protected void Application_End(object sender, EventArgs e)
        {
        }
    }
}