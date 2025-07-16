#Requires -Version 5.1
<#
.SYNOPSIS
    Creates a new .NET solution with a standard project structure.

.DESCRIPTION
    This script automates the creation of a .NET solution. It scaffolds a new solution file,
    a class library, an ASP.NET Core web application, and an xUnit test project based on a
    provided root name. It then adds the projects to the solution and sets up the necessary

    project references.

.PARAMETER RootName
    The base name to use for the solution and projects.

.EXAMPLE
    PS C:\dev> .\New-DotnetSolution.ps1 -RootName "WeatherDemo"

    This command will create the following structure:
    - ./WeatherDemo/WeatherDemo.sln
    - ./WeatherDemo/WeatherDemo.Library/WeatherDemo.Library.csproj
    - ./WeatherDemo/WeatherDemo.Web/WeatherDemo.Web.csproj
    - ./WeatherDemo/WeatherDemo.Tests/WeatherDemo.Tests.csproj

    It will also configure the following project references:
    - WeatherDemo.Web -> WeatherDemo.Library
    - WeatherDemo.Tests -> WeatherDemo.Library
#>
[CmdletBinding()]
param (
    [Parameter(Mandatory = $true, HelpMessage = "The base name for the solution and projects.")]
    [string]$RootName
)

# --- Define Project and Solution Names ---
$SolutionName = $RootName
$LibraryProjectName = "$($RootName).Library"
$WebAppProjectName = "$($RootName).Web"
$TestProjectName = "$($RootName).Tests"

# --- Define Directory Paths ---
$SolutionFolder = ".\$($SolutionName)"
$LibraryProjectPath = Join-Path $SolutionFolder $LibraryProjectName
$WebAppProjectPath = Join-Path $SolutionFolder $WebAppProjectName
$TestProjectPath = Join-Path $SolutionFolder $TestProjectName
$SolutionPath = Join-Path $SolutionFolder "$($SolutionName).sln"

# --- Script Execution ---
try {
    Write-Host "Starting .NET solution setup for '$RootName'..."

    # Check if dotnet CLI is available
    if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) {
        throw "The .NET SDK (dotnet CLI) is not installed or not found in your PATH. Please install it to continue."
    }

    # Create the root directory for the solution
    if (-not (Test-Path -Path $SolutionFolder)) {
        Write-Host "Creating solution directory: '$SolutionFolder'"
        New-Item -ItemType Directory -Path $SolutionFolder | Out-Null
    } else {
        Write-Warning "Directory '$SolutionFolder' already exists. Files may be overwritten."
    }

    # Set location to the new directory
    Set-Location $SolutionFolder

    # 1. Create the Solution File
    Write-Host "Creating solution file..."
    dotnet new sln --name $SolutionName

    # 2. Create the Class Library
    Write-Host "Creating class library: '$LibraryProjectName'"
    dotnet new classlib --name $LibraryProjectName

    # 3. Create the ASP.NET Web App
    Write-Host "Creating ASP.NET web app: '$WebAppProjectName'"
    dotnet new webapp --name $WebAppProjectName

    # 4. Create the xUnit Test Project
    Write-Host "Creating xUnit test project: '$TestProjectName'"
    dotnet new xunit --name $TestProjectName

    # 5. Add all projects to the Solution
    Write-Host "Adding projects to solution..."
    dotnet sln add $LibraryProjectName
    dotnet sln add $WebAppProjectName
    dotnet sln add $TestProjectName

    # 6. Add Project References
    Write-Host "Adding project references..."
    # Web App -> Class Library
    Write-Host "- Adding '$LibraryProjectName' reference to '$WebAppProjectName'"
    dotnet add (Join-Path . $WebAppProjectName) reference (Join-Path . $LibraryProjectName)

    # Test Project -> Class Library
    Write-Host "- Adding '$LibraryProjectName' reference to '$TestProjectName'"
    dotnet add (Join-Path . $TestProjectName) reference (Join-Path . $LibraryProjectName)

    Write-Host -ForegroundColor Green "✅ Solution '$RootName' created successfully!"
    Write-Host "Your new solution is located at: '$(Get-Location)'"

}
catch {
    Write-Error "An error occurred during script execution: $_"
}
finally {
    # Return to the original directory
    Pop-Location
}
