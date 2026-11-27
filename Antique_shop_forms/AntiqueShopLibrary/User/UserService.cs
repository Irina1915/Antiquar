using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AntiqueShopLibrary
{
    /// <summary>
    /// Класс управления списком пользователей.
    /// Реализует бизнес-логику авторизации и регистрации.
    /// Связывает форму, валидатор и репозиторий.
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

            /// <summary>
            /// Конструктор. Принимает репозиторий для работы с БД.
            /// </summary>
            public UserService(IUserRepository repository)
            {
                _repository = repository;
                _validator = new UserValidator();
                _authService = new AuthorizationService();
            }

            /// <summary>
            /// Выполняет вход пользователя в систему.
            /// Возвращает true при успехе, false при ошибке.
            /// </summary>
            public bool Login(string login, string password)
            {
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

                if (string.IsNullOrEmpty(password))
                {
                    LastMessage = "Введите пароль";
                    return false;
                }

                // При входе сверяем пароль с сохранённым хэшем; правила длины применяются при регистрации.
                User user = _repository.FindByLogin(login);

                if (user == null)
                {
                    LastMessage = "Пользователь с таким логином не найден";
                    return false;
                }

                if (!user.CheckPassword(password))
                {
                    LastMessage = "Неверный пароль";
                    return false;
                }

                CurrentUser = user;
                LastMessage = "Вход выполнен успешно";
                return true;
            }

            /// <summary>
            /// САМОСТОЯТЕЛЬНАЯ регистрация нового пользователя.
            /// Доступна всем. Самостоятельная регистрация создаёт клиента.
            /// </summary>
            public bool RegisterSelf(User user)
            {
                // 1. Валидация данных
                string validationError = _validator.GetFirstValidationError(user);
                if (validationError != null)
                {
                    LastMessage = validationError;
                    return false;
                }

                // 2. Проверка уникальности логина
                if (!IsLoginAvailable(user.Login))
                {
                    LastMessage = "Пользователь с таким логином уже существует";
                    return false;
                }

                // 3. Самостоятельно зарегистрированный пользователь получает роль клиента.
                user.SetRole(Role.Customer);

                // 4. Сохранение в БД
                bool success = _repository.AddUser(user);

                if (success)
                {
                    LastMessage = "Регистрация успешна! Теперь вы можете войти.";
                    return true;
                }
                else
                {
                    LastMessage = "Ошибка при сохранении пользователя";
                    return false;
                }
            }

            /// <summary>
            /// Регистрация пользователя АДМИНИСТРАТОРОМ.
            /// Проверяет права, позволяет назначить любую роль.
            /// </summary>
            public bool RegisterUser(User user)
            {
                if (!_authService.CanAssignRole(CurrentUser))
                {
                    LastMessage = "Недостаточно прав для выполнения операции";
                    return false;
                }

                string validationError = _validator.GetFirstValidationError(user);
                if (validationError != null)
                {
                    LastMessage = validationError;
                    return false;
                }

                if (!IsLoginAvailable(user.Login))
                {
                    LastMessage = "Пользователь с таким логином уже существует";
                    return false;
                }

                bool success = _repository.AddUser(user);

                if (success)
                {
                    LastMessage = "Пользователь успешно зарегистрирован";
                    return true;
                }
                else
                {
                    LastMessage = "Ошибка при сохранении пользователя";
                    return false;
                }
            }

            /// <summary>
            /// Проверяет доступность логина.
            /// </summary>
            public bool IsLoginAvailable(string login)
            {
                return _repository.CheckIfLoginExists(login);
            }

            /// <summary>
            /// Возвращает список пользователей для таблицы (только для админа).
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
            /// Проверяет, является ли текущий пользователь администратором.
            /// </summary>
            public bool IsAdmin()
            {
                return _authService.IsAdmin(CurrentUser);
            }

            /// <summary>
            /// Завершает сессию пользователя.
            /// </summary>
            public void Logout()
            {
                CurrentUser = null;
            }

            /// <summary>
            /// Устанавливает текущего пользователя (для гостевого входа).
            /// </summary>
            public void SetCurrentUser(User user)
            {
                CurrentUser = user;
            }
        }
    }
