namespace Manisai_PL_App.Models;

public partial class Pincodemaster
{
    public int Id { get; set; }
    public int? Pincode { get; set; }
    public string? Circle { get; set; }
    public string? Village { get; set; }
    public string? District { get; set; }
    public string? State { get; set; }
    public byte? Servicable { get; set; }
}