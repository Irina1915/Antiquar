using System;
using System.Collections.Generic;
using System.Linq;
using Antique_shop;

namespace AntiqueShopLibrary
{
    public class ProductFilterCriteria
    {
        public string SearchText { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public double? MinimumPrice { get; set; }
        public double? MaximumPrice { get; set; }
        public string Era { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public bool? Authenticity { get; set; }
        public string Country { get; set; } = string.Empty;
    }

    public static class ProductCatalogFilter
    {
        public static List<Tovar> Apply(IEnumerable<Tovar> products, ProductFilterCriteria criteria)
        {
            if (products == null) throw new ArgumentNullException(nameof(products));
            criteria = criteria ?? new ProductFilterCriteria();

            IEnumerable<Tovar> result = products;
            if (!string.IsNullOrWhiteSpace(criteria.SearchText))
                result = result.Where(p => Contains(p.Name, criteria.SearchText));
            if (!string.IsNullOrWhiteSpace(criteria.Category))
                result = result.Where(p => EqualsText(p.Categoria, criteria.Category));
            if (criteria.MinimumPrice.HasValue)
                result = result.Where(p => p.Prise >= criteria.MinimumPrice.Value);
            if (criteria.MaximumPrice.HasValue)
                result = result.Where(p => p.Prise <= criteria.MaximumPrice.Value);
            if (!string.IsNullOrWhiteSpace(criteria.Era))
                result = result.Where(p => EqualsText(p.Era, criteria.Era));
            if (!string.IsNullOrWhiteSpace(criteria.Author))
                result = result.Where(p => Contains(p.Autor, criteria.Author));
            if (criteria.Authenticity.HasValue)
                result = result.Where(p => string.Equals(p.AuthenMark, "Да", StringComparison.OrdinalIgnoreCase) == criteria.Authenticity.Value);
            if (!string.IsNullOrWhiteSpace(criteria.Country))
                result = result.Where(p => EqualsText(p.Country, criteria.Country));

            return result.ToList();
        }

        private static bool Contains(string value, string search) =>
            (value ?? string.Empty).IndexOf(search ?? string.Empty, StringComparison.CurrentCultureIgnoreCase) >= 0;

        private static bool EqualsText(string value, string filter) =>
            string.Equals(value ?? string.Empty, filter ?? string.Empty, StringComparison.CurrentCultureIgnoreCase);
    }
}
