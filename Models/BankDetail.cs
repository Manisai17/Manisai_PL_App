using System;
using System.Collections.Generic;

namespace Manisai_PL_App.Models;

public partial class Bankdetail
{
    public int Id { get; set; }
    public int? Appid { get; set; }
    public string? Bankname { get; set; }
    public string? Bankbranch { get; set; }
    public string? Ifsccode { get; set; }
    public string? Acctype { get; set; }
    public long? Accnumber { get; set; }
    public string? Accholdername { get; set; }

    public virtual Basicdetail? App { get; set; }
}