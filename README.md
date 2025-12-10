# Task & Project Tracking System

## Overview

The Task & Project Tracking System is a console-based application designed to help small teams organize and manage their work efficiently. The system provides comprehensive task management capabilities including creation, modification, searching, sorting, and reporting functionality. All data is persisted to disk, ensuring task information is retained between sessions.

## System Purpose

This application addresses the need for lightweight, efficient task management in small team environments. It provides:

- Centralized task tracking with unique identifiers
- Priority-based task organization
- Status tracking throughout the task lifecycle
- Assignee management
- Deadline monitoring with overdue task identification
- Persistent data storage using JSON format
- Comprehensive activity logging
- Flexible search and sort capabilities

## Features

### Core Functionality

- **Task Management**: Create, update, and delete tasks with detailed attributes
- **Status Tracking**: Monitor task progress through To-Do, In Progress, and Done states
- **Priority System**: Categorize tasks as Low, Medium, or High priority
- **Search Capabilities**: 
  - Binary search by task ID for efficient retrieval
  - Linear search by assignee name
- **Sorting Algorithms**:
  - Manual bubble sort implementation for priority-based ordering
  - Built-in sort for deadline-based organization
- **Reporting**: Generate reports highlighting overdue and upcoming tasks
- **Data Persistence**: JSON-based storage ensures data retention across sessions
- **Activity Logging**: Comprehensive logging of all system actions to text file

### Technical Features

- Object-oriented design with inheritance and polymorphism
- Generic data persistence layer
- Robust error handling and input validation
- Comprehensive unit test coverage
- .NET 8.0 framework implementation

## System Requirements

- .NET 8.0 SDK or later
- Windows, macOS, or Linux operating system
- Minimum 50MB available disk space

## Installation

1. Clone or download the repository to your local machine
2. Ensure .NET 8.0 SDK is installed on your system
3. Navigate to the project directory

## Running the Application

### Using Visual Studio

1. Open `Task & Project Tracker.sln` in Visual Studio
2. Set `Task & Project Tracker` as the startup project
3. Press F5 to run with debugging, or Ctrl+F5 to run without debugging

### Using Command Line

1. Open a terminal in the project directory
2. Navigate to the solution folder:
   ```bash
   cd "c:\Users\Admin\Downloads\bi81xp\Deliverables\Task & Project Tracker"
   ```
3. Run the application:
   ```bash
   dotnet run --project "Task & Project Tracker"
   ```

## Running Unit Tests

### Visual Studio

1. Open Test Explorer: **Test** > **Test Explorer**
2. Click **Run All Tests** or press Ctrl+R, A
3. View test results in the Test Explorer window

### Command Line

```bash
dotnet test
```

Expected output:
```
Passed!  - Failed: 0, Passed: 5, Skipped: 0, Total: 5
```

## Usage Guide

Upon launching the application, you will be presented with a menu-driven interface:

1. **Add Task**: Create a new task with ID, title, description, due date, priority, and assignee
2. **List All Tasks**: Display all tasks in the system
3. **Update Task Status**: Change the status of an existing task
4. **Delete Task**: Remove a task from the system
5. **Search Task by ID**: Locate a specific task using binary search
6. **Search Tasks by Assignee**: Find all tasks assigned to a specific person
7. **Sort Tasks by Priority**: Order tasks using manual bubble sort algorithm
8. **Sort Tasks by Due Date**: Order tasks chronologically using built-in sort
9. **Export Report**: Generate a text file report of overdue and upcoming tasks
0. **Exit**: Close the application

## Architecture and Design Choices

### Object-Oriented Design

The system employs a layered architecture with clear separation of concerns:

#### Models Layer
- **TaskItem**: Abstract base class defining core task properties and behavior
- **DevelopmentTask**: Concrete implementation extending TaskItem with code review functionality
- **Enumerations**: TaskStatus and Priority for type-safe state management

#### Services Layer
- **FilePersistenceService<T>**: Generic service for JSON serialization and deserialization
- **LoggerService**: Centralized logging to text file
- **TaskManager**: Core business logic orchestrating task operations

#### Presentation Layer
- **Program**: Console UI with input validation and user interaction handling

### Key Design Decisions

