using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatDemoBackend.Data;
using ChatDemoBackend.Models;

namespace ChatDemoBackend.Controllers
{
    public class AuthController : Controller
    {

        private readonly ApplicationDbContext _db; // database instanxe

        public AuthController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET - Login
        public IActionResult Login()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("userId")))
            {
                return View();
            }
            return RedirectToAction("Products", "Home");
        }

        // POST - Login
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var user = _db.User_Info.Select(info => new { info.UserId, info.UserName, info.User_Password })
                                        .Where(user => user.UserName == loginModel.UserName)
                                        .SingleOrDefault();
                if(user != null) 
                { 
                    if (user.User_Password == loginModel.User_Password)
                    {
                        HttpContext.Session.SetString("userId", user.UserId+"");
                        HttpContext.Session.SetString("username", loginModel.UserName);
                        return RedirectToAction("Index", "Chat");
                    }
                }
            }
            return View(loginModel);
        }

        //public IActionResult Register()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[AutoValidateAntiforgeryToken]
        //public IActionResult Register(RegistrationModel registerModel)
        //{
        //    if (ModelState.IsValid)
        //    {

        //        //return RedirectToAction("Products", "Home");
        //    }

        //    return View(registerModel);

        //}


    }
}
