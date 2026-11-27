using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AntiqueShopLibrary;

namespace Panel
{
   
        public partial class TovarCard : UserControl, ITovarView
        {
            private Tovar tovar_;
            // цвета
            private Color normalColor_ = ColorTranslator.FromHtml("#D2F6E7");
            private Color enteringColor_ = ColorTranslator.FromHtml("#70B2AF");
            private Color deficitColor_ = ColorTranslator.FromHtml("#FF8080");

        public TovarCard()
        {
            InitializeComponent();
            foreach (Control c in Controls)
            {
                c.MouseMove += TovarCard_MouseMove;
            }
        }

        public void Show(Tovar tovar)
            {
                tovar_ = tovar;
                LabelCategoriTovar.Text = tovar.Category;
                LebelOpisTovar.Text = tovar.Opisanie;
                LebelChenaTovar.Text = tovar.Price.ToString() + " руб.";

                ShowImage(tovar.ImagePath);
            }

            public void ShowImage(string imagePath)
            {
                pictureBox.ImageLocation = imagePath;
            }


            private void ProductCard_MouseMove(object sender, MouseEventArgs e)
            {
                BackColor = enteringColor_;
            }


        private void TovarCard_MouseMove(object sender, MouseEventArgs e)
        {
            BackColor = enteringColor_;
        }

    }
       
    } 

