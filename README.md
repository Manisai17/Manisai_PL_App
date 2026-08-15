# Personal Loan Application (PL_APP_MVC)

An ASP.NET Core MVC loan application wizard with JWT authentication and
enforced role-based authorization, Serilog structured logging, a custom
request-logging middleware, and OTP email verification — built from
scratch as a learning + portfolio project.

## Tech stack
- ASP.NET Core MVC (.NET 10)
- Entity Framework Core (code-first) + SQL Server
- JWT Bearer authentication with role-based authorization policies
- Serilog (structured logging, console + file sinks)
- MailKit (SMTP email via Brevo)
- Custom ASP.NET Core middleware

## Progress
- [x] Part 1 — Program.cs wiring + custom RequestLoggingMiddleware
- [x] Part 2 — EF Core models + PlappContext (database layer complete)
- [x] Part 3 — DTOs (RootDto + 6 stage-specific DTOs)
- [ ] Part 4 — Services (Mail/OTP, loan calculator)
- [ ] Part 5 — BasicDetails flow + OTP verification
- [ ] Part 6 — PersonalDetails, CompanyDetails
- [ ] Part 7 — BankDetails, LoanDetails, DocUpload, ThankYou
- [ ] Part 8 — JWT login, ManagerController, role enforcement
- [ ] Part 9 — Views
- [ ] Part 10 — End-to-end testing, secrets cleanup, final polish

---

## Part 1 — Application Startup & Custom Middleware
(unchanged — see previous commit)

---

## Part 2 — Database Models & PlappContext
(unchanged — see previous commit)

---

## Part 3 — DTOs (Data Transfer Objects)

### What this part does
Defines the data shapes that travel between the browser and the server
for each step of the loan wizard — deliberately separate from the EF
Core Models used for the database.

### DTOs included
- `RootDto` — base class carrying AppId, Name, AppStage, AppStatus,
  Message — fields every wizard stage needs, marked `[ValidateNever]`
  since they're populated by the server on the way out, not submitted
  by the user
- `BasicDetailsDto`, `PersonalDetailsDto`, `CompanyDetailsDto`,
  `BankDetailsDto`, `LoanDetailsDto` — one per wizard stage, each
  inheriting from `RootDto`
- `OtpDto` — request/response shape for OTP generation and validation

### Why DTOs are separate from Models
- Field names can differ from database columns to match what a form
  actually sends (e.g. `MobileNumber` vs the Model's `Mobile`)
- Avoids exposing database-only concerns (Ids, navigation properties)
  directly to incoming requests
- Prevents over-posting — a request can only set the fields explicitly
  listed on the DTO, nothing else

### Known inconsistencies carried from the original design (to fix later)
- `BasicDetailsDto.Dob` is `DateTime`; the Model correctly uses
  `DateOnly` — needs explicit conversion when mapping between them
- `LoanDetailsDto` uses `double` for money/interest fields; the Model
  correctly uses `decimal` — same conversion care needed

### Database changes needed this part
None — DTOs are not persisted; no migration required.