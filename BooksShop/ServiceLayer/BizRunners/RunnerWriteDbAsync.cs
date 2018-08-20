using BizLogic.GenericInterfaces;
using DataLayer;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ServiceLayer.BizRunners
{
    public class RunnerWriteDbAsync<TIn, TOut>
    {
        private readonly IBizActionAsync<TIn, TOut> actionClass;
        private readonly DataContext dataContext;

        public bool HasErrors => actionClass.HasErrors;
        public IImmutableList<ValidationResult> Errors => actionClass.Errors;

        public RunnerWriteDbAsync(
            IBizActionAsync<TIn, TOut> actionClass,
            DataContext dataContext)
        {
            this.actionClass = actionClass;
            this.dataContext = dataContext;
        }

        public async Task<TOut> RunAction(TIn dataIn)
        {
            var result = await actionClass.ActionAsync(dataIn).ConfigureAwait(false);

            if (!HasErrors)
            {
                await dataContext.SaveChangesAsync();
            }

            return result;
        }
    }
}
