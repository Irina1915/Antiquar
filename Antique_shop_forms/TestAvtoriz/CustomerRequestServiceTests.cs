using System.Collections.Generic;
using AntiqueShopLibrary;
using AntiqueShopLibrary.BD;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace TestAvtoriz
{
    [TestClass]
    public class CustomerRequestServiceTests
    {
        [TestMethod]
        public void GetMyRequests_UsesCurrentUserId_ReturnsOnlyRepositoryResult()
        {
            var expected = new List<ProposedProduct>
            {
                new ProposedProduct { Id = 4, Name = "Часы", Status = "review" }
            };
            var repository = new Mock<ICustomerRequestRepository>();
            repository.Setup(x => x.GetRequestsForUser(23)).Returns(expected);
            var service = new CustomerRequestService(repository.Object);

            var result = service.GetMyRequests(23);

            Assert.AreEqual(1, result.Count);
            Assert.AreSame(expected[0], result[0]);
            repository.Verify(x => x.GetRequestsForUser(23), Times.Once);
        }

        [TestMethod]
        public void DeleteMyRequest_ValidRequest_DeletesOnlyForThatUser()
        {
            var repository = new Mock<ICustomerRequestRepository>();
            repository.Setup(x => x.DeleteRequestForUser(4, 23)).Returns(true);
            var service = new CustomerRequestService(repository.Object);

            bool result = service.DeleteMyRequest(4, 23);

            Assert.IsTrue(result);
            repository.Verify(x => x.DeleteRequestForUser(4, 23), Times.Once);
        }

        [TestMethod]
        public void GetMyRequests_InvalidUserId_ReturnsEmptyWithoutRepositoryCall()
        {
            var repository = new Mock<ICustomerRequestRepository>();
            var service = new CustomerRequestService(repository.Object);

            var result = service.GetMyRequests(0);

            Assert.AreEqual(0, result.Count);
            repository.Verify(x => x.GetRequestsForUser(It.IsAny<int>()), Times.Never);
        }
    }
}
