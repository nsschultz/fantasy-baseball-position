# Deployment Guide

This document provides comprehensive procedures for deploying the Fantasy Baseball Position Service.

## Overview

The Position Service uses a containerized deployment model with Docker and Docker Compose. Releases follow semantic versioning and are automated via GitHub Actions.

## Continuous Integration

Before any code reaches main, it must pass PR validation checks via `.github/workflows/pr-validation.yml`:

**Automated Checks on Pull Requests**:
- **Build & Test**: Compiles code and runs 25+ unit and integration tests
- **SonarCloud Quality Gate**: Verifies code quality metrics and coverage ≥ 80%
- **Code Format**: Ensures consistent code style via `dotnet format`
- **Security Scan**: Scans with Trivy for vulnerabilities (SARIF format)

All PR validation jobs must pass before a PR can be merged to main.

## Release Process

### Step 1: Prepare Release

1. **Review Changes**
   - Check CHANGELOG.md for accuracy
   - Verify all tests pass (25+)
   - Confirm code coverage ≥ 80%
   - Validate security scans (0 critical vulnerabilities)

2. **Update Version**
   ```bash
   # Edit version.txt with new semantic version
   # Follows MAJOR.MINOR.PATCH
   # Example: 1.2.3 → 1.2.4 (patch), 1.2.3 → 1.3.0 (minor), 1.2.3 → 2.0.0 (major)
   echo "1.2.4" > version.txt
   ```

3. **Update CHANGELOG.md**
   ```markdown
   ## [1.2.4] - 2026-11-16
   
   ### Fixed
   - Bug fix description
   
   ### Changed
   - Feature improvement description
   ```

4. **Commit Changes**
   ```bash
   git add version.txt CHANGELOG.md
   git commit -m "Prepare release v1.2.4"
   git push origin main
   ```

### Step 2: Create Release Tag

```bash
# After all changes are merged to main
git tag -a v1.2.4 -m "Release v1.2.4: Description of release"
git push origin v1.2.4
```

**Tag Format**: `v<MAJOR>.<MINOR>.<PATCH>`
- Example: `v1.2.4`, `v2.0.0`

### Step 3: Automated Build & Push

The GitHub Actions workflow `docker-image.yml` automatically:

1. Triggers on tag push (v*.*.*)
2. Checks out code at tagged commit
3. Reads version from `version.txt`
4. Builds Docker image with multi-platform support (amd64, arm64)
5. Logs into Docker Hub
6. Pushes image with tags:
   - `nschultz/fantasy-baseball-position:1.2.4` (version tag)
   - `nschultz/fantasy-baseball-position:latest` (latest tag)

**Workflow File**: `.github/workflows/docker-image.yml`

### Step 4: Verify Docker Image

```bash
# Verify image on Docker Hub
docker pull nschultz/fantasy-baseball-position:1.2.4

# Test locally
docker run --rm nschultz/fantasy-baseball-position:1.2.4

# Check image layers
docker image history nschultz/fantasy-baseball-position:1.2.4
```

### Step 5: Create GitHub Release

1. Go to GitHub Releases
2. Create new release from tag `v1.2.4`
3. Add release notes from CHANGELOG.md
4. Include:
   - Summary of changes
   - Breaking changes (if any)
   - Migration instructions (if needed)
   - New features and improvements
   - Bug fixes
   - Known issues

## Deployment Environments

### Development Environment

**Configuration**: `.docker-compose/docker-compose-dev.yaml`

```bash
# Start development environment
docker compose -f .docker-compose/docker-compose-dev.yaml -p fantasy-baseball-position up --build

# Stop
docker compose -f .docker-compose/docker-compose-dev.yaml -p fantasy-baseball-position down
```

**Characteristics**:
- Local code changes mounted as volumes
- PostgreSQL in container
- Full logging and debugging
- No production data

### CI/CD Environment

**Configuration**: `.docker-compose/docker-compose-ci.yaml`

Used by GitHub Actions for automated testing:
- Isolated test environment
- Temporary database
- Clean slate for each run
- Security scanning enabled

### Production/Runtime Environment

**Configuration**: `.docker-compose/docker-compose-runtime.yaml`

```bash
# Start production environment
docker compose -f .docker-compose/docker-compose-runtime.yaml \
  -p fantasy-baseball-position up --build -d

# Stop
docker compose -f .docker-compose/docker-compose-runtime.yaml \
  -p fantasy-baseball-position down
```

**Characteristics**:
- Versioned Docker image (e.g., v1.2.4)
- Production database configuration
- Health checks enabled
- Persistent volumes
- Network isolation

## Deployment Procedure

### Pre-Deployment Checklist

