-- Привязывает иллюстрации к старым заявкам, у которых ранее не было фото.
-- Изображения лежат в папке images и копируются в папку приложения при сборке.
-- Скрипт безопасно запускать повторно: он не перезаписывает уже заполненные фото.
BEGIN;

UPDATE requests
SET photo = 'images/request_001_moscow_postcards.jpg'
WHERE name = 'Набор открыток Москвы' AND COALESCE(photo, '') = '';

UPDATE requests
SET photo = 'images/request_002_tula_samovar.jpg'
WHERE name = 'Самовар «Тула»' AND COALESCE(photo, '') = '';

UPDATE requests
SET photo = 'images/request_006_table_clock.jpg'
WHERE name = 'Настольные часы в корпусе из дерева' AND COALESCE(photo, '') = '';

UPDATE requests
SET photo = 'images/request_007_landscape_plate.jpg'
WHERE name = 'Фарфоровая тарелка с пейзажем' AND COALESCE(photo, '') = '';

UPDATE requests
SET photo = 'images/request_008_engraved_samovar.jpg'
WHERE name = 'Самовар с гравировкой' AND COALESCE(photo, '') = '';

COMMIT;
