# Rapport d'exécution des tests — ZooManager

## 1. Identification
- Projet : Zoo - Système de gestion du Zoo Municipal de Lyon
- Version testée : 2.0
- Auteur : Hisami Stolz, Alexandre Noens
- Date d'exécution : 2026-06-10
- Durée totale d'exécution : 2h30

## 2. Résumé exécutif
- Nombre total de tests : 25
- Tests réussis : 25 (100 %)
- Tests échoués : 0
- Tests ignorés : 0
- Couverture de lignes : 100 %
- Couverture de branches : 100 %
- Verdict global : [SUCCÈS]

## 3. Résultats détaillés par exigence
| Exigence  | Cas de test | Résultat | Durée  | Notes |
|-----------|-------------|----------|--------|-------|
| REQ-Z-001 | TC-001 - AddAnimal_ValidAnimal_ReturnsAssignedId | Réussi | < 1 ms | RAS |
| REQ-Z-002 | TC-002 - GetAnimal_ExistingId_ReturnsAnimal | Réussi | 14 ms | RAS |
| REQ-Z-003 | TC-003 - GetAnimal_UnknownId_ReturnsNull | Réussi | < 1 ms | RAS |
| REQ-Z-004 | TC-004 - TotalAnimals_AfterAddingThreeAnimals_ReturnsThree | Réussi | < 1 ms | RAS |
| REQ-Z-005 | TC-005 - AddAnimal_DuplicateId_ThrowsDuplicateAnimalException | Réussi | 10 ms | RAS |
| REQ-Z-006 | TC-006 - AddAnimal_AtMaxCapacity_ThrowsZooCapacityExceededException | Réussi | < 1 ms | RAS |
| REQ-Z-007 | TC-007 - TotalCapacityUsed_OneCriticalAnimal_CountsAsTwo | Réussi | < 1 ms | RAS |
| REQ-Z-008 | TC-008, TC-010 - CalculateDailyRation_ForACarnivore; CalculateDailyRation_ForAnOmnivore | Réussi | < 1 ms / < 1 ms | RAS |
| REQ-Z-009 | TC-009 - CalculateDailyRation_ForASickAnimalHerbivore | Réussi | < 1 ms | RAS |
| REQ-Z-010 | TC-012 - CalculateDailyCost_MultipleHealthyAnimals_ReturnsSumOfCategoryCosts | Réussi | < 1 ms | RAS |
| REQ-Z-011 | TC-013 - CalculateDailyCost_OneSickLion_Returns45Euros | Réussi | < 1 ms | RAS |
| REQ-Z-012 | TC-014 - CalculateDailyCost_OneCriticalLion_Returns75Euros | Réussi | < 1 ms | RAS |
| REQ-Z-013 | TC-016, TC-017 - GetCriticalAnimals_ThreeCriticalAndTwoHealthy_ReturnsThree; GetCriticalAnimals_NoCriticalAnimals_ReturnsEmptyList | Réussi | < 1 ms / 1 ms | RAS |
| REQ-Z-014 | TC-018 - RemoveAnimal_ExistingId_ReturnsTrue | Réussi | < 1 ms | RAS |
| REQ-Z-015 | TC-019 - RemoveAnimal_UnknownId_ReturnsFalse | Réussi | < 1 ms | RAS |
| REQ-Z-016 | TC-020 - CalculateMonthlyCost_ZooWithVariousAnimals_ReturnsExpectedMonthlyCost | Réussi | < 1 ms | RAS |
| REQ-Z-017 | TC-021 - GetAnimalsByCategory_MultipleAnimals_ReturnsOnlySpecifiedCategory | Réussi | 5 ms | RAS |

## 4. Anomalies détectées
« Aucune anomalie résiduelle. »

## 5. Métriques détaillées

### Distribution par type
- Tests nominaux : 21
- Tests d'erreur (exceptions) : 4
- Tests paramétrés [Theory] : 0

### Performance
- Test le plus rapide : < 1 ms
- Test le plus lent : 14 ms
- Durée moyenne par test : ~2,1 ms

## 6. Analyse de la couverture
- Lignes couvertes : 56 / 56
- Branches couvertes : 26 / 26
- Méthodes non couvertes : [« Aucune »]

## 7. Difficultés rencontrées
Le cycle Red-Green-Refactor a ete respecte sur l'ensemble des exigences implementees.
Nous n'avons pas ete tentes de coder avant d'ecrire les tests, ce qui a aide a garder une demarche TDD rigoureuse.
La progression a ete lineaire et aucune exigence n'a necessite de repasser par plusieurs cycles Red-Green consecutifs.
La principale difficulte a concerne l'evolution du type numerique dans `CalculateDailyRation`.
Une phase de refactorisation a ete necessaire pour passer d'un traitement en `double` a un traitement en `decimal`.
Ce changement a impose l'ajustement des assertions de tests afin de conserver la precision attendue sur les rations.
Apres ce refactoring, la suite de tests est restee stable et la couverture est demeuree complete.

## 8. Conclusion et recommandations
Le code est pret pour une mise en production sur le perimetre metier couvert par la suite de tests actuelle.
Les resultats obtenus (25/25 tests reussis et couverture complete) confirment la stabilite de l'implementation.
Des ameliorations restent toutefois recommandees pour renforcer la maintenabilite.
Une refactorisation complementaire est envisageable sur certaines zones du code.
Il est notamment conseille de revoir les types de retour de certaines fonctions lorsque ceux-ci ne sont pas pleinement adaptes au besoin metier.
Enfin, l'automatisation du formatage au moment du commit (hook pre-commit ou pipeline CI) permettrait de garantir une base de code homogène et de limiter les ecarts de style.

## 9. Signature
Auteur du rapport : Hisami Stolz, Alexandre Noens
Validé par
: [À remplir par le formateur]
