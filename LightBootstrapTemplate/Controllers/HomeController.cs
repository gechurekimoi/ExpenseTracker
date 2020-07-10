using LightBootstrapTemplate.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;


namespace LightBootstrapTemplate.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        ExpensesDbContext db ;

        public HomeController(ExpensesDbContext _db)
        {
            db = _db;
        }

        [AllowAnonymous]
        public ActionResult Index()
        {

            //if (Request.Cookies["LoggedInUser"] != null)
            //{
            //    return RedirectToAction("DashBoard", "Home");
            //}

            if (db.Users.Count()==0)
            {
                User user = new User()
                {
                    Name = "John Kimoi",
                    Phone = "0702740041",
                    password = "hil1song"
                };
                db.Users.Add(user);
                db.SaveChanges();
            }
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(User user)
        {
            if(user!=null)
            {

                var me = db.Users.Where(p => p.Phone == user.Phone && p.password == user.password).FirstOrDefault();

                if(me!=null)
                {

                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, me.Name)
                    };

                    var identity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var principal = new ClaimsPrincipal(identity);

                    var props = new AuthenticationProperties();

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props).Wait();


                    return RedirectToAction("Create","Expenses");
                }
                else
                {
                    return View();
                }
            }
            return View();
        }

        [Auth]
        public ActionResult Dashboard()
        {
            return View();
        }

        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync().ConfigureAwait(false);
            
            return RedirectToAction("Index", "Home");
        }


    }
}