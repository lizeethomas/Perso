# Utiliser une image .NET SDK pour construire l'application
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# D�finir le r�pertoire de travail
WORKDIR /app

# Copier les fichiers de l'application dans le conteneur
COPY . .

# Restaurer les packages NuGet et construire l'application
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Utiliser une image ASP.NET pour ex�cuter l'application
FROM mcr.microsoft.com/dotnet/aspnet:6.0

# D�finir le r�pertoire de travail
WORKDIR /app

# Copier les fichiers de l'application construite dans le conteneur
COPY --from=build /app/out .

# Exposer le port 80 pour l'application
EXPOSE 4654

# D�marrer l'application au lancement du conteneur
ENTRYPOINT ["dotnet", "MyWebsite.dll"]