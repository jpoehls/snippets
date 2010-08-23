using System;
using System.Linq;
using AgathaSample.Common;

namespace AgathaSample.Services.Security
{
    public interface IUserValidator
    {
        bool ValidateCredentials(SecurityCredentials credentials);
    }
}