<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
//
use Prometheus\RenderTextFormat;
use Prometheus\CollectorRegistry;
use Illuminate\Http\Response;
//
use Prometheus\Storage\Redis;

class PrometheusMetricsController extends Controller
{
    private $collectorRegistry;

    public function __construct()
    {
        // Adaptador de almacenamiento persistente(Redis):
        $this->collectorRegistry = new CollectorRegistry(new Redis(['host' => 'redis-metricas-pasajes', 'port' => 6379,])); // 'password' => '123',
    }

    public function index() //getMetrics
    {
        $renderer = new RenderTextFormat();
        $metrics = $this->collectorRegistry->getMetricFamilySamples();

        return new Response($renderer->render($metrics), 200, ['Content-Type' => RenderTextFormat::MIME_TYPE]);
    }
    //
}
