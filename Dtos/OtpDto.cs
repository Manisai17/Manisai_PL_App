using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Manisai_PL_App.Dtos
{
    public class OtpDto
    {
        public string EmailId { get; set; }

        [ValidateNever]
        public int Otp { get; set; }

        [ValidateNever]
        public string Message { get; set; }
    }
}