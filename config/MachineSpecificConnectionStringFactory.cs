using System;
using System.Configuration;

namespace Samples.Configuration
{
    public class MachineSpecificConnectionStringFactory : IConnectionStringFactory
    {
        private readonly IConfigurationManager _configManager;

        public MachineSpecificConnectionStringFactory(IConfigurationManager configManager)
        {
            _configManager = configManager;
        }

        #region IConnectionStringFactory Members

        /// <summary>
        /// Returns the connection string specified by the 'connectionStringName'
        /// appSetting, if present. Otherwise attempts to return the connection string
        /// with the same name as the current Environment.MachineName.
        /// </summary>
        public string GetConnectionString()
        {
            var connStrName = _configManager.AppSettings["connectionStringName"];
            var specificConnStr = _configManager.ConnectionStrings[connStrName];

            if (specificConnStr == null)
            {
                var machineConnStr = GetMachineSpecificConnectionString();
                return machineConnStr;
            }

            return specificConnStr.ConnectionString;
        }

        #endregion

        /// <summary>
        /// Returns the connection string with the same name as
        /// the current Environment.MachineName.
        /// 
        /// A ConfigurationErrorsException is thrown if a
        /// matching connection string is not found.
        /// </summary>
        /// <returns></returns>
        private string GetMachineSpecificConnectionString()
        {
            var connStrName = Environment.MachineName;
            var connStr = _configManager.ConnectionStrings[connStrName];

            if (connStr == null)
            {
                throw new ConfigurationErrorsException(
                    string.Format(
                        "No connection string named \"{0}\" was found.",
                        connStrName));
            }

            return connStr.ConnectionString;
        }
    }
}