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
- [ ] Part 3 — DTOs
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

### What this part does
Defines every table in the loan application as a C# class (EF Core
code-first models), and PlappContext — the class representing the whole
database, wiring the relationships between tables together.

### Models included
- Basicdetail — the core application record
- Personaldetail, Companydetail, Bankdetail, Loandetail,
  Docuploaddetail — child records linked to a Basicdetail via foreign key
- Panmaster, Aadharmaster, Pincodemaster, Companymaster, Doctypemaster,
  Rulesmaster — lookup tables used to validate submitted data
- Usermaster — login credentials and role, for JWT authentication (Part 8)
- Otpmaster — persisted OTP codes with expiry (used in Part 4/5)
- StandardMessages — static applicant-facing status text

### Why code-first
C# model classes are the source of truth; the database schema is
generated from them via EF Core migrations, never edited by hand.

### Key design choices
- Nullable properties (`string?`, `int?`) — most fields are optional
  since applications are saved in a partial state through the wizard
- `decimal` for money/interest fields — avoids floating-point rounding
  errors; exact for financial calculations
- `virtual ICollection<T>` navigation properties — EF Core loads related
  rows without manual join queries
- Explicit `HasOne().WithMany().HasForeignKey()` — removes ambiguity
  about how tables relate

### Database changes made this part