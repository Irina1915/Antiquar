-- Дополняет существующую базу демонстрационными товарами, покупками и заявками.
-- Скрипт можно запускать повторно: записи не дублируются.
BEGIN;

-- Расширяет категории каталога и добавляет записи, чтобы варианты фильтров имели несколько результатов.
INSERT INTO products (name, category_id, era, price, author, country, description, authenticity_mark, photo)
SELECT seed.name, c.id, seed.era, seed.price, seed.author, seed.country,
       seed.description, seed.authenticity_mark, ''
FROM (VALUES
    ('Этюд «Старый сад»', 'Живопись', 'Конец XIX века', 42000.00, 'Неизвестный художник', 'Россия', 'Небольшой этюд маслом на холсте.', TRUE),
    ('Натюрморт с грушами', 'Живопись', '1970-е годы', 68500.00, 'Неизвестный художник', 'Россия', 'Натюрморт в простой деревянной раме.', FALSE),
    ('Настенные часы «Весна»', 'Часы', 'Начало XX века', 28500.00, 'Челябинский часовой завод', 'СССР', 'Механические настенные часы с маятником.', TRUE),
    ('Будильник с ключом', 'Часы', 'Середина XX века', 7600.00, 'Челябинский часовой завод', 'СССР', 'Настольный механический будильник.', FALSE),
    ('Чайная пара с цветочным узором', 'Фарфор', '1970-е годы', 5400.00, 'ЛФЗ', 'СССР', 'Фарфоровая чашка с блюдцем ручной росписи.', TRUE),
    ('Ваза «Летний вечер»', 'Фарфор', 'Середина XX века', 11900.00, 'ЛФЗ', 'СССР', 'Декоративная фарфоровая ваза.', FALSE),
    ('Комод из ореха', 'Мебель', 'Конец XIX века', 74000.00, 'Мастерская неизвестна', 'Россия', 'Небольшой комод с четырьмя ящиками.', TRUE),
    ('Стул с резной спинкой', 'Мебель', 'Начало XX века', 31800.00, 'Мастерская неизвестна', 'Россия', 'Деревянный стул с резным декором.', FALSE),
    ('Серебряная монета 1901 года', 'Монеты', '1898 год', 16400.00, 'Монетный двор', 'Российская империя', 'Серебряная монета начала XX века.', TRUE),
    ('Медная монета СССР', 'Монеты', 'Середина XX века', 950.00, 'Монетный двор', 'СССР', 'Коллекционная монета советского периода.', FALSE),
    ('Кулон с гранатом', 'Украшения', '1898 год', 23500.00, 'Неизвестный мастер', 'Российская империя', 'Винтажный кулон с красным камнем.', TRUE),
    ('Серебряные серьги', 'Украшения', 'Середина XX века', 12800.00, 'Неизвестный мастер', 'СССР', 'Пара серебряных серёг с простым узором.', FALSE),
    ('Зеркало в резной раме', 'Предметы интерьера', 'Начало XX века', 27500.00, 'Неизвестная мастерская', 'Россия', 'Настенное зеркало в деревянной резной раме.', TRUE),
    ('Подсвечник латунный', 'Предметы интерьера', 'Середина XX века', 4800.00, 'Неизвестный мастер', 'СССР', 'Небольшой латунный подсвечник с патиной.', FALSE),
    ('Браслет в стиле ар-деко', 'Украшения', '1930-е годы', 17500.00, 'Неизвестная мастерская', 'Франция', 'Металлический браслет с геометрическим орнаментом.', FALSE)
) AS seed(name, category_name, era, price, author, country, description, authenticity_mark)
JOIN categories c ON c.name = seed.category_name
WHERE NOT EXISTS (SELECT 1 FROM products p WHERE p.name = seed.name);

-- Пять демонстрационных записей архива; каждая появляется в покупках своего клиента.
WITH seed(buyer_login, product_name, purchased_at) AS (VALUES
    ('demo_admin_2', 'Пейзаж «Золотая осень»', TIMESTAMPTZ '2026-10-12 14:20:00+03'),
    ('demo_admin_2', 'Фарфоровая статуэтка «Балерина»', TIMESTAMPTZ '2026-10-18 11:05:00+03'),
    ('demo_admin_3', 'Карманные часы «Молния»', TIMESTAMPTZ '2026-10-21 16:40:00+03'),
    ('demo_customer_1', 'Натюрморт с грушами', TIMESTAMPTZ '2026-10-25 13:15:00+03'),
    ('demo_customer_2', 'Комод из ореха', TIMESTAMPTZ '2026-11-02 10:30:00+03')
)
INSERT INTO purchases
    (buyer_id, buyer_login, buyer_name, product_id, name, category, era, price, author,
     country, description, authenticity_mark, photo, quantity, purchased_at)
SELECT u.id, u.login, u.fullname, p.id, p.name, COALESCE(c.name, ''), p.era, p.price,
       p.author, p.country, p.description, p.authenticity_mark, p.photo, 1, seed.purchased_at
FROM seed
JOIN users u ON u.login = seed.buyer_login
JOIN products p ON p.name = seed.product_name
LEFT JOIN categories c ON c.id = p.category_id
WHERE NOT EXISTS (
    SELECT 1 FROM purchases old
    WHERE old.buyer_login = seed.buyer_login AND old.product_id = p.id
);

-- Три дополнительные заявки от клиентов; статус «pending» соответствует «Отправлено».
WITH seed(owner_login, name, category, era, price, author, country, description, phone) AS (VALUES
    ('demo_customer_2', 'Самовар с гравировкой', 'Посуда', 'Начало XX века', 24500.00, 'Тульская мастерская', 'Россия', 'Самовар с декоративной гравировкой и комплектом труб.', 'REDACTED'),
    ('demo_admin_2', 'Настольные часы в корпусе из дерева', 'Часы', 'Середина XX века', 19800.00, 'Неизвестный мастер', 'СССР', 'Рабочие настольные часы с деревянным корпусом.', 'REDACTED'),
    ('demo_admin_3', 'Фарфоровая тарелка с пейзажем', 'Фарфор', '1970-е годы', 6300.00, 'ЛФЗ', 'СССР', 'Декоративная тарелка с изображением загородного пейзажа.', 'REDACTED')
)
INSERT INTO requests
    (name, category, era, price, author, country, description, phone, photo, status, submitted_by_user_id)
SELECT seed.name, seed.category, seed.era, seed.price, seed.author, seed.country,
       seed.description, seed.phone, '', 'pending', u.id
FROM seed
JOIN users u ON u.login = seed.owner_login
WHERE NOT EXISTS (
    SELECT 1 FROM requests old WHERE old.name = seed.name AND old.submitted_by_user_id = u.id
);

COMMIT;
