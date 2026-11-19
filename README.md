# Nova Store

Nova Store is a .NET 10 ASP.NET Core MVC e-commerce demo arranged as a three-project solution:

- `NovaStore.Presentation` contains the MVC controllers, Razor views, static assets, configuration, session cart, and application startup.
- `NovaStore.BusinessLogic` contains the store service interface and implementation.
- `NovaStore.DataAccess` contains the product, category, account, cart, and order models plus the repository interface and JSON repository.

Dependency direction is `Presentation → BusinessLogic → DataAccess`. Open `NovaStore.slnx` or run `dotnet run --project NovaStore.Presentation`.

## Existing functionality

The storefront supports product browsing, search, category filtering, sorting, product details, cart updates, checkout, customer registration/login/profile, and order history. The admin area supports product CRUD, product details, category management, and order status updates. Admin routes use the existing configured password and session-based sign-in. Set `AdminPassword` through a secure environment setting for deployments.

Products, orders, and accounts retain their original JSON format in `App_Data` under the app output folder. Category records are stored separately in `categories.json`; startup derives them from existing product category strings, so existing products remain associated without a database conversion. When upgrading from the original project output layout, existing JSON files are copied forward if the new output has no corresponding file; the source files are left untouched. The cart remains in ASP.NET Core session.

There is no EF Core/relational database configured in this repository, no connection string, and no existing migrations. Consequently, this restructuring adds no database migration and does not create, drop, or recreate a database. JSON persistence is retained for compatibility; replacing it with a relational store would require a separately planned data migration.

## Run

Install the .NET 10 SDK, then run:

```powershell
dotnet restore NovaStore.slnx
dotnet build NovaStore.slnx
dotnet run --project NovaStore.Presentation
```

The default admin password is `ChangeMe123!` for this demonstration project. The checkout is cash-on-delivery demo behavior and does not process payments. Product images use Unsplash URLs.
