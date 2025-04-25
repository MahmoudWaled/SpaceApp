# Space - Real-Time Social Media App

**Space** is a real-time social media web application built with a focus on clean architecture and scalable design. It allows users to communicate instantly through real-time messaging and supports core social features like posting, liking, and commenting.

---

## Technologies Used

- **ASP.NET Core (.NET 8)** – Backend API
- **SignalR** – Real-time communication (WebSocket)
- **Entity Framework Core** 
- **SQL Server** 
- **JWT Authentication** – Secure authentication & authorization
- **Clean Architecture** – (API-Application-Infrastructure-Domain) For code separation and maintainability
- **FluentValidation** – For clean and reusable model validation
- **AutoMapper** – For object-to-object mapping between DTOs and entities

---

## Features

- **JWT-Based User Authentication**
- **Real-Time Chat System**
- **Posts System**
  - Users can create, view, update and delete posts
-  **Like System**
  - Users can like/unlike posts
  - Get likes count on post
- **Comments System**
  - Users can comment on posts and edit or delete it
- **Local Image Uploads**
  - can upload image for post or profile
- **Notifications**
 - Real-time notifications for comments, messages, and other activities.
- **Clean & Modular Code Structure**
  - Follows best practices with Clean Architecture (API-Application-Infrastructure-Domain)
- **Extensible Design**
  - Ready for future features like:
    - Push notifications
    - Real-time user status
    - Mobile application integration

---

## Project Structure

The solution is structured for scalability and maintainability:

### `Space.API`
- Application entry point  
- Controllers (Users, Posts, Messages, etc.)  
- SignalR Hub (`ChatHub`)  
- Custom Middlewares (e.g., ExceptionHandler)  
- Configuration (`appsettings.json`)  
- Startup & Dependency Injection (`Program.cs`)

### `Space.Application`
- Core business logic  
- DTOs for API communication  
- Services and Interfaces  
- AutoMapper Profiles  
- FluentValidation Rules

### `Space.Domain`
- Domain Entities (`User`, `Post`, `Message`, etc.)  
- Identity Models (`ApplicationUser`)  
- Business rules

### `Space.Infrastructure`
- EF Core DbContext  
- Repositories (CRUD logic)  
- Database Migrations  
- External Services (e.g., EmailService, FileService)

![screencapture-localhost-7125-swagger-index-html-2025-04-25-03_03_49](https://github.com/user-attachments/assets/347843db-3ced-49c2-b9d3-e6ba3efe126e)

