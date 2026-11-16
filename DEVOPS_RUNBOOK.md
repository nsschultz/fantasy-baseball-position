# DevOps Runbook

Quick reference guide for operating the Fantasy Baseball Position Service.

## Emergency Contacts

| Role | Escalation |
|------|-----------|
| On-Call Engineer | #fantasy-baseball-incidents |
| Team Lead | @nsschultz |
| Backup | Escalate to team lead |

**On-Call Schedule**: [TBD - Configure in PagerDuty/similar]

## Quick Health Checks

### Service Status

```bash
# Check if service is running
docker ps | grep fantasy-baseball-position

# Get service health
curl http://localhost:8080/api/health

# View recent logs
docker logs -n 50 fantasy-baseball-position

# Check all endpoints
curl http://localhost:8080/api/v1/position | jq '.[0]'
curl http://localhost:8080/api/swagger/index.html
```

### Database Status

```bash
# Test database connectivity
docker exec fantasy-baseball-position-db psql -U position_user -d position_db -c "SELECT 1;"

# Check active connections
docker exec fantasy-baseball-position-db psql -U position_user -d position_db -c "SELECT count(*) FROM pg_stat_activity;"

# Check database size
docker exec fantasy-baseball-position-db psql -U position_user -d position_db -c "SELECT pg_size_pretty(pg_database_size('position_db'));"
```

## Common Issues and Solutions

### Issue: Service Returns 503 Service Unavailable

**Symptom**: Health check fails, cannot connect to database

**Diagnosis**:
```bash
# Check logs for error messages
docker logs fantasy-baseball-position | tail -50 | grep -i error

# Verify database is accessible
curl -v http://localhost:8080/api/health

# Check database container status
docker ps | grep postgres
```

**Solutions** (in order):

1. **Restart Database**
   ```bash
   docker-compose -f .docker-compose/docker-compose-runtime.yaml restart db
   sleep 5
   curl http://localhost:8080/api/health
   ```

2. **Restart Service**
   ```bash
   docker-compose -f .docker-compose/docker-compose-runtime.yaml restart
   sleep 5
   curl http://localhost:8080/api/health
   ```

3. **Check Connection String**
   ```bash
   # Verify environment variables
   docker-compose config | grep -A 5 "environment:"
   
   # Test connection manually
   psql -h $POSITION_DATABASE_HOST -U $POSITION_DATABASE_USER -d $POSITION_DATABASE -c "SELECT 1;"
   ```

4. **Full Restart**
   ```bash
   docker-compose -f .docker-compose/docker-compose-runtime.yaml down
   docker-compose -f .docker-compose/docker-compose-runtime.yaml up -d
   sleep 10
   curl http://localhost:8080/api/health
   ```

5. **Check Database Logs**
   ```bash
   docker logs fantasy-baseball-position-db | tail -100
   ```

### Issue: Slow Response Times (P95 > 200ms)

**Symptom**: API responses are taking longer than 200ms

**Diagnosis**:
```bash
# Check container resources
docker stats fantasy-baseball-position --no-stream

# Load test to measure current performance
curl -X GET http://localhost:8080/api/v1/position | jq '.' | wc -l

# Check database query performance
docker exec fantasy-baseball-position-db psql -U position_user -d position_db \
  -c "SELECT query, calls, mean_time FROM pg_stat_statements ORDER BY mean_time DESC LIMIT 10;"
```

**Solutions** (in order):

1. **Check CPU/Memory**
   ```bash
   docker stats fantasy-baseball-position
   # If > 80% CPU or RAM, may need optimization
   ```

2. **Restart Service to Clear Cache**
   ```bash
   docker-compose restart
   ```

3. **Optimize Database Indices**
   ```bash
   docker exec fantasy-baseball-position-db psql -U position_user -d position_db \
    -c "ANALYZE;" # Analyze table statistics
   ```

4. **Check for Connection Pooling Issues**
   ```bash
   # Verify connection count in logs
   docker logs fantasy-baseball-position | grep -i "connection" | tail -20
   ```

5. **Escalate**: If issue persists, contact team lead

### Issue: High Memory Usage

**Symptom**: Docker stats shows increasing memory usage

**Diagnosis**:
```bash
# Check memory usage over time
docker stats fantasy-baseball-position --no-stream

# Check for memory leaks in logs
docker logs fantasy-baseball-position | grep -i "memory\|garbage\|gc"

# Check running processes
docker top fantasy-baseball-position
```

**Solutions**:

1. **Restart Service**
   ```bash
   docker-compose restart
   ```

2. **Check for Runaway Processes**
   ```bash
   docker top fantasy-baseball-position
   # Kill if necessary: docker kill <container-id>
   ```

3. **Check Application Logs**
   ```bash
   docker logs fantasy-baseball-position | tail -100 | grep -i "error\|exception"
   ```

