using System;

namespace DataLayer
{
    public class ValidationDbContextServiceProvider : IServiceProvider
    {
        private readonly DataContext dataContext;

        public ValidationDbContextServiceProvider(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(DataContext))
            {
                return this.dataContext;
            }
            return null;
        }
    }
}
