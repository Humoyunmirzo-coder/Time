using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Time.TimeDbcontext;
using Time.Service;


namespace Time.ConfigureService
{
    public class Startup
    {

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<TimerService>();
        
            services.AddSignalR();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapHub<TimerHub>("/timerHub");
                endpoints.MapFallbackToPage("/_Host");
            });
        }

    }
}