4. **Escalate**: If memory doesn't stabilize, contact team lead

### Issue: Database Disk Space Full

**Symptom**: Cannot write to database, queries fail

**Diagnosis**:
```bash
# Check database size
docker exec fantasy-baseball-position-db psql -U position_user -d position_db \
  -c "SELECT pg_size_pretty(pg_database_size('position_db'));"

# Check available disk space
docker exec fantasy-baseball-position-db df -h
```

**Solutions**:

1. **Check for Large Tables**
   ```bash
   docker exec fantasy-baseball-position-db psql -U position_user -d position_db \
     -c "SELECT schemaname, tablename, pg_size_pretty(pg_total_relation_size(schemaname||'.'||tablename)) FROM pg_tables ORDER BY pg_total_relation_size(schemaname||'.'||tablename) DESC LIMIT 10;"
   ```

2. **Vacuum Database**
   ```bash
   docker exec fantasy-baseball-position-db psql -U position_user -d position_db \
     -c "VACUUM FULL;"
   ```

3. **Escalate**: If disk usage remains high, contact team lead

## Deployment Procedures

### Quick Deploy (Simple Patch)

```bash
# 1. Update version.txt
echo "1.2.5" > version.txt

# 2. Commit and tag
git add version.txt
git commit -m "Bump version to 1.2.5"
git tag -a v1.2.5 -m "Release v1.2.5"
git push origin main
git push origin v1.2.5

# 3. Wait for GitHub Actions build (~5-10 minutes)
# 4. Verify image on Docker Hub
docker pull nschultz/fantasy-baseball-position:1.2.5

# 5. Update docker-compose-runtime.yaml with new version
# 6. Deploy
docker-compose -f .docker-compose/docker-compose-runtime.yaml up -d

# 7. Verify
curl http://localhost:8080/api/health
curl http://localhost:8080/api/v1/position | jq '.[0]'
```

### Rolling Back (Emergency Rollback)

```bash
# 1. Find previous working version
# Check CHANGELOG.md or: docker images | grep fantasy-baseball-position

# 2. Update docker-compose-runtime.yaml with previous version
# Change: image: nschultz/fantasy-baseball-position:1.2.5
# To:     image: nschultz/fantasy-baseball-position:1.2.4

# 3. Restart service
docker-compose -f .docker-compose/docker-compose-runtime.yaml up -d --force-recreate

# 4. Verify health
sleep 5
curl http://localhost:8080/api/health

# 5. Confirm endpoints working
curl http://localhost:8080/api/v1/position | jq '.length'

# 6. Notify team
echo "Rolled back to v1.2.4 due to [REASON]" >> INCIDENT.log
```

## Monitoring and Alerting

### Daily Checks

- [ ] Service responds to health check: `curl http://localhost:8080/api/health`
- [ ] No error spikes in logs: `docker logs fantasy-baseball-position | grep -i error | wc -l`
- [ ] Database connectivity healthy: `docker logs fantasy-baseball-position | grep -i "database\|connection"`
- [ ] SonarCloud quality gate passing: Check dashboard
- [ ] GitHub Actions builds passing: Check workflow status

### Weekly Checks

- [ ] Review error logs for patterns
- [ ] Check disk usage: `df -h`
- [ ] Verify backups completed
- [ ] Review performance metrics (P95 response time)
- [ ] Check security scan results (0 critical vulnerabilities)

### Monthly Checks

- [ ] Review DORA metrics
- [ ] Analyze error budget utilization
- [ ] Performance trend analysis
- [ ] Incident review (if any)
- [ ] Update runbook based on learnings

## Backup and Restore

### Database Backup

```bash
# Manual backup
pg_dump -h $POSITION_DATABASE_HOST \
        -U $POSITION_DATABASE_USER \
        -d $POSITION_DATABASE \
        > backup-$(date +%Y%m%d-%H%M%S).sql

# Backup size
du -h backup-*.sql

# Upload to safe location
# [TBD - Configure backup strategy]
```

### Database Restore

```bash
# Restore from backup
psql -h $POSITION_DATABASE_HOST \
     -U $POSITION_DATABASE_USER \
     -d $POSITION_DATABASE \
     < backup-20261116-150000.sql

# Verify restoration
psql -c "SELECT COUNT(*) FROM positions;"
```

## Log Management

### View Logs

```bash
# Real-time logs
docker logs -f fantasy-baseball-position

# Last 50 lines
docker logs --tail 50 fantasy-baseball-position

# Logs with timestamps
docker logs --timestamps fantasy-baseball-position

# Filter for errors
docker logs fantasy-baseball-position 2>&1 | grep -i error

# Filter for specific time
docker logs --since 1h fantasy-baseball-position
```

### Log Retention

