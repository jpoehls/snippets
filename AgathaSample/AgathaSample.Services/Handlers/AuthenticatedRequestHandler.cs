using System;
using System.Security;
using Agatha.Common;
using Agatha.ServiceLayer;
using AgathaSample.Common.RequestsAndResponses;
using AgathaSample.Services.Security;

namespace AgathaSample.Services.Handlers
{
    /// <summary>
    /// Base request handler that authenticates the user's
    /// credentials before the request is handled.
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public abstract class AuthenticatedRequestHandler<TRequest, TResponse> : RequestHandler<TRequest, TResponse>
        where TRequest : AuthenticatedRequest
        where TResponse : Response
    {
        private readonly IUserValidator _userValidator;

        protected AuthenticatedRequestHandler(IUserValidator userValidator)
        {
            _userValidator = userValidator;
        }

        public override void BeforeHandle(TRequest request)
        {
            base.BeforeHandle(request);

            bool isValid = _userValidator.ValidateCredentials(request.Credentials);
            if (!isValid)
            {
                throw new SecurityException(
                    "Access denied. Credentials are invalid or do not have the necessary permissions to perform this action.");
            }
        }
    }
}