namespace Manisai_PL_App.Models;

public partial class Rulesmaster
{
    public int Id { get; set; }
    public string? Rulename { get; set; }
    public int? Minvalue { get; set; }
    public int? Maxvalue { get; set; }
}