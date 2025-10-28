-- Script d'initialisation de la base de données KineStat

CREATE TABLE IF NOT EXISTS RedFlags (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX) DEFAULT ''
);

CREATE TABLE IF NOT EXISTS QuestionBool (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(500) NOT NULL,
    Type NVARCHAR(50) NOT NULL DEFAULT 'Bool',
    RVPositif DOUBLE NOT NULL DEFAULT 0,
    RVNegatif DOUBLE NOT NULL DEFAULT 0
);


CREATE TABLE IF NOT EXISTS QuestionLadder (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(500) NOT NULL,
    Type NVARCHAR(50) NOT NULL DEFAULT 'Lader',
    RVPositif DOUBLE NOT NULL DEFAULT 0,
    RVNegatif DOUBLE NOT NULL DEFAULT 0,
    min INT NOT NULL DEFAULT 0,
    max INT NOT NULL DEFAULT 10
);


CREATE TABLE IF NOT EXISTS Answer (
    Id INT PRIMARY KEY IDENTITY(1,1),
    QuestionId INT NOT NULL,
    Value NVARCHAR(MAX) NOT NULL,
    DateReponse DATETIME NOT NULL DEFAULT GETDATE(),
    Comment NVARCHAR(1000),
    Score INT NOT NULL DEFAULT 0
);


INSERT INTO RedFlags (Name, Description) VALUES
('Tumeur primaire rachidienne', 'Métastase vertébrale au niveau de la colonne vertébrale'),
('Infection rachidienne', 'Spondylodiscite, infection vertébrale au niveau rachidien'),
('Syndrome de la queue de cheval (SQC)', 'Compression médullaire ou myélopathie nécessitant une intervention urgente'),
('Fracture vertébrale pathologique', 'Fracture vertébrale causée par une pathologie sous-jacente'),
('Spondylarthrite inflammatoire', 'Pathologie inflammatoire chronique du rachis');


INSERT INTO QuestionBool (Title, Type, RVPositif, RVNegatif) VALUES
('Antécédent personnel de cancer (actualisation/sévérité)', 'Bool', 14, 0);

INSERT INTO QuestionBool (Title, Type, RVPositif, RVNegatif) VALUES
('Perte de poids inexpliquée (>5% en 3-6 mois)', 'Bool', 9, 0);

INSERT INTO QuestionBool (Title, Type, RVPositif, RVNegatif) VALUES
('Douleur nocturne sévère, réveillant le patient', 'Bool', 33, 0);

INSERT INTO QuestionLadder (Title, Type, RVPositif, RVNegatif, min, max) VALUES
('Âge du patient (en années)', 'Echelle', 0, 0, 0, 100);

INSERT INTO QuestionLadder (Title, Type, RVPositif, RVNegatif, min, max) VALUES
('Intensité de la douleur actuelle (échelle 0-10)', 'Echelle', 0, 0, 0, 10);
