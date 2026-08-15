using System;
using System.Collections.Generic;

namespace Manisai_PL_App.Models;

public partial class Companydetail
{
    public int Id { get; set; }
    public int? AppId { get; set; }
    public string? Companyname { get; set; }
    public string? Category { get; set; }
    public string? Companymailid { get; set; }
    public int? Grossincome { get; set; }
    public int? Obligations { get; set; }
    public string? Companyaddress { get; set; }

    public virtual Basicdetail? App { get; set; }
}