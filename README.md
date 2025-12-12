# Observability-Loki-Tempo-Otel-Prometheus-Grafana

Acesse as métricas:
Serviço	Endpoint	Métricas
Node Exporter	http://localhost:9100/metrics	node_* (CPU, memória, disco, rede do host)
API Contagem	http://localhost:5001/metrics	http_server_*, process_runtime_dotnet_*
API Orquestração	http://localhost:5000/metrics	http_server_*, process_runtime_dotnet_*
OTEL Collector	http://localhost:8889/metrics	Métricas agregadas pelo collector
Prometheus	http://localhost:9090/graph	Consultar todas as métricas coletadas

dashboards:
https://grafana.com/grafana/dashboards/1860-node-exporter-full/
https://grafana.com/grafana/dashboards/17706-asp-net-otel-metrics/
