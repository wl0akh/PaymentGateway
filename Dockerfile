FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
# copy src and restore as distinct layers
COPY ./ .