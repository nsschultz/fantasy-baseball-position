## Position Service
* This service is the source of truth for information about positions.
* It is a readonly service as of now.

---
### Healthcheck:
* The service will fail a healthcheck if database cannot be accessed.  

---
### Dev Container
* Command for starting container:
```
docker run --rm -it --workdir /app -v $(pwd):/app nschultz/fantasy-baseball-common:2.0.1 bash
```
* Extensions are in the extensions.json file and should prompt to install on start
* Tasks are setup in tasks.json.
* View Swagger/Test Endpoints: http://localhost:5000/api/v1/position/swagger/index.html

---
### Build Image
```
docker build -t nschultz/fantasy-baseball-position:1.0.0 --build-arg VERSION=1.0.0 .
```