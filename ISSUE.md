# Database Migration Issue in Docker Compose

## Summary
The application fails to apply Entity Framework Core migrations when running the `migrate` service in Docker Compose. This results in the database schema not being created and the app failing to start due to missing tables.

## Error Message
```
Unable to create a 'DbContext' of type 'RuntimeType'. The exception 'Unable to resolve service for type 'Microsoft.EntityFrameworkCore.DbContextOptions`1[state_software_marketplace.Data.ApplicationDbContext]' while attempting to activate 'state_software_marketplace.Data.ApplicationDbContext'.' was thrown while attempting to create an instance. For the different patterns supported at design time, see https://go.microsoft.com/fwlink/?linkid=851728
```

## Approaches Tried
1. **Verified Docker Compose and Dockerfile setup:**
   - Ensured migration files and source code are present in the build context and container.
   - Confirmed the `migrate` service uses the SDK image and correct working directory (`/src`).
2. **Added a design-time factory:**
   - Implemented `ApplicationDbContextFactory` to provide EF Core with a way to construct the context at design time.
3. **Checked connection string configuration:**
   - Added a `DefaultConnection` entry to `appsettings.json`.
   - Verified the `DB_CONNECTION_STRING` environment variable is set in the `migrate` service.
4. **Debugged inside the container:**
   - Confirmed that `ApplicationDbContextFactory.cs` is present in `/src/Data`.
   - Attempted to log the working directory and connection string in the factory (no log output seen).
   - Confirmed that the migration command is being executed in the correct directory.

## Current State
- The error persists: EF Core cannot resolve the `DbContextOptions<ApplicationDbContext>` at design time.
- No log output from the factory, suggesting it may not be used or not loaded by EF Core.
- All source files and configuration appear present in the container.

## Possible Next Steps
- Double-check that the assembly is being built as expected and that the factory is not excluded from the build.
- Try running the migration command with increased verbosity or diagnostic output.
- Test running migrations outside of Docker to confirm the issue is container-specific.
- Review EF Core and .NET SDK versions for compatibility issues.
- Seek help from the EF Core community or GitHub discussions, providing this summary and error details.
