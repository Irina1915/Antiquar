-- Запустить после подключения к базе antique_shop.
BEGIN;

CREATE TABLE IF NOT EXISTS users (
    id SERIAL PRIMARY KEY,
    login VARCHAR(50) NOT NULL UNIQUE,
    password TEXT NOT NULL,
    fullname VARCHAR(150) NOT NULL,
    email VARCHAR(150) NOT NULL DEFAULT '',
    phone VARCHAR(40) NOT NULL DEFAULT '',
    role VARCHAR(20) NOT NULL CHECK (role IN ('Admin', 'Guest', 'Customer'))
);

CREATE TABLE IF NOT EXISTS categories (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL UNIQUE
);

CREATE TABLE IF NOT EXISTS products (
    id SERIAL PRIMARY KEY,
    name VARCHAR(200) NOT NULL,
    category_id INTEGER REFERENCES categories(id) ON DELETE SET NULL,
    era VARCHAR(100) NOT NULL DEFAULT '',
    price NUMERIC(12, 2) NOT NULL CHECK (price >= 0),
    author VARCHAR(150) NOT NULL DEFAULT '',
    country VARCHAR(100) NOT NULL DEFAULT '',
    description TEXT NOT NULL DEFAULT '',
    authenticity_mark BOOLEAN NOT NULL DEFAULT FALSE,
    photo TEXT NOT NULL DEFAULT ''
);

CREATE TABLE IF NOT EXISTS requests (
    id SERIAL PRIMARY KEY,
    name VARCHAR(200) NOT NULL,
    category VARCHAR(100) NOT NULL DEFAULT '',
    era VARCHAR(100) NOT NULL DEFAULT '',
    price NUMERIC(12, 2) NOT NULL DEFAULT 0 CHECK (price >= 0),
    author VARCHAR(150) NOT NULL DEFAULT '',
    country VARCHAR(100) NOT NULL DEFAULT '',
    description TEXT NOT NULL DEFAULT '',
    phone VARCHAR(40) NOT NULL,
    photo TEXT NOT NULL DEFAULT '',
    status VARCHAR(20) NOT NULL DEFAULT 'pending' CHECK (status IN ('pending', 'review', 'approved', 'rejected')),
    submitted_by_user_id INTEGER REFERENCES users(id) ON DELETE SET NULL,
    catalog_product_id INTEGER
);

CREATE TABLE IF NOT EXISTS cart_items (
    user_id INTEGER NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    product_id INTEGER NOT NULL REFERENCES products(id) ON DELETE CASCADE,
    quantity INTEGER NOT NULL DEFAULT 1 CHECK (quantity > 0),
    PRIMARY KEY (user_id, product_id)
);

