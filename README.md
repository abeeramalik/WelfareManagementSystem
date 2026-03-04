# Welfare Management System

## Overview
The Welfare Management System is a role-based web application developed using ASP.NET Core MVC and Entity Framework Core. The system digitalizes and centralizes welfare operations including donation management, receiver assistance requests, fund allocation, NGO collaboration, and administrative monitoring.

It ensures transparency, accountability, and structured data handling through a clean MVC architecture and relational database design.

## Key Features

- Role-Based Access Control (Admin, Donor, Receiver, NGO)
- Donation Management (Money, Food, Clothes)
- Digital Aid Request Submission
- Automated Fund Allocation Mechanism
- NGO Collaboration Module
- Transaction Logging & History Tracking
- Monthly Allocation Management
- Session-Based Authentication
- Clean and Modular MVC Architecture


## Technologies Used

### Frontend
- HTML5
- CSS3
- Bootstrap
- JavaScript

### Backend
- ASP.NET Core MVC
- C#

### Database
- Microsoft SQL Server
- Entity Framework Core (Code-First Approach)

### Development Tools
- Visual Studio
- SQL Server Management Studio
- Git & GitHub

## Database Setup

1. Open `appsettings.json`
2. Update the connection string according to your SQL Server instance:

```json
"DefaultConnection": "Data Source=.\\SQLEXPRESS;Initial Catalog=WelfareDb;Integrated Security=True;Encrypt=False;TrustServerCertificate=True"

3. Run Migrations
Add-Migration InitialCreate
4. Update-Database
5. Run the application

## **Project Architecture**

The application follows the MVC pattern:

Models → Entity definitions & database relationships

Views → Razor-based UI

Controllers → Business logic handling

Background Services → Automated fund allocation

Entity Framework Core → ORM for database operations

## **Limitations**

No integrated online payment gateway

No real-time email or SMS notifications

Admin and NGO accounts are pre-created.

Developed it as part of BSCS coursework to demonstrate enterprise-level web application concepts.
