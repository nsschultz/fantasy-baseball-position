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
docker run --rm -it --workdir /app -v $(pwd):/app nschultz/fantasy-baseball-common:1.0.3 bash
```
* Commands for installing VS Code extensions (from within container):
```
code --install-extension Fudge.auto-using
code --install-extension ms-dotnettools.csharp
```
* Dotnet Commands for Restore, Build, Test & Run
```
dotnet restore
dotnet build
dotnet test
dotnet run --project FantasyBaseball.PositionService/FantasyBaseball.PositionService.csproj
```
* View Swagger/Test Endpoints: http://localhost:5000/api/v1/projection/swagger/index.html