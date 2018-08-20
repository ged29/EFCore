using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace BizLogic.GenericInterfaces
{
    public interface IBizActionAsync<in TIn, TOut>
    {
        //This is the action that the BizRunner will call
        Task<TOut> ActionAsync(TIn dto);
        //This returns the error information from the business logic        
        bool HasErrors { get; }
        IImmutableList<ValidationResult> Errors { get; }
    }
}
