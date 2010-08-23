using System;

namespace Samples.Security
{
    public interface IUserValidator
    {
        bool ValidateCredentials(SecurityCredentials credentials);
    }
}