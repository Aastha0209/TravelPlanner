✈️ TravelPlanner
🌍 Project Overview

TravelPlanner is a web-based travel itinerary management system built using ASP.NET Core MVC and MySQL.
It allows users to register, log in, create and manage trips, add daily itineraries, and upload profile pictures.
The project provides an interactive dashboard that helps users plan and organize their trips efficiently.

🧠 Features

👤 User Registration & Login – Secure authentication system
🧳 Trip Management – Add, edit, view, and delete trips
🗓️ Itinerary Planner – Add daily plans and schedules for each trip
🖼️ Profile Picture Upload – Personalize user profiles
📅 Interactive Dashboard – Displays all trips with destinations and travel dates
💾 MySQL Database Integration – Reliable data storage
⚙️ Entity Framework Core (Migrations) – Automatic database creation and updates
🌐 ASP.NET Core MVC Framework – Follows MVC architecture for scalability

⚙️ Technologies Used

ASP.NET Core MVC – Backend Framework
C# – Application Logic
Entity Framework Core – ORM with Migrations for MySQL
MySQL – Database
HTML, CSS, Bootstrap – Frontend Design
Visual Studio / VS Code – Development Environment

🛠️ Installation & Setup

1️⃣ Clone the repository: git clone https://github.com/Aastha0209/TravelPlanner.git

2️⃣ Open the project in Visual Studio or VS Code.
3️⃣ Update appsettings.json with your MySQL credentials:
server=localhost;port=3306;database=travelplanner_db;user=root;password=yourpassword
4️⃣ Apply migrations by running: dotnet ef database update
(This automatically creates all required tables in MySQL.)
5️⃣ Run the project using Ctrl + F5 or “Start Without Debugging.”
It will open in your browser (example: http://localhost:5288
).
6️⃣ Register or log in → Create Trips → Add Itineraries → Upload Profile Picture.

🧭 Project Structure

TravelPlanner/
├── Controllers/ (AccountController.cs, HomeController.cs)
├── Models/ (User.cs, Trip.cs, Itinerary.cs)
├── Views/ (Home, Account)
├── Migrations/
├── wwwroot/ (images, css, js)
├── appsettings.json
├── Program.cs
├── TravelPlanner.csproj
├── README.md

👩‍💻 Author

Aastha Pandey
🎓 B.Tech CSE – 5th Semester
📅 Project: TravelPlanner 