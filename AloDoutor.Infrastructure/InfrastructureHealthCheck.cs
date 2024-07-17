using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AloDoutor.Infrastructure
{
    public static class InfrastructureHealthCheck
    {
        public static IServiceCollection AddHealthCheck(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                .AddSqlServer(connectionString: Environment.GetEnvironmentVariable("ConnectionString") ?? "",
                name: "Instancia do Sql Server");

            services.AddHealthChecksUI(options =>
            {
                options.SetEvaluationTimeInSeconds(5);
                options.MaximumHistoryEntriesPerEndpoint(10);
                options.AddHealthCheckEndpoint("API com Health Checks", "/health");
            })
             .AddSqlServerStorage(Environment.GetEnvironmentVariable("ConnectionString") ?? "");


            return services;
        }

        public static void ConfigureHealthCheck(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                Predicate = p => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseHealthChecksUI(options => { options.UIPath = "/dashboard"; });
        }

    }
}
