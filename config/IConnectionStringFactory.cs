using System;

namespace Samples.Configuration
{
    public interface IConnectionStringFactory
    {
        string GetConnectionString();
    }
}