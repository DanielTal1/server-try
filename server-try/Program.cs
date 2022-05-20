using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using server_try.Data;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<server_tryContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("server_tryContext") ?? throw new InvalidOperationException("Connection string 'server_tryContext' not found.")));

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("Allow All",
        builder =>
        {
            builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseCors("Allow All");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
