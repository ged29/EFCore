using BizLogic.GenericInterfaces;
using DataLayer;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;

namespace ServiceLayer.BizRunners
{
    public class RunnerWriteDb<TIn, TOut>
    {
        private readonly IBizAction<TIn, TOut> actionClass;
        private readonly DataContext dataContext;

        public bool HasErrors => actionClass.HasErrors;
        public IImmutableList<ValidationResult> Errors => actionClass.Errors;

        public RunnerWriteDb(
            IBizAction<TIn, TOut> actionClass,
            DataContext dataContext)
        {
            this.actionClass = actionClass;
            this.dataContext = dataContext;
        }

        public TOut RunAction(TIn dataIn)
        {
            TOut result = actionClass.Action(dataIn);

            if (!HasErrors)
            {
                dataContext.SaveChanges();
            }

            return result;
        }
    }
}
