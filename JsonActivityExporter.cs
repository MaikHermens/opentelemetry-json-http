using OpenTelemetry;
using OpenTelemetry.Exporter;
using Opentelemetry_json_http_exporter;
using System.Diagnostics;
using System.Text.Json;

namespace MaikHermens.Opentelemetry.exporter.HttpJson;

/// <summary>
/// This is an http json exporter for OpenTelemetry.
/// The difference from regular oltp exporter is that it generates JSON instead of protobuf for easy parsing.
/// Mainly used for debugging. And not suggested in production.
/// </summary>
internal class JsonActivityExporter(OtlpExporterOptions options, OltpJsonHttpExportClient oltpJsonHTTPExportClient) : OtlpTraceExporter(options)
{
    private readonly OtlpExporterOptions _options = options;
    private readonly OltpJsonHttpExportClient _exportClient = oltpJsonHTTPExportClient;


    public override ExportResult Export(in Batch<Activity> activityBatch)
    {
        // Prevents the exporter's HTTP operations from being instrumented.
        using var scope = SuppressInstrumentationScope.Begin();
        try
        {
            foreach (var activity in activityBatch)
            {
                var trace = OltpTraceMapper.Map(activity);

                var json = JsonSerializer.Serialize(trace);

                _exportClient.SendExport(json, _options);
            }

            return ExportResult.Success;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Export error: {ex.Message}");
            return ExportResult.Failure;
        }
    }
}