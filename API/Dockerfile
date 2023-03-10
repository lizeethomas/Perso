# Utiliser une image .NET SDK pour construire l'application
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# Définir le répertoire de travail
WORKDIR /app

# Copier les fichiers de l'application dans le conteneur
COPY . .

# Restaurer les packages NuGet et construire l'application
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Utiliser une image ASP.NET pour exécuter l'application
FROM mcr.microsoft.com/dotnet/aspnet:6.0

# Définir le répertoire de travail
WORKDIR /app

# Copier les fichiers de l'application construite dans le conteneur
COPY --from=build /app/out .

# Exposer le port 80 pour l'application
EXPOSE 4654

# Démarrer l'application au lancement du conteneur
ENTRYPOINT ["dotnet", "MyWebsite.dll"]