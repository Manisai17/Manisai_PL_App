# Personal Loan Application (PL_APP_MVC)

An ASP.NET Core MVC loan application wizard with JWT authentication and
enforced role-based authorization for its API, cookie-based
authentication for its browser-rendered admin area, Serilog structured
logging, a custom request-logging middleware, and OTP email
verification — built from scratch as a learning + portfolio project.

**Status: in active development.**

## Tech stack
- ASP.NET Core MVC (.NET 10)
- Entity Framework Core (code-first) + SQL Server
- JWT Bearer authentication (API) + Cookie authentication (browser),
  both backed by the same Usermasters table and role-based policies
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
- [x] Part 7 — BankDetails, LoanDetails, DocUpload, ThankYou + all wizard views, verified end-to-end
- [x] Part 8 — JWT login, ManagerAPIController, enforced AdminPolicy authorization, verified in Postman
- [x] Part 9 — ManagerController + views, rejection/error views, real cookie-based authentication protecting the Manager browser area, verified end-to-end
- [ ] Part 10 — Final secrets cleanup, full end-to-end test pass, polish

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
5. Seed the master/reference tables (PAN, Aadhaar, pincode, company,
   business rules) — see sample data below.
6. Create an admin user in `Usermasters` with a BCrypt-hashed password
   to log in and test both JWT and cookie authentication.
7. Press F5.

### Sample seed data
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

---

## Part 1 — Application Startup & Custom Middleware
(unchanged — see previous commits)

---

## Part 2 — Database Models & PlappContext
(unchanged — see previous commits)

---

## Part 3 — DTOs (Data Transfer Objects)
(unchanged — see previous commits)

---

## Part 4 — Services
(unchanged — see previous commits)

---

## Part 5 — BasicDetails Flow & OTP Verification
(unchanged — see previous commits)

---

## Part 6 — PersonalDetails & CompanyDetails
(unchanged — see previous commits)

---

## Part 7 — BankDetails, LoanDetails, DocUpload, ThankYou
(unchanged — see previous commits)

---

## Part 8 — JWT Login & Enforced Role Authorization
(unchanged — see previous commits)

---

## Part 9 — Completing the Views & Real Manager Authentication

### What this part does
Adds the browser-facing Manager review area and secures it with real
cookie-based authentication, running alongside the JWT authentication
built for the API in Part 8 — two separate schemes, each protecting
its own part of the app, both backed by the same Usermasters table.

### Files added/changed
- `Program.cs` — `AddAuthentication` now registers both JWT Bearer
  (default scheme) and a named `"Cookies"` scheme with a `LoginPath`
  that auto-redirects unauthenticated visitors
- `Controllers/HomeController.cs` — `LoginCheck` verifies real
  BCrypt-hashed credentials from `Usermasters` and calls
  `HttpContext.SignInAsync("Cookies", principal)` to issue a real
  authentication cookie; added a `Logout` action using `SignOutAsync`
- `Controllers/ManagerController.cs` — marked
  `[Authorize(AuthenticationSchemes = "Cookies", Policy = "AdminPolicy")]`
  at the class level; `Index` lists pending applications,
  `ViewApplication` shows one application, `UpdateLoanStatus` approves
  it (same EMI/ROI calculation as `ManagerAPIController.ApproveLoan`)
- `Views/Manager/Index.cshtml`, `Views/Manager/ViewApplication.cshtml`
- `Views/Home/Login.cshtml` (was missing, restored)
- `Views/Shared/Rejected/Thankyou.cshtml` — rejection confirmation page
- `Views/Shared/Error.cshtml` — cleaned up from the default template
- `_Layout.cshtml` — added a Logout link in the nav bar

### How the cookie protection actually works
An unauthenticated request to any `/Manager/*` route is automatically
redirected to `/Home/Login` by the cookie scheme's configured
`LoginPath` — no manual redirect code required anywhere. After a
successful login, `SignInAsync` issues an encrypted cookie containing
the user's role claim; the browser sends it automatically on every
subsequent request; `UseAuthorization` checks that claim against
`AdminPolicy` before any `ManagerController` action runs.

### Why two authentication schemes instead of one
JWT bearer tokens are carried manually by the client on every request
(an `Authorization` header) — ideal for Postman/JavaScript API calls,
but browsers don't attach custom headers automatically on normal page
navigation. Cookie authentication is the standard ASP.NET Core
approach for browser-rendered pages: the browser sends the cookie
automatically once issued. Both schemes are registered together in
`Program.cs`, and each controller specifies which one it expects.

### Verified end-to-end
- Visiting `/Manager/Index` with no prior login redirects to
  `/Home/Login` automatically (tested in an incognito window)
- Logging in with the real admin account (BCrypt-verified) grants
  access to the Manager area
- Logout clears the cookie; `/Manager/Index` redirects to login again
  afterward, confirming the session genuinely ended
- An application failing income/obligation rules correctly shows the
  rejection page instead of crashing

### Database changes needed this part
None. Reuses the same `Usermasters` table and admin account seeded in
Part 8.