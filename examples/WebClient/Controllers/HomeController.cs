using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ArcadePockets.Managers.Jwt;
using Microsoft.AspNetCore.Mvc;
using WebClient.Models;

namespace WebClient.Controllers
{
    public class HomeController : Controller
    {
        IJwtManager _jwtTokenManager;
        public HomeController(IJwtManager jwtTokenManager)
        {
            _jwtTokenManager = jwtTokenManager;
        }

        public IActionResult Index()
        {
            string token = _jwtTokenManager.AccessToken;

            ViewData["access_token"] = token;

            return View();
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
