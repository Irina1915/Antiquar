using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Antique_shop_library
{
    /// <summary>
    /// Класс для проверки корректности данных пользователя перед сохранением.
    /// </summary>
    public class UserValidator
    {
        /// <summary>
        /// Проверяет логин: длина от 3 до 50 символов, только латиница и цифры, без пробелов.
        /// </summary>
        public bool ValidateLogin(string login)
        {
            if (string.IsNullOrEmpty(login))
            {
                return false;
            }

            if (login.Length < 3 || login.Length > 50)
            {
                return false;
            }

            // Только латинские буквы и цифры
            Regex regex = new Regex("^[a-zA-Z0-9]+$");

            if (regex.IsMatch(login))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Проверяет пароль: длина от 6 до 100 символов, без пробелов.
        /// </summary>
        public bool ValidatePassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return false;
            }

            if (password.Length < 6 || password.Length > 100)
            {
                return false;
            }

            if (password.Contains(" "))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Проверяет ФИО: не пустое, только буквы, пробелы и дефис.
        /// </summary>
        public bool ValidateFullName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
            {
                return false;
            }

            // Буквы (в том числе кириллица), пробелы и дефис
            Regex regex = new Regex("^[a-zA-Zа-яА-ЯёЁ\\s\\-]+$");

            if (regex.IsMatch(fullName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Проверяет email: формат имя@домен.зона.
        /// </summary>
        public bool ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return true; // Email необязателен
            }

            Regex regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

            if (regex.IsMatch(email))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Проверяет телефон: маска +7 (XXX) XXX-XX-XX.
        /// </summary>
        public bool ValidatePhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
            {
                return true; // Телефон необязателен
            }

            Regex regex = new Regex(@"^\+7 \(\d{3}\) \d{3}-\d{2}-\d{2}$");

            if (regex.IsMatch(phone))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Проверяет, что выбрана одна из предусмотренных системой ролей.
        /// </summary>
        public bool ValidateRole(Role role)
        {
            if (role == Role.Admin || role == Role.Guest || role == Role.Customer)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Выполняет комплексную проверку всех полей пользователя.
        /// </summary>
        public bool ValidateAll(User user)
        {
            if (!ValidateLogin(user.Login))
            {
                return false;
            }

            if (!ValidatePassword(user.Password))
            {
                return false;
            }

            if (!ValidateFullName(user.FullName))
            {
                return false;
            }

            if (!ValidateRole(user.Role))
            {
                return false;
            }

            if (!ValidateEmail(user.Email))
            {
                return false;
            }

            if (!ValidatePhone(user.Phone))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Возвращает сообщение об ошибке для первого невалидного поля.
        /// Если все поля корректны — возвращает null.
        /// </summary>
        public string GetFirstValidationError(User user)
        {
            if (string.IsNullOrEmpty(user.Login))
            {
                return "Введите логин";
            }

            if (!ValidateLogin(user.Login))
            {
                return "Логин должен содержать от 3 до 50 символов";
            }

            if (string.IsNullOrEmpty(user.Password))
            {
                return "Введите пароль";
            }

            if (!ValidatePassword(user.Password))
            {
                return "Пароль должен содержать от 6 до 100 символов";
            }

            if (string.IsNullOrWhiteSpace(user.FullName))
            {
                return "Введите ФИО пользователя";
            }

            if (!ValidateFullName(user.FullName))
            {
                return "ФИО должно содержать только буквы, пробелы и дефис";
            }

            if (!ValidateRole(user.Role))
            {
                return "Выберите роль пользователя";
            }

            if (!ValidateEmail(user.Email))
            {
                return "Укажите корректный адрес электронной почты";
            }

            if (!ValidatePhone(user.Phone))
            {
                return "Телефон должен соответствовать маске +7 (XXX) XXX-XX-XX";
            }

            return null;
        }
    }
}
