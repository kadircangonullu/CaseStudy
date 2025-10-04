using CaseStudy.Models;
using CaseStudy.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CaseStudy.Services
{
    public class ProductService : IProductService
    {
        private readonly IGoldPriceService _gold;

        public ProductService(IGoldPriceService gold)
        {
            _gold = gold;
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            var path = System.Web.HttpContext.Current.Server.MapPath("~/Data/products.json");
            if (!File.Exists(path)) return new List<Product>();

            var json = File.ReadAllText(path);
            var list = JsonConvert.DeserializeObject<List<Product>>(json) ?? new List<Product>();

            var goldPerGram = await _gold.GetGoldPricePerGramAsync(); // async çağrı
            for (int i = 0; i < list.Count; i++)
            {
                var p = list[i];
                p.Id = i + 1; // Assign sequential IDs
                p.PriceUSD = ComputePrice(p.PopularityScore, p.Weight, goldPerGram);
            }

            return list;
        }

        // Backward compatibility için sync versiyon
        public List<Product> GetProducts()
        {
            return GetProductsAsync().GetAwaiter().GetResult();
        }

        private double ComputePrice(double popularityScore, double weight, double goldPerGram)
        {
            double pop = popularityScore;
            if (pop <= 1) pop *= 100; // 0..1 -> yüzde
            var price = (pop + 1) * weight * goldPerGram;
            return Math.Round(price, 2);
        }
    }
}
