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

- Command for starting/stopping dev containers:

```
export app_version=$(cat version.txt) && docker compose -f _dev/docker-compose-dev.yaml -p fantasy-baseball-position up --build -d
export app_version=$(cat version.txt) && docker compose -f _dev/docker-compose-dev.yaml -p fantasy-baseball-position down
```

- Extensions are in the extensions.json file and should prompt to install on start
- Tasks are setup in tasks.json.

---

### Runtime Container

- Command for starting/stopping runtime containers:

```
docker compose -f _dev/docker-compose-runtime.yaml -p fantasy-baseball-position up --build -d
docker compose -f _dev/docker-compose-runtime.yaml -p fantasy-baseball-positiondown
```

---

### Local Connections

- Player API
  - View Swagger/Test Endpoints: http://localhost:8080/api/v1/position/swagger/index.html
- PG Admin
  - GUI: http://localhost:9000
  - Register a new connection and use `database` as the Host Name
  - Credentials found in compose file
- Postgres Database
  - Available at database:5432 (not outside of the containers)
