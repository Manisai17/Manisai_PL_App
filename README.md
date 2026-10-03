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
- [x] Part 7 — BankDetails, LoanDetails, DocUpload, ThankYou + all views, full wizard verified end-to-end
- [x] Part 8 — JWT login, ManagerAPIController, enforced AdminPolicy authorization, verified in Postman
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
5. Seed the master/reference tables (see Part 7 section below for the
   exact SQL).
6. Create at least one admin user via `Usermasters` (BCrypt-hashed
   password) to test login/authorization — see Part 8 below.
7. Press F5.

---

## Parts 1-7
(unchanged — see previous commits)

---

## Part 8 — JWT Login & Enforced Role Authorization

### What this part does
Adds real authentication and authorization to the API layer. A user
logs in with a username/password; if valid, they receive a signed JWT
containing their role. Protected endpoints check that token and reject
anyone without a valid, correctly-signed token carrying the required
role — before any controller code runs.

### Files added
- `Dtos/LoginDto.cs` — username/password request shape
- `Controllers/TokenController.cs` — `POST api/Token/Login` verifies
  BCrypt-hashed credentials, issues a signed JWT with Id/Username/Role
  claims
- `Controllers/ManagerAPIController.cs` — `[Authorize(Policy =
  "AdminPolicy")]` at the class level; `GetPending` lists in-progress
  applications, `ApproveLoan` approves one, updates the loan record,
  and sends a confirmation email

### How enforcement actually works, step by step
1. Request arrives with `Authorization: Bearer <token>`
2. `UseAuthentication()` validates the token's signature, issuer,
   audience, and expiry
3. `UseAuthorization()` checks the token's Role claim against the
   policy required by the endpoint
4. Only if both pass does the controller action execute; otherwise the
   request is rejected with 401/403 before reaching any application
   code

### Security note
`TokenController.Login` returns an identical error message whether the
username doesn't exist or the password is wrong, to avoid revealing
which usernames are valid to someone probing the login endpoint.

### Verified in Postman
- No token → 401 on `/api/ManagerAPI/PendingApplications`
- Valid Admin token → 200 with application list
- `ApproveLoan` with valid token → loan record updated, confirmation
  email sent, confirmed directly via a SQL query showing
  `Approvedamount` changing from NULL to a real value

### Database changes needed this part
None. `Usermasters` table already existed from Part 2. One admin user
was seeded via a temporary one-time block in `Program.cs` (run once,
then removed).