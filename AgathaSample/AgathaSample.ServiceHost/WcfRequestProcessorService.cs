using System;
using System.Reflection;
using System.Security;
using System.ServiceProcess;
using Agatha.Ninject;
using Agatha.ServiceLayer;
using Agatha.ServiceLayer.WCF;
using AgathaSample.Services;

namespace AgathaSample.ServiceHost
{
    public partial class WcfRequestProcessorService : ServiceBase
    {
        private System.ServiceModel.ServiceHost _host;

        public WcfRequestProcessorService()
        {
            InitializeComponent();
        }

        public void Start()
        {
            InitializeAgatha();

            _host = new System.ServiceModel.ServiceHost(typeof (WcfRequestProcessor));

            _host.Open();
        }

        private static void InitializeAgatha()
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

        public void Shutdown()
        {
            _host.Close();
        }

        protected override void OnStart(string[] args)
        {
            Start();
        }

        protected override void OnStop()
        {
            Shutdown();
        }
    }
}