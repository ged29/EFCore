using BizLogic.GenericInterfaces;
using DataLayer;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ServiceLayer.BizRunners
{
    public class RunnerWriteDbWithValidationAsync<TIn, TOut>
    {
        private readonly IBizActionAsync<TIn, TOut> actionClass;
        private readonly DataContext dataContext;

        public bool HasErrors => Errors.Count > 0;
        public IImmutableList<ValidationResult> Errors { get; private set; }

        public RunnerWriteDbWithValidationAsync(
            IBizActionAsync<TIn, TOut> actionClass,
            DataContext dataContext)
        {
            this.actionClass = actionClass;
            this.dataContext = dataContext;
        }

        public async Task<TOut> RunActionAsync(TIn dataIn)
        {
            TOut result = await actionClass.ActionAsync(dataIn).ConfigureAwait(false);
            Errors = actionClass.Errors;

            if (!HasErrors)
            {
                Errors = await dataContext.SaveChangesWithValidationAsync().ConfigureAwait(false);
            }

            return result;
        }
    }
}
