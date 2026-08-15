using System;
using System.Collections.Generic;

namespace Manisai_PL_App.Models;

public partial class Personaldetail
{
    public int Id { get; set; }
    public int? Appid { get; set; }
    public string? Fathername { get; set; }
    public string? Mothername { get; set; }
    public string? Permanentaddress { get; set; }
    public string? Currentaddress { get; set; }
    public string? Reference1name { get; set; }
    public string? Reference1email { get; set; }
    public string? Reference1mobile { get; set; }
    public string? Reference1relation { get; set; }
    public string? Reference2name { get; set; }
    public string Reference2email { get; set; } = null!;
    public string? Reference2mobile { get; set; }
    public string? Reference2relation { get; set; }

    public virtual Basicdetail? App { get; set; }
}