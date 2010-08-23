using System;
using AgathaSample.Services.Security;
using Ninject;

namespace AgathaSample.Services
{
    public class KernelContainer
    {
        private static IKernel _kernel;

        public static IKernel Kernel
        {
            get
            {
                if (_kernel == null)
                {
                    CreateDefaultKernel();
                }
                return _kernel;
            }
            set
            {
                if (_kernel != null)
                {
                    throw new NotSupportedException("Kernel has already been set!");
                }
                _kernel = value;
            }
        }

        private static void CreateDefaultKernel()
        {
            _kernel = new StandardKernel();
            _kernel.Bind<IUserValidator>().To<DummyUserValidator>();
        }
    }
}