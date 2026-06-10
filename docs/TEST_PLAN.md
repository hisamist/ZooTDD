# Plan de test — ZooManager

## 1. Identification
- Projet : Zoo - Système de gestion du Zoo Municipal de Lyon
- Version : 2.0
- Auteur : Hisami Stolz
- Date : 2026-06-10
- Statut : Brouillon

## 2. Périmètre

### In scope
- Classe `ZooManager` (toutes les méthodes/propriétés publiques : `AddAnimal`, `GetAnimal`, `TotalAnimals`, `TotalCapacityUsed`, `CalculateDailyRation`, `CalculateDailyCost`, `GetCriticalAnimals`, `RemoveAnimal`)
- Classes `Animal`, `AnimalCategory`, `HealthStatus`
- Exceptions métier (`DuplicateAnimalException`, `ZooCapacityExceededException`)
- Règles métier : régimes/rations/coûts par catégorie, impact du statut de santé (Sick/Critical) sur la ration et le coût, capacité du zoo (50 animaux, Critical = 2 places), unicité des identifiants

### Out of scope
- Persistance en base de données
- Interface utilisateur
- API REST

## 3. Stratégie
- Méthodologie : Test-Driven Development (Red - Green - Refactor)
- Framework : xUnit + FluentAssertions
- Couverture cible : 100 % sur `ZooManager`

## 4. Critères d'entrée
- Spécifications validées (cf. section IV du sujet)
- Squelette de classes fourni (`AnimalCategory`, `HealthStatus`, `Animal`, `Exceptions`, `ZooManager`)

## 5. Critères de sortie
- 100 % des 15 exigences (REQ-Z-001 à REQ-Z-015) couvertes par au moins un test
- Tous les tests verts
- Couverture de lignes >= 95 % sur `ZooManager`

## 6. Environnement
- .NET 10
- xUnit 2.9.3
- FluentAssertions 8.10.0
- coverlet.collector 6.0.4 (couverture de code)

## 7. Cas de test prévus

TC-001 — Ajouter un carnivore retourne son ID
  Entrée : `Animal { Id=1, Name="Simba", Category=Carnivore }`
  Attendu : `1`
  REQ : REQ-Z-001

TC-002 — Récupérer un animal existant retourne l'animal
  Entrée : `Id=1` (après TC-001)
  Attendu : l'instance `Animal` correspondant à Simba (Id=1, Name="Simba", Category=Carnivore)
  REQ : REQ-Z-002

TC-003 — Récupérer un animal inexistant retourne null
  Entrée : `Id=999` (zoo vide ou ne contenant pas cet id)
  Attendu : `null`
  REQ : REQ-Z-003

TC-004 — Le total d'animaux reflète les ajouts
  Entrée : ajout de 3 animaux (Id=1,2,3)
  Attendu : `TotalAnimals == 3`
  REQ : REQ-Z-004

TC-005 — Ajouter un animal avec un ID déjà utilisé lève une exception
  Entrée : `Animal { Id=1, Name="Simba", Category=Carnivore }` déjà ajouté, puis `Animal { Id=1, Name="Nala", Category=Carnivore }`
  Attendu : `DuplicateAnimalException` avec message contenant "1 already exists"
  REQ : REQ-Z-005

TC-006 — Ajouter un animal au-delà de la capacité maximale lève une exception
  Entrée : zoo contenant déjà 50 animaux Healthy (Id=1..50), ajout d'un 51e animal (Id=51)
  Attendu : `ZooCapacityExceededException`
  REQ : REQ-Z-006

TC-007 — Un animal Critical occupe 2 places dans le calcul de capacité
  Entrée : 1 animal Healthy (Id=1) + 1 animal Critical (Id=2)
  Attendu : `TotalCapacityUsed == 3` (1 + 2)
  REQ : REQ-Z-007

TC-008 — Ration quotidienne d'un carnivore en bonne santé
  Entrée : `Animal { Category=Carnivore, Status=Healthy }`
  Attendu : `5.0` (kg)
  REQ : REQ-Z-008

TC-009 — Ration quotidienne d'un herbivore malade (-30 %)
  Entrée : `Animal { Category=Herbivore, Status=Sick }`
  Attendu : `7.0` (kg) → 10 kg × 0.7
  REQ : REQ-Z-009

TC-010 — Ration quotidienne d'un omnivore en bonne santé
  Entrée : `Animal { Category=Omnivore, Status=Healthy }`
  Attendu : `7.0` (kg)
  REQ : REQ-Z-008 (couverture complémentaire de la table des régimes)

TC-011 — Coût quotidien total d'un zoo vide
  Entrée : `ZooManager` sans animaux
  Attendu : `0.0` €
  REQ : REQ-Z-010

