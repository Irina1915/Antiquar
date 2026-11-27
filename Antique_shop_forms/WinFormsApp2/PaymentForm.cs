using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WinFormsApp2
{
    // Учебное окно оплаты: принимает реквизиты для демонстрации, но не сохраняет карту.
    public sealed class PaymentForm : Form
    {
        private readonly TextBox _cardNumber = new TextBox();
        private readonly MaskedTextBox _phone = new MaskedTextBox();
        private readonly TextBox _address = new TextBox();
        private readonly Label _error = new Label();

        public PaymentForm()
        {
            Text = "Оплата заказа";
            ClientSize = new Size(470, 355);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.PeachPuff;
            Font = new Font("Segoe UI", 10F);

            AddFieldLabel("Номер карты для учебного подтверждения:", 18);
            _cardNumber.Location = new Point(25, 46);
            _cardNumber.Size = new Size(420, 30);
            _cardNumber.MaxLength = 23;
            Controls.Add(_cardNumber);

            AddFieldLabel("Телефон для связи:", 86);
            _phone.Location = new Point(25, 114);
            _phone.Size = new Size(420, 30);
            _phone.Mask = "+7 (000) 000-00-00";
            _phone.PromptChar = '_';
            Controls.Add(_phone);

            AddFieldLabel("Адрес доставки:", 154);
            _address.Location = new Point(25, 182);
            _address.Size = new Size(420, 30);
            _address.MaxLength = 250;
            Controls.Add(_address);

            _error.Location = new Point(25, 220);
            _error.Size = new Size(420, 42);
            _error.ForeColor = Color.DarkRed;
            Controls.Add(_error);

            var confirm = new Button
            {
                Text = "Подтвердить оплату", Location = new Point(25, 280), Size = new Size(240, 42),
                BackColor = Color.Sienna, ForeColor = Color.PeachPuff, FlatStyle = FlatStyle.Flat
            };
            confirm.Click += ConfirmPayment;
            Controls.Add(confirm);

            var cancel = new Button
            {
                Text = "Отмена", Location = new Point(285, 280), Size = new Size(160, 42),
                BackColor = Color.BurlyWood, ForeColor = Color.SaddleBrown, DialogResult = DialogResult.Cancel
            };
            Controls.Add(cancel);
            AcceptButton = confirm;
            CancelButton = cancel;
        }

        private void AddFieldLabel(string text, int top)
        {
            Controls.Add(new Label
            {
                Text = text, Location = new Point(22, top), Size = new Size(420, 25), ForeColor = Color.SaddleBrown
            });
        }

        private void ConfirmPayment(object sender, System.EventArgs e)
        {
            string digits = new string(_cardNumber.Text.Where(char.IsDigit).ToArray());
            if (digits.Length < 12 || digits.Length > 19)
            {
                _error.Text = "Номер карты должен содержать от 12 до 19 цифр.";
                return;
            }
            if (!_phone.MaskCompleted)
            {
                _error.Text = "Введите телефон полностью: +7 (XXX) XXX-XX-XX.";
                _phone.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(_address.Text))
            {
                _error.Text = "Введите адрес доставки.";
                _address.Focus();
                return;
            }

            _error.Text = string.Empty;
            if (MessageBox.Show(this, "Подтвердить оплату заказа?", "Подтверждение оплаты",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
