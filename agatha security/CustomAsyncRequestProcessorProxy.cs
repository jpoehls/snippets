using Agatha.Common.WCF;
using Samples.Client.Services;

namespace Samples.Client.WCF
{
    public class CustomAsyncRequestProcessorProxy : AsyncRequestProcessorProxy
    {
        private const string EndpointConfigurationName = "IRequestProcessor";

        public CustomAsyncRequestProcessorProxy(SecurityContext securityContext)
            : base(EndpointConfigurationName, securityContext.ServiceUrl)
        { }
    }
}