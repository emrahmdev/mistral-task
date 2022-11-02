#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["backend/backend.csproj", "backend/"]
RUN dotnet restore "backend/backend.csproj"
COPY . .
WORKDIR "/src/backend"
RUN dotnet build "backend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "backend.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN  apt-get update \
  && apt-get install -y wget
RUN apt-get -q update && apt-get -qy install netcat \
  && rm -rf /var/lib/apt/lists/*

RUN wget -qO- https://raw.githubusercontent.com/eficode/wait-for/v2.2.3/wait-for -O wait-for
RUN chmod +x wait-for

ENTRYPOINT ["./wait-for","sql_db:1433", "-t", "20","--","dotnet", "backend.dll"]