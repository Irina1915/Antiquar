using AntiqueShopLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antique_shop
{
    public class Tovar
    {
        public int Id { get; set; }
        public double Prise { get; set; }
        public string Opisanie { get; set; }
        public string Name { get; set; }
        public string Categoria { get; set; }
        public string Era { get; set; }
        public string Autor { get; set; }
        public string Country { get; set; }
        public string AuthenMark { get; set; }
        public string Foto { get; set; }

    }
}
