using Antique_shop;
using AntiqueShopLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace TestAvtoriz
{
    [TestClass]
    public class CartServiceTests
    {
        [TestMethod]
        public void AddToCart_ValidProduct_ReturnsTrue()
        {
            var repository = new Mock<ICartRepository>();
            repository.Setup(r => r.AddToCart(7)).Returns(true);
            var service = new CartService(repository.Object);

            bool result = service.AddToCart(7);

            Assert.IsTrue(result);
            Assert.AreEqual("Товар добавлен в корзину", service.LastMessage);
            repository.Verify(r => r.AddToCart(7), Times.Once);
        }

        [TestMethod]
        public void AddToCart_ProductAlreadyInCart_IncreasesQuantity()
        {
            var item = new CartItem(new Tovar { Name = "Ваза", Prise = 100 }, 1);
            var repository = new Mock<ICartRepository>();
            repository.Setup(r => r.AddToCart(7)).Callback(() => item.Quantity++).Returns(true);
            var service = new CartService(repository.Object);

            Assert.IsTrue(service.AddToCart(7));
            Assert.AreEqual(2, item.Quantity);
            repository.Verify(r => r.AddToCart(7), Times.Once);
        }

        [TestMethod]
        public void RemoveFromCart_ValidProduct_ReturnsTrue()
        {
            var repository = new Mock<ICartRepository>();
            repository.Setup(r => r.RemoveFromCart(7)).Returns(true);
            var service = new CartService(repository.Object);

            Assert.IsTrue(service.RemoveFromCart(7));
            repository.Verify(r => r.RemoveFromCart(7), Times.Once);
        }

        [TestMethod]
        public void GetCartItems_ReturnsList()
        {
            var expected = new List<CartItem>
            {
                new CartItem(new Tovar { Name = "Ваза", Prise = 100 }),
                new CartItem(new Tovar { Name = "Часы", Prise = 250 })
            };
            var repository = new Mock<ICartRepository>();
            repository.Setup(r => r.GetCartItems()).Returns(expected);
            var service = new CartService(repository.Object);

            var actual = service.GetCartItems();

            Assert.AreEqual(2, actual.Count);
            CollectionAssert.AreEqual(expected.Select(item => item.Product.Name).ToList(), actual.Select(item => item.Product.Name).ToList());
        }

        [TestMethod]
        public void ClearCart_ReturnsTrue()
        {
            var items = new List<CartItem> { new CartItem(new Tovar { Name = "Ваза", Prise = 100 }) };
            var repository = new Mock<ICartRepository>();
            repository.Setup(r => r.ClearCart()).Callback(() => items.Clear()).Returns(true);
            var service = new CartService(repository.Object);

            Assert.IsTrue(service.ClearCart());
            Assert.AreEqual(0, items.Count);
            repository.Verify(r => r.ClearCart(), Times.Once);
        }
    }
}
