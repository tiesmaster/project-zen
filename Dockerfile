FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build

WORKDIR /app
COPY . .

RUN dotnet restore
RUN dotnet build
RUN dotnet test