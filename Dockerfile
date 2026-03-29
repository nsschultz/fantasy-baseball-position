FROM --platform=$BUILDPLATFORM nschultz/fantasy-baseball-common-backend:1.0.1 AS dev
ARG TARGETARCH

FROM dev AS ci
COPY --chown=appuser:appuser . .

FROM dev AS build
COPY FantasyBaseball.PositionService/FantasyBaseball.PositionService.csproj .
RUN dotnet restore -a "$TARGETARCH"
COPY FantasyBaseball.PositionService/ .
RUN dotnet publish -c Release -a "$TARGETARCH" --no-restore -o /app/out -v minimal

FROM mcr.microsoft.com/dotnet/aspnet:10.0
RUN apt-get update && \
    apt-get install -y --no-install-recommends curl && \
    rm -rf /var/lib/apt/lists/* && \
    useradd -u 5000 service-user && mkdir /app && chown -R service-user:service-user /app
ENV ASPNETCORE_URLS=http://+:8080
USER service-user:service-user
WORKDIR /app
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "FantasyBaseball.PositionService.dll"]