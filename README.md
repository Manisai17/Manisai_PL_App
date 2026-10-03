# Personal Loan Application (PL_APP_MVC)

An ASP.NET Core MVC loan application wizard with JWT authentication and
enforced role-based authorization, Serilog structured logging, a custom
request-logging middleware, and OTP email verification — built from
scratch as a learning + portfolio project.

**Status: in active development.**

## Tech stack
- ASP.NET Core MVC (.NET 10)
- Entity Framework Core (code-first) + SQL Server
- JWT Bearer authentication with role-based authorization policies
- Serilog (structured logging, console + file sinks)
- MailKit (SMTP email via Brevo)
- BCrypt (password hashing)
- Custom ASP.NET Core middleware

## Progress
- [x] Part 1 — Program.cs wiring + custom RequestLoggingMiddleware
- [x] Part 2 — EF Core models + PlappContext (database layer complete)
- [x] Part 3 — DTOs (RootDto + 6 stage-specific DTOs)
- [x] Part 4 — Services (Mail/OTP, loan calculator, stage view mapper)
- [x] Part 5 — HomeController, BasicDetailsController/API + real OTP flow
- [x] Part 6 — PersonalDetails, CompanyDetails (MVC + API), business-rule rejection
- [x] Part 7 — BankDetails, LoanDetails, DocUpload, ThankYou + all views — full wizard verified working end-to-end
- [ ] Part 8 — JWT login, ManagerAPIController, enforced role authorization
- [ ] Part 9 — Any remaining view polish
- [ ] Part 10 — End-to-end testing, secrets cleanup, final polish

## Actual wizard stage order
Basic Details → Company Details → Loan Details → Personal Details →
Bank Details → Document Upload → Thank You

## Running locally
1. Install .NET SDK and SQL Server (Express or Developer).
2. Clone the repo and open the solution in Visual Studio.
3. Store secrets with User Secrets (right-click project → Manage User
   Secrets) — never commit these:
   - `SmtpSettings:SmtpPassword` (Brevo SMTP key)
   - `Jwt:Key` (random 32+ character string)
4. In Package Manager Console run `Update-Database` to create the
   PLAPP database from migrations.
5. Seed the master/reference tables before testing the wizard:
```sql
   INSERT INTO Panmasters (Pancardnumber, Firstname, Middlename, Lastname, Fathername, Dob)
   VALUES ('ABCDE1234F', 'Test', NULL, 'User', 'Test Father', '1999-05-15');

   INSERT INTO Aadharmasters (Aadharnumber, Firstname, Middlename, Lastname, Fathername, Dob, Address, Gender)
   VALUES ('123456789012', 'Test', NULL, 'User', 'Test Father', '1999-05-15', 'Hyderabad', 1);

   INSERT INTO Pincodemasters (Pincode, Circle, Village, District, State, Servicable)
   VALUES (500001, 'Hyderabad', 'Abids', 'Hyderabad', 'Telangana', 1);

   INSERT INTO Companymasters (Companyname, Category)
   VALUES ('Infosys', 'CAT A'), ('TCS', 'CAT A'), ('Genpact', 'CAT B');

   INSERT INTO Rulesmasters (Rulename, Minvalue, Maxvalue)
   VALUES ('age', 21, 60), ('income', 15000, 500000), ('oblpercent', 0, 50);
```
6. Press F5. Use a fresh email address (e.g. `you+test1@gmail.com`) per
   test run of the full wizard, since resuming an existing application
   skips straight to its current stage.

---

## Part 1 — Application Startup & Custom Middleware
(unchanged — see previous commits)

---

## Part 2 — Database Models & PlappContext
(unchanged — see previous commits)

---

## Part 3 — DTOs (Data Transfer Objects)
(unchanged — see previous commits)

