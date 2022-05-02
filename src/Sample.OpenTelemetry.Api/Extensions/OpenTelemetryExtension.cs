﻿using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Sample.OpenTelemetry.Api.Configs;

namespace Sample.OpenTelemetry.Api.Extensions;

public static class OpenTelemetryExtension
{
    public static void AddOpenTelemetry(this IServiceCollection services, AppSettings appSettings)
    {
        services.AddOpenTelemetryTracing(telemetry =>
        {
            var resourceBuilder = ResourceBuilder
                .CreateDefault()
                .AddService(appSettings?.DistributedTracing?.Jaeger?.ServiceName);

            telemetry
                .SetResourceBuilder(resourceBuilder)
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .SetSampler(new AlwaysOnSampler())
                .AddJaegerExporter(jaegerOptions =>
                {
                    jaegerOptions.AgentHost = appSettings?.DistributedTracing?.Jaeger?.Host;
                    jaegerOptions.AgentPort = appSettings?.DistributedTracing?.Jaeger?.Port ?? 0;
                });
        });
    }
}
