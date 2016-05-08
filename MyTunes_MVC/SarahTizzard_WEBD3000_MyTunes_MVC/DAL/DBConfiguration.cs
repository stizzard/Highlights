using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace SarahTizzard_WEBD3000_MyTunes_MVC.DAL
{
    public class DBConfiguration : DbConfiguration
    {
        public DBConfiguration()
        {
            SetExecutionStrategy("System.Data.SqlClient", () => new SqlAzureExecutionStrategy());
        }
    }
}