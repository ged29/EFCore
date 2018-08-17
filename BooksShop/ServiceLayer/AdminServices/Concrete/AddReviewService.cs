using DataLayer;
using DataLayer.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ServiceLayer.AdminServices.Concrete
{
    public class AddReviewService
    {
        private readonly DataContext dataContext;

        public AddReviewService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public string BookTitle { get; private set; }

        public Review GetBlankReview(int bookId)
        {
            BookTitle = dataContext.Books
                .Where(book => book.BookId == bookId)
                .Select(book => book.Title)
                .Single();

            return new Review { BookId = bookId };
        }

        public Book AddReviewToBook(Review review)
        {
            Book bookToAdd = dataContext.Books
                .Include(book => book.Reviews)
                .Single(book => book.BookId == review.BookId);

            bookToAdd.Reviews.Add(review);
            dataContext.SaveChanges();

            return bookToAdd;
        }
    }
}
