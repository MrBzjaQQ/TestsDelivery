# Deployment Guide - TestsDelivery

## Prerequisites

### Production Requirements

- **Kubernetes Cluster** (v1.28+) or **Docker Swarm**
- **PostgreSQL** (v18.1+) - External or provisioned
- **RabbitMQ** (v3.13+) - External or provisioned
- **Load Balancer** (NGINX, HAProxy, or cloud LB)
- **Monitoring Stack** (Prometheus, Grafana, Loki, Tempo)
- **CI/CD Pipeline** (GitLab CI, GitHub Actions, Jenkins)

## Kubernetes Deployment

### Helm Chart Structure

```
helm-chart-testsdelivery/
├── Chart.yaml
├── values.yaml
├── values-prod.yaml
├── templates/
│   ├── identity-service/
│   ├── question-service/
│   ├── student-service/
│   ├── test-checking-service/
│   ├── file-storage-service/
│   ├── notification-service/
│   ├── bff-portal-service/
│   ├── postgres/
│   ├── rabbitmq/
│   └── ingress.yaml
└── README.md
```

### Kubernetes Manifests

#### Identity Service Deployment

```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: identity-service
  labels:
    app: identity-service
spec:
  replicas: 3
  selector:
    matchLabels:
      app: identity-service
  template:
    metadata:
      labels:
        app: identity-service
    spec:
      containers:
      - name: identity-service
        image: registry.example.com/identity-service:1.0
        ports:
        - containerPort: 8080
        env:
        - name: ConnectionStrings__DefaultConnection
          valueFrom:
            secretKeyRef:
              name: postgres-credentials
              key: connection-string
        - name: RabbitMQ__Host
          value: "rabbitmq"
        resources:
          requests:
            memory: "256Mi"
            cpu: "100m"
          limits:
            memory: "512Mi"
            cpu: "500m"
        livenessProbe:
          httpGet:
            path: /health
            port: 8080
          initialDelaySeconds: 30
          periodSeconds: 10
        readinessProbe:
          httpGet:
            path: /health
            port: 8080
          initialDelaySeconds: 5
          periodSeconds: 5
---
apiVersion: v1
kind: Service
metadata:
  name: identity-service
spec:
  selector:
    app: identity-service
  ports:
  - port: 8080
    targetPort: 8080
  type: ClusterIP
```

### Ingress Configuration

```yaml
apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  name: testsdelivery-ingress
  annotations:
    kubernetes.io/ingress.class: nginx
    nginx.ingress.kubernetes.io/ssl-redirect: "true"
spec:
  tls:
  - hosts:
    - api.testsdelivery.com
    secretName: testsdelivery-tls
  rules:
  - host: api.testsdelivery.com
    http:
      paths:
      - path: /api/v1/identity
        pathType: Prefix
        backend:
          service:
            name: identity-service
            port:
              number: 8080
      - path: /api/v1/question
        pathType: Prefix
        backend:
          service:
            name: question-management-service
            port:
              number: 8082
      - path: /api/v1/student
        pathType: Prefix
        backend:
          service:
            name: student-management-service
            port:
              number: 8083
```

## Deployment Stages

### 1. Pre-deployment Checklist

- [ ] All database migrations applied
- [ ] Environment variables configured
- [ ] TLS certificates installed
- [ ] Monitoring and logging configured
- [ ] Backup strategy ready
- [ ] Rollback plan documented

### 2. Deployment Process

```bash
# 1. Apply database migrations
kubectl apply -f k8s/migrations/identity-service-migration.yaml
kubectl apply -f k8s/migrations/question-service-migration.yaml

# 2. Update deployments
kubectl rollout restart deployments/identity-service
kubectl rollout restart deployments/question-management-service
kubectl rollout restart deployments/student-management-service

# 3. Monitor deployment
kubectl rollout status deploy/identity-service
kubectl rollout history deploy/identity-service
```

### 3. Post-deployment Verification

```bash
# Check pod status
kubectl get pods -l app=identity-service

# Check service health
kubectl port-forward svc/identity-service 8081:8080
curl http://localhost:8081/health

# View logs
kubectl logs -f deploy/identity-service
```

## Rolling Updates

```yaml
# Update strategy
strategy:
  type: RollingUpdate
  rollingUpdate:
    maxSurge: 25%
    maxUnavailable: 25%
```

## Blue-Green Deployment

```bash
# Deploy new version
kubectl apply -f k8s/deployment-identity-service-green.yaml

# Test new version
kubectl port-forward svc/identity-service-green 8081:8080

# Switch traffic
kubectl patch service identity-service -p '{"spec":{"selector":{"appVersion":"green"}}}'

# Clean up old version
kubectl delete deployments/identity-service-blue
```

## CI/CD Pipeline

### GitLab CI Example

