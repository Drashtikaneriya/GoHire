# GoHire - Recruitment Management System 🚀

An advanced .NET-based recruitment management platform. This system simplifies and streamlines the entire hiring and recruitment process — from job posting and candidate applications, to interviewing and final selection.

## 🌟 Features

- **Candidate Dashboard:** Register, manage profiles, upload resumes, and track job application statuses.
- **Company Management:** Create company profiles and seamlessly post or manage job openings.
- **Application Tracking:** Comprehensive views for HR professionals to review candidates and process applications.
- **Interview Scheduling:** Coordinate multi-round interviews and manage interviewer schedules efficiently.
- **Role-Based Authentication:** Secure access control with separate features for Admins, Employers, and Candidates.
- **Dual Architecture:** Built with a clean separation of concerns using Web API for backend services and MVC for client interaction.

## 🛠️ Tech Stack

- **Framework:** ASP.NET Core 
- **Architecture:** ASP.NET Core MVC & RESTful Web API
- **Language:** C#
- **Database ORM:** Entity Framework (EF) Core
- **Frontend Views:** Razor Pages (`.cshtml`), HTML5, CSS3, BootStrap
- **Data Exchange:** DTOs (Data Transfer Objects)

## 📁 Project Structure

- `RecruitmentsystemMVC`: Contains the main frontend web client built with ASP.NET Core MVC, handling Controllers, Services communicating with the API, and Razor Views.
- `RecruitmentUpdated`: Contains the RESTful Web API backend taking care of the fundamental business logic, data models, DTOs, and the Application Database Context.

## ⚙️ Getting Started

### Prerequisites
- Visual Studio 2022 (or Rider/VS Code)
- .NET SDK (6.0, 7.0 or 8.0)
- SQL Server

### Installation Steps

1. **Clone the repository:**
   ```bash
   git clone https://github.com/Drashtikaneriya/GoHire.git
   ```

2. **Database Setup:**
   Navigate into the Web API project (`RecruitmentUpdated`), make sure your Connection String inside `appsettings.json` is set to your local DB, and update the database:
   ```bash
   Add-Migration InitialCreate
   Update-Database
   ```

3. **Run the Project:**
   Set both `RecruitmentsystemMVC` and `RecruitmentUpdated` as startup projects (if you're using Visual Studio, right-click Solution -> Set Startup Projects -> Multiple startup projects). 

## 📝 About

This project was built as part of the Sem-6 Advanced .NET coursework.
