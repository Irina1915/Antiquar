namespace WinFormsApp2
{
    partial class ProposeProductForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            txtName = new TextBox();
            label2 = new Label();
            cmbCategory = new ComboBox();
            label3 = new Label();
            label4 = new Label();
            Автор = new Label();
            comboBox1 = new ComboBox();
            label6 = new Label();
            txtPrice = new TextBox();
            txtAuthor = new TextBox();
            txtCountry = new TextBox();
            label5 = new Label();
            textBox1 = new TextBox();
            label7 = new Label();
            txtPhone = new TextBox();
            label8 = new Label();
            btnSelectPhoto = new Button();
            lblPhotoPath = new Label();
            lblError = new Label();
            btnSubmit = new Button();
            btnCancel = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(26, 39);
            label1.Name = "label1";
            label1.Size = new Size(90, 15);
            label1.TabIndex = 0;
            label1.Text = "Наименование";
            // 
            // txtName
            // 
            txtName.Location = new Point(137, 41);
            txtName.Name = "txtName";
            txtName.Size = new Size(100, 23);
            txtName.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(23, 97);
            label2.Name = "label2";
            label2.Size = new Size(63, 15);
            label2.TabIndex = 2;
            label2.Text = "Категория";
            // 
            // cmbCategory
            // 
            cmbCategory.FormattingEnabled = true;
            cmbCategory.Location = new Point(137, 94);
            cmbCategory.Name = "cmbCategory";
            cmbCategory.Size = new Size(121, 23);
            cmbCategory.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(37, 153);
            label3.Name = "label3";
            label3.Size = new Size(40, 15);
            label3.TabIndex = 4;
            label3.Text = "Эпоха";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(26, 196);
            label4.Name = "label4";
            label4.Size = new Size(69, 15);
            label4.TabIndex = 5;
            label4.Text = "Цена (руб):";
            // 
            // Автор
            // 
            Автор.AutoSize = true;
            Автор.Location = new Point(23, 229);
            Автор.Name = "Автор";
            Автор.Size = new Size(40, 15);
            Автор.TabIndex = 6;
            Автор.Text = "Автор";
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(137, 145);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(121, 23);
            comboBox1.TabIndex = 7;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(23, 297);
            label6.Name = "label6";
            label6.Size = new Size(102, 15);
            label6.TabIndex = 8;
            label6.Text = "Страна создания:";
            // 
            // txtPrice
            // 
            txtPrice.Location = new Point(128, 192);
            txtPrice.Name = "txtPrice";
            txtPrice.Size = new Size(100, 23);
            txtPrice.TabIndex = 9;
            // 
            // txtAuthor
            // 
            txtAuthor.Location = new Point(113, 229);
            txtAuthor.Name = "txtAuthor";
            txtAuthor.Size = new Size(100, 23);
            txtAuthor.TabIndex = 10;
            // 
            // txtCountry
            // 
            txtCountry.Location = new Point(140, 300);
            txtCountry.Name = "txtCountry";
            txtCountry.Size = new Size(100, 23);
            txtCountry.TabIndex = 11;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(26, 358);
            label5.Name = "label5";
            label5.Size = new Size(62, 15);
            label5.TabIndex = 12;
            label5.Text = "Описание";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(123, 358);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(530, 98);
            textBox1.TabIndex = 13;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(17, 483);
            label7.Name = "label7";
            label7.Size = new Size(113, 15);
            label7.TabIndex = 14;
            label7.Text = "Телефон для связи:";
            // 
            // txtPhone
            // 
            txtPhone.Location = new Point(158, 486);
            txtPhone.Name = "txtPhone";
            txtPhone.Size = new Size(100, 23);
            txtPhone.TabIndex = 15;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(17, 541);
            label8.Name = "label8";
            label8.Size = new Size(75, 15);
            label8.TabIndex = 16;
            label8.Text = "Фото товара";
            // 
            // btnSelectPhoto
            // 
            btnSelectPhoto.Location = new Point(127, 539);
            btnSelectPhoto.Name = "btnSelectPhoto";
            btnSelectPhoto.Size = new Size(131, 23);
            btnSelectPhoto.TabIndex = 17;
            btnSelectPhoto.Text = "Выбрать файл...";
            btnSelectPhoto.UseVisualStyleBackColor = true;
            // 
            // lblPhotoPath
            // 
            lblPhotoPath.AutoSize = true;
            lblPhotoPath.Location = new Point(289, 544);
            lblPhotoPath.Name = "lblPhotoPath";
            lblPhotoPath.Size = new Size(97, 15);
            lblPhotoPath.TabIndex = 18;
            lblPhotoPath.Text = "Файл не выбран";
            // 
            // lblError
            // 
            lblError.AutoSize = true;
            lblError.Location = new Point(439, 589);
            lblError.Name = "lblError";
            lblError.Size = new Size(51, 15);
            lblError.TabIndex = 19;
            lblError.Text = "ошибка";
            // 
            // btnSubmit
            // 
            btnSubmit.Location = new Point(47, 593);
            btnSubmit.Name = "btnSubmit";
            btnSubmit.Size = new Size(75, 23);
            btnSubmit.TabIndex = 20;
            btnSubmit.Text = "Отправить на одобрение";
            btnSubmit.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(179, 601);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 21;
            btnCancel.Text = "Отмена";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // ProposeProductForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(679, 650);
            Controls.Add(btnCancel);
            Controls.Add(btnSubmit);
            Controls.Add(lblError);
            Controls.Add(lblPhotoPath);
            Controls.Add(btnSelectPhoto);
            Controls.Add(label8);
            Controls.Add(txtPhone);
            Controls.Add(label7);
            Controls.Add(textBox1);
            Controls.Add(label5);
            Controls.Add(txtCountry);
            Controls.Add(txtAuthor);
            Controls.Add(txtPrice);
            Controls.Add(label6);
            Controls.Add(comboBox1);
            Controls.Add(Автор);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(cmbCategory);
            Controls.Add(label2);
            Controls.Add(txtName);
            Controls.Add(label1);
            Name = "ProposeProductForm";
            Text = "ProposeProductForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtName;
        private Label label2;
        private ComboBox cmbCategory;
        private Label label3;
        private Label label4;
        private Label Автор;
        private ComboBox comboBox1;
        private Label label6;
        private TextBox txtPrice;
        private TextBox txtAuthor;
        private TextBox txtCountry;
        private Label label5;
        private TextBox textBox1;
        private Label label7;
        private TextBox txtPhone;
        private Label label8;
        private Button btnSelectPhoto;
        private Label lblPhotoPath;
        private Label lblError;
        private Button btnSubmit;
        private Button btnCancel;
    }
}