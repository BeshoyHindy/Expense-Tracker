# Expense Tracker API

A technical assessment implementation of an expense tracking API using .NET 9.

## Project Overview

This API allows users to:
- Record daily expenses with amount, category, date, and description
- View their expenses
- Edit expense amounts and categories

## Technical Details

Built using:
- .NET 9
- SQL Server 2022
- Entity Framework Core
- CQRS pattern with MediatR
- Clean Architecture principles
- Docker for containerization

## Running the Project

### Docker Setup
1. Make sure Docker Desktop is running
2. Run the application:
```bash
docker-compose up --build
```

This will:
- Start the API at `http://localhost:5001`
- Start SQL Server at port `1433`
- Set up the database `ExpenseTrackerDb`

### Connection Details
SQL Server connection details when running with Docker:
- Server: localhost,1433
- Database: ExpenseTrackerDb
- User: sa
- Password: YourStrong@Passw0rd

## Project Structure

```
src/
  ├── ExpenseTracker.Api/          # API Controllers & Configuration
  ├── ExpenseTracker.Application/  # Business Logic & CQRS Commands
  ├── ExpenseTracker.Domain/       # Domain Models & Business Rules
  └── ExpenseTracker.Infrastructure/# Data Access & External Services
```

## Design Choices

- Clean Architecture for separation of concerns
- CQRS for better scalability and separation of read/write operations
- Domain Events for better extensibility
- Repository Pattern for data access abstraction
- Fluent Validation for request validation
- Global exception handling middleware

## API Endpoints

### Expenses
- `POST /api/expenses` - Create expense
- `GET /api/expenses/{id}` - Get expense
- `GET /api/expenses/user/{userId}` - List user expenses
- `PUT /api/expenses/{id}` - Update expense amount and category
- `DELETE /api/expenses/{id}` - Delete expense

### Example Request
```json
POST /api/expenses
{
    "amount": 25.50,
    "category": "Food",
    "description": "Lunch",
    "date": "2025-02-18T12:00:00Z",
    "userId": "user-id-here"
}
```
