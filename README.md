# KineStat

Application pour kinésithérapeute utile dans la réalisation de bilan et dans la détection de Red Flags.

## 📁 Structure du projet
```
MonAppWeb/
├── Controllers/       
├── Data/              # Base de donnée
├── Models/            # Classes
├── Views/             # Interface WEB (cshtml)
├── wwwroot/           # Fichiers statiques (CSS, JS, images)
├── appsettings.json   
├── Program.cs         
└── Dockerfile         
```

## 🔧 Configuration

- **Technologie** : ASP .NET core (model-view-controller)
- **Base de donnée** : SQL server


## 📝 Conventions de code

- **Langue** : Code et commentaires en anglais
- **Nommage** : `camelCase`
- **Doc** : Documentation xml
- **BDD** : `camelCase`
- **Style** : [Tailwind](https://tailwindcss.com/)
- **Javascript** : [AlpineJS](https://alpinejs.dev/)

## 🔀 Workflow Git

Nous utilisons la gestion par GIT maintainer. 
Chaque dévelopeur doit effectuer une merge request pour ajouter son travail et le GIT maintainer la valide ou non.

### Branches

- `main` : Branche principale

### Messages de commit

Suivre la convention [Karma](https://karma-runner.github.io/6.3/dev/git-commit-msg.html) :

```
<type>(<scope>): <subject>

<body>

<footer>
```

**Exemples** :
```
fix(middleware): ensure Range headers adhere more closely to RFC 2616

Add one new dependency, use `range-parser` (Express dependency) to compute
range. It is more well-tested in the wild.

Fixes #2310
```

## 📚 Documentation

- Voir dossier `Doc` dans le projet.

## 👥 Équipe

- **Développeurs** : Joanna Imjalli, Melvyn Paul, Noah Lassence, Ryan Wilmart, Sacha Meunier, Jean Elly Fanoux

## 📄 Licence

MIT License - Voir [License](LICENSE)
