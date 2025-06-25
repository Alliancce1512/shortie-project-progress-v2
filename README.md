# ShortieProgress.Api — .NET 9 URL Shortener Backend

This is the backend service for the Shortie URL Shortener.  
Built with **ASP.NET Core 9 Web API** and **Entity Framework Core**, it handles:

- URL shortening and redirecting
- Secret stats URLs
- Visit and daily unique logging
- Full SQL Server relational structure
- Live-tested via Swagger UI

---

## 🚀 Features

- Create short links with associated secret stats link
- Track visits and IPs
- Track daily unique visits (IP + date)
- API-ready structure for frontend consumption
- Integrated Swagger UI
- X-Forwarded-For IP header support (for proxies)
- Fully relational MS SQL structure

---

## ⚙️ Tech Stack

| Layer     | Technology                 |
|-----------|-----------------------------|
| Backend   | ASP.NET Core 9 Web API      |
| Database  | Microsoft SQL Server        |
| ORM       | Entity Framework Core 9     |
| Tooling   | dotnet CLI, EF Core Migrations |
| Scripts   | Bash for setup, SQL for seeding |

---

## ▶️ How to Run

### 1. Clone the Repo

```bash
git clone https://github.com/YourUsername/ShortieProgress.Api.git
cd ShortieProgress.Api
```

---

### 2. Restore & Build

```bash
dotnet restore
dotnet build
```

---

### 3. Setup the Database

You’ll need to have SQL Server running locally (or via Docker).

Then run:

```bash
chmod +x scripts/setup.sh
./scripts/setup.sh
```

This script will:
- Run EF Core migrations
- Seed test data (`Urls`, `Visits`, `DailyUniques`)

> The DB connection string is configured in `appsettings.json`.

---

### 4. Run the API

```bash
dotnet run --project ShortieProgress.Api
```

By default it runs on:  
`https://localhost:5044` (with Swagger UI available)

---

## 📚 API Overview

All routes start with `/api`.

### POST `/api/Shorten`

Create a short URL:

```json
{
  "longUrl": "https://example.com"
}
```

**Response:**

```json
{
  "status": 0,
  "status_message": "Command completed successfully",
  "shortUrl": "https://shortie.presiyangeorgiev.eu/r/abc123",
  "secretUrl": "https://shortie.presiyangeorgiev.eu/s/secretcode1234567890"
}
```

---

### GET `/r/{shortCode}`

Redirects the user to the long URL. Logs the visit and checks for daily unique.

---

### GET `/s/{secretCode}`

Redirects to frontend stats page:  
`http://localhost:3000/stats/{secretCode}`

---

### GET `/api/stats/{secretCode}`

Returns the stats JSON:

```json
{
  "dailyVisits": [
    { "date": "2025-06-25", "count": 2 }
  ],
  "topIps": [
    { "ip": "62.73.73.198", "count": 2 }
  ]
}
```

---

## 🗃️ Database Schema

- `Urls` — stores long URLs, short codes, and secret codes
- `Visits` — logs every visit with timestamp and IP
- `DailyUniques` — stores unique visits per day and IP

---

## 🧪 Test Data

Run `scripts/setup.sh` to create tables and seed test entries.

Test short code: `uqzsre`  
Stats code: `1ccd40a7f6be5c1da48d`

---

## ✍️ Author

Made by [Presiyan Georgiev](https://www.linkedin.com/in/presiyan-georgiev/)