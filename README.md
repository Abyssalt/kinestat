# KineStat

Application web destinée aux **kinésithérapeutes**, utile dans la réalisation de **bilans cliniques** et dans la **détection de Red Flags** (signaux d'alerte nécessitant une orientation médicale).

---

## 📁 Structure du projet

```
MonAppWeb/
├── Controllers/       # Logique applicative (MVC)
├── Data/              # Accès et configuration de la base de données
├── Models/            # Classes et entités métier
├── Views/             # Interface WEB (Razor / cshtml)
├── wwwroot/           # Fichiers statiques (CSS, JS, images)
├── appsettings.json   # Configuration de l'application
├── Program.cs         # Point d'entrée
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

## 🚀 Démarrage rapide (Docker)

Aucune installation de .NET ni de PostgreSQL n'est nécessaire : **seul Docker est requis**. Tout le reste (SDK, base de données, migrations, données de démonstration) est géré automatiquement par les conteneurs.

### Prérequis selon votre système

#### 🪟 Windows 10/11

* [Docker Desktop pour Windows](https://www.docker.com/products/docker-desktop/)
* **WSL 2** activé (Docker Desktop le propose à l'installation ; sinon : `wsl --install` dans un PowerShell administrateur, puis redémarrer)
* La **virtualisation** activée dans le BIOS/UEFI (Intel VT-x / AMD-V — activée par défaut sur la plupart des machines récentes)
* Docker Desktop en mode **Linux containers** (c'est le mode par défaut ; vérifiable par clic droit sur l'icône Docker dans la barre des tâches)

#### 🍎 macOS (Intel et Apple Silicon)

* [Docker Desktop pour Mac](https://www.docker.com/products/docker-desktop/) — choisir la version correspondant à votre puce (Intel ou Apple Silicon)
* Toutes les images du projet sont multi-architecture (amd64 + arm64) : aucun réglage supplémentaire n'est nécessaire, y compris sur M1/M2/M3/M4

#### 🐧 Linux

* **Docker Engine** et le **plugin Compose v2** :

```bash
  # Debian / Ubuntu
  sudo apt install docker.io docker-compose-v2
  sudo usermod -aG docker $USER   # puis se déconnecter/reconnecter
```

  (ou suivre la [documentation officielle](https://docs.docker.com/engine/install/) pour votre distribution)
* ⚠️ Ce guide utilise la commande `docker compose` (avec **espace**, Compose v2). L'ancien binaire `docker-compose` v1 (avec tiret) n'est pas supporté.

#### Dans tous les cas

* Le port **8080** de votre machine doit être libre (voir le dépannage ci-dessous sinon)

### Lancement

### 4️⃣ Installation du projet

Depuis le dossier racine du projet :

```bash
git clone <url-du-depot>
cd projetgroupe1-main
docker compose up --build
```

Au premier lancement, l'application :
1. construit l'image .NET,
2. démarre PostgreSQL et attend qu'il soit prêt,
3. applique automatiquement les migrations Entity Framework,
4. charge les données et comptes de démonstration.

> ⏱️ Le premier lancement prend **3 à 5 minutes** (téléchargement des images et compilation) — c'est normal, ne pas interrompre. Les lancements suivants prennent quelques secondes.

L'application est ensuite disponible sur **http://localhost:8080**

### Comptes de démonstration

| Rôle  | Nom               | Email                    | Mot de passe    | Contenu                          |
|-------|-------------------|--------------------------|-----------------|----------------------------------|
| Kiné  | Etienne Skywalker | skywalker@kinestat.com   | K1n3$t@2901     | 16 patients de démonstration     |
| Kiné  | Luc LeMaire       | lemaire@kinestat.com     | K1n3$t@t2612    | 4 patients                       |
| Admin | Maxence Ramos     | ramos@kinestat.com       | @dm1n1str@t0r!  | Gestion des questions et comptes |

> ℹ️ Au premier démarrage, le service RGPD anonymise automatiquement les patients
> inactifs depuis plus de 20 ans — c'est le comportement attendu, pas un bug.

### Commandes utiles

```bash
docker compose up -d        # lancer en arrière-plan
docker compose logs -f web  # suivre les logs de l'application
docker compose down         # arrêter (les données sont conservées)
docker compose down -v      # arrêter ET supprimer la base de données
```

### En cas de problème

| Symptôme | Solution |
|----------|----------|
| `bind: address already in use` sur le port 8080 | Un autre service occupe le port. Changez le mappage dans `docker-compose.yml` : `"8081:8080"`, puis ouvrez http://localhost:8081 |
| Le build échoue immédiatement sous Windows | Vérifiez que Docker Desktop est en mode **Linux containers** (clic droit sur l'icône Docker → *Switch to Linux containers*) et que WSL 2 est actif (`wsl --status`) |
| `docker compose` : commande inconnue (Linux) | Installez le plugin Compose v2 (`docker-compose-v2` ou `docker-compose-plugin` selon la distribution) |
| `permission denied` sur le socket Docker (Linux) | Ajoutez votre utilisateur au groupe docker : `sudo usermod -aG docker $USER`, puis reconnectez-vous |
| Repartir d'une base totalement vierge | `docker compose down -v` puis `docker compose up --build` |

---

## 👥 Équipe initiale

* Joanna Imjalli
* Melvyn Paul
* Noah Lassence
* Ryan Wilmart
* Sacha Meunier
* Jean Elly Fanoux

## 👥 Équipe docker

* Marco Bogatu
* Melvyn Paul

---

## 📄 Licence

MIT License — voir le fichier [LICENSE](LICENSE).
