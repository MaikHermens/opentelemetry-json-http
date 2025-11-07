
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Trace;

namespace MaikHermens.Opentelemetry.exporter.HttpJson
{
    public static class OltpJsonExporterExtensions
    {
        /// <summary>
        /// Adds json http exporter to the TracerProvider.
        /// </summary>
        /// <param name="builder"><see cref="TracerProviderBuilder"/> builder to use.</param>
        /// <returns>The instance of <see cref="TracerProviderBuilder"/> to chain the calls.</returns>
        public static TracerProviderBuilder AddJsonHttpExporter(this TracerProviderBuilder builder)
            => builder.AddJsonHttpExporter(name: null, configure: null);

        /// <summary>
        /// Adds json http exporter to the TracerProvider.
        /// </summary>
        /// <param name="builder"><see cref="TracerProviderBuilder"/> builder to use.</param>
        /// <param name="configure">Callback action for configuring <see cref="OtlpExporterOptions"/>.</param>
        /// <returns>The instance of <see cref="TracerProviderBuilder"/> to chain the calls.</returns>
        public static TracerProviderBuilder AddJsonHttpExporter(this TracerProviderBuilder builder,
            Action<OtlpExporterOptions> configure)
            => builder.AddJsonHttpExporter(name: null, configure);

        /// <summary>
        /// Adds json http exporter to the TracerProvider.
        /// </summary>
        /// <param name="builder"><see cref="TracerProviderBuilder"/> builder to use.</param>
        /// <param name="name">Optional name which is used when retrieving options.</param>
        /// <param name="configure">Optional callback action for configuring <see cref="OtlpExporterOptions"/>.</param>
        /// <returns>The instance of <see cref="TracerProviderBuilder"/> to chain the calls.</returns>
        private static TracerProviderBuilder AddJsonHttpExporter(
            this TracerProviderBuilder builder,
            string? name,
            Action<OtlpExporterOptions>? configure)
        {
            ArgumentNullException.ThrowIfNull(builder);

            name ??= Options.DefaultName;

            if (configure != null)
            {
                builder.ConfigureServices(services => services.Configure(name, configure));
            }

            return builder.AddProcessor(sp =>
            {
                var options = sp.GetRequiredService<IOptionsMonitor<OtlpExporterOptions>>().Get(name);
                var httpClient = sp.GetRequiredService<HttpClient>();


                return new SimpleActivityExportProcessor(new JsonActivityExporter(options, new(httpClient)));
            });
        }
    }
}