- [ ] Release tag created and pushed (v1.2.4)
- [ ] Docker image built and available on Docker Hub
- [ ] All tests passing (25+)
- [ ] Code coverage ≥ 80%
- [ ] Security scans show 0 critical vulnerabilities
- [ ] CHANGELOG updated with release notes
- [ ] Downstream services notified of API changes (if any)
- [ ] Database migrations reviewed (if any)
- [ ] Rollback procedure documented
- [ ] On-call engineer aware of deployment

### Deployment Steps

1. **Verify Database Connectivity**
   ```bash
   # Test database connection
   psql -h $POSITION_DATABASE_HOST \
        -U $POSITION_DATABASE_USER \
        -d $POSITION_DATABASE \
        -c "SELECT 1;"
   ```

2. **Update Docker Compose File**
   ```bash
   # Edit .docker-compose/docker-compose-runtime.yaml
   # Update image version:
   # image: nschultz/fantasy-baseball-position:1.2.4
   ```

3. **Pull Latest Image**
   ```bash
   docker pull nschultz/fantasy-baseball-position:1.2.4
   ```

4. **Start Service with Rolling Update**
   ```bash
   # Start with new image
   docker compose -f .docker-compose/docker-compose-runtime.yaml \
     -p fantasy-baseball-position \
     up -d --force-recreate
   ```

5. **Verify Health**
   ```bash
   # Wait for service startup (usually 5-10 seconds)
   sleep 10
   
   # Check health endpoint
   curl http://localhost:8080/api/health
   
   # Expected response: 200 OK with healthy status
   ```

6. **Verify API Functionality**
   ```bash
   # Get positions
   curl http://localhost:8080/api/v1/position | jq . | head -20
   
   # Check Swagger documentation
   curl http://localhost:8080/api/swagger/index.html
   ```

7. **Check Logs**
   ```bash
   # View container logs
   docker compose -f .docker-compose/docker-compose-runtime.yaml \
     -p fantasy-baseball-position logs -f
   
   # Check for errors
   docker compose logs 2>&1 | grep -i error
   ```

### Post-Deployment Verification

```bash
# Verify running containers
docker ps | grep fantasy-baseball-position

# Check container stats
docker stats fantasy-baseball-position

# Verify volume mounts
docker inspect fantasy-baseball-position | grep -A 5 Mounts

# Test all key endpoints
./test-endpoints.sh  # Create this script
```

## Rollback Procedure

### Rollback to Previous Release

If issues are detected after deployment:

```bash
# 1. Identify previous stable version
# Check CHANGELOG.md or Docker Hub for previous version
# Example: 1.2.3

# 2. Update docker-compose-runtime.yaml
# Change image: nschultz/fantasy-baseball-position:1.2.4
# To:      image: nschultz/fantasy-baseball-position:1.2.3

# 3. Restart service with previous version
docker compose -f .docker-compose/docker-compose-runtime.yaml \
  -p fantasy-baseball-position \
  up -d --force-recreate

# 4. Verify health
sleep 10
curl http://localhost:8080/api/health

# 5. Check logs for issues
docker compose logs -f
```

### Database Rollback

If database schema migration caused issues:

1. **Review Migrations**
   ```bash
   # Check migration history in FantasyBaseball.PositionService/Database/Migrations/
   ls -la FantasyBaseball.PositionService/Database/Migrations/
   ```

2. **EF Core Rollback**
   - Entity Framework Core handles rollback on downgrade
   - Previous schema remains compatible
   - No manual SQL needed typically

3. **Backup and Restore (if needed)**
   ```bash
   # Backup before deployment
   pg_dump -h $POSITION_DATABASE_HOST \
           -U $POSITION_DATABASE_USER \
           -d $POSITION_DATABASE \
           > backup-$(date +%Y%m%d-%H%M%S).sql
   
   # Restore if needed
   psql -h $POSITION_DATABASE_HOST \
        -U $POSITION_DATABASE_USER \
        -d $POSITION_DATABASE \
        < backup-20261116-103000.sql
   ```

## Deployment Strategies

### Rolling Deployment

**Default Strategy**: Gradual replacement with health checks

- Replace one container at a time
- Verify health after each replacement
- Upstream load balancer handles traffic routing
- No downtime if configured properly

**Procedure**:
```bash
# Stop current version
docker compose down

# Start new version
docker compose up -d

# Verify health continuously
watch -n 5 'curl http://localhost:8080/api/health'
```

### Blue-Green Deployment (Future)

- Run two complete environments (blue and green)
- Deploy to inactive environment
- Run full tests
- Switch traffic to new environment
- Keep old environment as rollback option

### Canary Deployment (Future)

