# **User Management System**

This is a **Full-Stack User Management System** built with:
- **Backend**: .NET Core 8 (Web API) + MS SQL Server  
- **Frontend**: Angular 18+ (Material UI)  

---

## **🛠️ Prerequisites**
Before running the project, ensure you have installed:

### ✅ **Backend Requirements**
- [.NET SDK 8](https://dotnet.microsoft.com/download)  
- [MS SQL Server 2019+](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)  
- [SQL Server Management Studio (SSMS)](https://aka.ms/ssmsfullsetup) (Optional)

### ✅ **Frontend Requirements**
- [Node.js 18+](https://nodejs.org/)  
- [Angular CLI 18+](https://angular.io/cli)  

---

## **📌 Setup & Run the Backend (`UserManagementAPI`)**
1️⃣ **Clone the repository**
```sh
git clone [https://github.com/YOUR_GITHUB_USERNAME/YOUR_REPO_NAME.git](https://github.com/mhnnamosab/UserManagement.git)


2️⃣ **CREATE DATABASE UserManagementDB**

Update the connection string in appsettings.json:
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=UserManagementDB;Trusted_Connection=True;TrustServerCertificate=True;"
}

3️⃣ **Apply Migrations & Seed Data**
cd UserManagementAPI
dotnet ef database update

4️⃣ **Run the Backend**
dotnet run


## **🚀 Setup & Run the Frontend (user-management-app)**
1️⃣ **Navigate to the frontend folder**
cd user-management-app
2️⃣ **Install dependencies**
npm install

3️⃣ **Update API Base URL**
Modify environment.ts if needed:
export const environment = {
  production: false,
  apiUrl: 'https://localhost:5001/api'
};
4️⃣ **Run the frontend**
ng serve --open

