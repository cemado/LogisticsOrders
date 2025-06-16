using LogisticsOrders.Infrastructure.Data;
using LogisticsOrders.Infrastructure.Repositories;
using LogisticsOrders.Application.Orders;
using LogisticsOrders.Application.Reports;
using LogisticsOrders.Infrastructure.Reports;
using LogisticsOrders.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using LogisticsOrders.Application.Interfaces;
using LogisticsOrders.Application.Services;
using LogisticsOrders.Infrastructure.Services;
using LogisticsOrders.Application.Dashboard;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configura DbContext con la cadena de conexión de appsettings.json
builder.Services.AddDbContext<LogisticsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositorios
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderAuditRepository, OrderAuditRepository>();

// Servicios de aplicación y handlers
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<CreateOrderHandler>();
builder.Services.AddScoped<EditOrderHandler>();
builder.Services.AddScoped<DeleteOrderHandler>();
builder.Services.AddScoped<OrdersByClientReportService>();
builder.Services.AddScoped<OrdersByDistanceIntervalReportService>();

// Servicio de generación de Excel
builder.Services.AddScoped<ExcelReportGenerator>();

// Servicio de integración con APIs externas
builder.Services.AddHttpClient<IExternalGeoApiService, ExternalGeoApiService>();

// Servicio de dashboard
builder.Services.AddScoped<DashboardService>();

// Razor Pages
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages();

app.Run();
