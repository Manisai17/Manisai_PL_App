using Microsoft.AspNetCore.Mvc;
using Manisai_PL_App.Models;

namespace Manisai_PL_App.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult LoginCheck(string username, string password)
        {
            // TEMPORARY hardcoded check — replaced with real JWT + DB-backed login in Part 8
            if (username == "admin" && password == "admin")
            {
                return RedirectToAction("Index", "Manager");
            }
            else
            {
                ViewBag.message = "Invalid Credentials";
                return View("Login");
            }
        }
    }
}