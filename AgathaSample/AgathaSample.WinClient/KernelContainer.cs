using System;
using Ninject;

namespace AgathaSample.WinClient
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
        }
    }
}