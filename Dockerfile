# syntax=docker/dockerfile:experimental
FROM nschultz/fantasy-baseball-common:2.0.1 AS build
COPY . /app
ARG VERSION
ENV MAIN_PROJ=FantasyBaseball.PositionService \
    SONAR_KEY=fantasy-baseball-position
RUN ./build.sh

FROM nschultz/base-csharp-runner:6.0.0
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "FantasyBaseball.PositionService.dll"]