using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectSemester.Helpers;
using ProjectSemester3.Helpers;
using ProjectSemester3.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Syncfusion.HtmlConverter;
using System.IO;
using Syncfusion.Pdf;

namespace ProjectSemester3.Controllers
{
    [Route("transfer")]
    public class TransactionController : Controller
    {
        private DatabaseContext db;
        private readonly IHostEnvironment hostingEnvironment;
        public TransactionController(DatabaseContext _db, IHostEnvironment _hostingEnvironment)
        {
            db = _db;
            hostingEnvironment = _hostingEnvironment;
        }
        [Route("index")]
        [Route("")]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = db.Settings.Find(1).Title;
            ViewBag.mail = db.Helps.Find(1).Email;
            ViewBag.phone1 = db.Helps.Find(1).ContactNumber1;
            ViewBag.phone2 = db.Helps.Find(1).ContactNumber2;
            var account = db.AccountObjects.FirstOrDefault(a => a.Username.Equals(HttpContext.Session.GetString("username")));
            ViewBag.bankAccounts = db.BankAccounts.Where(a => a.UserAccountId == account.Id).ToList();
            ViewBag.name = account.Name;
            return View();
        }
        [Route("index")]
        [HttpPost]
        public IActionResult Index(Transaction transaction)
        {
            try
            {
                var bankCode = Request.Form["bankto"];
                var bank = db.BankAccounts.FirstOrDefault(b => b.BankCode.Equals(bankCode));
                transaction.BankAccountIdTo = bank.Id;
                ViewBag.bankFrom = db.BankAccounts.Find(transaction.BankAccountIdFrom);
                ViewBag.bankTo = db.BankAccounts.Find(transaction.BankAccountIdTo);
                return View("Step2",transaction);
            }
            catch(Exception e)
            {
                Debug.WriteLine("Step1 Failed: " + e.InnerException.Message);
                return null;
            }
        }
        [Route("step2")]
        public IActionResult Step2(Transaction transaction)
        {
            ViewBag.bankFrom = db.BankAccounts.Find(transaction.BankAccountIdFrom);
            ViewBag.bankTo = db.BankAccounts.Find(transaction.BankAccountIdTo);
            return View("Step2",transaction);
        }
        [Route("step2")]
        [HttpPost]
        public IActionResult Step22(Transaction transaction)
        {
           
            var bankAccountTo = db.BankAccounts.FirstOrDefault(b => b.Id == transaction.BankAccountIdTo);
            var bankAccountFrom = db.BankAccounts.FirstOrDefault(b => b.Id == transaction.BankAccountIdFrom);
            var accountTo = db.AccountObjects.FirstOrDefault(a => a.Id == bankAccountFrom.UserAccountId);
            var accounForm = db.Helps.Find(1);
            var nameFrom = db.Settings.Find(1).Title;
            var mailFrom = accounForm.Email;
            var password = accounForm.Password;
            var subject = accounForm.Subject;
            var nameTo = accountTo.Name;
            var mailTo = accountTo.Email;
            Debug.WriteLine("Mail to: " + mailTo);
            var otp = new Generator();
            var otpSend = otp.GenerateNumericString(6);
            var mail = new MailHelper();
            if (mail.Send(nameFrom, "tinhoang7901@gmail.com", "0347557353", nameTo, mailTo, subject, "Your OTP " + otpSend))
            {
                var bankOtp = new BankOtp();
                bankOtp.Otp = otpSend;
                bankOtp.Date = DateTime.Parse(DateTime.Now.ToString(("dd/MM/yyyy HH:mm:ss")));
                bankOtp.AccountObjectId = bankAccountFrom.UserAccountId;
                bankOtp.Status = false;
                db.BankOtps.Add(bankOtp);
                db.SaveChanges();

            }
            return View("Step3",transaction);
        }
        [Route("step3")]
        public IActionResult Step3(Transaction transaction)
        {
            return View("Step3",transaction);
        }
        [Route("step3")]
        [HttpPost]
        public IActionResult Step33(Transaction transaction)
        {
            
            try
            {
                var otpCode = Request.Form["otp"];
                var timeOtp = DateTime.Parse(DateTime.Now.ToString(("dd/MM/yyyy HH:mm:ss")));
                if (otpCode == "")
                {
                    ViewBag.otpFailed = "OTP Code invalid";
                    return View("Step3");
                }
                else
                {
                    var bankOtp = db.BankOtps.FirstOrDefault(b => b.Otp.Equals(otpCode));
                    if(bankOtp == null)
                    {
                        ViewBag.otpFailed = "OTP Code Wrong";
                        return View("Step3");
                    }
                    var diff = timeOtp.Subtract(bankOtp.Date).TotalSeconds;
                    Debug.WriteLine("TotalSeconds: " + diff);
                    if (bankOtp != null && bankOtp.Status == false && diff <= 30)
                    {
                        if (transaction != null)
                        {
                            var accountFrom = db.BankAccounts.Find(transaction.BankAccountIdFrom);
                            var accountTo = db.BankAccounts.Find(transaction.BankAccountIdTo);
                            accountFrom.Balance = accountFrom.Balance - transaction.Amount;
                            accountTo.Balance = accountTo.Balance + transaction.Amount;
                            bankOtp.Status = true;

                            transaction.Time = DateTime.Parse(DateTime.Now.ToString(("dd/MM/yyyy HH:mm:ss")));
                            transaction.ResultId = 1;
                            transaction.BalanceFrom = accountFrom.Balance;
                            transaction.BalanceTo = accountTo.Balance;


                            db.Transactions.Add(transaction);
                            db.SaveChanges();
                        }
                        else
                        {
                            ViewBag.otpFailed = "OTP Code invalid";
                            return View("Step3");
                        }
                        ViewBag.bankFrom = db.BankAccounts.Find(transaction.BankAccountIdFrom);
                        ViewBag.bankTo = db.BankAccounts.Find(transaction.BankAccountIdTo);
                        return View("Success",transaction);
                    }
                    else
                    {
                        ViewBag.otpFailed = "OTP Code invalid";
                        return View("Step3");
                    }

                }
                
            }
            catch(Exception e){
                Debug.WriteLine("OTP Failed: " + e.InnerException.Message);
                return View("Index", "Home");
            }
        }
        [Route("success")]
        public IActionResult Success(Transaction transaction)
        {
            ViewBag.bankFrom = db.BankAccounts.Find(transaction.BankAccountIdFrom);
            ViewBag.bankTo = db.BankAccounts.Find(transaction.BankAccountIdTo);
            return View("Success",transaction);
        }
        [Route("export-to-pdf")]
        public IActionResult ExportToPDF()
        {
            var id = Request.Form["id"];
            var value = Request.Form["value"];
            Debug.WriteLine("export id: " + id + value);
            //Initialize HTML to PDF converter with Blink rendering engine 
            HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter(HtmlRenderingEngine.Blink);
            BlinkConverterSettings settings = new BlinkConverterSettings();
            //Set the BlinkBinaries folder path 
            settings.BlinkPath = Path.Combine(hostingEnvironment.ContentRootPath, "BlinkBinariesWindows");
            //Assign Blink settings to HTML converter
            htmlConverter.ConverterSettings = settings;
            //Convert URL to PDF
            PdfDocument document = htmlConverter.Convert("http://localhost:22418/bank-account/reports/" + id + "/" + value);
            MemoryStream stream = new MemoryStream();
            document.Save(stream);
            return File(stream.ToArray(), System.Net.Mime.MediaTypeNames.Application.Pdf, "ListReport.pdf");
        }
        [Route("export-to-pdf-by-date")]
        public IActionResult ExportToPDFByDate()
        {
            var id = Request.Form["id"];
            var from = Request.Form["from"];
            var to = Request.Form["to"];
            var min = Request.Form["min"];
            var max = Request.Form["max"];
            
            //Initialize HTML to PDF converter with Blink rendering engine 
            HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter(HtmlRenderingEngine.Blink);
            BlinkConverterSettings settings = new BlinkConverterSettings();
            //Set the BlinkBinaries folder path 
            settings.BlinkPath = Path.Combine(hostingEnvironment.ContentRootPath, "BlinkBinariesWindows");
            //Assign Blink settings to HTML converter
            htmlConverter.ConverterSettings = settings;
            //Convert URL to PDF
            PdfDocument document = htmlConverter.Convert("http://localhost:22418/bank-account/report2s/" + id + "/" + from + "/" + to + "/" + min + "/" + max);
            MemoryStream stream = new MemoryStream();
            document.Save(stream);
            return File(stream.ToArray(), System.Net.Mime.MediaTypeNames.Application.Pdf, "ListReport.pdf");
        }

