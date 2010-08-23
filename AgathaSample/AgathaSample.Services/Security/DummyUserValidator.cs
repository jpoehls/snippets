using AgathaSample.Common;

namespace AgathaSample.Services.Security
{
    public class DummyUserValidator : IUserValidator
    {
        public bool ValidateCredentials(SecurityCredentials credentials)
        {
            //  sure, any user is valid! why not?
            return true;
        }
    }
}