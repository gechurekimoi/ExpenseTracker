using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using LightBootstrapTemplate.Models;
using LightBootstrapTemplate.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
//using Microsoft.Reporting.WebForms;
//using X.PagedList;

namespace LightBootstrapTemplate.Controllers
{
    [Authorize]
    public class ExpensesController : Controller
    {
        private ExpensesDbContext db;

        public ExpensesController(ExpensesDbContext _db)
        {
            db = _db;
        }

        // GET: Expenses
        public ActionResult Index(int? page)
        {
            int pageSize = 10;
            int pageIndex = 1;

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            var expenses = db.Expenses.Include(e => e.Category).OrderByDescending(p => p.Id).ToList();

            var lstExpenses = expenses;
            return View(lstExpenses);
        }

        //public ActionResult Report()
        //{
        //    LocalReport lr = new LocalReport();
        //    string pt = @"C: \Users\John Gechure Kimoi\source\repos\LightBootstrapTemplate\LightBootstrapTemplate\Reports\JustReport.rdlc";


        //    //string path = Path.Combine(Server.MapPath("~/Reports/JustReport.rdlc"));
        //    string path = pt;

        //    if (System.IO.File.Exists(path))
        //    {
        //        lr.ReportPath = path;
        //    }
        //    else
        //    {
        //        return View("Index");
        //    }
        //    //List<StateArea> cm = new List<StateArea>();
        //    //using (PopulationEntities dc = new PopulationEntities())
        //    //{
        //    //    cm = dc.StateAreas.ToList();
        //    //}

        //    var data = db.Expenses.ToList();
        //    ReportDataSource rd = new ReportDataSource("MyDataset", data);
        //    lr.DataSources.Add(rd);

        //    String id = "PDF";
        //    string reportType = id;
        //    string mimeType;
        //    string encoding;
        //    string fileNameExtension;



        //    string deviceInfo =

        //    "<DeviceInfo>" +
        //    "  <OutputFormat>" + id + "</OutputFormat>" +
        //    "  <PageWidth>8.5in</PageWidth>" +
        //    "  <PageHeight>11in</PageHeight>" +
        //    "  <MarginTop>0.5in</MarginTop>" +
        //    "  <MarginLeft>1in</MarginLeft>" +
        //    "  <MarginRight>1in</MarginRight>" +
        //    "  <MarginBottom>0.5in</MarginBottom>" +
        //    "</DeviceInfo>";

        //    Warning[] warnings;
        //    string[] streams;
        //    byte[] renderedBytes;

        //    renderedBytes = lr.Render(
        //        reportType,
        //        deviceInfo,
        //        out mimeType,
        //        out encoding,
        //        out fileNameExtension,
        //        out streams,
        //        out warnings);
        //    return File(renderedBytes, mimeType);
        //}



        // GET: Expenses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
            Expenses expenses = db.Expenses.Find(id);
            if (expenses == null)
            {
                //return HttpNotFound();
                return StatusCode((int)HttpStatusCode.NotFound);
            }
            return View(expenses);
        }

