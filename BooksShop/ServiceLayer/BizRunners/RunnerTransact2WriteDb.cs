using BizLogic.GenericInterfaces;
using DataLayer;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;

namespace ServiceLayer.BizRunners
{
    public class RunnerTransact2WriteDb<TIn, TPass, TOut>
        where TPass : class
        where TOut : class
    {
        private readonly IBizAction<TIn, TPass> actionPart1;
        private readonly IBizAction<TPass, TOut> actionPart2;
        private readonly DataContext dataContext;

        public bool HasErrors => Errors.Count > 0;
        public IImmutableList<ValidationResult> Errors { get; private set; }

        public RunnerTransact2WriteDb(
            DataContext dataContext,
            IBizAction<TIn, TPass> actionPart1,
            IBizAction<TPass, TOut> actionPart2)
        {
            this.dataContext = dataContext;
            this.actionPart1 = actionPart1;
            this.actionPart2 = actionPart2;
        }

        public TOut RunAction(TIn dataIn)
        {
            using (var transaction = dataContext.Database.BeginTransaction())
            {
                var passResult = RunPart(actionPart1, dataIn);
                if (HasErrors) return null;
                var result = RunPart(actionPart2, passResult);

                if (!HasErrors)
                {
                    transaction.Commit();
                }

                return result;
            }
        }

        private TPartOut RunPart<TPartIn, TPartOut>(IBizAction<TPartIn, TPartOut> bizPart, TPartIn dataIn) where TPartOut : class
        {
            TPartOut result = bizPart.Action(dataIn);
            Errors = bizPart.Errors;

            if (!HasErrors)
            {
                dataContext.SaveChanges();
            }

            return result;
        }
    }
}