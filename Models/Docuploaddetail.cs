using System;
using System.Collections.Generic;

namespace Manisai_PL_App.Models;

public partial class Docuploaddetail
{
    public int Id { get; set; }
    public int? Appid { get; set; }
    public string? Doctype { get; set; }
    public string? Docpath { get; set; }

    public virtual Basicdetail? App { get; set; }
}