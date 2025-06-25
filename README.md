# State Software Marketplace

[![.NET](https://github.com/mheadd/state-software-marketplace/actions/workflows/dotnet.yml/badge.svg)](https://github.com/mheadd/state-software-marketplace/actions/workflows/dotnet.yml)

[![CodeQL Vulnerability Scan](https://github.com/mheadd/state-software-marketplace/actions/workflows/dotnet-codeql.yml/badge.svg)](https://github.com/mheadd/state-software-marketplace/actions/workflows/dotnet-codeql.yml)

This is a basic CRUD web application built with ASP.NET Core MVC and Entity Framework Core, using a containerized PostgreSQL backend. The app is containerized with Docker and orchestrated with docker-compose.

<kbd>
  <img src="screenshot.png">
</kbd>

## Features
- General users can search a directory of approved software products.
- Admin users can add, modify, or delete entries (no authentication for now).
- Clean, simple UI for easy customization.

## To Do
- Need to add [authentication for admin users](https://github.com/mheadd/state-software-marketplace/issues/5) to add added or update new entries
- [Need robust search](https://github.com/mheadd/state-software-marketplace/issues/6) to make it easy for agencies to find authorized software tools

## Alternate approach

Absent compelling reasons for a custom built .NET application, agencies would probably be well served by using something like Drupal or WordPress, or something similar. These tools are open source, well documented, widely used, and battle tested.

## Getting Started

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) or [Colima](https://github.com/abiosoft/colima)

### Initial Setup
1. Clone the repository and navigate to the project directory.

2. Ensure Docker is running and no services are using ports 8080 or 5432.

3. Clean up any existing containers (if needed):
   ```bash
   docker-compose down -v
   ```

### Running the App (Recommended: Docker Compose)
1. Ensure all containers and volumes are cleaned up:
   ```bash
   docker-compose down -v
   ```

2. Build and start the containers:
   ```bash
   docker-compose up --build
   ```
   This will:
   - Build the .NET application
   - Start a PostgreSQL database
   - Run EF Core migrations
   - Start the web application

3. Wait for all services to start:
   - The database needs a few seconds to initialize
   - The migration service will run and set up the database schema
   - The web application will start last

4. Access the application:
   - Once all services are running, visit http://localhost:8080
   - You should see the software products directory

## Troubleshooting

### Common Startup Issues

If you encounter issues during startup, follow these steps:

1. **Check Container Status**
   ```bash
   docker ps
   ```
   Ensure that both `app` and `db` containers are running.

2. **Database Connection Issues**
   If you see database connection errors:
   - Verify the connection string:
     ```
     Host=db;Database=SoftwareDirectory;Username=postgres;Password=Your_password123;TrustServerCertificate=True
     ```
   - Ensure database container is healthy:
     ```bash
     docker-compose ps
     docker-compose logs db
     ```
   - The database might need a moment to initialize

3. **Migration Issues**
   If migrations aren't applying correctly:
   - Check migration logs:
     ```bash
     docker-compose logs migrate
     ```
   - Run migrations manually:
     ```bash
     docker-compose run --rm migrate
     ```

4. **DbContext Errors**
   If you see "Unable to create a 'DbContext' of type 'RuntimeType'...":
   ```bash
   # Stop everything and start fresh
   docker-compose down -v
   
   # Start database first
   docker-compose up db -d
   
   # Run migrations separately
   docker-compose run --rm migrate
   
   # Finally, start the app
   docker-compose up app
   ```

### Checking Logs
For detailed troubleshooting, check service logs:
```bash
# Application logs
docker logs state-software-marketplace-app-1 --tail 50

# Database logs
docker logs state-software-marketplace-db-1 --tail 50

# Migration service logs
docker logs state-software-marketplace-migrate-1 --tail 50
```

### Quick Reference
- **Start fresh**: `docker-compose down -v && docker-compose up --build`
- **Re-run migrations**: `docker-compose run --rm migrate`
- **Stop everything**: `docker-compose down -v`
- **Debug mode**: `docker-compose up --build --log-level DEBUG`
