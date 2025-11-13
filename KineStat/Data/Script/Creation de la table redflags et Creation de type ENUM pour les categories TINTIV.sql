-- 🔹 Création du type ENUM pour les catégories TINTIV
DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'redflagcategory') THEN
        CREATE TYPE redflagcategory AS ENUM (
            'Tumeur',
            'Infection',
            'Neurologique',
            'Traumatisme',
            'Inflammatoire',
            'Vasculaire'
        );
    END IF;
END $$;

-- 🔹 Création de la table RedFlag
CREATE TABLE IF NOT EXISTS "RedFlag" (
    "Id" SERIAL PRIMARY KEY,
    "Name" TEXT NOT NULL,
    "Category" redflagcategory NOT NULL,
    "SeverityLevel" INT CHECK ("SeverityLevel" BETWEEN 1 AND 5)
);
