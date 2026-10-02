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
- [ ] Part 7 — BankDetails, LoanDetails, DocUpload, ThankYou
- [ ] Part 8 — JWT login, ManagerAPIController, enforced role authorization
- [ ] Part 9 — Views
- [ ] Part 10 — End-to-end testing, secrets cleanup, final polish

## Actual wizard stage order
Basic Details → Company Details → Loan Details → Personal Details →
Bank Details → Document Upload → Thank You
(Driven entirely by `StageViewMapper`'s lookup table — not necessarily
the order these controllers were built in.)

---

## Part 1 — Application Startup & Custom Middleware
(unchanged — see previous commit)

---

## Part 2 — Database Models & PlappContext
(unchanged — see previous commit)

---

## Part 3 — DTOs (Data Transfer Objects)
(unchanged — see previous commit)

---

## Part 4 — Services
(unchanged — see previous commit)

---

## Part 5 — BasicDetails Flow & OTP Verification
(unchanged — see previous commit)

---

## Part 6 — PersonalDetails & CompanyDetails

### What this part does
Two more wizard stages, following the same pattern as Part 5: look up
an existing child record (or create one), save submitted data, advance
the application's stage, redirect to the next stage via
`StageViewMapper`. Company Details additionally runs business-rule
validation (income and obligation checks) and can reject the
application outright.

### Controllers included
- `PersonalDetailsController` — father/mother names, addresses, two
  references
- `CompanyDetailsController` — company info, income, obligations;
  derives `Category` from `Companymasters`; rejects applications that
  fail income or obligation rules from `Rulesmasters`
- `CompanyDetailsAPIController` — JSON equivalent of the above

### Key patterns introduced
- Null-coalescing (`??`) to satisfy non-nullable Model fields from
  nullable Dto fields
- Null-conditional (`?.`) combined with `??` to safely derive a value
  from a lookup that might not find a match
- Existing-vs-new record handling (`isExisting`/`isNew` flags) so
  resubmitting a stage updates rather than duplicates
- `StandardMessages` and `StageViewMapper` used for the first time

### Bug fixed
Division by zero when calculating obligation percentage if gross
income is submitted as 0 — guarded with a `> 0` check before dividing.

### Database changes needed this part
None. `Personaldetails` and `Companydetails` tables already exist from
Part 2.