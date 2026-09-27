using System.Collections;

namespace Manisai_PL_App.Services
{
    public class StageViewMapper
    {
        private static Hashtable _stages = new Hashtable() {
            {"OTP_VERIFIED","BasicDetails/BasicDetails"},
            {"BASIC_DETAILS","CompanyDetails/Index"},
            {"COMPANY_DETAILS","LoanDetails/Index"},
            {"LOAN_DETAILS","PersonalDetails/Index"},
            {"PERSONAL_DETAILS","BankDetails/Index"},
            {"BANK_DETAILS","DocUpload/Index"},
            {"DOC_UPLOADED","ThankYou/Index"}
        };

        public static string GetView(string stage)
        {
            return (string)_stages[stage];
        }
    }
}