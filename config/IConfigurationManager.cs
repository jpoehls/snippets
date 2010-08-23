using System;
using System.Collections.Specialized;
using System.Configuration;

namespace Samples.Configuration
{
    public interface IConfigurationManager
    {
        NameValueCollection AppSettings { get; }
        ConnectionStringSettingsCollection ConnectionStrings { get; }
    }
}