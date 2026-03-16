# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files first
COPY ApplicationTracker.sln ./
COPY ApplicationTracker.csproj ./
COPY ApplicationTracker.Tests/ApplicationTracker.Tests.csproj ApplicationTracker.Tests/

# Restore dependencies
RUN dotnet restore ApplicationTracker.sln

# Copy everything else
COPY . .

# Publish the API
RUN dotnet publish ApplicationTracker.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://0.0.0.0:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "ApplicationTracker.dll"]