using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Stl.CommandR;
using Stl.Fusion;
using Stl.Fusion.Extensions;
using Time.Data;
using Time.Repository;
using Time.Service;
using Time.TimeDbcontext;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();


        builder.Services.AddServerSideBlazor();
        builder.Services.AddDbContext<TimerContext>(options =>
                   options.UseNpgsql("Server=localhost;Database=TimeDB;Username=postgres;Password=2244;"));

        builder.Services.AddScoped<TimerRepository>();

        var fusion = builder.Services.AddFusion();
        fusion.AddFusionTime();
        fusion.AddService<TimerServices>();

        builder.Services.AddCommander();

        builder.Services.AddScoped<TimerServices>();

        builder.Services.AddSignalR();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();

        app.MapBlazorHub();
        app.MapFallbackToPage("/_Host");
        app.MapHub<TimerHub>("/timerHub"); // SignalR hub yo'lini qo'shish

        app.Run();
    }
}