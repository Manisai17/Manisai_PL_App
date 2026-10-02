using Microsoft.AspNetCore.Mvc;
using Manisai_PL_App.Dtos;
using Manisai_PL_App.Models;
using Manisai_PL_App.Services;

namespace Manisai_PL_App.Controllers
{
    public class PersonalDetailsController : Controller
    {
        private readonly PlappContext _context;

        public PersonalDetailsController(PlappContext context)
        {
            _context = context;
        }

        public IActionResult Index(int id)
        {
            var dto = new PersonalDetailsDto { AppId = id };
            return View(dto);
        }

        public IActionResult SavePersonalDetails(PersonalDetailsDto personalDetailsDto)
        {
            var personaldetail = _context.Personaldetails.FirstOrDefault(p => p.Appid == personalDetailsDto.AppId);
            var basicdetail = _context.Basicdetails.FirstOrDefault(b => b.Id == personalDetailsDto.AppId);
            if (basicdetail == null) return NotFound();

            bool isExisting = personaldetail != null;
            if (!isExisting) personaldetail = new Personaldetail();

            personaldetail.Appid = personalDetailsDto.AppId;
            personaldetail.Fathername = personalDetailsDto.Fathername;
            personaldetail.Mothername = personalDetailsDto.Mothername;
            personaldetail.Permanentaddress = personalDetailsDto.Permanentaddress;
            personaldetail.Currentaddress = personalDetailsDto.Currentaddress;
            personaldetail.Reference1name = personalDetailsDto.Reference1name;
            personaldetail.Reference1email = personalDetailsDto.Reference1email;
            personaldetail.Reference1mobile = personalDetailsDto.Reference1mobile;
            personaldetail.Reference1relation = personalDetailsDto.Reference1relation;
            personaldetail.Reference2name = personalDetailsDto.Reference2name;
            personaldetail.Reference2email = personalDetailsDto.Reference2email ?? "";
            personaldetail.Reference2mobile = personalDetailsDto.Reference2mobile;
            personaldetail.Reference2relation = personalDetailsDto.Reference2relation;

            basicdetail.Appstage = "PERSONAL_DETAILS";

            if (isExisting) _context.Personaldetails.Update(personaldetail);
            else _context.Personaldetails.Add(personaldetail);
            _context.Basicdetails.Update(basicdetail);
            _context.SaveChanges();

            string mapping = StageViewMapper.GetView(basicdetail.Appstage);
            string[] words = mapping.Split('/');
            return RedirectToAction(words[1], words[0], new { id = basicdetail.Id });
        }
    }
}