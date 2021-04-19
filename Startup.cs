using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
        StaticConfig = Configuration;
    }

    public IConfiguration Configuration { get; }
    public static IConfiguration StaticConfig { get; private set; }
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddRazorPages().WithRazorPagesRoot("/"); ;
    }
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorPages();
            SampleApis.Map(endpoints);
        });
    }
}