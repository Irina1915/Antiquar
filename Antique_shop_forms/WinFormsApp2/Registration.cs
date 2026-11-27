using System;
using System.Windows.Forms;
using System.Collections.Generic;
using AntiqueShopLibrary;
using System.Linq;

namespace WinFormsApp2
{
    public partial class Registration : Form
    {
        private readonly UserService _userService;
        private readonly UserValidator _validator;
        private ErrorProvider errorProvider;

        public Registration(UserService userService)
        {
            // Получает общий сервис учётных записей и готовит проверки полей.
            InitializeComponent();
            _userService = userService;
            _validator = new UserValidator();

            errorProvider = new ErrorProvider();
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            // Снимает показанную ошибку, когда пользователь исправляет поле.
            Login.TextChanged += txtRegLogin_TextChanged;
            textBox2.TextChanged += txtRegPassword_TextChanged;
            textBox3.TextChanged += txtRegFullName_TextChanged;
            textBox4.TextChanged += txtRegEmail_TextChanged;
            textBox5.TextChanged += txtRegPhone_TextChanged;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Считывает форму, проверяет поля и отправляет нового пользователя в сервис.
            // 1. Очищаем старые ошибки
            errorProvider.Clear();

            // 2. Считываем данные
            string login = Login.Text.Trim();
            string password = textBox2.Text;
            string fullName = textBox3.Text.Trim();
            string email = textBox4.Text.Trim();
            // Маска возвращает только введённые цифры, без +7, пробелов и дефисов.
            string phoneDigits = textBox5.Text.Trim();
            string phone = string.Empty;

            var validationErrors = new List<string>();

            // 3. Локальная валидация полей с выводом ошибок рядом с полями

            // Логин
            if (!_validator.ValidateLogin(login))
            {
                errorProvider.SetError(Login, "Логин: 3-50 символов, только латиница и цифры");
                validationErrors.Add("Логин: от 3 до 50 символов, только латинские буквы и цифры без пробелов.");
            }

            // Пароль
            if (!_validator.ValidatePassword(password))
            {
                errorProvider.SetError(textBox2, "Пароль: 6-100 символов, без пробелов");
                validationErrors.Add("Пароль: от 6 до 100 символов, без пробелов.");
            }

            // ФИО
            if (!_validator.ValidateFullName(fullName))
            {
                errorProvider.SetError(textBox3, "ФИО: только буквы, пробелы и дефис");
                validationErrors.Add("ФИО: укажите имя буквами; разрешены пробелы и дефис.");
            }

            // Email (если не пустой)
            if (!string.IsNullOrEmpty(email) && !_validator.ValidateEmail(email))
            {
                errorProvider.SetError(textBox4, "Введите почту вида name@example.ru (до 150 символов).");
                validationErrors.Add("Почта: формат name@example.ru, максимум 150 символов. Поле можно оставить пустым.");
            }

            // Телефон (если не пустой)
            if (!string.IsNullOrEmpty(phoneDigits) &&
                (phoneDigits.Length != 10 || !phoneDigits.All(char.IsDigit)))
            {
                errorProvider.SetError(textBox5, "Введите все 10 цифр номера после +7.");
                validationErrors.Add("Телефон: заполните все 10 цифр после +7 или оставьте поле пустым.");
            }
            else if (phoneDigits.Length == 10)
            {
                phone = $"+7 ({phoneDigits.Substring(0, 3)}) {phoneDigits.Substring(3, 3)}-{phoneDigits.Substring(6, 2)}-{phoneDigits.Substring(8, 2)}";
            }

            if (validationErrors.Count > 0)
            {
                MessageBox.Show(
                    "Исправьте ошибки в форме:" + Environment.NewLine + Environment.NewLine + "• " +
                    string.Join(Environment.NewLine + "• ", validationErrors),
                    "Проверка регистрации",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            // 4. Если локальные проверки прошли, создаем объект и отправляем в сервис
            User newUser = new User();
            newUser.SetLogin(login);
            newUser.SetPassword(password); // Хеширование
            newUser.FullName = fullName;
            newUser.Email = email;
            newUser.Phone = phone;
            newUser.SetRole(Role.Customer);

            // 5. Вызов сервиса (здесь может быть ошибка "Логин занят")
            bool isSuccess;
            try
            {
                isSuccess = _userService.RegisterSelf(newUser);
            }
            catch (Exception)
            {
                // Показывает понятное сообщение при сбое PostgreSQL вместо необработанного исключения.
                MessageBox.Show(
                    "Не удалось сохранить регистрацию. Проверьте данные и подключение к базе данных.",
                    "Ошибка регистрации",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            if (isSuccess)
            {
                MessageBox.Show("Регистрация успешна!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                // Если сервис вернул ошибку (например, логин занят), показываем её
                // Можно показать под логином, так как это самая частая причина
                errorProvider.SetError(Login, _userService.LastMessage);

                // Показывает ошибку сервиса и краткую подсказку, сохраняя значок у поля.
                Login.Focus();
                MessageBox.Show(
                    _userService.LastMessage + Environment.NewLine + Environment.NewLine +
                    GetRegistrationSuggestion(_userService.LastMessage),
                    "Регистрация",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private static string GetRegistrationSuggestion(string message)
        {
            if (message == "Пользователь с таким логином уже существует")
                return "Выберите другой логин.";
            if (message.Contains("логин", StringComparison.OrdinalIgnoreCase))
                return "Проверьте правила логина: от 3 до 50 символов, латинские буквы и цифры.";
            if (message.Contains("парол", StringComparison.OrdinalIgnoreCase))
                return "Пароль должен содержать от 6 до 100 символов и не содержать пробелов.";
            if (message.Contains("почт", StringComparison.OrdinalIgnoreCase) || message.Contains("email", StringComparison.OrdinalIgnoreCase))
                return "Укажите адрес в формате name@example.ru. Поле можно оставить пустым.";
            if (message.Contains("телефон", StringComparison.OrdinalIgnoreCase))
                return "Введите все 10 цифр после +7 или оставьте поле пустым.";

            return "Проверьте введённые данные и доступность подключения к базе данных.";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Закрывает регистрацию без создания учётной записи.
            this.Close();
        }

        // Опционально: убирать ошибку, когда пользователь начинает исправлять поле
        private void txtRegLogin_TextChanged(object sender, EventArgs e)
        {
            // Убирает подсказку об ошибке, пока пользователь исправляет логин.
            errorProvider.SetError(Login, "");
        }
        private void txtRegPassword_TextChanged(object sender, EventArgs e)
        {
            // Убирает подсказку об ошибке пароля при редактировании.
            errorProvider.SetError(textBox2, "");
        }
        private void txtRegFullName_TextChanged(object sender, EventArgs e)
        {
            // Убирает подсказку об ошибке ФИО при редактировании.
            errorProvider.SetError(textBox3, "");
        }
        private void txtRegEmail_TextChanged(object sender, EventArgs e)
        {
            // Убирает подсказку об ошибке почты при редактировании.
            errorProvider.SetError(textBox4, "");
        }
        private void txtRegPhone_TextChanged(object sender, EventArgs e)
        {
            // Убирает подсказку об ошибке телефона при редактировании.
            errorProvider.SetError(textBox5, "");
        }

    }
}
