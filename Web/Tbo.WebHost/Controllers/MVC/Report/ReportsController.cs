using System.Web.Mvc;
using Tbo.WebHost.Controllers.MVC.Common;
using FastReport.Web;
using FastReport;
using System.Web.UI.WebControls;
using System.Data;
using Core.DataAccess.Interfaces;
using Domain.Registries.Requests.Entities;
using System.Linq;
using Core.Extensions;
using System.Collections.Generic;
using System;
using System.Globalization;
using Domain.Registries.Waybills.Entities;
using Domain.Registries.Schedules.Entities;
using Domain.Registries.Requests.Enums;

namespace Tbo.WebHost.Controllers.MVC.Domain
{
    /// <summary>
    /// Контоллер для отчетов
    /// </summary>
    public class ReportsController : BaseController
    {
        private readonly WebReport webReport = new WebReport();

        private readonly IDataStore dataStore;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="dataStore"></param>
        public ReportsController(IDataStore dataStore)
        {
            this.dataStore = dataStore;
        }

        /// <summary>
        /// Конфигурирование отчета
        /// </summary>
        /// <returns></returns>
        private void SetReport()
        {
            webReport.Width = Unit.Percentage(100);
            webReport.Height = Unit.Percentage(100);
            webReport.ToolbarIconsStyle = ToolbarIconsStyle.Black;
            webReport.ToolbarStyle = ToolbarStyle.Large;
            webReport.LocalizationFile = "~/Reports/Localization/Russian.frl";
            webReport.PrintInPdf = true;
            webReport.ShowExports = true;
            webReport.ShowPrint = true;
            webReport.Layers = true;
            webReport.XlsxPageBreaks = false;
        }

        /// <summary>
        /// Путь к шаблонам
        /// </summary>
        /// <returns></returns>
        private string GetReportPath()
        {
            return Server.MapPath("~/Reports/Templates/");
        }

        /// <summary>
        /// Получение доходности
        /// </summary>
        /// <returns></returns>
        public ActionResult Profitability(FormCollection formValue)
        {
            int selectedYear;
            int.TryParse(formValue["Year"], out selectedYear);
            selectedYear = selectedYear == 0 ? DateTime.Now.AddMonths(-1).Year : selectedYear;

            ViewData["Year"] =
                Enumerable.Range(DateTime.Now.Year, 10)
                    .Select(x => new SelectListItem
                    {
                        Text = x.ToString(),
                        Value = x.ToString(),
                        Selected = x == selectedYear
                    });

            int selectedMonth;
            int.TryParse(formValue["Month"], out selectedMonth);
            selectedMonth = selectedMonth == 0 ? DateTime.Now.AddMonths(-1).Month : selectedMonth;

            ViewData["Month"] =
                Enumerable.Range(1, 12)
                    .Select(x => new SelectListItem
                    {
                        Text = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[x - 1],
                        Value = x.ToString(),
                        Selected = x == selectedMonth
                    });

            var reportPath = GetReportPath();

            SetReport();

            var result =
                (from request in dataStore
                        .GetAll<Request>()
                        .Where(x => x.PlannedDateTime.Year == selectedYear && x.PlannedDateTime.Month == selectedMonth)
                        .Where(x => x.Type == RequestType.Install)
                        .ToList()
                    group request by new {request.Customer, request.Address}
                    into r
                    select new
                    {
                        CustomerName = r.Key.Customer?.Name ?? "-",
                        r.Key.Address,
                        RequestsCount = r.Count(),
                        RequestSum = r.Sum(x => x.Sum)
                    }
                )
                .OrderBy(x => x.CustomerName)
                .ThenBy(x => x.Address)
                .ToList();


            var dataSet = new DataSet();
            var dataTable = result.ToDataTable();
            dataTable.TableName = "Profitability";
            dataSet.Tables.Add(dataTable);
            webReport.Report.RegisterData(dataSet, "NorthWind");
            webReport.Report.Load(reportPath + "Profitability.frx");
            webReport.CurrentTab.Name = "Доходность";

            webReport.Report.SetParameterValue("RequestsPeriod",
                $"{new CultureInfo("ru-RU").DateTimeFormat.GetMonthName(selectedMonth)} {selectedYear}");
            webReport.Report.SetParameterValue("RequestsTotalSum", result.Sum(x => x.RequestSum));
            webReport.Report.SetParameterValue("RequestsTotalCount", result.Sum(x => x.RequestsCount));

            ViewBag.WebReport = webReport;
            ViewBag.Title = webReport.CurrentTab.Name;
            return View();
        }

