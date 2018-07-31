using DataLayer.Entities;
using ServiceLayer.BookServices.Dtos;
using System;
using System.Linq;

namespace ServiceLayer.BookServices.QueryObjects
{
    public static class BookListDtoSelect
    {
        public static IQueryable<BookListDto> MapToDto(this IQueryable<Book> books)
        {
            return books.Select(b => new BookListDto
            {
                BookId = b.BookId,
                Title = b.Title,
                PublishedOn = b.PublishedOn,
                Price = b.Price,
                ActualPrice = b.Promotion == null ? b.Price : b.Promotion.NewPrice,
                PromotionPromotionalText = b.Promotion == null ? null : b.Promotion.PromotionalText,
                AuthorsOrdered = String.Join(", ", b.AuthorsLink.OrderBy(a => a.Order).Select(a => a.Author.Name)),
                ReviewsCount = b.Reviews.Count,
                ReviewsAverageVotes = b.Reviews.Select(r => (double?)r.NumStars).Average()
            });
        }
    }
}
