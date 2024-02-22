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

- VS Code should auto-prompt to reopen the workspace in a contaienr, which will start the rest of the containers as well.
- Tasks are setup in tasks.json.
- Command for manually starting/stopping dev containers:

```
docker compose -f .docker-compose/docker-compose-dev.yaml -p fantasy-baseball-position up --build -d
docker compose -f .docker-compose/docker-compose-dev.yaml -p fantasy-baseball-position down
```

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
  - View Swagger/Test Endpoints: http://localhost:8080/api/v1/position/swagger/index.html
- PG Admin
  - GUI: http://localhost:9000
- Postgres Database
  - Available at database:5432 (not outside of the containers)