        /// <summary>
        /// Сумма наличности у водителя
        /// </summary>
        /// <returns></returns>
        public ActionResult CashFromDriver(FormCollection formValue)
        {
            int selectedYear;
            int.TryParse(formValue["Year"], out selectedYear);
            selectedYear = selectedYear == 0 ? DateTime.Now.Year : selectedYear;

            ViewData["Year"] =
                Enumerable.Range(DateTime.Now.Year, 10)
                    .Select(x => new SelectListItem
                    {
                        Text = x.ToString(),
                        Value = x.ToString(),
                        Selected = x == selectedYear
                    });

            int selectedMonth;
            int.TryParse(formValue["Month"], out selectedMonth);
            selectedMonth = selectedMonth == 0 ? DateTime.Now.Month : selectedMonth;

            ViewData["Month"] =
                Enumerable.Range(1, 12)
                    .Select(x => new SelectListItem
                    {
                        Text = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[x - 1],
                        Value = x.ToString(),
                        Selected = x == selectedMonth
                    });

            int selectedDay;
            int.TryParse(formValue["Day"], out selectedDay);
            selectedDay = selectedDay == 0 ? DateTime.Now.Day : selectedDay;

            ViewData["Day"] =
                Enumerable.Range(1, DateTime.DaysInMonth(selectedYear, selectedMonth))
                    .Select(x => new SelectListItem
                    {
                        Text = x.ToString(),
                        Value = x.ToString(),
                        Selected = x == selectedDay
                    });

            var reportPath = GetReportPath();

            SetReport();

            var dataTable =
                dataStore.GetAll<Request>()
                    .Where(x => x.PlannedDateTime.Year == selectedYear
                                && x.PlannedDateTime.Month == selectedMonth
                                && x.PlannedDateTime.Day == selectedDay)
                    .Where(x => x.PaymentType == PaymentType.Cash)
                    .Where(x => x.Status == RequestStatus.Done)
                    .GroupBy(x => x.Driver)
                    .Select(x => new
                    {
                        DriverName = x.Key.Name,
                        CashSum = x.Sum(s => s.Sum)
                    })
                    .ToList()
                    .ToDataTable();

            var dataSet = new DataSet();
            dataTable.TableName = "CashFromDriver";
            dataSet.Tables.Add(dataTable);
            webReport.Report.RegisterData(dataSet, "NorthWind");
            webReport.Report.Load(reportPath + "CashFromDriver.frx");
            webReport.CurrentTab.Name = "Наличные у водителя";
            webReport.Report.SetParameterValue("RequestsPeriod",
                $"{selectedDay} {new CultureInfo("ru-RU").DateTimeFormat.MonthGenitiveNames[selectedMonth - 1]} {selectedYear}");

            ViewBag.WebReport = webReport;
            ViewBag.Title = webReport.CurrentTab.Name;
            return View();
        }

        /// <summary>
        /// Заявки по поставщикам
        /// </summary>
        /// <returns></returns>
        public ActionResult RequestsByCustomer(FormCollection formValue)
        {
            int selectedYear;
            int.TryParse(formValue["Year"], out selectedYear);
            selectedYear = selectedYear == 0 ? DateTime.Now.AddMonths(-1).Year : selectedYear;

            ViewData["Year"] =
                Enumerable.Range(DateTime.Now.Year, 10)
                    .Select(x => new SelectListItem
                    {
                        Text = x.ToString(),
                        Value = x.ToString(),
                        Selected = x == selectedYear
                    });

            int selectedMonth;
            int.TryParse(formValue["Month"], out selectedMonth);
            selectedMonth = selectedMonth == 0 ? DateTime.Now.AddMonths(-1).Month : selectedMonth;

            ViewData["Month"] =
                Enumerable.Range(1, 12)
                    .Select(x => new SelectListItem
                    {
                        Text = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[x - 1],
                        Value = x.ToString(),
                        Selected = x == selectedMonth
                    });

            var reportPath = GetReportPath();

            SetReport();

            var requests =
                (from request in dataStore
                        .GetAll<Request>()
                        .Where(x => x.PlannedDateTime.Year == selectedYear && x.PlannedDateTime.Month == selectedMonth)
                        .Where(x => x.Type == RequestType.Install)
                        .Where(x => x.Status == RequestStatus.Done)
                        .ToList()
                    group request by new {request.Customer, request.Address, request.PlannedDateTime.Date}
                    into r
                    select new ReportData
                    {
                        CustomerName = r.Key.Customer?.Name ?? "-",
                        Address = r.Key.Address,
                        DayNumber = r.Key.Date.Day,
                        RequestsCount = r.Count(),
                        RequestsSum = r.Sum(x => x.Sum)
                    }
                )
                .ToList();


            var result = new List<ReportData>();

            foreach (var day in Enumerable.Range(1, DateTime.DaysInMonth(selectedYear, selectedMonth)))
            {
                foreach (var request in requests)
                {
                    result.Add(new ReportData
                    {
                        CustomerName = request.CustomerName,
                        Address = request.Address,
                        DayNumber = day,
                        RequestsCount = request.DayNumber == day ? request.RequestsCount : 0,
                        RequestsSum = request.DayNumber == day ? request.RequestsSum : 0
                    });
                }
            }

            var dataTable = result.ToDataTable();
            dataTable.TableName = "ReportData";
            var dataSet = new DataSet();
            dataSet.Tables.Add(dataTable);
            webReport.Report.RegisterData(dataSet, "ReportDataSet");
            webReport.Report.Load(reportPath + "RequestsByCustomer.frx");
            webReport.CurrentTab.Name = "По заказчикам";

            webReport.Report.SetParameterValue("RequestsPeriod",
                $"{new CultureInfo("ru-RU").DateTimeFormat.GetMonthName(selectedMonth)} {selectedYear}");
            webReport.Report.SetParameterValue("RequestsTotalSum", requests.Sum(x => x.RequestsSum));
            webReport.Report.SetParameterValue("RequestsTotalCount", requests.Sum(x => x.RequestsCount));

            ViewBag.WebReport = webReport;
            ViewBag.Title = webReport.CurrentTab.Name;
            return View();
        }

