





# Task Manager API

A RESTful API built with ASP.NET Core and Entity Framework for managing task data.

## Requirements

- .NET 9 SDK
- ASP.NET Core
- Entity Framework Core
- SQLite
- Microsoft.EntityFrameworkCore.Design

## Installation


1. Use the guide to install .NET SDK - 9 (using Linux for mine)
    https://learn.microsoft.com/en-gb/dotnet/core/install/linux-debian?tabs=dotnet9

2. Install Entity Framework tools (if not already installed):

```bash
dotnet tool install --global dotnet-ef
```

3. Navigate to project directory:

```bash
cd TaskManagerApi
```

4. Restore dependencies:

```bash
dotnet restore
```



## Configuration

1. The database resides within a SQLite file named `tasks.db` (included with pre-populated data)
2. Connection string is configured in `appsettings.json`
3. Database context is configured with seeded data for immediate testing

## Usage

1. Run the application:

```bash
dotnet run
```

2. Open your web browser and go to `http://localhost:5157` to access the API
3. API Documentation available at: `http://localhost:5157/scalar/v1`

## Project Structure

```
TaskManagerApi/
├── Controllers/
│   └── TasksController.cs    # API endpoints and route handling
├── DTOs/
│   ├── TaskItemDtos.cs      # Data transfer objects for requests / update and create
│   ├── PaginationParameters.cs      # Data transfer objects for pagination params
│
├── Extensions/
│   └── TasksQuery.cs     # Logic to manipulate and structure data for tasks
    └── TaskAnalytics.cs     # Logic to manipulate and structure data for analytics
├── Models/
│   ├── TaskItem.cs           # Core task entity model
│   └── Responses/
│       ├── AnalyticsResponse.cs        # Analytics data structure
│       └── PaginatedTResponse.cs    # Paginated task results
├── Data/
│   └── TaskItemDbContext.cs      # Entity Framework database context with seeding
├── Migrations/               # Entity Framework migration files
├── Program.cs               # Application startup and configuration
├── appsettings.json         # Configuration settings
└── tasks.db                 # SQLite database file (pre-populated)
```

## Models

The application uses Entity Framework Core for database models:

### TaskItem Model

Core entity with the following attributes:

- `Id`: Primary key
- `Name`: Task title/description
- `Status`: Task status (Pending, InProgress, Completed, Cancelled)
- `CreatedAt`: Timestamp of task creation

### Response Models

- `AnalyticsResponse`: Contains task statistics and distribution data
- `PaginatedTaskResponse`: Wraps task collections with pagination metadata and sort items on created date

## Database Setup

1. The application uses SQLite. Database file `tasks.db` is included with pre-populated data
2. Database context automatically creates tables and seeds data on startup
3. To run migrations manually:

```bash
dotnet ef database update
```

4. To create new migrations:

```bash
dotnet ef migrations add MigrationName
```

## API Endpoints

The application provides the following REST endpoints:

### Tasks Management

- `GET /api/tasks` - Get paginated list of tasks
- `POST /api/tasks` - Create new task
- `PATCH /api/tasks/{id}` - Update existing task
- `DELETE /api/tasks/{id}` - Delete task

### Analytics

- `GET /api/tasks/analytics` - Get task statistics and distribution

## Running the Application

1. Start the ASP.NET Core server:

```bash
dotnet run
```

2. Access the API:

- API Documentation: http://localhost:5157/scalar/v1
- API Base URL: http://localhost:5157/api/

## Development Notes

- Database migrations have already been applied
- SQLite database file includes seeded data for immediate testing
- Task statuses support multiple states beyond simple complete/incomplete
- Extensions folder contains helper methods for data manipulation and controller logic
- DTOs ensure clean separation between API contracts and internal models

## Database Migration Commands

If you need to work with migrations:

```bash
# Update database to latest migration
dotnet ef database update

# Create new migration
dotnet ef migrations add MigrationName

# Remove last migration (if not applied)
dotnet ef migrations remove
```

## Trade-offs

### Testing Strategy:

- No unit tests included to prioritize rapid prototyping and core functionality
- Trade-off: Faster initial development cycle with focus on feature delivery over test coverage

### Error Handling:

- Streamlined error handling focused on essential scenarios
- Trade-off: Simplified codebase with clear error paths, prioritizing readability and maintainability

### Authentication & Authorization:

- No authentication layer implemented for simplified development and testing
- Trade-off: Enables immediate API testing and frontend integration without auth complexity
- Perfect for demonstration purposes and rapid prototyping phase

### Database Design:

- SQLite chosen for simplicity and portability
- Single TaskItem entity with essential attributes keeps the model focused
- Trade-off: Lightweight, zero-configuration database perfect for development and demos
- Easy to understand and modify without complex relationships
