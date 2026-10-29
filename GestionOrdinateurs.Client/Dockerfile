# =========================
# Build
# =========================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ../GestionOrdinateurs.Client/GestionOrdinateurs.Client.csproj ./GestionOrdinateurs.Client/
COPY ../GestionOrdinateurs.Shared/GestionOrdinateurs.Shared.csproj ./GestionOrdinateurs.Shared/
RUN dotnet restore ./GestionOrdinateurs.Client
RUN dotnet restore ./GestionOrdinateurs.Shared

# Copy the remaining source code
COPY ./GestionOrdinateurs.Client/ ./GestionOrdinateurs.Client/
COPY ./GestionOrdinateurs.Shared/ ./GestionOrdinateurs.Shared/
RUN dotnet publish ./GestionOrdinateurs.Client -c Release -o out

# Runtime image : nginx sert les fichiers du front
FROM nginx:alpine AS runtime
COPY --from=build /app/out/wwwroot /usr/share/nginx/html
COPY ./GestionOrdinateurs.Client/nginx.conf /etc/nginx/conf.d/default.conf

# Expose port 5000
EXPOSE 5000