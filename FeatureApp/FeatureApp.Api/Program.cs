namespace FeatureApp.Api
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;

    public class Program
    {
        /// <summary>
        /// Main method to start application.
        /// </summary>
        /// <param name="args">Arguments to inject to application.</param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Creates host builder of .NET Core application.
        /// </summary>
        /// <param name="args">Arguments to inject to application.</param>
        /// <returns>The .NET Core host builder.</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
