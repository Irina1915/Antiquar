using System.Collections.Generic;
using Antique_shop;
using AntiqueShopLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestAvtoriz
{
    [TestClass]
    public class ProductCatalogFilterTests
    {
        [TestMethod]
        public void Apply_SearchAndFilters_ReturnsMatchingProduct()
        {
            var products = new List<Tovar>
            {
                new Tovar { Name = "Старинные часы", Categoria = "Часы", Era = "XIX век", Prise = 15000, Autor = "Мастер Иванов", Country = "Россия", AuthenMark = "Да" },
                new Tovar { Name = "Фарфоровая ваза", Categoria = "Фарфор", Era = "XX век", Prise = 8000, Autor = "Неизвестен", Country = "Франция", AuthenMark = "Нет" }
            };
            var criteria = new ProductFilterCriteria
            {
                SearchText = "ЧАСЫ",
                Category = "Часы",
                MinimumPrice = 10000,
                MaximumPrice = 20000,
                Era = "XIX век",
                Author = "иванов",
                Country = "Россия",
                Authenticity = true
            };

            var result = ProductCatalogFilter.Apply(products, criteria);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Старинные часы", result[0].Name);
        }

        [TestMethod]
        public void Apply_NoMatchingFilters_ReturnsEmptyList()
        {
            var products = new List<Tovar> { new Tovar { Name = "Ваза", Categoria = "Фарфор", Prise = 5000, AuthenMark = "Нет" } };

            var result = ProductCatalogFilter.Apply(products, new ProductFilterCriteria { Category = "Часы", Authenticity = true });

            Assert.AreEqual(0, result.Count);
        }
    }
}
