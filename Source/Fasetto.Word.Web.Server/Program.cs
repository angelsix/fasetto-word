using Dna;
using Dna.AspNet;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Fasetto.Word.Web.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //
            // NOTE: If you get the following error...
            //
            //     System.IO.IOException: 'Failed to bind to address http://localhost:5000.'
            //
            // Then it means you have something already listening on that port
            // so type `cmd` into the start menu to find Command Prompt
            // and hold Shift + Ctrl when clicking on it to Run as Admin
            //
            // Then type `netstat -a -b` to see if/what is locking that port
            // then fix that application to listen on another port, or change
            // your port
            //
            CreateBuildWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateBuildWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder()
                // Add Dna Framework
                .UseDnaFramework(construct =>
                {
                    // Configure framework

                    // Add file logger
                    construct.AddFileLogger();
                })
                .UseStartup<Startup>();
        }
    }
}
