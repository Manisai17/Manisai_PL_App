using Microsoft.AspNetCore.Mvc;
using Manisai_PL_App.Dtos;
using Manisai_PL_App.Models;
using Manisai_PL_App.Services;

namespace Manisai_PL_App.Controllers
{
    public class BasicDetailsController : Controller
    {
        private readonly PlappContext _context;
        private readonly IMailService _mailService;

        public BasicDetailsController(PlappContext context, IMailService mailService)
        {
            _context = context;
            _mailService = mailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult OtpVerification(string EmailId)
        {
            Random random = new Random();
            int otp = random.Next(1111, 9999);

            _context.Otpmasters.Add(new Otpmaster
            {
                EmailId = EmailId,
                OtpCode = otp,
                CreatedAt = DateTime.UtcNow,
                ExpiryAt = DateTime.UtcNow.AddMinutes(5)
            });
            _context.SaveChanges();

            string response = _mailService.SendEmail("LoanApp Admin", "manisaigodishala@gmail.com",
                                                        "Customer", EmailId,
                                                        "Otp Verification for Loan App",
                                                        "Here is your 4 digit OTP for Loan App Login Verification: " + otp);
            if (response == "OK")
            {
                ViewBag.EmailId = EmailId;
                return View("OtpVerification");
            }
            else
            {
                ViewBag.msg = "Could not send email: " + response;
                return View("Index");
            }
        }

        public IActionResult ValidateOtp(int otp, string Emailid)
        {
            var validRecord = _context.Otpmasters
                .Where(o => o.EmailId == Emailid && o.OtpCode == otp && o.ExpiryAt > DateTime.UtcNow)
                .OrderByDescending(o => o.CreatedAt)
                .FirstOrDefault();

            if (validRecord != null)
            {
                Basicdetail basicDetails = _context.Basicdetails.FirstOrDefault(details => details.Emailid == Emailid);
                if (basicDetails == null)
                {
                    basicDetails = new Basicdetail
                    {
                        Emailid = Emailid,
                        Appstage = "OTP_VERIFIED",
                        Appstatus = "APPLICATION_ID_GENERATED"
                    };
                    _context.Basicdetails.Add(basicDetails);
                    _context.SaveChanges();
                    ViewBag.Emailid = Emailid;
                    return View("BasicDetails");
                }
                else
                {
                    string mapping = StageViewMapper.GetView(basicDetails.Appstage);
                    string[] words = mapping.Split('/');
                    return RedirectToAction(words[1], words[0], new { id = basicDetails.Id });
                }
            }

            ViewBag.EmailId = Emailid;
            ViewBag.msg = "Wrong or expired OTP";
            return View("OtpVerification");
        }

        public IActionResult ValidateBasicDetails(BasicDetailsDto basicDetails)
        {
            if (basicDetails == null || basicDetails.MobileNumber == null || basicDetails.FirstName == null
                || basicDetails.PanNumber == null || basicDetails.AadharNumber == null)
            {
                TempData["Error"] = "All fields are required";
                return RedirectToAction("Index");
            }

            var panMaster = _context.Panmasters.FirstOrDefault(p => p.Pancardnumber == basicDetails.PanNumber);
            if (panMaster == null)
            {
                TempData["Error"] = "Invalid Pan Number";
                return RedirectToAction("Index");
            }

            var aadharMaster = _context.Aadharmasters.FirstOrDefault(a => a.Aadharnumber == basicDetails.AadharNumber);
            if (aadharMaster == null)
            {
                TempData["Error"] = "Invalid Aadhar Number";
                return RedirectToAction("Index");
            }

            var pincodeMaster = _context.Pincodemasters.FirstOrDefault(p => p.Pincode == basicDetails.Pincode);
            if (pincodeMaster == null)
            {
                TempData["Error"] = "Invalid Pincode";
                return RedirectToAction("Index");
            }

            var today = DateOnly.FromDateTime(DateTime.Today);
            var dob = DateOnly.FromDateTime(basicDetails.Dob);
            int age = today.Year - dob.Year;
            if (dob > today.AddYears(-age)) age--;

            var ageRule = _context.Rulesmasters.FirstOrDefault(rule => rule.Rulename == "age");
            if (ageRule != null && (age < ageRule.Minvalue || age > ageRule.Maxvalue))
            {
                TempData["Error"] = "Applicant does not meet the age requirement";
                return RedirectToAction("Index");
            }

            var basicDetail = _context.Basicdetails.FirstOrDefault(details => details.Emailid == basicDetails.EmailId);
            if (basicDetail == null)
            {
                TempData["Error"] = "Application not found — please verify OTP first";
                return RedirectToAction("Index");
            }

            basicDetail.Firstname = basicDetails.FirstName;
            basicDetail.Lastname = basicDetails.LastName;
            basicDetail.Pannumber = basicDetails.PanNumber;
            basicDetail.Aadharnumber = basicDetails.AadharNumber;
            basicDetail.Pincode = basicDetails.Pincode;
            basicDetail.Dob = dob;
            basicDetail.Mobile = basicDetails.MobileNumber;
            basicDetail.Leadid = basicDetail.Id + "-" + basicDetails.MobileNumber;
            basicDetail.Appstage = "BASIC_DETAILS";
            basicDetail.Appstatus = "APPLICATION_IN_PROGRESS";

            _context.Basicdetails.Update(basicDetail);
            _context.SaveChanges();

            string mapping = StageViewMapper.GetView(basicDetail.Appstage);
            string[] words = mapping.Split('/');
            return RedirectToAction(words[1], words[0], new { id = basicDetail.Id });
        }
    }
}