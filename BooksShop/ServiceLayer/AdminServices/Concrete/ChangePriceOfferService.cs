using DataLayer;
using DataLayer.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ServiceLayer.AdminServices.Concrete
{
    public class ChangePriceOfferService
    {
        private readonly DataContext dataContex;

        public ChangePriceOfferService(DataContext dataContex)
        {
            this.dataContex = dataContex;
        }

        public Book OrgBook { get; private set; }

        public PriceOffer GetOriginal(int bookId)
        {
            OrgBook = dataContex.Books
                .Include(book => book.Promotion)
                .Single(book => book.BookId == bookId);

            return OrgBook?.Promotion ?? new PriceOffer
            {
                BookId = bookId,
                NewPrice = OrgBook.Price
            };
        }

        public Book UpdateBook(PriceOffer priceOffer)
        {
            var bookToUpdate = dataContex.Books
                .Include(book => book.Promotion)
                .Single(book => book.BookId == priceOffer.BookId);

            if (bookToUpdate.Promotion == null)
            {
                bookToUpdate.Promotion = priceOffer;
            }
            else
            {
                bookToUpdate.Promotion.PromotionalText = priceOffer.PromotionalText;
                bookToUpdate.Promotion.NewPrice = priceOffer.NewPrice;
            }

            dataContex.SaveChanges();

            return bookToUpdate;
        }
    }
}