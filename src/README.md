# Dotnet Opentelemetry Exporter using the HTTP JSON format

This project provides a mapper to convert .NET `Activity` objects to OTLP (OpenTelemetry Protocol) JSON format for HTTP export.
This will allow you to send telemetry data to backends that only support OTLP over HTTP in JSON format without protobuf.

## Features

- **Complete OTLP Mapping**: Maps .NET Activity objects to proper OTLP JSON structure
- **Resource Support**: Handles OpenTelemetry Resource information
- **Batch Processing**: Supports both single activity and batch conversion
- **Type Safety**: Properly handles different attribute value types
- **Standards Compliant**: Follows OTLP JSON specification

### OTLP JSON Structure

The mapper creates the following OTLP JSON structure:

```csharp
OtlpTrace
??? ResourceSpans[]
    ??? Resource
    ?   ??? Attributes[]
    ??? ScopeSpans[]
        ??? Scope
        ?   ??? Name
        ?   ??? Version
        ?   ??? Attributes[]
        ??? Spans[]
            ??? TraceId
            ??? SpanId
            ??? ParentSpanId (optional)
            ??? Name
            ??? StartTimeUnixNano
            ??? EndTimeUnixNano
            ??? Kind
            ??? Attributes[]
```

## Usage Examples

### Basic Usage

```csharp
using MaikHermens.Opentelemetry.exporter.HttpJson;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

builder.Services.AddHttpClient();

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing =>
    {
        tracing
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(builder.Environment.ApplicationName))
            .AddAspNetCoreInstrumentation()
            .AddJsonHttpExporter(options =>
            {
                options.Endpoint = new Uri(otlpEndpoint);
            });
    });
```