- Docker logs stored in: `/var/lib/docker/containers/<container-id>/<container-id>-json.log`
- Log rotation configured via Docker daemon
- [TBD - Configure centralized logging]

## Performance Tuning

### Database Query Optimization

```bash
# Enable query logging
docker exec fantasy-baseball-position-db psql -U position_user -d position_db \
  -c "ALTER SYSTEM SET log_min_duration_statement = 100; SELECT pg_reload_conf();"

# Check query performance
docker logs fantasy-baseball-position | grep "duration:"
```

### Connection Pooling

```bash
# Check database connections
docker exec fantasy-baseball-position-db psql -U position_user -d position_db \
  -c "SELECT datname, usename, count(*) as num_connections FROM pg_stat_activity GROUP BY datname, usename;"

# Verify connection settings in Program.cs (currently unlimited in connection string)
```

### Caching Strategy

- [TBD] Consider Redis for frequently accessed position data
- [TBD] Configure HTTP caching headers for position endpoint

## Incident Response

### Incident Classification

| Severity | Impact | Response Time | Example |
|----------|--------|---|---|
| P1 | Service down | Immediate | Cannot reach /api/health |
| P2 | Partial outage | 15 min | Health check failing |
| P3 | Degraded | 1 hour | Response times > 500ms |
| P4 | Minor | Best effort | Typo in logs |

### Incident Workflow

1. **Detect**: Monitoring alert or customer report
2. **Alert**: Page on-call engineer
3. **Triage**: Classify severity (P1-P4)
4. **Mitigate**: Apply temporary fix or rollback
5. **Resolve**: Find root cause and deploy fix
6. **Review**: Post-incident analysis

### Post-Incident Actions

```bash
# Document in incident log
cat >> INCIDENTS.log << EOF
Date: $(date)
Severity: P1
Duration: 30 minutes
Issue: Database unavailable
Root Cause: Connection pool exhausted
Resolution: Restarted database container
Learnings: Need better connection monitoring
Action Items:
  - [ ] Implement connection pooling alerts
  - [ ] Add database monitoring dashboard
  - [ ] Update runbook with connection troubleshooting
EOF

# Schedule post-mortem review
# Review within 48 hours of incident
```

## Useful Commands

```bash
# Container management
docker-compose up -d                           # Start service
docker-compose down                            # Stop service
docker-compose restart                         # Restart service
docker-compose logs -f                         # View logs
docker-compose ps                              # List containers

# Health checks
curl http://localhost:8080/api/health          # Check health
curl http://localhost:8080/api/v1/position     # Test API

# Database access
docker exec fantasy-baseball-position-db psql -U $USER -d $DB -c "SELECT ..."

# Service updates
docker pull nschultz/fantasy-baseball-position:latest
docker image prune                             # Clean up old images

# Performance monitoring
docker stats fantasy-baseball-position         # Real-time resource usage
docker inspect <container>                     # Detailed container info
```

## Development Setup

### Pre-Commit Hooks

The repository includes pre-commit hooks to enforce code quality standards before commits.

**Installation:**

```bash
# Automatic (recommended)
git config core.hooksPath .githooks

# Manual (if needed)
cp .githooks/pre-commit .git/hooks/pre-commit
chmod +x .git/hooks/pre-commit
```

**What it checks:**

1. **Secrets Detection**: Prevents accidental commit of passwords, tokens, API keys
2. **Code Formatting**: Runs `dotnet format` to enforce consistent style
3. **File Size Check**: Prevents large files (> 5MB) from being committed
4. **Build Check** (optional): Can be enabled to verify build succeeds
5. **Test Check** (optional): Can be enabled to verify tests pass

**Bypassing the hook** (use with caution):

```bash
git commit --no-verify
```

**Enabling optional checks:**

Edit `.githooks/pre-commit` and uncomment the build or test sections if desired:

```bash
# Uncomment to enable build check (makes commits slower but more reliable)
# Uncomment to enable test check (ensures quality but increases commit time)
```

## Resources and References

- **Project**: https://github.com/nsschultz/fantasy-baseball-position
- **Deployment Guide**: [DEPLOYMENT.md](DEPLOYMENT.md)
- **SLO/SLA**: [SLO_SLA.md](SLO_SLA.md)
- **DevOps Agent**: [.github/agents/DevOpsExpert.agent.md](.github/agents/DevOpsExpert.agent.md)
- **C# Standards**: [.github/agents/CSharpExpert.agent.md](.github/agents/CSharpExpert.agent.md)
- **Pre-Commit Hooks**: [.githooks/pre-commit](.githooks/pre-commit)

## Revision History

| Date | Author | Changes |
|------|--------|---------|
| 2026-11-16 | DevOps Expert | Initial runbook |
| TBD | | TBD |

---

**Last Updated**: 2026-11-16
**Next Review**: 2026-12-16
