using AntiqueShopLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace TestAvtoriz
{
    [TestClass]
    public class Korzina
    {
        [TestMethod]
        public void TestKorzina()
        {
            /// <summary>
            /// Тест успешной авторизации.
            /// </summary>
                // Arrange (Подготовка)
                var mockRepository = new Mock<IUserRepository>();

                // Создаём тестового пользователя
                var testUser = new User();
                testUser.SetLogin("admin");
                testUser.SetPassword("admin123"); // хэшируется внутри
                testUser.SetRole(Role.Admin);
                testUser.FullName = "Иванов Иван Иванович";

                // Настраиваем мок: при вызове FindByLogin("admin") вернуть testUser
                mockRepository.Setup(repo => repo.FindByLogin("admin"))
                              .Returns(testUser);

                var userService = new UserService(mockRepository.Object);

                // Act (Действие)
                bool result = userService.Login("admin", "admin123");

                // Assert (Проверка)
                Assert.IsTrue(result);
                Assert.AreEqual("Вход выполнен успешно", userService.LastMessage);
                Assert.IsNotNull(userService.CurrentUser);
                Assert.AreEqual("admin", userService.CurrentUser.Login);
            

        }

        /// <summary>
        /// Тест авторизации с неправильным паролем.
        /// </summary>
        [TestMethod]
        public void Login_WrongKorzina()
        {
            // Arrange
            var mockRepository = new Mock<IUserRepository>();

            var testUser = new User();
            testUser.SetLogin("admin");
            testUser.SetPassword("admin123");
            testUser.SetRole(Role.Admin);

            mockRepository.Setup(repo => repo.FindByLogin("admin"))
                          .Returns(testUser);

            var userService = new UserService(mockRepository.Object);

            // Act
            bool result = userService.Login("admin", "wrongpassword");

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual("Неверный пароль", userService.LastMessage);
            Assert.IsNull(userService.CurrentUser);
        }

        /// <summary>
        /// Тест авторизации с неправильным логином.
        /// </summary>
        [TestMethod]
        public void Korzna()
        {
            // Arrange
            var mockRepository = new Mock<IUserRepository>();

            // Настраиваем мок: при вызове FindByLogin("unknown") вернуть null
            mockRepository.Setup(repo => repo.FindByLogin("unknown"))
                          .Returns((User)null);

            var userService = new UserService(mockRepository.Object);

            // Act
            bool result = userService.Login("unknown", "anypassword");

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual("Пользователь с таким логином не найден", userService.LastMessage);
            Assert.IsNull(userService.CurrentUser);
        }
    }
}
