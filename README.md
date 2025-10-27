TravelPlanner
Project Overview

TravelPlanner is a web-based travel itinerary management system built using ASP.NET Core MVC and MySQL.
It allows users to register, log in, create and manage trips, add daily itineraries, and upload profile pictures.
The project includes an interactive dashboard that helps users plan and organize their trips efficiently.

Features

User Registration and Login

Trip Management (Add, Edit, Delete)

Itinerary Planner for Daily Schedules

Profile Picture Upload

Dashboard Displaying All Trips

Data Stored in MySQL Database

Built with ASP.NET Core MVC and Entity Framework Core

Technologies Used
Technology	Purpose
ASP.NET Core MVC	Backend Framework
C#	Application Logic
Entity Framework Core	ORM for MySQL
MySQL	Database
HTML, CSS, Bootstrap	Frontend Design
Visual Studio / VS Code	Development Environment
Installation and Setup

Clone the repository using Git.

Open the project in Visual Studio or VS Code.

Configure the database connection in appsettings.json by updating your MySQL credentials.

Run migrations using Entity Framework Core to create the necessary tables.

Start the project using Ctrl + F5 or “Start Without Debugging”.

The project will open in your browser, usually at http://localhost:5001 (the port may vary).

Register or log in to use the system, create trips, add itineraries, and upload profile pictures.

Steps to Run from GitHub

Download or clone this repository.

Open the solution file TravelPlanner.sln in Visual Studio.

Restore NuGet packages if prompted.

Ensure MySQL is running locally.

Apply migrations with Update-Database (only if the database is not created).

Run the project — the web application will launch in your browser.

Project Structure

TravelPlanner/
├── Controllers/ (AccountController, HomeController)
├── Models/ (User, Trip, Itinerary)
├── Views/ (Home, Account)
├── wwwroot/ (CSS, JS, Images)
├── appsettings.json
├── Program.cs
├── TravelPlanner.sln
├── README.md

Author

Aastha Pandey
B.Tech CSE, 4th Semester
Project: TravelPlanner