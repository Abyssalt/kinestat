-- ========================================
-- SCRIPT DE SEED AVEC IDs MANUELS
-- Pour les données de référence uniquement
-- ========================================

-- ========================================
-- CATEGORIES (IDs 1-15)
-- ========================================
INSERT INTO "Categories" ("Id", "Name") VALUES 
(1, 'Tumeur / Métastase'),
(2, 'Infection'),
(3, 'Neurologique'),
(4, 'Traumatisme / Fracture'),
(5, 'Inflammatoire'),
(6, 'Vasculaire'),
(7, 'Articulaire / Structurel'),
(8, 'Myofascial'),
(9, 'Douleur Nociceptive'),
(10, 'Douleur Neuropathique'),
(11, 'Douleur Nociplastique'),
(12, 'Contrôle Sensorimoteur'),
(13, 'Croyances et Cognition'),
(14, 'Socio-environnemental'),
(15, 'Emotionnel / Affectif');

-- ========================================
-- PATHOLOGIES (IDs 1-83)
-- ========================================
INSERT INTO "Pathologies" ("Id", "Name") VALUES
(1, 'Métastase vertébrale'),
(2, 'Cancer métastatique'),
(3, 'Spondylodiscite ou métastase'),
(4, 'Tumeur vertébrale'),
(5, 'Spondylodiscite'),
(6, 'Spondylodiscite tuberculeuse (Mal de Pott)'),
(7, 'Anévrisme de l''aorte abdominale'),
(8, 'Syndrome de la queue de cheval'),
(9, 'Radiculopathie compressive aiguë'),
(10, 'Radiculopathie compressive progressive'),
(11, 'Compression médullaire'),
(12, 'Myélopathie'),
(13, 'Myélopathie cervicale'),
(14, 'Myélopathie (atteinte pyramidale)'),
(15, 'Douleur lombaire non spécifique'),
(16, 'Fracture vertébrale'),
(17, 'Fracture vertébrale aiguë'),
(18, 'Fracture ostéoporotique'),
(19, 'Traumatisme vertébral grave'),
(20, 'Lombalgie mécanique'),
(21, 'Lombalgie mécanique posturale'),
(22, 'Lombalgie discogénique'),
(23, 'Lombalgie inflammatoire'),
(24, 'Spondylarthrite'),
(25, 'Artériopathie oblitérante des membres inférieurs (AOMI)'),
(26, 'AOMI'),
(27, 'AAA'),
(28, 'Malignité ou infection possible'),
(29, 'Douleur sacro-iliaque'),
(30, 'Douleur référée myofasciale ou SIJ'),
(31, 'Douleur inflammatoire'),
(32, 'Instabilité lombaire'),
(33, 'Instabilité segmentaire'),
(34, 'Instabilité segmentaire (Hicks)'),
(35, 'Hypermobilité lombaire'),
(36, 'Hypermobilité'),
(37, 'Claudication neurogénique'),
(38, 'Claudication neurogénique (sténose)'),
(39, 'Claudication neurogénique (Cook+)'),
(40, 'Sténose lombaire'),
(41, 'Douleur facettaire'),
(42, 'Myofascial'),
(43, 'Myofascial (trigger points)'),
(44, 'Déconditionnement musculaire'),
(45, 'Déconditionnement'),
(46, 'Dysfonction du contrôle moteur'),
(47, 'Trouble de l''équilibre / contrôle moteur'),
(48, 'Trouble de l''équilibre central'),
(49, 'Faiblesse musculaire'),
(50, 'Faible capacité fonctionnelle'),
(51, 'Radiculopathie'),
(52, 'Radiculopathie haute'),
(53, 'Radiculopathie L3–L4'),
(54, 'Radiculopathie L4'),
(55, 'Radiculopathie L5'),
(56, 'Radiculopathie S1'),
(57, 'Faiblesse psoas / L2–L3'),
(58, 'Neuropathique'),
(59, 'Centralisation discogénique'),
(60, 'Appréhension mouvementale'),
(61, 'Douleur mécanique locale'),
(62, 'Nociplastique'),
(63, 'Kinésiophobie'),
(64, 'Catastrophisme'),
(65, 'Coping inefficace'),
(66, 'Croyances inadaptées'),
(67, 'Croyances dysfonctionnelles'),
(68, 'Risque psychosocial élevé'),
(69, 'Faible auto-efficacité'),
(70, 'Anxiété / dépression'),
(71, 'Trouble du sommeil'),
(72, 'Composante émotionnelle'),
(73, 'Composante émotionnelle / irritabilité'),
(74, 'Bonne santé générale'),
(75, 'Sédentarité'),
(76, 'Contrainte physique'),
(77, 'Contrainte physique élevée'),
(78, 'Soutien social faible'),
(79, 'Insatisfaction au travail'),
(80, 'Comportements à risque'),
(81, 'Altération qualité de vie'),
(82, 'Travail'),
(83, 'Tabagisme');

-- ========================================
-- CLUSTERS (IDs 1-15)
-- ========================================
INSERT INTO "Cluster" ("Id", "Name", "Description") VALUES 
(1, 'Screening Red Flags - Tumeur/Métastase', 'Dépistage des drapeaux rouges liés aux tumeurs et métastases'),
(2, 'Screening Red Flags - Infection', 'Dépistage des drapeaux rouges liés aux infections'),
(3, 'Tests Neurologiques Urgents', 'Tests pour détecter les urgences neurologiques'),
(4, 'Screening Red Flags - Traumatisme', 'Dépistage des drapeaux rouges liés aux traumatismes et fractures'),
(5, 'Screening Red Flags - Inflammatoire', 'Dépistage des drapeaux rouges liés aux pathologies inflammatoires'),
(6, 'Screening Red Flags - Vasculaire', 'Dépistage des drapeaux rouges liés aux pathologies vasculaires'),
(7, 'Évaluation Articulaire', 'Tests d''évaluation de la fonction articulaire'),
(8, 'Tests Myofasciaux', 'Évaluation des tensions et trigger points myofasciaux'),
(9, 'Évaluation Nociceptive', 'Tests pour identifier la douleur nociceptive'),
(10, 'Tests Neuropathiques', 'Évaluation de la douleur neuropathique'),
(11, 'Évaluation Nociplastique', 'Tests pour identifier la sensibilisation centrale'),
(12, 'Tests Sensorimoteurs', 'Évaluation du contrôle sensorimoteur'),
(13, 'Évaluation Cognitive', 'Questionnaires sur les croyances et la cognition'),
(14, 'Facteurs Psychosociaux', 'Évaluation des facteurs socio-environnementaux'),
(15, 'Évaluation Émotionnelle', 'Tests pour l''état émotionnel et affectif');

-- ========================================
-- MEDICAL CONTEXTS (IDs 1-3)
-- ========================================
INSERT INTO "MedicalContexts" ("Id", "Name") VALUES 
(1, 'Hôpital'),
(2, 'Accès non direct'),
(3, 'Accès direct');

-- ========================================
-- PRIOR CONTEXTS (IDs 1-18)
-- ========================================
INSERT INTO "PriorContexts" ("Id", "Value", "CategoryId", "MedicalContextId") VALUES
-- Catégorie 1 : Tumeur / Métastase
(1, 0.003, 1, 1),
(2, 0.003, 1, 2),
(3, 0.003, 1, 3),
-- Catégorie 2 : Infection
(4, 0.0005, 2, 1),
(5, 0.0001, 2, 2),
(6, 0.0001, 2, 3),
-- Catégorie 3 : Neurologique
(7, 0.001, 3, 1),
(8, 0.0004, 3, 2),
(9, 0.0004, 3, 3),
-- Catégorie 4 : Traumatisme / Fracture
(10, 0.04, 4, 1),
(11, 0.04, 4, 2),
(12, 0.04, 4, 3),
-- Catégorie 5 : Inflammatoire
(13, 0.002, 5, 1),
(14, 0.002, 5, 2),
(15, 0.002, 5, 3),
-- Catégorie 6 : Vasculaire
(16, 0.0001, 6, 1),
(17, 0.0001, 6, 2),
(18, 0.0001, 6, 3),
-- Catégorie 7 : Articulaire / Structurel
(19, 0.03, 7, 1),
(20, 0.03, 7, 2),
(21, 0.03, 7, 3),
-- Catégorie 8 : Myofascial
(22, 0.02, 8, 1),
(23, 0.02, 8, 2),
(24, 0.02, 8, 3),
-- Catégorie 9 : Douleur Nociceptive
(25, 0.03, 9, 1),
(26, 0.03, 9, 2),
(27, 0.03, 9, 3),
-- Catégorie 10 : Douleur Neuropathique
(28, 0.01, 10, 1),
(29, 0.01, 10, 2),
(30, 0.01, 10, 3),
-- Catégorie 11 : Douleur Nociplastique
(31, 0.005, 11, 1),
(32, 0.005, 11, 2),
(33, 0.005, 11, 3),
-- Catégorie 12 : Contrôle Sensorimoteur
(34, 0.02, 12, 1),
(35, 0.02, 12, 2),
(36, 0.02, 12, 3),
-- Catégorie 13 : Croyances et Cognition
(37, 0.03, 13, 1),
(38, 0.03, 13, 2),
(39, 0.03, 13, 3),
-- Catégorie 14 : Socio-environnemental
(40, 0.02, 14, 1),
(41, 0.02, 14, 2),
(42, 0.02, 14, 3),
-- Catégorie 15 : Émotionnel / Affectif
(43, 0.03, 15, 1),
(44, 0.03, 15, 2),
(45, 0.03, 15, 3);


