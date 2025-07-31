using Prometheus;

namespace Infra.CrossCutting.Metrics
{
    public static class MetricsRegistry
    {
        private static readonly MetricFactory DefaultMetrics = global::Prometheus.Metrics.DefaultFactory;

        public static readonly Histogram HttpRequestDuration = DefaultMetrics
            .CreateHistogram("http_request_duration_seconds", 
                "Duração das requisições HTTP em segundos",
                new HistogramConfiguration
                {
                    LabelNames = new[] { "method", "endpoint" },
                    Buckets = Histogram.ExponentialBuckets(0.01, 2, 10)
                });

        public static readonly Gauge ActiveConnections = DefaultMetrics
            .CreateGauge("active_connections", 
                "Número de conexões ativas");

        public static readonly Counter DatabaseOperationsTotal = DefaultMetrics
            .CreateCounter("database_operations_total", 
                "Número total de operações no banco de dados",
                new CounterConfiguration
                {
                    LabelNames = new[] { "operation", "entity" }
                });

        public static readonly Histogram DatabaseOperationDuration = DefaultMetrics
            .CreateHistogram("database_operation_duration_seconds",
                "Duração das operações no banco de dados em segundos",
                new HistogramConfiguration
                {
                    LabelNames = new[] { "operation", "entity" },
                    Buckets = Histogram.ExponentialBuckets(0.001, 2, 10)
                });

        public static readonly Counter CacheHits = DefaultMetrics
            .CreateCounter("cache_hits_total", 
                "Número total de acertos no cache",
                new CounterConfiguration
                {
                    LabelNames = new[] { "cache" }
                });

        public static readonly Counter CacheMisses = DefaultMetrics
            .CreateCounter("cache_misses_total", 
                "Número total de falhas no cache",
                new CounterConfiguration
                {
                    LabelNames = new[] { "cache" }
                });

        public static readonly Counter HttpRequestsTotal = DefaultMetrics
            .CreateCounter("http_requests_total", 
                "Número total de requisições HTTP",
                new CounterConfiguration
                {
                    LabelNames = new[] { "method", "endpoint", "status" }
                });
    }
} 
