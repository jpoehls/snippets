using Agatha.Common.WCF;

namespace AgathaSample.WinClient.Security
{
    public class CustomRequestProcessorProxy : RequestProcessorProxy
    {
        private const string EndpointConfigurationName = "IRequestProcessor";

        public CustomRequestProcessorProxy(UserSecurityContext securityContext)
            : base(EndpointConfigurationName, securityContext.ServiceUrl)
        { }
    }
}