-- ========================================
-- PRIOR CONTEXT PATHOLOGIES (IDs 1-249)
-- ========================================
INSERT INTO "PriorContextPathologies" ("Id", "Value", "PathologyId", "MedicalContextId") VALUES
-- TUMEUR / MÉTASTASE (Pathologies 1-4)
(1, 0.05, 1, 1), (2, 0.03, 1, 2), (3, 0.005, 1, 3),
(4, 0.05, 2, 1), (5, 0.03, 2, 2), (6, 0.005, 2, 3),
(7, 0.05, 3, 1), (8, 0.03, 3, 2), (9, 0.005, 3, 3),
(10, 0.05, 4, 1), (11, 0.03, 4, 2), (12, 0.005, 4, 3),
-- INFECTION (Pathologies 5-6)
(13, 0.05, 5, 1), (14, 0.005, 5, 2), (15, 0.005, 5, 3),
(16, 0.05, 6, 1), (17, 0.005, 6, 2), (18, 0.005, 6, 3),
-- VASCULAIRE (Pathologies 7, 25-27)
(19, 0.02, 7, 1), (20, 0.01, 7, 2), (21, 0.005, 7, 3),
(22, 0.02, 25, 1), (23, 0.01, 25, 2), (24, 0.005, 25, 3),
(25, 0.02, 26, 1), (26, 0.01, 26, 2), (27, 0.005, 26, 3),
(28, 0.02, 27, 1), (29, 0.01, 27, 2), (30, 0.005, 27, 3),
-- NEUROLOGIQUE URGENT (Pathologies 8-14)
(31, 0.20, 8, 1), (32, 0.10, 8, 2), (33, 0.05, 8, 3),
(34, 0.20, 9, 1), (35, 0.10, 9, 2), (36, 0.05, 9, 3),
(37, 0.20, 10, 1), (38, 0.10, 10, 2), (39, 0.05, 10, 3),
(40, 0.20, 11, 1), (41, 0.10, 11, 2), (42, 0.05, 11, 3),
(43, 0.20, 12, 1), (44, 0.10, 12, 2), (45, 0.05, 12, 3),
(46, 0.20, 13, 1), (47, 0.10, 13, 2), (48, 0.05, 13, 3),
(49, 0.20, 14, 1), (50, 0.10, 14, 2), (51, 0.05, 14, 3),
-- DOULEUR LOMBAIRE NON SPÉCIFIQUE (Pathologie 15)
(52, 0.20, 15, 1), (53, 0.10, 15, 2), (54, 0.05, 15, 3),
-- FRACTURE / TRAUMATISME (Pathologies 16-19)
(55, 0.15, 16, 1), (56, 0.02, 16, 2), (57, 0.02, 16, 3),
(58, 0.15, 17, 1), (59, 0.02, 17, 2), (60, 0.02, 17, 3),
(61, 0.15, 18, 1), (62, 0.02, 18, 2), (63, 0.02, 18, 3),
(64, 0.15, 19, 1), (65, 0.02, 19, 2), (66, 0.02, 19, 3),
-- LOMBALGIE MÉCANIQUE (Pathologies 20-22)
(67, 0.20, 20, 1), (68, 0.25, 20, 2), (69, 0.30, 20, 3),
(70, 0.20, 21, 1), (71, 0.25, 21, 2), (72, 0.30, 21, 3),
(73, 0.20, 22, 1), (74, 0.25, 22, 2), (75, 0.30, 22, 3),
-- INFLAMMATOIRE (Pathologies 23-24)
(76, 0.10, 23, 1), (77, 0.05, 23, 2), (78, 0.07, 23, 3),
(79, 0.10, 24, 1), (80, 0.05, 24, 2), (81, 0.07, 24, 3),
-- MALIGNITÉ/INFECTION POSSIBLE (Pathologie 28)
(82, 0.02, 28, 1), (83, 0.01, 28, 2), (84, 0.005, 28, 3),
-- DOULEUR SACRO-ILIAQUE / RÉFÉRÉE (Pathologies 29-31)
(85, 0.20, 29, 1), (86, 0.25, 29, 2), (87, 0.30, 29, 3),
(88, 0.20, 30, 1), (89, 0.25, 30, 2), (90, 0.30, 30, 3),
(91, 0.20, 31, 1), (92, 0.25, 31, 2), (93, 0.30, 31, 3),
-- INSTABILITÉ / HYPERMOBILITÉ (Pathologies 32-36)
(94, 0.20, 32, 1), (95, 0.25, 32, 2), (96, 0.30, 32, 3),
(97, 0.20, 33, 1), (98, 0.25, 33, 2), (99, 0.30, 33, 3),
(100, 0.65, 34, 1), (101, 0.50, 34, 2), (102, 0.40, 34, 3),
(103, 0.20, 35, 1), (104, 0.25, 35, 2), (105, 0.30, 35, 3),
(106, 0.65, 36, 1), (107, 0.50, 36, 2), (108, 0.40, 36, 3),
-- CLAUDICATION NEUROGÉNIQUE / STÉNOSE (Pathologies 37-40)
(109, 0.50, 37, 1), (110, 0.30, 37, 2), (111, 0.20, 37, 3),
(112, 0.20, 38, 1), (113, 0.25, 38, 2), (114, 0.30, 38, 3),
(115, 0.50, 39, 1), (116, 0.30, 39, 2), (117, 0.20, 39, 3),
(118, 0.20, 40, 1), (119, 0.25, 40, 2), (120, 0.30, 40, 3),
-- DOULEUR FACETTAIRE / MYOFASCIAL (Pathologies 41-43)
(121, 0.20, 41, 1), (122, 0.25, 41, 2), (123, 0.30, 41, 3),
(124, 0.20, 42, 1), (125, 0.25, 42, 2), (126, 0.30, 42, 3),
(127, 0.20, 43, 1), (128, 0.25, 43, 2), (129, 0.30, 43, 3),
-- DÉCONDITIONNEMENT / CONTRÔLE MOTEUR (Pathologies 44-48)
(130, 0.60, 44, 1), (131, 0.40, 44, 2), (132, 0.35, 44, 3),
(133, 0.60, 45, 1), (134, 0.40, 45, 2), (135, 0.35, 45, 3),
(136, 0.60, 46, 1), (137, 0.40, 46, 2), (138, 0.35, 46, 3),
(139, 0.20, 47, 1), (140, 0.25, 47, 2), (141, 0.30, 47, 3),
(142, 0.50, 48, 1), (143, 0.30, 48, 2), (144, 0.20, 48, 3),
-- FAIBLESSE / CAPACITÉ FONCTIONNELLE (Pathologies 49-50)
(145, 0.60, 49, 1), (146, 0.40, 49, 2), (147, 0.35, 49, 3),
(148, 0.60, 50, 1), (149, 0.40, 50, 2), (150, 0.35, 50, 3),
-- RADICULOPATHIE (Pathologies 51-57)
(151, 0.65, 51, 1), (152, 0.50, 51, 2), (153, 0.40, 51, 3),
(154, 0.50, 52, 1), (155, 0.30, 52, 2), (156, 0.20, 52, 3),
(157, 0.50, 53, 1), (158, 0.30, 53, 2), (159, 0.20, 53, 3),
(160, 0.50, 54, 1), (161, 0.30, 54, 2), (162, 0.20, 54, 3),
(163, 0.50, 55, 1), (164, 0.30, 55, 2), (165, 0.20, 55, 3),
(166, 0.50, 56, 1), (167, 0.30, 56, 2), (168, 0.20, 56, 3),
(169, 0.50, 57, 1), (170, 0.30, 57, 2), (171, 0.20, 57, 3),
-- NEUROPATHIQUE (Pathologie 58)
(172, 0.50, 58, 1), (173, 0.65, 58, 2), (174, 0.80, 58, 3),
-- CENTRALISATION / APPRÉHENSION (Pathologies 59-61)
(175, 0.65, 59, 1), (176, 0.50, 59, 2), (177, 0.40, 59, 3),
(178, 0.65, 60, 1), (179, 0.50, 60, 2), (180, 0.40, 60, 3),
(181, 0.65, 61, 1), (182, 0.50, 61, 2), (183, 0.40, 61, 3),
-- NOCIPLASTIQUE (Pathologie 62)
(184, 0.50, 62, 1), (185, 0.30, 62, 2), (186, 0.20, 62, 3),
-- CROYANCES ET COGNITION (Pathologies 63-69)
(187, 0.60, 63, 1), (188, 0.45, 63, 2), (189, 0.35, 63, 3),
(190, 0.60, 64, 1), (191, 0.45, 64, 2), (192, 0.35, 64, 3),
(193, 0.60, 65, 1), (194, 0.45, 65, 2), (195, 0.35, 65, 3),
(196, 0.50, 66, 1), (197, 0.30, 66, 2), (198, 0.20, 66, 3),
(199, 0.60, 67, 1), (200, 0.45, 67, 2), (201, 0.35, 67, 3),
(202, 0.60, 68, 1), (203, 0.45, 68, 2), (204, 0.35, 68, 3),
(205, 0.60, 69, 1), (206, 0.45, 69, 2), (207, 0.35, 69, 3),
-- ÉMOTIONNEL / AFFECTIF (Pathologies 70-73)
(208, 0.50, 70, 1), (209, 0.40, 70, 2), (210, 0.30, 70, 3),
(211, 0.50, 71, 1), (212, 0.40, 71, 2), (213, 0.30, 71, 3),
(214, 0.50, 72, 1), (215, 0.30, 72, 2), (216, 0.20, 72, 3),
(217, 0.50, 73, 1), (218, 0.40, 73, 2), (219, 0.30, 73, 3),
-- SANTÉ GÉNÉRALE / STYLE DE VIE (Pathologies 74-76)
(220, 0.50, 74, 1), (221, 0.40, 74, 2), (222, 0.30, 74, 3),
(223, 0.45, 75, 1), (224, 0.35, 75, 2), (225, 0.25, 75, 3),
(226, 0.45, 76, 1), (227, 0.25, 76, 2), (228, 0.15, 76, 3),
-- SOCIO-ENVIRONNEMENTAL (Pathologies 77-83)
(229, 0.45, 77, 1), (230, 0.35, 77, 2), (231, 0.25, 77, 3),
(232, 0.45, 78, 1), (233, 0.35, 78, 2), (234, 0.25, 78, 3),
(235, 0.45, 79, 1), (236, 0.35, 79, 2), (237, 0.25, 79, 3),
(238, 0.45, 80, 1), (239, 0.35, 80, 2), (240, 0.25, 80, 3),
(241, 0.45, 81, 1), (242, 0.35, 81, 2), (243, 0.25, 81, 3),
(244, 0.45, 82, 1), (245, 0.35, 82, 2), (246, 0.25, 82, 3),
(247, 0.45, 83, 1), (248, 0.35, 83, 2), (249, 0.25, 83, 3);

-- ========================================
-- QUESTIONS (IDs 1-190)
-- ========================================

