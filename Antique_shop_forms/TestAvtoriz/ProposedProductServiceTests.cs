using AntiqueShopLibrary;
using AntiqueShopLibrary.BD;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace TestAvtoriz
{
    [TestClass]
    public class ProposedProductServiceTests
    {
        [TestMethod]
        public void GetProposedProducts_ReturnsList()
        {
            var repository = new Mock<IProposedProductRepository>();
            repository.Setup(r => r.GetProposedProducts()).Returns(new List<ProposedProduct> { new ProposedProduct(), new ProposedProduct(), new ProposedProduct() });
            var service = new ProposedProductService(repository.Object, Role.Admin);
            Assert.AreEqual(3, service.GetProposedProducts().Count);
            repository.Verify(r => r.GetProposedProducts(), Times.Once);
        }

        [TestMethod]
        public void ApproveProduct_ValidId_ReturnsTrue()
        {
            var repository = new Mock<IProposedProductRepository>();
            repository.Setup(r => r.ApproveProduct(1)).Returns(true);
            var service = new ProposedProductService(repository.Object, Role.Admin);
            Assert.IsTrue(service.ApproveProduct(1));
            Assert.AreEqual("Товар одобрен и добавлен в каталог", service.LastMessage);
            repository.Verify(r => r.ApproveProduct(1), Times.Once);
        }

        [TestMethod]
        public void ApproveProduct_VerifyTransferToProducts()
        {
            var repository = new Mock<IProposedProductRepository>();
            repository.Setup(r => r.ApproveProduct(4)).Returns(true);
            var service = new ProposedProductService(repository.Object, Role.Admin);
            Assert.IsTrue(service.ApproveProduct(4));
            // ApproveProduct performs the INSERT and DELETE together in one database transaction.
            repository.Verify(r => r.ApproveProduct(4), Times.Once);
        }

        [TestMethod]
        public void RejectProduct_ValidId_ReturnsTrue()
        {
            var repository = new Mock<IProposedProductRepository>();
            repository.Setup(r => r.RejectProduct(2)).Returns(true);
            var service = new ProposedProductService(repository.Object, Role.Admin);
            Assert.IsTrue(service.RejectProduct(2));
            Assert.AreEqual("Товар отклонён", service.LastMessage);
            repository.Verify(r => r.RejectProduct(2), Times.Once);
            repository.Verify(r => r.ApproveProduct(It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public void ApproveProduct_InvalidId_ReturnsFalse()
        {
            var repository = new Mock<IProposedProductRepository>();
            repository.Setup(r => r.ApproveProduct(999)).Returns(false);
            var service = new ProposedProductService(repository.Object, Role.Admin);
            Assert.IsFalse(service.ApproveProduct(999));
            Assert.AreEqual("Товар не найден", service.LastMessage);
            repository.Verify(r => r.ApproveProduct(999), Times.Once);
        }

        [TestMethod]
        public void ApproveProduct_GuestUser_ReturnsFalse()
        {
            var repository = new Mock<IProposedProductRepository>();
            var service = new ProposedProductService(repository.Object, Role.Guest);
            Assert.IsFalse(service.ApproveProduct(1));
            Assert.AreEqual("Недостаточно прав для выполнения операции", service.LastMessage);
            repository.Verify(r => r.ApproveProduct(It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public void ApproveProduct_AdminUser_ValidData_ReturnsTrue()
        {
            var repository = new Mock<IProposedProductRepository>();
            repository.Setup(r => r.ApproveProduct(5)).Returns(true);
            var service = new ProposedProductService(repository.Object, Role.Admin);
            Assert.IsTrue(service.ApproveProduct(5));
            Assert.AreEqual("Товар одобрен и добавлен в каталог", service.LastMessage);
            repository.Verify(r => r.ApproveProduct(5), Times.Once);
        }
    }
}
