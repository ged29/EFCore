using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using EfCoreInAction.Models;
using DataLayer;
using ServiceLayer.BookServices.Dtos;
using ServiceLayer.BookServices.Concrete;
using ServiceLayer.Logger;

namespace EfCoreInAction.Controllers
{
    public class HomeController : BaseTraceController
    {
        private readonly DataContext dataContext;

        public HomeController(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public IActionResult Index(SortFilterPageOptionsDto options)
        {
            var listBooksService = new ListBooksService(dataContext);
            IEnumerable<BookListDto> booksList = listBooksService.SortFilterPage(options).ToArray();

            return View(new BookListCombinedDto(HttpContext.TraceIdentifier, options, booksList));
        }

        [HttpGet]
        public JsonResult GetFilterSearchContent(SortFilterPageOptionsDto options)
        {
            var bookFilterDropdown = new BookFilterDropdownService(dataContext);
            var dropDownValues = bookFilterDropdown.GetFilterDropDownValues(options.BooksFilterBy).ToArray();

            return Json(new TraceIndentGeneric<IEnumerable<DropdownTuple>>(HttpContext.TraceIdentifier, dropDownValues));
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
