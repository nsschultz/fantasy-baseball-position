FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:10.0 AS dev
ARG TARGETARCH
ENV PATH="$PATH:/root/.dotnet/tools"
WORKDIR /app

FROM dev AS ci
RUN mkdir -p /usr/share/man/man1 /usr/share/man/man2
RUN apt-get update && apt-get install -y --no-install-recommends default-jre && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*
RUN dotnet tool install --global dotnet-sonarscanner --version 6.2.0
ENV DOTNET_ROLL_FORWARD=Major

FROM dev AS build
COPY FantasyBaseball.PositionService/FantasyBaseball.PositionService.csproj .
RUN dotnet restore -a "$TARGETARCH"
COPY FantasyBaseball.PositionService/ .
RUN dotnet publish -c Release -a "$TARGETARCH" --no-restore -o /app/out -v minimal

FROM mcr.microsoft.com/dotnet/aspnet:10.0
RUN apt-get update && \
    apt-get install -y --no-install-recommends curl && \
    rm -rf /var/lib/apt/lists/*
RUN useradd -u 5000 service-user && mkdir /app && chown -R service-user:service-user /app
ENV ASPNETCORE_URLS=http://+:8080
USER service-user:service-user
WORKDIR /app
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "FantasyBaseball.PositionService.dll"]