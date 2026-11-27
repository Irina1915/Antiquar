using System.Text;
using System.Security.Cryptography;

namespace Antique_shop_library
{
    /// <summary>
    /// Перечисление ролей пользователей системы.
    /// </summary>
    public enum Role
    {
        Admin,
        Guest,
        Customer
    }

    /// <summary>
    /// Модель пользователя системы.
    /// В качестве уникального идентификатора выступает логин.
    /// </summary>
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        /// <summary>
        /// Устанавливает логин пользователя.
        /// </summary>
        public void SetLogin(string login)
        {
            Login = login;
        }

        /// <summary>
        /// Устанавливает пароль пользователя (сохраняется в виде хэша SHA256).
        /// </summary>
        public void SetPassword(string password)
        {
            Password = User.HashPassword(password);
        }

        /// <summary>
        /// Назначает роль пользователя.
        /// </summary>
        public void SetRole(Role role)
        {
            Role = role;
        }

        /// <summary>
        /// Устанавливает контактные данные пользователя.
        /// </summary>
        public void SetContactInfo(string email, string phone)
        {
            Email = email;
            Phone = phone;
        }

        /// <summary>
        /// Возвращает строковое представление данных пользователя для отображения в таблице.
        /// </summary>
        public string GetEmployeeInfo()
        {
            return Login + " | " +
                   FullName + " | " +
                   User.RoleToString(Role) + " | " +
                   Email + " | " +
                   Phone;
        }

        /// <summary>
        /// Сравнивает переданный пароль с сохранённым хэшем.
        /// </summary>
        public bool CheckPassword(string password)
        {
            if (Password == null || password == null)
            {
                return false;
            }

            string enteredHash = User.HashPassword(password);

            if (Password == enteredHash)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Преобразует строку из БД в значение перечисления Role.
        /// </summary>
        public static Role ParseRole(string role)
        {
            Role result;

            if (role == null)
            {
                result = Role.Guest;
            }
            else
            {
                string lowerRole = role.Trim().ToLower();

                if (lowerRole == "admin")
                {
                    result = Role.Admin;
                }
                else if (lowerRole == "employee")
                {
                    result = Role.Customer;
                }
                else if (lowerRole == "guest")
                {
                    result = Role.Guest;
                }
                else if (lowerRole == "customer")
                {
                    result = Role.Customer;
                }
                else
                {
                    result = Role.Guest;
                }
            }

            return result;
        }

        /// <summary>
        /// Преобразует значение перечисления Role в строку для сохранения в БД.
        /// </summary>
        public static string RoleToString(Role role)
        {
            string result;

            if (role == Role.Admin)
            {
                result = "Admin";
            }
            else if (role == Role.Customer)
            {
                result = "Customer";
            }
            else
            {
                result = "Guest";
            }

            return result;
        }

        /// <summary>
        /// Возвращает читаемое название роли на русском языке.
        /// </summary>
        public static string RoleToDisplayString(Role role)
        {
            string result;

            if (role == Role.Admin)
            {
                result = "Администратор";
            }
            else if (role == Role.Guest)
            {
                result = "Гость";
            }
            else
            {
                result = "Клиент";
            }

            return result;
        }

        /// <summary>
        /// Создаёт стойкий хэш пароля с помощью SHA256 (встроен в .NET).
        /// </summary>
        public static string HashPassword(string password)
        {
            string result;

            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha.ComputeHash(bytes);
                result = Convert.ToBase64String(hash);
            }

            return result;
        }
    }
}
