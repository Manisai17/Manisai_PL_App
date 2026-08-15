using System;
using System.Collections.Generic;

namespace Manisai_PL_App.Models;

public partial class Basicdetail
{
    public int Id { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string? Mobile { get; set; }
    public string? Emailid { get; set; }
    public string? Pannumber { get; set; }
    public string? Aadharnumber { get; set; }
    public int? Pincode { get; set; }
    public DateOnly? Dob { get; set; }
    public string? Leadid { get; set; }
    public string? Appstatus { get; set; }
    public string? Appstage { get; set; }
    public string? Statusremarks { get; set; }

    public virtual ICollection<Bankdetail> Bankdetails { get; set; } = new List<Bankdetail>();
    public virtual ICollection<Companydetail> Companydetails { get; set; } = new List<Companydetail>();
    public virtual ICollection<Docuploaddetail> Docuploaddetails { get; set; } = new List<Docuploaddetail>();
    public virtual ICollection<Loandetail> Loandetails { get; set; } = new List<Loandetail>();
    public virtual ICollection<Personaldetail> Personaldetails { get; set; } = new List<Personaldetail>();
}