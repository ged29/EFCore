using DataLayer.Entities;
using ServiceLayer.AdminServices.Dtos;

namespace ServiceLayer.AdminServices.Concrete
{
    public interface IChangePubDateService
    {
        ChangePubDateDto GetOriginal(int bookId);
        Book UpdateBook(ChangePubDateDto changePubDate);
    }
}