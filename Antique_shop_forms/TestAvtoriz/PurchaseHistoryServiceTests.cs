using System.Collections.Generic;
using Antique_shop;
using AntiqueShopLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace TestAvtoriz
{
    [TestClass]
    public class PurchaseHistoryServiceTests
    {
        [TestMethod]
        public void GetMyPurchases_PassesUserIdToRepository()
        {
            var expected = new List<PurchaseRecord>
            {
                new PurchaseRecord { BuyerId = 12, BuyerLogin = "client12", BuyerName = "Клиент", Product = new Tovar { Name = "Ваза" } }
            };
            var repository = new Mock<IPurchaseRepository>();
            repository.Setup(x => x.GetPurchasesForUser(12)).Returns(expected);
            var service = new PurchaseHistoryService(repository.Object);

            var result = service.GetMyPurchases(12);

            Assert.AreEqual(1, result.Count);
            Assert.AreSame(expected[0], result[0]);
            repository.Verify(x => x.GetPurchasesForUser(12), Times.Once);
        }

        [TestMethod]
        public void GetArchive_GuestIsDenied_DoesNotCallRepository()
        {
            var repository = new Mock<IPurchaseRepository>();
            var service = new PurchaseHistoryService(repository.Object);

            var result = service.GetArchive(Role.Guest);

            Assert.AreEqual(0, result.Count);
            repository.Verify(x => x.GetPurchaseArchive(), Times.Never);
        }

        [TestMethod]
        public void CompletePurchase_ValidBuyerAndItems_CallsRepositoryOnce()
        {
            var repository = new Mock<IPurchaseRepository>();
            repository.Setup(x => x.CompletePurchase(12, "client12", "Клиент",
                It.Is<IEnumerable<CartItem>>(value => value != null))).Returns(true);
            var service = new PurchaseHistoryService(repository.Object);
            var buyer = new User { Id = 12, Login = "client12", FullName = "Клиент" };
            var items = new List<CartItem> { new CartItem(new Tovar { Id = 7, Name = "Ваза" }, 1) };

            bool result = service.CompletePurchase(buyer, items);

            Assert.IsTrue(result);
            repository.Verify(x => x.CompletePurchase(12, "client12", "Клиент",
                It.Is<IEnumerable<CartItem>>(value => value != null)), Times.Once);
        }
    }
}
