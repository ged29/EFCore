using DataLayer.Entities;

namespace ServiceLayer.AdminServices.Concrete
{
    public interface IChangePriceOfferService
    {
        Book OrgBook { get; }

        PriceOffer GetOriginal(int bookId);
        Book UpdateBook(PriceOffer priceOffer);
    }
}