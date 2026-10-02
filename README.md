# Personal Loan Application (PL_APP_MVC)

An ASP.NET Core MVC loan application wizard with OTP email verification,
JWT authentication with role-based authorization, Serilog structured
logging, and custom middleware. Built from scratch as a learning and
portfolio project.

**Status: in active development.**

## Tech stack
- Entity Framework Core (code-first) + SQL Server
- JWT Bearer authentication with policy-based role authorization
- Serilog (console + rolling file sinks)
- MailKit (SMTP email via Brevo)
- BCrypt (password hashing)
- Custom ASP.NET Core middleware

## Progress
- [x] Part 1: Program.cs wiring and custom RequestLoggingMiddleware
- [x] Part 2: EF Core models, PlappContext, initial migration (14 tables)
- [x] Part 3: DTOs (RootDto plus stage-specific DTOs)
- [x] Part 4: Services (MailService, LoanCalculatorService, StageViewMapper)
- [x] Part 5: OTP verification and Basic Details (MVC and API controllers)
- [ ] Part 6: Personal and Company details
- [ ] Part 7: Bank, Loan, Document upload, Thank you
- [ ] Part 8: JWT login and role-protected Manager API
- [ ] Part 9: Remaining views
- [ ] Part 10: End-to-end testing and polish

## Running locally
1. Install .NET SDK and SQL Server (Express or Developer).
2. Clone the repo and open the solution in Visual Studio.
3. Store secrets with user-secrets (never commit them):
   - `SmtpSettings:SmtpPassword` (SMTP key)
   - `Jwt:Key` (random string, 32+ characters)
4. In Package Manager Console run `Update-Database` to create the PLAPP database.
5. Seed the master tables (PAN, Aadhaar, pincode, rules) with test rows.
6. Press F5.

## Part 1: Startup and middleware
`Program.cs` registers MVC, Serilog, the EF Core DbContext, the mail
service, JWT authentication and role policies, then defines the request
pipeline: HTTPS redirect, static files, Serilog request logging, custom
`RequestLoggingMiddleware`, routing, authentication, authorization.
Order matters: authentication must run before authorization.

## Part 2: Database layer
14 code-first entities: the loan application (`Basicdetail`) with child
records (personal, company, bank, loan, documents), master tables used
for validation (PAN, Aadhaar, pincode, company, document types, rules),
`Usermaster` for logins and `Otpmaster` for OTPs. Money and rate fields
use `decimal` to avoid floating-point rounding.

## Part 3: DTOs
Separate request/response shapes for each wizard stage, inheriting
shared fields (AppId, stage, status, message) from `RootDto`. DTOs keep
database entities out of the web layer and prevent over-posting.

## Part 4: Services
- `IMailService` / `MailService`: SMTP email through MailKit
- `LoanCalculatorService`: eligibility (tenure, rate, EMI) from rules
- `StageViewMapper`: maps an application's stage to the next page

## Part 5: OTP verification and Basic Details
Two controllers share the same logic:
- `BasicDetailsController` (MVC): the browser wizard
- `BasicDetailsAPIController` (JSON): `GenerateOtp`, `ValidateOtp`,
  `ValidateBasicDetails`, testable in Postman

Flow: email, then an OTP is created and emailed (stored in `Otpmaster`
with a 5-minute expiry), then verification creates the application
record, then PAN, Aadhaar, pincode and age are validated against master
tables and the details saved.

Design decisions and fixes:
- OTPs persisted with expiry instead of an in-memory Hashtable
  (survives restarts, expires properly)
- Age calculated from date of birth, with a corrected range check
- Null guards added so invalid input returns a clear error, not a crash

### Database changes in this part
None. Tables were created in Part 2.