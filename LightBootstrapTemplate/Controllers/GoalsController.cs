using System;
using System.Collections.Generic;
using System.Data;

using System.Linq;
using System.Net;
using LightBootstrapTemplate.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LightBootstrapTemplate.Controllers
{
    [Authorize]
    public class GoalsController : Controller
    {
        private ExpensesDbContext db;

        public GoalsController(ExpensesDbContext _db)
        {
            db = _db;
        }

        // GET: Goals
        public ActionResult Index()
        {
            var date = DateTime.Now.Date;
            var data = db.Goals.Where(p => p.DateToBeAchieved >= date).ToList();
            return View(data);
        }

        public ActionResult IndexGoals(int? page)
        {
            int pageSize = 10;
            int pageIndex = 1;

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            var data = db.Goals.OrderByDescending(p => p.Id).ToList();
            var lstgoals = data;

            return View(lstgoals);
        }

        // GET: Goals/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
            Goal goal = db.Goals.Find(id);
            if (goal == null)
            {
                //return HttpNotFound();
                return StatusCode((int)HttpStatusCode.NotFound);
            }
            return View(goal);
        }

        // GET: Goals/Create
        public ActionResult Create()
        {
            return View();
        }

        public struct goal{

        public string Description { get; set; }
        public string typeGoal { get; set; }
        public string DateToBeAchieved { get; set; }
    }

        // POST: Goals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(goal model)
        {

            Goal goal = new Goal()
            {
                DateSet = DateTime.Now,
                DateToBeAchieved = DateTime.ParseExact(model.DateToBeAchieved,"dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture),
                Description = model.Description,
                typeGoal = model.typeGoal
            };
            
            db.Goals.Add(goal);
            db.SaveChanges();
                
            return Json(goal);
        }

        // GET: Goals/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
            Goal goal = db.Goals.Find(id);
            if (goal == null)
            {
                //return HttpNotFound();
                return StatusCode((int)HttpStatusCode.NotFound);
            }
            return View(goal);
        }

        // POST: Goals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Goal goal)
        {
            if (ModelState.IsValid)
            {
                db.Entry(goal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(goal);
        }

        // GET: Goals/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
            Goal goal = db.Goals.Find(id);
            if (goal == null)
            {
                //return HttpNotFound();
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
            return View(goal);
        }

        // POST: Goals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Goal goal = db.Goals.Find(id);
            db.Goals.Remove(goal);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
