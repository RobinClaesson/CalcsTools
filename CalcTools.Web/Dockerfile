FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["/CalcTools.Web/CalcTools.Web.csproj", "CalcTools.Web/"]
COPY ["/CalcTools.Core/CalcTools.Core.csproj", "CalcTools.Core/"]
RUN dotnet restore "CalcTools.Web/CalcTools.Web.csproj"
COPY . .
WORKDIR "/src/CalcTools.Web"
RUN dotnet build "./CalcTools.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CalcTools.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM nginx:alpine AS final
WORKDIR /var/www/web
COPY --from=publish /app/publish/wwwroot .
COPY ["/CalcTools.Web/nginx.conf", "/etc/nginx/nginx.conf"]
EXPOSE 80
