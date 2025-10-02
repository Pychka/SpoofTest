using Microsoft.EntityFrameworkCore;
using SpoofTestApi.Entities;
using SpoofTestApi.Services;
using SpoofTestApi.Services.Realization;

namespace SpoofTestApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions { WebRootPath = "Pages" });

        builder.Services.AddControllers();
        builder.Services.AddDbContext<SpoofTestContext>(s => s.UseLazyLoadingProxies().UseSqlServer("DefaultConnection"));
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });
        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseHttpsRedirection();
        app.UseCors("AllowAll");
        app.UseAuthorization();
        app.UseStaticFiles();
        app.MapControllers();

        app.Run();
    }
}
