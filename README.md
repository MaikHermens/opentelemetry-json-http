# ActivityJsonMapper - OpenTelemetry Activity to OTLP JSON Mapper

This project provides a mapper to convert .NET `Activity` objects to OTLP (OpenTelemetry Protocol) JSON format for HTTP export.

## Features

- **Complete OTLP Mapping**: Maps .NET Activity objects to proper OTLP JSON structure
- **Resource Support**: Handles OpenTelemetry Resource information
- **Batch Processing**: Supports both single activity and batch conversion
- **Type Safety**: Properly handles different attribute value types
- **Standards Compliant**: Follows OTLP JSON specification

## Classes

### ActivityJsonMapper

The main mapper class that provides static methods for converting Activity objects to OTLP format.


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
using var activity = activitySource.StartActivity("ProcessRequest");
if (activity != null)
{
    activity.SetTag("http.method", "GET");
    activity.SetTag("http.status_code", 200);
    
    // Map to OTLP format
    var otlpTrace = ActivityJsonMapper.MapActivityToOtlpTrace(activity);
    
    // Serialize to JSON
    var json = JsonSerializer.Serialize(otlpTrace, new JsonSerializerOptions 
    { 
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
    });
}
```