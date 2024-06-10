using NanikaAPI.Authorization;
using NanikaAPI.Models;
using NanikaAPI.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAuthorization, Authorization>();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

/*
RngCalculator<string> test = new(new List<RngSample<string>>() { 
    new("Common", 500000, 0),
    new("Uncommon", 320000, 0),
    new("Rare", 100000, 0),
    new("Epic", 55000, 0.1),
    new("Exotic", 22000, 0.25),
    new("Legendary", 3000, 1.0)
}, true);
int i = 0;
for (; test.Pull().Value != "Legendary"; i++) Console.WriteLine(test[0].Probability/test.Ceil*100.0);
Console.WriteLine(i);*/