```yaml
stages:
  - build
  - test
  - deploy-staging
  - deploy-production

variables:
  DOCKER_TLS_CERTDIR: "/certs"
  IMAGE_TAG: $CI_REGISTRY_IMAGE/identity-service:$CI_COMMIT_SHA

build:
  stage: build
  script:
    - docker login -u gitlab-ci-token -p $CI_JOB_TOKEN $CI_REGISTRY
    - docker build -f IdentityService/WebApi/Dockerfile . -t $IMAGE_TAG
    - docker push $IMAGE_TAG
  only:
    - develop
    - master

test:
  stage: test
  script:
    - dotnet restore
    - dotnet build
    - dotnet test --no-build
  artifacts:
    reports:
      junit: test-results.xml

deploy-staging:
  stage: deploy-staging
  script:
    - kubectl apply -f k8s/staging/
    - kubectl rollout status deploy/identity-service -n staging
  environment:
    name: staging
  only:
    - develop

deploy-production:
  stage: deploy-production
  script:
    - kubectl apply -f k8s/production/
    - kubectl rollout status deploy/identity-service -n production
  environment:
    name: production
  only:
    - master
```

## Monitoring & Observability

### Prometheus Metrics

```yaml
# ServiceMonitor for Prometheus
apiVersion: monitoring.coreos.com/v1
kind: ServiceMonitor
metadata:
  name: identity-service
spec:
  selector:
    matchLabels:
      app: identity-service
  endpoints:
  - port: http
    interval: 15s
    path: /metrics
```

### Grafana Dashboards

- HTTP Requests
- Database Connections
- RabbitMQ Queue Lengths
- Error Rates
- Response Times

### Logging with Loki

```yaml
# LogQL queries
# Errors in last 5 minutes
{app="identity-service"} |= "Error" | __error__="" | count_over_time(metric[5m])

# Slow requests (>1s)
{app="identity-service", method="GET"} | logfmt | duration_seconds > 1
```

## Security Hardening

### Production Checklist

- [ ] TLS 1.2+ enabled
- [ ] JWT tokens with 15-30 min expiration
- [ ] CORS properly configured
- [ ] Rate limiting applied
- [ ] Database passwords in secrets
- [ ] Network policies configured
- [ ] Pod security policies applied
- [ ] Automatic security updates enabled

### Network Policies

```yaml
apiVersion: networking.k8s.io/v1
kind: NetworkPolicy
metadata:
  name: identity-service-policy
spec:
  podSelector:
    matchLabels:
      app: identity-service
  policyTypes:
  - Ingress
  - Egress
  ingress:
  - from:
    - namespaceSelector:
        matchLabels:
          name: ingress-nginx
    ports:
    - port: 8080
      protocol: TCP
  egress:
  - to:
    - namespaceSelector: {}
      podSelector:
        matchLabels:
          app: rabbitmq
    ports:
    - port: 5672
      protocol: TCP
  - to:
    - namespaceSelector: {}
      podSelector:
        matchLabels:
          app: postgres
    ports:
    - port: 5432
      protocol: TCP
```

## Backup Strategy

### Database Backups

```bash
# Automated backup script
pg_dump -h pg-host -U app-user testsdelivery_identity | gzip > backup-$(date +%Y%m%d).sql.gz

# Schedule with CronJob
apiVersion: batch/v1
kind: CronJob
metadata:
  name: database-backup
spec:
  schedule: "0 2 * * *"
  jobTemplate:
    spec:
      template:
        spec:
          containers:
          - name: backup
            image: postgres:18.1
            command: ["sh", "-c", "pg_dump ... | gzip > /backup/backup.sql.gz"]
            volumeMounts:
            - name: backup-volume
              mountPath: /backup
          restartPolicy: OnFailure
          volumes:
          - name: backup-volume
            persistentVolumeClaim:
              claimName: backup-pvc
```

## Rollback Plan

### Manual Rollback

```bash
# Rollback deployment
kubectl rollout undo deployment/identity-service

# Or specify revision
kubectl rollout undo deployment/identity-service --to-revision=3

# Verify rollback
kubectl rollout status deploy/identity-service
```

### Automated Rollback (Kubernetes)

```yaml
# Add to deployment spec
strategy:
  type: RollingUpdate
  rollingUpdate:
    maxSurge: 25%
    maxUnavailable: 25%

# Monitoring for automatic rollback
# Use Prometheus alerting to trigger rollback on high error rates
```

## Performance Tuning

### Database Connection Pooling

```csharp
options.UseNpgsql(connectionString, npgsqlOptions =>
{
    npgsqlOptions.MaxPoolSize = 50;
    npgsqlOptions.MinPoolSize = 5;
    npgsqlOptions.ConnectionIdleTimeout = 60;
});
```

### Kubernetes Resource Limits

```yaml
resources:
  requests:
    memory: "256Mi"
    cpu: "100m"
  limits:
    memory: "512Mi"
    cpu: "500m"
```

## Scaling

### Horizontal Pod Autoscaler

```yaml
apiVersion: autoscaling/v2
kind: HorizontalPodAutoscaler
metadata:
  name: identity-service-hpa
spec:
  scaleTargetRef:
    apiVersion: apps/v1
    kind: Deployment
    name: identity-service
  minReplicas: 2
  maxReplicas: 10
  metrics:
  - type: Resource
    resource:
      name: cpu
      target:
        type: Utilization
        averageUtilization: 70
  - type: Resource
    resource:
      name: memory
      target:
        type: Utilization
        averageUtilization: 80
```
