using Antique_shop_library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Antique_shop_library
{
    /// <summary>
    /// Интерфейс для абстрагирования работы с базой данных.
    /// Позволяет использовать Mock-объекты при тестировании.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Проверка существования логина в БД.
        /// </summary>
        /// <param name="login">Логин для проверки.</param>
        /// <returns>
        /// true — логин свободен (не занят);
        /// false — пользователь с таким логином уже существует.
        /// </returns>
        bool CheckIfLoginExists(string login);

        /// <summary>
        /// Добавление нового пользователя в базу данных.
        /// </summary>
        /// <param name="user">Объект пользователя с данными для сохранения.</param>
        /// <returns>
        /// true — пользователь успешно сохранён;
        /// false — произошла ошибка записи.
        /// </returns>
        bool AddUser(User user);

        /// <summary>
        /// Поиск пользователя по логину для последующей авторизации.
        /// </summary>
        /// <param name="login">Логин пользователя.</param>
        /// <returns>Объект User или null, если пользователь не найден.</returns>
        User FindByLogin(string login);

        /// <summary>
        /// Получение списка всех зарегистрированных пользователей
        /// для отображения в таблице списка пользователей.
        /// </summary>
        /// <returns>Список объектов User.</returns>
        List<User> GetAllUsers();
    }
}