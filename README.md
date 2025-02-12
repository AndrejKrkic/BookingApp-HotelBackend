# Hotel Room Reservation – Backend Application

This is the backend API for a hotel room reservation system, built using **ASP.NET Core** and **Entity Framework**. The application provides essential functionalities for managing room reservations, customers, and availability.

## Features
- Room management (CRUD operations)
- Customer management
- Reservation management
- Real-time availability check
- Database integration with **Entity Framework**
- RESTful API structure

## Technologies
- **ASP.NET Core** – Web API framework  
- **Entity Framework** – ORM for database management  
- **SQL Server** – Database  
- **C#** – Programming language  

## Database Structure
The database includes the following entities:  
- **Hotel Rooms** – Details about rooms (room type, number, status)  
- **Customers** – Personal details of customers  
- **Reservations** – Information about bookings (dates, room assigned, customer reference)

## Setup and Installation
1. **Clone the repository**:
   ```bash
   git clone https://github.com/your-username/hotel-reservation-backend.git
2. **Open the solution in Visual Studio.**:

3. Apply migrations and update the database:
dotnet ef database update

4. Run the application:
dotnet run

5. The API will be available at https://localhost:5001.

API Endpoints
Here’s a summary of the key API endpoints:

Method	Endpoint	Description
GET	/api/rooms	Get a list of all rooms
POST	/api/rooms	Add a new room
GET	/api/reservations	Get all reservations
POST	/api/reservations	Create a new reservation
GET	/api/customers	Get customer details
Future Improvements
Authentication and authorization
Integration with third-party payment systems
Email notifications for booking confirmation
