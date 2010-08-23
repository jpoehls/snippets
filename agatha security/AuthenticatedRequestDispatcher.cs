using System;
using Agatha.Common;
using Samples.Client.Services;
using Samples.Common.Operations;

namespace Samples.Client.WCF
{
    public class AuthenticatedRequestDispatcher : RequestDispatcher
    {
        private readonly SecurityContext _securityContext;

        public AuthenticatedRequestDispatcher(IRequestProcessor requestProcessor, SecurityContext securityContext) : base(requestProcessor)
        {
            _securityContext = securityContext;
        }

        protected override void BeforeSendingRequests(System.Collections.Generic.IEnumerable<Request> requestsToProcess)
        {
            base.BeforeSendingRequests(requestsToProcess);

            foreach (var req in requestsToProcess)
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