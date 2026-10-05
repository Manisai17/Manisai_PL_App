# Personal Loan Application (PL_APP_MVC)

A full-stack ASP.NET Core MVC loan application wizard, built from
scratch as a learning and portfolio project. Features a complete
applicant-facing multi-step wizard with OTP email verification and
business-rule-driven eligibility (identity, geography, income), plus a
Manager review area secured with real cookie-based authentication, and
a parallel JWT-secured REST API with enforced, policy-based role
authorization.

**Status: feature-complete.**

## Tech stack
- ASP.NET Core MVC (.NET 10)
- Entity Framework Core (code-first) + SQL Server
- JWT Bearer authentication (API) + Cookie authentication (browser),
  both backed by the same `Usermasters` table and role-based policies
- Serilog (structured logging, console + rolling file sinks)
- MailKit (SMTP email via Brevo)
- BCrypt (password hashing)
- Custom ASP.NET Core middleware (request/response logging)
- Bootstrap (UI)

## Features
- Multi-step loan application wizard: Basic Details → Company Details
  → Loan Details → Personal Details → Bank Details → Document Upload
  → Thank You
- OTP email verification with persisted, expiring codes (not
  in-memory — survives app restarts, enforces a 5-minute expiry)
- Identity validation: PAN and Aadhaar checked against reference
  master tables before an application can proceed
- Geographic serviceability check: pincodes are validated against a
  whitelist/blacklist (`Pincodemaster.Servicable`) — applications from
  non-serviceable areas are rejected with a distinct, honest message
  separate from "invalid pincode"
- Configurable business rules (age, income, obligation percentage)
  driving automatic application rejection
- Company category lookup against a reference table, with an "Others"
  fallback for unlisted employers
- Dynamic loan eligibility calculation (tenure, interest rate, EMI)
  based on age and income rules
- Document upload with file storage
- JWT-authenticated REST API with enforced role-based authorization
  (`[Authorize(Policy = "AdminPolicy")]`) protecting loan approval
  endpoints — verified to return 401 without a valid token
- Cookie-authenticated browser Manager area with automatic
  redirect-to-login for unauthenticated access
- Structured logging via Serilog plus a custom
  `RequestLoggingMiddleware` logging every request's method, path,
  status code, and duration

## Architecture notes
- **Code-first EF Core**: the database schema is generated entirely
  from C# model classes via migrations; the database is never edited
  by hand.
- **DTOs separate from Models**: request/response shapes are distinct
  from database entities, preventing over-posting and decoupling the
  API contract from the schema.
- **Two parallel authentication schemes**: JWT for the API (manually
  attached by the client on every request) and cookies for the
  browser (attached automatically by the browser after login) — each
  protecting its own surface, registered together in `Program.cs`.
- **Explicit decimal precision** on money/rate columns
  (`Loandetail.Roi`, `Emi`, `Approvedroi`, `Approvedemi`) to prevent
  silent truncation.
- **`bigint`/`long` for account numbers**, not `int` — real bank
  account numbers routinely exceed `int`'s ~10-digit range.

## Running locally
1. Install the .NET SDK and SQL Server (Express or Developer).
2. Clone the repo and open the solution in Visual Studio.
3. Right-click the project → **Manage User Secrets** and add:
```json
   {
     "SmtpSettings": { "SmtpPassword": "your-brevo-smtp-key" },
     "Jwt": { "Key": "a-random-32-plus-character-string" }
   }
```
4. In Package Manager Console, run `Update-Database` to create the
   `PLAPP` database from the included migrations.
5. Seed the master/reference tables:
```sql
   INSERT INTO Panmasters (Pancardnumber, Firstname, Middlename, Lastname, Fathername, Dob)
   VALUES ('ABCDE1234F', 'Test', NULL, 'User', 'Test Father', '1999-05-15');

   INSERT INTO Aadharmasters (Aadharnumber, Firstname, Middlename, Lastname, Fathername, Dob, Address, Gender)
   VALUES ('123456789012', 'Test', NULL, 'User', 'Test Father', '1999-05-15', 'Hyderabad', 1);

   INSERT INTO Companymasters (Companyname, Category) VALUES
   ('Infosys', 'CAT A'), ('TCS', 'CAT A'), ('Wipro', 'CAT A'),
   ('HCL Technologies', 'CAT A'), ('Accenture', 'CAT A'),
   ('Genpact', 'CAT B'), ('Capgemini', 'CAT B'), ('Cognizant', 'CAT B'),
   ('Tech Mahindra', 'CAT B'), ('IBM India', 'CAT B'),
   ('Startup Solutions Pvt Ltd', 'CAT C'), ('Local Traders Co', 'CAT C');

   INSERT INTO Pincodemasters (Pincode, Circle, Village, District, State, Servicable) VALUES
   (500001, 'Hyderabad', 'Abids', 'Hyderabad', 'Telangana', 1),
   (500032, 'Hyderabad', 'Gachibowli', 'Hyderabad', 'Telangana', 1),
   (500081, 'Hyderabad', 'Madhapur', 'Hyderabad', 'Telangana', 1),
   (400001, 'Mumbai', 'Fort', 'Mumbai', 'Maharashtra', 1),
   (400051, 'Mumbai', 'Bandra', 'Mumbai', 'Maharashtra', 1),
   (560001, 'Bangalore', 'MG Road', 'Bangalore', 'Karnataka', 1),
   (560103, 'Bangalore', 'Bellandur', 'Bangalore', 'Karnataka', 1),
   (110001, 'Delhi', 'Connaught Place', 'New Delhi', 'Delhi', 1),
   (700001, 'Kolkata', 'BBD Bagh', 'Kolkata', 'West Bengal', 0),
   (781001, 'Guwahati', 'Pan Bazaar', 'Guwahati', 'Assam', 0);

   INSERT INTO Rulesmasters (Rulename, Minvalue, Maxvalue)
   VALUES ('age', 21, 60), ('income', 15000, 500000), ('oblpercent', 0, 50);
```
6. Seed one admin user: temporarily add a one-time seeding block to
   `Program.cs` using `BCrypt.Net.BCrypt.HashPassword(...)` against
   `Usermasters`, run once, then remove it.
7. Press F5.

## Testing
- **Applicant wizard**: walk through `/BasicDetails` start to finish
  with a fresh email address. Try pincode `700001` or `781001` to
  confirm the serviceability rejection; try an unlisted PAN/Aadhaar to
  confirm identity validation.
- **Manager (browser, cookie auth)**: visit `/Manager/Index` while
  logged out — confirm redirect to `/Home/Login`; log in with the
  seeded admin account; confirm access is granted; log out and confirm
  the session is genuinely cleared.
- **Manager API (JWT)**: `POST /api/Token/Login` with admin
  credentials to get a token; call `GET /api/ManagerAPI/PendingApplications`
  with no token (expect 401) and with a Bearer token (expect 200).

## Known limitations
- Business rules, reference data, and the first admin user must be
  seeded manually — no automated seed script is included yet.
- No automated tests (unit/integration) — all testing has been manual
  and documented above.
- Document uploads are stored on local disk, not cloud storage.
- Company category is looked up and stored but does not currently
  influence interest rate pricing — a reasonable future enhancement,
  deliberately scoped out of this build.

## Project structure
```
Controllers/   - MVC and API controllers for every wizard stage, auth, and admin
Models/        - EF Core entities + PlappContext
Dtos/          - Request/response shapes, separate from entities
Services/      - Mail, loan calculation, stage routing
Middleware/    - Custom request logging middleware
Views/         - Razor views for the applicant wizard and Manager area
Migrations/    - EF Core code-first migrations
```