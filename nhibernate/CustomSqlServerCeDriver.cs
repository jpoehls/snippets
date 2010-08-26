using System;
using System.Data;
using System.Data.SqlServerCe;
using System.Linq;
using NHibernate.Driver;
using NHibernate.SqlTypes;

namespace Samples
{
    /// <summary>
    /// Overridden Nhibernate SQL CE Driver,
    /// so that ntext fields are not truncated at 4000 characters
    /// and so that image fields are not truncated at 8000 bytes
    /// </summary>
    public class CustomSqlServerCeDriver : SqlServerCeDriver
    {
        protected override void InitializeParameter(IDbDataParameter dbParam, string name, SqlType sqlType)
        {
            base.InitializeParameter(dbParam, name, sqlType);

            if (sqlType is StringClobSqlType)
            {
                var parameter = (SqlCeParameter)dbParam;
                parameter.SqlDbType = SqlDbType.NText;
            }
            else if (sqlType is BinarySqlType)
            {
                var parameter = (SqlCeParameter)dbParam;
                parameter.SqlDbType = SqlDbType.Image;
            }
        }
    }
}