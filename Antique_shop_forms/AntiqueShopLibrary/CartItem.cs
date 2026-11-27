using Antique_shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntiqueShopLibrary
{
    public class CartItem
    {
        public Tovar Product { get; set; }
        public int Quantity { get; set; }

        public CartItem(Tovar product, int quantity = 1)
        {
            Product = product;
            Quantity = quantity;
        }

        public double TotalPrice => Product.Prise * Quantity;
    }
}