### Note: Views require their own namespace import
`@model SomeDto` in a `.cshtml` file only resolves if
`Views/_ViewImports.cshtml` includes `@using Manisai_PL_App.Dtos`.
This was initially missing, causing `BankDetailsDto`/`LoanDetailsDto`
"could not be found" errors that looked like missing files but were
actually a missing import. `_ViewImports.cshtml` now reads:
```cshtml
@using Manisai_PL_App
@using Manisai_PL_App.Models
@using Manisai_PL_App.Dtos
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
```

---

## Part 4 — Services
(unchanged — see previous commits)

### Hardening added
`LoanCalculatorService.CalculateLoanDetails` now throws a clear
`InvalidOperationException` if the `age` rule is missing from
`Rulesmasters`, instead of throwing an unhandled `NullReferenceException`
that silently hangs the calling request.

---

## Part 5 — BasicDetails Flow & OTP Verification
(unchanged — see previous commits)

### Bug found and fixed during full end-to-end testing
Resuming an in-progress application (an existing `OTP_VERIFIED`
application verifying OTP again) redirected to `BasicDetails/BasicDetails/{id}`,
an action that didn't exist — `BasicDetails.cshtml` had only ever been
shown directly from inside `ValidateOtp`, never as its own reachable
action. Added:
```csharp
public IActionResult BasicDetails(int id)
{
    var basicDetails = _context.Basicdetails.FirstOrDefault(b => b.Id == id);
    if (basicDetails == null) return NotFound();

    ViewBag.Emailid = basicDetails.Emailid;
    return View("BasicDetails");
}
```

---

## Part 6 — PersonalDetails & CompanyDetails
(unchanged — see previous commits)

### Bug found and fixed during full end-to-end testing
`rulesIncome`/`rulesObligation` lookups in `CompanyDetailsController`
and `CompanyDetailsAPIController` had no null guard — an empty
`Rulesmasters` table caused an unhandled `NullReferenceException` that
silently hung the request (looked like a timeout in Postman, since
Visual Studio's debugger pauses on unhandled exceptions with no
response ever sent). Fixed with an explicit check:
```csharp
if (rulesIncome == null || rulesObligation == null)
{
    return StatusCode(500, "Business rules not configured — missing 'income' or 'oblpercent' in Rulesmasters");
}
```

---

## Part 7 — BankDetails, LoanDetails, DocUpload, ThankYou

### What this part does
Completes the core applicant-facing wizard. Bank Details follows the
same create-or-update pattern as earlier stages. Loan Details
calculates age correctly from date of birth and uses
`LoanCalculatorService` to compute eligible loan amount, tenure,
interest rate and EMI. Document Upload handles real file uploads to
disk. Thank You renders the final confirmation page.

### Controllers included
- `BankDetailsController`
- `LoanDetailsController` — correct age calculation (rough subtraction
  + one-year correction based on whether the birthday has passed this
  year), wraps `LoanCalculatorService` in try/catch
- `DocUploadController` — real file uploads via `IFormFile`, saved
  under `IWebHostEnvironment.ContentRootPath` (fixed from a hardcoded,
  machine-specific path in the original)
- `ThankYouController`

### Views added
`Views/BankDetails/Index.cshtml`, `Views/LoanDetails/Index.cshtml`,
`Views/CompanyDetails/Index.cshtml`, `Views/PersonalDetails/Index.cshtml`,
`Views/DocUpload/Index.cshtml` + `AddNewDocument.cshtml`,
`Views/ThankYou/Index.cshtml`, and a corrected `Views/Shared/_Layout.cshtml`
(the default Visual Studio template layout was initially still active
instead of the custom one, which hid TempData error messages from the
user during form validation failures).

### Known gap (not fixed, noted for later)
`BankDetailsController`'s manual validation checks every field except
`Accnumber` — a missing account number currently saves as null rather
than being rejected.

### Database changes needed this part
None. `Bankdetails`, `Loandetails`, `Docuploaddetails` tables already
exist from Part 2.

### .gitignore addition