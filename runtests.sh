#!/bin/sh
[ -d "./coverage" ] && echo "deleting old coverage" && rm -rf ./coverage
[ -d "./coveragereport" ] && echo "deleting old coverage report" && rm -rf ./coveragereport
dotnet test --collect:"XPlat Code Coverage" --results-directory ./coverage
reportgenerator -reports:"./coverage/**/*.xml" -targetdir:"coveragereport" -reporttypes:"Html"