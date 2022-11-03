// See https://aka.ms/new-console-template for more information

var builder = WebApplication.CreateBuilder();

var app = builder.Build();

app.MapGet("/test", () => "Hello Radek");

app.Run();