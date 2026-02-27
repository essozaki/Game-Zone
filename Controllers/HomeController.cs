using System.Diagnostics;
using GameZone.Models;
using GameZone.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameZone.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGameService _gameservices;

        public HomeController(ILogger<HomeController> logger, IGameService gameservices)
        {
            _logger = logger;
            _gameservices = gameservices;
        }

        public IActionResult Index()
        {
            var games = _gameservices.GetAll();
            return View(games);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
