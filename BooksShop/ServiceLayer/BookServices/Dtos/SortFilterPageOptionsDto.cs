using ServiceLayer.BookServices.QueryObjects;
using System;
using System.Linq;

namespace ServiceLayer.BookServices.Dtos
{
    public class SortFilterPageOptionsDto
    {
        public const int DefaultPageSize = 10;   //default page size is 10
        public BookOrderBy BookOrderBy { get; set; }
        public BooksFilterBy BooksFilterBy { get; set; }
        public string FilterValue { get; set; }

        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int[] PageSizes = new[] { 5, DefaultPageSize, 20, 50, 100, 500, 1000 };
        /// <summary>
        /// This is set to the number of pages available based on the number of entries in the query
        /// </summary>
        public int PagesCount { get; private set; }
        /// <summary>
        /// This holds the state of the key parts of the SortFilterPage parts 
        /// </summary>
        public string PrevStateCheck { get; set; }

        public void SetupRestOfDto<T>(IQueryable<T> query)
        {
            PagesCount = (int)Math.Ceiling((decimal)query.Count() / PageSize);
            CurrentPage = Math.Min(Math.Max(1, CurrentPage), PagesCount);

            var newStateCheck = GetStateCheck();
            if (PrevStateCheck != newStateCheck) CurrentPage = 1;

            PrevStateCheck = newStateCheck;
        }

        private string GetStateCheck()
        {
            return $"{(int)BooksFilterBy},{FilterValue},{PageSize},{PagesCount}";
        }
    }
}