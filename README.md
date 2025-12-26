# KineStat

Application web destinée aux **kinésithérapeutes**, utile dans la réalisation de **bilans cliniques** et dans la **détection de Red Flags** (signaux d’alerte nécessitant une orientation médicale).

---

## 📁 Structure du projet

```
MonAppWeb/
├── Controllers/       # Logique applicative (MVC)
├── Data/              # Accès et configuration de la base de données
├── Models/            # Classes et entités métier
├── Views/             # Interface WEB (Razor / cshtml)
├── wwwroot/           # Fichiers statiques (CSS, JS, images)
├── appsettings.json   # Configuration de l’application
├── Program.cs         # Point d’entrée
```

---

## 🔧 Configuration

* **Technologie** : ASP.NET Core (Model–View–Controller)
* **ORM** : Entity Framework Core
* **Base de données** : PostgreSQL

---

## 📝 Conventions de code

* **Langue** : Code et commentaires en anglais
* **Nommage** : `camelCase`
* **Documentation** : XML comments
* **Base de données** : `camelCase`
* **Style CSS** : [Bootswatch](https://https://bootswatch.com//) : Thème : Lux
* **JavaScript** : [AlpineJS](https://alpinejs.dev/)

---

## 🔀 Workflow Git

Le projet utilise une gestion **Git avec mainteneur**.

* Chaque développeur travaille sur sa branche
* Une **Merge Request** est obligatoire pour intégrer du code
* Le mainteneur valide ou refuse la MR après revue

### Branches

* `main` : branche principale stable

### Messages de commit

Convention **Karma** :

```
<type>(<scope>): <subject>

<body>

<footer>
```

**Exemple** :

```
fix(middleware): ensure Range headers adhere more closely to RFC 2616

Add one new dependency, use `range-parser` (Express dependency) to compute
range. It is more well-tested in the wild.

Fixes #2310
```

---

## ⚙️ Installation & Lancement

> ⚠️ Le projet doit être **récupéré via Git** avant toute installation. Les étapes ci-dessous décrivent le **processus complet depuis un poste vierge**.

### 1️⃣ Prérequis

Avant de commencer, assurez-vous d’avoir installé :

* **Git** (pour cloner le projet)
* **.NET SDK** (version compatible avec ASP.NET Core)
* **Entity Framework Core**

  * `Microsoft.EntityFrameworkCore`
  * `Microsoft.EntityFrameworkCore.Tools`
* **PostgreSQL 18** (serveur de base de données)
* **pgAdmin** (outil graphique de gestion PostgreSQL)
* **Visual Studio** 
* **OpenSSL** (nécessaire pour la configuration SSL PostgreSQL)

---

### 2️⃣ Récupération du projet (Git clone)

Le projet doit être cloné depuis le dépôt Git.

```bash
git clone <url-du-depot>
cd KineStat
```

---

### 3️⃣ Configuration de la base de données

La gestion de la base de données est réalisée avec **Entity Framework Core**.

1. Ouvrir **pgAdmin** et se connecter au serveur PostgreSQL
2. Créer une base de données (exemple : `kinestatDb`)
3. Vérifier les identifiants (utilisateur, mot de passe, port)
4. Renseigner la chaîne de connexion dans `appsettings.json` :

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=kinestatDb;Username=postgres;Password=yourPassword"
}
```

---

### 4️⃣ Installation du projet

Depuis le dossier racine du projet :

```bash
dotnet restore
```

Cette commande installe toutes les dépendances nécessaires, y compris **Entity Framework Core**.

---

### 5️⃣ Configuration SSL

#### SSL pour ASP.NET Core (HTTPS)

L’application nécessite un certificat HTTPS avant le premier lancement.

**Via Visual Studio** :

1. Ouvrir le projet dans **Visual Studio**
2. Au premier lancement, accepter l’installation du **certificat SSL de développement**
3. Vérifier que le profil de lancement utilise **HTTPS**

> ⚠️ Sans cette étape, l’application peut refuser de démarrer ou afficher des erreurs de sécurité.

---

#### SSL pour PostgreSQL

Afin de sécuriser la connexion entre l’application et PostgreSQL :

1. **Ouvrir PowerShell en tant qu’administrateur**
2. Aller dans le dossier `data` de PostgreSQL :

```powershell
cd "C:\Program Files\PostgreSQL\18\data"
```

3. **Générer la clé privée du serveur** :

```powershell
openssl genrsa -out server.key 2048
```

Si cette commande ne fonctionne pas, il se peut qu'il faut spécifier le chemin de l'installation d'`openssl`, comme ceci :  

```powershell
& "C:\Program Files\OpenSSL-Win64\bin\openssl.exe" genrsa -out server.key 2048
```

4. **Générer le certificat auto-signé** :

```powershell
openssl req -new -x509 -days 365 -key server.key -out server.crt -subj "/CN=localhost"
```  

Même chose que pour la commande précédente, si ça ne fonctionne pas, on peut utiliser la commande suivante :  

```powershell
& "C:\Program Files\OpenSSL-Win64\bin\openssl.exe" req -new -x509 -days 365 -key server.key -out server.crt -subj "/CN=localhost"
```  

5. **Sécuriser la clé privée** :

```powershell
icacls server.key /inheritance:r
icacls server.key /grant:r "NT AUTHORITY\\NetworkService:R"
```

6. **Activer SSL dans PostgreSQL** (`postgresql.conf`) :

```powershell
Add-Content -Path postgresql.conf -Value "ssl = on"
```

7. **Redémarrer le service PostgreSQL** :

   * `Windows + R` → `services.msc`
   * Redémarrer le service **postgresql-x64-18** (ou équivalent)

---

### 6️⃣ Migration et création de la base de données

Le projet utilise **Entity Framework Core avec migrations**.

**Via Visual Studio (Gestionnaire NuGet)** :

1. Ouvrir **Outils → Gestionnaire de package NuGet → Console du gestionnaire de package**
2. Vérifier le projet par défaut
3. Exécuter :

```powershell
Update-Database
```

Cette commande :

* applique les migrations
* crée automatiquement les tables
* met à jour la table `__EFMigrationsHistory`

---

### 7️⃣ Lancement de l’application

Le lancement de l’application se fait **directement via Visual Studio**.

1. Ouvrir le projet dans **Visual Studio**
2. Vérifier que le profil de lancement sélectionné est **HTTPS**
3. Cliquer sur le bouton **Run ▶️ (HTTPS)** en haut de l’IDE

Application accessible à l’adresse :

```
https://localhost:5001
```

---

## 📚 Documentation

Voir le dossier `Doc` du projet.

---

## 👥 Équipe

* Joanna Imjalli
* Melvyn Paul
* Noah Lassence
* Ryan Wilmart
* Sacha Meunier
* Jean Elly Fanoux

---

## 📄 Licence

MIT License — voir le fichier [LICENSE](LICENSE).
