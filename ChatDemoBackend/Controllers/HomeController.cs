using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ChatDemoBackend.Models;
using ChatDemoBackend.Data;
using Microsoft.AspNetCore.Http;

namespace ChatDemoBackend.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationDbContext _db; // database instanxe

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;

            _db = db;

        }

        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("userId")))
                return RedirectToAction("Login", "Auth");

            //Console.WriteLine("Been here ;)");
            return View(); 
        }

        public IActionResult Privacy()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("userId")))
                return RedirectToAction("Login", "Auth");

            return View();
        }

        public IActionResult Products()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("userId")))
                return RedirectToAction("Login", "Auth");

            IEnumerable<Product> productList = _db.Product;
            return View(productList);
        }

        public IActionResult Add()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("userId")))
                return RedirectToAction("Login", "Auth");

            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Add(Product p)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("userId")))
                return RedirectToAction("Login", "Auth");

            if (ModelState.IsValid)
            {
                p.Product_Name = p.Product_Name.Trim();

                if (p.Product_Id == 0)
                    _db.Product.Add(p);
                else
                    _db.Product.Update(p);

                _db.SaveChanges();

                return RedirectToAction("Products");
            }

            return View(p);
        }

        public IActionResult Edit(int Id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("userId")))
                return RedirectToAction("Login", "Auth");

            Product p = _db.Product.Find(Id);
            return View("Add", p);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Delete(int Id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("userId")))
                return RedirectToAction("Login", "Auth");

            Product p = _db.Product.Find(Id);
            if(p != null)
            {
                _db.Product.Remove(p);
                _db.SaveChanges();
            }
            return RedirectToAction("Products");
        }

        [HttpPost]
        public IActionResult GetCredentials([FromBody]LoginModel loginModel)
        {
            if(loginModel == null)
                return BadRequest();
            else if (string.IsNullOrEmpty(loginModel.UserName) || string.IsNullOrEmpty(loginModel.User_Password))
                return BadRequest();

            var user = _db.User_Info.FirstOrDefault(e => e.UserName == loginModel.UserName);

            if(user == null) // no such username
                return Unauthorized();
            else if (user.User_Password != loginModel.User_Password) // the password is invalid
                return Unauthorized();

            return Ok(
                new { 
                    userId = user.UserId,
                    username = user.UserName,
                    register_date = user.Register_Date
                }
            );
        }

        public IActionResult GetChatHubInfo()
        {
            return Ok(new
            {
                list = ChatDemoBackend.Hubs.ChatHub.list,
                numberOfConnected = ChatDemoBackend.Hubs.ChatHub.numberOfConnected
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
