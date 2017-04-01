using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpenseManager.ViewModel
{
    public class RecordViewModel
    {
        public RecordViewModel() {
            expenses = new List<Expenses>();
            report = new List<Report>();
        }
        public int RecordId { get; set; }
        public int TypeId { get; set; }
        public string Description { get; set; }
        public decimal? Payment { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<Expenses> expenses { get; set; }
        public List<Report> report { get; set; }
    }

    public class Report {
        public Report() {
            this.Severity = "green";
        }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public int TypeId { get; set; }
        public decimal Threshold { get; set; }
        public string Severity { get; set; }
    }

    public class Expenses
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public decimal Payment { get; set; }
    }
}