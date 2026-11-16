# Service Level Objectives and Agreements

This document defines the Service Level Objectives (SLOs) and Service Level Agreements (SLAs) for the Fantasy Baseball Position Service.

## Overview

The Position Service is a critical microservice that serves as the source of truth for position information. It is used by multiple downstream services and must maintain high reliability, performance, and quality standards.

## Service Level Objectives (SLOs)

SLOs are internal targets that guide operational decisions and priorities.

### Availability SLO

**Target**: 99.5% uptime (per calendar month)

- **Definition**: HTTP requests to the service successfully complete with a 2xx or 3xx status code
- **Exclusions**: Planned maintenance windows (documented in advance)
- **Measurement**: Continuous monitoring of `/api/health` endpoint
- **Error Budget**: 36 minutes (3.6 hours) downtime per month
- **Escalation**: Any incident approaching error budget triggers incident review

### Performance SLO

**Target**: P95 response time < 200ms for all GET endpoints

- **Definition**: 95th percentile of response times for position data queries
- **Measurement**: Application request/response logging
- **Baseline**: Established via load testing and production monitoring
- **Escalation**: Sustained performance degradation triggers investigation

### Reliability SLO

**Target**: 99.9% CI/CD pipeline success rate

- **Definition**: Percentage of automated builds, tests, and deployments that complete successfully
- **Measurement**: GitHub Actions workflow status
- **Exclusions**: Expected failures (security findings, code quality gates)
- **Escalation**: Repeated build failures (3+) trigger infrastructure review

### Quality SLO

**Target**: ≥ 80% code coverage (enforced by SonarCloud)

- **Definition**: Line coverage percentage of codebase by automated tests
- **Measurement**: SonarCloud analysis on every commit
- **Escalation**: Coverage drops below 80% block pull request merge

### Security SLO

**Target**: Zero critical vulnerabilities

- **Definition**: No unmitigated CVSS score ≥ 9.0 vulnerabilities
- **Measurement**: Trivy container scanning + SonarQube SAST + dependency scanning
- **Escalation**: Critical vulnerability discovered requires immediate action
- **Timeline**: Critical vulnerabilities must be addressed within 24 hours

## Service Level Agreements (SLAs)

SLAs are commitments to external consumers of the service.

### Availability SLA

**Commitment**: 99.5% uptime (per calendar month)

- **Service Window**: 24/7 with planned maintenance notifications
- **Response Time**: Maximum 1 hour for critical incidents
- **Escalation Path**:
  - P1 (Critical): Immediate response (pager)
  - P2 (High): 15 minute response
  - P3 (Medium): 1 hour response
  - P4 (Low): Best effort

### Performance SLA

**Commitment**: P95 response time < 200ms

- **Applies To**: All read operations on `/api/v1/position` endpoint
- **Conditions**: Under normal load (< 100 req/sec)
- **Monitoring**: Published metrics dashboard

### Support SLA

**Availability**: Business hours support (9 AM - 5 PM EST, Monday - Friday)

- **Response Time**: 2 hours for P1 incidents during business hours
- **On-Call**: On-call rotation for P1 critical incidents
- **Communication**: Status updates every 30 minutes for ongoing incidents

## Error Budget

Error budgets allow for controlled risk-taking while maintaining SLO targets.

### Monthly Error Budget (99.5% SLO)

| Month Days | Error Budget | Duration |
|-----------|--------------|----------|
| 28 | 20.16 minutes | |
| 29 | 20.88 minutes | |
| 30 | 21.6 minutes | |
| 31 | 22.32 minutes | |

**Average**: ~21.6 minutes per month

### Error Budget Utilization

- **0-25%**: Green - Normal operations
- **25-75%**: Yellow - Caution, avoid high-risk changes
- **75-100%**: Orange - Pause non-critical changes
- **>100%**: Red - Incident post-mortem required

### Error Budget Policy

1. **Tracking**: Error budget calculated daily and published
2. **Alerts**: Warn when approaching 75% utilization
3. **Freeze**: Recommend deployment freeze at 75% consumption
4. **Recovery**: Post-incident review when exceeding budget
5. **Planning**: Use metrics to plan reliability improvements

## DORA Metrics

DevOps Research and Assessment (DORA) metrics measure delivery performance.

### Deployment Frequency

**Target**: 1-2 deployments per month

| Metric | Current | Target | Status |
|--------|---------|--------|--------|
| Releases/Month | 1-2 | 1-2 | ✅ |

