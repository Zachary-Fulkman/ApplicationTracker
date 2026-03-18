## Live API

Swagger documentation:

https://applicationtracker-papa.onrender.com/swagger

# Application Tracker API

This is a backend API built with ASP.NET Core that allows users to manage job applications.  

I built this project to strengthen my backend development skills and to demonstrate clean architecture, integration testing, and CI automation using modern .NET practices.

---

## What This API Does

The Application Tracker allows you to:

- Create new job applications
- Update existing applications
- Delete applications
- Retrieve an application by ID
- Search and paginate through results

The API follows REST principles and returns appropriate HTTP status codes for all operations.

---

## Tech Stack

- **.NET 8**
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **SQLite** (development database)
- **EF Core InMemory Provider** (for tests)
- **xUnit**
- **Microsoft.AspNetCore.Mvc.Testing**
- **GitHub Actions (CI)**

---

## Project Structure

The project is organized with separation of concerns in mind:

- **Controllers** handle HTTP requests
- **Services** contain business logic
- **Data (DbContext)** handles database interaction
- **Dependency Injection** is used throughout the application

Integration tests boot the full application pipeline using `WebApplicationFactory`, which allows the API to be tested end-to-end without mocking the entire system.

---

## Testing Approach

Instead of only writing unit tests, this project focuses on **integration testing**.

Each test:

- Spins up the API in memory
- Uses an isolated EF Core InMemory database
- Sends real HTTP requests
- Verifies status codes and responses

This ensures the entire request pipeline works correctly — from controller to database.

All tests run automatically through GitHub Actions on every push and pull request.

## Testing CI/CD trigger
