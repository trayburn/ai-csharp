# AI C# Repository Instructions

Always reference these instructions first and fallback to search or bash commands only when you encounter unexpected information that does not match the info here.

## Working Effectively

**CRITICAL**: This repository requires .NET 9.0 SDK to build. If you see NETSDK1045 errors, install .NET 9.0 first.

### Initial Setup and Build Process
- Install .NET 9.0 SDK: `curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --version latest --channel 9.0`
- Add to PATH: `export PATH="$HOME/.dotnet:$PATH"`
- Navigate to solution: `cd WeatherDemo`
- Restore packages: `dotnet restore` -- takes 11 seconds. NEVER CANCEL. Set timeout to 30+ minutes.
- Build solution: `dotnet build` -- takes 12 seconds. NEVER CANCEL. Set timeout to 30+ minutes.
- Run tests: `dotnet test` -- takes 4 seconds. NEVER CANCEL. Set timeout to 15+ minutes.
- Clean build artifacts: `dotnet clean` -- takes 1 second.

### Running the Application
- ALWAYS run the setup steps first
- Navigate to web project: `cd WeatherDemo/WeatherDemo.Web`
- Run web application: `dotnet run`
- Application starts on `http://localhost:5219` 
- Access weather page: `http://localhost:5219/Weather` or `http://localhost:5219/` (redirects)
- Stop with Ctrl+C

### Code Quality and Formatting
- Check formatting: `dotnet format --verify-no-changes` (will exit with code 2 if formatting issues exist)
- Fix formatting: `dotnet format`
- ALWAYS run `dotnet format` before committing changes or CI will fail

## Validation

### Manual Testing Requirements
- ALWAYS manually validate the web application after making changes by:
  1. Starting the web app with `dotnet run`
  2. Making HTTP request to `http://localhost:5219/Weather`
  3. Verify weather data table displays 5 random cities with temperature data
  4. Example working response should contain HTML table with cities like "Paris", "New York", etc.

### End-to-End Testing
- Build the solution: `dotnet build`
- Run all unit tests: `dotnet test` (should show "Test summary: total: 5, failed: 0, succeeded: 5")
- Start web app and verify weather data displays correctly
- Check formatting with `dotnet format --verify-no-changes`

## Repository Structure

### Projects
This is a .NET 9.0 solution with three projects:

1. **WeatherDemo.Library** - Core business logic
   - Location: `WeatherDemo/WeatherDemo.Library/`
   - Contains `WeatherService`, `WeatherReport`, and `IRandomProvider`
   - Target framework: net9.0

2. **WeatherDemo.Web** - ASP.NET Core web application  
   - Location: `WeatherDemo/WeatherDemo.Web/`
   - Razor Pages application displaying weather data
   - Main page: `/Weather` shows 5 random cities
   - Target framework: net9.0

3. **WeatherDemo.Tests** - xUnit test project
   - Location: `WeatherDemo/WeatherDemo.Tests/`
   - Uses Moq for mocking
   - Contains 5 unit tests for WeatherService
   - Target framework: net9.0

### Key Files
- Solution file: `WeatherDemo/WeatherDemo.sln`
- Scaffolding script: `new.ps1` (PowerShell script for creating new .NET solutions)
- Git ignore: `.gitignore` (configured for .NET projects)

### Common Commands Reference

#### Repository Root Structure
```
/home/runner/work/ai-csharp/ai-csharp/
├── .git/
├── .gitignore
├── WeatherDemo/
│   ├── WeatherDemo.sln
│   ├── WeatherDemo.Library/
│   ├── WeatherDemo.Web/
│   └── WeatherDemo.Tests/
└── new.ps1
```

#### WeatherDemo Solution Structure
```
WeatherDemo/
├── WeatherDemo.sln
├── WeatherDemo.Library/
│   ├── WeatherDemo.Library.csproj
│   ├── WeatherService.cs
│   ├── WeatherReport.cs
│   ├── IRandomProvider.cs
│   └── RandomProvider.cs
├── WeatherDemo.Web/
│   ├── WeatherDemo.Web.csproj
│   ├── Program.cs
│   ├── Pages/
│   │   ├── Weather.cshtml
│   │   ├── Weather.cshtml.cs
│   │   └── ...
│   └── wwwroot/
└── WeatherDemo.Tests/
    ├── WeatherDemo.Tests.csproj
    ├── WeatherServiceTests.cs
    └── UnitTest1.cs
```

## Development Workflow

### Making Code Changes
1. Always navigate to `WeatherDemo/` directory first
2. Make your changes to the appropriate project
3. Build: `dotnet build` (12 seconds - NEVER CANCEL)
4. Test: `dotnet test` (4 seconds - NEVER CANCEL)  
5. Format: `dotnet format`
6. Manually test web app functionality
7. Commit changes

### Expected Build Output
- Build should complete with warnings but no errors
- Common warnings include nullable reference type warnings (this is normal)
- Tests should show: "Test summary: total: 5, failed: 0, succeeded: 5, skipped: 0"

### Troubleshooting
- If NETSDK1045 error occurs: Install .NET 9.0 SDK and ensure PATH includes `$HOME/.dotnet`
- If build fails: Run `dotnet clean` then `dotnet restore` then `dotnet build`
- If tests fail: Check that WeatherService logic hasn't been broken
- If web app doesn't start: Check that port 5219 is available

## Time Expectations
- **NEVER CANCEL** any of these operations before the specified times:
- Package restore: 11 seconds (set 30+ minute timeout)
- Build: 12 seconds (set 30+ minute timeout)  
- Test execution: 4 seconds (set 15+ minute timeout)
- Clean: 1 second
- Web app startup: 3-5 seconds
- .NET 9.0 SDK download/install: 2-10 minutes (set 15+ minute timeout)

## Validation Scenarios
After making changes, ALWAYS test these scenarios:

1. **Basic Build Test**: `dotnet build` succeeds without errors
2. **Unit Test Validation**: `dotnet test` shows all 5 tests passing
3. **Web Application Test**: 
   - Start with `dotnet run`
   - Verify `curl http://localhost:5219/Weather` returns HTML with weather data table
   - Should show 5 different cities with temperature data
4. **Code Quality Check**: `dotnet format --verify-no-changes` passes (exit code 0)

## Critical Notes
- This repository REQUIRES .NET 9.0 - .NET 8.0 will not work
- Build times are fast (under 15 seconds) - longer times indicate a problem
- All 5 unit tests should always pass
- Web application must display weather data for validation
- Code formatting is enforced - always run `dotnet format` before committing