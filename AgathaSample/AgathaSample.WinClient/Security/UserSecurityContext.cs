using System;
using System.Configuration;
using AgathaSample.Common;

namespace AgathaSample.WinClient.Security
{
    /// <summary>
    /// Security context for the current user session.
    /// </summary>
    public class UserSecurityContext
    {
        /// <summary>
        /// URL of the service we are authenticating against.
        /// </summary>
        public string ServiceUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["ServiceEndpointUrl"];
            }
        }

        public SecurityCredentials Credentials { get; set; }
    }
}