        // GET: Expenses/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories
                                .Select(c=> new { Id= c.Id, Name = c.Name}), "Id", "Name");
            return View();
        }

        // POST: Expenses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(Expenses expenses)
        {
            //[Bind(Include = "Id,Description,CategoryId,Amount")]
             expenses.DateSpent = DateTime.Now.Date;
            if (ModelState.IsValid)
            {
                
                db.Expenses.Add(expenses);
                db.SaveChanges();
               
                return Json("Successful");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", expenses.CategoryId);
            return View(expenses);
        }

        // GET: Expenses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
            Expenses expenses = db.Expenses.Find(id);
            if (expenses == null)
            {
                //return HttpNotFound();
                return StatusCode((int)HttpStatusCode.NotFound);
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", expenses.CategoryId);
            return View(expenses);
        }

        // POST: Expenses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Expenses expenses)
        {
            if (ModelState.IsValid)
            {
                db.Entry(expenses).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", expenses.CategoryId);
            return View(expenses);
        }

        // GET: Expenses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
            Expenses expenses = db.Expenses.Find(id);
            if (expenses == null)
            {
                //return HttpNotFound();
                return StatusCode((int)HttpStatusCode.NotFound);
            }
            return View(expenses);
        }

        // POST: Expenses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Expenses expenses = db.Expenses.Find(id);
            db.Expenses.Remove(expenses);
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

        public ActionResult GetBarChartForMonthlyExpenseCategories(int? Year,int? Month)
        {
            try
            {
                GraphDataViewModel vm = new GraphDataViewModel();

                if (!Year.HasValue)
                {
                    Year = DateTime.Now.Year;
                }

                if (!Month.HasValue)
                {
                    Month = DateTime.Now.Month;
                }


                List<Subtotals> categories = new List<Subtotals>();

                var dat = db.Categories.Include(p => p.Expenses).ToList();

                //Session["Year"] = totalExpenses.year;
                //Session["Month"] = totalExpenses.month;
                foreach (var item in dat)
                {
                    Subtotals subtotals = new Subtotals()
                    {
                        name = item.Name,
                        Amount = item.Expenses.Where(p => p.DateSpent.Month == Month && p.DateSpent.Year == Year).Sum(p => p.Amount)
                    };

                    categories.Add(subtotals);
                }

                vm.categories = categories.Select(p => p.name).ToArray();
                vm.data = categories.Select(p => (int)p.Amount).ToArray();


                return Json(vm);

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult GetChartDataForMonthsOfTheYear(int? Year)
        {

            if (!Year.HasValue)
            {
                Year = DateTime.Now.Year;
            }

            var data = db.Expenses.Where(p => p.DateSpent.Year == Year.Value).GroupBy(p => p.DateSpent.Month).Select(x=>new { x.Key, MonthlyTotal = x.Sum(s=>s.Amount) }).ToList();

            List<Subtotals> MonthlySubTotals = new List<Subtotals>();

            data = data.OrderBy(p => p.Key).ToList();

            foreach (var item in data)
            {
                Subtotals subtotals = new Subtotals()
                {
                    Amount = item.MonthlyTotal,
                    name = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(item.Key)
                };

                MonthlySubTotals.Add(subtotals);
            }

            GraphDataViewModel vm = new GraphDataViewModel()
            {
                categories = MonthlySubTotals.Select(p => p.name).ToArray(),
                data = MonthlySubTotals.Select(p => (int)p.Amount).ToArray()
            };

            return Json(vm);

        }

        public ActionResult GetTotalExpenses(TotalExpenses totalExpenses)
        {

            ViewBag.TotalIncome = db.Income.Where(p => p.DateReceived.Month == totalExpenses.month && p.DateReceived.Year == totalExpenses.year).ToList();

            List<Subtotals> categories = new List<Subtotals>();

            var dat = db.Categories.Include(p => p.Expenses).ToList();

            //Session["Year"] = totalExpenses.year;
            //Session["Month"] = totalExpenses.month;
            foreach (var item in dat)
            {
                Subtotals subtotals = new Subtotals()
                {
                    name = item.Name,
                    Amount = item.Expenses.Where(p => p.DateSpent.Month == totalExpenses.month && p.DateSpent.Year == totalExpenses.year).Sum(p => p.Amount)
                };

                categories.Add(subtotals);
            }

            return PartialView("_Subtotals",categories);
        }

        public ActionResult PieChartAnalysis()
        {
            //int Year = (int)Session["Year"];
            //int month = (int)Session["Month"];
            int Year = 2019;
            int month = 12;

            var data = db.Categories.Include(p => p.Expenses).ToList();

            var xvalues = new List<string>();
            var yvalues = new List<decimal>();

            foreach (var item in data)
            {
                xvalues.Add(item.Name);
                decimal amountPerCategory = Convert.ToDecimal(item.Expenses.Where(x=>x.DateSpent.Month == month && x.DateSpent.Year == Year).Sum(p => p.Amount));
                yvalues.Add(amountPerCategory);
            }

            //Session["Year"] = null;
            //Session["Month"] = null;

            //new Chart(width:1000, height: 700)
            //    .AddSeries(
            //    chartType: "Column",
            //    xValue: xvalues,
            //    yValues: yvalues)
            //    .Write("png");

            return null;
        }

        public ActionResult IndexIncome(int? page)
        {
            int pageSize = 10;
            int pageIndex = 1;

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            var incomes = db.Income.OrderByDescending(p => p.Id).ToList();

            var lstIncomes = incomes;
            return View(lstIncomes);
        }

        // GET: Incomes/Details/5
        public ActionResult DetailsIncome(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
            Income income = db.Income.Find(id);
            if (income == null)
            {
                //return HttpNotFound();
                return StatusCode((int)HttpStatusCode.NotFound);
            }
            return View(income);
        }

        // GET: Incomes/CreateIncome
        public ActionResult CreateIncome()
        {
            return View();
        }

        // POST: Incomes/CreateIncome
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateIncome(Income income)
        {
            income.DateReceived = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Income.Add(income);
                db.SaveChanges();
                return RedirectToAction("IndexIncome");
            }

            return View(income);
        }

        // GET: Incomes/Edit/5
        public ActionResult EditIncome(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
            Income income = db.Income.Find(id);
            if (income == null)
            {
                //return HttpNotFound();
                return StatusCode((int)HttpStatusCode.NotFound);
            }
            return View(income);
        }

        // POST: Incomes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditIncome( Income income)
        {
            if (ModelState.IsValid)
            {
                db.Entry(income).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexIncome");
            }
            return View(income);
        }

        // GET: Incomes/Delete/5
        public ActionResult DeleteIncome(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
            Income income = db.Income.Find(id);
            if (income == null)
            {
                //return HttpNotFound();
                return StatusCode((int)HttpStatusCode.NotFound);
            }
            return View(income);
        }

        // POST: Incomes/Delete/5
        [HttpPost, ActionName("DeleteIncome")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedIncome(int id)
        {
            Income income = db.Income.Find(id);
            db.Income.Remove(income);
            db.SaveChanges();
            return RedirectToAction("IndexIncome");
        }

        public ActionResult IndexDiary(int? page)
        {
            int pageSize = 10;
            int pageIndex = 1;

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            var entries = db.Diaries.OrderByDescending(p => p.Id).ToList();
            var diaryentires = entries;

            return View(diaryentires);
        }

        public ActionResult DetailsDairy(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
            Diary diary = db.Diaries.Find(id);
            if (diary == null)
            {
                // return HttpNotFound();
                return StatusCode((int)HttpStatusCode.NotFound);
            }
            return View(diary);
        }

        // GET: Diaries/Create
        public ActionResult CreateDiary()
        {
            return View();
        }

        // POST: Diaries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult CreateDiary( Diary diary)
        {
            diary.EntryDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Diaries.Add(diary);
                db.SaveChanges();
                return RedirectToAction("IndexDiary");
            }

            return View(diary);
        }

        public ActionResult EditDiary(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
            Diary diary = db.Diaries.Find(id);
            if (diary == null)
            {
                //return HttpNotFound();
                return StatusCode((int)HttpStatusCode.NotFound);
            }

            return View(diary);
        }

        // POST: Diaries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDiary(Diary diary)
        {
            if (ModelState.IsValid)
            {
                db.Entry(diary).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexDiary");
            }
            return View(diary);
        }

        // GET: Diaries/Delete/5
        public ActionResult DeleteDiary(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
            Diary diary = db.Diaries.Find(id);
            if (diary == null)
            {
                //return HttpNotFound();
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
            return View(diary);
        }

        // POST: Diaries/Delete/5
        [HttpPost, ActionName("DeleteDiary")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedDiary(int id)
        {
            Diary diary = db.Diaries.Find(id);
            db.Diaries.Remove(diary);
            db.SaveChanges();
            return RedirectToAction("IndexDiary");
        }


    }
}