CREATE TABLE IF NOT EXISTS purchases (
    id BIGSERIAL PRIMARY KEY,
    buyer_id INTEGER NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    buyer_login VARCHAR(50) NOT NULL,
    buyer_name VARCHAR(150) NOT NULL,
    product_id INTEGER NOT NULL,
    name VARCHAR(200) NOT NULL,
    category VARCHAR(100) NOT NULL DEFAULT '',
    era VARCHAR(100) NOT NULL DEFAULT '',
    price NUMERIC(12, 2) NOT NULL CHECK (price >= 0),
    author VARCHAR(150) NOT NULL DEFAULT '',
    country VARCHAR(100) NOT NULL DEFAULT '',
    description TEXT NOT NULL DEFAULT '',
    authenticity_mark BOOLEAN NOT NULL DEFAULT FALSE,
    photo TEXT NOT NULL DEFAULT '',
    quantity INTEGER NOT NULL DEFAULT 1 CHECK (quantity > 0),
    purchased_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE INDEX IF NOT EXISTS ix_products_category_id ON products(category_id);
CREATE INDEX IF NOT EXISTS ix_requests_name ON requests(name);
CREATE INDEX IF NOT EXISTS ix_requests_owner ON requests(submitted_by_user_id);
CREATE INDEX IF NOT EXISTS ix_purchases_buyer ON purchases(buyer_id, purchased_at DESC);

-- Хэши начальных паролей рассчитаны в SHA-256 + Base64 формате User.CheckPassword.
INSERT INTO users (login, password, fullname, email, phone, role) VALUES
('demo_admin_1', 'RESET_REQUIRED', 'Demo Administrator 1', NULL, NULL, 'Admin'),
('demo_admin_2', 'RESET_REQUIRED', 'Demo Administrator 2', NULL, NULL, 'Admin'),
('demo_admin_3', 'RESET_REQUIRED', 'Demo Administrator 3', NULL, NULL, 'Admin'),
('demo_customer_1', 'RESET_REQUIRED', 'Demo Customer 1', NULL, NULL, 'Customer'),
('demo_customer_2', 'RESET_REQUIRED', 'Demo Customer 2', NULL, NULL, 'Customer'),
('demo_guest', 'RESET_REQUIRED', 'Demo Guest', NULL, NULL, 'Guest')
ON CONFLICT (login) DO UPDATE SET
    fullname = EXCLUDED.fullname,
    email = EXCLUDED.email,
    phone = EXCLUDED.phone,
    role = EXCLUDED.role;

INSERT INTO categories (name) VALUES
('Живопись'), ('Часы'), ('Фарфор'), ('Мебель'), ('Монеты'), ('Украшения')
ON CONFLICT (name) DO NOTHING;

INSERT INTO products (name, category_id, era, price, author, country, description, authenticity_mark, photo)
SELECT seed.name, category.id, seed.era, seed.price, seed.author, seed.country, seed.description, seed.authenticity_mark, ''
FROM (VALUES
 ('Пейзаж «Золотая осень»', 'Живопись', 'Начало XX века', 185000.00, 'Неизвестный художник', 'Россия', 'Масло на холсте в деревянной раме.', TRUE),
 ('Карманные часы «Молния»', 'Часы', 'Середина XX века', 12500.00, 'Челябинский часовой завод', 'СССР', 'Механические часы с ручным заводом.', TRUE),
 ('Фарфоровая статуэтка «Балерина»', 'Фарфор', '1970-е годы', 8400.00, 'ЛФЗ', 'СССР', 'Фарфоровая фигурка ручной росписи.', TRUE),
 ('Письменный стол', 'Мебель', 'Конец XIX века', 96000.00, 'Мастерская неизвестна', 'Россия', 'Стол из массива дерева с двумя выдвижными ящиками.', FALSE),
 ('Серебряный рубль 1898 года', 'Монеты', '1898 год', 22000.00, 'Монетный двор', 'Российская империя', 'Серебряная монета Николая II.', TRUE),
 ('Брошь с янтарём', 'Украшения', 'Середина XX века', 15700.00, 'Неизвестный мастер', 'СССР', 'Винтажная брошь в металлической оправе.', FALSE)
) AS seed(name, category_name, era, price, author, country, description, authenticity_mark)
JOIN categories category ON category.name = seed.category_name
WHERE NOT EXISTS (SELECT 1 FROM products p WHERE p.name = seed.name);

INSERT INTO requests (name, category, era, price, author, country, description, phone, photo)
SELECT seed.name, seed.category, seed.era, seed.price, seed.author, seed.country, seed.description, seed.phone, ''
FROM (VALUES
 ('Самовар «Тула»', 'Посуда', 'Начало XX века', 18000.00, 'Тульская мастерская', 'Россия', 'Самовар на углях, сохранена оригинальная ручка.', 'REDACTED'),
 ('Настольная лампа', 'Предметы интерьера', '1930-е годы', 12500.00, 'Неизвестный мастер', 'Франция', 'Латунное основание и стеклянный плафон.', 'REDACTED'),
 ('Набор открыток Москвы', 'Коллекционирование', '1960-е годы', 3200.00, 'Издательство «Планета»', 'СССР', 'Комплект из двенадцати открыток в конверте.', 'REDACTED')
) AS seed(name, category, era, price, author, country, description, phone)
WHERE NOT EXISTS (SELECT 1 FROM requests r WHERE r.name = seed.name AND r.phone = seed.phone);

COMMIT;
