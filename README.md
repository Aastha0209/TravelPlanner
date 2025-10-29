# 🌍 **TravelPlanner — ASP.NET Core MVC**

**TravelPlanner** is a smart and user-friendly trip-planning web application built using **ASP.NET Core MVC** and **SQLite**.  
It helps users **create, view, and manage travel itineraries** — complete with destinations, activities, notes, and images — all in one organized dashboard.

---

## 🧭 **Table of Contents**

1. [Demo & Purpose](#demo--purpose)  
2. [Key Features](#key-features)  
3. [Tech Stack](#tech-stack)  
4. [Prerequisites](#prerequisites)  
5. [Quick Start](#quick-start)  
6. [Configuration](#configuration)  
7. [Database & Migrations (EF Core)](#database--migrations-ef-core)  
8. [Default Demo Account](#default-demo-account)  
9. [Project Structure](#project-structure)  
10. [Troubleshooting](#troubleshooting)  
11. [Author](#author)

---

## 🎯 **Demo & Purpose**

**Purpose:**  
To demonstrate a full-stack ASP.NET MVC project with authentication, CRUD operations, and database integration using Entity Framework Core with SQLite.

**Use Cases:**  
- College mini-project for demonstrating MVC architecture.  
- Practical example of CRUD functionality and authentication.  
- Lightweight personal travel organizer.

---

## ✈️ **Key Features**

- ✅ User Authentication (Login / Signup)  
- ✅ Create, Edit, and Delete Trips  
- ✅ Add detailed itineraries (places, restaurants, activities, etc.)  
- ✅ Upload and view images for trips  
- ✅ Automatically seeded demo data  
- ✅ Responsive and modern UI using Bootstrap  
- ✅ SQLite Database (auto-created on first run)  

---

## ⚙️ **Tech Stack**

| Layer | Technology |
|-------|-------------|
| **Frontend** | HTML, CSS, Bootstrap |
| **Backend** | ASP.NET Core MVC (C#) |
| **Database** | SQLite |
| **ORM** | Entity Framework Core |
| **Authentication** | BCrypt Password Hashing |
| **IDE (Recommended)** | Visual Studio 2022 / Visual Studio Code |

---

## 🧩 **Prerequisites**

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)  
- Visual Studio 2022 or VS Code  
- SQLite (included by default — no setup needed)

Check your version:
```
dotnet --version
```
✅ It should show **8.x.x** or higher.

---

## 🚀 **Quick Start**

1. **Clone or Download the Repository**
   ```
   git clone https://github.com/Aastha0209/TravelPlanner.git
   cd TravelPlanner
   ```

2. **Open the Project**
   - In **Visual Studio** or **VS Code**.

3. **Run the Application**
   ```
   dotnet run
   ```

4. **Open in Browser**
   ```
   http://localhost:5288
   ```

5. **Login Using Demo Account**
   - ✉️ Email: **demo@travelplanner.com**  
   - 🔑 Password: **demo123**

---

## 🔧 **Configuration**

You don’t need to configure anything manually.  
When the app first runs, it automatically creates a SQLite database named **travelplanner.db** in the project folder.

If you want to use a custom path, edit `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=travelplanner.db"
  }
}
```

---

## 🗄️ **Database & Migrations (EF Core)**

The project uses **Entity Framework Core (Code-First)** with **SQLite**.

To recreate or update the database manually:
```
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 🧳 Seeded Demo Data
The app auto-seeds the following data when first launched:

**User:**  
- Email: demo@travelplanner.com  
- Password: demo123  

**Trips:**
1. ✨ Paris Getaway (France)  
   - Eiffel Tower visit, Seine Cruise, fine dining.  
2. 🗾 Tokyo Adventure (Japan)  
   - Shibuya Crossing, Mount Fuji day trip.  
3. 🏝️ Bali Escape (Indonesia)  
   - Uluwatu Temple, beach activities, sunset dinner.

---

## 📁 **Project Structure**

```
TravelPlanner/
├── Controllers/        → MVC Controllers (Trip, Itinerary, Account)
├── Models/             → Entity Models (User, Trip, Itinerary)
├── Data/               → ApplicationDbContext and seeding logic
├── Views/              → Razor Views (UI)
├── wwwroot/            → Static files (CSS, JS, images)
├── appsettings.json    → Configuration and DB connection
├── Program.cs          → Startup & automatic DB seeding
└── travelplanner.db    → SQLite database (auto-generated)
```

---

## 🧰 **Troubleshooting**

**Database not created?**  
→ Run the app once; EF Core auto-creates it.  
→ Or manually run:
```
dotnet ef database update
```

**Login not working?**  
→ Use demo credentials (`demo@travelplanner.com`, `demo123`).  
→ If you delete the DB, rerun the app — it will reseed automatically.

**SQLite locked error?**  
→ Close all running instances before deleting `travelplanner.db`.

---

## 👩‍💻 **Author**

Developed by: **Aastha Pandey**  
📧 **Contact:** 23amtics407@gmail.com  
