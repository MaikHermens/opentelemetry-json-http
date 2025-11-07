using System.Text.Json.Serialization;

namespace MaikHermens.Opentelemetry.exporter.HttpJson
{
    internal class OltpTraceModel
    {
        [JsonPropertyName("resourceSpans")]
        public List<OTLPResourceSpan> ResourceSpans { get; set; } = new();
    }

    internal class OTLPAttribute
    {
        [JsonPropertyName("key")]
        public string Key { get; set; } = string.Empty;

        [JsonPropertyName("value")]
        public OTLPAttributeValue Value { get; set; } = new();
    }

    internal class OTLPAttributeValue
    {
        [JsonPropertyName("stringValue")]
        public string? StringValue { get; set; }

        [JsonPropertyName("intValue")]
        public string? IntValue { get; set; }

        [JsonPropertyName("doubleValue")]
        public double? DoubleValue { get; set; }

        [JsonPropertyName("boolValue")]
        public bool? BoolValue { get; set; }

        [JsonPropertyName("arrayValue")]
        public OTLPArrayValue? ArrayValue { get; set; }

        [JsonPropertyName("kvlistValue")]
        public OTLPKvListValue? KvListValue { get; set; }
    }

    internal class OTLPArrayValue
    {
        [JsonPropertyName("values")]
        public List<object> Values { get; set; } = new();
    }

    internal class OTLPKvListValue
    {
        [JsonPropertyName("values")]
        public List<OTLPAttribute> Values { get; set; } = new();
    }

    internal class OTLPSpan
    {
        [JsonPropertyName("traceId")]
        public string TraceId { get; set; } = string.Empty;

        [JsonPropertyName("spanId")]
        public string SpanId { get; set; } = string.Empty;

        [JsonPropertyName("parentSpanId")]
        public string? ParentSpanId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("kind")]
        public int Kind { get; set; }

        [JsonPropertyName("startTimeUnixNano")]
        public string StartTimeUnixNano { get; set; } = string.Empty;

        [JsonPropertyName("endTimeUnixNano")]
        public string? EndTimeUnixNano { get; set; }

        [JsonPropertyName("attributes")]
        public List<OTLPAttribute>? Attributes { get; set; }

        [JsonPropertyName("status")]
        public OTLPSpanStatus? Status { get; set; }
    }

    internal class OTLPSpanStatus
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }
    }

    internal class OTLPScopeSpan
    {
        [JsonPropertyName("scope")]
        public OTLPScope? Scope { get; set; }

        [JsonPropertyName("spans")]
        public List<OTLPSpan> Spans { get; set; } = new();
    }

    internal class OTLPScope
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("version")]
        public string? Version { get; set; }
    }

    internal class OTLPResourceSpan
    {
        [JsonPropertyName("resource")]
        public OTLPResource? Resource { get; set; }

        [JsonPropertyName("scopeSpans")]
        public List<OTLPScopeSpan> ScopeSpans { get; set; } = new();
    }

    internal class OTLPResource
    {
        [JsonPropertyName("attributes")]
        public List<OTLPAttribute>? Attributes { get; set; }
    }
}
