namespace WinFormsApp2
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            pictureBox1 = new PictureBox();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            tabPage2 = new TabPage();
            comboBox1 = new ComboBox();
            panel1 = new Panel();
            dataGridView1 = new DataGridView();
            PhotoColumn = new DataGridViewImageColumn();
            NameColumn = new DataGridViewTextBoxColumn();
            CategoryColumn = new DataGridViewTextBoxColumn();
            EpochColumn = new DataGridViewTextBoxColumn();
            PriceColumn = new DataGridViewTextBoxColumn();
            AuthorColumn = new DataGridViewTextBoxColumn();
            CountryColumn = new DataGridViewTextBoxColumn();
            DescriptionColumn = new DataGridViewTextBoxColumn();
            AuthenticityColumn = new DataGridViewCheckBoxColumn();
            KorzinaButton = new Button();
            button1 = new Button();
            tabPage3 = new TabPage();
            panel3 = new Panel();
            DataGridView2 = new DataGridView();
            dataGridViewImageColumn1 = new DataGridViewImageColumn();
            dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn6 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn7 = new DataGridViewTextBoxColumn();
            dataGridViewCheckBoxColumn1 = new DataGridViewCheckBoxColumn();
            btnDelete = new Button();
            button3 = new Button();
            panel2 = new Panel();
            label1 = new Label();
            tabPage4 = new TabPage();
            lblTotalSum = new Label();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            tabPage2.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            tabPage3.SuspendLayout();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)DataGridView2).BeginInit();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Controls.Add(tabPage4);
            tabControl1.Font = new Font("Tempus Sans ITC", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tabControl1.Location = new Point(0, 72);
            tabControl1.Margin = new Padding(3, 2, 3, 2);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1422, 630);
            tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.BackColor = Color.PeachPuff;
            tabPage1.Controls.Add(pictureBox1);
            tabPage1.Controls.Add(label4);
            tabPage1.Controls.Add(label3);
            tabPage1.Controls.Add(label2);
            tabPage1.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 204);
            tabPage1.Location = new Point(4, 29);
            tabPage1.Margin = new Padding(3, 2, 3, 2);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3, 2, 3, 2);
            tabPage1.Size = new Size(1414, 597);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Главная";
            // 
            // pictureBox1
            // 
            pictureBox1.BackgroundImage = (Image)resources.GetObject("pictureBox1.BackgroundImage");
            pictureBox1.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBox1.InitialImage = null;
            pictureBox1.Location = new Point(569, 227);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(316, 229);
            pictureBox1.TabIndex = 3;
            pictureBox1.TabStop = false;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(342, 519);
            label4.Name = "label4";
            label4.Size = new Size(737, 76);
            label4.TabIndex = 2;
            label4.Text = resources.GetString("label4.Text");
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 16.2F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label3.Location = new Point(149, 112);
            label3.Name = "label3";
            label3.Size = new Size(1170, 90);
            label3.TabIndex = 1;
            label3.Text = resources.GetString("label3.Text");
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe Print", 28.2F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label2.Location = new Point(320, 45);
            label2.Name = "label2";
            label2.Size = new Size(783, 67);
            label2.TabIndex = 0;
            label2.Text = "Добро пожаловать на сайт \"Корона\"";
            // 
            // tabPage2
            // 
            tabPage2.BackColor = Color.PeachPuff;
            tabPage2.Controls.Add(comboBox1);
            tabPage2.Controls.Add(panel1);
            tabPage2.Controls.Add(KorzinaButton);
            tabPage2.Controls.Add(button1);
            tabPage2.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 204);
            tabPage2.Location = new Point(4, 29);
            tabPage2.Margin = new Padding(3, 2, 3, 2);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3, 2, 3, 2);
            tabPage2.Size = new Size(1414, 597);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Товары";
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(1253, 23);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(121, 27);
            comboBox1.TabIndex = 5;
            // 
            // panel1
            // 
            panel1.BackColor = Color.Sienna;
            panel1.Controls.Add(dataGridView1);
            panel1.Location = new Point(6, 5);
            panel1.Name = "panel1";
            panel1.Size = new Size(1207, 506);
            panel1.TabIndex = 4;
            // 
            // dataGridView1
            // 
            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { PhotoColumn, NameColumn, CategoryColumn, EpochColumn, PriceColumn, AuthorColumn, CountryColumn, DescriptionColumn, AuthenticityColumn });
            dataGridView1.Location = new Point(8, 5);
            dataGridView1.Margin = new Padding(3, 2, 3, 2);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(1192, 493);
            dataGridView1.TabIndex = 3;
            // 
            // PhotoColumn
            // 
            PhotoColumn.HeaderText = "Фото";
            PhotoColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
            PhotoColumn.MinimumWidth = 6;
            PhotoColumn.Name = "PhotoColumn";
            PhotoColumn.Width = 125;
            // 
            // NameColumn
            // 
            NameColumn.HeaderText = "Наименование";
            NameColumn.MinimumWidth = 6;
            NameColumn.Name = "NameColumn";
            NameColumn.Width = 125;
            // 
            // CategoryColumn
            // 
            CategoryColumn.HeaderText = "Категория";
            CategoryColumn.MinimumWidth = 6;
            CategoryColumn.Name = "CategoryColumn";
            CategoryColumn.Width = 125;
            // 
            // EpochColumn
            // 
            EpochColumn.HeaderText = "Эпоха";
            EpochColumn.MinimumWidth = 6;
            EpochColumn.Name = "EpochColumn";
            EpochColumn.Width = 125;
            // 
            // PriceColumn
            // 
            PriceColumn.HeaderText = "Цена";
            PriceColumn.MinimumWidth = 6;
            PriceColumn.Name = "PriceColumn";
            PriceColumn.Width = 125;
            // 
            // AuthorColumn
            // 
            AuthorColumn.HeaderText = "Автор";
            AuthorColumn.MinimumWidth = 6;
            AuthorColumn.Name = "AuthorColumn";
            AuthorColumn.Width = 125;
            // 
            // CountryColumn
            // 
            CountryColumn.HeaderText = "Страна";
            CountryColumn.MinimumWidth = 6;
            CountryColumn.Name = "CountryColumn";
            CountryColumn.Width = 125;
            // 
            // DescriptionColumn
            // 
            DescriptionColumn.HeaderText = "Описание";
            DescriptionColumn.MinimumWidth = 6;
            DescriptionColumn.Name = "DescriptionColumn";
            DescriptionColumn.Width = 125;
            // 
            // AuthenticityColumn
            // 
            AuthenticityColumn.HeaderText = "Подлинность";
            AuthenticityColumn.MinimumWidth = 6;
            AuthenticityColumn.Name = "AuthenticityColumn";
            AuthenticityColumn.Width = 125;
            // 
            // KorzinaButton
            // 
            KorzinaButton.BackColor = Color.Sienna;
            KorzinaButton.ForeColor = Color.PeachPuff;
            KorzinaButton.Location = new Point(1258, 160);
            KorzinaButton.Margin = new Padding(3, 2, 3, 2);
            KorzinaButton.Name = "KorzinaButton";
            KorzinaButton.Size = new Size(124, 62);
            KorzinaButton.TabIndex = 2;
            KorzinaButton.Text = "Добавить в корзину";
            KorzinaButton.UseVisualStyleBackColor = false;
            KorzinaButton.Click += KorzinaButton_Click;
            // 
            // button1
            // 
            button1.BackColor = Color.Sienna;
            button1.ForeColor = Color.PeachPuff;
            button1.Location = new Point(1253, 71);
            button1.Margin = new Padding(3, 2, 3, 2);
            button1.Name = "button1";
            button1.Size = new Size(129, 65);
            button1.TabIndex = 1;
            button1.Text = "Предложить товар";
            button1.UseVisualStyleBackColor = false;
            // 
            // tabPage3
            // 
            tabPage3.BackColor = Color.PeachPuff;
            tabPage3.Controls.Add(lblTotalSum);
            tabPage3.Controls.Add(panel3);
            tabPage3.Controls.Add(btnDelete);
            tabPage3.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 204);
            tabPage3.Location = new Point(4, 29);
            tabPage3.Margin = new Padding(3, 2, 3, 2);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3, 2, 3, 2);
            tabPage3.Size = new Size(1414, 597);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Корзина";
            // 
            // panel3
            // 
            panel3.BackColor = Color.Sienna;
            panel3.Controls.Add(DataGridView2);
            panel3.Location = new Point(8, 5);
            panel3.Name = "panel3";
            panel3.Size = new Size(693, 517);
            panel3.TabIndex = 6;
            // 
            // DataGridView2
            // 
            DataGridView2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            DataGridView2.BackgroundColor = Color.White;
            DataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DataGridView2.Columns.AddRange(new DataGridViewColumn[] { dataGridViewImageColumn1, dataGridViewTextBoxColumn1, dataGridViewTextBoxColumn2, dataGridViewTextBoxColumn3, dataGridViewTextBoxColumn4, dataGridViewTextBoxColumn5, dataGridViewTextBoxColumn6, dataGridViewTextBoxColumn7, dataGridViewCheckBoxColumn1 });
            DataGridView2.Location = new Point(7, 9);
            DataGridView2.Margin = new Padding(3, 2, 3, 2);
            DataGridView2.Name = "DataGridView2";
            DataGridView2.RowHeadersWidth = 51;
            DataGridView2.Size = new Size(667, 496);
            DataGridView2.TabIndex = 4;
            // 
            // dataGridViewImageColumn1
            // 
            dataGridViewImageColumn1.HeaderText = "Фото";
            dataGridViewImageColumn1.ImageLayout = DataGridViewImageCellLayout.Zoom;
            dataGridViewImageColumn1.MinimumWidth = 6;
            dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            dataGridViewImageColumn1.Width = 125;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewTextBoxColumn1.HeaderText = "Наименование";
            dataGridViewTextBoxColumn1.MinimumWidth = 6;
            dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            dataGridViewTextBoxColumn1.Width = 125;
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewTextBoxColumn2.HeaderText = "Категория";
            dataGridViewTextBoxColumn2.MinimumWidth = 6;
            dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            dataGridViewTextBoxColumn2.Width = 125;
            // 
            // dataGridViewTextBoxColumn3
            // 
            dataGridViewTextBoxColumn3.HeaderText = "Эпоха";
            dataGridViewTextBoxColumn3.MinimumWidth = 6;
            dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            dataGridViewTextBoxColumn3.Width = 125;
            // 
            // dataGridViewTextBoxColumn4
            // 
            dataGridViewTextBoxColumn4.HeaderText = "Цена";
            dataGridViewTextBoxColumn4.MinimumWidth = 6;
            dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            dataGridViewTextBoxColumn4.Width = 125;
            // 
            // dataGridViewTextBoxColumn5
            // 
            dataGridViewTextBoxColumn5.HeaderText = "Автор";
            dataGridViewTextBoxColumn5.MinimumWidth = 6;
            dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            dataGridViewTextBoxColumn5.Width = 125;
            // 
            // dataGridViewTextBoxColumn6
            // 
            dataGridViewTextBoxColumn6.HeaderText = "Страна";
            dataGridViewTextBoxColumn6.MinimumWidth = 6;
            dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            dataGridViewTextBoxColumn6.Width = 125;
            // 
            // dataGridViewTextBoxColumn7
            // 
            dataGridViewTextBoxColumn7.HeaderText = "Описание";
            dataGridViewTextBoxColumn7.MinimumWidth = 6;
            dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            dataGridViewTextBoxColumn7.Width = 125;
            // 
            // dataGridViewCheckBoxColumn1
            // 
            dataGridViewCheckBoxColumn1.HeaderText = "Подлинность";
            dataGridViewCheckBoxColumn1.MinimumWidth = 6;
            dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            dataGridViewCheckBoxColumn1.Width = 125;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = Color.Sienna;
            btnDelete.ForeColor = Color.PeachPuff;
            btnDelete.Location = new Point(720, 14);
            btnDelete.Margin = new Padding(3, 2, 3, 2);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(102, 64);
            btnDelete.TabIndex = 5;
            btnDelete.Text = "Убрать товар";
            btnDelete.UseVisualStyleBackColor = false;
            btnDelete.Click += btnDelete_Click;
            // 
            // button3
            // 
            button3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button3.BackColor = Color.PeachPuff;
            button3.ForeColor = Color.DarkRed;
            button3.Location = new Point(1312, 9);
            button3.Margin = new Padding(3, 2, 3, 2);
            button3.Name = "button3";
            button3.Size = new Size(98, 31);
            button3.TabIndex = 1;
            button3.Text = "Выйти";
            button3.UseVisualStyleBackColor = false;
            button3.Click += button3_Click;
            // 
            // panel2
            // 
            panel2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel2.BackColor = Color.LightSalmon;
            panel2.Controls.Add(label1);
            panel2.Controls.Add(button3);
            panel2.Location = new Point(0, 0);
            panel2.Margin = new Padding(3, 2, 3, 2);
            panel2.Name = "panel2";
            panel2.Size = new Size(1422, 68);
            panel2.TabIndex = 9;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ForeColor = Color.Black;
            label1.Location = new Point(19, 9);
            label1.Name = "label1";
            label1.Size = new Size(38, 15);
            label1.TabIndex = 2;
            label1.Text = "label1";
            // 
            // tabPage4
            // 
            tabPage4.Location = new Point(4, 29);
            tabPage4.Name = "tabPage4";
            tabPage4.Padding = new Padding(3);
            tabPage4.Size = new Size(1414, 597);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "tabPage4";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // lblTotalSum
            // 
            lblTotalSum.AutoSize = true;
            lblTotalSum.Location = new Point(15, 545);
            lblTotalSum.Name = "lblTotalSum";
            lblTotalSum.Size = new Size(91, 19);
            lblTotalSum.TabIndex = 7;
            lblTotalSum.Text = "Итого: 0 руб.";
            // 
            // MainForm
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.Sienna;
            ClientSize = new Size(1422, 702);
            Controls.Add(panel2);
            Controls.Add(tabControl1);
            ForeColor = Color.Sienna;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 2, 3, 2);
            Name = "MainForm";
            Text = "Корона";
            WindowState = FormWindowState.Maximized;
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            tabPage2.ResumeLayout(false);
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            tabPage3.ResumeLayout(false);
            tabPage3.PerformLayout();
            panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)DataGridView2).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private Button KorzinaButton;
        private Button button1;
        private TabPage tabPage3;
        private Button button3;
        private Panel panel2;
        private Label label1;
        private Label label4;
        private Label label3;
        private Label label2;
        private DataGridView dataGridView1;
        private DataGridViewImageColumn PhotoColumn;
        private DataGridViewTextBoxColumn NameColumn;
        private DataGridViewTextBoxColumn CategoryColumn;
        private DataGridViewTextBoxColumn EpochColumn;
        private DataGridViewTextBoxColumn PriceColumn;
        private DataGridViewTextBoxColumn AuthorColumn;
        private DataGridViewTextBoxColumn CountryColumn;
        private DataGridViewTextBoxColumn DescriptionColumn;
        private DataGridViewCheckBoxColumn AuthenticityColumn;
        private Button btnDelete;
        private DataGridView DataGridView2;
        private DataGridViewImageColumn dataGridViewImageColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private Panel panel1;
        private Panel panel3;
        private ComboBox comboBox1;
        private PictureBox pictureBox1;
        private TabPage tabPage4;
        private Label lblTotalSum;
    }
}