TC-012 — Coût quotidien total avec un herbivore en bonne santé
  Entrée : 1 `Animal { Category=Herbivore, Status=Healthy }`
  Attendu : `8.0` €
  REQ : REQ-Z-010

TC-013 — Un animal Sick coûte +20 € par jour
  Entrée : 1 `Animal { Category=Carnivore, Status=Sick }`
  Attendu : `45.0` € (25 € + 20 €)
  REQ : REQ-Z-011

TC-014 — Un animal Critical coûte +50 € par jour
  Entrée : 1 `Animal { Category=Carnivore, Status=Critical }`
  Attendu : `75.0` € (25 € + 50 €)
  REQ : REQ-Z-012

TC-015 — Coût quotidien total avec plusieurs animaux de statuts différents
  Entrée : 1 Carnivore Healthy (25 €) + 1 Herbivore Sick (8 € + 20 €) + 1 Omnivore Critical (15 € + 50 €)
  Attendu : `118.0` €
  REQ : REQ-Z-010

TC-016 — Liste des animaux Critical
  Entrée : zoo avec 3 animaux Critical et 2 animaux Healthy
  Attendu : liste de 3 animaux, tous avec `Status == Critical`
  REQ : REQ-Z-013

TC-017 — Liste des animaux Critical quand aucun n'est Critical
  Entrée : zoo avec uniquement des animaux Healthy
  Attendu : liste vide
  REQ : REQ-Z-013 (cas limite complémentaire)

TC-018 — Retirer un animal existant
  Entrée : `Id=1` (après ajout)
  Attendu : `true`, et `TotalAnimals` décrémenté de 1
  REQ : REQ-Z-014

TC-019 — Retirer un animal inexistant
  Entrée : `Id=999`
  Attendu : `false`
  REQ : REQ-Z-015

TC-020 — Calculer le coût mensuel (30 jours) du zoo
  Entrée : zoo contenant 1 Carnivore Healthy (25 €) + 1 Herbivore Sick (8 € + 20 €) + 1 Omnivore Critical (15 € + 50 €)
  Attendu : `3540.0` € (coût quotidien `118.0` € × 30 jours)
  REQ : REQ-Z-016

TC-021 — Retourner les animaux par catégorie alimentaire.
  Entrée : zoo avec 2 Herbivores, 1 Carnivore, 1 Omnivore ; filtre sur `AnimalCategory.Herbivore`
  Attendu : liste de 2 animaux, tous avec `Category == Herbivore`
  REQ : REQ-Z-017

## 8. Matrice de traçabilité

| Exigence  | Cas de test            | Statut prévu |
|-----------|------------------------|--------------|
| REQ-Z-001 | TC-001                 | À faire      |
| REQ-Z-002 | TC-002                 | À faire      |
| REQ-Z-003 | TC-003                 | À faire      |
| REQ-Z-004 | TC-004                 | À faire      |
| REQ-Z-005 | TC-005                 | À faire      |
| REQ-Z-006 | TC-006                 | À faire      |
| REQ-Z-007 | TC-007                 | À faire      |
| REQ-Z-008 | TC-008, TC-010         | À faire      |
| REQ-Z-009 | TC-009                 | À faire      |
| REQ-Z-010 | TC-011, TC-012, TC-015 | À faire      |
| REQ-Z-011 | TC-013                 | À faire      |
| REQ-Z-012 | TC-014                 | À faire      |
| REQ-Z-013 | TC-016, TC-017         | À faire      |
| REQ-Z-014 | TC-018                 | À faire      |
| REQ-Z-015 | TC-019                 | À faire      |
| REQ-Z-016 | TC-020                 | À faire      |
| REQ-Z-017 | TC-021                 | À faire      |

## 9. Risques

| Risque | Probabilité | Impact | Mitigation |
|--------|-------------|--------|------------|
| Lien ambigu entre la capacité max de 50 animaux (REQ-Z-006) et la règle « Critical = 2 places » (REQ-Z-007) | Moyenne | Moyen | Hypothèse retenue : `AddAnimal` limite `TotalAnimals` à 50 ; `TotalCapacityUsed` est une métrique indépendante (peut dépasser 50) reflétant l'occupation réelle des enclos. À confirmer avec le formateur. |
| La « ration de soin » d'un animal Critical n'a pas de valeur chiffrée dans le sujet | Moyenne | Moyen | Hypothèse retenue : ration normale (100 % de la ration de base) tant qu'aucune autre valeur n'est précisée. À confirmer avec le formateur. |
| Ordre de vérification doublon vs capacité dans `AddAnimal` | Faible | Faible | Vérifier l'unicité de l'ID avant la capacité (un doublon est rejeté même si le zoo est plein). |
| Précision flottante sur les rations/coûts (ex. -30 %) | Faible | Faible | Utiliser `Should().BeApproximately()` si nécessaire pour les comparaisons de `double`. |
