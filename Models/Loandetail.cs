using System;
using System.Collections.Generic;

namespace Manisai_PL_App.Models;

public partial class Loandetail
{
    public int Id { get; set; }
    public int? Appid { get; set; }
    public int? Appliedamount { get; set; }
    public int? Appliedtenure { get; set; }
    public decimal? Roi { get; set; }
    public decimal? Emi { get; set; }
    public int? Approvedamount { get; set; }
    public int? Approvedtenure { get; set; }
    public decimal? Approvedroi { get; set; }
    public decimal? Approvedemi { get; set; }
    public DateOnly? Approvaldate { get; set; }

    public virtual Basicdetail? App { get; set; }
}