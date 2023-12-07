/*
using Microsoft.AspNetCore.Mvc;
using Prometheus;
using System.IO;

[ApiController]
[Route("[metrics]")]
public class PrometheusMetricsController : ControllerBase
{
    private readonly CollectorRegistry _collectorRegistry;

    public PrometheusMetricsController(CollectorRegistry collectorRegistry)
    {
        _collectorRegistry = collectorRegistry;
    }

    [HttpGet]
    public IActionResult GetMetrics()
    {
        var metrics = _collectorRegistry.GetMetricFamilies();
        var serializer = new TextSerializer();
        using (var stream = new MemoryStream())
        {
            serializer.Serialize(stream, metrics);//Restablecer la posición del "cursor" al comienzo del flujo de memoria
            stream.Position = 0;
            return File(stream.ToArray(), "text/plain; version=0.0.4; charset=utf-8");
        }
    }
}
*/