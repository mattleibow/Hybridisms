// This file is generated from the Aspire AppHost project. Rerun the Aspire AppHost
// to regenerate it.

public static class AspireAppSettings
{
    public static readonly Dictionary<string, string> Settings =
        new Dictionary<string, string>
        {
            ["DOTNET_ENVIRONMENT"] = "Development",
            ["DOTNET_LAUNCH_PROFILE"] = "Hybridisms.Client.NativeApp.ClientStub",
            ["DOTNET_SYSTEM_CONSOLE_ALLOW_ANSI_COLOR_REDIRECTION"] = "true",
            ["ENVIRONMENT"] = "Development",
            ["LAUNCH_PROFILE"] = "Hybridisms.Client.NativeApp.ClientStub",
            ["LOGGING:CONSOLE:FORMATTERNAME"] = "simple",
            ["OTEL_BLRP_SCHEDULE_DELAY"] = "1000",
            ["OTEL_BSP_SCHEDULE_DELAY"] = "1000",
            ["OTEL_DOTNET_EXPERIMENTAL_ASPNETCORE_DISABLE_URL_QUERY_REDACTION"] = "true",
            ["OTEL_DOTNET_EXPERIMENTAL_HTTPCLIENT_DISABLE_URL_QUERY_REDACTION"] = "true",
            ["OTEL_DOTNET_EXPERIMENTAL_OTLP_EMIT_EVENT_LOG_ATTRIBUTES"] = "true",
            ["OTEL_DOTNET_EXPERIMENTAL_OTLP_EMIT_EXCEPTION_LOG_ATTRIBUTES"] = "true",
            ["OTEL_DOTNET_EXPERIMENTAL_OTLP_RETRY"] = "in_memory",
            ["OTEL_EXPORTER_OTLP_ENDPOINT"] = "https://localhost:21075",
            ["OTEL_EXPORTER_OTLP_HEADERS"] = "x-otlp-api-key=f1eccfb4b047496a148246226912143e",
            ["OTEL_EXPORTER_OTLP_PROTOCOL"] = "grpc",
            ["OTEL_METRIC_EXPORT_INTERVAL"] = "1000",
            ["OTEL_METRICS_EXEMPLAR_FILTER"] = "trace_based",
            ["OTEL_RESOURCE_ATTRIBUTES"] = "service.instance.id=vrvavtrd",
            ["OTEL_SERVICE_NAME"] = "mobile",
            ["OTEL_TRACES_SAMPLER"] = "always_on",
            ["services:webapp:http:0"] = "http://localhost:5118",
            ["services:webapp:https:0"] = "https://localhost:7201",
            ["SYSTEM_CONSOLE_ALLOW_ANSI_COLOR_REDIRECTION"] = "true",
        };
}