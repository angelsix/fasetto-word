using Fasetto.Word.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Fasetto.Word.Web.Server
{
    /// <summary>
    /// Extension methods for any SendGrid classes
    /// </summary>
    public static class SendGridExtensions
    {
        /// <summary>
        /// Injects the <see cref="SendGridEmailSender"/> into the services to handle the 
        /// <see cref="IEmailSender"/> service
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSendGridEmailSender(this IServiceCollection services)
        {
            // Inject the SendGridEmailSender
            services.AddTransient<IEmailSender, SendGridEmailSender>();

            // Return collection for chaining
            return services;
        }
    }
}
