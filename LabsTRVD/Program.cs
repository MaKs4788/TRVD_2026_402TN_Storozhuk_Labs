using LabsTRVD.Components;
using LabsTRVD.Data;
using LabsTRVD.Repositories;
using LabsTRVD.Repositories.Interfaces;
using LabsTRVD.Services;
using LabsTRVD.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using LabsTRVD.Mapping;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=app.db"));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();
builder.Services.AddScoped<IBudgetService, BudgetService>();
builder.Services.AddScoped<IExpenseService, ExpenseService>();
builder.Services.AddScoped<IIncomeService, IncomeService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "LabsTRVD API V1");
    c.RoutePrefix = "swagger";
});

// Configure middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // Переконайся, що база створена
    db.Database.EnsureCreated();

    // Додаємо юзера лише якщо його ще немає
    if (!db.Users.Any(u => u.UserId == Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6")))
    {
        var user = new LabsTRVD.Entities.User
        {
            UserId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
            Email = "test@example.com",
            PasswordHash = "hash123"
        };
        db.Users.Add(user);
        db.SaveChanges();
    }
}
// ----------------------


app.Run();