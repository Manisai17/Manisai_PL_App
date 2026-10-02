using Microsoft.AspNetCore.Mvc;
using Manisai_PL_App.Dtos;
using Manisai_PL_App.Models;
using Manisai_PL_App.Services;

namespace Manisai_PL_App.Controllers
{
    public class CompanyDetailsController : Controller
    {
        private readonly PlappContext _context;

        public CompanyDetailsController(PlappContext context)
        {
            _context = context;
        }

        public IActionResult Index(int id)
        {
            ViewBag.Appid = id;
            return View();
        }

        public IActionResult SaveCompanyDetails(CompanyDetailsDto companyDto)
        {
            bool isNew = false, isRejected = false;

            var rulesIncome = _context.Rulesmasters.FirstOrDefault(r => r.Rulename == "income");
            var rulesObligation = _context.Rulesmasters.FirstOrDefault(r => r.Rulename == "oblpercent");
            var companymaster = _context.Companymasters.FirstOrDefault(c => c.Companyname == companyDto.Companyname);
            var basicdetail = _context.Basicdetails.FirstOrDefault(b => b.Id == companyDto.AppId);
            if (basicdetail == null) return NotFound();

            var companydetail = _context.Companydetails.FirstOrDefault(c => c.AppId == companyDto.AppId);
            if (companydetail == null) { companydetail = new Companydetail(); isNew = true; }

            companydetail.Companyname = companyDto.Companyname;
            companydetail.AppId = companyDto.AppId;
            companydetail.Companymailid = companyDto.Companymailid;
            companydetail.Companyaddress = companyDto.Companyaddress;
            companydetail.Grossincome = companyDto.Grossincome;
            companydetail.Obligations = companyDto.Obligations;
            companydetail.Category = companymaster?.Category ?? "Others";

            if (companyDto.Grossincome <= rulesIncome.Minvalue || companyDto.Grossincome >= rulesIncome.Maxvalue)
            {
                isRejected = true;
                basicdetail.Statusremarks = "INCOME_RULE_FAILED";
            }

            if (companyDto.Grossincome > 0)
            {
                int obligationPercent = (companyDto.Obligations * 100) / companyDto.Grossincome;
                if (obligationPercent > rulesObligation.Maxvalue)
                {
                    isRejected = true;
                    basicdetail.Statusremarks = "OBLIGATION_RULE_FAILED";
                }
            }

            basicdetail.Appstage = "COMPANY_DETAILS";
            basicdetail.Appstatus = isRejected ? "APPLICATION_REJECTED" : basicdetail.Appstatus;

            if (isNew) _context.Companydetails.Add(companydetail);
            else _context.Companydetails.Update(companydetail);
            _context.Basicdetails.Update(basicdetail);
            _context.SaveChanges();

            if (isRejected) return View("Rejected/Thankyou");

            string mapping = StageViewMapper.GetView(basicdetail.Appstage);
            string[] words = mapping.Split('/');
            return RedirectToAction(words[1], words[0], new { id = basicdetail.Id });
        }
    }
}