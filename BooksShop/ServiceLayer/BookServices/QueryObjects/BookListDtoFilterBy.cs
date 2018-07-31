using ServiceLayer.BookServices.Dtos;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ServiceLayer.BookServices.QueryObjects
{
    public enum BooksFilterBy
    {
        [Display(Name = "All")]
        NoFilter = 0,
        [Display(Name = "By Votes...")]
        ByVotes,
        [Display(Name = "By Year published...")]
        ByPublicationYear
    }

    public static class BookListDtoFilterBy
    {
        public const string AllBooksNotPublishedString = "Coming Soon";

        public static IQueryable<BookListDto> FilterBy(
            this IQueryable<BookListDto> books, BooksFilterBy filterBy, string filterValue)
        {
            if (string.IsNullOrEmpty(filterValue))
            {
                return books; //nothing to filter
            }

            switch (filterBy)
            {
                case BooksFilterBy.NoFilter:
                    return books;

                case BooksFilterBy.ByVotes:
                    var voteValue = int.Parse(filterValue);
                    return books.Where(b => b.ReviewsAverageVotes >= voteValue);

                case BooksFilterBy.ByPublicationYear:
                    if (filterValue == AllBooksNotPublishedString)
                    {
                        return books.Where(b => b.PublishedOn > DateTime.UtcNow);
                    }

                    var publicationYearValue = int.Parse(filterValue);
                    return books.Where(b => b.PublishedOn.Year == publicationYearValue);

                default: throw new ArgumentOutOfRangeException(nameof(filterBy), filterBy, null);
            }
        }
    }
}