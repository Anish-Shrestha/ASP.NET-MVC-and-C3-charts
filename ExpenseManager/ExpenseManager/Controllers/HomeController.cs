using ExpenseManager.Models;
using ExpenseManager.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExpenseManager.Controllers
{
    public class HomeController : Controller
    {
        ExpenseManagerEntities db = new ExpenseManagerEntities();

        public void PopulateData()
        {
            ViewData["ExpenseTypes"] = db.Type.Where(x => x.IsAutomatic == false).Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.TypeId.ToString()
            });
        }

        public ActionResult Index(int month = 0, int year = 0)
        {
            if (month == 0)
                month = DateTime.Now.Month;
            if (year == 0)
                year = DateTime.Now.Year;

            var model = new RecordViewModel();
            var previousMonthLastDate = new DateTime(year, month, 1).AddDays(-1);
            var nextMonthFirstDate = new DateTime(new DateTime(year, month, 1).AddMonths(1).Year, new DateTime(year, month, 1).AddMonths(1).Month, 1);
            PopulateData();

            model.expenses = db.ExpenseRecord.Where(x => x.CreatedDate > previousMonthLastDate && x.CreatedDate < nextMonthFirstDate).OrderByDescending(x => x.RecordId).Select(x => new Expenses()
            {
                Id = x.RecordId,
                TypeId = x.TypeId,
                Name = db.Type.FirstOrDefault(y => y.TypeId == x.TypeId).Name,
                Date = x.CreatedDate,
                Payment = x.Expense
            }).ToList();

            ViewBag.Total = model.expenses.Sum(x => x.Payment);

            //daily expense graph
            var dailyExpenseGraph =new List<object>();
          dailyExpenseGraph.Add("DailyExpense");
            dailyExpenseGraph.Add(1);
            for (var count = 1; count < nextMonthFirstDate.AddDays(-1).Day; count++)
            {
               var dailyExpense = model.expenses.Where(x => x.Date.Day == count).Sum(x => x.Payment);
                dailyExpenseGraph.Add(Convert.ToInt32(dailyExpense));
            }
            ViewBag.DailyExpenseGraph = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(dailyExpenseGraph);



            // report section
            model.report = model.expenses.GroupBy(x => x.Name).Select(g => new Report
            {
                Value = g.Sum(s => s.Payment),
                Name = g.FirstOrDefault().Name,
                TypeId = g.FirstOrDefault().TypeId


            }).ToList();

            var totalExpenseGraph = new Dictionary<string, decimal>();
            foreach (var data in model.report)
            {
                totalExpenseGraph.Add(data.Name, data.Value);
                var threshold = db.Threshold.FirstOrDefault(x => x.TypeId == data.TypeId);
                if (threshold != null)
                {
                    data.Threshold = threshold.Value;
                    if (data.Value > data.Threshold)
                        data.Severity = "red";
                }
                else
                {
                    data.Severity = "blue";
                }
            }
            ViewBag.totalExpenseGraph = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(totalExpenseGraph);




            return View(model);
        }

        public ActionResult DeleteRecord(int id) {

            var record = db.ExpenseRecord.FirstOrDefault(x => x.RecordId == id);
            if (record != null)
            {
                db.ExpenseRecord.Remove(record);
                db.SaveChanges();
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PaymentsDue()
        {

            return PartialView("partialView");

        }

        [HttpPost]
        public ActionResult Index(RecordViewModel obj)
        {

            db.ExpenseRecord.Add(new ExpenseRecord()
            {
                TypeId = obj.TypeId,
                Expense = obj.Payment ?? 0,
                CreatedDate = DateTime.Now,
                Description = obj.Description

            });
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}