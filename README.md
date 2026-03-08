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

## **Screenshots** 
### **Registration Form**

![registration](https://github.com/user-attachments/assets/d514bff9-f177-459d-afa2-8d34b6aef0ed)
<br><br>

### **Login Form**

![Login](https://github.com/user-attachments/assets/1e53b3d1-7c4f-4117-9156-78fa9627cedc)
<br><br>

### **Admin Dashboard**


![admin](https://github.com/user-attachments/assets/efbb7a4f-45d5-446d-8d2b-ab6c5c7ab584)
<br><br>

### **NGO Dashboard**


<img width="975" height="456" alt="NGO (2)" src="https://github.com/user-attachments/assets/cca81e66-bc55-461d-a24e-51adc6c2524f" />
<br><br>

### **Donor Dashboard**


![donor](https://github.com/user-attachments/assets/d3938583-81ea-431d-9532-80ffcabc1440)
  <br><br>

  ### **Receiver Dashboard**

  
![receiver](https://github.com/user-attachments/assets/39cfa94d-b9ab-4071-a0d2-fe2319f2356f)
<br><br>

##**Database View**

<img width="505" height="1047" alt="Database" src="https://github.com/user-attachments/assets/fa73f368-b9b9-469d-b56e-f4d56724adb9" />

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
