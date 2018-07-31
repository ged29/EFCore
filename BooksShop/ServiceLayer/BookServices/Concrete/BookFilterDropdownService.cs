using DataLayer;
using ServiceLayer.BookServices.Dtos;
using ServiceLayer.BookServices.QueryObjects;
using System.Linq;
using System.Collections.Generic;
using System;

namespace ServiceLayer.BookServices.Concrete
{
    public class BookFilterDropdownService
    {
        private readonly DataContext dataContext;

        public BookFilterDropdownService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public IEnumerable<DropdownTuple> GetFilterDropDownValues(BooksFilterBy filterBy)
        {
            switch (filterBy)
            {
                case BooksFilterBy.NoFilter:
                    return new DropdownTuple[0];

                case BooksFilterBy.ByVotes:
                    return GetVotesDropDown();

                case BooksFilterBy.ByPublicationYear:
                    return GetPublicationYearDropDown();

                default: throw new ArgumentOutOfRangeException(nameof(filterBy), filterBy, null);
            }
        }

        private IEnumerable<DropdownTuple> GetVotesDropDown()
        {
            return new[]
            {
                new DropdownTuple {Value = "4", Text = "4 stars and up"},
                new DropdownTuple {Value = "3", Text = "3 stars and up"},
                new DropdownTuple {Value = "2", Text = "2 stars and up"},
                new DropdownTuple {Value = "1", Text = "1 star and up"},
            };
        }

        private IEnumerable<DropdownTuple> GetPublicationYearDropDown()
        {
            var nextYear = DateTime.UtcNow.AddYears(1).Year;
            var result = dataContext.Books
                .Select(book => book.PublishedOn.Year)
                .Distinct()
                .Where(year => year < nextYear)
                .OrderByDescending(year => year)
                .Select(year => new DropdownTuple { Value = year.ToString(), Text = year.ToString() })
                .ToList();

            var isComingSoonExist = dataContext.Books.Any(book => book.PublishedOn > DateTime.UtcNow);
            if (isComingSoonExist)
            {
                result.Insert(0, new DropdownTuple
                {
                    Value = BookListDtoFilterBy.AllBooksNotPublishedString,
                    Text = BookListDtoFilterBy.AllBooksNotPublishedString
                });
            }

            return result;
        }
    }
}
