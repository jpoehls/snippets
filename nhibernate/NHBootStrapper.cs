using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.IO;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace Samples
{
    public class NHBootStrapper
    {
        public static ISessionFactory SessionFactory { get; private set; }

        private static Configuration Configuration { get; set; }
        private static string ConnectionString { get; set; }

        private const string Password = "{6F3A5B85-BA19-49a2-A295-8625305E423D}";

        public static void Initialize(FileInfo databaseFile)
        {
            ConnectionString = string.Format("data source={0};Password={1};Encryption Mode=Engine Default",
                                             databaseFile.FullName, Password);

            var nhProps = new Dictionary<string, string>();
            nhProps.Add("connection.driver_class", typeof(CustomSqlServerCeDriver).AssemblyQualifiedName);
            nhProps.Add("dialect", "NHibernate.Dialect.MsSqlCeDialect");
            nhProps.Add("connection.provider", "NHibernate.Connection.DriverConnectionProvider");
            nhProps.Add("connection.connection_string", ConnectionString);

            Configuration = new Configuration();
            Configuration.Properties = nhProps;
            Configuration.AddAssembly(typeof(NHBootStrapper).Assembly);
            SessionFactory = Configuration.BuildSessionFactory();

            if (!databaseFile.Exists)
            {
                if (databaseFile.Directory != null && !databaseFile.Directory.Exists)
                {
                    databaseFile.Directory.Create();
                }

                CreateDatabase();
            }
        }

        protected static void CreateDatabase()
        {
            var engine = new SqlCeEngine(ConnectionString);
            engine.CreateDatabase();

						//	let nhibernate create the database schema automagically
						//	by using our mapping files to infer table schemas
            var export = new SchemaExport(Configuration);
            export.Execute(false, true, false, false);
        }
    }
}