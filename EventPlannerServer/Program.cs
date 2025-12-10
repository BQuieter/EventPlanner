using EventPlannerServer.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<EventPlannerDbContext>(options => options.UseSqlServer(connection));
var app = builder.Build();

app.MapGet("/", (EventPlannerDbContext context) =>  $"{context.EventImportances.ToList<EventImportance>().Count}");

app.Run();
