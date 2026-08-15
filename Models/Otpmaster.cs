using System;

namespace Manisai_PL_App.Models;

public partial class Otpmaster
{
    public int Id { get; set; }
    public string EmailId { get; set; } = null!;
    public int OtpCode { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiryAt { get; set; }
}