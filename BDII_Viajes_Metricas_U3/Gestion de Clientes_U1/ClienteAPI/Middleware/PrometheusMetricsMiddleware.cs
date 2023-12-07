using Microsoft.AspNetCore.Http;
using Prometheus;
using System.Diagnostics;
using System.Threading.Tasks;
//
using Microsoft.AspNetCore.Routing;

namespace ClienteAPI.Middleware
{
    public class PrometheusMetricsMiddleware
    {
        private readonly RequestDelegate _next;

        // Definicion del contador y el histograma
        private static readonly Counter HttpResponses = Metrics.CreateCounter(
            "api_http_responses",
            "HTTP Responses",
            new CounterConfiguration
            {
                LabelNames = new[] { "method", "endpoint", "status" }
            });

        private static readonly Histogram RequestLatency = Metrics.CreateHistogram(
            "api_request_latency",
            "Request latency",
            new HistogramConfiguration
            {
                Buckets = new[] { 0.001, 0.005, 0.01, 0.025, 0.05, 0.075, 0.1, 0.25, 0.5, 1.0, 2.0, 5.0 },//Buckets Personalizados
                LabelNames = new[] { "method", "endpoint" }
            });

        public PrometheusMetricsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            // Omitir metricas para el endpoint "/metrics" (quitan este if si quieren)
            if (context.Request.Path.StartsWithSegments("/metrics"))
            {
                await _next(context);
                return;
            }


            var watch = Stopwatch.StartNew();
            try
            {
                await _next(context);
            }
            finally
            {
                watch.Stop();

                // Obtiene la información necesaria de la solicitud y la respuesta
                var method = context.Request.Method;
                var endpoint = context.Request.Path.ToString();// Convertir PathString a String
                var status = context.Response.StatusCode.ToString();
                var latency = watch.Elapsed.TotalSeconds;


                // Obtener enpoints "api/TipoCorreo/5" y evitar "api/TipoCorreo/{id}"
                var routeData = context.GetRouteData();
                if (routeData != null)
                {
                    foreach (var param in routeData.Values)
                    {
                        endpoint = endpoint.Replace($"{{{param.Key}}}", param.Value.ToString());
                    }
                }


                // Registra el contador y el histograma
                HttpResponses.WithLabels(method, endpoint, status).Inc();
                RequestLatency.WithLabels(method, endpoint).Observe(latency);
            }
        }
    }
}
//===============================================================================================

/*
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Prometheus;

public class PrometheusMetricsMiddleware
{
    private readonly RequestDelegate _next;

    public PrometheusMetricsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Código para ejecutar antes de la solicitud

        await _next(context); // Pasar control al siguiente middleware

        // Código para ejecutar después de la solicitud
    }
}
*/
//===============================================================================================
/*
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Threading.Tasks;

public class MetricsMiddleware
{
    private readonly RequestDelegate _next;

    public MetricsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        await _next(context);

        stopwatch.Stop();
        var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

        // Aquí registrarías las métricas usando prometheus-net
    }
}
*/