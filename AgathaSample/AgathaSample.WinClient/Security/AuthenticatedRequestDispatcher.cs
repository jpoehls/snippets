using System.Collections.Generic;
using Agatha.Common;
using AgathaSample.Common.RequestsAndResponses;

namespace AgathaSample.WinClient.Security
{
    public class AuthenticatedRequestDispatcher : RequestDispatcher
    {
        private readonly UserSecurityContext _securityContext;

        public AuthenticatedRequestDispatcher(IRequestProcessor requestProcessor, UserSecurityContext securityContext)
            : base(requestProcessor)
        {
            _securityContext = securityContext;
        }

        protected override void BeforeSendingRequests(IEnumerable<Request> requestsToProcess)
        {
            base.BeforeSendingRequests(requestsToProcess);

            foreach (Request req in requestsToProcess)
            {
                var authReq = req as AuthenticatedRequest;
                if (authReq != null)
                {
                    authReq.Credentials = _securityContext.Credentials;
                }
            }
        }
    }
}