#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Collectio.Presentation/Collectio.Presentation.csproj", "Collectio.Presentation/"]
COPY ["Collectio.Application/Collectio.Application.csproj", "Collectio.Application/"]
COPY ["Collectio.Domain/Collectio.Domain.csproj", "Collectio.Domain/"]
COPY ["Collectio.Infra.CrossCutting.Services/Collectio.Infra.CrossCutting.Services.csproj", "Collectio.Infra.CrossCutting.Services/"]
COPY ["Collectio.Infra.CrossCutting.Ioc/Collectio.Infra.CrossCutting.Ioc.csproj", "Collectio.Infra.CrossCutting.Ioc/"]
COPY ["Collectio.Infra.Data/Collectio.Infra.Data.csproj", "Collectio.Infra.Data/"]
RUN dotnet restore "Collectio.Presentation/Collectio.Presentation.csproj"
COPY . .
WORKDIR "/src/Collectio.Presentation"
RUN dotnet build "Collectio.Presentation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Collectio.Presentation.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Collectio.Presentation.dll"]