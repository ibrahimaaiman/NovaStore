# NOVA. — Modern ASP.NET Core MVC E-commerce Demo

A responsive, premium-looking e-commerce sample built with **ASP.NET Core MVC / .NET 10** and **no third-party NuGet dependencies**.

## Run in Visual Studio 2026 / .NET 10
1. Install the .NET 10 SDK and ASP.NET/web development workload.
2. Extract ZIP and open `NovaStore.csproj` in Visual Studio.
3. Run with F5, or in a terminal run `dotnet restore` then `dotnet run`.
4. Browse `http://localhost:5188` when using the included launch profile (or use the URL shown in your terminal).

## Features
- Responsive modern storefront, hero, promotional sections, curated product grid and remote Unsplash photography (requires internet; local placeholder on image errors).
- Product categories, keyword search, sorting, product details and related products.
- Session shopping bag: add, update quantities, remove, totals, free shipping at $200.
- Checkout with server-side validation and **cash-on-delivery demo** (no real payment processing); order confirmation.
- Password-protected demo admin at `/Admin/Login`: product add/edit/delete, stock and order status management, basic revenue summary.
- Products and orders persisted as JSON files in `App_Data` under the application output folder, initialized with 12 demo products on first run.

## Admin login
Default password: `ChangeMe123!`. **Change `AdminPassword` in `appsettings.json` before use** or override it via environment configuration. This is a demo login, not production identity/authentication.

## Important limitations
This is a functional portfolio/demo project, **not a production-ready shop**. It does not include real payment, email, tax, delivery integration, secure ASP.NET Identity, a relational database, image upload, multi-admin permissions, or hardened concurrency/transactional stock reservation. Product photos are loaded from Unsplash; internet access is required for those images. For production, use ASP.NET Core Identity, EF Core/SQL Server, proper payment provider, secrets management, logging and transactional stock handling. Prices are illustrative USD amounts.

## Structure
`Controllers/` MVC actions · `Models/` entities and checkout form · `Services/` JSON store and session cart · `Views/` Razor templates · `wwwroot/` styles, scripts, local fallback graphic.


## Customer accounts (added)
- Register: `/Account/Register`
- Log in: `/Account/Login`
- Profile: `/Account/Profile`
- Order history: `/Account/MyOrders`
- Log out is available in the header and profile page.

Customer accounts are stored in `App_Data/accounts.json`; passwords are stored as ASP.NET Core password hashes, not plain text. Customer login is a lightweight demo implementation using session state and JSON storage, not ASP.NET Core Identity/SQL Server. Use HTTPS and a persistent database/Identity for production. Orders appear under My Orders when the checkout email matches the signed-in account email.
