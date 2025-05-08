
# P2PDelivery

A peer-to-peer delivery platform that connects people who need items delivered with individuals willing to transport them.

## Overview

This project was developed as the graduation project for the Full Stack Web Development using .NET track at the Information Technology Institute (ITI), Egypt.

P2PDelivery is a web-based platform built on ASP.NET Core that facilitates peer-to-peer delivery services. The system allows users to create delivery requests, apply to fulfill others' requests, and communicate in real-time throughout the delivery process.

## System Architecture

The application follows a clean architecture approach with four distinct layers:

- **Domain Layer**: Contains the core business entities and logic
- **Application Layer**: Houses services, DTOs, and business logic
- **Infrastructure Layer**: Handles data persistence and external services
- **API Layer**: Provides HTTP endpoints and real-time communication

## Key Features

- **User Authentication**: Secure JWT-based authentication system `Program.cs:135-174`
- **Delivery Request Management**: Create, update, track, and delete delivery requests `DeliveryRequestController.cs:32-56`
- **Application System**: Apply to fulfill delivery requests `DeliveryRequest.cs:39`
- **Real-time Chat**: Communicate with delivery partners using SignalR `ChatHub.cs:39-64`
- **Notifications**: Receive updates on delivery status changes `Program.cs:203`
- **Tracking**: Monitor the status and location of deliveries `TrackController.cs:1`

## Technology Stack

- **Backend**: ASP.NET Core 6+ `Program.cs:19`
- **Database**: SQL Server with Entity Framework Core `Program.cs:83-87`
- **Authentication**: JWT Bearer tokens `Program.cs:136-141`
- **Real-time Communication**: SignalR `Program.cs:115-123`
- **API Documentation**: Swagger/OpenAPI `Program.cs:29-64`

## Getting Started

### Prerequisites

- .NET 6.0 SDK or later
- SQL Server
- Visual Studio 2022 or Visual Studio Code

### Setup Instructions

1. Clone the repository

```bash
git clone https://github.com/minamichael14/P2PDelivery.git
```

2. Navigate to the project directory

```bash
cd P2PDelivery
```

3. Update the connection string in `appsettings.json` to point to your SQL Server instance

4. Apply migrations to create the database

```bash
dotnet ef database update
```

5. Run the application

```bash
dotnet run --project P2PDelivery.API
```

6. Access the Swagger UI at `https://localhost:5001/swagger`

## API Endpoints

### Authentication

- `POST /api/auth/register` - Register a new user
- `POST /api/auth/login` - Login and get JWT token

### Delivery Requests

- `POST /api/deliveryrequest` - Create a new delivery request `DeliveryRequestController.cs:32-56`
- `GET /api/deliveryrequest/{id}` - Get a specific delivery request `DeliveryRequestController.cs:58-69`
- `GET /api/deliveryrequest/my` - Get all delivery requests for the current user `DeliveryRequestController.cs:72-85`
- `GET /api/deliveryrequest` - Get all delivery requests with pagination `DeliveryRequestController.cs:90-104`
- `PUT /api/deliveryrequest/{id}` - Update a delivery request `DeliveryRequestController.cs:123-131`
- `DELETE /api/deliveryrequest/{id}` - Delete a delivery request `DeliveryRequestController.cs:133-142`

### Real-time Communication

- SignalR hub at `/hub/chat` - For real-time messaging `Program.cs:202`
- SignalR hub at `/hub/notification` - For real-time notifications `Program.cs:203`

## Collaborators
- [Kyrillos Adel](https://github.com/kyrillos-adel)
- [Mina Michael](https://github.com/minamichael14)
- [Omnia Nassef](https://github.com/omn22)
- [Rahil Raafat](https://github.com/RahilRafat)
- [Rawan Ragab](https://github.com/rawanragab44)

## Contact
For any issues or inquiries, contact **Kyrillos Adel** at kyrillosadelfahim@gmail.com
