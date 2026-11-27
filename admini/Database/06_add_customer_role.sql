-- Добавляет отдельные роли клиента и гостя в существующей базе.
BEGIN;

-- Снимает старое ограничение на роль, если оно присутствует.
DO $$
DECLARE
    role_check RECORD;
BEGIN
    FOR role_check IN
        SELECT conname
        FROM pg_constraint
        WHERE conrelid = 'public.users'::regclass
          AND contype = 'c'
          AND position('role' IN lower(pg_get_constraintdef(oid))) > 0
    LOOP
        EXECUTE format('ALTER TABLE public.users DROP CONSTRAINT %I', role_check.conname);
    END LOOP;
END $$;

-- Именные учётные записи с ролью Guest становятся клиентами.
UPDATE public.users
SET role = 'Customer'
WHERE role = 'Guest' AND login <> 'guest';

-- Существующие учётные записи сотрудников становятся клиентами.
UPDATE public.users
SET role = 'Customer'
WHERE lower(role) = 'employee';

-- Создаёт отдельную гостевую запись, нужную для гостевой корзины в БД.
INSERT INTO public.users (login, password, fullname, email, phone, role)
VALUES ('guest', '5LuZzCyQu75iAs3A9MYu99f/62owkZkOjYcdAeeabbQ=', 'Гость', '', '', 'Guest')
ON CONFLICT (login) DO UPDATE
SET fullname = EXCLUDED.fullname,
    email = EXCLUDED.email,
    phone = EXCLUDED.phone,
    role = EXCLUDED.role;

-- Разрешает только предусмотренные приложением роли.
ALTER TABLE public.users
ADD CONSTRAINT users_role_check
CHECK (role IN ('Admin', 'Guest', 'Customer'));

COMMIT;
