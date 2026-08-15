namespace Manisai_PL_App.Dtos
{
    public class CompanyDetailsDto : RootDto
    {
        public string Companyname { get; set; }
        public string Companymailid { get; set; }
        public int Grossincome { get; set; }
        public int Obligations { get; set; }
        public string Companyaddress { get; set; }
    }
}