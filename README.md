# 🎓 Online Course Platform — Backend

ASP.NET Core Web API for the Online Course Platform, featuring PayPal payment integration, real-time SignalR notifications, and a clean architecture design.

---

## 🚀 Tech Stack

- **ASP.NET Core 8** — Web API
- **Entity Framework Core** — ORM with SQL Server
- **MediatR** — CQRS (Commands, Queries, Domain Events)
- **SignalR** — Real-time notifications
- **PayPal REST API** — Payment processing
- **ASP.NET Core Identity** — User management
- **JWT Bearer** — Token-based authentication

### Architecture
- **Clean Architecture** — Domain, Application, Infrastructure, API layers
- **CQRS + MediatR** — Separated read/write operations
- **Domain Events** — Decoupled side effects, dispatched after `SaveChangesAsync`
- **Result Pattern** — Explicit error handling without exceptions

---

## 🏗️ Project Structure

```
OnlineCoursePlatform/
├── API/
│   ├── Controllers/            # PaymentsController, EnrollmentsController...
│   └── Program.cs
├── Application/
│   ├── Features/
│   │   ├── Payments/           # CreateOrder, ConfirmPayment commands
│   │   └── Enrollments/        # CreateEnrollment, GetMyEnrollments
│   └── Common/                 # Result pattern, interfaces, ICurrentUser
├── Domain/
│   ├── Entities/               # Course, Enrollment, Payment, User
│   └── Events/                 # PaymentSucceededEvent, EnrollmentCreatedEvent
└── Infrastructure/
    ├── Data/                   # AppDbContext, EF configurations
    ├── Services/               # PayPalService, SignalRNotificationService
    └── Identity/               # CurrentUser implementation
```

---

## 💳 Payment Flow

```
1. User initiates payment  →  CreateOrderAsync (PayPal)
2. User completes payment on PayPal
3. PayPal webhook fires    →  HandleWebHook
4. ConfirmPaymentCommand   →  marks Payment as Succeeded
5. PaymentSucceededEvent   →  CreateEnrollmentCommand (new scope)
6. EnrollmentCreatedEvent  →  SignalR notifies user in real-time
```

---

## ⚡ Real-Time Notifications

```
EnrollmentCreatedEvent
  └── EnrollmentCreatedEventHandler
        └── IEnrollmentNotifier.SendEnrollmentCreatedAsync()
              └── SignalR → pushes EnrollmentDto to client instantly
```

Hub endpoint: `GET /hubs/enrollments`

---

## 🛠️ Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server)
- PayPal Developer account ([sandbox](https://developer.paypal.com))

### Setup

```bash
# Clone the repository
git clone https://github.com/your-username/online-course-platform-api.git
cd online-course-platform-api

# Restore dependencies
dotnet restore

# Set up secrets
dotnet user-secrets set "PayPal:ClientId" "your-client-id"
dotnet user-secrets set "PayPal:Secret" "your-secret"
dotnet user-secrets set "PayPal:BaseUrl" "https://api-m.sandbox.paypal.com"
dotnet user-secrets set "PayPal:WebhookId" "your-webhook-id"
dotnet user-secrets set "JwtSettings:SecretKey" "your-secret-key"

# Apply migrations
dotnet ef database update

# Run
dotnet run --project API
```

API runs on: `http://localhost:5050`

---

## ⚙️ Configuration

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=OnlineCoursePlatform;Trusted_Connection=True;"
  },
  "JwtSettings": {
    "SecretKey": "",
    "Issuer": "OnlineCoursePlatform",
    "Audience": "OnlineCoursePlatform",
    "ExpiryMinutes": 60
  },
  "PayPal": {
    "ClientId": "",
    "Secret": "",
    "BaseUrl": "https://api-m.sandbox.paypal.com",
    "WebhookId": ""
  },
  "AllowedOrigins": ["http://localhost:4200"]
}
```

---

## 🔑 Key Design Decisions

### Result Pattern
All business operations return `Result<T>` instead of throwing exceptions — error handling is explicit and predictable. Exceptions are reserved for unexpected infrastructure failures.

### Domain Events After SaveChangesAsync
Events are collected before saving, then dispatched after `SaveChangesAsync` completes — preventing concurrent DbContext access.

### IServiceScopeFactory in Event Handlers
Event handlers that need DB access create their own scope via `IServiceScopeFactory` — fresh `DbContext`, fresh connection, fully isolated from the request that raised the event.

### ICurrentUser
User identity is always resolved server-side from the JWT `NameIdentifier` claim — never trusted from the request body.

### PayPal Token Caching
Access tokens are cached with a 60-second safety buffer against their `expires_in` value, with a `SemaphoreSlim` lock to prevent concurrent refresh races.

---

## 📄 License

MIT
