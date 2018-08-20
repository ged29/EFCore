using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;

namespace BizLogic.GenericInterfaces
{
    public abstract class BizActionErrors
    {
        private readonly List<ValidationResult> errors = new List<ValidationResult>();
        public IImmutableList<ValidationResult> Errors => errors.ToImmutableList();
        public bool HasErrors => errors.Count > 0;

        protected void AddError(string errorMessage, params string[] memberNames)
        {
            errors.Add(new ValidationResult(errorMessage, memberNames));
        }
    }
}
