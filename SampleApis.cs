using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

class User
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public static class SampleApis
{
    public static void Map(IEndpointRouteBuilder endpoints, ILogger logger, IConfiguration config)
    {
        var db = new DataAccess(config.GetConnectionString("DefaultConnection"));

        endpoints.MapGet("/users", async context =>
        {
            logger.Log(LogLevel.Warning, "Test Log");
            var data = db.GetJson(@"
            SELECT 
            JSON_QUERY((
                SELECT Id AS [id], 
                Name AS [name] 
                FROM Users 
                FOR JSON PATH
            )) AS [usersList] 
            FOR JSON PATH, WITHOUT_ARRAY_WRAPPER",
            new List<DbParameter> { }, commandType: CommandType.Text);
            await context.Response.WriteAsync(data);
        });

        endpoints.MapGet("/users/{id:int}", async context =>
        {
            var userId = int.Parse(context.Request.RouteValues["id"].ToString());

            var data = db.GetJson(@"
            SELECT 
            JSON_QUERY((
                SELECT Id AS [id], 
                Name AS [name] 
                FROM Users 
                WHERE Id = @userid 
                FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
            )) AS [user] 
            FOR JSON PATH, WITHOUT_ARRAY_WRAPPER",
            new List<DbParameter> {
                db.GetParameter("userid", userId)
            }, commandType: CommandType.Text);
            await context.Response.WriteAsync(data);
        });

        endpoints.MapPost("/users", async context =>
        {
            if (!context.Request.HasJsonContentType())
            {
                context.Response.StatusCode = StatusCodes.Status415UnsupportedMediaType;
                return;
            }

            var user = await context.Request.ReadFromJsonAsync<User>();

            var data = db.GetJson(@"
            DECLARE @tbl TABLE (id INT);

            INSERT INTO Users (Name) OUTPUT inserted.Id INTO @tbl SELECT @username;

            SELECT 
            JSON_QUERY((
                SELECT Id AS [id], 
                Name AS [name] 
                FROM Users 
                WHERE Id = (SELECT TOP(1) id FROM @tbl) 
                FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
            )) AS [user] 
            FOR JSON PATH, WITHOUT_ARRAY_WRAPPER",
            new List<DbParameter> {
                db.GetParameter("username", user.Name)
            }, commandType: CommandType.Text);

            await context.Response.WriteAsync(data);
        });

        endpoints.MapPut("/users/{id:int}", async context =>
        {
            var userId = int.Parse(context.Request.RouteValues["id"].ToString());
            if (!context.Request.HasJsonContentType())
            {
                context.Response.StatusCode = StatusCodes.Status415UnsupportedMediaType;
                return;
            }

            var user = await context.Request.ReadFromJsonAsync<User>();

            var data = db.GetJson(@"
            UPDATE Users SET Name = @username WHERE Id = @userid;

            SELECT 
            JSON_QUERY((
                SELECT Id AS [id], 
                Name AS [name] 
                FROM Users 
                WHERE Id = @userid
                FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
            )) AS [user] 
            FOR JSON PATH, WITHOUT_ARRAY_WRAPPER",
            new List<DbParameter> {
                db.GetParameter("userid", userId),
                db.GetParameter("username", user.Name)
            }, commandType: CommandType.Text);

            await context.Response.WriteAsync(data);
        });

        endpoints.MapDelete("/users/{id:int}", async context =>
        {
            var userId = int.Parse(context.Request.RouteValues["id"].ToString());

            var data = db.GetJson(@"
            DELETE FROM Users WHERE Id = @userid;

            SELECT 
            JSON_QUERY((
                SELECT Id AS [id], 
                Name AS [name] 
                FROM Users 
                FOR JSON PATH
            )) AS [usersList] 
            FOR JSON PATH, WITHOUT_ARRAY_WRAPPER",
            new List<DbParameter> {
                db.GetParameter("userid", userId)
            }, commandType: CommandType.Text);

            await context.Response.WriteAsync(data);
        });
    }
}