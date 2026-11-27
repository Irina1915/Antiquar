using Antique_shop;
using System.Collections.Generic;

namespace AntiqueShopLibrary
{
    /// <summary>Storage contract for the cart operations used by CartService.</summary>
    public interface ICartRepository
    {
        // Методы хранилища, которые CartService вызывает при операциях с корзиной.
        bool AddToCart(int productId);
        bool RemoveFromCart(int productId);
        List<CartItem> GetCartItems();
        bool ClearCart();
    }

    public class CartService
    {
        // Сервис передаёт команды репозиторию и хранит текст результата добавления.
        private readonly ICartRepository _repository;

        public CartService(ICartRepository repository)
        {
            _repository = repository;
        }

        public string LastMessage { get; private set; } = string.Empty;

        public bool AddToCart(int productId)
        {
            // Репозиторий изменяет корзину; сервис формирует понятный статус для формы.
            bool added = _repository.AddToCart(productId);
            LastMessage = added ? "Товар добавлен в корзину" : "Не удалось добавить товар в корзину";
            return added;
        }

        // Удаляет товар по идентификатору.
        public bool RemoveFromCart(int productId) => _repository.RemoveFromCart(productId);

        // Возвращает строки корзины для отображения.
        public List<CartItem> GetCartItems() => _repository.GetCartItems();

        // Передаёт репозиторию команду очистить корзину.
        public bool ClearCart() => _repository.ClearCart();
    }
}
