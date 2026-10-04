using Manisai_PL_App.Models;
using Manisai_PL_App.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Manisai_PL_App.Controllers
{
    [Authorize(AuthenticationSchemes = "Cookies", Policy = "AdminPolicy")]
    public class ManagerController : Controller
    {
        private readonly PlappContext _context;
        private readonly IMailService _mailService;

        public ManagerController(PlappContext context, IMailService mailService)
        {
            _context = context;
            _mailService = mailService;
        }

        public IActionResult Index()
        {
            var pending = _context.Basicdetails
                .Where(b => b.Appstatus == "APPLICATION_IN_PROGRESS")
                .ToList();
            return View(pending);
        }

        public IActionResult ViewApplication(int id)
        {
            var basicdetail = _context.Basicdetails.FirstOrDefault(b => b.Id == id);
            var loandetail = _context.Loandetails.FirstOrDefault(l => l.Appid == id);
            if (basicdetail == null) return NotFound();

            ViewBag.Id = id;
            ViewBag.Basicdetail = basicdetail;
            ViewBag.Loandetail = loandetail;
            return View();
        }

        public IActionResult UpdateLoanStatus(int id, int approvedLoanAmt, int approvedTenure, decimal approvedRoi)
        {
            var basicdetail = _context.Basicdetails.FirstOrDefault(b => b.Id == id);
            var loandetail = _context.Loandetails.FirstOrDefault(l => l.Appid == id);
            if (basicdetail == null || loandetail == null) return NotFound();

            int years = approvedTenure / 12;
            decimal roiPerAnnum = (approvedLoanAmt * approvedRoi) / 100;
            decimal totalRoi = roiPerAnnum * years;
            decimal emi = (totalRoi + approvedLoanAmt) / approvedTenure;

            basicdetail.Appstatus = "LOAN_APPROVED";
            loandetail.Approvedamount = approvedLoanAmt;
            loandetail.Approvedtenure = approvedTenure;
            loandetail.Approvedroi = approvedRoi;
            loandetail.Approvedemi = Math.Round(emi, 2);
            loandetail.Approvaldate = DateOnly.FromDateTime(DateTime.Today);

            _context.Loandetails.Update(loandetail);
            _context.Basicdetails.Update(basicdetail);
            _context.SaveChanges();

            _mailService.SendEmail("LoanApp Admin", "manisaigodishala@gmail.com",
                basicdetail.Firstname, basicdetail.Emailid,
                "Loan Approved", StandardMessages.CONGRATS_MESSAGE);

            return RedirectToAction("Index");
        }
    }
}