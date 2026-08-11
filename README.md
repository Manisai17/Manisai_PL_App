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
- [ ] Part 2 — EF Core models + PlappContext
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

### What this part does
This is the entry point of the entire application. `Program.cs` is the
first code that runs when the app starts — it configures every service
the app needs (MVC, database, authentication, logging) and defines the
pipeline every HTTP request passes through before reaching a controller.

### Why it's structured this way
ASP.NET Core uses a builder pattern: first you register everything the
app might need (`builder.Services.Add...`), then you `Build()` the app,
then you configure the order in which incoming requests get processed
(`app.Use...`).

### Key pieces and their purpose
- **AddControllersWithViews()** — enables the MVC pattern: controllers
  that return HTML views.
- **UseSerilog(...)** — structured logging (named fields, not just text),
  searchable and filterable later.
- **AddDbContext<PlappContext>** — registers the database connection via
  dependency injection.
- **AddJwtBearer(...)** — configures JWT validation: issuer, audience,
  expiry, and signature checks. Not yet used (Part 8).
- **AddAuthorization policies** — AdminPolicy/UserPolicy define what role
  a token needs for certain endpoints. Not yet enforced anywhere (Part 8).
- **Middleware pipeline order** — HTTPS redirect → static files →
  Serilog request logging → custom RequestLoggingMiddleware → routing →
  authentication → authorization → controller action. Order matters:
  each step depends on information the previous step established.

### Custom Middleware — RequestLoggingMiddleware.cs
Wraps every request, logging when it started, finished, how long it
took, and the response status code. Written by hand to demonstrate real
understanding of the RequestDelegate pipeline pattern, rather than
relying only on Serilog's built-in request logging.

### Database changes needed this part
None. No models exist yet.