using System;
using System.Windows.Forms;
using System.Collections.Generic;
using AntiqueShopLibrary;

namespace WinFormsApp2
{
    public partial class Authorization : Form
    {
        private readonly UserService _userService;
        private readonly DBConnection _dbConnection;
        // Компонент для показа ошибок рядом с контролами
        private ErrorProvider errorProvider;

        public Authorization()
        {
            // Настраивает подсказки ошибок и создаёт сервис входа с подключением к БД.
            InitializeComponent();

            // Инициализация ErrorProvider
            errorProvider = new ErrorProvider();
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink; // Не мигать, просто показывать

            string connString = DatabaseConfig.ConnectionString;
            _dbConnection = new DBConnection(connString);
            _userService = new UserService(_dbConnection);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Проверяет заполнение полей, вызывает вход и открывает главное окно.
            // Очищаем предыдущие ошибки
            errorProvider.Clear();

            string login = txtLogin.Text.Trim();
            string password = textBox1.Text;

            var validationErrors = new List<string>();

            // Простая проверка на пустоту перед отправкой в сервис
            if (string.IsNullOrWhiteSpace(login))
            {
                errorProvider.SetError(txtLogin, "Введите логин");
                validationErrors.Add("Логин: введите свой логин. Проверьте, что поле не пустое.");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                errorProvider.SetError(textBox1, "Введите пароль");
                validationErrors.Add("Пароль: введите пароль. Проверьте, что поле не пустое.");
            }

            if (validationErrors.Count > 0)
            {
                MessageBox.Show(
                    "Проверьте данные для входа:" + Environment.NewLine + Environment.NewLine + "• " +
                    string.Join(Environment.NewLine + "• ", validationErrors),
                    "Ошибка авторизации",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            // Пытаемся войти через сервис
            bool isSuccess = _userService.Login(login, password);

            if (isSuccess)
            {
                MainForm mainForm = new MainForm(_userService);
                mainForm.Show();
                this.Hide();
            }
            else
            {
                // Показывает ошибку у соответствующего поля, чтобы не путать пароль с логином.
                bool passwordIsWrong = string.Equals(
                    _userService.LastMessage,
                    "Неверный пароль",
                    StringComparison.Ordinal);
                Control errorControl = passwordIsWrong ? textBox1 : txtLogin;
                errorProvider.SetError(errorControl, _userService.LastMessage);
                if (passwordIsWrong)
                    textBox1.Focus();

                // Добавляет подсказку, которая помогает исправить ошибку входа.
                string suggestion = GetLoginSuggestion(_userService.LastMessage);
                string message = string.IsNullOrWhiteSpace(suggestion)
                    ? _userService.LastMessage
                    : _userService.LastMessage + Environment.NewLine + Environment.NewLine + "Подсказка: " + suggestion;
                MessageBox.Show(
                    message,
                    "Ошибка авторизации",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private static string GetLoginSuggestion(string message)
        {
            if (message == "Неверный пароль")
                //return "Проверьте раскладку клавиатуры и Caps Lock, затем введите пароль ещё раз.";
            if (message == "Пользователь с таким логином не найден")
                return "Проверьте написание логина или зарегистрируйтесь.";
            if (message.Contains("Логин"))
                return string.Empty;
            if (message.Contains("Пароль"))
                return string.Empty;

            return "Проверьте введённые данные и повторите попытку.";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Создаёт гостевую сессию без ввода логина и пароля.
            // Использует отдельную учётную запись гостя для сохранения его корзины в БД.
            User guest = _dbConnection.FindByLogin("guest") ?? new User
            {
                Login = "Гость", FullName = "Гость", Role = Role.Guest
            };

            _userService.SetCurrentUser(guest);

            MainForm mainForm = new MainForm(_userService);
            mainForm.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Открывает регистрацию и возвращает окно входа после её закрытия.
            Registration regForm = new Registration(_userService);
            regForm.FormClosed += (s, args) =>
            {
                this.Show();
                errorProvider.Clear(); // Очистить ошибки при возврате
            };

            this.Hide();
            regForm.Show();
        }

    }
}
