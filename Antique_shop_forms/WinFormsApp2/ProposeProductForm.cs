using AntiqueShopLibrary.BD;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using AntiqueShopLibrary;

namespace WinFormsApp2
{
    public partial class ProposeProductForm : Form
    {
        private readonly IProposedProductRepository _repository;
        private readonly int? _submittedByUserId;
        private readonly TextBox _name = new TextBox();
        private readonly ComboBox _category = new ComboBox();
        private readonly ComboBox _era = new ComboBox();
        private readonly TextBox _price = new TextBox();
        private readonly TextBox _author = new TextBox();
        private readonly TextBox _country = new TextBox();
        private readonly TextBox _description = new TextBox();
        private readonly MaskedTextBox _phone = new MaskedTextBox();
        private readonly Label _photoLabel = new Label();
        private string _photoPath = string.Empty;
        public ProposeProductForm(IProposedProductRepository repository) : this(repository, null)
        {
        }

        public ProposeProductForm(IProposedProductRepository repository, int? submittedByUserId)
        {
            // Принимает репозиторий, через который заявка будет сохранена в БД.
            InitializeComponent();
            _repository = repository;
            _submittedByUserId = submittedByUserId;
            BuildForm();
        }

        private void BuildForm()
        {
            // Собирает интерфейс формы в палитре главного окна.
            Text = "Предложить товар";
            ClientSize = new Size(620, 720);
            MinimumSize = MaximumSize = Size;
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.PeachPuff;
            Font = new Font("Segoe UI", 10F);
            Controls.Clear();
            Controls.Add(new Label { Text = "Предложение товара", Font = new Font("Segoe Print", 20F, FontStyle.Bold), ForeColor = Color.Sienna, AutoSize = true, Location = new Point(28, 20) });
            Controls.Add(new Label { Text = "Заполните сведения — заявка будет отправлена администратору на проверку.", ForeColor = Color.SaddleBrown, AutoSize = true, Location = new Point(32, 68) });

            int y = 112;
            AddField("Наименование *", _name, ref y);
            AddDropDown("Категория", _category, new[] { "Живопись", "Часы", "Фарфор", "Мебель", "Монеты", "Украшения", "Посуда", "Предметы интерьера", "Коллекционирование", "Другое" }, ref y);
            AddDropDown("Эпоха", _era, new[] { "Античность", "Средневековье", "XVIII век", "XIX век", "Начало XX века", "Середина XX века", "Вторая половина XX века", "Другое" }, ref y);
            AddField("Предполагаемая цена, руб.", _price, ref y);
            AddField("Автор или производитель", _author, ref y);
            AddField("Страна создания", _country, ref y);
            AddField("Описание *", _description, ref y, true);
            AddPhoneField("Телефон для связи *", _phone, ref y);

            var photoButton = new Button { Text = "Выбрать фото", Location = new Point(205, y + 4), Size = new Size(130, 34), BackColor = Color.Sienna, ForeColor = Color.PeachPuff, FlatStyle = FlatStyle.Flat };
            photoButton.Click += SelectPhoto;
            _photoLabel.SetBounds(346, y + 10, 240, 24);
            _photoLabel.Text = "Фото не выбрано";
            Controls.Add(photoButton);
            Controls.Add(_photoLabel);
            y += 54;

            var status = new Label { ForeColor = Color.DarkRed, AutoSize = false, Location = new Point(30, y), Size = new Size(555, 40) };
            Controls.Add(status);
            var submit = new Button { Text = "Отправить на утверждение", Location = new Point(205, y + 48), Size = new Size(245, 42), BackColor = Color.Sienna, ForeColor = Color.PeachPuff, FlatStyle = FlatStyle.Flat };
            submit.Click += (sender, args) => Submit(status);
            var cancel = new Button { Text = "Отмена", Location = new Point(465, y + 48), Size = new Size(120, 42), BackColor = Color.BurlyWood, ForeColor = Color.SaddleBrown, FlatStyle = FlatStyle.Flat, DialogResult = DialogResult.Cancel };
            Controls.Add(submit);
            Controls.Add(cancel);
            AcceptButton = submit;
            CancelButton = cancel;
        }

        private void AddField(string caption, TextBox input, ref int y, bool multiline = false)
        {
            // Добавляет подпись и поле ввода; описание получает несколько строк.
            Controls.Add(new Label { Text = caption, AutoSize = false, TextAlign = ContentAlignment.MiddleRight, Location = new Point(20, y), Size = new Size(175, multiline ? 58 : 30), ForeColor = Color.SaddleBrown });
            input.Location = new Point(205, y);
            input.Size = new Size(380, multiline ? 58 : 30);
            input.BorderStyle = BorderStyle.FixedSingle;
            input.Multiline = multiline;
            if (multiline) input.ScrollBars = ScrollBars.Vertical;
            Controls.Add(input);
            y += multiline ? 66 : 42;
        }

