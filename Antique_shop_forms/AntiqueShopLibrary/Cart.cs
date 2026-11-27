using Antique_shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntiqueShopLibrary
{
    public class Cart
    {
        // Текущий список покупок хранится в памяти до закрытия приложения.
        private List<CartItem> items = new List<CartItem>();

        // Даёт форме доступ к товарам для вывода строк корзины.
        public List<CartItem> Items => items;

        public void AddProduct(Tovar product, int quantity = 1)
        {
            // Повторно добавленный товар увеличивает количество существующей строки.
            // Проверяем, есть ли уже такой товар в корзине
            CartItem existingItem = items.FirstOrDefault(i => i.Product.Name == product.Name);

            if (existingItem != null)
            {
                // Если есть - увеличиваем количество
                existingItem.Quantity += quantity;
            }
            else
            {
                // Если нет - добавляем новый
                items.Add(new CartItem(product, quantity));
            }
        }

        public void RemoveProduct(string productName)
        {
            // Удаляет строки, совпадающие с названием товара.
            items.RemoveAll(i => i.Product.Name == productName);
        }

        public void Clear()
        {
            // Очищает корзину целиком.
            items.Clear();
        }

        public double GetTotalSum()
        {
            // Складывает стоимость всех строк с учётом количества.
            return items.Sum(i => i.TotalPrice);
        }

        public int GetTotalItems()
        {
            // Возвращает общее количество единиц товара.
            return items.Sum(i => i.Quantity);
        }
    }
}
