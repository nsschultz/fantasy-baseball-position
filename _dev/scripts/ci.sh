#!/bin/bash
set -e
EXIT_CODE=0
docker compose -f _dev/docker-compose-ci.yaml -p fantasy-baseball-position up --build --exit-code-from api || EXIT_CODE=$?
docker compose -f _dev/docker-compose-ci.yaml -p fantasy-baseball-position down
docker volume rm fantasy-baseball-position_data_volume
exit $EXIT_CODE