#### 1. Inheritance and Polymorphism
The abstract `TaskItem` class provides a foundation for different task types. `DevelopmentTask` extends this base class, demonstrating inheritance. The system uses `IComparable<TaskItem>` interface implementation for flexible sorting capabilities.

#### 2. Generic Persistence Layer
`FilePersistenceService<T>` uses generics to provide reusable data persistence functionality. This design allows the same service to handle different entity types without code duplication.

#### 3. JSON Serialization
The system uses `System.Text.Json` for data persistence, chosen for:
- High performance and low memory allocation
- Native .NET integration
- Strong type safety
- Support for polymorphic serialization via `[JsonDerivedType]` attribute

#### 4. Algorithm Implementation
Two search algorithms demonstrate different approaches:
- **Binary Search**: O(log n) complexity for ID-based lookups, requiring sorted data
- **Linear Search**: O(n) complexity for assignee-based searches where sorting is not applicable

Two sorting implementations showcase algorithmic variety:
- **Bubble Sort**: Manual implementation demonstrating fundamental sorting logic
- **Built-in Sort**: Leverages `IComparable` interface for efficient sorting

#### 5. Error Handling Strategy
The application implements defensive programming with:
- Try-catch blocks around file I/O operations
- Input validation using TryParse methods
- Graceful degradation when data files are missing or corrupted
- User-friendly error messages

#### 6. Input Validation
All user inputs are validated through dedicated helper methods:
- `ReadInt()`: Ensures valid integer input
- `ReadDate()`: Validates date format
- `ReadString()`: Prevents empty string submission
- `ReadPriority()` and `ReadTaskStatus()`: Enforce enumeration constraints

#### 7. Logging Strategy
All significant operations (create, update, delete, export) are logged with timestamps to `log.txt`, providing an audit trail for system activity.

## File Structure

```
Task & Project Tracker/
├── Models/
│   ├── TaskItem.cs           # Abstract base class
│   └── DevelopmentTask.cs    # Concrete task implementation
├── Services/
│   ├── FilePersistenceService.cs  # Generic JSON persistence
│   ├── LoggerService.cs           # Activity logging
│   └── TaskManager.cs             # Core business logic
├── Program.cs                # Console UI and entry point
├── Task & Project Tracker.csproj
└── README.md

Task & Project Tracker.Tests/
├── TaskManagerTests.cs       # Unit tests
└── Task & Project Tracker.Tests.csproj
```

## Data Storage

### tasks.json
Task data is stored in JSON format in the application directory. The file is automatically created on first task creation and updated with each modification.

### log.txt
Activity logs are appended to this file with timestamps. Each significant operation is recorded for audit purposes.

### report.txt
Generated on demand via the Export Report function. Contains lists of overdue and upcoming tasks.

## Testing

The application includes comprehensive unit tests covering:

1. **AddTask_ShouldIncreaseTaskCount**: Validates task creation
2. **DeleteTask_ShouldRemoveTask**: Verifies task deletion
3. **UpdateTaskStatus_ShouldChangeStatus**: Tests status modification
4. **BinarySearchById_ShouldReturnCorrectTask**: Validates search algorithm
5. **SortByPriority_ShouldSortTasksCorrectly**: Confirms sorting implementation

All tests use isolated test data files to prevent interference with production data.

## Troubleshooting

### Build Errors
- Ensure .NET 8.0 SDK is installed: `dotnet --version`
- Rebuild the solution: `dotnet clean` followed by `dotnet build`

### Test Discovery Issues in Visual Studio
- Close and reopen Visual Studio
- Delete `.vs`, `bin`, and `obj` folders
- Rebuild the solution
- Alternatively, use `dotnet test` from command line

### Data File Corruption
If `tasks.json` becomes corrupted:
1. Close the application
2. Delete or rename `tasks.json`
3. Restart the application (a new file will be created)

## Future Enhancements

Potential areas for expansion include:

- Multi-user support with authentication
- Web-based interface
- Database backend for enterprise scalability
- Task dependencies and project hierarchies
- Email notifications for approaching deadlines
- Advanced filtering and query capabilities
- Export to multiple formats (CSV, PDF)

## License

This project is developed for educational purposes as part of a software development coursework assignment.

## Contact

For questions or support regarding this application, please contact the development team.
