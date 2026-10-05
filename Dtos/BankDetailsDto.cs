namespace Manisai_PL_App.Dtos
{
    public class BankDetailsDto : RootDto
    {
        public string Bankname { get; set; }
        public string Bankbranch { get; set; }
        public string Ifsccode { get; set; }
        public string Acctype { get; set; }
        public long Accnumber { get; set; }
        public string Accholdername { get; set; }
    }
}