using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

class User
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public static class SampleApis
{
    public static void Map(IEndpointRouteBuilder endpoints)
    {

        var usersList = new List<User>{
            new User{ Id = 1, Name = "Mike" },
            new User{ Id = 2, Name = "Alex" },
            new User{ Id = 3, Name = "John" }
        };

        endpoints.MapGet("/users", async context =>
        {
            var name = context.Request.RouteValues["name"];
            var res = new { usersList };
            await context.Response.WriteAsJsonAsync(res);
        });

        endpoints.MapGet("/users/{id:int}", async context =>
        {
            var id = int.Parse(context.Request.RouteValues["id"].ToString());
            var user = usersList.Find(x => x.Id == id);
            await context.Response.WriteAsJsonAsync(new { user });
        });

        endpoints.MapPost("/users", async context =>
        {
            if (!context.Request.HasJsonContentType())
            {
                context.Response.StatusCode = StatusCodes.Status415UnsupportedMediaType;
                return;
            }

            var user = await context.Request.ReadFromJsonAsync<User>();

            var newId = usersList[usersList.Count - 1].Id + 1;

            var newUser = new User { Name = user.Name, Id = newId };

            usersList.Add(newUser);

            await context.Response.WriteAsJsonAsync(new { user = newUser });
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

            var newId = usersList[usersList.Count - 1].Id + 1;

            var userToUpdate = usersList.Find(x => x.Id == userId);

            userToUpdate.Name = user.Name;

            await context.Response.WriteAsJsonAsync(new { user = userToUpdate });
        });

        endpoints.MapDelete("/users/{id:int}", async context =>
        {
            var userId = int.Parse(context.Request.RouteValues["id"].ToString());

            var userToDelete = usersList.Find(x => x.Id == userId);

            usersList.Remove(userToDelete);

            await context.Response.WriteAsJsonAsync(new { usersList });
        });
    }
}