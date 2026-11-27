using AntiqueShopLibrary.BD;
using System.Collections.Generic;

namespace AntiqueShopLibrary
{
    public class ProposedProductService
    {
        // Сервис отделяет правила модерации и проверки роли от интерфейса и SQL.
        private readonly IProposedProductRepository _repository;
        private readonly Role _role;
        public string LastMessage { get; private set; } = string.Empty;

        public ProposedProductService(IProposedProductRepository repository, Role role)
        {
            // Запоминает хранилище заявок и роль текущего пользователя.
            _repository = repository;
            _role = role;
        }

        // Возвращает заявки для таблицы администратора.
        public List<ProposedProduct> GetProposedProducts() => _repository.GetProposedProducts();

        public bool ApproveProduct(int id)
        {
            // Разрешает перенос в каталог только администратору.
            if (_role != Role.Admin) { LastMessage = "Недостаточно прав для выполнения операции"; return false; }
            if (!_repository.ApproveProduct(id)) { LastMessage = "Товар не найден"; return false; }
            LastMessage = "Товар одобрен и добавлен в каталог";
            return true;
        }

        public bool RejectProduct(int id)
        {
            // Проверяет права, затем удаляет заявку из очереди.
            if (_role != Role.Admin) { LastMessage = "Недостаточно прав для выполнения операции"; return false; }
            if (!_repository.RejectProduct(id)) { LastMessage = "Товар не найден"; return false; }
            LastMessage = "Товар отклонён";
            return true;
        }

        public bool MarkForReview(int id)
        {
            if (_role != Role.Admin) { LastMessage = "Недостаточно прав для выполнения операции"; return false; }
            if (!_repository.MarkForReview(id)) { LastMessage = "Заявка не найдена"; return false; }
            LastMessage = "Заявка отправлена на рассмотрение";
            return true;
        }
    }
}
