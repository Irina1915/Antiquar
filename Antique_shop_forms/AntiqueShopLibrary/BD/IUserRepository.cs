using AntiqueShopLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AntiqueShopLibrary
{
        /// <summary>
        /// Интерфейс для абстрагирования работы с базой данных.
        /// Позволяет использовать Mock-объекты при тестировании.
        /// </summary>
        public interface IUserRepository
        {
            /// <summary>
            /// Проверяет, занят ли указанный логин.
            /// true — логин свободен; false — уже существует.
            /// </summary>
            bool CheckIfLoginExists(string login);

            /// <summary>
            /// Добавляет нового пользователя в базу данных.
            /// </summary>
            bool AddUser(User user);

            /// <summary>
            /// Находит пользователя по логину для авторизации.
            /// </summary>
            User FindByLogin(string login);

            /// <summary>
            /// Возвращает список всех пользователей для отображения в таблице.
            /// </summary>
            List<User> GetAllUsers();
        }
    }