-- QUESTIONS PERMANENTES (IDs 1-9)
INSERT INTO "Questions" ("Id", "Title", "SourceRv", "RVPositive", "RVNegative", "CategoryId", "ClusterId", "Discriminator", "HasPermanentAnswer", "Min", "Max") VALUES
(1, 'Antécédent personnel de cancer (localisation/année).', 'HAS (2000)', 14.7, 0, 1, NULL, 'QuestionBool', true, NULL, NULL),
(2, 'Antécédent familial de cancer significatif.', 'Données cliniques', 0, 0, 1, NULL, 'QuestionBool', true, NULL, NULL),
(3, 'Antécédent récent d''infection ou chirurgie.', 'Synthèse revues lombalgie', 4, 0.6, 2, NULL, 'QuestionBool', true, NULL, NULL),
(4, 'Traitement chronique corticoïdes / ostéoporose / >70 ans.', 'Osteoporosis review', 2, 0.7, 4, NULL, 'QuestionBool', true, NULL, NULL),
(5, 'Antécédent psoriasis / MICI / familial.', 'RESPONDIA / REGISPONSER', 3.2, 0.6, 5, NULL, 'QuestionBool', true, NULL, NULL),
(6, 'Antécédent familial anévrisme / AOMI.', 'Funican 2020', 9, 0, 6, NULL, 'QuestionBool', true, NULL, NULL),
(7, 'Anticoagulants / antécédent anévrisme / chirurgie vasculaire.', 'BJA Education', 4, 0.7, 6, NULL, 'QuestionBool', true, NULL, NULL),
(8, 'Age > 50 ans', 'Livre exam et bilan', 3, 0.53, 7, NULL, 'QuestionBool', true, NULL, NULL),
(9, 'Age > 65 ans', 'Livre exam clinique et bilan', 2.5, 0.53, 10, NULL, 'QuestionBool', true, NULL, NULL);

-- QUESTIONS TUMEUR / MÉTASTASE (IDs 10-15)
INSERT INTO "Questions" ("Id", "Title", "SourceRv", "RVPositive", "RVNegative", "CategoryId", "ClusterId", "Discriminator", "HasPermanentAnswer", "Min", "Max") VALUES
(10, 'Perte de poids inexpliquée (>5% en 3–6 mois).', 'PubMed 32495276', 9.2, 0, 1, NULL, 'QuestionBool', false, NULL, NULL),
(11, 'Douleur nocturne sévère, réveillant le patient.', 'PubMed 32377894', 33.25, 0, 1, NULL, 'QuestionBool', false, NULL, NULL),
(12, 'Douleur progressive sans amélioration malgré traitement (>4 semaines).', 'PubMed 32377894', 3.1, 0.8, 1, NULL, 'QuestionBool', false, NULL, NULL),
(13, 'Fatigue générale, malaise, perte d''appétit.', 'Funicane 2020', 3, 0, 1, NULL, 'QuestionBool', false, NULL, NULL),
(14, 'Douleur à la percussion vertébrale.', 'Funicane 2020', 13, 0, 1, NULL, 'QuestionBool', false, NULL, NULL),
(15, 'Déformation vertébrale ou masse palpable.', 'Funicane 2020', 10, 0, 1, NULL, 'QuestionBool', false, NULL, NULL);

-- QUESTIONS INFECTION (IDs 16-23)
INSERT INTO "Questions" ("Id", "Title", "SourceRv", "RVPositive", "RVNegative", "CategoryId", "ClusterId", "Discriminator", "HasPermanentAnswer", "Min", "Max") VALUES
(16, 'Fièvre ≥ 38°C ou frissons récents.', 'PubMed 32495276', 68.8, 0, 2, NULL, 'QuestionBool', false, NULL, NULL),
(17, 'Signes systémiques d''infection (sueurs nocturnes, fatigue).', 'PubMed 24335669', 1.8, 1, 2, NULL, 'QuestionBool', false, NULL, NULL),
(18, 'Antécédent récent d''infection ou chirurgie.', 'Synthèse revues lombalgie', 4, 0.6, 2, NULL, 'QuestionBool', false, NULL, NULL),
(19, 'Immunodépression documentée (corticothérapie chronique).', 'PubMed 24335669', 48.5, 0.8, 2, NULL, 'QuestionBool', false, NULL, NULL),
(20, 'Douleur thoracique.', 'PubMed 32377894', 1, 1, 2, NULL, 'QuestionBool', false, NULL, NULL),
(21, 'Sueurs nocturnes, fatigue, perte d''énergie.', 'Funican 2020', 12, 0, 2, NULL, 'QuestionBool', false, NULL, NULL),
(22, 'Douleur persistante malgré traitement antibiotique préalable.', 'Funican 2020', 5.2, 0, 2, NULL, 'QuestionBool', false, NULL, NULL),
(23, 'Voyage ou provenance zone endémique tuberculose.', 'Funican 2020', 9, 0, 2, NULL, 'QuestionBool', false, NULL, NULL);

-- QUESTIONS NEUROLOGIQUE (IDs 24-30)
INSERT INTO "Questions" ("Id", "Title", "SourceRv", "RVPositive", "RVNegative", "CategoryId", "ClusterId", "Discriminator", "HasPermanentAnswer", "Min", "Max") VALUES
(24, 'Anesthésie en selle / hypoesthésie périnéale.', 'Dionne et al. 2019 CES', 1.7, 0.7, 3, NULL, 'QuestionBool', false, NULL, NULL),
(25, 'Rétention urinaire ou incontinence fécale / urinaire.', 'Revue CES soins primaires', 2, 0.6, 3, NULL, 'QuestionBool', false, NULL, NULL),
(26, 'Faiblesse motrice aiguë ou progressive (MRC ≤ 3).', 'Cohorte urgence CES', 9.4, 0.1, 3, NULL, 'QuestionBool', false, NULL, NULL),
(27, 'Troubles de la marche rapides / signes pyramidaux.', 'International Framework Red Flags', 3, 0.4, 3, NULL, 'QuestionBool', false, NULL, NULL),
(28, 'Douleur radiculaire avec déficit moteur progressif.', 'StatPearls', 4, 0.3, 3, NULL, 'QuestionBool', false, NULL, NULL),
(29, 'Douleur radiculaire évoluant vers une faiblesse progressive.', 'Funican 2020', 9, 0.6, 3, NULL, 'QuestionBool', false, NULL, NULL),
(30, 'Altération récente des réflexes ostéo-tendineux bilatéraux.', 'Funican 2020', 8, 0, 3, NULL, 'QuestionBool', false, NULL, NULL);

-- QUESTIONS TRAUMATISME / FRACTURE (IDs 31-36)
INSERT INTO "Questions" ("Id", "Title", "SourceRv", "RVPositive", "RVNegative", "CategoryId", "ClusterId", "Discriminator", "HasPermanentAnswer", "Min", "Max") VALUES
(31, 'Douleur lombaire après traumatisme majeur ou mineur.', 'Revue fractures vertébrales', 12.8, 0.37, 4, NULL, 'QuestionBool', false, NULL, NULL),
(32, 'Traitement chronique corticoïdes / ostéoporose / >70 ans.', 'Osteoporosis review', 2, 0.7, 4, NULL, 'QuestionBool', false, NULL, NULL),
(33, 'Contusion ou abrasion dorsale.', 'PubMed fractures', 31, 0.2, 4, NULL, 'QuestionBool', false, NULL, NULL),
(34, 'Douleur aiguë localisée après effort / chute.', 'Revue fractures vertébrales', 6.7, 0.44, 4, NULL, 'QuestionBool', false, NULL, NULL),
(35, 'Douleur persistante après traitement conservateur.', 'Funican 2020', 4.5, 0, 4, NULL, 'QuestionBool', false, NULL, NULL),
(36, 'Choc violent (AVP, chute de hauteur).', 'Funican 2020', 12, 0, 4, NULL, 'QuestionBool', false, NULL, NULL);

-- QUESTIONS INFLAMMATOIRE (IDs 37-41)
INSERT INTO "Questions" ("Id", "Title", "SourceRv", "RVPositive", "RVNegative", "CategoryId", "ClusterId", "Discriminator", "HasPermanentAnswer", "Min", "Max") VALUES
(37, 'Raideur matinale >30 min (<45–50 ans).', 'ASAS-handbook', 2, 0.5, 5, NULL, 'QuestionBool', false, NULL, NULL),
(38, 'Douleur améliorée à l''exercice mais pas au repos.', 'ASAS-handbook', 2.5, 0.5, 5, NULL, 'QuestionBool', false, NULL, NULL),
(39, 'Douleur non soulagée par repos prolongé.', 'Funican 2020', 5.8, 0, 5, NULL, 'QuestionBool', false, NULL, NULL),
(40, 'Réveil nocturne 2e partie nuit par douleur.', 'Funican 2020', 6, 0, 5, NULL, 'QuestionBool', false, NULL, NULL),
(41, 'Atteinte autres articulations (épaule/hanche/genou).', 'Funican 2020', 5.5, 0, 5, NULL, 'QuestionBool', false, NULL, NULL);

-- QUESTIONS VASCULAIRE (IDs 42-48)
INSERT INTO "Questions" ("Id", "Title", "SourceRv", "RVPositive", "RVNegative", "CategoryId", "ClusterId", "Discriminator", "HasPermanentAnswer", "Min", "Max") VALUES
(42, 'Douleur à la marche qui cède au repos (claudication).', 'PMC claudication study', 3, 0.3, 6, NULL, 'QuestionBool', false, NULL, NULL),
(43, 'Pâleur, froid, pouls périphériques diminués.', 'PMC PAD detection', 6, 0.15, 6, NULL, 'QuestionBool', false, NULL, NULL),
(44, 'Douleur abdominale + hypotension (possible AAA).', 'PMC AAA review', 15, 0.2, 6, NULL, 'QuestionBool', false, NULL, NULL),
(45, 'Douleur pulsatile ou battante lombaire.', 'Funican 2020', 20, 0, 6, NULL, 'QuestionBool', false, NULL, NULL),
(46, 'Pouls périphériques diminués ou asymétriques.', 'Funican 2020', 10, 0, 6, NULL, 'QuestionBool', false, NULL, NULL),
(47, 'Tabagisme, hypercholestérolémie, âge >65 ans.', 'Funican 2020', 10, 0, 6, NULL, 'QuestionBool', false, NULL, NULL),
(48, 'Âge extrême (<18 ou >55–60 ans).', 'PMC malignancy red flags', 2, 0.7, 2, NULL, 'QuestionBool', false, NULL, NULL);

-- QUESTIONS NOCIPLASTIQUE (IDs 49-53)
INSERT INTO "Questions" ("Id", "Title", "SourceRv", "RVPositive", "RVNegative", "CategoryId", "ClusterId", "Discriminator", "HasPermanentAnswer", "Min", "Max") VALUES
(49, 'Douleur diffuse, mal localisee ?', 'Livre', 1, 1, 11, NULL, 'QuestionBool', false, NULL, NULL),
(50, 'Hypersensibilite sensorielle (bruit, lumiere, toucher) ?', 'Livre', 1, 1, 11, NULL, 'QuestionBool', false, NULL, NULL),
(51, 'Sommeil non reparateur ?', 'Livre', 1, 1, 11, NULL, 'QuestionBool', false, NULL, NULL),
(52, 'Douleur disproportionnee a la charge physique ?', 'Livre', 1, 1, 11, NULL, 'QuestionBool', false, NULL, NULL),
(53, 'Plusieurs sites douloureux simultanes ?', 'Livre', 1, 1, 11, NULL, 'QuestionBool', false, NULL, NULL);

