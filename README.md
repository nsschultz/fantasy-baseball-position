## Position Service
* This service is the source of truth for information about positions.
* It is a readonly service as of now.

---
### Healthcheck:
* The service will fail a healthcheck if database cannot be accessed.  

---
### Dev Container
* Command for starting/stopping dev container:
```
docker compose -f compose/docker-compose-dev.yaml up
docker compose -f compose/docker-compose-dev.yaml down
```
* Extensions are in the extensions.json file and should prompt to install on start
* Tasks are setup in tasks.json.

---
### Runtime Container
* Command for starting/stopping runtime container:
```
docker compose -f compose/docker-compose-runtime.yaml up
docker compose -f compose/docker-compose-runtime.yaml down
```
* View Swagger/Test Endpoints: http://localhost:8080/api/v1/position/swagger/index.html

---
### Build Image
```
docker build -t nschultz/fantasy-baseball-position:1.0.0 --build-arg VERSION=1.0.0 .
```