        private void AddDropDown(string caption, ComboBox input, string[] values, ref int y)
        {
            // Создаёт поле выбора категории или эпохи с готовыми значениями.
            Controls.Add(new Label { Text = caption, AutoSize = false, TextAlign = ContentAlignment.MiddleRight, Location = new Point(20, y), Size = new Size(175, 30), ForeColor = Color.SaddleBrown });
            input.Location = new Point(205, y);
            input.Size = new Size(380, 30);
            input.DropDownStyle = ComboBoxStyle.DropDown;
            input.Items.AddRange(values);
            Controls.Add(input);
            y += 42;
        }

        private void AddPhoneField(string caption, MaskedTextBox input, ref int y)
        {
            // Ограничивает телефон российским форматом и добавляет поле в форму.
            Controls.Add(new Label { Text = caption, AutoSize = false, TextAlign = ContentAlignment.MiddleRight, Location = new Point(20, y), Size = new Size(175, 30), ForeColor = Color.SaddleBrown });
            input.Location = new Point(205, y);
            input.Size = new Size(380, 30);
            input.BorderStyle = BorderStyle.FixedSingle;
            input.Mask = "+7 (000) 000-00-00";
            input.PromptChar = '_';
            input.BeepOnError = true;
            Controls.Add(input);
            y += 42;
        }

        private void SelectPhoto(object sender, EventArgs e)
        {
            // Открывает выбор изображения и запоминает путь к файлу.
            using (var dialog = new OpenFileDialog { Title = "Выберите фотографию товара", Filter = "Изображения|*.jpg;*.jpeg;*.png;*.bmp" })
            {
                if (dialog.ShowDialog(this) != DialogResult.OK) return;
                _photoPath = dialog.FileName;
                _photoLabel.Text = System.IO.Path.GetFileName(_photoPath);
            }
        }

        private void Submit(Label status)
        {
            // Проверяет обязательные данные, формирует модель и отправляет заявку.
            status.Text = string.Empty;
            if (string.IsNullOrWhiteSpace(_name.Text) || string.IsNullOrWhiteSpace(_description.Text))
            {
                status.Text = "Заполните наименование и описание товара.";
                return;
            }
            if (!_phone.MaskCompleted)
            {
                status.Text = "Введите телефон полностью: +7 (XXX) XXX-XX-XX.";
                _phone.Focus();
                return;
            }
            double price = 0;
            if (!string.IsNullOrWhiteSpace(_price.Text) && !double.TryParse(_price.Text.Trim().Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out price))
            {
                status.Text = "Укажите цену числом.";
                return;
            }
            if (price < 0) { status.Text = "Цена не может быть отрицательной."; return; }

            try
            {
                var product = new ProposedProduct { Name = _name.Text.Trim(), Description = _description.Text.Trim(), Price = price,
                    AdditionalInfo = _phone.Text.Trim(), Phone = _phone.Text.Trim(), Category = _category.Text.Trim(), Era = _era.Text.Trim(),
                    Author = _author.Text.Trim(), Country = _country.Text.Trim(), PhotoPath = ReadPhotoAsDataUri(),
                    SubmittedByUserId = _submittedByUserId };
                if (!_repository.AddProposedProduct(product))
                {
                    status.Text = "Не удалось отправить заявку. Проверьте подключение к базе данных.";
                    return;
                }
                MessageBox.Show(this, "Заявка отправлена администратору на утверждение.", "Заявка отправлена", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                status.Text = "Ошибка отправки заявки: " + ex.Message;
            }
        }

        private string ReadPhotoAsDataUri()
        {
            // Сохраняет фото в самой заявке, чтобы оно было доступно любому окну приложения.
            if (string.IsNullOrWhiteSpace(_photoPath) || !System.IO.File.Exists(_photoPath)) return string.Empty;

            string extension = System.IO.Path.GetExtension(_photoPath).ToLowerInvariant();
            string mimeType = extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".bmp" => "image/bmp",
                _ => throw new System.IO.InvalidDataException("Выберите изображение JPG, PNG или BMP.")
            };
            return $"data:{mimeType};base64,{Convert.ToBase64String(System.IO.File.ReadAllBytes(_photoPath))}";
        }
    }
}