-- QUESTIONS CROYANCES / COGNITION (IDs 54-59)
INSERT INTO "Questions" ("Id", "Title", "SourceRv", "RVPositive", "RVNegative", "CategoryId", "ClusterId", "Discriminator", "HasPermanentAnswer", "Min", "Max") VALUES
(54, 'Peur du mouvement (Tampa) ?', 'Journal of PHYSIOTHERAPY', 2, 0.5, 13, NULL, 'QuestionBool', false, 1, 4),
(55, 'Catastrophisme (PCS) ?', 'Expertise Clinique', 2.5, 0.7, 13, NULL, 'QuestionLadder', false, 0, 52),
(56, 'Strategies de coping inefficaces ?', 'Expertise Clinique', 2, 0.7, 13, NULL, 'QuestionBool', false, NULL, NULL),
(57, 'Croyances inadaptees sur la douleur ?', 'Expertise Clinique', 2, 0.7, 13, NULL, 'QuestionBool', false, NULL, NULL),
(58, 'Start Back / Orebro score eleve ?', 'Expertise Clinique', 2, 0.7, 13, NULL, 'QuestionLadder', false, 0, 9),
(59, 'Auto-efficacite faible (PSEQ bas) ?', 'Expertise Clinique', 2, 0.7, 13, NULL, 'QuestionLadder', false, 0, 10);

-- QUESTIONS ÉMOTIONNEL / AFFECTIF (IDs 60-64)
INSERT INTO "Questions" ("Id", "Title", "SourceRv", "RVPositive", "RVNegative", "CategoryId", "ClusterId", "Discriminator", "HasPermanentAnswer", "Min", "Max") VALUES
(60, 'Anxiete ou depression (HADS / BDI) ?', 'wikipedia HADS sur senejet.spe', 4.09, 0.13, 15, NULL, 'QuestionLadder', false, 0, 21),
(61, 'Sommeil perturbe ou insomnie liee a la douleur ?', 'wikipedia HADS sur senejet.spe', 2, 0.75, 15, NULL, 'QuestionBool', false, NULL, NULL),
(62, 'Score PSQI altere ?', 'https://emo.ncbi.nlm.nih.gov/articles/PMC4513260/', 6.84, 0.12, 15, NULL, 'QuestionLadder', false, 0, 21),
(63, 'Tension emotionnelle / irritabilite ?', 'wikipedia HADS sur senejet.spe', 1.9, 0.45, 15, NULL, 'QuestionBool', false, NULL, NULL),
(64, 'Bonne sante generale / actif ?', 'DOI: 10.1001/jama.2010.344', 1.8, 0.85, 15, NULL, 'QuestionBool', false, NULL, NULL);

-- QUESTIONS SOCIO-ENVIRONNEMENTAL (IDs 65-72)
INSERT INTO "Questions" ("Id", "Title", "SourceRv", "RVPositive", "RVNegative", "CategoryId", "ClusterId", "Discriminator", "HasPermanentAnswer", "Min", "Max") VALUES
(65, 'Niveau d''activite physique faible ?', 'Expertise Clinique', 2.1, 0.77, 14, NULL, 'QuestionBool', false, NULL, NULL),
(66, 'Contrainte pro physique elevee ?', 'DOI: 10.1001/jama.2010.344', 1.2, 0.87, 14, NULL, 'QuestionLadder', false, 0, 10),
(67, 'Soutien social faible ?', 'Expertise Clinique', 2.2, 0.89, 14, NULL, 'QuestionBool', false, NULL, NULL),
(68, 'Satisfaction au travail diminuee ?', 'DOI: 10.1001/jama.2010.344', 1.55, 0.88, 14, NULL, 'QuestionBool', false, NULL, NULL),
(69, 'Consommation d''alcool ou comportements a risque ?', 'Expertise Clinique', 2.2, 0.74, 14, NULL, 'QuestionBool', false, NULL, NULL),
(70, 'SF-36 / EQ-5D altere ?', 'Expertise Clinique', 1.9, 0.87, 14, NULL, 'QuestionLadder', false, 0, 10),
(71, 'Travail penible', 'https://hal.univ-lorraine.fr/hal-03297880/document', 1.4, 0.84, 14, NULL, 'QuestionBool', false, NULL, NULL),
(72, 'Tabagisme', 'https://hal.univ-lorraine.fr/hal-03297880/document', 1, 1, 14, NULL, 'QuestionBool', false, NULL, NULL);

-- QUESTIONS DOULEUR NOCICEPTIVE (IDs 73-78)
INSERT INTO "Questions" ("Id", "Title", "SourceRv", "RVPositive", "RVNegative", "CategoryId", "ClusterId", "Discriminator", "HasPermanentAnswer", "Min", "Max") VALUES
(73, 'Douleur localisee et coherente anatomiquement ?', 'Expertise Clinique', 3.5, 0.3, 9, NULL, 'QuestionBool', false, NULL, NULL),
(74, 'Douleur aigue, sourde ou lancinante ?', 'Expertise Clinique', 3.5, 0.6, 9, NULL, 'QuestionBool', false, NULL, NULL),
(75, 'Douleur liee a la charge mecanique ?', 'Expertise Clinique', 4.5, 0.7, 9, NULL, 'QuestionBool', false, NULL, NULL),
(76, 'Soulagement au repos ?', 'Expertise Clinique', 4.5, 0.5, 9, NULL, 'QuestionBool', false, NULL, NULL),
(77, 'Douleur proportionnelle a la contrainte tissulaire ?', 'Expertise Clinique', 4.5, 0.55, 9, NULL, 'QuestionBool', false, NULL, NULL),
(78, 'Exacerbation', 'Livre exam et bilan', 6.3, 0.63, 9, NULL, 'QuestionBool', false, NULL, NULL);

-- QUESTIONS CONTRÔLE SENSORIMOTEUR (IDs 79-87)
INSERT INTO "Questions" ("Id", "Title", "SourceRv", "RVPositive", "RVNegative", "CategoryId", "ClusterId", "Discriminator", "HasPermanentAnswer", "Min", "Max") VALUES
(79, 'Tests proprioceptifs / coordination deficients (Luomajoki) ?', 'Expertise Clinique', 3.5, 0.55, 12, 12, 'QuestionBool', false, NULL, NULL),
(80, 'Equilibre postural altere ?', 'Expertise Clinique', 2.5, 0.55, 12, 12, 'QuestionBool', false, NULL, NULL),
(81, 'Force isometrique lombaire ou quadriceps faible ?', 'Expertise Clinique', 3, 0.55, 12, 12, 'QuestionLadder', false, 0, 10),
(82, 'Endurance au gainage faible ?', 'Expertise Clinique', 3, 0.45, 12, 12, 'QuestionLadder', false, 0, 60),
(83, 'Tests fonctionnels (6MWT, VO2) diminues ?', 'Expertise Clinique', 3.5, 0.45, 12, 12, 'QuestionBool', false, NULL, NULL),
(84, 'Luomajoki dysfonction du controle', 'Expertise Clinique', 3.5, 0.45, 12, 12, 'QuestionBool', false, NULL, NULL),
(85, 'Pression de 40mmgh a 42-44 mmgh', 'physiotutot', 0.34, 1.02, 12, 12, 'QuestionBool', false, NULL, NULL),
(86, 'Regle de prediction clinique de Hicks - 2 elements positifs ou +', 'physiotutot', 4, 0.52, 12, 12, 'QuestionBool', false, NULL, NULL),
(87, 'Regle de prediction clinique de Hicks - 3 elements positifs ou +', 'physiotutot', 6.3, 0.18, 12, 12, 'QuestionBool', false, NULL, NULL);

-- QUESTIONS NEUROPATHIQUE - SCREENING (IDs 88-109)
INSERT INTO "Questions" ("Id", "Title", "SourceRv", "RVPositive", "RVNegative", "CategoryId", "ClusterId", "Discriminator", "HasPermanentAnswer", "Min", "Max") VALUES
(88, 'Douleur suivant un trajet neuro-anatomique coherent ?', 'Expertise Clinique', 1, 1, 10, NULL, 'QuestionBool', false, NULL, NULL),
(89, 'Brulures, decharges electriques, picotements ?', 'Expertise Clinique', 1, 1, 10, NULL, 'QuestionBool', false, NULL, NULL),
(90, 'Engourdissement ou hypoesthesie distrale ?', 'Expertise Clinique', 1, 1, 10, NULL, 'QuestionBool', false, NULL, NULL),
(91, 'Deficit moteur / sensitif objectif ?', 'Expertise Clinique', 1, 1, 10, NULL, 'QuestionBool', false, NULL, NULL),
(92, 'Questionnaire DN4 (PainDetect positif ?', 'https://www.sfetd-douleur.org/wp-content/uploads/2019/08/dn4.pdf', 8.21, 0.19, 10, NULL, 'QuestionBool', false, NULL, NULL),
(93, 'Sensibiliteapse touche', 'Livre exam clinique et bilan', 1.32, 0.91, 10, NULL, 'QuestionBool', false, NULL, NULL),
(94, 'Sensibilite vibration', 'Livre exam clinique et bilan', 1.32, 0.81, 10, NULL, 'QuestionBool', false, NULL, NULL),
(95, 'Faiblesse Etastrocemien-soleaire', 'Livre exam clinique et bilan', 1.96, 0.7, 10, NULL, 'QuestionBool', false, NULL, NULL),
(96, 'Faiblesse Long extenseur des orteils', 'Livre exam clinique et bilan', 1.36, 0.77, 10, NULL, 'QuestionBool', false, NULL, NULL),
(97, 'Faiblesse Flechisseur de la hanche', 'Livre exam clinique et bilan', 4.38, 0.8, 10, NULL, 'QuestionBool', false, NULL, NULL),
(98, 'Faiblesse flechisseur du genou', 'Livre exam clinique et bilan', 1.49, 0.96, 10, NULL, 'QuestionBool', false, NULL, NULL),
(99, 'Lasegue', 'Livre exam clinique et bilan', 1, 1, 10, NULL, 'QuestionBool', false, NULL, NULL),
(100, 'Eridure', 'Livre exam clinique et bilan', 1, 1, 10, NULL, 'QuestionBool', false, NULL, NULL),
(101, 'Test hyperflexion du genou', 'Livre exam clinique et bilan', 6, 0, 10, NULL, 'QuestionBool', false, NULL, NULL),
(102, 'Douleur au-dessous des genoux', 'Livre exam clinique et bilan', 1.3, 0.7, 10, NULL, 'QuestionBool', false, NULL, NULL),
(103, 'Douleur au-dessus des fesses', 'Livre exam clinique et bilan', 1.5, 0.95, 10, NULL, 'QuestionBool', false, NULL, NULL),
(104, 'Incapacite douleurs distales', 'Livre exam clinique et bilan', 2, 0.52, 10, NULL, 'QuestionBool', false, NULL, NULL),
(105, 'Pas d''enchainement', 'Livre exam clinique et bilan', 1, 0.31, 10, NULL, 'QuestionBool', false, NULL, NULL),
(106, 'Avez-vous des douleurs dans vos membres inferieurs en marchant et qui diminuent en vous asseyant ?', 'Livre exam clinique et bilan', 0.82, 1.27, 10, NULL, 'QuestionBool', false, NULL, NULL),
(107, 'Sentez-vous que vous marchez mieux lorsque vous poussez un caddie de supermarche?', 'Livre exam clinique et bilan', 1.9, 0.55, 10, NULL, 'QuestionBool', false, NULL, NULL),
(108, 'La position assise est celle ou vous sentez le mieux par rapport a vos symptomes?', 'Livre exam clinique et bilan', 1.5, 0.28, 10, NULL, 'QuestionBool', false, NULL, NULL),
(109, 'Marche/etre debout sont les pires positions selon vous ?', 'Livre exam clinique et bilan', 1.3, 0.33, 10, NULL, 'QuestionBool', false, NULL, NULL);

