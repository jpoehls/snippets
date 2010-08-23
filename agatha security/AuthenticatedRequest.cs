using System;
using System.Linq;
using Agatha.Common;
using Samples.Common.Operations.DataContracts;
using Samples.Security;

namespace Samples.Common.Operations
{
    public abstract class AuthenticatedRequest : Request
    {
        public SecurityCredentials Credentials { get; set; }
    }
}