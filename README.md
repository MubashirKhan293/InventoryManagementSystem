Inventory Management System
Angular + .NET 8 + SQL Server inventory system with Products, Sales, and Purchases modules.

Prerequisites
.NET 8 SDK
Node.js (v16+)
SQL Server
Git

Setup
1. Clone Repository
bashgit clone [repo-url]
cd InventoryManagement

3. Backend Setup
cd InventoryManagement.Server
dotnet restore
dotnet ef database update
dotnet run

Runs on: https://localhost:7045

5. Frontend Setup
cd InventoryManagement.Client
npm install
ng serve
Runs on: http://localhost:4200

Configuration
Update appsettings.json connection string if needed:
copy the server name from sql management studio and replace it here

"ConnectionStrings": {
  "DefaultConnection": "Server=.\\SQLEXPRESS;Database=InventoryManagementDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
}

Features
✅ Products CRUD
✅ Sales Management (Bonus)
✅ Purchase Tracking (Bonus)
✅ Inventory Updates
✅ Responsive UI

Both services must be running to use the application.
