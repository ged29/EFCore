using DataLayer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using EfCoreInAction.Helpers;
using ServiceLayer.AdminServices.Concrete;
using ServiceLayer.AdminServices.Dtos;
using System.Globalization;
using DataLayer.Entities;
using ServiceLayer.DatabaseServices;

namespace EfCoreInAction.Controllers
{
    public class AdminController : BaseTraceController
    {
        private readonly DataContext dataContext;
        private readonly IHostingEnvironment hostingEnvironment;

        public AdminController(DataContext dataContext, IHostingEnvironment hostingEnvironment)
        {
            this.dataContext = dataContext;
            this.hostingEnvironment = hostingEnvironment;
        }

        public IActionResult ChangePubDate(int bookId)
        {
            Request.ThrowErrorIfNotLocal();

            var service = new ChangePubDateService(dataContext);
            var original = service.GetOriginal(bookId);

            return View(original);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePubDate(ChangePubDateDto changePubDate)
        {
            Request.ThrowErrorIfNotLocal();

            var service = new ChangePubDateService(dataContext);
            var updatedBook = service.UpdateBook(changePubDate);

            return View("BookUpdated", "Successfully changed publication date");
        }


        public IActionResult ChangePromotion(int bookId)
        {
            Request.ThrowErrorIfNotLocal();

            var service = new ChangePriceOfferService(dataContext);
            var priceOffer = service.GetOriginal(bookId);

            ViewData["BookTilte"] = service.OrgBook.Title;
            ViewData["OrgPrice"] = service.OrgBook.Price < 0
                ? "Not currently for sale"
                : service.OrgBook.Price.ToString("c", new CultureInfo("en-US"));

            return View(priceOffer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePromotion(PriceOffer priceOffer)
        {
            Request.ThrowErrorIfNotLocal();

            var service = new ChangePriceOfferService(dataContext);
            var updatedBook = service.UpdateBook(priceOffer);

            return View("BookUpdated", "Successfully added/changed a promotion");
        }


        public IActionResult AddBookReview(int bookId)
        {
            Request.ThrowErrorIfNotLocal();

            var service = new AddReviewService(dataContext);
            var blankReview = service.GetBlankReview(bookId);
            ViewData["BookTitle"] = service.BookTitle;

            return View(blankReview);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddBookReview(Review review)
        {
            Request.ThrowErrorIfNotLocal();

            var service = new AddReviewService(dataContext);
            var updatedReview = service.AddReviewToBook(review);

            return View("BookUpdated", "Successfully added a review");
        }

        //------------------------------------------------
        //Amdin commands that are called from the top menu

        public IActionResult ResetDatabase()
        {
            Request.ThrowErrorIfNotLocal();

            dataContext.DevelopmentEnsureCreated();
            var numBooks = dataContext.SeedDatabase(hostingEnvironment.WebRootPath);            
            return View("BookUpdated", $"Successfully reset the database and added {numBooks} books.");
        }
    }
}
