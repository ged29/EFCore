using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataLayer.Entities
{
    public class LineItem : IValidatableObject
    {
        public int LineItemId { get; set; }

        [Range(1, 5, ErrorMessage = "This order is over the limit of 5 books.")]
        public byte LineNum { get; set; }

        public short NumBooks { get; set; }

        public decimal BookPrice { get; set; }
        //FK
        public int OrderId { get; set; }
        public int BookId { get; set; }

        public Book ChosenBook { get; set; }

        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            //var context = validationContext.GetService(typeof(DataContext));

            if (ChosenBook.Price < 0)
            {
                yield return new ValidationResult($"Sorry, the book '{ChosenBook.Title}' is not for sale.");
            }


            if (NumBooks > 100)
            {
                yield return new ValidationResult("If you want to order a 100 or more books please phone us on 01234-5678-90", new[] { nameof(NumBooks) });
            }
        }
    }
}