- Small, frequent releases reduce risk
- Each release should be testable and shippable

### Lead Time for Changes

**Target**: < 1 week from commit to production

| Metric | Current | Target | Status |
|--------|---------|--------|--------|
| Days to Deploy | < 7 | < 7 | ✅ |

- Automate CI/CD pipeline
- Reduce manual approval gates
- Enable fast feedback loops

### Mean Time to Recovery (MTTR)

**Target**: < 30 minutes to restore service

| Metric | Current | Target | Status |
|--------|---------|--------|--------|
| MTTR | < 30 min | < 30 min | ✅ |

- Keep deployments reversible
- Document rollback procedures
- Practice incident response

### Change Failure Rate

**Target**: < 15% of changes cause incidents

| Metric | Current | Target | Status |
|--------|---------|--------|--------|
| Failure Rate | < 15% | < 15% | ✅ |

- Comprehensive testing (≥ 80% coverage)
- Staged rollouts and canary deployments
- Quality gates and approvals

## Monitoring and Tracking

### Key Metrics Dashboard

**Location**: https://sonarcloud.io/project/overview?id=nsschultz_fantasy-baseball-position

| Metric | Tool | Frequency | Action |
|--------|------|-----------|--------|
| Availability | Health checks | Real-time | Alert if down |
| Response Time | Application logs | Real-time | Alert if P95 > 200ms |
| Code Coverage | SonarCloud | Per commit | Alert if < 80% |
| Quality Gate | SonarCloud | Per commit | Block if failing |
| Build Success | GitHub Actions | Per build | Alert if failed |
| Security | Trivy + SonarQube | Per build | Block if critical |

### Incident Severity Levels

| Level | Definition | Response | Target |
|-------|-----------|----------|--------|
| P1 - Critical | Service unavailable or major data loss risk | Immediate | 1 hour |
| P2 - High | Significant degradation, core feature affected | 15 minutes | 4 hours |
| P3 - Medium | Limited functionality, workaround available | 1 hour | 1 day |
| P4 - Low | Minor issue, no workaround needed | Best effort | 7 days |

### Monthly Review Checklist

```
[ ] Availability SLO met (99.5%+)?
[ ] Performance SLO met (P95 < 200ms)?
[ ] Reliability SLO met (99.9% CI/CD)?
[ ] Quality SLO met (≥ 80% coverage)?
[ ] Security SLO met (0 critical vulnerabilities)?
[ ] DORA metrics trending positively?
[ ] Error budget utilization reviewed?
[ ] Incidents analyzed for patterns?
[ ] Action items from incidents tracked?
[ ] Coverage trends and technical debt reviewed?
```

## Continuous Improvement

### Feedback Loop

```
Monitor SLOs → Analyze Trends → Identify Issues → Plan Improvements → Implement → Monitor
```

### Examples of SLO-Driven Improvements

| Problem | Solution | Expected Impact |
|---------|----------|-----------------|
| High MTTR | Improve runbook, practice drills | Reduce MTTR to < 15 min |
| Coverage drops | Add test cases, improve CI gates | Maintain ≥ 85% coverage |
| Slow response times | Optimize queries, add caching | Reduce P95 to < 100ms |
| Build failures | Infrastructure improvements, better logging | Achieve 99.95% success |
| Security issues | Dependency updates, code review | Maintain 0 critical |

## Escalation Procedures

### Escalation Path

1. **On-Call Engineer**: First responder (usually dev team)
2. **Team Lead**: Notified if P1 or SLO at risk
3. **Engineering Manager**: Notified if SLO breached
4. **Service Owner**: Notified for post-incident review

### Communication

- **Slack Channel**: #fantasy-baseball-incidents
- **Status Page**: GitHub Status (when available)
- **Downstream Services**: Notified of major incidents

## Related Documentation

- [DEPLOYMENT.md](DEPLOYMENT.md) - Release procedures
- [DEVOPS_RUNBOOK.md](DEVOPS_RUNBOOK.md) - Operational procedures
- [README.md](README.md) - Project overview
- [.github/agents/DevOpsExpert.agent.md](.github/agents/DevOpsExpert.agent.md) - DevOps guidance

## Review Schedule

- **Weekly**: Error budget consumption and trends
- **Monthly**: Full SLO/SLA review and DORA metrics
- **Quarterly**: Performance targets and SLO adjustments
- **Annually**: SLA commitments and capability assessment
