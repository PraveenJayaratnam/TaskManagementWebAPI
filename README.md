# Task Management Web API

A .NET 8 Web API for managing tasks and users.

## Prerequisites

- .NET 8 SDK
- SQL Server (LocalDB or full instance)

## Setup Instructions

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd TaskManagementWebAPI
   ```

2. **Restore packages**
   ```bash
   dotnet restore
   ```

3. **Update connection string** in `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TaskManagementDB;Trusted_Connection=true;MultipleActiveResultSets=true"
     }
   }
   ```

4. **Run the application**
   ```bash
   dotnet run --launch-profile https
   ```

## Database

The application uses Entity Framework Core with SQL Server. The database will be created automatically on first run.

### Database Migrations

Use the following commands to manage database migrations:

#### Install EF Core Tools (if not already installed)
```bash
dotnet tool install --global dotnet-ef
```

#### Create a new migration
```bash
dotnet ef migrations add <MigrationName>
```

#### Update database with latest migrations
```bash
dotnet ef database update
```

#### Remove the last migration (if not applied to database)
```bash
dotnet ef migrations remove
```

#### Generate SQL script for migrations
```bash
dotnet ef migrations script
```

#### List all migrations
```bash
dotnet ef migrations list
```


##### Using Visual Studio Package Manager Console
1. Open **Tools** → **NuGet Package Manager** → **Package Manager Console**
2. Run these commands:
```powershell
Add-Migration <MigrationName>
Update-Database
Remove-Migration
```

### Default User
A default admin user is seeded:
- userName: `admin`
- Password: `Admin123!`

### CORS
The API is configured to allow requests from `http://localhost:4200` (Angular development server).