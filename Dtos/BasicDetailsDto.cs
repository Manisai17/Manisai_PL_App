namespace Manisai_PL_App.Dtos
{
    public class BasicDetailsDto : RootDto
    {
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string MobileNumber { get; set; }
        public string EmailId { get; set; }
        public string PanNumber { get; set; }
        public string AadharNumber { get; set; }
        public int Pincode { get; set; }
        public DateTime Dob { get; set; }
    }
}