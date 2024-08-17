# MaritesChat

MaritesChat is a lightweight ASP.NET MVC chat application optimized for Opera Mini and low-bandwidth environments. This project utilizes real-time messaging with SignalR, manages sessions using cookies, and offers a responsive UI specifically designed for feature tuplok-tuplok phones.

## Table of Contents

- [Features](#features)
- [Installation](#installation)
- [Usage](#usage)
- [Configuration](#configuration)
- [Technology Stack](#technology-stack)

## Features

- **Real-Time Messaging:** Powered by SignalR for instant message delivery.
- **Opera Mini Optimization:** Ensures smooth performance on low-bandwidth networks and feature phones.
- **Session Management:** Stores user information and channel history in session cookies.
- **Channel Management:** Supports multiple chat channels with a history of the last five accessed channels.
- **Dynamic UI Updates:** Responsive design with minimalistic UI tailored for Opera Mini's capabilities.

## Installation

### Prerequisites

- **Visual Studio 2022**
- **Microsoft Access Database (`.mdb`)**
- **Entity Framework Core** with the Jet provider for Microsoft Access

### Steps

1. **Clone the Repository:**
   ```bash
   git clone https://github.com/hubert17/marites-chat-opera-mini.git
   cd marites-chat-opera-mini
   ```

2. **Open the Project in Visual Studio:**
   - Open Visual Studio 2022.
   - Navigate to `File` -> `Open` -> `Project/Solution`.
   - Select the solution file (`.sln`) from the cloned repository.

3. **Configure the Database:**
   - Ensure your Microsoft Access `.mdb` file is in the project directory.
   - Update `appsettings.json` with the connection string:
     ```json
     "ConnectionStrings": {
         "DefaultConnection": "Data Source=Data\\MaritesChatDb.mdb"
     }
     ```

4. **Install EF Core Jet Provider:**
   - Open **NuGet Package Manager** in Visual Studio and install `EntityFrameworkCore.Jet`.

5. **Run Database Migrations:**
   - Open the **Package Manager Console** and execute:
     ```powershell
     Update-Database
     ```

6. **Build and Run the Project:**
   - Press `Ctrl + Shift + B` to build.
   - Press `F5` to run the application.

7. **Access the Application:**
   - Navigate to `http://localhost:5000` in your web browser.

## Usage

### Joining a Chat Channel

- Use the URL format `http://localhost:5000/{sender}/{channelCode}` to join a specific chat channel.
- Example: `http://localhost:5000/johndoe/general`

### Real-Time Updates

- Messages sent from one user appear instantly for others in the same channel.

### Managing Recent Channels

- The last five channels accessed are stored in cookies, ensuring quick access to your most recent chats.

## Configuration

### appsettings.json

- **Database Connection:**
  - Ensure the `DefaultConnection` string points to your `.mdb` file.

- **SignalR Settings:**
  - Modify settings if required.

### Cookie Settings

- **Sender and ChannelCode:** Stored in session cookies.
- **ChannelList:** Stores the last five accessed channels as a JSON string in a cookie.

## Technology Stack

- **Backend:** ASP.NET Core MVC
- **Real-Time Communication:** SignalR
- **Frontend:** HTML, CSS (minimal JavaScript)
- **Database:** Microsoft Access (`.mdb`) with Entity Framework Core
- **Development Environment:** Visual Studio 2022
