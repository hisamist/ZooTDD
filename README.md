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
