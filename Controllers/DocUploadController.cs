using Microsoft.AspNetCore.Mvc;
using Manisai_PL_App.Models;
using Manisai_PL_App.Services;

namespace Manisai_PL_App.Controllers
{
    public class DocUploadController : Controller
    {
        private readonly PlappContext _context;
        private readonly IWebHostEnvironment _env;

        public DocUploadController(PlappContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index(int id)
        {
            var docList = _context.Docuploaddetails.Where(d => d.Appid == id).ToList();
            ViewBag.Id = id;
            return View(docList);
        }

        public IActionResult AddNewDocument(int id)
        {
            ViewBag.Id = id;
            return View();
        }

        public IActionResult UploadDocument(IFormFile document, int id, string docType)
        {
            var basicdetail = _context.Basicdetails.FirstOrDefault(b => b.Id == id);
            if (basicdetail == null) return NotFound($"Application with ID {id} not found");

            string uploadDir = Path.Combine(_env.ContentRootPath, "Documents", id.ToString());
            Directory.CreateDirectory(uploadDir);
            string uploadPath = Path.Combine(uploadDir, document.FileName);

            using (var stream = new FileStream(uploadPath, FileMode.Create))
            {
                document.CopyTo(stream);
            }

            var details = new Docuploaddetail
            {
                Doctype = docType,
                Docpath = uploadPath,
                Appid = id
            };
            _context.Docuploaddetails.Add(details);
            _context.SaveChanges();

            return RedirectToAction("Index", new { id });
        }

        public IActionResult SaveChanges(int id)
        {
            var basicdetail = _context.Basicdetails.FirstOrDefault(b => b.Id == id);
            if (basicdetail == null) return NotFound();

            basicdetail.Appstage = "DOC_UPLOADED";
            _context.Basicdetails.Update(basicdetail);
            _context.SaveChanges();

            string mapping = StageViewMapper.GetView(basicdetail.Appstage);
            string[] words = mapping.Split('/');
            return RedirectToAction(words[1], words[0], new { id = basicdetail.Id });
        }
    }
}