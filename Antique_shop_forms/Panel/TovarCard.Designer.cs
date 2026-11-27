namespace Panel
{
    partial class TovarCard
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.LebelNameTovar = new System.Windows.Forms.Label();
            this.labelCategori = new System.Windows.Forms.Label();
            this.LabelCategoriTovar = new System.Windows.Forms.Label();
            this.labelOpis = new System.Windows.Forms.Label();
            this.LebelOpisTovar = new System.Windows.Forms.Label();
            this.LebelChenaTovar = new System.Windows.Forms.Label();
            this.labelChena = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(18, 15);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(281, 188);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // LebelNameTovar
            // 
            this.LebelNameTovar.AutoSize = true;
            this.LebelNameTovar.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LebelNameTovar.Location = new System.Drawing.Point(495, 15);
            this.LebelNameTovar.Name = "LebelNameTovar";
            this.LebelNameTovar.Size = new System.Drawing.Size(225, 32);
            this.LebelNameTovar.TabIndex = 1;
            this.LebelNameTovar.Text = "Наименование";
            // 
            // labelCategori
            // 
            this.labelCategori.AutoSize = true;
            this.labelCategori.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelCategori.Location = new System.Drawing.Point(326, 91);
            this.labelCategori.Name = "labelCategori";
            this.labelCategori.Size = new System.Drawing.Size(150, 29);
            this.labelCategori.TabIndex = 2;
            this.labelCategori.Text = "Категория:";
            // 
            // LabelCategoriTovar
            // 
            this.LabelCategoriTovar.AutoSize = true;
            this.LabelCategoriTovar.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LabelCategoriTovar.Location = new System.Drawing.Point(482, 94);
            this.LabelCategoriTovar.Name = "LabelCategoriTovar";
            this.LabelCategoriTovar.Size = new System.Drawing.Size(107, 25);
            this.LabelCategoriTovar.TabIndex = 3;
            this.LabelCategoriTovar.Text = "категория";
            // 
            // labelOpis
            // 
            this.labelOpis.AutoSize = true;
            this.labelOpis.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelOpis.Location = new System.Drawing.Point(329, 120);
            this.labelOpis.Name = "labelOpis";
            this.labelOpis.Size = new System.Drawing.Size(147, 29);
            this.labelOpis.TabIndex = 4;
            this.labelOpis.Text = "Описание: ";
            // 
            // LebelOpisTovar
            // 
            this.LebelOpisTovar.AutoSize = true;
            this.LebelOpisTovar.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LebelOpisTovar.Location = new System.Drawing.Point(482, 124);
            this.LebelOpisTovar.Name = "LebelOpisTovar";
            this.LebelOpisTovar.Size = new System.Drawing.Size(102, 25);
            this.LebelOpisTovar.TabIndex = 5;
            this.LebelOpisTovar.Text = "Описание";
            // 
            // LebelChenaTovar
            // 
            this.LebelChenaTovar.AutoSize = true;
            this.LebelChenaTovar.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LebelChenaTovar.Location = new System.Drawing.Point(414, 66);
            this.LebelChenaTovar.Name = "LebelChenaTovar";
            this.LebelChenaTovar.Size = new System.Drawing.Size(56, 25);
            this.LebelChenaTovar.TabIndex = 6;
            this.LebelChenaTovar.Text = "цена";
            // 
            // labelChena
            // 
            this.labelChena.AutoSize = true;
            this.labelChena.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelChena.Location = new System.Drawing.Point(326, 62);
            this.labelChena.Name = "labelChena";
            this.labelChena.Size = new System.Drawing.Size(82, 29);
            this.labelChena.TabIndex = 7;
            this.labelChena.Text = "Цена:";
            // 
            // TovarCard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelChena);
            this.Controls.Add(this.LebelChenaTovar);
            this.Controls.Add(this.LebelOpisTovar);
            this.Controls.Add(this.labelOpis);
            this.Controls.Add(this.LabelCategoriTovar);
            this.Controls.Add(this.labelCategori);
            this.Controls.Add(this.LebelNameTovar);
            this.Controls.Add(this.pictureBox);
            this.Name = "TovarCard";
            this.Size = new System.Drawing.Size(967, 222);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TovarCard_MouseMove);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Label LebelNameTovar;
        private System.Windows.Forms.Label labelCategori;
        private System.Windows.Forms.Label LabelCategoriTovar;
        private System.Windows.Forms.Label labelOpis;
        private System.Windows.Forms.Label LebelOpisTovar;
        private System.Windows.Forms.Label LebelChenaTovar;
        private System.Windows.Forms.Label labelChena;
    }
}
