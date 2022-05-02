FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Sample.OpenTelemetry.Api/Sample.OpenTelemetry.Api.csproj", "Sample.OpenTelemetry.Api/"]
RUN dotnet restore "Sample.OpenTelemetry.Api/Sample.OpenTelemetry.Api.csproj"
COPY . .
WORKDIR "/src/Sample.OpenTelemetry.Api"
RUN dotnet build "Sample.OpenTelemetry.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Sample.OpenTelemetry.Api.csproj" -c Release -o /app/publish

FROM base AS final

ENV TZ=America/Sao_Paulo
ENV LANG pt-BR
ENV LANGUAGE pt-BR
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sample.OpenTelemetry.Api.dll"]