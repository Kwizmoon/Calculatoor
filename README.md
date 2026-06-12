# Calculatoor

**Cours :** 420-246-AH · Programmation Microsoft  
**Professeur :** Steve Lévesque  
**Cégep Ahuntsic**

---

## Description

Extension du TP2 : l'application console C# est devenue une **API REST ASP.NET Core** connectée à un **frontend web**. L'interface permet de s'authentifier, de saisir des expressions mathématiques, d'obtenir un résultat en temps réel, de consulter son historique personnel et de voir le classement des utilisateurs les plus actifs. Le tout est déployé dans le cloud.

---

## URLs de déploiement

| Service  | URL |
|----------|-----|
| Frontend | [https://calculator-web-sage-zeta.vercel.app/](https://calculatoor-mu.vercel.app/) |
| Backend  | [https://calculatoor-hphjfqabfcbkc8gr.canadacentral-01.azurewebsites.net](https://calculatoor-hphjfqabfcbkc8gr.canadacentral-01.azurewebsites.net/) |
| Swagger  | [https://calculatoor-hphjfqabfcbkc8gr.canadacentral-01.azurewebsites.net/swagger](https://calculatoor-hphjfqabfcbkc8gr.canadacentral-01.azurewebsites.net/swagger) |

---

## Architecture

```
Navigateur (Vercel)  |           | API REST (Azure)         |    | Base de données
index.html           |           | AuthController.cs        |    | EF Core
auth.html            |  →HTTP→   | CalculatorController.cs  | →  | Users
style.css            |           | LeaderboardController.cs |    | CalculationLogs
calculator.js        |           |                          |    |
auth.js              |           |                          |    |
```

---

## Structure du projet

```
TP2/
├── Frontend/                        ← Déployé sur Vercel
│   ├── index.html                   ← Calculatrice + classement
│   ├── auth.html                    ← Connexion / inscription
│   ├── style.css                    ← Thème clair/sombre, mise en page
│   ├── calculator.js                ← Logique calculatrice, historique, classement
│   ├── auth.js                      ← Logique authentification
│   ├── moon.png                     ← Icône thème clair
│   └── sun-icon-30.png              ← Icône thème sombre
│
└── Backend/                         ← Déployé sur Azure App Service
    ├── Program.cs                   ← Config, CORS, injection de dépendances
    ├── AuthController.cs            ← Inscription et connexion
    ├── CalculatorController.cs      ← Calcul et historique
    ├── LeaderboardController.cs     ← Classement et rang utilisateur
    └── CalculatriceLibrary/         ← Logique métier réutilisée du TP1
        ├── Calculator.cs            ← Évaluateur d'expressions
        ├── Models/
        │   ├── User.cs
        │   └── CalculationLog.cs
        └── Data/
            └── AppDbContext.cs
```

---

## Fonctionnalités

| Fonctionnalité           | Détail |
|--------------------------|--------|
| Inscription / Connexion  | Création de compte et authentification via l'API |
| Session persistante      | `userId` et `username` stockés dans le `localStorage` |
| Opérations de base       | Addition, soustraction, multiplication, division |
| Exposant 2 (x²)          | Enveloppe l'expression courante : `(expr)^2` |
| Exposant N (xⁿ)          | Ajoute `^` à l'expression pour saisir l'exposant |
| Racine carrée (√x)       | Enveloppe l'expression : `sqrt(expr)` |
| Parenthèses              | Boutons `(` et `)` pour grouper les sous-expressions |
| Calcul local             | Utilisateur non connecté : calcul via `eval()` en JavaScript |
| Calcul API               | Utilisateur connecté : calcul envoyé au backend et sauvegardé |
| Historique personnel     | Chargé au démarrage, mis à jour après chaque calcul |
| Suppression de log       | Suppression individuelle d'une entrée de l'historique |
| Classement               | Top 10 des utilisateurs, filtrable par période |
| Pagination               | Navigation page par page dans le classement |
| Rang personnel           | Affichage du rang et du total de calculs de l'utilisateur connecté |
| Thème clair / sombre     | Bascule persistante via `localStorage` |
| Validation d'expression  | Bloque l'envoi si l'expression se termine par un opérateur |
| Affichage d'erreurs      | Expressions invalides affichées en rouge |

---

## Endpoints de l'API

### Auth — `/auth`

| Méthode | Endpoint | Description |
|---------|----------|-------------|
| `POST` | `/auth/register` | Créer un compte utilisateur |
| `POST` | `/auth/login` | Connexion — retourne `userId` et `username` |

### Calculator — `/calculator`

| Méthode | Endpoint | Description |
|---------|----------|-------------|
| `POST` | `/calculator/calculer` | Évalue une expression et sauvegarde le résultat |
| `GET` | `/calculator/historique/{userId}` | Historique d'un utilisateur |
| `DELETE` | `/calculator/historique/{id}` | Supprime une entrée de l'historique |

### Leaderboard — `/api/leaderboard`

| Méthode | Endpoint | Paramètres | Description |
|---------|----------|------------|-------------|
| `GET` | `/api/leaderboard` | `limit`, `page`, `timespan` | Classement paginé et filtrable |
| `GET` | `/api/leaderboard/user/{userId}` | — | Rang et stats d'un utilisateur |

### Format des requêtes

**POST `/auth/login`**
```json
// Corps
{ "Username": "momo", "Password": "1234" }

// Réponse
{ "userId": 3, "username": "momo" }
```

**POST `/calculator/calculer`**
```json
// Corps
{ "Expression": "2+3*4", "UserId": 3 }

// Réponse
{ "res": 14 }
```

**GET `/api/leaderboard?limit=10&page=1&timespan=alltime`**
```json
[
  { "userId": 3, "username": "momo", "expressionCount": 42 }
]
```

**GET `/api/leaderboard/user/3`**
```json
{ "userId": 3, "rank": 1, "totalExpressions": 42 }
```

---

## Déploiement

Frontend — Vercel

Backend — Azure App Service

## DataBase

PostgreSQL - Azure Database for PostgreSQL



### CORS

Le backend autorise toutes les origines pour permettre les appels depuis Vercel :

```csharp
app.UseCors(policy => policy
    .AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod());
```

---

## Prérequis

### Développement
- Visual Studio 2022+
- .NET 8.0 SDK

### Déploiement
- Compte Vercel (gratuit)
- Compte Azure (abonnement étudiant ou gratuit)