-- QUESTIONS NEUROPATHIQUE - TESTS PHYSIQUES (IDs 110-134)
INSERT INTO "Questions" ("Id", "Title", "SourceRv", "RVPositive", "RVNegative", "CategoryId", "ClusterId", "Discriminator", "HasPermanentAnswer", "Min", "Max") VALUES
(110, 'Tests SLR ou Slump positifs ?', 'https://www.jptonline.org/article/S1466-853X(10)00044-4/fulltext', 4.94, 0.19, 10, 10, 'QuestionBool', false, NULL, NULL),
(111, 'Douleur neuro-(bp->membre-distal) anormaux ?', 'chatgpt', 4, 0.3, 10, 10, 'QuestionBool', false, NULL, NULL),
(112, 'Reflexes diminues en comparaison avec le cote oppose Achilleen', 'Livre exam clinique et bilan', 4.7, 0.53, 10, 10, 'QuestionBool', false, NULL, NULL),
(113, 'Reflexes diminues en comparaison avec le cote oppose Rotulien', 'Livre exam clinique et bilan', 7.14, 0.54, 10, 10, 'QuestionBool', false, NULL, NULL),
(114, 'Reflexe aboli Achilleen', 'Livre exam clinique et bilan', 1.93, 0.3, 10, 10, 'QuestionBool', false, NULL, NULL),
(115, 'Reflexe aboli Rotulien/jambier', 'Livre exam clinique et bilan', 5.07, 0.26, 10, 10, 'QuestionBool', false, NULL, NULL),
(116, 'Reflexe aboli Rotellaire', 'Livre exam clinique et bilan', 6.23, 0.14, 10, 10, 'QuestionBool', false, NULL, NULL),
(117, 'Engourdissement', 'Livre exam clinique et bilan', 1.03, 0.94, 10, 10, 'QuestionBool', false, NULL, NULL),
(118, 'Picotement', 'Livre exam clinique et bilan', 0.97, 1.06, 10, 10, 'QuestionBool', false, NULL, NULL),
(119, 'Prone knee bending test', 'Physiotutot', 5.7, 0.54, 10, 10, 'QuestionBool', false, NULL, NULL),
(120, 'Test de lasegue croise', 'Physiotutot', 2.8, 0.8, 10, 10, 'QuestionBool', false, NULL, NULL),
(121, 'Flexion involontaire', 'Physiotutot', 1.7, 0.94, 10, 10, 'QuestionBool', false, NULL, NULL),
(122, 'Perte force ou endurance extension du genou', 'Physiotutot', 1.11, 0.85, 10, 10, 'QuestionBool', false, NULL, NULL),
(123, 'Jomball-Test perte de force ou d''endurance', 'Physiotutot', 1.55, 0.7, 10, 10, 'QuestionBool', false, NULL, NULL),
(124, 'Extenseur de l''hallux perte de force ou d''endurance', 'Physiotutot', 1.5, 0.72, 10, 10, 'QuestionBool', false, NULL, NULL),
(125, 'Flexion plantaire de la cheville perte de force ou d''endurance', 'Physiotutot', 1.2, 0.81, 10, 10, 'QuestionBool', false, NULL, NULL),
(126, 'Dermatome L4', 'Physiotutot', 1.61, 0.73, 10, 10, 'QuestionBool', false, NULL, NULL),
(127, 'dermatome L5', 'Physiotutot', 2.07, 0.72, 10, 10, 'QuestionBool', false, NULL, NULL),
(128, 'Dermatome S1', 'Physiotutot', 1.46, 0.63, 10, 10, 'QuestionBool', false, NULL, NULL),
(129, 'Two stage treadmill test', 'Physiotutot', 6.43, 0.54, 10, 10, 'QuestionBool', false, NULL, NULL),
(130, 'Test de romberg modifie', 'Livre exam clinique et bilan', 4.06, 0.68, 10, 10, 'QuestionBool', false, NULL, NULL),
(131, 'Pas de douleurs en position assise', 'Livre exam clinique et bilan', 6.6, 0.58, 10, 10, 'QuestionBool', false, NULL, NULL),
(132, 'Symptomes ameliores en position assise', 'Livre exam clinique et bilan', 3.1, 0.58, 10, 10, 'QuestionBool', false, NULL, NULL),
(133, 'Equilibre diminue', 'Livre exam clinique et bilan', 1.5, 0.57, 10, 10, 'QuestionBool', false, NULL, NULL),
(134, 'Equibre perturbe', 'Livre exam clinique et bilan', 1.5, 0.57, 10, 10, 'QuestionBool', false, NULL, NULL);

-- QUESTIONS ARTICULAIRE / STRUCTUREL - SCREENING (IDs 135-154)
INSERT INTO "Questions" ("Id", "Title", "SourceRv", "RVPositive", "RVNegative", "CategoryId", "ClusterId", "Discriminator", "HasPermanentAnswer", "Min", "Max") VALUES
(135, 'Douleur mecanique (varie avec activite/charge) ?', 'Expertise Clinique', 3, 0.3, 7, NULL, 'QuestionBool', false, NULL, NULL),
(136, 'Raideur matinal< 30 min?', 'Expertise Clinique', 5, 0.4, 7, NULL, 'QuestionBool', false, NULL, NULL),
(137, 'Douleur amelioree par le mouvement?', 'Expertise Clinique', 4, 0.5, 7, NULL, 'QuestionBool', false, NULL, NULL),
(138, 'Douleur aggravee par le mouvement ?', 'Expertise Clinique', 2.5, 0.6, 7, NULL, 'QuestionBool', false, NULL, NULL),
(139, 'Douleur a la percussion vertebrale ?', 'Expertise Clinique', 3, 0.4, 7, NULL, 'QuestionBool', false, NULL, NULL),
(140, 'Instabilite articulaire suspectee (Laslett +) ?', 'Expertise Clinique', 2.5, 0.5, 7, NULL, 'QuestionBool', false, NULL, NULL),
(141, 'Distraction +', 'Expertise Clinique', 4, 0.5, 7, NULL, 'QuestionBool', false, NULL, NULL),
(142, 'Compression +', 'Expertise Clinique', 4, 0.5, 7, NULL, 'QuestionBool', false, NULL, NULL),
(143, 'thigh trust', 'Expertise Clinique', 4, 0.5, 7, NULL, 'QuestionBool', false, NULL, NULL),
(144, 'gaenslein', 'Expertise Clinique', 4, 0.5, 7, NULL, 'QuestionBool', false, NULL, NULL),
(145, 'sacral thrust', 'Expertise Clinique', 4, 0.5, 7, NULL, 'QuestionBool', false, NULL, NULL),
(146, 'La douleur n''est pas apaisee par la position allongee', 'Livre exam et bilan', 1.57, 0.41, 7, NULL, 'QuestionBool', false, NULL, NULL),
(147, 'douleur nocturne', 'Livre exam et bilan', 1.51, 0.55, 7, NULL, 'QuestionBool', false, NULL, NULL),
(148, 'raideur matinale pendant plus d''une demi-heure', 'Livre exam et bilan', 1.56, 0.68, 7, NULL, 'QuestionBool', false, NULL, NULL),
(149, 'La douleur ou la raideur sont calmees par l''exercice', 'Livre exam et bilan', 1.3, 0.6, 7, NULL, 'QuestionBool', false, NULL, NULL),
(150, 'l''age du debut est inferieur ou egal a 40 ans', 'Livre exam et bilan', 1.07, 0, 7, NULL, 'QuestionBool', false, NULL, NULL),
(151, 'Extension lombale passive', 'test clinique', 8.8, 0.2, 7, NULL, 'QuestionBool', false, NULL, NULL),
(152, 'Examen dans la cuisse apres 30s d''extension', 'test clinique', 16, 0.7, 7, NULL, 'QuestionBool', false, NULL, NULL),
(153, 'Flexion lombale > 53°', 'Livre exam et bilan', 1.3, 0.53, 7, NULL, 'QuestionBool', false, NULL, NULL),
(154, 'Extension totale > 26', 'Livre exam et bilan', 2.1, 0.68, 7, NULL, 'QuestionBool', false, NULL, NULL);

