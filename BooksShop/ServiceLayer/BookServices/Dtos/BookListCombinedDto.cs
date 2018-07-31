using ServiceLayer.Logger;
using System.Collections.Generic;

namespace ServiceLayer.BookServices.Dtos
{
    public class BookListCombinedDto : TraceIdentBaseDto
    {
        public BookListCombinedDto(
            string traceIdentifier, SortFilterPageOptionsDto sortFilterPageOptions, IEnumerable<BookListDto> booksList)
            : base(traceIdentifier)
        {
            SortFilterPageOptions = sortFilterPageOptions;
            BooksList = booksList;
        }

        public SortFilterPageOptionsDto SortFilterPageOptions { get; set; }

        public IEnumerable<BookListDto> BooksList { get; set; }
    }
}
