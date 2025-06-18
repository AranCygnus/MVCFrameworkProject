# ProjectGame 線上遊戲商城（ASP.NET Core MVC .NET 8）

這是一個使用 Visual Studio 開發的 ASP.NET Core MVC (.NET 8) 範例專案。專案主要展示 MVC 架構在 .NET 8 下的基本應用，包括員工後台、商品瀏覽與購物車功能。

This is a sample ASP.NET Core MVC (.NET 8) project developed using Visual Studio.  
The project demonstrates the basic use of the MVC architecture in .NET 8, including staff management, product browsing, and shopping cart features.


## 目錄 Table of Contents

- [專案簡介 | Project Overview](#專案簡介--project-overview)
- [功能介紹 | Features](#功能介紹--features)
- [開發工具 | Development Tools](#開發工具--development-tools)
- [安裝與執行方式 | Installation & Usage](#安裝與執行方式--installation--usage)
- [專案結構 | Project Structure](#專案結構--project-structure)
- [備註 | Notes](#備註--notes)


---


## 專案簡介 | Project Overview


本專案為一個線上遊戲商城系統示範，提供會員、員工註冊/登入、商品瀏覽、購物車與後台管理功能。方便了解 ASP.NET Core MVC 開發流程與架構設計。

This project is a demonstration of an online game store system, featuring user and staff registration/login, product browsing, shopping cart, and backend management. It is designed to help understand the development flow and architecture of ASP.NET Core MVC.


## 功能介紹

- 會員註冊 / 登入 / 登出
  - 依 Email 辨別身分：一般 Email 註冊為會員，公司 Email（@gamebox.com）註冊為員工
- 商品瀏覽、搜尋
- 購物車管理
- 歷史訂單下單查看
- 後台商品管理（新增、編輯、刪除）
- 後台顧客名單與訂單查看

- User registration / login / logout
  - Identity based on email: normal email for users, company email (@gamebox.com) for staff
- Product browsing and search
- Shopping cart management
- Order history viewing
- Backend product management (create, edit, delete)
- Backend customer list and order management


## 開發工具 | Development Tools

- Visual Studio 2022 or later
- .NET 8 SDK
- SQL Server Express / LocalDB
- NuGet Package Manager
- [SQL Server Management Studio (SSMS)](https://aka.ms/ssms) (recommended for database management)


## 安裝與執行方式 | Installation & Usage

1. **安裝必要軟體 Required Software**
   - [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)（本專案建議使用 SQL Server Express 作為資料庫）
   - [SQL Server Management Studio (SSMS)](https://aka.ms/ssms)（管理與查詢資料庫用）
   - [Visual Studio 2022](https://visualstudio.microsoft.com/downloads/)

2. **下載專案原始碼 Download the source code**
   ```bash
   git clone https://github.com/your-username/ProjectGame.git

3. 用 Visual Studio 開啟 ProjectGame.sln Open the solution with Visual Studio

4. 設定資料庫連線字串（於 appsettings.json）Set your connection string in appsettings.json:
   ```bash
    "ConnectionStrings": {
        "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=MyAspNetCoreMvcDb;Trusted_Connection=True;"
    }

5. 進行資料庫遷移 Run database migrations:

    開啟 Visual Studio「套件管理員主控台」，執行：
    Open Visual Studio Package Manager Console, and run:

    Add-Migration UpdataGameDataBase
    Update-Database

6. 執行專案
    按下 F5 或點選「啟動」按鈕。


## 專案結構 | Project Structure
```bash
│
├── Controllers/        控制器 | Controllers
├── Models/             資料模型（資料庫用） | Data Models
├── ViewModels/         視圖模型（畫面資料傳遞） | View Models
├── Views/              Razor 頁面 | Razor Views
├── Data/               DbContext 與種子資料 | DbContext & Seed Data
├── wwwroot/            靜態資源 | Static Files
├── Program.cs          程式進入點 | Main Entry
├── appsettings.json    設定檔 | Configuration
├── ProjectGame.sln     方案檔 | Solution File
└── ...
```

## 備註 | Notes

本專案為學校期末報告之用，僅供學習與交流。

This project was created as a school final project and is intended solely for educational and non-commercial use.
