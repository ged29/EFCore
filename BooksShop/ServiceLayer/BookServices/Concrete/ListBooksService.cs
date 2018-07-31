using DataLayer;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.BookServices.Dtos;
using ServiceLayer.BookServices.QueryObjects;
using System.Linq;

namespace ServiceLayer.BookServices.Concrete
{
    public class ListBooksService
    {
        private readonly DataContext dataContext;

        public ListBooksService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public IQueryable<BookListDto> SortFilterPage(SortFilterPageOptionsDto options)
        {
            var bookQuery = dataContext.Books
                .AsNoTracking()
                .MapToDto()
                .OrderBy(options.BookOrderBy)
                .FilterBy(options.BooksFilterBy, options.FilterValue);

            options.SetupRestOfDto(bookQuery);
            return bookQuery.Page(options.CurrentPage - 1, options.PageSize);
        }
    }
}