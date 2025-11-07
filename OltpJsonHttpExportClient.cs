using OpenTelemetry.Exporter;
using System.Net.Http.Headers;
using System.Text;

namespace Opentelemetry_json_http_exporter
{
    internal sealed class OltpJsonHttpExportClient(HttpClient httpClient)
    {
        public HttpResponseMessage SendExport(string jsonContent, OtlpExporterOptions options)
        {
            var uri = new Uri(options.Endpoint ?? new Uri("http://localhost:4318"), "/v1/traces");

            var content = new StringContent(jsonContent, Encoding.UTF8);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return httpClient.PostAsync(uri, content).Result;
        }
    }
}
