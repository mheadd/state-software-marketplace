FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["state-software-marketplace.csproj", "."]
RUN dotnet restore "./state-software-marketplace.csproj"
COPY . .
RUN dotnet build "state-software-marketplace.csproj" -c Release -o /app/build
# Install dotnet-ef tool for migrations
RUN dotnet tool install --global dotnet-ef && \
    export PATH="$PATH:/root/.dotnet/tools" && \
    dotnet ef --version

FROM build AS publish
RUN dotnet publish "state-software-marketplace.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
# Create a non-root user and switch to it
RUN adduser --disabled-password --gecos '' appuser \
    && chown -R appuser /app
USER appuser
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "state-software-marketplace.dll"]