-- QUESTIONS ARTICULAIRE / STRUCTUREL - TESTS PHYSIQUES (IDs 155-177)
INSERT INTO "Questions" ("Id", "Title", "SourceRv", "RVPositive", "RVNegative", "CategoryId", "ClusterId", "Discriminator", "HasPermanentAnswer", "Min", "Max") VALUES
(155, 'Mobilite active lombaire limitee ?', 'Expertise Clinique', 3, 0.4, 7, 7, 'QuestionBool', false, NULL, NULL),
(156, 'Mobilite passive lombaire limitee ?', 'Expertise Clinique', 3, 0.4, 7, 7, 'QuestionBool', false, NULL, NULL),
(157, 'Palpation lombaire douloureuse ?', 'Expertise Clinique', 3, 0.4, 7, 7, 'QuestionBool', false, NULL, NULL),
(158, 'Douleur referee fessiere ou proximale ?', 'Expertise Clinique', 3, 0.4, 7, 7, 'QuestionBool', false, NULL, NULL),
(159, 'Tests de reproduction McKenzie positifs ?', 'Expertise Clinique', 3, 0.4, 7, 7, 'QuestionBool', false, NULL, NULL),
(160, 'Mesure de l''expansion thoracique < 7 cm', 'test clinique', 1.34, 0.7, 7, 7, 'QuestionBool', false, NULL, NULL),
(161, 'Mesure de l''expansion thoracique <2.5 cm', 'test clinique', 0.31, 0.68, 7, 7, 'QuestionBool', false, NULL, NULL),
(162, 'diminution de la flexion lombaire', 'test clinique', 1.9, 0.73, 7, 7, 'QuestionBool', false, NULL, NULL),
(163, 'Diminution de la lordose lombale', 'test clinique', 1.8, 0.8, 7, 7, 'QuestionBool', false, NULL, NULL),
(164, 'Hypersensibilite directe de la jonction sacro-iliaque', 'test clinique', 0.84, 1.07, 7, 7, 'QuestionBool', false, NULL, NULL),
(165, 'Test de romberg anormal', 'test clinique', 4.3, 0.67, 7, 7, 'QuestionBool', false, NULL, NULL),
(166, 'Test sur tapis roulant en deux etapes (temps d''apparition)', 'test clinique', 4.07, 0.33, 7, 7, 'QuestionBool', false, NULL, NULL),
(167, 'Test sur tapis roulant en deux etapes (distance parcourue)', 'test clinique', 6.46, 0.54, 7, 7, 'QuestionBool', false, NULL, NULL),
(168, 'Test sur tapis roulant en deux etapes (duree de retour a la normale)', 'test clinique', 2.53, 0.28, 7, 7, 'QuestionBool', false, NULL, NULL),
(169, 'Extension lombale passive', 'test clinique', 8.8, 0.2, 7, 7, 'QuestionBool', false, NULL, NULL),
(170, 'Prone instability test', 'Expertise Clinique', 1.67, 0.5, 7, 7, 'QuestionBool', false, NULL, NULL),
(171, 'Changement dans l''ecart inter-epineux', 'Expertise Clinique', 8.84, 0.17, 7, 7, 'QuestionBool', false, NULL, NULL),
(172, 'Low midline sill sign', 'Expertise Clinique', 8.84, 0.17, 7, 7, 'QuestionBool', false, NULL, NULL),
(173, 'N''importe quelle hypermobilite durant les tests intervertebraux', 'Expertise Clinique', 3, 0.53, 7, 7, 'QuestionBool', false, NULL, NULL),
(174, 'N''importe quelle hypermobilite durant les tests intervertebraux', 'Expertise Clinique', 2.4, 0.66, 7, 7, 'QuestionBool', false, NULL, NULL),
(175, 'Hyperflexion rachidienne', 'Expertise Clinique', 12.8, 0.72, 7, 7, 'QuestionBool', false, NULL, NULL),
(176, 'Hyperflexion rachidienne', 'Expertise Clinique', 4.94, 0.13, 7, 7, 'QuestionBool', false, NULL, NULL),
(177, 'Test de Kemp', 'Expertise Clinique', 0.86, 1.6, 7, 7, 'QuestionBool', false, NULL, NULL);

-- QUESTIONS CONTRÔLE SENSORI-MOTEUR SUPPLÉMENTAIRES (IDs 178-184)
INSERT INTO "Questions" ("Id", "Title", "SourceRv", "RVPositive", "RVNegative", "CategoryId", "ClusterId", "Discriminator", "HasPermanentAnswer", "Min", "Max") VALUES
(178, 'Age < 40 ans', 'Expertise Clinique', 2.5, 0.6, 12, NULL, 'QuestionBool', false, NULL, NULL),
(179, 'Hypermobilite', 'Expertise Clinique', 3.5, 0.55, 12, 12, 'QuestionBool', false, NULL, NULL),
(180, 'Douleur augmentee en position prolongee', 'Expertise Clinique', 3.5, 0.55, 12, NULL, 'QuestionBool', false, NULL, NULL),
(181, 'Catch ou giving way en flexion', 'Expertise Clinique', 3.5, 0.55, 12, NULL, 'QuestionBool', false, NULL, NULL),
(182, 'Test de shear positif', 'Expertise Clinique', 4, 0.55, 12, 12, 'QuestionBool', false, NULL, NULL),
(183, 'SLR positif en actif', 'Expertise Clinique', 3.5, 0.45, 12, 12, 'QuestionBool', false, NULL, NULL),
(184, 'Apprehension lors de mouvement rapide', 'Expertise Clinique', 4, 0.45, 12, NULL, 'QuestionBool', false, NULL, NULL);

-- QUESTIONS MYOFASCIAL (IDs 185-189)
INSERT INTO "Questions" ("Id", "Title", "SourceRv", "RVPositive", "RVNegative", "CategoryId", "ClusterId", "Discriminator", "HasPermanentAnswer", "Min", "Max") VALUES
(185, 'Points gachettes palpes (Travell / Simons) ?', 'Expertise Clinique', 3, 0.6, 8, 8, 'QuestionBool', false, NULL, NULL),
(186, 'Tension musculaire locale ou irradiee ?', 'Expertise Clinique', 2.5, 0.5, 8, 8, 'QuestionBool', false, NULL, NULL),
(187, 'Tests Sorensen / Ito-Shirado / Side-bridge faibles ?', 'Expertise Clinique', 3.5, 0.5, 8, 8, 'QuestionLadder', false, 0, 60),
(188, 'Endurance musculaire du tronc faible ?', 'Expertise Clinique', 3, 0.55, 8, 8, 'QuestionLadder', false, 0, 60),
(189, 'Douleur reproduite a la compression musculaire ?', 'Expertise Clinique', 3, 0.55, 8, 8, 'QuestionBool', false, NULL, NULL);

-- QUESTION NOCIPLASTIQUE SUPPLÉMENTAIRE (ID 190)
INSERT INTO "Questions" ("Id", "Title", "SourceRv", "RVPositive", "RVNegative", "CategoryId", "ClusterId", "Discriminator", "HasPermanentAnswer", "Min", "Max") VALUES
(190, 'Fatigue generalisee ?', 'Livre', 3, 0.5, 11, NULL, 'QuestionLadder', false, 1, 10);

-- ========================================
-- QUESTION PATHOLOGIES (IDs 1-191)
-- ========================================
INSERT INTO "QuestionPathologies" ("Id", "QuestionId", "PathologyId") VALUES
-- QUESTIONS PERMANENTES (1-9)
(1, 1, 1),    -- Antécédent personnel cancer → Métastase vertébrale
(2, 2, 2),    -- Antécédent familial cancer → Cancer métastatique
(3, 3, 5),    -- Antécédent infection/chirurgie → Spondylodiscite
(4, 4, 17),   -- Traitement corticoïdes → Fracture vertébrale aiguë
(5, 5, 24),   -- Antécédent psoriasis/MICI → Spondylarthrite
(6, 6, 7),    -- Antécédent familial anévrisme → Anévrisme aorte abdominale
(7, 7, 28),   -- Anticoagulants → Malignité ou infection possible
(8, 8, 20),   -- Age > 50 ans → Lombalgie mécanique
(9, 9, 58),   -- Age > 65 ans → Neuropathique

-- QUESTIONS TUMEUR / MÉTASTASE (10-15)
(10, 10, 2),  -- Perte de poids → Cancer métastatique
(11, 11, 1),  -- Douleur nocturne → Métastase vertébrale
(12, 12, 1),  -- Douleur progressive → Métastase vertébrale
(13, 13, 2),  -- Fatigue générale → Cancer métastatique
(14, 14, 3),  -- Douleur percussion → Spondylodiscite ou métastase
(15, 15, 4),  -- Déformation vertébrale → Tumeur vertébrale

-- QUESTIONS INFECTION (16-23)
(16, 16, 5),  -- Fièvre → Spondylodiscite
(17, 17, 5),  -- Signes systémiques → Spondylodiscite
(18, 18, 5),  -- Antécédent infection → Spondylodiscite
(19, 19, 5),  -- Immunodépression → Spondylodiscite
(20, 20, 7),  -- Douleur thoracique → Anévrisme aorte abdominale
(21, 21, 5),  -- Sueurs nocturnes → Spondylodiscite
(22, 22, 5),  -- Douleur persistante antibiotique → Spondylodiscite
(23, 23, 6),  -- Voyage zone endémique → Spondylodiscite tuberculeuse

-- QUESTIONS NEUROLOGIQUE (24-30)
(24, 24, 8),  -- Anesthésie en selle → Syndrome queue de cheval
(25, 25, 8),  -- Rétention urinaire → Syndrome queue de cheval
(26, 26, 8),  -- Faiblesse motrice → Syndrome queue de cheval
(27, 27, 8),  -- Troubles marche → Syndrome queue de cheval
(28, 28, 9),  -- Douleur radiculaire déficit → Radiculopathie compressive aiguë
(29, 29, 10), -- Douleur radiculaire faiblesse → Radiculopathie compressive progressive
(30, 30, 11), -- Altération réflexes → Compression médullaire

-- QUESTIONS TRAUMATISME / FRACTURE (31-36)
(31, 31, 16), -- Douleur après traumatisme → Fracture vertébrale
(32, 32, 17), -- Traitement corticoïdes → Fracture vertébrale aiguë
(33, 33, 16), -- Contusion dorsale → Fracture vertébrale
(34, 34, 19), -- Douleur aiguë effort → Traumatisme vertébral grave
(35, 35, 18), -- Douleur persistante → Fracture ostéoporotique
(36, 36, 18), -- Choc violent → Fracture ostéoporotique

-- QUESTIONS INFLAMMATOIRE (37-41)
(37, 37, 24), -- Raideur matinale → Spondylarthrite
(38, 38, 24), -- Douleur améliorée exercice → Spondylarthrite
(39, 39, 24), -- Douleur non soulagée repos → Spondylarthrite
(40, 40, 24), -- Réveil nocturne → Spondylarthrite
(41, 41, 24), -- Atteinte autres articulations → Spondylarthrite

-- QUESTIONS VASCULAIRE (42-48)
(42, 42, 26), -- Claudication → AOMI
(43, 43, 7),  -- Pâleur, froid → Anévrisme aorte abdominale
(44, 44, 27), -- Douleur abdominale + hypotension → AAA
(45, 45, 26), -- Douleur pulsatile → AOMI
(46, 46, 26), -- Pouls périphériques diminués → AOMI
(47, 46, 27), -- Pouls périphériques diminués → AAA (double)
(48, 47, 26), -- Tabagisme → AOMI
(49, 48, 20), -- Âge extrême → Lombalgie mécanique

-- QUESTIONS NOCIPLASTIQUE (49-53)
(50, 49, 74), -- Douleur diffuse → Bonne santé générale
(51, 50, 75), -- Hypersensibilité sensorielle → Sédentarité
(52, 51, 76), -- Sommeil non réparateur → Contrainte physique
(53, 52, 78), -- Douleur disproportionnée → Soutien social faible
(54, 53, 80), -- Plusieurs sites douloureux → Comportements à risque

