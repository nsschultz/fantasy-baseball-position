#!/bin/bash
rm -rf coverage-results/
dotnet build
dotnet sonarscanner begin \
  /k:FantasyBaseball.PositionService \
  /n:fantasy-baseball-position \
  /v:$VERSION \
  /d:"sonar.cs.opencover.reportsPaths=coverage-results/coverage.opencover.xml" \
  $([ -n "Database/Migrations/*.cs" ] && echo "/d:sonar.coverage.exclusions=\"Database/Migrations/*.cs\"") \
  $([ -n "Database/Migrations/*.cs" ] && echo "/d:sonar.exclusions=\"Database/Migrations/*.cs\"") \
  /d:"sonar.host.url=$SONAR_URL"
dotnet test --no-build \
  "/p:CollectCoverage=true" \
  "/p:CoverletOutput=../coverage-results/" \
  "/p:CoverletOutputFormat=\"json,opencover\"" \
  "/p:MergeWith=../coverage-results/coverage.json"
dotnet publish -c Release -o /app/out -v minimal
dotnet sonarscanner end