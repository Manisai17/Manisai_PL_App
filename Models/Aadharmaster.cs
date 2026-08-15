namespace Manisai_PL_App.Models;

public partial class Aadharmaster
{
    public int Id { get; set; }
    public string? Aadharnumber { get; set; }
    public string? Firstname { get; set; }
    public string? Middlename { get; set; }
    public string? Lastname { get; set; }
    public string? Fathername { get; set; }
    public DateOnly? Dob { get; set; }
    public string? Address { get; set; }
    public byte? Gender { get; set; }
}