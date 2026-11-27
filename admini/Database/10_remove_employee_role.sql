-- Переводит существующих сотрудников в клиенты и удаляет роль Employee.
BEGIN;

-- Сначала снимает ограничение с прежним перечнем ролей.
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

-- Все старые учётные записи Employee становятся обычными клиентами.
UPDATE public.users
SET role = 'Customer'
WHERE lower(role) = 'employee';

-- Оставляет только роли администратора, клиента и гостя.
ALTER TABLE public.users
ADD CONSTRAINT users_role_check
CHECK (role IN ('Admin', 'Guest', 'Customer'));

COMMIT;
