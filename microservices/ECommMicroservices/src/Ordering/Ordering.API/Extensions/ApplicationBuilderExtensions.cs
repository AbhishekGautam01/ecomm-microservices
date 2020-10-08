using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Ordering.API.RabbitMQ;

namespace Ordering.API.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static EventBusRabbitMQConsumer Listner { get; set; }
        public static IApplicationBuilder UseRabbitListener(this IApplicationBuilder application)
        {
            Listner = application.ApplicationServices.GetService<EventBusRabbitMQConsumer>();
            var life = application.ApplicationServices.GetService<IHostApplicationLifetime>();

            life.ApplicationStarted.Register(OnStarted);
            life.ApplicationStopping.Register(OnStopping);

            return application;
        }

        private static void OnStarted()
        {
            Listner.Consume();
        }

        private static void OnStopping()
        {
            Listner.Disconnect();
        }
    }
}