- Deploy to subset of servers first
- Monitor metrics for issues
- Gradually increase traffic percentage
- Full rollback if issues detected

## Database Migrations

### Before Deployment

1. **Review Migration Files**
   ```bash
   # Check for new migrations
   git diff main...v1.2.4 -- FantasyBaseball.PositionService/Database/Migrations/
   ```

2. **Test Migration Locally**
   ```bash
   # Run application locally to test migration
   dotnet run
   
   # Verify database schema changed correctly
   psql -c "\dt"  # List tables
   psql -c "\d positions"  # Describe table
   ```

3. **Backup Production Database**
   ```bash
   pg_dump -h $POSITION_DATABASE_HOST \
           -U $POSITION_DATABASE_USER \
           -d $POSITION_DATABASE \
           > backup-pre-v1.2.4.sql
   ```

### During Deployment

- EF Core automatically applies migrations on startup
- Migration scripts run before service starts responding
- Service won't start until migrations complete
- Long-running migrations may delay startup

### After Deployment

1. **Verify Schema**
   ```bash
   # Connect to database
   psql -h $POSITION_DATABASE_HOST \
        -U $POSITION_DATABASE_USER \
        -d $POSITION_DATABASE
   
   # Check schema version
   SELECT * FROM __EFMigrationsHistory ORDER BY MigrationId DESC LIMIT 5;
   ```

2. **Verify Data Integrity**
   ```bash
   # Check row counts
   SELECT COUNT(*) FROM positions;
   SELECT COUNT(*) FROM "AdditionalPositions";
   
   # Spot-check data
   SELECT * FROM positions LIMIT 5;
   ```

## Performance Optimization

### Before Production Deployment

```bash
# Load test locally
# Use Apache Bench or similar
ab -n 1000 -c 100 http://localhost:8080/api/v1/position

# Monitor resource usage
docker stats fantasy-baseball-position
```

### Production Monitoring

```bash
# Monitor CPU and memory
docker stats fantasy-baseball-position --no-stream

# Check database performance
psql -c "SELECT query, calls, mean_time FROM pg_stat_statements ORDER BY mean_time DESC LIMIT 10;"
```

## Incident Procedures

### Service Not Starting

1. **Check Logs**
   ```bash
   docker logs fantasy-baseball-position 2>&1 | tail -50
   ```

2. **Verify Configuration**
   ```bash
   # Check environment variables
   docker inspect fantasy-baseball-position | grep -A 20 Env
   
   # Test database connectivity
   psql -h $POSITION_DATABASE_HOST ...
   ```

3. **Verify Docker Image**
   ```bash
   docker pull nschultz/fantasy-baseball-position:1.2.4
   docker image inspect nschultz/fantasy-baseball-position:1.2.4
   ```

### Service Running but Unhealthy

1. **Check Health Endpoint**
   ```bash
   curl -v http://localhost:8080/api/health
   ```

2. **Monitor Database**
   ```bash
   psql -c "SELECT datname, current_setting('max_connections') FROM pg_database WHERE datname = 'position_db';"
   ```

3. **Check Application Logs**
   ```bash
   docker logs -f --tail 100 fantasy-baseball-position
   ```

### High Response Times

1. **Check Database Queries**
   ```bash
   # Enable query logging in PostgreSQL
   # Check slow queries in pg_stat_statements
   ```

2. **Monitor Container Resources**
   ```bash
   docker stats fantasy-baseball-position
   ```

3. **Scale if Needed**
   - Add more instances behind load balancer
   - Optimize database indices
   - Implement caching

## Maintenance Windows

### Scheduled Maintenance

```bash
# Announce maintenance (update status page if available)
# Set start time: 2:00 AM UTC (low traffic time)
# Duration: 15-30 minutes

# During maintenance:
docker compose down

# Perform work (backups, updates, etc.)
# ...

# Bring service back up
docker compose up -d

# Verify health
curl http://localhost:8080/api/health

# Announce completion
```

## Rollback Timeline

| Component | Time to Rollback |
|-----------|-----------------|
| Docker image revert | 2-5 minutes |
| Health check detection | 1-2 minutes |
| Database rollback | 5-30 minutes (depends on migration size) |
| **Total** | **~15 minutes** |

## Related Documentation

- [SLO_SLA.md](SLO_SLA.md) - Service level definitions and targets
- [DEVOPS_RUNBOOK.md](DEVOPS_RUNBOOK.md) - Operational quick reference
- [CHANGELOG.md](CHANGELOG.md) - Release notes and version history
- [README.md](README.md) - Project overview and setup
- [.github/agents/DevOpsExpert.agent.md](.github/agents/DevOpsExpert.agent.md) - DevOps guidance
