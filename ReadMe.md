# Hello .NET Core
A sample .NET Core 5 web app with route-to-code APIs and Razore pages.

# Restore steps
1- Restore the packages `dotnet restore`. 
2- Place your SQL Database connection string in `DefaultConnection` key in appsettings.json.   
3- Run this script to create the demo table:  
```
CREATE TABLE Users (
	Id int IDENTITY(0,1) NOT NULL,
	Name nvarchar(250) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT NewTable_PK PRIMARY KEY (Id)
);
```
4- Run the project `dotnet run`. 
5- Publish later with the command `dotnet publish`  

---
# Presentation
https://docs.google.com/presentation/d/1bzaXPHi-7njsMb52jSfOZqCrag1pt-bJzdbFMhV8mlQ/edit?usp=sharing

# Files description

## appsettings.json

Contains configuration data, like connection strings.  
https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-5.0

## Program.cs

Contains the entry point for the app.  
https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host?view=aspnetcore-5.0

## Startup.cs

Contains code that configures app behavior.  
https://docs.microsoft.com/en-us/aspnet/core/fundamentals/startup?view=aspnetcore-5.0

## core.csproj

Project file that contain references to packages.  
https://docs.microsoft.com/en-us/dotnet/core/project-sdk/overview

## index.cshtml

A simple Razor page file.  
https://www.learnrazorpages.com/

## SampleApis.cs

A simple route-to-code API class.  
https://docs.microsoft.com/en-us/aspnet/core/web-api/route-to-code?view=aspnetcore-5.0
