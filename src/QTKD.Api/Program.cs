using Microsoft.EntityFrameworkCore;
using QTKD.Api.Data;
using QTKD.Api.Middleware;
using QTKD.Api.Repositories;
using QTKD.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<QtkdDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("QtkdDb")));

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IContractRepository, ContractRepository>();
builder.Services.AddScoped<IPriceListRepository, PriceListRepository>();

builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IContractService, ContractService>();
builder.Services.AddScoped<IPriceListService, PriceListService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ApiExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();
}

app.MapControllers();

if (!EF.IsDesignTime && !app.Environment.IsEnvironment("Testing"))
{
    using IServiceScope scope = app.Services.CreateScope();
    QtkdDbContext db = scope.ServiceProvider.GetRequiredService<QtkdDbContext>();
    ILogger logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("QtkdDbSeeder");

    try
    {
        await QtkdDbSeeder.SeedAsync(db);
    }
    catch (Exception ex)
    {
        logger.LogError(
            ex,
            "Không thể khởi tạo dữ liệu mẫu. Hãy kiểm tra User Secrets (ConnectionStrings:QtkdDb) và chạy 'dotnet ef database update' trước.");
        throw;
    }
}

app.Lifetime.ApplicationStarted.Register(() =>
{
    string? address = app.Urls.FirstOrDefault(url =>
        url.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
        ?? app.Urls.FirstOrDefault();

    if (address == null) return;

    address = address.TrimEnd('/');
    if (app.Environment.IsDevelopment())
    {
        Console.WriteLine($"Swagger UI : {address}/swagger");
    }

    Console.WriteLine($"Customers  : {address}/api/customers");
    Console.WriteLine($"Contract #1: {address}/api/contracts/1");
    Console.WriteLine($"PriceLists : {address}/api/pricelists");
});

app.Run();

public partial class Program
{
}
