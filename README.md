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
* **Style CSS** : [Bootswatch](https://bootswatch.com/) : Thème : Lux
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
* **Visual Studio 22/26** 
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
Si vous ne disposez pas de PostGreSQL, vous devez l'installer. Lors de l'installation, il faut choisir comme mot de passe **root**

---

### 4️⃣ Installation du projet

Depuis le dossier racine du projet :

```bash
dotnet restore
```

Cette commande installe toutes les dépendances nécessaires, y compris **Entity Framework Core**.

---

### 5️⃣ Configuration SSL

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
2. Exécuter :

```powershell
Update-Database
```
Cette commande :

* applique les migrations
* crée automatiquement les tables
* met à jour la table `__EFMigrationsHistory`

3. Pour remplir la BD, nous mettons à disposition 2 scripts SQL. Un des 2 contient uniquement un admin, et l'autre contient une fausse population.
vous pouvez copier un des scripts SQL fournis dans le dossier de remise du projet, et le coller dans pgAdmin afin de l'éxécuter (Clic droit sur KineStatDB -> QueryT ool).
Le script avec la population contient 4 patients qui seront automatiquement anonymisés par le service d'anonymisation au premier lancement de l'app :

- Alexandre LEROY (05-11-2000) - Inactif depuis 21 ans
- Marc JANSSENS (12-06-1970) - Inactif depuis 25 ans
- Clara Renard (25-01-1955) - Inactive depuis 30 ans
- Manon Bertrand (11-11-1982) - Inactive depuis 21 ans

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

## 👥 Équipe

* Joanna Imjalli (https://gitlab.com/Joanna_im)
* Melvyn Paul (https://gitlab.com/MelvynPaul)
* Noah Lassence (https://gitlab.com/Noah2901)
* Ryan Wilmart (https://gitlab.com/Ryanwii)
* Sacha Meunier (https://gitlab.com/Sacha_Meunier)
* Jean Elly Fanoux (https://gitlab.com/jfanoux)

---

## 📄 Licence

MIT License — voir le fichier [LICENSE](LICENSE).
