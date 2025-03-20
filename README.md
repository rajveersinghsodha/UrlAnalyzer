# URL Analyzer

A web application that analyzes URLs to extract images and perform word frequency analysis. Built with ASP.NET Core 6.0 and modern web technologies.

## Technologies & Dependencies

### NuGet Packages

1. **HtmlAgilityPack** (Version 1.11.59)
   - Used for HTML parsing and manipulation
   - Extracts images and text content from web pages
   - [NuGet Link](https://www.nuget.org/packages/HtmlAgilityPack/)

2. **FluentValidation.AspNetCore** (Version 11.3.0)
   - Handles input validation
   - Validates URL format and other user inputs
   - [NuGet Link](https://www.nuget.org/packages/FluentValidation.AspNetCore)

3. **Serilog.AspNetCore** (Version 8.0.1) and **Serilog.Sinks.File** (Version 5.0.0)
   - Structured logging for the application
   - Logs both to console and files
   - Daily rolling log files with size limits
   - [Serilog.AspNetCore NuGet](https://www.nuget.org/packages/Serilog.AspNetCore)
   - [Serilog.Sinks.File NuGet](https://www.nuget.org/packages/Serilog.Sinks.File)

### Frontend Libraries (CDN)

1. **Bootstrap** (Version 5.3.0)
   - Frontend framework for responsive design
   - Used for layout, components, and styling
   - CDN: `https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css`
   - JS: `https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js`

2. **Bootstrap Icons** (Version 1.11.3)
   - Icon library for UI elements
   - Used for navigation and button icons
   - CDN: `https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.css`

3. **Chart.js** (Latest Version)
   - JavaScript charting library
   - Used for word frequency visualization
   - CDN: `https://cdn.jsdelivr.net/npm/chart.js`

### Features

1. **Image Carousel**
   - Implemented using Bootstrap's built-in carousel component
   - Customized with black navigation controls
   - Responsive image display with max height constraints

2. **Word Frequency Analysis**
   - Table view using Bootstrap tables
   - Chart visualization using Chart.js
   - Custom filtering for HTML content tags

3. **Logging System**
   - Comprehensive logging using Serilog
   - Logs stored in "Logs" directory
   - Daily rolling log files (e.g., "urlanalyzer-20240320.log")
   - Maximum file size: 5MB
   - Retains last 7 days of logs
   - Captures:
     - Application startup/shutdown
     - Errors and exceptions
     - URL analysis operations
     - General application activity

## Setup Instructions

1. **Prerequisites**
   - Visual Studio 2019 or later
   - .NET 6.0 SDK
   - Internet connection (for CDN resources)

2. **Installation**
   ```bash
   # Clone the repository
   git clone [repository-url]

   # Open the solution in Visual Studio
   # Build the solution (this will restore NuGet packages automatically)
   ```

3. **Running the Application**
   - Press F5 in Visual Studio to run the application
   - Or use the command line:
     ```bash
     dotnet run
     ```

## Project Structure

- `Controllers/`: MVC controllers
- `Views/`: Razor views
- `Services/`: Business logic and URL analysis
- `DTOs/`: Data transfer objects
- `wwwroot/`: Static files (CSS, JavaScript)
- `Logs/`: Application log files (created automatically)

## Configuration

The application uses standard ASP.NET Core configuration with the following features:
- Serilog for structured logging
  - Console output for development
  - File output for persistent logging
  - Automatic log rotation and size management
- HTTP client configuration with timeout settings
- Custom error handling
- Static file serving

## Notes

- The application is configured to run on HTTPS by default
- Bootstrap components are loaded via CDN for better performance
- Chart.js is used for dynamic data visualization
- The carousel is responsive and supports touch navigation on mobile devices
- Log files are automatically managed with retention policies 