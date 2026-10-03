using Microsoft.AspNetCore.Mvc;
using Manisai_PL_App.Dtos;
using Manisai_PL_App.Models;
using Manisai_PL_App.Services;

namespace Manisai_PL_App.Controllers
{
    public class LoanDetailsController : Controller
    {
        private readonly PlappContext _context;

        public LoanDetailsController(PlappContext context)
        {
            _context = context;
        }

        public IActionResult Index(int id)
        {
            var rulesmasters = _context.Rulesmasters.ToList();
            var companyDetail = _context.Companydetails.FirstOrDefault(c => c.AppId == id);
            var basicdetail = _context.Basicdetails.FirstOrDefault(b => b.Id == id);
            if (companyDetail == null || basicdetail == null || basicdetail.Dob == null) return NotFound();

            var today = DateOnly.FromDateTime(DateTime.Today);
            int age = today.Year - basicdetail.Dob.Value.Year;
            if (basicdetail.Dob.Value > today.AddYears(-age)) age--;

            int grossIncome = companyDetail.Grossincome ?? 0;
            int obligations = companyDetail.Obligations ?? 0;

            var service = new LoanCalculatorService();

            try
            {
                var dto = service.CalculateLoanDetails(age, grossIncome, obligations, rulesmasters);
                dto.AppId = id;
                return View(dto);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        public IActionResult SaveLoanDetails(LoanDetailsDto loanDto)
        {
            var loandetail = _context.Loandetails.FirstOrDefault(l => l.Appid == loanDto.AppId);
            var basicdetail = _context.Basicdetails.FirstOrDefault(b => b.Id == loanDto.AppId);
            if (basicdetail == null) return NotFound();

            bool isExisting = loandetail != null;
            if (!isExisting) loandetail = new Loandetail();

            loandetail.Appid = loanDto.AppId;
            loandetail.Appliedamount = (int)loanDto.MaxLoanAmount;
            loandetail.Appliedtenure = loanDto.Tenure;
            loandetail.Roi = (decimal)loanDto.Roi;
            loandetail.Emi = loanDto.Emi;

            basicdetail.Appstage = "LOAN_DETAILS";

            if (isExisting) { _context.Loandetails.Update(loandetail); }
            else { _context.Loandetails.Add(loandetail); }
            _context.Basicdetails.Update(basicdetail);
            _context.SaveChanges();

            string mapping = StageViewMapper.GetView(basicdetail.Appstage);
            string[] words = mapping.Split('/');
            return RedirectToAction(words[1], words[0], new { id = basicdetail.Id });
        }
    }
}