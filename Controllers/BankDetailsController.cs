using Microsoft.AspNetCore.Mvc;
using Manisai_PL_App.Dtos;
using Manisai_PL_App.Models;
using Manisai_PL_App.Services;

namespace Manisai_PL_App.Controllers
{
    public class BankDetailsController : Controller
    {
        private readonly PlappContext _context;

        public BankDetailsController(PlappContext context)
        {
            _context = context;
        }

        public IActionResult Index(int id)
        {
            var dto = new BankDetailsDto { AppId = id };
            return View(dto);
        }

        public IActionResult SaveBankDetails(BankDetailsDto bankDetailsDto)
        {
            if (bankDetailsDto.Bankname == null || bankDetailsDto.Bankbranch == null ||
                bankDetailsDto.Ifsccode == null || bankDetailsDto.Acctype == null ||
                bankDetailsDto.Accholdername == null || bankDetailsDto.Accnumber == 0)
            {
                TempData["Error"] = "All fields are required";
                return RedirectToAction("Index", new { id = bankDetailsDto.AppId });
            }

            var basicdetail = _context.Basicdetails.FirstOrDefault(b => b.Id == bankDetailsDto.AppId);
            if (basicdetail == null) return NotFound();

            var bankdetail = _context.Bankdetails.FirstOrDefault(b => b.Appid == bankDetailsDto.AppId);
            bool isExisting = bankdetail != null;
            if (!isExisting) bankdetail = new Bankdetail();

            bankdetail.Appid = bankDetailsDto.AppId;
            bankdetail.Bankname = bankDetailsDto.Bankname;
            bankdetail.Bankbranch = bankDetailsDto.Bankbranch;
            bankdetail.Ifsccode = bankDetailsDto.Ifsccode;
            bankdetail.Acctype = bankDetailsDto.Acctype;
            bankdetail.Accnumber = bankDetailsDto.Accnumber;
            bankdetail.Accholdername = bankDetailsDto.Accholdername;

            if (isExisting) _context.Bankdetails.Update(bankdetail);
            else _context.Bankdetails.Add(bankdetail);

            basicdetail.Appstage = "BANK_DETAILS";
            _context.Basicdetails.Update(basicdetail);
            _context.SaveChanges();

            string mapping = StageViewMapper.GetView(basicdetail.Appstage);
            string[] words = mapping.Split('/');
            return RedirectToAction(words[1], words[0], new { id = basicdetail.Id });
        }
        
    }
}