-- QUESTIONS CROYANCES / COGNITION (54-59)
(55, 54, 63), -- Peur du mouvement → Kinésiophobie
(56, 55, 64), -- Catastrophisme → Catastrophisme
(57, 56, 65), -- Coping inefficace → Coping inefficace
(58, 57, 67), -- Croyances inadaptées → Croyances dysfonctionnelles
(59, 58, 68), -- Start Back élevé → Risque psychosocial élevé
(60, 59, 69), -- Auto-efficacité faible → Faible auto-efficacité

-- QUESTIONS ÉMOTIONNEL / AFFECTIF (60-64)
(61, 60, 70), -- Anxiété dépression → Anxiété / dépression
(62, 61, 71), -- Sommeil perturbé → Trouble du sommeil
(63, 62, 71), -- Score PSQI → Trouble du sommeil
(64, 63, 73), -- Tension émotionnelle → Composante émotionnelle / irritabilité
(65, 64, 74), -- Bonne santé générale → Bonne santé générale

-- QUESTIONS SOCIO-ENVIRONNEMENTAL (65-72)
(66, 65, 75), -- Activité physique faible → Sédentarité
(67, 66, 77), -- Contrainte physique élevée → Contrainte physique élevée
(68, 67, 78), -- Soutien social faible → Soutien social faible
(69, 68, 79), -- Satisfaction travail diminuée → Insatisfaction au travail
(70, 69, 80), -- Consommation alcool → Comportements à risque
(71, 70, 81), -- SF-36 altéré → Altération qualité de vie
(72, 71, 82), -- Travail pénible → Travail
(73, 72, 83), -- Tabagisme → Tabagisme

-- QUESTIONS DOULEUR NOCICEPTIVE (73-78)
(74, 73, 58), -- Douleur localisée → Neuropathique
(75, 74, 58), -- Douleur aiguë → Neuropathique
(76, 75, 58), -- Douleur charge mécanique → Neuropathique
(77, 76, 58), -- Soulagement repos → Neuropathique
(78, 77, 58), -- Douleur proportionnelle → Neuropathique
(79, 78, 58), -- Centralisation → Neuropathique

-- QUESTIONS CONTRÔLE SENSORIMOTEUR (79-87)
(80, 79, 34), -- Tests proprioceptifs → Instabilité segmentaire (Hicks)
(81, 80, 32), -- Équilibre postural → Instabilité lombaire
(82, 81, 36), -- Force isométrique faible → Hypermobilité
(83, 82, 21), -- Endurance gainage → Lombalgie mécanique posturale
(84, 83, 33), -- Tests fonctionnels → Instabilité segmentaire
(85, 84, 51), -- Luomajoki → Radiculopathie
(86, 85, 60), -- Pression 40mmgh → Appréhension mouvementale
(87, 86, 61), -- Règle Hicks 2 éléments → Douleur mécanique locale
(88, 87, 20), -- Règle Hicks 3 éléments → Lombalgie mécanique

-- QUESTIONS NEUROPATHIQUE - SCREENING (88-109)
(89, 88, 56),  -- Douleur trajet neuro → Radiculopathie S1
(90, 89, 55),  -- Brûlures → Radiculopathie L5
(91, 90, 57),  -- Engourdissement hypoesthésie → Faiblesse psoas / L2–L3
(92, 91, 53),  -- Déficit moteur → Radiculopathie L3–L4
(93, 92, 55),  -- DN4 positif → Radiculopathie L5
(94, 93, 56),  -- Sensibilité pique touche → Radiculopathie S1
(95, 94, 51),  -- Sensibilité vibration → Radiculopathie
(96, 95, 58),  -- Faiblesse Gastrocnémien → Neuropathique
(97, 96, 58),  -- Faiblesse Long extenseur → Neuropathique
(98, 97, 58),  -- Faiblesse Fléchisseur hanche → Neuropathique
(99, 98, 58),  -- Faiblesse fléchisseur genou → Neuropathique
(100, 99, 54), -- Lasègue → Radiculopathie L4
(101, 100, 37), -- Brûlure (Eridure) → Claudication neurogénique
(102, 101, 48), -- Test hyperflexion genou → Trouble équilibre central
(103, 102, 62), -- Douleur au-dessous genoux → Nociplastique
(104, 103, 62), -- Douleur au-dessus fesses → Nociplastique
(105, 104, 63), -- Incapacité douleurs distales → Kinésiophobie
(106, 105, 62), -- Pas d'enchaînement → Nociplastique
(107, 106, 69), -- Douleurs MI marchant → Faible auto-efficacité
(108, 107, 70), -- Marche mieux caddie → Anxiété / dépression
(109, 108, 71), -- Position assise meilleure → Trouble du sommeil
(110, 109, 71), -- Marche/debout pires → Trouble du sommeil

-- QUESTIONS NEUROPATHIQUE - TESTS PHYSIQUES (110-134)
(111, 110, 56), -- Tests SLR/Slump → Radiculopathie S1
(112, 111, 54), -- Douleur neuro anormaux → Radiculopathie L4
(113, 112, 58), -- Réflexes diminués Achilléen → Neuropathique
(114, 113, 52), -- Réflexes diminués Rotulien → Radiculopathie haute
(115, 114, 52), -- Réflexe aboli Achilléen → Radiculopathie haute
(116, 115, 52), -- Réflexe aboli Rotulien/jambier → Radiculopathie haute
(117, 116, 52), -- Réflexe aboli Rotellaire → Radiculopathie haute
(118, 117, 55), -- Engourdissement → Radiculopathie L5
(119, 118, 56), -- Picotement → Radiculopathie S1
(120, 119, 37), -- Prone knee bending → Claudication neurogénique
(121, 120, 51), -- Test lasègue croisé → Radiculopathie
(122, 121, 37), -- Flexion involontaire → Claudication neurogénique
(123, 122, 37), -- Perte force extension genou → Claudication neurogénique
(124, 123, 37), -- Dorsiflexion → Claudication neurogénique
(125, 124, 37), -- Extenseur hallux → Claudication neurogénique
(126, 125, 37), -- Flexion plantaire → Claudication neurogénique
(127, 126, 37), -- Dermatome L4 → Claudication neurogénique
(128, 127, 37), -- Dermatome L5 → Claudication neurogénique
(129, 128, 39), -- Dermatome S1 → Claudication neurogénique (Cook+)
(130, 129, 62), -- Two stage treadmill → Nociplastique
(131, 130, 62), -- Test romberg modifié → Nociplastique
(132, 131, 62), -- Pas de douleurs position assise → Nociplastique
(133, 132, 64), -- Symptômes améliorés position assise → Catastrophisme
(134, 133, 65), -- Équilibre diminué → Coping inefficace
(135, 134, 68), -- Équilibre perturbé → Risque psychosocial élevé

-- QUESTIONS ARTICULAIRE / STRUCTUREL - SCREENING (135-154)
(136, 135, 24), -- Douleur mécanique → Spondylarthrite
(137, 136, 20), -- Raideur matinal < 30 min → Lombalgie mécanique
(138, 137, 20), -- Douleur améliorée mouvement → Lombalgie mécanique
(139, 138, 20), -- Douleur aggravée mouvement → Lombalgie mécanique
(140, 139, 29), -- Douleur percussion vertébrale → Douleur sacro-iliaque
(141, 140, 29), -- Instabilité articulaire (Laslett +) → Douleur sacro-iliaque
(142, 141, 29), -- Distraction + → Douleur sacro-iliaque
(143, 142, 29), -- Compression + → Douleur sacro-iliaque
(144, 143, 29), -- Thigh trust → Douleur sacro-iliaque
(145, 144, 30), -- Gaenslen → Douleur référée myofasciale ou SIJ
(146, 145, 22), -- Sacral thrust → Lombalgie discogénique
(147, 146, 24), -- Douleur non apaisée allongée → Spondylarthrite
(148, 147, 24), -- Douleur nocturne → Spondylarthrite
(149, 148, 24), -- Raideur matinale > 30 min → Spondylarthrite
(150, 149, 24), -- Douleur calmée par exercice → Spondylarthrite
(151, 150, 24), -- Age début ≤ 40 ans → Spondylarthrite
(152, 151, 35), -- Extension lombale passive → Hypermobilité lombaire
(153, 152, 40), -- Douleurs cuisse après 30s → Sténose lombaire
(154, 153, 43), -- Flexion lombale > 53° → Myofascial (trigger points)
(155, 154, 42), -- Extension totale > 26 → Myofascial

-- QUESTIONS ARTICULAIRE / STRUCTUREL - TESTS PHYSIQUES (155-177)
(156, 155, 20), -- Mobilité active limitée → Lombalgie mécanique
(157, 156, 20), -- Mobilité passive limitée → Lombalgie mécanique
(158, 157, 20), -- Palpation lombaire douloureuse → Lombalgie mécanique
(159, 158, 23), -- Douleur référée fessière → Lombalgie inflammatoire
(160, 159, 31), -- Tests McKenzie → Douleur inflammatoire
(161, 160, 24), -- Expansion thoracique < 7 cm → Spondylarthrite
(162, 161, 24), -- Expansion thoracique < 2.5 cm → Spondylarthrite
(163, 162, 32), -- Diminution flexion lombaire → Instabilité lombaire
(164, 163, 38), -- Diminution lordose → Claudication neurogénique (sténose)
(165, 164, 37), -- Hypersensibilité jonction SI → Claudication neurogénique
(166, 165, 37), -- Test romberg anormal → Claudication neurogénique
(167, 166, 32), -- Tapis roulant temps apparition → Instabilité lombaire
(168, 167, 32), -- Tapis roulant distance → Instabilité lombaire
(169, 168, 32), -- Tapis roulant retour normale → Instabilité lombaire
(170, 169, 35), -- Extension lombale passive → Hypermobilité lombaire
(171, 170, 35), -- Prone instability test → Hypermobilité lombaire
(172, 171, 35), -- Changement écart inter-épineux → Hypermobilité lombaire
(173, 172, 35), -- Low midline sill sign → Hypermobilité lombaire
(174, 173, 41), -- Manque d'hypomobilité → Douleur facettaire
(175, 174, 44), -- N'importe quelle hypermobilité → Déconditionnement musculaire
(176, 175, 42), -- Hyperflexion rachidienne (1ère) → Myofascial
(177, 176,46), -- Hyperflexion rachidienne (2ème) → Dysfonction du contrôle moteur
(178, 177, 47), -- Test de Kemp → Trouble de l'équilibre / contrôle moteur
-- QUESTIONS CONTRÔLE SENSORI-MOTEUR SUPPLÉMENTAIRES (178-184)
(179, 178, 20), -- Age < 40 ans → Lombalgie mécanique
(180, 179, 20), -- Hypermobilité → Lombalgie mécanique
(181, 180, 20), -- Douleur augmentée position prolongée → Lombalgie mécanique
(182, 181, 59), -- Catch ou giving way → Centralisation discogénique
(183, 182, 51), -- Test de shear positif → Radiculopathie
(184, 183, 58), -- SLR positif actif → Neuropathique
(185, 184, 58), -- Appréhension mouvement rapide → Neuropathique
-- QUESTIONS MYOFASCIAL (185-189)
(186, 185, 49), -- Points gâchettes → Faiblesse musculaire
(187, 186, 45), -- Tension musculaire → Déconditionnement
(188, 187, 50), -- Tests Sorensen → Faible capacité fonctionnelle
(189, 188, 46), -- Endurance musculaire tronc → Dysfonction du contrôle moteur
(190, 189, 34), -- Douleur compression musculaire → Instabilité segmentaire (Hicks)
-- QUESTION NOCIPLASTIQUE SUPPLÉMENTAIRE (190)
(191, 190, 78); -- Fatigue généralisée → Soutien social faible

