-- Выполнить один раз в Query Tool базы antique_shop под пользователем postgres.
-- Перед запуском замените пароль на свой и сохраните его для переменной Windows
-- ANTIQUE_SHOP_DB_PASSWORD. Не отправляйте пароль в чат и не добавляйте его в Git.
CREATE ROLE antique_app LOGIN PASSWORD 'loh_ahah';

GRANT CONNECT ON DATABASE antique_shop TO antique_app;
GRANT USAGE ON SCHEMA public TO antique_app;
GRANT SELECT, INSERT, UPDATE, DELETE ON ALL TABLES IN SCHEMA public TO antique_app;
GRANT USAGE, SELECT ON ALL SEQUENCES IN SCHEMA public TO antique_app;

ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA public
    GRANT SELECT, INSERT, UPDATE, DELETE ON TABLES TO antique_app;
ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA public
    GRANT USAGE, SELECT ON SEQUENCES TO antique_app;
