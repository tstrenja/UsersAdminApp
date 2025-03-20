# Blazor User Management App

This is a **Blazor UI** application that provides **user management, and logging**.  
It communicates with a **.NET 8 Web API** for backend operations, including user test logins, CRUD operations, and log tracking.

## 🚀 Features

- **User Management**: Create, update, delete, and list users.
- **Authentication**: Secure login with password hashing.
- **Logging System**: Tracks login attempts and stores logs.
- **Responsive UI**: Built with Blazor UI.
- **API Integration**: Communicates with a .NET 8 Web API. 
- **Global Error Handling**: Ensures consistency in API responses.
- **Swagger API Documentation**: For easy API testing and usage.

---

## 📂 Project Structure
```

```

---

## ⚡ Setup & Installation
### 🛠 Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or VS Code
- SQLite in memory 


 

## 📌 API Endpoints 
| Method | Endpoint | Description |
|--------|---------|-------------|
| `GET` | `/users/getusers` | Get all users |
| `GET` | `/users/getbyid/{id}` | Get user by ID |
| `POST` | `/users/addorupdate` | Add or update user |
| `DELETE` | `/users/delete/{id}` | Delete user | 
| `POST` | `/users/login` | Authenticate user |
| `GET` | `/users/getbyusername/{username}` | Get user by username | 
| `GET` | `/users/getlogs` | Get login logs |
| `POST` | `/users/savelog` | Save log entry |

---

 