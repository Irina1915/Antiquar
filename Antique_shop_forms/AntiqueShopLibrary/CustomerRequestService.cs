using System.Collections.Generic;
using AntiqueShopLibrary.BD;

namespace AntiqueShopLibrary
{
    public interface ICustomerRequestRepository
    {
        List<ProposedProduct> GetRequestsForUser(int userId);
        bool DeleteRequestForUser(int requestId, int userId);
    }

    public class CustomerRequestService
    {
        private readonly ICustomerRequestRepository _repository;
        public string LastMessage { get; private set; } = string.Empty;

        public CustomerRequestService(ICustomerRequestRepository repository)
        {
            _repository = repository;
        }

        public List<ProposedProduct> GetMyRequests(int userId)
        {
            if (userId <= 0)
            {
                LastMessage = "Войдите в аккаунт, чтобы посмотреть свои заявки.";
                return new List<ProposedProduct>();
            }

            LastMessage = string.Empty;
            return _repository.GetRequestsForUser(userId);
        }

        public bool DeleteMyRequest(int requestId, int userId)
        {
            if (requestId <= 0 || userId <= 0)
            {
                LastMessage = "Не удалось определить заявку или пользователя.";
                return false;
            }

            bool deleted = _repository.DeleteRequestForUser(requestId, userId);
            LastMessage = deleted ? "Заявка удалена." : "Заявка не найдена или уже удалена.";
            return deleted;
        }
    }
}
