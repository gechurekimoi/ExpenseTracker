using LightBootstrapTemplate.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace LightBootstrapTemplate
{
    public class Auth : ActionFilterAttribute
    {
        ExpensesDbContext db ;

        public Auth()
        {
        }

        public Auth(ExpensesDbContext _db)
        {
            db = _db;
        }
       

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            //HttpSessionStateBase session = filterContext.HttpContext.Session;
            //if (session["User"] == null)
            //{
            //    filterContext.Result = new RedirectToRouteResult(
            //        new RouteValueDictionary {
            //        { "Controller", "Home" },
            //        { "Action", "Index" }
            //        });
            //}

            //var LoggedInUser = filterContext.HttpContext.Request.Cookies["LoggedInUser"];

            //if (LoggedInUser == null)
            //{
            //    filterContext.Result = new RedirectToRouteResult(
            //        new RouteValueDictionary {
            //        { "Controller", "Home" },
            //        { "Action", "Index" }
            //        });
            //}

        }
    }
}