-- ========================================
-- DONNÉES DYNAMIQUES (sans IDs manuels)
-- Physios, Patients, Administrators
-- ========================================
INSERT INTO "Physios" ("Id", "LastName", "FirstName", "Email", "PhoneNumber", "Password", "INAMINumber")
VALUES
(1, 'Skywalker', 'Etienne', 'skywalker@kinestat.com', '00352621774537', 'EszYFFGkPp4GfI+8jmfiBW1yBGfMiNfRjlTllYNgOHhXvTkHhxMYO/FRugKR00V2', 789101),
(0, 'LeMaire', 'Luc', 'lemaire@kinestat.com', '00352621774537', '3eD/EtCsK7thQO/AIJrGiuoYWaiKlzrC5qiWNewHiijyb/5vsfaHng+ASJjSWS/q', 123456);
INSERT INTO "Administrators" ("LastName", "FirstName", "Email", "Password")
VALUES ('Ramos', 'Maxence', 'ramos@kinestat.com', '79z5R4e+NYXNmhAI4h7n+eUk8C0qQkdf+bDz+f70a8+wkXUjsThhnJ1aJdt6vTtz');

-- Patients (IDs auto-générés pour ces données dynamiques)
INSERT INTO "Patients"
("LastName", "FirstName", "Email", "PhoneNumber", "BirthDate", "Weight", "Height", "Gender", "SocialSecurityNumber", "Status", "Address", "Country", "PhysioId", "CreatedDate", "IsAnonymized", "InactiveSinceDate")
VALUES
('O', 'Ryan', 'ryano@gmail.com', '+32 471 123 456', '1985-03-15', 75, 180, 0, '12345678901', 0, 'Rue de la Paix 12, 1000 Bruxelles', 0, 1, CURRENT_TIMESTAMP - INTERVAL '2 years', false, NULL),
('Moreau', 'Sophie', 'sophie.moreau@example.com', '+32 472 654 321', '1990-07-22', 62, 165, 1, '23456789012', 0, 'Avenue Louise 45, 1050 Bruxelles', 0, 1, CURRENT_TIMESTAMP - INTERVAL '5 years', false, NULL),
('Leroy', 'Alexandre', 'alexandre.leroy@example.com', '+32 473 987 654', '2000-11-05', 80, 175, 0, '34567890123', 2, 'Chaussée de Charleroi 78, 1060 Bruxelles', 0, 1, CURRENT_TIMESTAMP - INTERVAL '25 years', false, CURRENT_TIMESTAMP - INTERVAL '21 years'),
('Dubois', 'Julie', 'julie.dubois@example.com', '+32 474 111 222', '1992-05-10', 58, 168, 1, '92051012345', 0, 'Rue Royale 20, 1000 Bruxelles', 0, 1, CURRENT_TIMESTAMP - INTERVAL '1 year', false, NULL),
('Peeters', 'Thomas', 'thomas.peeters@example.com', '+32 475 333 444', '1975-09-28', 85, 182, 0, '75092823456', 2, 'Kloosterstraat 5, 2000 Antwerpen', 0, 1, CURRENT_TIMESTAMP - INTERVAL '25 years', false, CURRENT_TIMESTAMP - INTERVAL '5 years'),
('Maes', 'Emma', 'emma.maes@example.com', '+32 476 555 666', '1998-02-14', 55, 162, 1, '98021434567', 0, 'Veldstraat 10, 9000 Gent', 0, 1, CURRENT_TIMESTAMP - INTERVAL '3 months', false, NULL),
('Janssens', 'Marc', 'marc.janssens@example.com', '+32 477 888 999', '1970-06-12', 90, 178, 0, '70061234567', 2, 'Mechelsesteenweg 100, 2018 Antwerpen', 0, 1, CURRENT_TIMESTAMP - INTERVAL '30 years', false, CURRENT_TIMESTAMP - INTERVAL '25 years'),
('Vermeulen', 'Laura', 'laura.vermeulen@example.com', '+32 478 222 333', '1988-09-20', 60, 170, 1, '88092012345', 0, 'Kortrijksesteenweg 50, 9000 Gent', 0, 0, CURRENT_TIMESTAMP - INTERVAL '15 years', false, NULL),
('Goossens', 'Nathan', 'nathan.goossens@example.com', '+32 479 121 314', '2001-08-20', 72, 176, 0, '01082067890', 0, 'Rue de Fer 15, 5000 Namur', 0, 1, CURRENT_TIMESTAMP - INTERVAL '2 years', false, NULL),
('De Smet', 'Lea', 'lea.desmet@example.com', '+32 480 232 425', '1995-12-05', 60, 164, 1, '95120578901', 0, 'Lippenslaan 88, 8300 Knokke', 0, 1, CURRENT_TIMESTAMP - INTERVAL '8 years', false, NULL),
('Vermeer', 'Hugo', 'hugo.vermeer@example.com', '+32 481 343 536', '1980-06-18', 90, 185, 0, '80061889012', 0, 'Meir 22, 2000 Antwerpen', 0, 1, CURRENT_TIMESTAMP - INTERVAL '19 years', false, NULL),
('Renard', 'Clara', 'clara.renard@example.com', '+32 482 454 647', '1955-01-25', 65, 160, 1, '55012590123', 2, 'Place Saint-Lambert 4, 4000 Liège', 0, 1, CURRENT_TIMESTAMP - INTERVAL '30 years', false, CURRENT_TIMESTAMP - INTERVAL '30 years'),
('Simon', 'Louis', 'louis.simon@example.com', '+32 483 565 758', '2005-03-03', 70, 175, 0, '05030301234', 0, 'Rue de la Loi 100, 1040 Bruxelles', 0, 1, CURRENT_TIMESTAMP - INTERVAL '6 months', false, NULL),
('Michel', 'Alice', 'alice.michel@example.com', '+32 484 676 869', '1983-07-14', 59, 167, 1, '83071412345', 0, 'Chaussée de Wavre 50, 1050 Ixelles', 0, 0, CURRENT_TIMESTAMP - INTERVAL '12 years', false, NULL),
('Lefebvre', 'Gabriel', 'gabriel.lefebvre@example.com', '+32 485 787 970', '1978-10-10', 82, 180, 0, '78101023456', 0, 'Avenue de Tervueren 200, 1150 Woluwe', 0, 1, CURRENT_TIMESTAMP - INTERVAL '15 years', false, NULL),
('Andre', 'Chloe', 'chloe.andre@example.com', '+32 486 898 081', '1999-09-09', 54, 163, 1, '99090934567', 0, 'Rue Haute 1, 1000 Bruxelles', 0, 1, CURRENT_TIMESTAMP - INTERVAL '1 year', false, NULL),
('Gerard', 'Leo', 'leo.gerard@example.com', '+32 487 909 192', '1990-02-22', 76, 177, 0, '90022245678', 0, 'Boulevard du Souverain 30, 1160 Auderghem', 0, 1, CURRENT_TIMESTAMP - INTERVAL '18 years', false, NULL),
('Bertrand', 'Manon', 'manon.bertrand@example.com', '+32 488 010 203', '1982-11-11', 63, 166, 1, '82111156789', 2, 'Rue Neuve 50, 6000 Charleroi', 0, 1, CURRENT_TIMESTAMP - INTERVAL '21 years', false, CURRENT_TIMESTAMP - INTERVAL '21 years'),
('Dumont', 'Noah', 'noah.dumont@example.com', '+32 489 121 314', '2003-05-30', 74, 179, 0, '03053067890', 0, 'Grand Rue 5, 7000 Mons', 0, 0, CURRENT_TIMESTAMP - INTERVAL '2 weeks', false, NULL),
('Roussel', 'Eva', 'eva.roussel@example.com', '+32 490 232 425', '1996-08-15', 57, 165, 1, '96081578901', 0, 'Rue des Guillemins 10, 4000 Liège', 0, 0, CURRENT_TIMESTAMP - INTERVAL '1 month', false, NULL);
-- ========================================
-- RÉINITIALISATION DES SÉQUENCES
-- Pour permettre l'ajout de nouvelles données via l'app
-- ========================================
SELECT setval('"Categories_Id_seq"', COALESCE((SELECT MAX("Id") FROM "Categories"), 1));
SELECT setval('"Pathologies_Id_seq"', COALESCE((SELECT MAX("Id") FROM "Pathologies"), 1));
SELECT setval('"Cluster_Id_seq"', COALESCE((SELECT MAX("Id") FROM "Cluster"), 1));
SELECT setval('"MedicalContexts_Id_seq"', COALESCE((SELECT MAX("Id") FROM "MedicalContexts"), 1));
SELECT setval('"PriorContexts_Id_seq"', COALESCE((SELECT MAX("Id") FROM "PriorContexts"), 1));
SELECT setval('"PriorContextPathologies_Id_seq"', COALESCE((SELECT MAX("Id") FROM "PriorContextPathologies"), 1));
SELECT setval('"Questions_Id_seq"', COALESCE((SELECT MAX("Id") FROM "Questions"), 1));
SELECT setval('"QuestionPathologies_Id_seq"', COALESCE((SELECT MAX("Id") FROM "QuestionPathologies"), 1));
SELECT setval('"Physios_Id_seq"', COALESCE((SELECT MAX("Id") FROM "Physios"), 1));
SELECT setval('"Administrators_Id_seq"', COALESCE((SELECT MAX("Id") FROM "Administrators"), 1));
SELECT setval('"Patients_Id_seq"', COALESCE((SELECT MAX("Id") FROM "Patients"), 1));
