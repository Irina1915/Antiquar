namespace WinFormsApp2
{
    partial class Registration
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Registration));
            button1 = new Button();
            button2 = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            Login = new TextBox();
            textBox2 = new TextBox();
            textBox3 = new TextBox();
            textBox4 = new TextBox();
            textBox5 = new MaskedTextBox();
            SuspendLayout();
            // 
            // button1
            // 
            button1.BackColor = Color.PeachPuff;
            button1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            button1.Location = new Point(211, 309);
            button1.Name = "button1";
            button1.Size = new Size(120, 45);
            button1.TabIndex = 0;
            button1.Text = "Выйти";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.BackColor = Color.PeachPuff;
            button2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            button2.Location = new Point(474, 304);
            button2.Name = "button2";
            button2.Size = new Size(195, 51);
            button2.TabIndex = 1;
            button2.Text = "Зарегистрироваться";
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.PeachPuff;
            label1.Font = new Font("Segoe Print", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label1.Location = new Point(49, 35);
            label1.Name = "label1";
            label1.Size = new Size(99, 35);
            label1.TabIndex = 2;
            label1.Text = "Логин:  ";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.PeachPuff;
            label2.Font = new Font("Segoe Print", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label2.Location = new Point(49, 83);
            label2.Name = "label2";
            label2.Size = new Size(103, 35);
            label2.TabIndex = 3;
            label2.Text = "Пароль: ";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.PeachPuff;
            label3.Font = new Font("Segoe Print", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label3.Location = new Point(49, 131);
            label3.Name = "label3";
            label3.Size = new Size(100, 35);
            label3.TabIndex = 4;
            label3.Text = "ФИО:   ";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.PeachPuff;
            label4.Font = new Font("Segoe Print", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label4.Location = new Point(49, 184);
            label4.Name = "label4";
            label4.Size = new Size(101, 35);
            label4.TabIndex = 5;
            label4.Text = "Телефон";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.PeachPuff;
            label5.Font = new Font("Segoe Print", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label5.Location = new Point(49, 241);
            label5.Name = "label5";
            label5.Size = new Size(101, 35);
            label5.TabIndex = 6;
            label5.Text = "Почта  ";
            // 
            // Login
            // 
            Login.Location = new Point(211, 35);
            Login.Name = "Login";
            Login.Size = new Size(471, 27);
            Login.TabIndex = 7;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(211, 83);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(471, 27);
            textBox2.TabIndex = 8;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(211, 131);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(471, 27);
            textBox3.TabIndex = 9;
            // 
            // textBox4
            // 
            textBox4.Location = new Point(211, 247);
            textBox4.MaxLength = 150;
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(471, 27);
            textBox4.TabIndex = 10;
            // 
            // textBox5
            // 
            textBox5.Location = new Point(211, 190);
            textBox5.Mask = "+7 (000) 000-00-00";
            textBox5.Name = "textBox5";
            textBox5.Size = new Size(471, 27);
            textBox5.TabIndex = 11;
            textBox5.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
            // 
            // Registration
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.LightSalmon;
            ClientSize = new Size(718, 384);
            Controls.Add(textBox5);
            Controls.Add(textBox4);
            Controls.Add(textBox3);
            Controls.Add(textBox2);
            Controls.Add(Login);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(button2);
            Controls.Add(button1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Registration";
            Text = "Регистрация";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Button button2;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private TextBox Login;
        private TextBox textBox2;
        private TextBox textBox3;
        private TextBox textBox4;
        private MaskedTextBox textBox5;
    }
}
