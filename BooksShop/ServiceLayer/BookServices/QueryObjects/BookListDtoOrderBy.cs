using ServiceLayer.BookServices.Dtos;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ServiceLayer.BookServices.QueryObjects
{
    public enum BookOrderBy
    {
        [Display(Name = "sort by...")]
        SimpleOrder = 0,
        [Display(Name = "Votes ↑")]
        ByVotes,
        [Display(Name = "Publication Date ↑")]
        ByPublicationDate,
        [Display(Name = "Price ↓")]
        ByPriceLowestFirst,
        [Display(Name = "Price ↑")]
        ByPriceHigestFirst
    }

    public static class BookListDtoOrderBy
    {
        public static IQueryable<BookListDto> OrderBy(
            this IQueryable<BookListDto> books, BookOrderBy orderBy)
        {
            switch (orderBy)
            {
                case BookOrderBy.SimpleOrder:
                    return books.OrderByDescending(b => b.BookId);

                case BookOrderBy.ByVotes:
                    return books.OrderByDescending(b => b.ReviewsAverageVotes);

                case BookOrderBy.ByPublicationDate:
                    return books.OrderByDescending(b => b.PublishedOn);

                case BookOrderBy.ByPriceLowestFirst:
                    return books.OrderBy(b => b.Price);

                case BookOrderBy.ByPriceHigestFirst:
                    return books.OrderByDescending(b => b.Price);

                default: throw new ArgumentOutOfRangeException(nameof(orderBy), orderBy, null);
            }
        }
    }
}
