using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DataLayer
{
    public class MigrationHelper : IDesignTimeDbContextFactory<DataContext>
    {
        private const string ConnectionString =
            "Server=.;Database=MyEFCoreInActionDb;Trusted_Connection=True;MultipleActiveResultSets=true";

        public DataContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseSqlServer(ConnectionString);

            return new DataContext(optionsBuilder.Options);
        }
    }
}

// Add-Migration Chapter02 -Project DataLayer -StartupProject EfCoreInAction
// Update-database -Project DataLayer -StartupProject EfCoreInAction