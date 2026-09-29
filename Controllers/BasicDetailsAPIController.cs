using Microsoft.AspNetCore.Mvc;
using Manisai_PL_App.Dtos;
using Manisai_PL_App.Models;
using Manisai_PL_App.Services;

namespace Manisai_PL_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasicDetailsAPIController : ControllerBase
    {
        private readonly PlappContext _context;
        private readonly IMailService _mailService;

        public BasicDetailsAPIController(PlappContext context, IMailService mailService)
        {
            _context = context;
            _mailService = mailService;
        }
        [HttpPost]
        [Route("GenerateOtp")]
        public ActionResult GenerateOtp(OtpDto otpDto)
        {
            Random random = new Random();
            int otp = random.Next(1111, 9999);

            var record = new Otpmaster
            {
                EmailId = otpDto.EmailId,
                OtpCode = otp,
                CreatedAt = DateTime.UtcNow,
                ExpiryAt = DateTime.UtcNow.AddMinutes(5)
            };
            _context.Otpmasters.Add(record);
            _context.SaveChanges();

            _mailService.SendEmail("LoanApp Admin", "loanapp.admin@example.com",
                "Customer", otpDto.EmailId,
                "Otp Verification for Loan App",
                "Here is your 4 digit OTP for Loan App Login Verification: " + otp);

            otpDto.Message = "Otp Sent successfully";
            return Ok(otpDto);
        }
        [HttpPost]
        [Route("ValidateOtp")]
        public ActionResult ValidateOtp(OtpDto otpDto)
        {
            var validRecord = _context.Otpmasters
                .Where(o => o.EmailId == otpDto.EmailId
                         && o.OtpCode == otpDto.Otp
                         && o.ExpiryAt > DateTime.UtcNow)
                .OrderByDescending(o => o.CreatedAt)
                .FirstOrDefault();

            Basicdetail basicDetails = null;

            if (validRecord != null)
            {
                basicDetails = _context.Basicdetails.FirstOrDefault(d => d.Emailid == otpDto.EmailId);

                if (basicDetails == null)
                {
                    basicDetails = new Basicdetail
                    {
                        Emailid = otpDto.EmailId,
                        Appstage = "OTP_VERIFIED",
                        Appstatus = "APPLICATION_ID_GENERATED"
                    };
                    _context.Basicdetails.Add(basicDetails);
                    _context.SaveChanges();
                }
            }

            if (basicDetails == null)
            {
                return BadRequest(new OtpDto { EmailId = otpDto.EmailId, Message = "Invalid or expired OTP" });
            }

            var rootDto = new RootDto
            {
                Name = basicDetails.Firstname + " " + basicDetails.Lastname,
                AppId = basicDetails.Id,
                AppStage = basicDetails.Appstage,
                AppStatus = basicDetails.Appstatus,
                Message = "Success"
            };
            return Ok(rootDto);
        }
        [HttpPost]
        [Route("ValidateBasicDetails")]
        public ActionResult ValidateBasicDetails(BasicDetailsDto basicDetails)
        {
            if (basicDetails == null || basicDetails.MobileNumber == null || basicDetails.FirstName == null
                || basicDetails.PanNumber == null || basicDetails.AadharNumber == null)
                return BadRequest("Required fields are empty");

            var panMaster = _context.Panmasters.FirstOrDefault(p => p.Pancardnumber == basicDetails.PanNumber);
            if (panMaster == null) return BadRequest("Invalid PAN Number");

            var aadharMaster = _context.Aadharmasters.FirstOrDefault(a => a.Aadharnumber == basicDetails.AadharNumber);
            if (aadharMaster == null) return BadRequest("Invalid Aadhar Number");

            var pincodeMaster = _context.Pincodemasters.FirstOrDefault(p => p.Pincode == basicDetails.Pincode);
            if (pincodeMaster == null) return BadRequest("Invalid Pincode");

            var basicDetail = _context.Basicdetails.FirstOrDefault(d => d.Emailid == basicDetails.EmailId);
            if (basicDetail == null) return BadRequest("Application not found — verify OTP first");

            basicDetail.Firstname = basicDetails.FirstName;
            basicDetail.Lastname = basicDetails.LastName;
            basicDetail.Pannumber = basicDetails.PanNumber;
            basicDetail.Aadharnumber = basicDetails.AadharNumber;
            basicDetail.Pincode = basicDetails.Pincode;
            basicDetail.Dob = DateOnly.FromDateTime(basicDetails.Dob);
            basicDetail.Mobile = basicDetails.MobileNumber;
            basicDetail.Leadid = basicDetail.Id + "-" + basicDetails.MobileNumber;
            basicDetail.Appstage = "BASIC_DETAILS";
            basicDetail.Appstatus = "APPLICATION_IN_PROGRESS";

            _context.Basicdetails.Update(basicDetail);
            _context.SaveChanges();

            basicDetails.AppId = basicDetail.Id;
            basicDetails.AppStage = basicDetail.Appstage;
            basicDetails.AppStatus = basicDetail.Appstatus;
            basicDetails.Name = basicDetail.Firstname + " " + basicDetail.Lastname;
            basicDetails.Message = "Basic Details Saved Successfully";

            return Ok(basicDetails);
        }
    }
}