using Manisai_PL_App.Dtos;
using Manisai_PL_App.Models;

namespace Manisai_PL_App.Services
{
    public class LoanCalculatorService
    {
        public LoanDetailsDto CalculateLoanDetails(int age, int grossIncome, int currentObligation, List<Rulesmaster> rulesMasters)
        {
            // tenure should not exceed available age limit
            Rulesmaster rule = rulesMasters.FirstOrDefault(rule => rule.Rulename == "age");
            int MinTenure = 12;

            int Tenure = (int)(rule.Maxvalue - age) * 12;
            if (Tenure < 12) Tenure = 12;
            if (Tenure > 72) Tenure = 72;

            // 65 - obligation% left will be the max emi that customer can pay
            double maxObligation = (grossIncome * 65) / 100;
            int maxEmi = (int)maxObligation - currentObligation;

            // roi will be calculated based on tenure
            double roi = 10.25;

            if (Tenure < 24) roi += 3.0;
            else if (Tenure >= 24 && Tenure <= 36) roi += 2.5;
            else if (Tenure >= 36 && Tenure <= 48) roi += 2.0;
            else if (Tenure >= 48 && Tenure <= 60) roi += 1.5;
            else if (Tenure >= 60) roi += 1.0;

            // loan amount will be left over part of (tenure*emi) - roi per annum
            double MinLoanAmount = 50000.00;

            double totalEmiPayable = Tenure * maxEmi;
            int years = Tenure / 12;
            double roiAmount = (totalEmiPayable * (roi * years)) / 100;
            double MaxLoanAmount = totalEmiPayable - roiAmount;

            LoanDetailsDto dto = new LoanDetailsDto();
            dto.MinLoanAmount = MinLoanAmount;
            dto.MaxLoanAmount = MaxLoanAmount;
            dto.Tenure = Tenure;
            dto.Roi = roi;
            dto.Emi = maxEmi;

            return dto;
        }
    }
}