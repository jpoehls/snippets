using System;
using Agatha.Common.WCF;
using Samples.Client.Services;

namespace Samples.Client.WCF
{
    public class CustomRequestProcessorProxy : RequestProcessorProxy
    {
        private const string EndpointConfigurationName = "IRequestProcessor";

        public CustomRequestProcessorProxy(SecurityContext securityContext)
            : base(EndpointConfigurationName, securityContext.ServiceUrl)
        { }
    }
}