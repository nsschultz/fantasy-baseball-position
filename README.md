[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=nsschultz_fantasy-baseball-position&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=nsschultz_fantasy-baseball-position)

## Position Service

- This service is the source of truth for information about positions.
- It is a readonly service as of now.

---

### Healthcheck:

- The service will fail a healthcheck if database cannot be accessed.

---

### Build Image

```
version=$(cat version.txt) && docker build -t nschultz/fantasy-baseball-position:$version .
```

---

### Dev Container

### Dev Container

- In VS Code, use the "Clone Repository into Container Volume..." option to open the workspace.
- Tasks are setup in tasks.json.

---

### Runtime Container

- Command for starting/stopping runtime containers:

```
docker compose -f .docker-compose/docker-compose-runtime.yaml -p fantasy-baseball-position up --build -d
docker compose -f .docker-compose/docker-compose-runtime.yaml -p fantasy-baseball-position down
```

---

### Local Connections

- Player API
  - View Swagger/Test Endpoints: http://localhost:8080/api/swagger/index.html
- PG Admin
  - GUI: http://localhost:9000
- Postgres Database
  - Available at database:5432 (not outside of the containers)
