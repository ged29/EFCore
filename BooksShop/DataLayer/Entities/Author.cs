using System.Collections.Generic;

namespace DataLayer.Entities
{
    public class Author
    {
        public int AuthorId { get; set; }
        public string Name { get; set; }

        public ICollection<BookAuthor> BooksLink { get; set; }
    }
}