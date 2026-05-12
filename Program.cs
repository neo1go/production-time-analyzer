using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProductionTimeAnalyzer.Data;
using ProductionTimeAnalyzer.Services;
using System.Net.NetworkInformation;
using System.Security.Principal;
using System.Security;



namespace ProductionTimeAnalyzer
{
    public class Program
    {
        public static void Main(string[] args)
        {

          


            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddDbContext<ProductionTimeAnalyzerContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<ProductionTimeAnalyzerContext>();



            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<TimeAnalysisService>();


            builder.Services.AddScoped<ProductionInsightAgent>();

            // ✅ HttpClient für den Agenten
            builder.Services.AddHttpClient();


            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            var app = builder.Build();


            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            



            // Datenbank initialisieren, CreateScpoe erstellt ProductionTimeAnalyzerContext neu und dann wird es durch 
            // using auch direkt wieder disposed.
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ProductionTimeAnalyzerContext>();


                



                db.Database.Migrate();

                SeedData.Initialize(db);
            }

            app.Run();
        }
    }
}
