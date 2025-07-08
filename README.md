# Hotel Booking App - Backend

This is the backend part of the Hotel Booking App built with **ASP.NET Core** and **Entity Framework Core**. It provides a RESTful API for managing hotel rooms, reservations, users, and more.

## 🚀 Features

- Room listing and details
- Room availability check
- Reservation creation
- Basic promo code system
- Entity Framework Core integration with SQL Server

## 🛠️ Technologies Used

- ASP.NET Core Web API (.NET 6+)
- Entity Framework Core
- SQL Server
- AutoMapper
- Dependency Injection
- Swagger / OpenAPI
- CORS policy setup for frontend communication

## 📦 Installation & Setup

1. Clone the repository:

   ```bash
   git clone https://github.com/your-username/hotel-booking-backend.git
   cd hotel-booking-backend

2. Open the solution in Visual Studio or run from CLI.

3. Update the database connection string in appsettings.json:
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=HotelBookingDB;Trusted_Connection=True;TrustServerCertificate=True;"
}

4. Apply migrations and create the database:

5. Run the application: dotnet run