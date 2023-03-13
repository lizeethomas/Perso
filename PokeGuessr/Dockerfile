# Utilise l'image de base .NET 6 SDK
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# Définit le répertoire de travail
WORKDIR /app

# Copie le fichier csproj et les sources de l'application dans le conteneur
COPY *.csproj ./
COPY . ./

# Restore les dépendances
RUN dotnet restore

# Build l'application
RUN dotnet publish -c Release -o out

# Utilise l'image de base .NET 6 ASP.NET runtime
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime

# Définit le répertoire de travail
WORKDIR /app

# Copie les fichiers de l'application compilée
COPY --from=build /app/out ./

# Expose le port 80 du conteneur
EXPOSE 80
EXPOSE 4654

# Lance l'application
ENTRYPOINT ["dotnet", "PokeGuessr.dll"]