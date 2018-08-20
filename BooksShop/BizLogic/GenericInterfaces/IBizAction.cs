using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;

namespace BizLogic.GenericInterfaces
{
    public interface IBizAction<in TIn, out TOut>
    {        
        //This is the action that the BizRunner will call
        TOut Action(TIn dto);
        //This returns the error information from the business logic        
        bool HasErrors { get; }
        IImmutableList<ValidationResult> Errors { get; }
    }
}
