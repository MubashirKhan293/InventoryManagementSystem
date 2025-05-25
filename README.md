# Inventory Management System

Angular + .NET 8 + SQL Server inventory system with Products, Sales, and Purchases modules.

## Prerequisites

- .NET 8 SDK
- Node.js (v16+)
- SQL Server
- Git

## Setup

### 1. Clone Repository
```bash
git clone [repo-url]
cd InventoryManagement
```

### 2. Backend Setup
```bash
cd InventoryManagement.Server
dotnet restore
dotnet ef database update
dotnet run
```
**Runs on:** https://localhost:7045

### 3. Frontend Setup
```bash
cd InventoryManagement.Client
npm install
ng serve
```
**Runs on:** http://localhost:4200

## Configuration

Update `appsettings.json` connection string if needed:

> Copy the server name from SQL Management Studio and replace it here

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.\\SQLEXPRESS;Database=InventoryManagementDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
}
```

## Features

✅ Products CRUD  
✅ Sales Management (Bonus)  
✅ Purchase Tracking (Bonus)  
✅ Inventory Updates  
✅ Responsive UI  

> **Note:** Both services must be running to use the application.
