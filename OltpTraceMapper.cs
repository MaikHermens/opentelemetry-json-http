using System.Diagnostics;

namespace MaikHermens.Opentelemetry.exporter.HttpJson
{
    internal static class OltpTraceMapper
    {
        public static OltpTraceModel Map(Activity activity)
        {
            var traceModel = new OltpTraceModel();

            // Create resource span
            var resourceSpan = new OTLPResourceSpan
            {
                Resource = CreateResource(activity),
                ScopeSpans =
                [
                    new() {
                        Scope = CreateScope(activity),
                        Spans = new List<OTLPSpan> { CreateSpan(activity) }
                    }
                ]
            };

            traceModel.ResourceSpans.Add(resourceSpan);
            return traceModel;
        }

        private static OTLPResource CreateResource(Activity activity)
        {
            var attributes = new List<OTLPAttribute>();

            // Add service name if available
            if (activity.Source.Name != null)
            {
                attributes.Add(new OTLPAttribute
                {
                    Key = "service.name",
                    Value = new OTLPAttributeValue { StringValue = activity.Source.Name }
                });
            }

            // Add service version if available
            if (activity.Source.Version != null)
            {
                attributes.Add(new OTLPAttribute
                {
                    Key = "service.version",
                    Value = new OTLPAttributeValue { StringValue = activity.Source.Version }
                });
            }

            return new OTLPResource { Attributes = attributes.Count > 0 ? attributes : null };
        }

        private static OTLPScope CreateScope(Activity activity)
        {
            return new OTLPScope
            {
                Name = activity.Source.Name ?? "unknown",
                Version = activity.Source.Version
            };
        }

        private static OTLPSpan CreateSpan(Activity activity)
        {
            var span = new OTLPSpan
            {
                TraceId = activity.TraceId.ToString(),
                SpanId = activity.SpanId.ToString(),
                ParentSpanId = activity.ParentSpanId != default ? activity.ParentSpanId.ToString() : null,
                Name = activity.DisplayName ?? activity.OperationName,
                Kind = MapSpanKind(activity.Kind),
                StartTimeUnixNano = ToUnixTimeNanoseconds(activity.StartTimeUtc).ToString(),
                EndTimeUnixNano = activity.Duration != TimeSpan.Zero
                    ? ToUnixTimeNanoseconds(activity.StartTimeUtc.Add(activity.Duration)).ToString()
                    : null,
                Attributes = MapAttributes(activity),
                Status = MapStatus(activity)
            };

            return span;
        }

        private static long ToUnixTimeNanoseconds(DateTime dateTime)
        {
            var timeSpan = dateTime.ToUniversalTime() - DateTime.UnixEpoch;
            return timeSpan.Ticks * 100; // Convert ticks to nanoseconds
        }

        private static int MapSpanKind(ActivityKind kind)
        {
            return kind switch
            {
                ActivityKind.Internal => 1,
                ActivityKind.Server => 2,
                ActivityKind.Client => 3,
                ActivityKind.Producer => 4,
                ActivityKind.Consumer => 5,
                _ => 0 // SPAN_KIND_UNSPECIFIED
            };
        }

        private static List<OTLPAttribute>? MapAttributes(Activity activity)
        {
            var attributes = new List<OTLPAttribute>();

            foreach (var tag in activity.Tags)
            {
                attributes.Add(new OTLPAttribute
                {
                    Key = tag.Key,
                    Value = new OTLPAttributeValue { StringValue = tag.Value }
                });
            }

            foreach (var baggage in activity.Baggage)
            {
                attributes.Add(new OTLPAttribute
                {
                    Key = baggage.Key,
                    Value = new OTLPAttributeValue { StringValue = baggage.Value }
                });
            }

            return attributes.Count > 0 ? attributes : null;
        }

        private static OTLPSpanStatus? MapStatus(Activity activity)
        {
            if (activity.Status == ActivityStatusCode.Error)
            {
                return new OTLPSpanStatus
                {
                    Code = 2, // STATUS_CODE_ERROR
                    Message = activity.StatusDescription
                };
            }
            else if (activity.Status == ActivityStatusCode.Ok)
            {
                return new OTLPSpanStatus
                {
                    Code = 1, // STATUS_CODE_OK
                    Message = activity.StatusDescription
                };
            }
            return null;
        }
    }
}
