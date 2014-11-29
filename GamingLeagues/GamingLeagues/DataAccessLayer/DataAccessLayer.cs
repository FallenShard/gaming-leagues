using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Mapping;

using GamingLeagues.Mappings;

namespace GamingLeagues.DataAccessLayer
{
    public class DataAccessLayer
    {
        private static ISessionFactory m_factory = null;

        private static string m_dbFile = "GamingLeagues.db";

        // Opens the session on demand, akin to singleton pattern
        public static ISession GetSession()
        {
            // In case the factory isn't initialized, create it
            if (m_factory == null)
            {
                m_factory = CreateSessionFactory();
            }

            return m_factory.OpenSession();
        }

        // Configures and creates session factory, if a database file exists, it's created upon that file
        // else a new file is created
        private static ISessionFactory CreateSessionFactory()
        {
            if (File.Exists(m_dbFile))
            {
                return Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.UsingFile(m_dbFile))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<PlayerMapping>())
                .BuildSessionFactory();
                
            }
            else
            {
                return Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.UsingFile(m_dbFile))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<PlayerMapping>())
                .ExposeConfiguration(CreateDatabase)
                .BuildSessionFactory();
            }
        }

        // In case the file doesn't exist, create a new database
        private static void CreateDatabase(Configuration config)
        {
            // This NHibernate tool takes a configuration (with mapping info in)
            // and exports a database schema from it
            new SchemaExport(config).Create(false, true);
        }
    }
}