        /// <summary>
        /// Суммы по поставщикам
        /// </summary>
        /// <returns></returns>
        public ActionResult RequestsSumsByCustomer(FormCollection formValue)
        {
            int selectedYear;
            int.TryParse(formValue["Year"], out selectedYear);
            selectedYear = selectedYear == 0 ? DateTime.Now.AddMonths(-1).Year : selectedYear;

            ViewData["Year"] =
                Enumerable.Range(DateTime.Now.Year, 10)
                    .Select(x => new SelectListItem
                    {
                        Text = x.ToString(),
                        Value = x.ToString(),
                        Selected = x == selectedYear
                    });

            int selectedMonth;
            int.TryParse(formValue["Month"], out selectedMonth);
            selectedMonth = selectedMonth == 0 ? DateTime.Now.AddMonths(-1).Month : selectedMonth;

            ViewData["Month"] =
                Enumerable.Range(1, 12)
                    .Select(x => new SelectListItem
                    {
                        Text = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[x - 1],
                        Value = x.ToString(),
                        Selected = x == selectedMonth
                    });

            var reportPath = GetReportPath();

            SetReport();

            var requests =
                (from request in dataStore
                        .GetAll<Request>()
                        .Where(x => x.PlannedDateTime.Year == selectedYear && x.PlannedDateTime.Month == selectedMonth)
                        .Where(x => x.Type == RequestType.Install)
                        .Where(x => x.Status == RequestStatus.Done)
                        .ToList()
                    group request by new {request.Customer, request.Address, request.PlannedDateTime.Date}
                    into r
                    select new ReportData
                    {
                        CustomerName = r.Key.Customer?.Name ?? "-",
                        Address = r.Key.Address,
                        DayNumber = r.Key.Date.Day,
                        RequestsSum = r.Sum(x => x.Sum)
                    }
                )
                .ToList();


            var result = new List<ReportData>();

            foreach (var day in Enumerable.Range(1, DateTime.DaysInMonth(selectedYear, selectedMonth)))
            {
                foreach (var request in requests)
                {
                    result.Add(new ReportData
                    {
                        CustomerName = request.CustomerName,
                        Address = request.Address,
                        DayNumber = day,
                        RequestsSum = request.DayNumber == day ? request.RequestsSum : 0
                    });
                }
            }

            var dataTable = result.ToDataTable();
            dataTable.TableName = "ReportData";
            var dataSet = new DataSet();
            dataSet.Tables.Add(dataTable);
            webReport.Report.RegisterData(dataSet, "ReportDataSet");
            webReport.Report.Load(reportPath + "RequestsSumsByCustomer.frx");
            webReport.CurrentTab.Name = "Суммы по заказчикам";

            webReport.Report.SetParameterValue("RequestsPeriod",
                $"{new CultureInfo("ru-RU").DateTimeFormat.GetMonthName(selectedMonth)} {selectedYear}");
            webReport.Report.SetParameterValue("RequestsTotalSum", requests.Sum(x => x.RequestsSum));

            ViewBag.WebReport = webReport;
            ViewBag.Title = webReport.CurrentTab.Name;
            return View();
        }
    }

    /// <summary>
    /// Вспомогательный класс
    /// </summary>
    public class ReportData
    {
        /// <summary>
        /// Наименование заказчика
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// Адрес заказчика
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// День месяца
        /// </summary>
        public int DayNumber { get; set; }

        /// <summary>
        /// Количество заявок
        /// </summary>
        public int RequestsCount { get; set; }

        /// <summary>
        /// Стоимость заявок
        /// </summary>
        public decimal RequestsSum { get; set; }
    }
}