using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DataLayer
{
    public static class DbContextValidationHelper
    {
        //see https://blogs.msdn.microsoft.com/dotnet/2016/09/29/implementing-seeding-custom-conventions-and-interceptors-in-ef-core-1-0/
        //for why I call DetectChanges before ChangeTracker, and why I then turn ChangeTracker.AutoDetectChangesEnabled off/on around SaveChanges
        public static async Task<ImmutableList<ValidationResult>> SaveChangesWithValidationAsync(this DataContext dataContext)
        {
            var result = dataContext.ExecuteValidation();
            if (result.Count > 1) return result;

            dataContext.ChangeTracker.AutoDetectChangesEnabled = false;

            try
            {
                await dataContext.SaveChangesAsync().ConfigureAwait(false);
            }
            finally
            {
                dataContext.ChangeTracker.AutoDetectChangesEnabled = true;
            }

            return result;
        }

        //see https://blogs.msdn.microsoft.com/dotnet/2016/09/29/implementing-seeding-custom-conventions-and-interceptors-in-ef-core-1-0/
        //for why I call DetectChanges before ChangeTracker, and why I then turn ChangeTracker.AutoDetectChangesEnabled off/on around SaveChanges
        public static ImmutableList<ValidationResult> SaveChangesWithValidation(this DataContext dataContext)
        {
            var result = dataContext.ExecuteValidation(); //#C
            if (result.Count > 1) return result;   //#D

            //I leave out the AutoDetectChangesEnabled on/off from the code shown in chapter 4 as its only a performance issue
            //I'ts a concept that doesn't add anything to chapter 4. However I leave it in the real code as it has a (small) improvement on performance
            dataContext.ChangeTracker.AutoDetectChangesEnabled = false; //LEAVE OUT OF CHAPTER 4 - 

            try
            {
                dataContext.SaveChanges();
            }
            finally
            {
                dataContext.ChangeTracker.AutoDetectChangesEnabled = true;
            }

            return result;
        }

        public static ImmutableList<ValidationResult> ExecuteValidation(this DataContext dataContext)
        {
            var result = new List<ValidationResult>();
            var entries = dataContext.ChangeTracker.Entries().Where(ee => (ee.State == EntityState.Added || ee.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                var entity = entry.Entity;
                var valProvider = new ValidationDbContextServiceProvider(dataContext);
                var valContext = new ValidationContext(entity, valProvider, null);
                var valErrors = new List<ValidationResult>();

                if (!Validator.TryValidateObject(entity, valContext, valErrors, true))
                {
                    result.AddRange(valErrors);
                }
            }

            return result.ToImmutableList();
        }
    }
}
