using DataLayer;
using ServiceLayer.AdminServices.Dtos;
using System.Linq;
using DataLayer.Entities;

namespace ServiceLayer.AdminServices.Concrete
{
    public class ChangePubDateService : IChangePubDateService
    {
        private readonly DataContext dataContext;

        public ChangePubDateService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public ChangePubDateDto GetOriginal(int bookId)
        {
            return dataContext
                .Books
                .Select(book => new ChangePubDateDto
                {
                    BookId = book.BookId,
                    Title = book.Title,
                    PublishedOn = book.PublishedOn
                })
                .Single(x => x.BookId == bookId);
        }

        public Book UpdateBook(ChangePubDateDto changePubDate)
        {
            Book book = dataContext.Books.Find(changePubDate.BookId);
            book.PublishedOn = changePubDate.PublishedOn;

            dataContext.SaveChanges();

            return book;
        }
    }
}
