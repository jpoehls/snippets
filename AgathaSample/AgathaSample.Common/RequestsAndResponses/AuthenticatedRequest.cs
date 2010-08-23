using Agatha.Common;

namespace AgathaSample.Common.RequestsAndResponses
{
    public abstract class AuthenticatedRequest : Request
    {
        public SecurityCredentials Credentials { get; set; }
    }
}