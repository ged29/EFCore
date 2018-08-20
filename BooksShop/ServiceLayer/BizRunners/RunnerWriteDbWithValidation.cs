using BizLogic.GenericInterfaces;
using DataLayer;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;

namespace ServiceLayer.BizRunners
{
    public class RunnerWriteDbWithValidation<TIn, TOut>
    {
        private readonly IBizAction<TIn, TOut> actionClass;
        private readonly DataContext dataContext;

        public bool HasErrors => Errors.Count > 0;
        public IImmutableList<ValidationResult> Errors { get; private set; }

        public RunnerWriteDbWithValidation(
            IBizAction<TIn, TOut> actionClass,
            DataContext dataContext)
        {
            this.actionClass = actionClass;
            this.dataContext = dataContext;
        }

        public TOut RunAction(TIn dataIn)
        {
            TOut result = actionClass.Action(dataIn);
            Errors = actionClass.Errors;

            if (!HasErrors)
            {
                Errors = dataContext.SaveChangesWithValidation();
            }

            return result;
        }
    }
}
