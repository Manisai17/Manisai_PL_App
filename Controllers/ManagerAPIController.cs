using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Manisai_PL_App.Models;
using Manisai_PL_App.Services;

namespace Manisai_PL_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "AdminPolicy")]
    public class ManagerAPIController : ControllerBase
    {
        private readonly PlappContext _context;
        private readonly IMailService _mailService;

        public ManagerAPIController(PlappContext context, IMailService mailService)
        {
            _context = context;
            _mailService = mailService;
        }

        [HttpGet]
        [Route("PendingApplications")]
        public ActionResult GetPending()
        {
            var apps = _context.Basicdetails.Where(a => a.Appstatus == "APPLICATION_IN_PROGRESS").ToList();
            return Ok(apps);
        }

        [HttpPost]
        [Route("ApproveLoan")]
        public ActionResult ApproveLoan(int id, int approvedAmount, int approvedTenure, decimal approvedRoi)
        {
            var basicdetail = _context.Basicdetails.FirstOrDefault(b => b.Id == id);
            var loandetail = _context.Loandetails.FirstOrDefault(l => l.Appid == id);
            if (basicdetail == null || loandetail == null) return NotFound();

            int years = approvedTenure / 12;
            decimal roiPerAnnum = (approvedAmount * approvedRoi) / 100;
            decimal totalRoi = roiPerAnnum * years;
            decimal emi = (totalRoi + approvedAmount) / approvedTenure;

            basicdetail.Appstatus = "LOAN_APPROVED";
            loandetail.Approvedamount = approvedAmount;
            loandetail.Approvedtenure = approvedTenure;
            loandetail.Approvedroi = approvedRoi;
            loandetail.Approvedemi = Math.Round(emi, 2);
            loandetail.Approvaldate = DateOnly.FromDateTime(DateTime.Today);

            _context.Loandetails.Update(loandetail);
            _context.Basicdetails.Update(basicdetail);
            _context.SaveChanges();

            string mailResult = _mailService.SendEmail("LoanApp Admin", "manisaigodishala@gmail.com",
    basicdetail.Firstname, basicdetail.Emailid,
    "Loan Approved", StandardMessages.CONGRATS_MESSAGE);

            return Ok(new { message = "Loan approved successfully", mailStatus = mailResult });
        }
    }
}