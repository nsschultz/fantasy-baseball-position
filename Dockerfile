# syntax=docker/dockerfile:1
FROM nschultz/fantasy-baseball-common:2.0.6 AS build
COPY . /app
RUN dotnet publish -c Release -o /app/out -v minimal

FROM nschultz/base-csharp-runner:6.0.10
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "FantasyBaseball.PositionService.dll"]