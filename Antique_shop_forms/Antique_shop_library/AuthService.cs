using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antique_shop_library
{
    /// <summary>
    /// Сервис для выполнения операций авторизации пользователей.
    /// </summary>
    public class AuthService
    {
        private readonly IUserRepository _repository;

        /// <summary>
        /// Текущий авторизованный пользователь.
        /// </summary>
        public User CurrentUser { get; private set; }

        /// <summary>
        /// Конструктор сервиса. Принимает репозиторий для работы с БД.
        /// </summary>
        public AuthService(IUserRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Выполняет вход пользователя в систему.
        /// Возвращает null при успехе или сообщение об ошибке.
        /// </summary>
        public string Login(string login, string password)
        {
            // --- Валидация логина ---
            if (string.IsNullOrWhiteSpace(login))
            {
                return "Введите логин";
            }

            if (login.Length < 3 || login.Length > 50)
            {
                return "Логин должен содержать от 3 до 50 символов";
            }

            // --- Валидация пароля ---
            if (string.IsNullOrEmpty(password))
            {
                return "Введите пароль";
            }

            if (password.Length < 6 || password.Length > 100)
            {
                return "Пароль должен содержать от 6 до 100 символов";
            }

            // --- Поиск пользователя в БД ---
            User user = _repository.FindByLogin(login);

            if (user == null)
            {
                return "Пользователь с таким логином не найден";
            }

            // --- Проверка пароля ---
            if (!user.CheckPassword(password))
            {
                return "Неверный пароль";
            }

            // --- Успешный вход ---
            CurrentUser = user;
            return null;
        }

        /// <summary>
        /// Завершает сессию пользователя.
        /// </summary>
        public void Logout()
        {
            CurrentUser = null;
        }
    }
}