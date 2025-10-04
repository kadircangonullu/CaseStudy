using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.Json.Serialization;

namespace CaseStudy.Models
{
    public class Product
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("popularityScore")]
        public double PopularityScore { get; set; }
        [JsonPropertyName("weight")]
        public double Weight { get; set; }
        [JsonPropertyName("images")]
        public Dictionary<string, string> Images { get; set; } = new Dictionary<string, string>();
        // runtime-only:
        [JsonIgnore]
        public double PriceUSD { get; set; }
    }
}