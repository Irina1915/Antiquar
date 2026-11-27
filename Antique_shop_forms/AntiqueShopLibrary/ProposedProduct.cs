using System;
namespace AntiqueShopLibrary
{
    // Данные клиентской заявки; Photo и старые поля нужны для совместимости с формой и БД.
    public class ProposedProduct
    {
        // Идентификатор строки в таблице requests.
        public int Id { get; set; }
        // Основные сведения о предмете.
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Price { get; set; }
        // Контактная и прочая дополнительная информация.
        public string AdditionalInfo { get; set; } = string.Empty;

        // Характеристики, которые выводятся в каталоге и в очереди модерации.
        public string Category { get; set; } = string.Empty;
        public string Era { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        // Путь к фото; Photo — удобное имя для требований, PhotoPath сохраняет старый код.
        public string PhotoPath { get; set; } = string.Empty;
        public string Photo { get { return PhotoPath; } set { PhotoPath = value ?? string.Empty; } }
        // Дополнительные документы, если они приложены к заявке.
        public string Documents { get; set; } = string.Empty;
        // Текущий этап модерации: новая заявка или передана на рассмотрение.
        public string Status { get; set; } = "pending";
        // Идентификатор клиента, который отправил заявку; у старых заявок может отсутствовать.
        public int? SubmittedByUserId { get; set; }
        public int? CatalogProductId { get; set; }
    }
}
