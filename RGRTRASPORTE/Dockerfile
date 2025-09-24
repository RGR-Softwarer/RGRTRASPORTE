FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /build

COPY ./RGRTRASPORTE/ ./RGRTRASPORTE/
#COPY ./Teste/ ./Teste/
COPY ./Service/ ./Service/
#COPY ./Jobs/ ./Jobs/
COPY ./Infra.Ioc/ ./Infra.Ioc/
COPY ./Infra.Data/ ./Infra.Data/
COPY ./Infra.CrossCutting/ ./Infra.CrossCutting/
COPY ./Dominio/ ./Dominio/
COPY ./Application/ ./Application/

WORKDIR /build/RGRTRASPORTE

RUN dotnet restore --verbosity normal

RUN dotnet publish "RGRTRASPORTE.csproj" -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0

ENV ASPNETCORE_ENVIRONMENT=Development

WORKDIR /app

COPY --from=build /app .

# Define o ponto de entrada para o cont�iner
ENTRYPOINT ["dotnet", "RGRTRASPORTE.dll"]
