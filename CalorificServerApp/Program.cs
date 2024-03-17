using CalorificServerApp.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using SQLitePCL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
//Add Database class and FoodService class
builder.Services.AddDbContext<AppDatabase>();
builder.Services.AddScoped<FoodService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<AppDatabase>();
    var foodService = services.GetRequiredService<FoodService>();

    // Ensure database is created
    dbContext.Database.EnsureCreated();

    // Populate food items if the table is empty
    if (!dbContext.FoodItems.Any())
    {
        await foodService.PopulateFoodItems();
    }
}

app.Run();
