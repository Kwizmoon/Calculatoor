# Calculatrice Avancée WEB— TP2

**Cours :** 420-246-AH · Programmation Microsoft  
**Professeur :** Steve Lévesque  
**Cégep Ahuntsic**

---

URL du projet : https://calculator-web-sage-zeta.vercel.app/ 

## Membres de l'équipe

| Prénom            | Nom       | Matricule |
|-------------------|-----------|-----------|
| Nanou Ange Robert | Kouassi   | 2007471   |
| Mohamed Sabry     | Banwan    | 2252825   |
| Bao Tran          | Bach      | 1642345   |
| Mohammed Amine    | Lemgandez | 2257191   |

---

## Description

Extension du TP1 : l'application console C# est devenue une **API REST ASP.NET Core** connectée à un **frontend web**. L'interface permet de saisir des expressions mathématiques, d'obtenir un résultat en temps réel et de consulter l'historique des calculs. Le tout est déployé dans le cloud.

---

## URLs de déploiement

| Service  |                                     URL                                           |
|----------|-----------------------------------------------------------------------------------|
| Frontend | https://calculator-web-sage-zeta.vercel.app/                                      |← URL du site web
| Backend  | https://calculatortp2-eca3egh7gffhdpfy.canadacentral-01.azurewebsites.net         |
| Swagger  | https://calculatortp2-eca3egh7gffhdpfy.canadacentral-01.azurewebsites.net/swagger |← API avec requêtes HTTP

---

## Architecture

```
Navigateur (Vercel) |           | API REST (Azure)        |    | Base de données
index.html          |           | CalculatorController.cs |    | EF Core In-Memory
style.css           |  →HTTP→   | /calculator/calculer    | →  | CalculationLogs
script.js           |           | /calculator/historique  |    |
```

---

## Structure du projet

```
TP2/
├── Frontend/                        ← Déployé sur Vercel
│   ├── index.html                   ← Structure de la page
│   ├── style.css                    ← Thème clair/sombre, mise en page
│   ├── script.js                    ← Logique, appels API, historique
│   ├── moon.png                     ← Icône thème
│   └── sun-icon-30.png              ← Icône thème
│
└── Backend/                         ← Déployé sur Azure App Service
    ├── Program.cs                   ← Config, CORS, injection de dépendances
    ├── CalculatorController.cs      ← Endpoints REST
    └── CalculatriceLibrary/         ← Logique métier réutilisée du TP1
        ├── Calculator.cs            ← Évaluateur d'expressions
        ├── Models/
        │   └── CalculationLog.cs
        └── Data/
            └── AppDbContext.cs
```

---

## Fonctionnalités

| Fonctionnalité          |                            Détail                             |
|-------------------------|---------------------------------------------------------------|
| Opérations de base      | Addition, soustraction, multiplication, division              |
| Exposant 2 (x²)         | Enveloppe l'expression courante : `(expr)^2`                  |
| Exposant N (xⁿ)         | Ajoute `^` à l'expression pour saisir l'exposant              |
| Racine carrée (√x)      | Enveloppe l'expression : `sqrt(expr)`                         |
| Parenthèses             | Boutons `(` et `)` pour grouper les sous-expressions          |
| Thème clair / sombre    | Bascule via un bouton en haut à droite                        |
| Validation d'expression | Bloque l'envoi si l'expression se termine par un opérateur    |
| Historique              | Chargé au démarrage, mis à jour après chaque calcul           |
| Supprimer log           | Supprimer un log dans l'historique avec son id                |
| Affichage d'erreurs     | Division par zéro et expressions invalides affichées en rouge |

---

## Endpoints de l'API

| Méthode | Endpoint                      |                            Description                        |
|---------|-------------------------------|---------------------------------------------------------------|
| `POST`  | `/calculator/calculer`        | Évalue une expression mathématique et sauvegarde le résultat  |
| `GET`   | `/calculator/historique`      | Retourne l'historique des calculs triés par ordre décroissant |
| `DELETE`| `/calculator/historique/{id}` | Supprime un calcul spécifique de l'historique                 |

### Format des requêtes

**POST `/calculator/calculer`**
```json
// Corps de la requête
{ "expression": "2+3*4" }

// Réponse
{ "res": 14 }
```

**GET `/calculator/historique`**
```json
[
  {
    "id": 1,
    "expression": "2+3*4",
    "result": "14",
    "createdAt": "2025-01-01T12:00:00"
  }
]
```

---

## Déploiement

### Frontend — Vercel

1. Créer un compte sur [vercel.com](https://vercel.com) et lier le dépôt GitHub
2. Importer le projet (les fichiers `index.html`, `style.css`, `script.js` + images)
3. Vercel détecte automatiquement un projet statique — aucune configuration requise
4. Cliquer sur **Deploy** — l'URL publique est générée automatiquement

### Backend — Azure App Service

1. Dans Visual Studio : clic droit sur le projet API → **Publish**
2. Choisir **Azure** → **Azure App Service (Windows)**
3. Créer une nouvelle App Service (région Canada Central recommandée)
4. Laisser les paramètres par défaut — EF Core In-Memory ne nécessite pas de base de données externe
5. Cliquer sur **Publish** — Azure génère l'URL du service
6. Copier l'URL dans la constante `API_URL` de `script.js` et redéployer sur Vercel

### CORS

Le backend autorise toutes les origines pour permettre les appels depuis Vercel :

```C#
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
- Compte GitHub (pour Vercel)

### Déploiement
- Compte Vercel (gratuit)
- Compte Azure (abonnement étudiant ou gratuit)
