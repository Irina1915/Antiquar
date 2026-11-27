using AntiqueShopLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace TestAvtoriz
{
    [TestClass]
    public class Registrachia
    {
            /// <summary>
            /// Тест успешной самостоятельной регистрации.
            /// Роль должна быть автоматически установлена как Customer.
            /// </summary>
            [TestMethod]
            public void RegisterSelf_Success_ReturnsTrueAndSetsCustomerRole()
            {
                // Arrange (Подготовка)
                var mockRepository = new Mock<IUserRepository>();

                // Логин свободен
                mockRepository.Setup(repo => repo.CheckIfLoginExists("newuser"))
                              .Returns(true);

                // Имитация успешного добавления в БД
                mockRepository.Setup(repo => repo.AddUser(It.IsAny<User>()))
                              .Returns(true);

                var userService = new UserService(mockRepository.Object);

                var newUser = new User();
                newUser.SetLogin("newuser");
                newUser.SetPassword("pass123");
                newUser.FullName = "Новый Пользователь";
                // Роль явно не задаем, это делает сервис

                // Act (Действие)
                bool result = userService.RegisterSelf(newUser);

                // Assert (Проверка)
                Assert.IsTrue(result);
                Assert.AreEqual(Role.Customer, newUser.Role);
                Assert.AreEqual("Регистрация успешна! Теперь вы можете войти.", userService.LastMessage);

                // Проверяем, что метод AddUser был вызван ровно 1 раз
                mockRepository.Verify(repo => repo.AddUser(It.IsAny<User>()), Times.Once);
            }

            /// <summary>
            /// Тест самостоятельной регистрации с уже существующим логином.
            /// </summary>
            [TestMethod]
            public void RegisterSelf_LoginExists_ReturnsFalse()
            {
                // Arrange
                var mockRepository = new Mock<IUserRepository>();

                // Логин уже занят
                mockRepository.Setup(repo => repo.CheckIfLoginExists("admin"))
                              .Returns(false);

                var userService = new UserService(mockRepository.Object);

                var existingUser = new User();
                existingUser.SetLogin("admin");
                existingUser.SetPassword("pass123");
                existingUser.FullName = "Админ";

                // Act
                bool result = userService.RegisterSelf(existingUser);

                // Assert
                Assert.IsFalse(result);
                Assert.AreEqual("Пользователь с таким логином уже существует", userService.LastMessage);

                // Проверяем, что добавление в БД НЕ происходило
                mockRepository.Verify(repo => repo.AddUser(It.IsAny<User>()), Times.Never);
            }

            /// <summary>
            /// Тест успешной регистрации пользователя АДМИНИСТРАТОРОМ.
            /// Администратор регистрирует пользователя с ролью клиента.
            /// </summary>
            [TestMethod]
            public void RegisterUser_ByAdmin_Success_ReturnsTrue()
            {
                // Arrange
                var mockRepository = new Mock<IUserRepository>();

                mockRepository.Setup(repo => repo.CheckIfLoginExists("customer1"))
                              .Returns(true);

                mockRepository.Setup(repo => repo.AddUser(It.IsAny<User>()))
                              .Returns(true);

                var userService = new UserService(mockRepository.Object);

                // Имитируем, что в систему вошел Администратор
                var admin = new User();
                admin.SetRole(Role.Admin);
                userService.SetCurrentUser(admin);

                var newCustomer = new User();
                newCustomer.SetLogin("customer1");
                newCustomer.SetPassword("work123");
                newCustomer.FullName = "Клиент Один";
                newCustomer.SetRole(Role.Customer);

                // Act
                bool result = userService.RegisterUser(newCustomer);

                // Assert
                Assert.IsTrue(result);
                Assert.AreEqual("Пользователь успешно зарегистрирован", userService.LastMessage);
                mockRepository.Verify(repo => repo.AddUser(It.IsAny<User>()), Times.Once);
            }

            /// <summary>
            /// Тест попытки регистрации пользователя ГОСТЕМ (без прав админа).
            /// Должно быть отказано в доступе.
            /// </summary>
            [TestMethod]
            public void RegisterUser_ByGuest_ReturnsFalseAndNoDbCall()
            {
                // Arrange
                var mockRepository = new Mock<IUserRepository>();
                var userService = new UserService(mockRepository.Object);

                // Имитируем, что в систему вошел Гость
                var guest = new User();
                guest.SetRole(Role.Guest);
                userService.SetCurrentUser(guest);

                var newUser = new User();
                newUser.SetLogin("hacker");
                newUser.SetPassword("hack123");
                newUser.FullName = "Хакер";
                newUser.SetRole(Role.Admin); // Пытается назначить себе админа

                // Act
                bool result = userService.RegisterUser(newUser);

                // Assert
                Assert.IsFalse(result);
                Assert.AreEqual("Недостаточно прав для выполнения операции", userService.LastMessage);

                // Проверяем, что метод AddUser НЕ вызывался, так как проверка прав упала раньше
                mockRepository.Verify(repo => repo.AddUser(It.IsAny<User>()), Times.Never);
            }
    }
}
 

