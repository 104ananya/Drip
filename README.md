# Drip 💧

A personal spend tracker API that aggregates spending across Swiggy, Zomato, Flipkart, Amazon, office cafeteria, and grocery stores — with smart categorization, budget alerts, and spending insights.

## Tech Stack

- **C# / .NET 9 / ASP.NET Core Web API**
- **PostgreSQL** — database
- **Dapper** — raw SQL (Phase 1)
- **Entity Framework Core** — ORM (Phase 2)
- **JWT + BCrypt** — authentication
- **Swagger** — API documentation

## Getting Started

### Prerequisites

- [.NET SDK 9+](https://dotnet.microsoft.com/download)
- [PostgreSQL 17](https://www.postgresql.org/download/)
- [Git](https://git-scm.com/)

### Run Locally

```bash
cd Drip
dotnet run
```

Open [http://localhost:5062/swagger](http://localhost:5062/swagger) to explore the API.

## Features

- Expense CRUD with platform & category tracking
- Smart merchant-to-category mapping (Swiggy → Food Delivery)
- Filtering by platform, category, date, and amount
- Monthly summaries & platform split views
- Budget alerts & recurring spend detection
- Spending trends & insights
- CSV export

## Project Status

🚧 Under active development — Phase 1 (API Basics + Raw SQL)
