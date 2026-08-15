namespace Manisai_PL_App.Dtos
{
    public class LoanDetailsDto : RootDto
    {
        public double MinLoanAmount { get; set; }
        public double MaxLoanAmount { get; set; }
        public double Roi { get; set; }
        public int Tenure { get; set; }
        public int Emi { get; set; }
    }
}