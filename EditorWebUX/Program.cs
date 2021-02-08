using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EditorWebUX
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var env = ReadEnvironment(args).Build();

            var host = ConfigureWebHost(env).Build();

            host.Run();
        }

        private static IConfigurationBuilder ReadEnvironment(string[] args)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();

            builder.AddEnvironmentVariables();

            if (args != null)
            {
                builder.AddCommandLine(args);
            }

            var basicConfig = builder.Build();

            var environmentName = basicConfig.GetValue<string>("ASPNETCORE_ENVIRONMENT")
                ?? basicConfig.GetValue<string>("DOTNET_ENVIRONMENT");

            builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
                .AddConfiguration(basicConfig);

            return builder;
        }

        private static IWebHostBuilder ConfigureWebHost(IConfigurationRoot env)
        {
            var builder = new WebHostBuilder();

            builder.UseConfiguration(env);

            builder.UseContentRoot(Directory.GetCurrentDirectory());

            builder.ConfigureLogging(loggingBuilder =>
            {
                loggingBuilder.AddConfiguration(env.GetSection("logging"));
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();
                loggingBuilder.AddEventSourceLogger();
            });

            builder.UseDefaultServiceProvider((context, options) =>
            {
                options.ValidateScopes = context.HostingEnvironment.IsDevelopment();
            });

            builder.UseKestrel();
            builder.UseStaticWebAssets();
            builder.UseIISIntegration();

            builder.ConfigureServices(services =>
            {
                services.AddControllersWithViews();

                // In production, the React files will be served from this directory
                services.AddSpaStaticFiles(configuration =>
                {
                    configuration.RootPath = "ClientApp/build";
                });
            });

            builder.Configure(ConfigureStartup);

            return builder;
        }

        private static void ConfigureStartup(WebHostBuilderContext context, IApplicationBuilder app)
        {
            var isDevelopment = context.HostingEnvironment.IsDevelopment();

            if (isDevelopment)
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (isDevelopment)
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
