# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore
COPY ApplicationTracker.csproj ./
COPY ApplicationTracker.Tests/ApplicationTracker.Tests.csproj ApplicationTracker.Tests/
RUN dotnet restore ApplicationTracker.sln

# Copy everything else and publish
COPY . .
RUN dotnet publish ApplicationTracker.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

# Render injects PORT; ASP.NET must listen on it
ENV ASPNETCORE_URLS=http://0.0.0.0:10000

EXPOSE 10000

ENTRYPOINT ["dotnet", "ApplicationTracker.dll"]