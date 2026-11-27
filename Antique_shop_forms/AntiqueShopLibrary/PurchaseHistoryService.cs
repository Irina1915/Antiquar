using System;
using System.Collections.Generic;
using Antique_shop;

namespace AntiqueShopLibrary
{
    public class PurchaseRecord
    {
        public Tovar Product { get; set; }
        public int BuyerId { get; set; }
        public string BuyerLogin { get; set; } = string.Empty;
        public string BuyerName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public DateTime PurchasedAt { get; set; }
    }

    public interface IPurchaseRepository
    {
        List<PurchaseRecord> GetPurchasesForUser(int userId);
        List<PurchaseRecord> GetPurchaseArchive();
        bool CompletePurchase(int userId, string buyerLogin, string buyerName, IEnumerable<CartItem> items);
    }

    public class PurchaseHistoryService
    {
        private readonly IPurchaseRepository _repository;
        public string LastMessage { get; private set; } = string.Empty;

        public PurchaseHistoryService(IPurchaseRepository repository)
        {
            _repository = repository;
        }

        public List<PurchaseRecord> GetMyPurchases(int userId)
        {
            if (userId <= 0)
            {
                LastMessage = "Войдите в аккаунт, чтобы посмотреть историю покупок.";
                return new List<PurchaseRecord>();
            }

            LastMessage = string.Empty;
            return _repository.GetPurchasesForUser(userId);
        }

        public List<PurchaseRecord> GetArchive(Role role)
        {
            if (role != Role.Admin)
            {
                LastMessage = "Недостаточно прав для просмотра архива покупок.";
                return new List<PurchaseRecord>();
            }

            LastMessage = string.Empty;
            return _repository.GetPurchaseArchive();
        }

        public bool CompletePurchase(User buyer, IEnumerable<CartItem> items)
        {
            if (buyer == null || buyer.Id <= 0)
            {
                LastMessage = "Для оформления покупки войдите в аккаунт.";
                return false;
            }
            if (items == null)
            {
                LastMessage = "Корзина пуста.";
                return false;
            }

            bool completed = _repository.CompletePurchase(buyer.Id, buyer.Login, buyer.FullName, items);
            LastMessage = completed ? "Покупка оформлена." : "Не удалось завершить покупку.";
            return completed;
        }
    }
}
