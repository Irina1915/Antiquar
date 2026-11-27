-- Добавляет личные заявки пользователей и историю покупок.
BEGIN;

ALTER TABLE public.requests
    ADD COLUMN IF NOT EXISTS submitted_by_user_id INTEGER REFERENCES public.users(id) ON DELETE SET NULL,
    ADD COLUMN IF NOT EXISTS catalog_product_id INTEGER;

-- Привязывает старые заявки по телефону, если он совпадает с контактом пользователя.
UPDATE public.requests AS request
SET submitted_by_user_id = users.id
FROM public.users AS users
WHERE request.submitted_by_user_id IS NULL
  AND request.phone <> ''
  AND request.phone = users.phone;

-- Новые значения статуса сохраняются для личного списка, а очередь модерации читает pending/review.
ALTER TABLE public.requests DROP CONSTRAINT IF EXISTS requests_status_check;
ALTER TABLE public.requests
    ADD CONSTRAINT requests_status_check
    CHECK (status IN ('pending', 'review', 'approved', 'rejected'));

CREATE TABLE IF NOT EXISTS public.purchases (
    id BIGSERIAL PRIMARY KEY,
    buyer_id INTEGER NOT NULL REFERENCES public.users(id) ON DELETE CASCADE,
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

CREATE INDEX IF NOT EXISTS ix_requests_owner ON public.requests(submitted_by_user_id);
CREATE INDEX IF NOT EXISTS ix_purchases_buyer ON public.purchases(buyer_id, purchased_at DESC);

GRANT SELECT, INSERT, UPDATE, DELETE ON public.requests TO antique_app;
GRANT SELECT, INSERT ON public.purchases TO antique_app;
GRANT USAGE, SELECT ON SEQUENCE public.purchases_id_seq TO antique_app;

COMMIT;
