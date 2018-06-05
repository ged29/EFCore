using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PartyInvites.Models;

namespace PartyInvites.Controllers
{
    public class HomeController : Controller
    {
        private DataContext dataCxt;

        public HomeController(DataContext context) => this.dataCxt = context;

        public IActionResult Index() => View();

        public IActionResult Respond() => View();

        [HttpPost]
        public IActionResult Respond(GuestResponse response)
        {
            dataCxt.Responses.Add(response);
            dataCxt.SaveChanges();
            return RedirectToAction(nameof(Thanks), routeValues: new { response.Name, response.WillAttend });
        }

        public IActionResult Thanks(GuestResponse response) => View(response);

        public IActionResult ListResponses(string searchTerm = "555-123-5678")
        {
            var result = dataCxt.Responses
                .Where(r => r.Phone == searchTerm)
                .OrderBy(r => r.Email);

            return View(result);
        }
    }
}