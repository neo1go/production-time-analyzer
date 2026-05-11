using Microsoft.EntityFrameworkCore;
using ProductionTimeAnalyzer.Data;
using ProductionTimeAnalyzer.Services;
using System.Net.NetworkInformation;

namespace ProductionTimeAnalyzer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")));



            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<TimeAnalysisService>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();


            // Datenbank initialisieren, CreateScpoe erstellt AppDbContext neu und dann wird es durch 
            // using auch direkt wieder disposed.
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                SeedData.Initialize(db);
            }

            app.Run();
        }
    }
}
