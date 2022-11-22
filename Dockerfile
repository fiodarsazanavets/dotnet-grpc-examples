FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env

RUN dotnet dev-certs https -ep localhost.crt --format PEM
COPY localhost.crt /usr/lib/ssl/certs/aspnetcore-https-localhost.pem

EXPOSE 7100
EXPOSE 5100
EXPOSE 5000