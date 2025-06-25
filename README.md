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
- Need to add authentication for admin users to add added or update new entries
- Need robust search to make it easy for agencies to find authorized software tools

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

### Troubleshooting
- If you see database connection errors:
  - Ensure the database container is healthy (`docker-compose ps`)
  - Check logs with `docker-compose logs db`
  - The database might need a moment to initialize
- If migrations fail:
  - Check the migrate service logs: `docker-compose logs migrate`
  - You can see more detailed logs by running migrations manually:
    ```bash
    docker-compose run --rm migrate
    ```
   - Launch the web application

2. Wait for all services to start:
   - The database needs a few seconds to initialize
   - The migration service will run first
   - The web app will start after migrations complete

3. Access the app at [http://localhost:8080](http://localhost:8080)

### Common Startup Issues

If you see the error: "Unable to create a 'DbContext' of type 'RuntimeType'..." during startup:

1. Stop all containers:
   ```bash
   docker-compose down -v
   ```

2. Rebuild and start with verbose output:
   ```bash
   docker-compose up --build --log-level DEBUG
   ```

3. If the error persists, try running migrations separately:
   ```bash
   # First, ensure the database is ready
   docker-compose up db -d
   
   # Then run migrations
   docker-compose run --rm migrate
   
   # Finally, start the app
   docker-compose up app
   ```

### Running Database Migrations
If you need to re-run migrations (e.g., after updating models):
```bash
docker-compose run --rm migrate
```

### Stopping the App
To stop and remove containers, networks, and volumes:
```bash
docker-compose down -v
```

## Troubleshooting

### Database Connection Issues
If you see database connection errors or missing tables:

1. **Check Container Status**
   ```bash
   docker ps
   ```
   Ensure that the `app` and `db` containers are both running.

2. **Verify Database Connection**
   The app uses this connection string format:
   ```
   Host=db;Database=SoftwareDirectory;Username=postgres;Password=Your_password123;TrustServerCertificate=True
   ```
   Make sure:
   - The database container name matches the host ('db')
   - The credentials match those in docker-compose.yml
   - The database name is correct

3. **Check Application Logs**
   ```bash
   docker logs state-software-marketplace-app-1 --tail 50
   ```

4. **Check Database Logs**
   ```bash
   docker logs state-software-marketplace-db-1 --tail 50
   ```

5. **Check Migration Service Logs**
   ```bash
   docker logs state-software-marketplace-migrate-1 --tail 50
   ```

### Fixing Database Schema Issues
If tables are missing or migrations haven't been applied:

1. **Remove existing containers and volumes**
   ```bash
   docker-compose down -v
   ```

2. **Start fresh with verbose logging**
   ```bash
   docker-compose up --build --log-level DEBUG
   ```

3. **Run migrations manually if needed**
   ```bash
   docker-compose run --rm migrate
   ```

If issues persist, you can:
- Check the application's logs for specific error messages
- Verify that the database is accepting connections
- Ensure no other services are using the required ports
- Review the Entity Framework Core migrations in the Migrations folder
