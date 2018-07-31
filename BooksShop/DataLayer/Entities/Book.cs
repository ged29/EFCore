using System;
using System.Collections.Generic;

namespace DataLayer.Entities
{
    public class Book
    {
        public int BookId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PublishedOn { get; set; }
        public string Publisher { get; set; }
        public decimal Price { get; set; }
        /// <summary>
        /// Holds the url to get the image of the book
        /// </summary>
        public string ImageUrl { get; set; }
        public bool SoftDeleted { get; set; }

        public PriceOffer Promotion { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<BookAuthor> AuthorsLink { get; set; }
    }
}