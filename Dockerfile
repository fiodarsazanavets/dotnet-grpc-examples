FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env

RUN dotnet dev-certs https --trust

EXPOSE 7100
EXPOSE 5100
EXPOSE 5000