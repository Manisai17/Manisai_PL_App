using Microsoft.AspNetCore.Mvc;
using Manisai_PL_App.Dtos;
using Manisai_PL_App.Models;

namespace Manisai_PL_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyDetailsAPIController : ControllerBase
    {
        private readonly PlappContext _context;

        public CompanyDetailsAPIController(PlappContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("SaveCompanyDetails")]
        public ActionResult SaveCompanyDetails(CompanyDetailsDto companyDto)
        {
            bool isNew = false, isRejected = false;

            var rulesIncome = _context.Rulesmasters.FirstOrDefault(r => r.Rulename == "income");
            var rulesObligation = _context.Rulesmasters.FirstOrDefault(r => r.Rulename == "oblpercent");

            if (rulesIncome == null || rulesObligation == null)
            {
                return StatusCode(500, "Business rules not configured — missing 'income' or 'oblpercent' in Rulesmasters");
            }
            var companymaster = _context.Companymasters.FirstOrDefault(c => c.Companyname == companyDto.Companyname);
            var basicdetail = _context.Basicdetails.FirstOrDefault(b => b.Id == companyDto.AppId);
            if (basicdetail == null) return BadRequest("Application not found");

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
            if (isRejected) basicdetail.Appstatus = "APPLICATION_REJECTED";

            if (isNew) _context.Companydetails.Add(companydetail);
            else _context.Companydetails.Update(companydetail);
            _context.Basicdetails.Update(basicdetail);
            _context.SaveChanges();

            companyDto.AppStage = basicdetail.Appstage;
            companyDto.AppStatus = basicdetail.Appstatus;
            companyDto.Message = isRejected
                ? StandardMessages.REJECTED_MESSAGE + " - Reason: " + basicdetail.Statusremarks
                : "Company Details Saved Successfully";

            return Ok(companyDto);
        }
    }
}