
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
User app
WORKDIR /app
EXPOSE 8080


FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Project.Api", "Project.Api/" ]
COPY ["Project.Logic", "Project.Logic/" ]
COPY ["Project.Repo", "Project.Repo/" ]

RUN dotnet restore "Project.Api/Project.Api.csproj"

RUN dotnet build "Project.Api/Project.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Project.Api/Project.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Project.Api.dll"]
