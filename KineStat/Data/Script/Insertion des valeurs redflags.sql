INSERT INTO "RedFlag" ("Name", "Category", "SeverityLevel") VALUES
-- 🩸 Tumeur / Métastase
('Antécédent personnel de cancer (localisation/année)', 'Tumeur', 5),
('Perte de poids inexpliquée (>5% en 3–6 mois)', 'Tumeur', 4),
('Douleur nocturne sévère, réveillant le patient', 'Tumeur', 4),
('Douleur progressive sans amélioration malgré traitement (>4 semaines)', 'Tumeur', 3),
('Antécédent familial de cancer significatif (si pertinent)', 'Tumeur', 3),
('Fatigue générale, malaise, perte d’appétit', 'Tumeur', 2),
('Douleur à la percussion vertébrale', 'Tumeur', 3),
('Déformation vertébrale ou masse palpable', 'Tumeur', 4),

-- 🦠 Infection
('Fièvre ≥ 38 °C ou frissons récents', 'Infection', 3),
('Signes systémiques d’infection (sueurs nocturnes, fatigue importante)', 'Infection', 3),
('Antécédent récent d’infection (urinaire, cutanée, dentaire, IV drug use, chirurgie/épisiotomie)', 'Infection', 3),
('Immunodépression documentée (corticothérapie chronique)', 'Infection', 4),
('Douleur thoracique', 'Infection', 2),
('Sueurs nocturnes, fatigue, perte d’énergie', 'Infection', 3),
('Douleur persistante malgré traitement antibiotique préalable', 'Infection', 3),
('Voyage ou provenance d’une zone endémique tuberculeuse', 'Infection', 3),

-- ⚡ Neurologique
('Anesthésie en selle / hypoesthésie périnéale', 'Neurologique', 5),
('Rétention urinaire récente ou incontinence fécale / urinaire', 'Neurologique', 5),
('Faiblesse motrice aiguë ou progressive des membres inférieurs (MRC ≤ 3) ou chute récente', 'Neurologique', 5),
('Troubles de la marche rapides ou signes pyramidaux / trouble de la coordination', 'Neurologique', 4),
('Douleur radiculaire avec déficit moteur progressif', 'Neurologique', 4),
('Douleur radiculaire évoluant vers une faiblesse progressive', 'Neurologique', 4),
('Altération récente des réflexes ostéo-tendineux bilatéraux', 'Neurologique', 3),

-- 🦴 Traumatisme / Fracture
('Douleur lombaire après traumatisme majeur ou mineur chez sujet âgé/ostéoporotique', 'Traumatisme', 4),
('Traitement chronique par corticoïdes / ostéoporose connue / femme âgée >70 ans', 'Traumatisme', 3),
('Contusion / abrasion dorsale', 'Traumatisme', 2),
('Douleur localisée très aiguë après effort ou chute, perte fonctionnelle immédiate', 'Traumatisme', 3),
('Douleur persistante après traitement conservateur initial', 'Traumatisme', 3),
('Choc violent (AVP, chute de hauteur, écrasement)', 'Traumatisme', 4),

-- 🔥 Inflammatoire
('Raideur matinale >30 min surtout chez <45–50 ans', 'Inflammatoire', 3),
('Douleur améliorée à l’exercice mais pas au repos, début avant 40–50 ans, atteinte d’autres articulations', 'Inflammatoire', 3),
('Antécédent psoriasis, maladie inflammatoire intestinale ou antécédent familial', 'Inflammatoire', 2),
('Douleur non soulagée par le repos prolongé', 'Inflammatoire', 3),
('Réveil nocturne en 2e partie de nuit par douleur', 'Inflammatoire', 3),
('Atteinte d’autres articulations (épaule, hanche, genou)', 'Inflammatoire', 3),

-- ❤️ Vasculaire / Circulatoire
('Douleur à la marche qui cède au repos (claudication)', 'Vasculaire', 3),
('Pâleur, froid, diminution des pouls périphériques aux pieds', 'Vasculaire', 3),
('Douleur abdominale associée à hypotension ou douleur brève intense (anévrisme)', 'Vasculaire', 5),
('Douleur pulsatile ou battante dans la région lombaire', 'Vasculaire', 4),
('Pouls périphériques diminués ou asymétriques', 'Vasculaire', 3),
('Tabagisme, hypercholestérolémie, âge >65 ans', 'Vasculaire', 2),
('Antécédent familial d’anévrisme ou d’AOMI', 'Vasculaire', 3),

-- 🔁 Cas multi-catégories
-- Vasculaire / Traumatique
('Utilisation d’anticoagulants / antécédent d’anévrisme / chirurgie vasculaire', 'Vasculaire', 4),
('Utilisation d’anticoagulants / antécédent d’anévrisme / chirurgie vasculaire', 'Traumatisme', 4),

-- Tumeur / Infection / Autre
('Âge extrême pour le contexte (douleur nouvelle <18 ans ou >55–60 ans selon risque tumeur)', 'Tumeur', 3),
('Âge extrême pour le contexte (douleur nouvelle <18 ans ou >55–60 ans selon risque tumeur)', 'Infection', 3);
