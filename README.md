# Settlements

Web app for crud access to settlements stored in SQL Server. Utilizes Blazor for frontend and ASP NET Core for backend. Validation is performed on client and server using Fluent Validation and api has Swagger generated documentation.

## Running web app

To run app needs to connect to SQL Server, connection string for connecting is in '.\src\Settlements.Server\appsettings.json' file.
Database can be created from BACPAC , or by running:

    dotnet ef database update --project .\src\Settlements.Server\

Server can be started with command:

    dotnet run --project .\src\Settlements.Server\

from the solution directory.

Swagger documentation is located at '/swagger/index.html'.