        [Route("statement")]
        public IActionResult Statement()
        {
            var id = Request.Form["id"];

            //Initialize HTML to PDF converter with Blink rendering engine 
            HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter(HtmlRenderingEngine.Blink);
            BlinkConverterSettings settings = new BlinkConverterSettings();
            //Set the BlinkBinaries folder path 
            settings.BlinkPath = Path.Combine(hostingEnvironment.ContentRootPath, "BlinkBinariesWindows");
            //Assign Blink settings to HTML converter
            htmlConverter.ConverterSettings = settings;
            //Convert URL to PDF
            PdfDocument document = htmlConverter.Convert("http://localhost:22418/home/reports/" + id);
            MemoryStream stream = new MemoryStream();
            document.Save(stream);
            return File(stream.ToArray(), System.Net.Mime.MediaTypeNames.Application.Pdf, "ListReport.pdf");
        }

        public IActionResult ExportToPDFFile()
        {
            //Initialize HTML to PDF converter with Blink rendering engine 
            HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter(HtmlRenderingEngine.Blink);
            BlinkConverterSettings settings = new BlinkConverterSettings();
            //Set the BlinkBinaries folder path 
            settings.BlinkPath = Path.Combine(hostingEnvironment.ContentRootPath, "BlinkBinariesWindows");
            //Assign Blink settings to HTML converter
            htmlConverter.ConverterSettings = settings;
            //Convert URL to PDF
            PdfDocument document = htmlConverter.Convert("http://localhost:22418/reports");
            MemoryStream stream = new MemoryStream();
            document.Save(stream);
            return File(stream.ToArray(), System.Net.Mime.MediaTypeNames.Application.Pdf, "Report.pdf");
        }
    }
}
