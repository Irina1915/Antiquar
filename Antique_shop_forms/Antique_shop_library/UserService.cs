using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Antique_shop_library
{
    /// <summary>
    /// Класс управления списком пользователей.
    /// Реализует бизнес-логику регистрации и связывает форму, валидатор и репозиторий.
    /// </summary>
    public class UserService
    {
        private readonly IUserRepository _repository;
        private readonly UserValidator _validator;
        private readonly AuthorizationService _authService;

        /// <summary>
        /// Текущий авторизованный пользователь.
        /// </summary>
        public User CurrentUser { get; private set; }

        /// <summary>
        /// Последнее сообщение об ошибке или успехе.
        /// </summary>
        public string LastMessage { get; private set; }

        public UserService(IUserRepository repository)
        {
            _repository = repository;
            _validator = new UserValidator();
            _authService = new AuthorizationService();
        }

        /// <summary>
        /// Выполняет вход пользователя в систему.
        /// Возвращает true при успехе, false при ошибке.
        /// Сообщение об ошибке/успехе сохраняется в LastMessage.
        /// </summary>
        public bool Login(string login, string password)
        {
            // Валидация логина
            if (string.IsNullOrWhiteSpace(login))
            {
                LastMessage = "Введите логин";
                return false;
            }

            if (login.Length < 3 || login.Length > 50)
            {
                LastMessage = "Логин должен содержать от 3 до 50 символов";
                return false;
            }

            // Валидация пароля
            if (string.IsNullOrEmpty(password))
            {
                LastMessage = "Введите пароль";
                return false;
            }

            if (password.Length < 6 || password.Length > 100)
            {
                LastMessage = "Пароль должен содержать от 6 до 100 символов";
                return false;
            }

            // Поиск пользователя в БД
            User user = _repository.FindByLogin(login);

            if (user == null)
            {
                LastMessage = "Пользователь с таким логином не найден";
                return false;
            }

            // Проверка пароля
            if (!user.CheckPassword(password))
            {
                LastMessage = "Неверный пароль";
                return false;
            }

            // Успешный вход
            CurrentUser = user;
            LastMessage = "Вход выполнен успешно";
            return true;
        }

        /// <summary>
        /// Выполняет регистрацию нового пользователя.
        /// Проверяет данные, уникальность логина, права администратора, хэширует пароль.
        /// Возвращает true при успехе, false при ошибке.
        /// Сообщение сохраняется в LastMessage.
        /// </summary>
        public bool RegisterUser(User user)
        {
            // 1. Проверка прав администратора
            if (!_authService.CanAssignRole(CurrentUser))
            {
                LastMessage = "Недостаточно прав для выполнения операции";
                return false;
            }

            // 2. Комплексная валидация данных
            string validationError = _validator.GetFirstValidationError(user);

            if (validationError != null)
            {
                LastMessage = validationError;
                return false;
            }

            // 3. Проверка уникальности логина
            if (!IsLoginAvailable(user.Login))
            {
                LastMessage = "Пользователь с таким логином уже существует";
                return false;
            }

            // 4. Сохранение пользователя в БД (пароль уже захэширован в SetPassword)
            bool success = _repository.AddUser(user);

            if (success)
            {
                LastMessage = "Регистрация успешна";
                return true;
            }
            else
            {
                LastMessage = "Ошибка при сохранении пользователя";
                return false;
            }
        }

        /// <summary>
        /// Проверяет доступность логина (свободен ли он).
        /// </summary>
        public bool IsLoginAvailable(string login)
        {
            return _repository.CheckIfLoginExists(login);
        }

        /// <summary>
        /// Возвращает список зарегистрированных пользователей для отображения в таблице.
        /// </summary>
        public List<User> GetUsers()
        {
            if (!_authService.IsAdmin(CurrentUser))
            {
                return new List<User>();
            }

            return _repository.GetAllUsers();
        }

        /// <summary>
        /// Завершает сессию пользователя.
        /// </summary>
        public void Logout()
        {
            CurrentUser = null;
        }

        /// <summary>
        /// Проверяет, является ли текущий пользователь администратором.
        /// </summary>
        public bool IsAdmin()
        {
            return _authService.IsAdmin(CurrentUser);
        }
    }
}