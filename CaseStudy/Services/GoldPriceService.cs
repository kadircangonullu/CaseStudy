using System;
using System.Configuration;
using System.Net.Http;
using System.Runtime.Caching;
using System.Text.Json;
using System.Threading.Tasks;

namespace CaseStudy.Services
{
    public class GoldPriceService : IGoldPriceService
    {
        private static readonly HttpClient _http = new HttpClient();
        private readonly ObjectCache _cache = MemoryCache.Default;
        private const string CacheKey = "GoldPricePerGram";
        private const double GramsPerTroyOunce = 31.1034768;

        public GoldPriceService() { }

        public async Task<double> GetGoldPricePerGramAsync()
        {
            if (_cache.Contains(CacheKey)) return (double)_cache.Get(CacheKey);

            var url = ConfigurationManager.AppSettings["GoldApi:Url"];
            var token = ConfigurationManager.AppSettings["GoldApi:Token"];
            var cacheMinutes = int.TryParse(ConfigurationManager.AppSettings["GoldApi:CacheMinutes"], out var cm) ? cm : 10;
            var fallback = double.TryParse(ConfigurationManager.AppSettings["GoldApi:FallbackPerGram"], out var f) ? f : 60.0;

            if (string.IsNullOrEmpty(url))
            {
                _cache.Set(CacheKey, fallback, DateTimeOffset.Now.AddMinutes(cacheMinutes));
                return fallback;
            }

            try
            {

                _http.DefaultRequestHeaders.Clear();
                if (!string.IsNullOrEmpty(token))
                    _http.DefaultRequestHeaders.Add("x-access-token", token);

                var res = await _http.GetAsync(url);
                res.EnsureSuccessStatusCode();
                var json = await res.Content.ReadAsStringAsync();

                double perGram = 0;

                using (var doc = JsonDocument.Parse(json))
                {
                    // 1) "price" doğrudan gram cinsinden gelmiş olabilir
                    if (doc.RootElement.TryGetProperty("price", out var priceElem) && priceElem.ValueKind == JsonValueKind.Number)
                    {
                        perGram = priceElem.GetDouble();
                    }
                    // 2) "pricePerOunce" varsa -> gram'a çevir
                    else if (doc.RootElement.TryGetProperty("pricePerOunce", out var pOunce) && pOunce.ValueKind == JsonValueKind.Number)
                    {
                        perGram = pOunce.GetDouble() / GramsPerTroyOunce;
                    }
                    // 3) common: rates.XAU gibi; (provider'a göre adapte et)
                    else if (doc.RootElement.TryGetProperty("rates", out var rates) &&
                             rates.TryGetProperty("XAU", out var xau) &&
                             xau.ValueKind == JsonValueKind.Number)
                    {
                        perGram = xau.GetDouble();
                    }
                }

                if (perGram <= 0) perGram = fallback;
                _cache.Set(CacheKey, perGram, DateTimeOffset.Now.AddMinutes(cacheMinutes));
                return perGram;
            }
            catch
            {
                _cache.Set(CacheKey, fallback, DateTimeOffset.Now.AddMinutes(cacheMinutes));
                return fallback;
            }

        }
    }
}
