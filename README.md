# ZooTDD

Projet .NET avec une librairie de domaine et des tests unitaires.

## Prerequis

- .NET SDK installe (`dotnet --version`)
- Outil de formatage .NET installe (`dotnet format`)

Installation (globale) de `dotnet-format` :

```bash
dotnet tool install -g dotnet-format
```

## Commandes de base

Depuis la racine du repo:

```bash
# Restaurer les dependances
dotnet restore

# Compiler la solution
dotnet build

# Lancer les tests
dotnet test

# Formater le code
dotnet format
```

## Couverture de code

Depuis la racine du repo:

```bash
# Ajouter le collecteur de couverture au projet de tests
dotnet add src/Zoo.UnitTests/Zoo.UnitTests.csproj package coverlet.collector

# Executer les tests avec collecte de couverture
dotnet test --collect:"XPlat Code Coverage"

# Installer l'outil de generation de rapport HTML
dotnet tool install -g dotnet-reportgenerator-globaltool

# Generer un rapport HTML
reportgenerator \
	-reports:**/coverage.cobertura.xml \
	-targetdir:coveragereport \
	-reporttypes:Html

# Ouvrir le rapport
xdg-open coveragereport/index.html
```
