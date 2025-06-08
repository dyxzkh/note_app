# 📝 Note App

A full-stack note-taking application built with **ASP.NET Core 9 Web API** and **Vue 3** frontend. This app allows users to register, log in, and manage personal notes with a clean UI and secure backend.

---

## 🗂️ Project Structure

```
note_app/
├── note_webapi   # ASP.NET Core 9 backend (Web API)
└── note_webui    # Vue 3 frontend (Vite + TypeScript)
```

---

## 🚀 Features

### Backend (`note_webapi`)
- ASP.NET Core 9
- RESTful API architecture
- JWT authentication
- Role-based user system
- Notes CRUD operations
- Password hashing with Bcrypt
- Repository & Service pattern
- Docker support

### Frontend (`note_webui`)
- Vue 3 + Vite + TypeScript
- Pinia (state management)
- Vue Router
- Axios service for API interaction
- Modular architecture
- Auth and note views

---

## 🔧 Prerequisites

### Backend
- [.NET 9 SDK](https://dotnet.microsoft.com/)
- (Optional) Docker

### Frontend
- Node.js (LTS)
- npm or yarn

---

## 🛠️ Getting Started

### 1. Clone the repo

```bash
git clone https://github.com/dyxzkh/note_app.git
cd note_app
```

### 2. Run the Backend

#### Using .NET CLI

```bash
cd note_webapi/note_webapi
dotnet restore
dotnet run
```

#### OR using Docker

```bash
docker build -t note_webapi .
docker run -p 5225:80 note_webapi
```

API should be available at `http://localhost:5225`.

### 3. Run the Frontend

```bash
cd note_webui
npm install
npm run dev
```

Frontend app should be available at `http://localhost:3000`.

---

## 🔐 Environment Variables

### Backend (`note_webapi/appsettings.json`)
- JWT Secret
- Database connection string
- Token expiration settings

### Frontend (`.env.example`)
```
VITE_API_BASE_URL=http://localhost:5225/api
```

---

## 🧪 API Endpoints

| Method | Endpoint           | Description              |
|--------|--------------------|--------------------------|
| POST   | /api/auth/login    | Login with credentials   |
| POST   | /api/auth/register | Register new user        |
| GET    | /api/notes         | Get all user notes       |
| POST   | /api/notes         | Create a new note        |
| PUT    | /api/notes/{id}    | Update a note            |
| DELETE | /api/notes/{id}    | Delete a note            |

---

## 📦 Deployment

- Backend: Docker, Azure App Service, etc.
- Frontend: Vercel, Netlify, or static hosting

---

## 👨‍💻 Author

**Khgney Sovandy**  
GitHub: [@dyxzkh](https://github.com/dyxzkh)

---

## 📝 License

This project is open-source and available under the [MIT License](LICENSE).