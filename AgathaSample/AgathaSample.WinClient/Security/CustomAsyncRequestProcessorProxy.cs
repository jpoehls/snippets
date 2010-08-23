using Agatha.Common.WCF;

namespace AgathaSample.WinClient.Security
{
    public class CustomAsyncRequestProcessorProxy : AsyncRequestProcessorProxy
    {
        private const string EndpointConfigurationName = "IRequestProcessor";

        public CustomAsyncRequestProcessorProxy(UserSecurityContext securityContext)
            : base(EndpointConfigurationName, securityContext.ServiceUrl)
        { }
    }
}