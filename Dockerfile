FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
RUN dotnet --version
WORKDIR /source

# copy src and restore as distinct layers
COPY ./src .
RUN dotnet restore

# copy and publish app and libraries
COPY ./src .

RUN dotnet publish --no-restore -c Release -o /app --no-cache /restore

# final stage/image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build /app ./
EXPOSE 5000